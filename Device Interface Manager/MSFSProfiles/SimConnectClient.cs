using System;
using System.Linq;
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

    public void SimConnect_Open()
    {
        while (true)
        {
            try
            {
                simConnect = new SimConnect("Device-Interface-Manager", handle, wM_USER_SIMCONNECT, null, 0);

                simConnect.OnRecvOpen += SimConnect_OnRecvOpen;
                simConnect.OnRecvQuit += SimConnect_OnRecvQuit;

                simConnect.OnRecvException += SimConnect_OnRecvException;

                simConnect.RequestSystemState(DATA_REQUEST_ID.AIR_PATH_REQUEST, "AircraftLoaded");

                LoadEventPresets();
                simConnect.OnRecvSimobjectData += Simconnect_OnRecvSimobjectData;
                break;
            }
            catch (COMException)
            {

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
            StrongReferenceMessenger.Default.Send(new SimConnectStausMessage(isConnected = false));
        }
    }

    public void SimConnect_Close()
    {
        simConnect?.Dispose();
        simConnect = null;
        StrongReferenceMessenger.Default.Send(new SimConnectStausMessage(isConnected = false));
    }

    private void SimConnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
    {
        StrongReferenceMessenger.Default.Send(new SimConnectStausMessage(isConnected = true));
    }

    private void SimConnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
    {
        StrongReferenceMessenger.Default.Send(new SimConnectStausMessage(isConnected = false));
    }

    private void SimConnect_OnRecvException(SimConnect sender, SIMCONNECT_RECV_EXCEPTION data)
    {
        if (data.dwException != 9)
        {

        }
    }

    //NEW
    public delegate void SimVarChanged (SimVar simVar);
    public event SimVarChanged OnSimVarChanged;

    private void Simconnect_OnRecvSimobjectData(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA data)
    {
        if (data.dwRequestID != 0)
        {
            if (SimVars.Count < (int)data.dwRequestID)
            {
                return;
            }
            SimVars[(int)data.dwRequestID - 1].Data = (double)data.dwData[0];
            OnSimVarChanged?.Invoke(SimVars[(int)data.dwRequestID - 1]);
        }
    }

    private bool isConnected;
    private uint maxClientDataDefinition;
    private readonly List<SimVar> SimVars = new();
    private Dictionary<string, List<Tuple<string, uint>>> Events { get; set; }

    private void LoadEventPresets()
    {
        Events ??= new Dictionary<string, List<Tuple<string, uint>>>();
        Events.Clear();

        string[] lines = System.IO.File.ReadAllLines(@"Presets\msfs2020_eventids.cip");
        var groupKey = "Dummy";
        uint eventIdx = 0;

        Events[groupKey] = new List<Tuple<string, uint>>();
        foreach (string line in lines)
        {
            if (line.StartsWith("//"))
            {
                continue;
            }

            var cols = line.Split(':');
            if (cols.Length > 1)
            {
                groupKey = cols[0];
                if (Events.ContainsKey(groupKey))
                {
                    continue;
                }

                Events[groupKey] = new List<Tuple<string, uint>>();
                continue;
            }

            Events[groupKey].Add(new Tuple<string, uint>(cols[0], eventIdx++));
        }
    }

    public void SetEventID(string eventID)
    {
        if (simConnect is null || !isConnected)
        {
            return;
        }

        Tuple<string, uint> eventItem = null;

        foreach (string groupKey in Events.Keys)
        {
            eventItem = Events[groupKey].Find(x => x.Item1 == eventID);
            if (eventItem is not null)
            {
                break;
            }
        }

        if (eventItem is null)
        {
            return;
        }

        simConnect?.TransmitClientEvent(
                0,
                (EVENT_ID)eventItem.Item2,
                1,
                SIMCONNECT_GROUP_PRIORITY.DEFAULT,
                SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
    }

    public void SetSimVar(int simVarValue, string simVarName)
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

    private enum REQUEST_ID
    {
        DUMMY
    }

    private enum DEFINE_ID
    {
        DUMMY
    }

    private enum EVENT_ID
    {
        DUMMY
    }
}