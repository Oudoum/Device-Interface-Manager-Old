using System;
using System.Linq;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CommunityToolkit.Mvvm.Messaging;
using MSFS = Microsoft.FlightSimulator.SimConnect;
using P3D = LockheedMartin.Prepar3D.SimConnect;
using Device_Interface_Manager.ViewModels;
using MahApps.Metro.Controls;

namespace Device_Interface_Manager.SimConnectProfiles;

public sealed class SimConnectClient
{
    private static readonly SimConnectClient _instance = new();

    public static SimConnectClient Instance { get =>  _instance; }

    public MSFS.SimConnect SimConnectMSFS { get; private set; } = null;

    public P3D.SimConnect SimConnectP3D { get; private set; } = null;

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
                        SimConnectMSFS?.ReceiveMessage();
                        SimConnectP3D?.ReceiveMessage();
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
        if (SimConnectMSFS is not null || SimConnectP3D is not null)
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
                if (!Properties.Settings.Default.P3D)
                {
                    SimConnectMSFS = new MSFS.SimConnect("Device-Interface-Manager", handle, WM_USER_SIMCONNECT, null, 0);
                    break;
                }

                SimConnectP3D = new P3D.SimConnect("Device-Interface-Manager", handle, WM_USER_SIMCONNECT, null, 0);
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

        if (!Properties.Settings.Default.P3D)
        {
            SimConnectMSFS.OnRecvException += SimConnect_OnRecvException;

            SimConnectMSFS.OnRecvOpen += SimConnect_OnRecvOpen;
            SimConnectMSFS.OnRecvQuit += SimConnect_OnRecvQuit;


            SimConnectMSFS.MapClientDataNameToID(CLIENT_DATA_NAME_COMMAND, CLIENT_DATA_ID.CLIENT_DATA_ID_COMMAND);
            SimConnectMSFS.AddToClientDataDefinition(DEFINE_ID.DATA_DEFINITION_ID_COMMAND, 0, MESSAGE_SIZE, 0, 0);

            PMDG.PMDG.RegisterPMDGNG3DataEvents(SimConnectMSFS);

            RegisterSimVar("CAMERA STATE", "Enum");

            SimConnectMSFS.OnRecvSimobjectData += Simconnect_OnRecvSimobjectData;
        }

        else if (Properties.Settings.Default.P3D)
        {
            SimConnectP3D.OnRecvException += SimConnect_OnRecvException;

            SimConnectP3D.OnRecvOpen += SimConnect_OnRecvOpen;
            SimConnectP3D.OnRecvQuit += SimConnect_OnRecvQuit;

            //PMDG.PMDG.RegisterPMDG777DataEvents(SimConnectP3D);
            PMDG.PMDG.RegisterPMDG747DataEvents(SimConnectP3D);

            RegisterSimVar("CAMERA STATE", "Enum");

            SimConnectP3D.OnRecvSimobjectData += Simconnect_OnRecvSimobjectData;
        }
    }

    public void SimConnect_Close()
    {
        CloseConnection(SimConnectMSFS);
        CloseConnection(SimConnectP3D);
    }

    private void CloseConnection(dynamic simConnect)
    {
        if (simConnect is not null)
        {
            handleSource.RemoveHook(HandleSimConnectEvents);
            if (simConnect is MSFS.SimConnect)
            {
                SimConnectMSFS.OnRecvSimobjectData -= Simconnect_OnRecvSimobjectData;
                SimConnectMSFS.OnRecvQuit -= SimConnect_OnRecvQuit;
                SimConnectMSFS.OnRecvOpen -= SimConnect_OnRecvOpen;
                SimConnectMSFS.OnRecvException -= SimConnect_OnRecvException;
                SimConnectMSFS.Dispose();
                SimConnectMSFS = null;
            }
            else if (simConnect is P3D.SimConnect)
            {
                SimConnectP3D.OnRecvSimobjectData -= Simconnect_OnRecvSimobjectData;
                SimConnectP3D.OnRecvQuit -= SimConnect_OnRecvQuit;
                SimConnectP3D.OnRecvOpen -= SimConnect_OnRecvOpen;
                SimConnectP3D.OnRecvException -= SimConnect_OnRecvException;
                SimConnectP3D.Dispose();
                SimConnectP3D = null;
            }

            SimVars.Clear();
            SendSimConnectConnectionStatus(false);
        }
    }

    private void SimConnect_OnRecvOpen(MSFS.SimConnect sender, MSFS.SIMCONNECT_RECV_OPEN data)
    {
        SendSimConnectConnectionStatus(true);
    }

    private void SimConnect_OnRecvOpen(P3D.SimConnect sender, P3D.SIMCONNECT_RECV_OPEN data)
    {
        SendSimConnectConnectionStatus(true);
    }

    private void SimConnect_OnRecvQuit(MSFS.SimConnect sender, MSFS.SIMCONNECT_RECV data)
    {
        SendSimConnectConnectionStatus(false);
    }

    private void SimConnect_OnRecvQuit(P3D.SimConnect sender, P3D.SIMCONNECT_RECV data)
    {
        SendSimConnectConnectionStatus(false);
    }

    private void SendSimConnectConnectionStatus(bool isConnected)
    {
        IsConnected = isConnected;
        StrongReferenceMessenger.Default.Send(new SimConnectStausMessage(isConnected));
    }

    private void SimConnect_OnRecvException(MSFS.SimConnect sender, MSFS.SIMCONNECT_RECV_EXCEPTION data)
    {
        OnRecvException<MSFS.SIMCONNECT_EXCEPTION>(data);
    }

    private void SimConnect_OnRecvException(P3D.SimConnect sender, P3D.SIMCONNECT_RECV_EXCEPTION data)
    {
        OnRecvException<P3D.SIMCONNECT_EXCEPTION>(data);
    }

    private static void OnRecvException<T>(dynamic data) where T : Enum
    {
        foreach (T item in Enum.GetValues(typeof(T)))
        {
            if ((int)(object)item == data.dwException)
            {
                System.Diagnostics.Debug.WriteLine(item);
            }
        }
    }

    public event EventHandler<SimVar> OnSimVarChanged;

    private const int offset = 4;

    private void Simconnect_OnRecvSimobjectData(MSFS.SimConnect sender, MSFS.SIMCONNECT_RECV_SIMOBJECT_DATA data)
    {
        OnRecvSimobjectData(data.dwRequestID, data.dwData[0]);
    }

    private void Simconnect_OnRecvSimobjectData(P3D.SimConnect sender, P3D.SIMCONNECT_RECV_SIMOBJECT_DATA data)
    {
        OnRecvSimobjectData(data.dwRequestID, data.dwData[0]);
    }

    private void OnRecvSimobjectData(uint dwRequestID, object dwData)
    {
        if (dwRequestID != 0)
        {
            if (SimVars.Count < (int)dwRequestID - offset)
            {
                return;
            }
            SimVars[(int)dwRequestID - offset - 1].Data = (double)dwData;
            OnSimVarChanged?.Invoke(this, SimVars[(int)dwRequestID - offset - 1]);
        }
    }

    private readonly List<SimVar> SimVars = new();

    private readonly object lockObject = new();

    public void TransmitEvent(uint data, Enum eventID)
    {
        lock (lockObject)
        {
            SimConnectMSFS?.TransmitClientEvent(
                0,
                eventID,
                data,
                PMDG.PMDG.SIMCONNECT_GROUP_PRIORITY.STANDARD,
                MSFS.SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);

            SimConnectP3D?.TransmitClientEvent(
                0,
                eventID,
                data,
                PMDG.PMDG.SIMCONNECT_GROUP_PRIORITY.STANDARD,
                P3D.SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
        }
    }

    public void SendWASMEvent(string eventID)
    {
        lock (lockObject)
        {
            SimConnectMSFS?.SetClientData(
                CLIENT_DATA_ID.CLIENT_DATA_ID_COMMAND,
                DEFINE_ID.DATA_DEFINITION_ID_COMMAND,
                MSFS.SIMCONNECT_CLIENT_DATA_SET_FLAG.DEFAULT,
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

        SimVar simVar = SimVars.Find(lvar => lvar.Name == simVarName);
        if (simVar is null)
        {
            return;
        }

        lock (lockObject)
        {
            SimConnectMSFS?.SetDataOnSimObject(
                (DEFINE_ID)simVar.ID,
                MSFS.SimConnect.SIMCONNECT_OBJECT_ID_USER,
                MSFS.SIMCONNECT_DATA_SET_FLAG.DEFAULT,
                simVarValue);

            SimConnectP3D?.SetDataOnSimObject(
                (DEFINE_ID)simVar.ID,
                P3D.SimConnect.SIMCONNECT_OBJECT_ID_USER,
                P3D.SIMCONNECT_DATA_SET_FLAG.DEFAULT,
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
            SimVar simVar = new((uint)SimVars.Count + offset + 1, simVarName, simVarUnit);
            SimVars.Add(simVar);

            SimConnectMSFS?.AddToDataDefinition(
                (DEFINE_ID)simVar.ID,
                simVar.Name,
                simVar.Unit,
                MSFS.SIMCONNECT_DATATYPE.FLOAT64,
                0,
                0);

            SimConnectP3D?.AddToDataDefinition(
                (DEFINE_ID)simVar.ID,
                simVar.Name,
                simVar.Unit,
                P3D.SIMCONNECT_DATATYPE.FLOAT64,
                0,
                0);

            SimConnectMSFS?.RegisterDataDefineStruct<double>((DEFINE_ID)simVar.ID);
            SimConnectP3D?.RegisterDataDefineStruct<double>((DEFINE_ID)simVar.ID);

            if (request)
            {
                SimConnectMSFS?.RequestDataOnSimObject(
                    (REQUEST_ID)simVar.ID,
                    (DEFINE_ID)simVar.ID,
                    MSFS.SimConnect.SIMCONNECT_OBJECT_ID_USER,
                    MSFS.SIMCONNECT_PERIOD.SIM_FRAME,
                    MSFS.SIMCONNECT_DATA_REQUEST_FLAG.CHANGED,
                    0,
                    0,
                    0);

                SimConnectP3D?.RequestDataOnSimObject(
                    (REQUEST_ID)simVar.ID,
                    (DEFINE_ID)simVar.ID,
                    P3D.SimConnect.SIMCONNECT_OBJECT_ID_USER,
                    P3D.SIMCONNECT_PERIOD.SIM_FRAME,
                    P3D.SIMCONNECT_DATA_REQUEST_FLAG.CHANGED,
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

    private struct CommandStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MESSAGE_SIZE)]
        public string command;
    }

    public enum CameraState
    {
        Cockpit = 2,
        ExternalChase = 3,
        Drone = 4,
        FixedOnPlane = 5,
        Environment = 6,
        SixDoF = 7,
        Gameplay = 8,
        Showcase = 9,
        DroneAircraft = 10,
        Waiting = 11,
        WorldMap = 12,
        HangarRTC = 13,
        HangarCustom = 14,
        MenuRTC = 15,
        InGameRTC = 16,
        Replay = 17,
        DroneTopDown = 19,
        Hangar = 21,
        Ground = 24,
        FollowTrafficAircraft = 25
    }
}