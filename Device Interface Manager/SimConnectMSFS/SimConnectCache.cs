using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CommunityToolkit.Mvvm.Messaging;
using Device_Interface_Manager.MVVM.ViewModel;
using Microsoft.FlightSimulator.SimConnect;

namespace Device_Interface_Manager.SimConnectMSFS;

public class SimConnectCache
{
    public event EventHandler Closed;
    public event EventHandler Connected;
    public event EventHandler ConnectionLost;
    public event EventHandler LVarListUpdated;

    private uint MaxClientDataDefinition = 0;


    private const string STANDARD_EVENT_GROUP = "STANDARD";
    private const string MOBIFLIGHT_CLIENT_DATA_NAME_SIMVAR = "MobiFlight.LVars";
    private const string MOBIFLIGHT_CLIENT_DATA_NAME_COMMAND = "MobiFlight.Command";
    private const string MOBIFLIGHT_CLIENT_DATA_NAME_RESPONSE = "MobiFlight.Response";

    /// The message size for commands and responses
    /// This has to be changed also in SimConnectDefintions
    private const int MOBIFLIGHT_MESSAGE_SIZE = 1024;

    /// User-defined win32 event
    public const int WM_USER_SIMCONNECT = 0x0402;

    /// Window handle
    private IntPtr _handle = new(0);

    /// SimConnect object
    private SimConnect m_oSimConnect = null;

    private bool _simConnectConnected = false;
    private bool _connected = false;

    public Dictionary<string, List<Tuple<string, uint>>> Events { get; private set; }

    public string PresetFile = null;
    public string PresetFileUser = null;

    private readonly List<SimVar> SimVars = new();
    private readonly List<string> LVars = new();
    private string ResponseStatus = "NEW";

    public void Clear()
    {
        // do nothing
    }

    public void SetHandle(IntPtr handle)
    {
        _handle = handle;
    }

    public void ReceiveSimConnectMessage()
    {
        try
        {
            m_oSimConnect?.ReceiveMessage();
        }
        catch (Exception)
        {
            StrongReferenceMessenger.Default.Send(new SimConnectStausMessage(false));
            Disconnect();
        }
    }

    private void LoadEventPresets()
    {
        Events ??= new Dictionary<string, List<Tuple<string, uint>>>();
        Events.Clear();

        PresetFile ??= @"Presets\msfs2020_eventids.cip";
        string[] lines = System.IO.File.ReadAllLines(PresetFile);
        var GroupKey = "Dummy";
        uint EventIdx = 0;

        Events[GroupKey] = new List<Tuple<string, uint>>();
        foreach (string line in lines)
        {
            if (line.StartsWith("//")) continue;

            var cols = line.Split(':');
            if (cols.Length > 1)
            {
                GroupKey = cols[0];
                if (Events.ContainsKey(GroupKey)) continue;

                Events[GroupKey] = new List<Tuple<string, uint>>();
                continue; // we found a group
            }

            Events[GroupKey].Add(new Tuple<string, uint>(cols[0], EventIdx++));
        }

        PresetFileUser ??= @"Presets\msfs2020_eventids_user.cip";
        if (System.IO.File.Exists(PresetFileUser))
        {
            lines = System.IO.File.ReadAllLines(PresetFileUser);
            GroupKey = "User";
            Events[GroupKey] = new List<Tuple<string, uint>>();
            foreach (string line in lines)
            {
                if (line.StartsWith("//")) continue;
                var cols = line.Split(':');
                if (cols.Length > 1)
                {
                    GroupKey = cols[0];
                    if (Events.ContainsKey(GroupKey)) continue;

                    Events[GroupKey] = new List<Tuple<string, uint>>();
                    continue; // we found a group
                }

                Events[GroupKey].Add(new Tuple<string, uint>(cols[0], EventIdx++));
            }
        }
    }

    internal void RefreshLVarsList()
    {
        if (m_oSimConnect == null) return;
        WasmModuleClient.GetLVarList(m_oSimConnect);
    }

    public bool Connect()
    {
        // If we have already established
        // a connection with SimConnect
        // but we are still waiting for the
        // WASM module to become available
        if (_simConnectConnected)
        {
            WasmModuleClient.Ping(m_oSimConnect);
            return true;
        }


        // Here we are only when we try
        // to connect to SimConnect first
        LoadEventPresets();

        try
        {
            // The constructor is similar to SimConnect_Open in the native API
            m_oSimConnect = new SimConnect("DeviceInterfaceManager - WASM", _handle, WM_USER_SIMCONNECT, null, 0);

            // Listen to connect and quit msgs
            m_oSimConnect.OnRecvOpen += new SimConnect.RecvOpenEventHandler(SimConnect_OnRecvOpen);
            m_oSimConnect.OnRecvQuit += new SimConnect.RecvQuitEventHandler(SimConnect_OnRecvQuit);

            // Listen to exceptions
            m_oSimConnect.OnRecvException += new SimConnect.RecvExceptionEventHandler(SimConnect_OnRecvException);
        }
        catch (COMException)
        {
            return false;
        }

        return true;
    }

    private void SimConnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
    {
        StrongReferenceMessenger.Default.Send(new SimConnectStausMessage(_simConnectConnected = true));
        // register Events
        foreach (string GroupKey in Events.Keys)
        {
            foreach (Tuple<string, uint> eventItem in Events[GroupKey])
            {
                var prefix = "";
                if (GroupKey != STANDARD_EVENT_GROUP) prefix = "MobiFlight.";
                sender.MapClientEventToSimEvent((MOBIFLIGHT_EVENTS)eventItem.Item2, prefix + eventItem.Item1);
            }
        }

        InitializeClientDataAreas(sender);
        // register receive data events
        sender.OnRecvClientData += SimConnectCache_OnRecvClientData;

        Connected?.Invoke(this, null);

        WasmModuleClient.Ping(sender);
    }

    private static void InitializeClientDataAreas(SimConnect sender)
    {
        // register Client Data (for SimVars)
        sender.MapClientDataNameToID(MOBIFLIGHT_CLIENT_DATA_NAME_SIMVAR, SIMCONNECT_CLIENT_DATA_ID.MOBIFLIGHT_LVARS);
        sender.CreateClientData(SIMCONNECT_CLIENT_DATA_ID.MOBIFLIGHT_LVARS, 4096, SIMCONNECT_CREATE_CLIENT_DATA_FLAG.DEFAULT);

        // register Client Data (for WASM Module Commands)
        //var ClientDataStringSize = (uint)Marshal.SizeOf(typeof(ClientDataString));
        sender.MapClientDataNameToID(MOBIFLIGHT_CLIENT_DATA_NAME_COMMAND, SIMCONNECT_CLIENT_DATA_ID.MOBIFLIGHT_CMD);
        sender.CreateClientData(SIMCONNECT_CLIENT_DATA_ID.MOBIFLIGHT_CMD, MOBIFLIGHT_MESSAGE_SIZE, SIMCONNECT_CREATE_CLIENT_DATA_FLAG.DEFAULT);

        // register Client Data (for WASM Module Responses)
        sender.MapClientDataNameToID(MOBIFLIGHT_CLIENT_DATA_NAME_RESPONSE, SIMCONNECT_CLIENT_DATA_ID.MOBIFLIGHT_RESPONSE);
        sender.CreateClientData(SIMCONNECT_CLIENT_DATA_ID.MOBIFLIGHT_RESPONSE, MOBIFLIGHT_MESSAGE_SIZE, SIMCONNECT_CREATE_CLIENT_DATA_FLAG.DEFAULT);

        sender.AddToClientDataDefinition((SIMCONNECT_DEFINE_ID)0, 0, MOBIFLIGHT_MESSAGE_SIZE, 0, 0);
        sender.RegisterStruct<SIMCONNECT_RECV_CLIENT_DATA, ResponseString>((SIMCONNECT_DEFINE_ID)0);
        sender.RequestClientData(
            SIMCONNECT_CLIENT_DATA_ID.MOBIFLIGHT_RESPONSE,
            (SIMCONNECT_REQUEST_ID)0,
            (SIMCONNECT_DEFINE_ID)0,
            SIMCONNECT_CLIENT_DATA_PERIOD.ON_SET,
            SIMCONNECT_CLIENT_DATA_REQUEST_FLAG.CHANGED,
            0,
            0,
            0
        );
    }

    internal void Start()
    {
        WasmModuleClient.SetConfig(m_oSimConnect, "MAX_VARS_PER_FRAME", "30");
    }

    private void SimConnectCache_OnRecvClientData(SimConnect sender, SIMCONNECT_RECV_CLIENT_DATA data)
    {
        if (data.dwRequestID != 0)
        {
            var simData = (ClientDataValue)data.dwData[0];
            if (SimVars.Count < (int)data.dwRequestID) return;
            if (SimVars[(int)(data.dwRequestID - 1)].Data != simData.data)
            SimVars[(int)(data.dwRequestID - 1)].Data = simData.data;
        }
        else
        {
            var simData = (ResponseString)data.dwData[0];

            if (simData.Data == "MF.Pong")
            {
                _connected = true;
                Connected?.Invoke(this, null);
            }
            if (simData.Data == "MF.LVars.List.Start")
            {
                ResponseStatus = "LVars.List.Receiving";
                LVars.Clear();
            }
            else if (simData.Data == "MF.LVars.List.End")
            {
                ResponseStatus = "LVars.List.Completed";
                LVarListUpdated?.Invoke(LVars, new EventArgs());
            }
            else if (ResponseStatus == "LVars.List.Receiving")
            {
                LVars.Add(simData.Data);
            }
        }
    }

    internal void Stop()
    {
        WasmModuleClient.Stop(m_oSimConnect);
        ClearSimVars();
    }

    // The case where the user closes game
    private void SimConnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
    {
        ConnectionLost?.Invoke(this, null);
        Disconnect();
    }

    private void SimConnect_OnRecvException(SimConnect sender, SIMCONNECT_RECV_EXCEPTION data)
    {
        SIMCONNECT_EXCEPTION eException = (SIMCONNECT_EXCEPTION)data.dwException;
        if (eException == SIMCONNECT_EXCEPTION.ALREADY_CREATED)
        {

        }
        else
        {

        }

    }

    public bool Disconnect()
    {
        ClearSimVars();
        MaxClientDataDefinition = 0;

        if (m_oSimConnect != null)
        {
            // Dispose serves the same purpose as SimConnect_Close()
            m_oSimConnect.Dispose();
            m_oSimConnect = null;
        }

        if (_simConnectConnected || _connected)
        {
            StrongReferenceMessenger.Default.Send(new SimConnectStausMessage(_simConnectConnected = false));

            _connected = false;

            Closed?.Invoke(this, null);
        }

        return true;
    }

    public bool IsConnected()
    {
        return _connected;
    }

    public bool IsSimConnectConnected()
    {
        return _simConnectConnected;
    }

    public void SetEventID(string eventID)
    {
        if (m_oSimConnect == null || !IsConnected()) return;

        Tuple<string, uint> eventItem = null;

        foreach (string GroupKey in Events.Keys)
        {
            eventItem = Events[GroupKey].Find(x => x.Item1 == eventID);
            if (eventItem != null) break;
        }

        if (eventItem == null)
        {
            return;
        }
        m_oSimConnect?.TransmitClientEvent(
                0,
                (MOBIFLIGHT_EVENTS)eventItem.Item2,
                1,
                SIMCONNECT_NOTIFICATION_GROUP_ID.SIMCONNECT_GROUP_PRIORITY_DEFAULT,
                SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY
        );
    }

    public float GetSimVar(string SimVarName)
    {
        float result = 0;
        if (!IsConnected())
            return result;

        if (!SimVars.Exists(lvar => lvar.Name == SimVarName))
        {
            RegisterSimVar(SimVarName);
            WasmModuleClient.SendWasmCmd(m_oSimConnect, "MF.SimVars.Add." + SimVarName);
        }

        result = SimVars.Find(lvar => lvar.Name == SimVarName).Data;

        return result;
    }

    public void SetSimVar(string SimVarCode)
    {
        WasmModuleClient.SendWasmCmd(m_oSimConnect, "MF.SimVars.Set." + SimVarCode);
        WasmModuleClient.DummyCommand(m_oSimConnect);
    }

    private void RegisterSimVar(string SimVarName)
    {
        SimVar NewSimVar = new() { Name = SimVarName, ID = (uint)SimVars.Count + 1 };
        SimVars.Add(NewSimVar);

        if (MaxClientDataDefinition >= NewSimVar.ID)
        {
            return;
        }

        MaxClientDataDefinition = NewSimVar.ID;

        m_oSimConnect?.AddToClientDataDefinition(
            (SIMCONNECT_DEFINE_ID)NewSimVar.ID,
            (uint)((SimVars.Count - 1) * sizeof(float)),
            sizeof(float),
            0,
            0);

        m_oSimConnect?.RegisterStruct<SIMCONNECT_RECV_CLIENT_DATA, ClientDataValue>((SIMCONNECT_DEFINE_ID)NewSimVar.ID);

        m_oSimConnect?.RequestClientData(
            SIMCONNECT_CLIENT_DATA_ID.MOBIFLIGHT_LVARS,
            (SIMCONNECT_REQUEST_ID)NewSimVar.ID,
            (SIMCONNECT_DEFINE_ID)NewSimVar.ID,
            SIMCONNECT_CLIENT_DATA_PERIOD.ON_SET,
            SIMCONNECT_CLIENT_DATA_REQUEST_FLAG.CHANGED,
            0,
            0,
            0
        );
    }

    private void ClearSimVars()
    {
        SimVars.Clear();
    }
}
