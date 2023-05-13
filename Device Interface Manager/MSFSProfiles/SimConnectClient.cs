using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.FlightSimulator.SimConnect;
using Device_Interface_Manager.MVVM.ViewModel;
using static Device_Interface_Manager.MSFSProfiles.PMDG.PMDG_NG3_SDK;

namespace Device_Interface_Manager.MSFSProfiles;

public class SimConnectClient
{
    public SimConnect simConnect;
    private readonly IntPtr handle = new(0);
    private const int wM_USER_SIMCONNECT = 0x0402;
    private const int MESSAGE_SIZE = 1024;
    private const string CLIENT_DATA_NAME_COMMAND = "DIM.Command";

    public async Task SimConnect_OpenAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                simConnect = new SimConnect("Device-Interface-Manager", handle, wM_USER_SIMCONNECT, null, 0);

                simConnect.OnRecvOpen += SimConnect_OnRecvOpen;
                simConnect.OnRecvQuit += SimConnect_OnRecvQuit;

                simConnect.OnRecvException += SimConnect_OnRecvException;

                simConnect.RequestSystemState(DATA_REQUEST_ID.AIR_PATH_REQUEST, "AircraftLoaded");

                simConnect.MapClientDataNameToID(CLIENT_DATA_NAME_COMMAND, CLIENT_DATA_ID.CLIENT_DATA_ID_COMMAND);
                simConnect.CreateClientData(CLIENT_DATA_ID.CLIENT_DATA_ID_COMMAND, MESSAGE_SIZE, SIMCONNECT_CREATE_CLIENT_DATA_FLAG.DEFAULT);
                simConnect.AddToClientDataDefinition(DEFINE_ID.DATA_DEFINITION_ID_COMMAND, 0, MESSAGE_SIZE, 0, 0);

                simConnect.OnRecvSimobjectData += Simconnect_OnRecvSimobjectData;
                break;
            }
            catch (COMException)
            {
                await Task.Delay(1000, cancellationToken);
            }
        }
    }

    public void ReceiveSimConnectMessage()
    {
        try
        {
            simConnect?.ReceiveMessage();
        }
        catch (Exception)
        {
            StrongReferenceMessenger.Default.Send(new SimConnectStausMessage(false));
        }
    }

    public void SimConnect_Close()
    {
        simConnect?.Dispose();
        simConnect = null;
        StrongReferenceMessenger.Default.Send(new SimConnectStausMessage(false));
    }

    private void SimConnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
    {
        StrongReferenceMessenger.Default.Send(new SimConnectStausMessage(true));
    }

    private void SimConnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
    {
        StrongReferenceMessenger.Default.Send(new SimConnectStausMessage(false));
    }

    private void SimConnect_OnRecvException(SimConnect sender, SIMCONNECT_RECV_EXCEPTION data)
    {

    }

    public delegate void SimVarChanged(SimVar simVar);
    public event SimVarChanged OnSimVarChanged;

    private void Simconnect_OnRecvSimobjectData(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA data)
    {
        if (data.dwRequestID >= 5)
        {
            if (SimVars.Count < (int)data.dwRequestID)
            {
                return;
            }
            SimVars[(int)data.dwRequestID - 1].Data = (double)data.dwData[0];
            OnSimVarChanged?.Invoke(SimVars[(int)data.dwRequestID - 1]);
        }
    }

    private uint maxClientDataDefinition;
    private readonly List<SimVar> SimVars = new();

    public void TransmitEvent(uint data, Enum eventID)
    {
        simConnect.TransmitClientEvent(
            0,
            eventID,
            data,
            SIMCONNECT_GROUP_PRIORITY.HIGHEST,
            SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
    }

    public void SendEvent(string eventID)
    {
        simConnect.SetClientData(
            CLIENT_DATA_ID.CLIENT_DATA_ID_COMMAND,
            DEFINE_ID.DATA_DEFINITION_ID_COMMAND,
            SIMCONNECT_CLIENT_DATA_SET_FLAG.DEFAULT,
            0,
            new CommandStruct() { command = "(>" + eventID + ')' });
    }

    public void SetSimVar(int simVarValue, string simVarName)
    {
        SetSimVars(simVarName, null, simVarValue);
    }

    public void SetSimVar(uint simVarValue, string simVarName)
    {
        SetSimVars(simVarName, null, simVarValue);
    }

    public void SetSimVar(int simVarValue, string simVarName, string simVarUnit)
    {
        SetSimVars(simVarName, simVarUnit, simVarValue);
    }

    private void SetSimVars(string simVarName, string simVarUnit, double simVarValue)
    {
        if (!SimVars.Exists(lvar => lvar.Name == simVarName))
        {
            RegisterSimVars(simVarName, simVarUnit, false);
        }

        simConnect?.SetDataOnSimObject(
            (DEFINE_ID)SimVars.Find(lvar => lvar.Name == simVarName).ID,
            SimConnect.SIMCONNECT_OBJECT_ID_USER,
            SIMCONNECT_DATA_SET_FLAG.DEFAULT,
            simVarValue);
    }

    public double GetSimVar(string simVarName)
    {
        return SimVars.FirstOrDefault(lvar => lvar.Name == simVarName).Data;
    }

    public void RegisterSimVar(string simVarName)
    {
        RegisterSimVars(simVarName, null, true);
    }

    public void RegisterSimVar(string simVarName, string simVarUnit)
    {
        RegisterSimVars(simVarName, simVarUnit, true);
    }

    private void RegisterSimVars(string simVarName, string simVarUnit, bool request)
    {
        if (!SimVars.Exists(lvar => lvar.Name == simVarName))
        {
            SimVar newSimVar = new() { Name = simVarName, ID = (uint)SimVars.Count + 1 };
            SimVars.Add(newSimVar);

            if (maxClientDataDefinition >= newSimVar.ID)
            {
                return;
            }

            maxClientDataDefinition = newSimVar.ID;

            simConnect?.AddToDataDefinition(
                (DEFINE_ID)newSimVar.ID,
                simVarName,
                simVarUnit,
                SIMCONNECT_DATATYPE.FLOAT64,
                0,
                0);

            simConnect?.RegisterDataDefineStruct<double>((DEFINE_ID)newSimVar.ID);

            if (request)
            {
                simConnect?.RequestDataOnSimObject(
                    (REQUEST_ID)newSimVar.ID,
                    (DEFINE_ID)newSimVar.ID,
                    SimConnect.SIMCONNECT_OBJECT_ID_USER,
                    SIMCONNECT_PERIOD.SIM_FRAME,
                    SIMCONNECT_DATA_REQUEST_FLAG.CHANGED,
                    0,
                    0,
                    0);
            }
        }
    }

    public class SimVar
    {
        public uint ID { get; set; }
        public string Name { get; set; }
        public double Data { get; set; }
        public bool BData()
        {
            return Convert.ToBoolean(Data);
        }
    }

    private enum CLIENT_DATA_ID
    {
        CLIENT_DATA_ID_COMMAND
    }

    private enum REQUEST_ID
    {
        DATA_REQUEST_ID_COMMAND
    }

    private enum DEFINE_ID
    {
        DATA_DEFINITION_ID_COMMAND
    }

    private enum EVENT_ID
    {
        DUMMY
    }

    private struct CommandStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MESSAGE_SIZE)]
        public string command;
    }
}