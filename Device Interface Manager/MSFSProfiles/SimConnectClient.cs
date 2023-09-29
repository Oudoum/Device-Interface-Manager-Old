using System;
using System.Linq;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.FlightSimulator.SimConnect;
using Device_Interface_Manager.MVVM.ViewModel;
using MahApps.Metro.Controls;
using Device_Interface_Manager.MSFSProfiles.PMDG.B737;

namespace Device_Interface_Manager.MSFSProfiles;

public sealed class SimConnectClient
{
    private static readonly SimConnectClient _instance = new();

    public static SimConnectClient Instance { get =>  _instance; }

    public SimConnect SimConnect { get; private set; } = null;

    private IntPtr handle;
    private HwndSource handleSource;
    private const int WM_USER_SIMCONNECT = 0x0402;
    private const int MESSAGE_SIZE = 1024;
    private const string CLIENT_DATA_NAME_COMMAND = "DIM.Command";

    public bool IsConnected { get; private set; }

    private IntPtr HandleSimConnectEvents(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam, ref bool isHandled)
    {
        isHandled = false;

        switch (message)
        {
            case WM_USER_SIMCONNECT:
                {
                    try
                    {
                        SimConnect?.ReceiveMessage();
                        isHandled = true;
                    }
                    catch (COMException)
                    {
                        SimConnect_Close();
                    }
                }
                break;
        }

        return IntPtr.Zero;
    }

    public async Task SimConnect_OpenAsync(CancellationToken cancellationToken)
    {
        if (SimConnect is not null)
        {
            return;
        }

        Application.Current.Invoke(() => handle = new WindowInteropHelper(Application.Current.MainWindow).Handle);
        handleSource = HwndSource.FromHwnd(handle);
        handleSource.AddHook(HandleSimConnectEvents);

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                SimConnect = new SimConnect("Device-Interface-Manager", handle, WM_USER_SIMCONNECT, null, 0);
                break;
            }
            catch (Exception)
            {
                try
                {
                    await Task.Delay(1000, cancellationToken);
                }
                catch (TaskCanceledException)
                {

                }
            }
        }

        SimConnect.OnRecvException += SimConnect_OnRecvException;

        SimConnect.OnRecvOpen += SimConnect_OnRecvOpen;
        SimConnect.OnRecvQuit += SimConnect_OnRecvQuit;


        SimConnect.MapClientDataNameToID(CLIENT_DATA_NAME_COMMAND, CLIENT_DATA_ID.CLIENT_DATA_ID_COMMAND);
        SimConnect.AddToClientDataDefinition(DEFINE_ID.DATA_DEFINITION_ID_COMMAND, 0, MESSAGE_SIZE, 0, 0);

        PMDG737.RegisterPMDGDataEvents(SimConnect);

        RegisterSimVar("CAMERA STATE", "Enum");

        SimConnect.OnRecvSimobjectData += Simconnect_OnRecvSimobjectData;
    }

    public void SimConnect_Close()
    {
        if (SimConnect is not null)
        {
            SimConnect.OnRecvSimobjectData -= Simconnect_OnRecvSimobjectData;
            SimConnect.OnRecvQuit -= SimConnect_OnRecvQuit;
            SimConnect.OnRecvOpen -= SimConnect_OnRecvOpen;
            SimConnect.OnRecvException -= SimConnect_OnRecvException;
            handleSource.RemoveHook(HandleSimConnectEvents);
            SimConnect.Dispose();
            SimConnect = null;
            SimVars.Clear();
            SendSimConnectConnectionStatus(false);
        }
    }

    private void SimConnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
    {
        SendSimConnectConnectionStatus(true);
    }

    private void SimConnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
    {
        SendSimConnectConnectionStatus(false);
    }

    private void SendSimConnectConnectionStatus(bool isConnected)
    {
        IsConnected = isConnected;
        StrongReferenceMessenger.Default.Send(new SimConnectStausMessage(isConnected));
    }

    private void SimConnect_OnRecvException(SimConnect sender, SIMCONNECT_RECV_EXCEPTION data)
    {

    }

    public event EventHandler<SimVar> OnSimVarChanged;

    private const int offset = 4;

    private void Simconnect_OnRecvSimobjectData(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA data)
    {
        if (data.dwRequestID != 0)
        {
            if (SimVars.Count < (int)data.dwRequestID - offset)
            {
                return;
            }
            SimVars[(int)data.dwRequestID - offset - 1].Data = (double)data.dwData[0];
            OnSimVarChanged?.Invoke(this, SimVars[(int)data.dwRequestID - offset - 1]);
        }
    }

    private readonly List<SimVar> SimVars = new();

    private readonly object lockObject = new();

    public void TransmitEvent(uint data, Enum eventID)
    {
        lock (lockObject)
        {
            SimConnect?.TransmitClientEvent(
                0,
                eventID,
                data,
                PMDG.PMDG_NG3_SDK.SIMCONNECT_GROUP_PRIORITY.STANDARD,
                SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
        }
    }

    public void SendWASMEvent(string eventID)
    {
        lock (lockObject)
        {
            SimConnect?.SetClientData(
            CLIENT_DATA_ID.CLIENT_DATA_ID_COMMAND,
            DEFINE_ID.DATA_DEFINITION_ID_COMMAND,
            SIMCONNECT_CLIENT_DATA_SET_FLAG.DEFAULT,
            0,
            new CommandStruct() { command = eventID });
        }
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

        lock (lockObject)
        {
            SimConnect?.SetDataOnSimObject(
            (DEFINE_ID)SimVars.Find(lvar => lvar.Name == simVarName).ID,
            SimConnect.SIMCONNECT_OBJECT_ID_USER,
            SIMCONNECT_DATA_SET_FLAG.DEFAULT,
            simVarValue);
        }
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
            SimVar newSimVar = new((uint)SimVars.Count + offset + 1, simVarName, simVarUnit);
            SimVars.Add(newSimVar);

            SimConnect?.AddToDataDefinition(
                (DEFINE_ID)newSimVar.ID,
                simVarName,
                simVarUnit,
                SIMCONNECT_DATATYPE.FLOAT64,
                0,
                0);

            SimConnect?.RegisterDataDefineStruct<double>((DEFINE_ID)newSimVar.ID);

            if (request)
            {
                SimConnect?.RequestDataOnSimObject(
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

        public string Unit { get; set; }

        public double Data { get; set; }

        public SimVar(uint id, string name, string unit)
        {
            ID = id;
            Name = name;
            Unit = unit;
        }

        public SimVar(string name, double data)
        {
            Name = name;
            Data = data;
        }

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