using System;
using Microsoft.FlightSimulator.SimConnect;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using Device_Interface_Manager.MVVM.View;
using System.Threading;
using static Device_Interface_Manager.Profiles.PMDG.PMDG_NG3_SDK;
using Device_Interface_Manager.MVVM.Model;
using Device_Interface_Manager.Profiles.PMDG.B737;
using Device_Interface_Manager.MVVM.ViewModel;
using Microsoft.VisualBasic;
using System.Diagnostics;

namespace Device_Interface_Manager.Profiles
{

    public class SimConnectClient
    {
        public static SimConnect Simconnect { get; set; } = null;
        private readonly IntPtr m_hWnd = new(0);
        private const int WM_USER_SIMCONNECT = 0x0402;


        public ObservableCollection<SimvarRequest> LSimvarRequests { get; private set; }
        public SimvarRequest OSelectedSimvarRequest
        {
            get { return m_oSelectedSimvarRequest; }
            set { m_oSelectedSimvarRequest = value; }
        }
        private SimvarRequest m_oSelectedSimvarRequest = null;

        public bool BObjectIDSelectionEnabled
        {
            get { return m_bObjectIDSelectionEnabled; }
            set { m_bObjectIDSelectionEnabled = value; }
        }
        private bool m_bObjectIDSelectionEnabled = false;

        public ObservableCollection<uint> LObjectIDs { get; private set; }
        public uint IObjectIdRequest
        {
            get { return m_iObjectIdRequest; }
            set
            { m_iObjectIdRequest = value; }
        }
        private uint m_iObjectIdRequest = 0;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        struct Struct1
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string sValue;
        };


        //The SimConnect_Open function is used to send a request to the Microsoft Flight Simulator server to open up communications with a new client.
        public void SimConnect_Open()
        {
            while (MainViewModel.HomeVM.IsSimConnectOpen == false)
            {
                try
                {
                    /// The constructor is similar to SimConnect_Open in the native API
                    Simconnect = new SimConnect("DeviceInterfaceManager", m_hWnd, WM_USER_SIMCONNECT, null, 0);

                    /// Listen to connect and quit msgs
                    Simconnect.OnRecvOpen += new SimConnect.RecvOpenEventHandler(SimConnect_OnRecvOpen);
                    Simconnect.OnRecvQuit += new SimConnect.RecvQuitEventHandler(SimConnect_OnRecvQuit);

                    /// Listen to exceptions
                    Simconnect.OnRecvException += new SimConnect.RecvExceptionEventHandler(SimConnect_OnRecvException);

                    /// Listen to ClientData
                    Simconnect.OnRecvClientData += new SimConnect.RecvClientDataEventHandler(SimConnect_OnRecvClientData);

                    /// Listen to SystemState
                    Simconnect.OnRecvSystemState += new SimConnect.RecvSystemStateEventHandler(SimConnect_OnRecvSystemState);

                    //request current aircraft file path
                    Simconnect.RequestSystemState(DATA_REQUEST_ID.AIR_PATH_REQUEST, "AircraftLoaded");

                    break;
                }
                catch (COMException)
                {

                }
            }


            if (Simconnect != null)
            {
                //
                //PMDG NG3
                //
                //     //Methode 1
                //     // Associate an ID with the PMDG control area name
                //     simconnect.MapClientDataNameToID(PMDG_SDK.PMDG_NG3_SDK.PMDG_NG3_CONTROL_NAME, PMDG_SDK.PMDG_NG3_SDK.PMDG_NG3.CONTROL_ID);
                //     // Define the control area - this is a required step
                //     simconnect.AddToClientDataDefinition(PMDG_SDK.PMDG_NG3_SDK.PMDG_NG3.CONTROL_DEFINITION, 0, (uint)Marshal.SizeOf<PMDG_SDK.PMDG_NG3_SDK.PMDG_NG3_Control>(), 0, 0);
                //     // Sign up for notification of control change
                //     simconnect.RequestClientData(
                //         PMDG_SDK.PMDG_NG3_SDK.PMDG_NG3.CONTROL_ID,
                //         PMDG_SDK.PMDG_NG3_SDK.DATA_REQUEST_ID.CONTROL_REQUEST,
                //         PMDG_SDK.PMDG_NG3_SDK.PMDG_NG3.CONTROL_DEFINITION,
                //         SIMCONNECT_CLIENT_DATA_PERIOD.ON_SET,
                //         (SIMCONNECT_CLIENT_DATA_REQUEST_FLAG)SIMCONNECT_DATA_REQUEST_FLAG.CHANGED,
                //         0, 0, 0);
                //
                //     // Create new PMDG_NG3_Control object
                //     PMDG_SDK.PMDG_NG3_SDK.PMDG_NG3_Control pmdg_NG3_Control = new PMDG_SDK.PMDG_NG3_SDK.PMDG_NG3_Control
                //     {
                //         Event = (uint)PMDG_SDK.PMDG_NG3_SDK.PMDGEvents.EVT_OH_ELEC_BATTERY_SWITCH,
                //         Parameter = 1
                //     };
                //
                //     // Set PMDG Event to Sim
                //     simconnect.SetClientData(PMDG_SDK.PMDG_NG3_SDK.PMDG_NG3.CONTROL_ID, PMDG_SDK.PMDG_NG3_SDK.PMDG_NG3.CONTROL_DEFINITION, 0, (uint)Marshal.SizeOf<PMDG_SDK.PMDG_NG3_SDK.PMDG_NG3_Control>(), pmdg_NG3_Control);
                //     simconnect.ReceiveMessage();

                //Methode 2
                // Map the PMDG Events to SimConnect
                foreach (PMDGEvents eventid in Enum.GetValues(typeof(PMDGEvents)))
                {
                    Simconnect.MapClientEventToSimEvent(eventid, "#" + Convert.ChangeType(eventid, eventid.GetTypeCode()).ToString());
                }


                // Associate an ID with the PMDG data area name
                Simconnect.MapClientDataNameToID(PMDG_NG3_DATA_NAME, PMDG_NG3.DATA_ID);
                // Define the data area structure - this is a required step
                Simconnect.AddToClientDataDefinition(PMDG_NG3.DATA_DEFINITION, 0, (uint)Marshal.SizeOf<PMDG_NG3_Data>(), 0, 0);
                // Register the data are structure
                Simconnect.RegisterStruct<SIMCONNECT_RECV_CLIENT_DATA, PMDG_NG3_Data>(PMDG_NG3.DATA_DEFINITION);
                // Sign up for notification of data change
                Simconnect.RequestClientData(
                    PMDG_NG3.DATA_ID,
                    DATA_REQUEST_ID.DATA_REQUEST,
                    PMDG_NG3.DATA_DEFINITION,
                    SIMCONNECT_CLIENT_DATA_PERIOD.ON_SET,
                    (SIMCONNECT_CLIENT_DATA_REQUEST_FLAG)SIMCONNECT_DATA_REQUEST_FLAG.CHANGED,
                    0, 0, 0);

                // Associate an ID with the PMDG data CDU0 area name
                Simconnect.MapClientDataNameToID(PMDG_NG3_CDU_0_NAME, PMDG_NG3.CDU_0_ID);
                // Define the data area CDU0 structure - this is a required step
                Simconnect.AddToClientDataDefinition(PMDG_NG3.CDU_0_DEFINITION, 0, (uint)Marshal.SizeOf<PMDG_NG3_CDU_Screen>(), 0, 0);
                // Register the data area CDU0 structure
                Simconnect.RegisterStruct<SIMCONNECT_RECV_CLIENT_DATA, PMDG_NG3_CDU_Screen>(PMDG_NG3.CDU_0_DEFINITION);
                // Sign up for notification of CDU0 data change
                Simconnect.RequestClientData(
                    PMDG_NG3.CDU_0_ID,
                    DATA_REQUEST_ID.CDU0_REQUEST,
                    PMDG_NG3.CDU_0_DEFINITION,
                    SIMCONNECT_CLIENT_DATA_PERIOD.ON_SET,
                    SIMCONNECT_CLIENT_DATA_REQUEST_FLAG.CHANGED,
                    0, 0, 0);


                // Associate an ID with the PMDG data CDU1 area name
                Simconnect.MapClientDataNameToID(PMDG_NG3_CDU_1_NAME, PMDG_NG3.CDU_1_ID);
                // Define the data area CDU1 structure - this is a required step
                Simconnect.AddToClientDataDefinition(PMDG_NG3.CDU_1_DEFINITION, 0, (uint)Marshal.SizeOf<PMDG_NG3_CDU_Screen>(), 0, 0);
                // Register the data area CDU1 structure
                Simconnect.RegisterStruct<SIMCONNECT_RECV_CLIENT_DATA, PMDG_NG3_CDU_Screen>(PMDG_NG3.CDU_1_DEFINITION);
                // Sign up for notification of CDU1 data change
                Simconnect.RequestClientData(
                    PMDG_NG3.CDU_1_ID,
                    DATA_REQUEST_ID.CDU1_REQUEST,
                    PMDG_NG3.CDU_1_DEFINITION,
                    SIMCONNECT_CLIENT_DATA_PERIOD.ON_SET,
                    SIMCONNECT_CLIENT_DATA_REQUEST_FLAG.CHANGED,
                    0, 0, 0);
                //
                //
                //
            }
        }

        public static void ReceiveSimConnectMessage(CancellationToken token)
        {
            while (true)
            {
                try
                {
                    Simconnect?.ReceiveMessage();
                    Thread.Sleep(10);
                }
                catch
                {

                }
                if (token.IsCancellationRequested)
                {
                    return;
                }
            }
        }

        private void SimConnect_OnRecvSystemState(SimConnect sender, SIMCONNECT_RECV_SYSTEM_STATE data)
        {

        }

        public PMDG_NG3_Data PMDGData { get; set; }
        public bool PMDGMCPIsStarted { get; set; }
        public PMDG737CDU PMDG737CDU0 { get; set; }
        private bool pMDG737CDU0Settings = false;
        public PMDG737CDU PMDG737CDU1 { get; set; }
        private bool pMDG737CDU1Settings = false;
        private void SimConnect_OnRecvClientData(SimConnect sender, SIMCONNECT_RECV_CLIENT_DATA data)
        {
            switch (data.dwRequestID)
            {
                case (uint)DATA_REQUEST_ID.CONTROL_REQUEST:
                    PMDG_NG3_Control PMDGControl = (PMDG_NG3_Control)data.dwData[0];
                    break;

                case (uint)DATA_REQUEST_ID.DATA_REQUEST:
                    PMDGData = (PMDG_NG3_Data)data.dwData[0];
                    if (MainViewModel.HomeVM.MSFS_PMDG_B737_MCP_Enabled == 2)
                    {
                        if (!PMDGMCPIsStarted)
                        {
                            MSFS_PMDG_737_MCP_USB.MSFSPMDG737MCP();
                            PMDGMCPIsStarted = true;
                        }
                    }
                    if (MSFS_PMDG_737_MCP_USB.Flaggone)
                    {
                        MSFS_PMDG_737_MCP_USB.HandleVariableReceivedMSFS_PMDG_737_MCP();
                    }
                    if (HomeModel.Profile0_MSFS_PMDG_737_CDU != null)
                    {
                        MSFS_PMDG_737_CDU_USB.MSFS_PMDG_737_Captain_CDU_Data.HandleVariableReceivedMSFS_PMDG_737_CDU();
                    }
                    if (HomeModel.Profile1_MSFS_PMDG_737_CDU != null)
                    {
                        MSFS_PMDG_737_CDU_USB.MSFS_PMDG_737_Firstofficer_CDU_Data.HandleVariableReceivedMSFS_PMDG_737_CDU();
                    }
                    if (MSFS_PMDG_737_CDU_E.MSFS_PMDG_737_Captain_Events.ReceivedDataThread != null)
                    {
                        MSFS_PMDG_737_CDU_E.MSFS_PMDG_737_Captain_CDU_Data.HandleVariableReceivedMSFS_PMDG_737_CDU();
                    }
                    if (!PMDGData.ELEC_BusPowered[3])
                    {
                        PMDG737CDU0?.Dispatcher.BeginInvoke(delegate ()
                        {
                            GetPMDG737CDU0Settings();
                            PMDG737CDU0.ClearPMDGCDUCells();
                        });
                        PMDG737CDU1?.Dispatcher.BeginInvoke(delegate ()
                        {
                            GetPMDG737CDU1Settings();
                            PMDG737CDU1.ClearPMDGCDUCells();
                        });
                    }
                    break;

                case (uint)DATA_REQUEST_ID.CDU0_REQUEST:

                    if (PMDG737CDU0 != null)
                    {
                        PMDG737CDU0.Closing += PMDG737CDU0_Closing;
                        PMDG737CDU0.Dispatcher.BeginInvoke(delegate ()
                        {
                            GetPMDG737CDU0Settings();
                            PMDG737CDU0.GetPMDGCDUCells((PMDG_NG3_CDU_Screen)data.dwData[0]);
                            if (!PMDG737CDU0.IsActive)
                            {
                                Thread.Sleep(500);
                                Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_DOT, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                                Thread.Sleep(500);
                                Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_CLR, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                                PMDG737CDU0?.Show();
                            }
                        });
                    }
                    break;

                case (uint)DATA_REQUEST_ID.CDU1_REQUEST:
                    if (PMDG737CDU1 != null)
                    {
                        PMDG737CDU1.Closing += PMDG737CDU1_Closing;
                        PMDG737CDU1.Dispatcher.BeginInvoke(delegate ()
                        {
                            GetPMDG737CDU1Settings();
                            PMDG737CDU1.GetPMDGCDUCells((PMDG_NG3_CDU_Screen)data.dwData[0]);
                            if (!PMDG737CDU1.IsActive)
                            {
                                Thread.Sleep(500);
                                Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_DOT, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                                Thread.Sleep(500);
                                Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_CLR, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                                PMDG737CDU1?.Show();
                            }
                        });
                    }
                    break;
            }
        }

        private void GetPMDG737CDU0Settings()
        {
            if (!pMDG737CDU0Settings)
            {
                PMDG737CDU0.Top = Properties.Settings.Default.PMDG737CDU0Top;
                PMDG737CDU0.Left = Properties.Settings.Default.PMDG737CDU0Left;
                PMDG737CDU0.Height = Properties.Settings.Default.PMDG737CDU0Height;
                PMDG737CDU0.Width = Properties.Settings.Default.PMDG737CDU0Width;
                PMDG737CDU0.fontSize = Properties.Settings.Default.PMDG737CDU0FontSize;
                PMDG737CDU0.marginTop = Properties.Settings.Default.PMDG737CDU0MarginTop;
                PMDG737CDU0.marginBottom = Properties.Settings.Default.PMDG737CDU0MarginBottom;
                PMDG737CDU0.marginLeft = Properties.Settings.Default.PMDG737CDU0MarginLeft;
                PMDG737CDU0.marginRight = Properties.Settings.Default.PMDG737CDU0MarginRight;
                if (Properties.Settings.Default.PMDG737CDU0GridWidth == 0)
                {
                    PMDG737CDU0.CDUGrid.Width = double.NaN;
                }
                else
                {
                    PMDG737CDU0.CDUGrid.Width = Properties.Settings.Default.PMDG737CDU0GridWidth;
                }
                if (Properties.Settings.Default.PMDG737CDU0GridHeight == 0)
                {
                    PMDG737CDU0.CDUGrid.Height = double.NaN;
                }
                else
                {
                    PMDG737CDU0.CDUGrid.Height = Properties.Settings.Default.PMDG737CDU0GridHeight;
                }
                pMDG737CDU0Settings = true;
            }
        }

        private void GetPMDG737CDU1Settings()
        {
            if (!pMDG737CDU1Settings)
            {
                PMDG737CDU1.Top = Properties.Settings.Default.PMDG737CDU1Top;
                PMDG737CDU1.Left = Properties.Settings.Default.PMDG737CDU1Left;
                PMDG737CDU1.Height = Properties.Settings.Default.PMDG737CDU1Height;
                PMDG737CDU1.Width = Properties.Settings.Default.PMDG737CDU1Width;
                PMDG737CDU1.fontSize = Properties.Settings.Default.PMDG737CDU1FontSize;
                PMDG737CDU1.marginTop = Properties.Settings.Default.PMDG737CDU1MarginTop;
                PMDG737CDU1.marginBottom = Properties.Settings.Default.PMDG737CDU1MarginBottom;
                PMDG737CDU1.marginLeft = Properties.Settings.Default.PMDG737CDU1MarginLeft;
                PMDG737CDU1.marginRight = Properties.Settings.Default.PMDG737CDU1MarginRight;
                if (Properties.Settings.Default.PMDG737CDU1GridWidth == 0)
                {
                    PMDG737CDU1.CDUGrid.Width = double.NaN;
                }
                else
                {
                    PMDG737CDU1.CDUGrid.Width = Properties.Settings.Default.PMDG737CDU1GridWidth;
                }
                if (Properties.Settings.Default.PMDG737CDU1GridHeight == 0)
                {
                    PMDG737CDU1.CDUGrid.Height = double.NaN;
                }
                else
                {
                    PMDG737CDU1.CDUGrid.Height = Properties.Settings.Default.PMDG737CDU1GridHeight;
                }
                pMDG737CDU1Settings = true;
            }
        }

        private void PMDG737CDU0_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.PMDG737CDU0Top = PMDG737CDU0.Top;
            Properties.Settings.Default.PMDG737CDU0Left = PMDG737CDU0.Left;
            Properties.Settings.Default.PMDG737CDU0Height = PMDG737CDU0.Height;
            Properties.Settings.Default.PMDG737CDU0Width = PMDG737CDU0.Width;
            Properties.Settings.Default.PMDG737CDU0FontSize = PMDG737CDU0.fontSize;
            Properties.Settings.Default.PMDG737CDU0MarginTop = PMDG737CDU0.marginTop;
            Properties.Settings.Default.PMDG737CDU0MarginBottom = PMDG737CDU0.marginBottom;
            Properties.Settings.Default.PMDG737CDU0MarginLeft = PMDG737CDU0.marginLeft;
            Properties.Settings.Default.PMDG737CDU0MarginRight = PMDG737CDU0.marginRight;
            Properties.Settings.Default.PMDG737CDU0GridWidth = PMDG737CDU0.CDUGrid.Width;
            Properties.Settings.Default.PMDG737CDU0GridHeight = PMDG737CDU0.CDUGrid.Height;
            Properties.Settings.Default.Save();
            pMDG737CDU0Settings = false;
        }

        private void PMDG737CDU1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.PMDG737CDU1Top = PMDG737CDU1.Top;
            Properties.Settings.Default.PMDG737CDU1Left = PMDG737CDU1.Left;
            Properties.Settings.Default.PMDG737CDU1Height = PMDG737CDU1.Height;
            Properties.Settings.Default.PMDG737CDU1Width = PMDG737CDU1.Width;
            Properties.Settings.Default.PMDG737CDU1FontSize = PMDG737CDU1.fontSize;
            Properties.Settings.Default.PMDG737CDU1MarginTop = PMDG737CDU1.marginTop;
            Properties.Settings.Default.PMDG737CDU1MarginBottom = PMDG737CDU1.marginBottom;
            Properties.Settings.Default.PMDG737CDU1MarginLeft = PMDG737CDU1.marginLeft;
            Properties.Settings.Default.PMDG737CDU1MarginRight = PMDG737CDU1.marginRight;
            Properties.Settings.Default.PMDG737CDU1GridWidth = PMDG737CDU1.CDUGrid.Width;
            Properties.Settings.Default.PMDG737CDU1GridHeight = PMDG737CDU1.CDUGrid.Height;
            Properties.Settings.Default.Save();
            pMDG737CDU1Settings = false;
        }


        //Register Data to SimConnect
        public bool RegisterToSimConnect(SimvarRequest _oSimvarRequest)
        {
            if (Simconnect != null)
            {
                if (_oSimvarRequest.BIsString)
                {
                    /// Define a data structure containing string value
                    Simconnect.AddToDataDefinition(_oSimvarRequest.eDef, _oSimvarRequest.SName, "", SIMCONNECT_DATATYPE.STRING256, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                    Simconnect.RegisterDataDefineStruct<Struct1>(_oSimvarRequest.eDef);
                }
                else
                {
                    /// Define a data structure containing numerical value
                    Simconnect.AddToDataDefinition(_oSimvarRequest.eDef, _oSimvarRequest.SName, _oSimvarRequest.SUnits, SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                    Simconnect.RegisterDataDefineStruct<double>(_oSimvarRequest.eDef);
                }

                /// Catch a simobject data request
                Simconnect.OnRecvSimobjectDataBytype += new SimConnect.RecvSimobjectDataBytypeEventHandler(SimConnect_OnRecvSimobjectDataBytype);

                return true;
            }
            else
            {
                return false;
            }
        }

        //The SimConnect_Close function is used to request that the communication with the server is ended.
        public void SimConnect_Close()
        {
            Simconnect?.Dispose();
            Simconnect = null;
            MainViewModel.HomeVM.IsSimConnectOpen = false;
        }

        //The SIMCONNECT_RECV_OPEN structure is used to return information to the client, after a successful call to SimConnect_Open.
        public void SimConnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        {
            MainViewModel.HomeVM.IsSimConnectOpen = true;
        }

        //The SIMCONNECT_RECV_QUIT is an identical structure to the SIMCONNECT_RECV structure.
        //The SIMCONNECT_RECV structure is used with the SIMCONNECT_RECV_ID enumeration to indicate which type of structure has been returned.
        private void SimConnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
        {
            MainViewModel.HomeVM.IsSimConnectOpen = false;
        }

        //The SIMCONNECT_RECV_EXCEPTION structure is used with the SIMCONNECT_EXCEPTION enumeration type to return information on an error that has occurred.
        private void SimConnect_OnRecvException(SimConnect sender, SIMCONNECT_RECV_EXCEPTION data)
        {
            if (data.dwException != 9)
            {
                WriteENETLog("SimConnect_OnRecvException: " + ((SIMCONNECT_EXCEPTION)data.dwException).ToString());
            }
        }

        //The SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE structure will be received by the client after a successful call to SimConnect_RequestDataOnSimObjectType.
        //It is an identical structure to SIMCONNECT_RECV_SIMOBJECT_DATA.
        private void SimConnect_OnRecvSimobjectDataBytype(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE data)
        {
            uint iRequest = data.dwRequestID;
            uint iObject = data.dwObjectID;
            if (!LObjectIDs.Contains(iObject))
            {
                LObjectIDs.Add(iObject);
            }
            foreach (SimvarRequest oSimvarRequest in LSimvarRequests)
            {
                if (iRequest == (uint)oSimvarRequest.eRequest && (!BObjectIDSelectionEnabled || iObject == m_iObjectIdRequest))
                {
                    if (oSimvarRequest.BIsString)
                    {
                        Struct1 result = (Struct1)data.dwData[0];
                        oSimvarRequest.DValue = 0;
                        oSimvarRequest.SValue = result.sValue;
                    }
                    else
                    {
                        double dValue = (double)data.dwData[0];
                        oSimvarRequest.DValue = dValue;
                        oSimvarRequest.SValue = dValue.ToString("F9");
                    }

                    oSimvarRequest.bPending = false;
                    oSimvarRequest.BStillPending = false;
                }
            }
        }

        private const string simconnectlog = @"Log\SimConnectLog.txt";
        private string WriteENETLog(string data)
        {
            System.IO.Directory.CreateDirectory("Log");
            System.IO.File.AppendAllText(simconnectlog, Environment.NewLine + DateAndTime.Now.ToString() + Environment.NewLine);
            System.IO.File.AppendAllText(simconnectlog, data);
            return data;
        }
    }


    public enum DEFINITION
    {
        Dummy = 0
    };

    public enum REQUEST
    {
        Dummy = 0,
        Struct1
    };

    public enum COPY_ITEM
    {
        Name = 0,
        Value,
        Unit
    }

    public class SimvarRequest
    {
        public DEFINITION eDef = DEFINITION.Dummy;
        public REQUEST eRequest = REQUEST.Dummy;

        public string SName { get; set; }

        public bool BIsString { get; set; }

        public double DValue
        {
            get { return m_dValue; }
            set { m_dValue = value; }
        }
        private double m_dValue = 0.0;

        public string SValue
        {
            get { return m_sValue; }
            set { m_sValue = value; }
        }
        private string m_sValue = null;

        public string SUnits { get; set; }

        public bool bPending = true;

        public bool BStillPending
        {
            get { return m_bStillPending; }
            set { m_bStillPending = value; }
        }
        private bool m_bStillPending = false;
    };
}