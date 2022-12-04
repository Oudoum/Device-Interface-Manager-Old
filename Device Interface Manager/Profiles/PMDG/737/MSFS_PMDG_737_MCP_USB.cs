using Device_Interface_Manager.MVVM.ViewModel;
using Device_Interface_Manager.Profiles;
using Microsoft.FlightSimulator.SimConnect;
using System;
using System.Linq;
using System.Threading;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceITAPI_Data;
using static Device_Interface_Manager.Profiles.PMDG.PMDG_NG3_SDK;

namespace Device_Interface_Manager.MVVM.Model
{
    public class MSFS_PMDG_737_MCP_USB
    {
        //MCP SETUP
        public static int MSFSPMDG737MCPSession { get; set; }
        private static INTERFACEIT_KEY_NOTIFY_PROC KeyNotifyCallback;
        public static bool MSFSPMDG737MCPIsEnabled { get; set; } = false;
        public static bool Flaggone { get; set; }
        static bool MCPLightOn = false;
        private static CancellationTokenSource pmdg737MCPBlinkingCancellationTokenSource = new();
        private static CancellationTokenSource pmdg737MCPIASOverspeedFlashingCancellationTokenSource = new();
        private static CancellationTokenSource pmdg737MCPIASUnderspeedFlashingCancellationTokenSource = new();
        private static CancellationTokenSource pmdg737MCPLightCancellationTokenSource = new();

        public static void MSFSPMDG737MCP()
        {
            INTERFACEIT_KEY_NOTIFY_PROC _keyNotifyCallback = new(KeyPressedProc);
            KeyNotifyCallback = _keyNotifyCallback;

            interfaceIT_LED_Enable(MSFSPMDG737MCPSession, true);
            interfaceIT_7Segment_Enable(MSFSPMDG737MCPSession, true);
            interfaceIT_Dataline_Enable(MSFSPMDG737MCPSession, true);
            interfaceIT_Switch_Enable_Callback(MSFSPMDG737MCPSession, true, KeyNotifyCallback);
            MSFSPMDG737MCPIsEnabled = true;
        }

        //MSFS PMDG737 MCP Close
        public static void MSFSPMDG737MCPClose()
        {
            interfaceIT_Switch_Enable_Callback(MSFSPMDG737MCPSession, false, KeyNotifyCallback);
            interfaceIT_Dataline_Set(MSFSPMDG737MCPSession, 5 , false);
            interfaceIT_Dataline_Enable(MSFSPMDG737MCPSession, false);
            interfaceIT_LED_Enable(MSFSPMDG737MCPSession, false);
            interfaceIT_7Segment_Enable(MSFSPMDG737MCPSession, false);

            pmdg737MCPBlinkingCancellationTokenSource?.Cancel();
            pmdg737MCPIASOverspeedFlashingCancellationTokenSource?.Cancel();
            pmdg737MCPIASUnderspeedFlashingCancellationTokenSource?.Cancel();
            pmdg737MCPLightCancellationTokenSource?.Cancel();

            MSFSPMDG737MCPIsEnabled = false;
        }

        //Method for MCP Lights
        private static Thread pmdg737MCPLight;
        static bool pmdg737NotPowered;
        static private void PMDG737MCPLight(bool pmdg737MCPLightValue , CancellationToken token)
        {
            if (pmdg737MCPLightValue && !pmdg737NotPowered)
            {
                Thread.Sleep(1500);
            }
            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 209, pmdg737MCPLightValue);
            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 210, pmdg737MCPLightValue);
            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 211, pmdg737MCPLightValue);
            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 212, pmdg737MCPLightValue);
            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 213, pmdg737MCPLightValue);
            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 214, pmdg737MCPLightValue);
            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 215, pmdg737MCPLightValue);
            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 217, pmdg737MCPLightValue);
            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 218, pmdg737MCPLightValue);
            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 219, pmdg737MCPLightValue);
            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 220, pmdg737MCPLightValue);
            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 221, pmdg737MCPLightValue);
            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 222, pmdg737MCPLightValue);
            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 223, pmdg737MCPLightValue);
            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 233, pmdg737MCPLightValue);
            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 234, pmdg737MCPLightValue);
            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 235, pmdg737MCPLightValue);
            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 236, pmdg737MCPLightValue);
            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 237, pmdg737MCPLightValue);
            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 238, pmdg737MCPLightValue);
            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 239, pmdg737MCPLightValue);
            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 240, pmdg737MCPLightValue);

            if (HomeViewModel.SimConnectClient.PMDGData.ELEC_BusPowered[15])
            {
                pmdg737NotPowered = false;
            }

            if (token.IsCancellationRequested) 
            {
                return;
            }
        }

        //Thread Method for IAS/MACH flashing
        static private void PMDG737MCPFlashing(string flashingAB, CancellationToken token)
        {
            while (true)
            {
                if (HomeViewModel.SimConnectClient.PMDGData.MCP_IASMach < 1)
                {
                    interfaceIT_7Segment_Display(MSFSPMDG737MCPSession, flashingAB + string.Format("{0,3}", HomeViewModel.SimConnectClient.PMDGData.MCP_IASMach.ToString("#.00", System.Globalization.CultureInfo.InvariantCulture)), 6);
                    Thread.Sleep(500);
                    interfaceIT_7Segment_Display(MSFSPMDG737MCPSession, " " + string.Format("{0,3}", HomeViewModel.SimConnectClient.PMDGData.MCP_IASMach.ToString("#.00", System.Globalization.CultureInfo.InvariantCulture)), 6);
                    Thread.Sleep(500);
                }
                else
                {
                    interfaceIT_7Segment_Display(MSFSPMDG737MCPSession, flashingAB + string.Format("{0,3}", HomeViewModel.SimConnectClient.PMDGData.MCP_IASMach.ToString(System.Globalization.CultureInfo.InvariantCulture)), 5);
                    Thread.Sleep(500);
                    interfaceIT_7Segment_Display(MSFSPMDG737MCPSession, " " + string.Format("{0,3}", HomeViewModel.SimConnectClient.PMDGData.MCP_IASMach.ToString(System.Globalization.CultureInfo.InvariantCulture)), 5);
                    Thread.Sleep(500);
                }

                if (token.IsCancellationRequested)
                {
                    return;
                }
            }
        }

        //Thread Method for MCP blinking
        static private void PMDG737MCPBlinking(CancellationToken token)
        {
            while (true)
            {
                interfaceIT_7Segment_Display(MSFSPMDG737MCPSession, "888 888888888880-8880888", 1);
                Thread.Sleep(2000);
                interfaceIT_7Segment_Display(MSFSPMDG737MCPSession, new string('-', 24).Replace("-", " "), 1);
                Thread.Sleep(1000);

                if (token.IsCancellationRequested) 
                {
                    return;
                }
            }
        }

        //Method for sending PMDG data to the FDS-MCP
        private static Thread pmdg737MCPIASOverspeedFlashing;
        private static Thread pmdg737MCPIASUnderspeedFlashing;
        private static Thread pmdg737MCPBlinking;

        public static void HandleVariableReceivedMSFS_PMDG_737_MCP()
        {
            // MCP Powered
            if (HomeViewModel.SimConnectClient.PMDGData.MCP_indication_powered)
            {
                // Lighttets
                if (HomeViewModel.SimConnectClient.PMDGData.MAIN_LightsSelector !=0)
                {
                    pmdg737MCPBlinkingCancellationTokenSource?.Cancel();
                    pmdg737MCPBlinking = null;

                    interfaceIT_7Segment_Display(MSFSPMDG737MCPSession, HomeViewModel.SimConnectClient.PMDGData.MCP_Course[0].ToString("D3"), 1);
                    interfaceIT_7Segment_Display(MSFSPMDG737MCPSession, HomeViewModel.SimConnectClient.PMDGData.MCP_Course[1].ToString("D3"), 22);
                    interfaceIT_7Segment_Display(MSFSPMDG737MCPSession, HomeViewModel.SimConnectClient.PMDGData.MCP_Heading.ToString("D3"), 9);

                    // IAS Blank
                    if (HomeViewModel.SimConnectClient.PMDGData.MCP_IASBlank)
                    {
                        interfaceIT_7Segment_Display(MSFSPMDG737MCPSession, "   ", 6);
                    }
                    else if (!HomeViewModel.SimConnectClient.PMDGData.MCP_IASBlank && !HomeViewModel.SimConnectClient.PMDGData.MCP_IASOverspeedFlash  && !HomeViewModel.SimConnectClient.PMDGData.MCP_IASUnderspeedFlash )
                    {
                        if (HomeViewModel.SimConnectClient.PMDGData.MCP_IASMach < 1)
                        {
                            interfaceIT_7Segment_Display(MSFSPMDG737MCPSession, string.Format("{0,3}", HomeViewModel.SimConnectClient.PMDGData.MCP_IASMach.ToString("#.00", System.Globalization.CultureInfo.InvariantCulture)), 6);
                        }
                        else
                        {
                            interfaceIT_7Segment_Display(MSFSPMDG737MCPSession, string.Format("{0,3}", HomeViewModel.SimConnectClient.PMDGData.MCP_IASMach.ToString(System.Globalization.CultureInfo.InvariantCulture)), 6);
                        }
                    }

                    // MCP Altitude 0 => 00000
                    if (HomeViewModel.SimConnectClient.PMDGData.MCP_Altitude == 0)
                    {
                        interfaceIT_7Segment_Display(MSFSPMDG737MCPSession, "00000", 12);
                    }
                    else if (HomeViewModel.SimConnectClient.PMDGData.MCP_Altitude > 0)
                    {
                        interfaceIT_7Segment_Display(MSFSPMDG737MCPSession, string.Format("{0,5}", HomeViewModel.SimConnectClient.PMDGData.MCP_Altitude.ToString()), 12);
                    }

                    // VertSpeed Blank
                    if (HomeViewModel.SimConnectClient.PMDGData.MCP_VertSpeedBlank || HomeViewModel.SimConnectClient.PMDGData.MCP_VertSpeed == 0)
                    {
                        interfaceIT_7Segment_Display(MSFSPMDG737MCPSession, "     ", 17);
                    }
                    else if (!HomeViewModel.SimConnectClient.PMDGData.MCP_VertSpeedBlank)
                    {
                        interfaceIT_7Segment_Display(MSFSPMDG737MCPSession, string.Format("{0,5}", HomeViewModel.SimConnectClient.PMDGData.MCP_VertSpeed.ToString()), 17);
                    }

                    //IAS Overspeed Flash
                    if (HomeViewModel.SimConnectClient.PMDGData.MCP_IASOverspeedFlash && !HomeViewModel.SimConnectClient.PMDGData.MCP_IASBlank)
                    {
                        if (pmdg737MCPIASOverspeedFlashing == null)
                        {
                            pmdg737MCPIASOverspeedFlashingCancellationTokenSource = new();
                            pmdg737MCPIASOverspeedFlashing = new Thread(() => PMDG737MCPFlashing("B", pmdg737MCPIASOverspeedFlashingCancellationTokenSource.Token));
                            pmdg737MCPIASOverspeedFlashing.Start();
                        }
                    }
                    //IAS Underspeed Flash
                    else if (HomeViewModel.SimConnectClient.PMDGData.MCP_IASUnderspeedFlash && !HomeViewModel.SimConnectClient.PMDGData.MCP_IASBlank)
                    {
                        if (pmdg737MCPIASUnderspeedFlashing == null)
                        {
                            pmdg737MCPIASUnderspeedFlashingCancellationTokenSource = new();
                            pmdg737MCPIASUnderspeedFlashing = new Thread(() => PMDG737MCPFlashing("A", pmdg737MCPIASUnderspeedFlashingCancellationTokenSource.Token));
                            pmdg737MCPIASUnderspeedFlashing.Start();
                        }
                    }
                    if (!HomeViewModel.SimConnectClient.PMDGData.MCP_IASOverspeedFlash)
                    {
                        pmdg737MCPIASOverspeedFlashingCancellationTokenSource?.Cancel();
                        pmdg737MCPIASOverspeedFlashing = null;
                    }
                    if (!HomeViewModel.SimConnectClient.PMDGData.MCP_IASUnderspeedFlash)
                    {
                        pmdg737MCPIASUnderspeedFlashingCancellationTokenSource?.Cancel();
                        pmdg737MCPIASUnderspeedFlashing = null;
                    }
                    if (!HomeViewModel.SimConnectClient.PMDGData.MCP_IASOverspeedFlash && !HomeViewModel.SimConnectClient.PMDGData.MCP_IASUnderspeedFlash)
                    {
                        interfaceIT_7Segment_Display(MSFSPMDG737MCPSession, " ", 5);
                    }
                }
                else if (HomeViewModel.SimConnectClient.PMDGData.MAIN_LightsSelector == 0)
                {
                    if (pmdg737MCPBlinking == null)
                    {
                        interfaceIT_7Segment_Display(MSFSPMDG737MCPSession, "888 888888888880-8880888", 1);
                        pmdg737MCPBlinkingCancellationTokenSource = new();
                        pmdg737MCPBlinking = new Thread(() => PMDG737MCPBlinking(pmdg737MCPBlinkingCancellationTokenSource.Token));
                        pmdg737MCPBlinking.Start();
                    }
                }
            }
            else if (!HomeViewModel.SimConnectClient.PMDGData.MCP_indication_powered)
            {
                pmdg737MCPIASOverspeedFlashingCancellationTokenSource?.Cancel();
                pmdg737MCPIASOverspeedFlashing = null;
                pmdg737MCPIASUnderspeedFlashingCancellationTokenSource?.Cancel();
                pmdg737MCPIASUnderspeedFlashing = null;
                pmdg737MCPBlinkingCancellationTokenSource?.Cancel();
                pmdg737MCPBlinking = null;
                interfaceIT_7Segment_Display(MSFSPMDG737MCPSession, new string('-', 24).Replace("-", " "), 1);
            }
 

            // AT Arm (Solenoid)
            interfaceIT_Dataline_Set(MSFSPMDG737MCPSession, 5, HomeViewModel.SimConnectClient.PMDGData.MCP_ATArmSw);


            // LED
            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 225, HomeViewModel.SimConnectClient.PMDGData.MCP_annunFD[0]);

            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 226, HomeViewModel.SimConnectClient.PMDGData.MCP_annunFD[1]);

            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 227, HomeViewModel.SimConnectClient.PMDGData.MCP_annunATArm);

            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 193, HomeViewModel.SimConnectClient.PMDGData.MCP_annunN1);

            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 194, HomeViewModel.SimConnectClient.PMDGData.MCP_annunSPEED);

            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 195, HomeViewModel.SimConnectClient.PMDGData.MCP_annunVNAV);

            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 196, HomeViewModel.SimConnectClient.PMDGData.MCP_annunLVL_CHG);

            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 197, HomeViewModel.SimConnectClient.PMDGData.MCP_annunHDG_SEL);

            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 198, HomeViewModel.SimConnectClient.PMDGData.MCP_annunLNAV);

            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 199, HomeViewModel.SimConnectClient.PMDGData.MCP_annunVOR_LOC);

            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 201, HomeViewModel.SimConnectClient.PMDGData.MCP_annunAPP);

            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 202, HomeViewModel.SimConnectClient.PMDGData.MCP_annunALT_HOLD);

            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 203, HomeViewModel.SimConnectClient.PMDGData.MCP_annunVS);

            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 204, HomeViewModel.SimConnectClient.PMDGData.MCP_annunCMD_A);

            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 205, HomeViewModel.SimConnectClient.PMDGData.MCP_annunCWS_A);

            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 206, HomeViewModel.SimConnectClient.PMDGData.MCP_annunCMD_B);

            interfaceIT_LED_Set(MSFSPMDG737MCPSession, 207, HomeViewModel.SimConnectClient.PMDGData.MCP_annunCWS_B);


            // Background LED
            if (HomeViewModel.SimConnectClient.PMDGData.ELEC_BusPowered[15] && HomeViewModel.SimConnectClient.PMDGData.LTS_MainPanelKnob[0] > 0)
            {
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 209, true);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 210, true);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 211, true);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 212, true);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 213, true);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 214, true);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 215, true);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 217, true);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 218, true);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 219, true);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 220, true);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 221, true);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 222, true);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 223, true);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 233, true);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 234, true);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 235, true);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 236, true);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 237, true);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 238, true);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 239, true);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 240, true);
            }
            if (!HomeViewModel.SimConnectClient.PMDGData.ELEC_BusPowered[2])
            {
                pmdg737NotPowered = true;
            }
            if (HomeViewModel.SimConnectClient.PMDGData.ELEC_BusPowered[15] && !HomeViewModel.SimConnectClient.PMDGData.ELEC_BusPowered[7])
            {
                if (!MCPLightOn)
                {
                    MCPLightOn = true;
                    pmdg737MCPLightCancellationTokenSource = new();
                    pmdg737MCPLight = new Thread(() => PMDG737MCPLight(true, pmdg737MCPLightCancellationTokenSource.Token));
                    pmdg737MCPLight.Start();
                }
            }
            else if (!HomeViewModel.SimConnectClient.PMDGData.ELEC_BusPowered[15] || HomeViewModel.SimConnectClient.PMDGData.ELEC_BusPowered[7] && HomeViewModel.SimConnectClient.PMDGData.LTS_MainPanelKnob[0] == 0)
            {
                MCPLightOn = false;
                pmdg737MCPLightCancellationTokenSource?.Cancel();
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 209, false);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 210, false);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 211, false);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 212, false);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 213, false);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 214, false);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 215, false);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 217, false);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 218, false);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 219, false);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 220, false);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 221, false);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 222, false);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 223, false);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 233, false);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 234, false);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 235, false);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 236, false);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 237, false);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 238, false);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 239, false);
                interfaceIT_LED_Set(MSFSPMDG737MCPSession, 240, false);
            }
        }

        //Delegate for button presses
        static uint EFIS_CPT_VOR_ADF_SELECTOR_R;
        static uint EFIS_CPT_VOR_ADF_SELECTOR_L;
        static uint nMCPCourse_0;
        private static bool KeyPressedProc(int Session, int Switch, int Direction)
        {
            uint nDirection = (uint)Direction;

            //1-0
            if (new int[] { 2, 3, 25, 69, 71, 79, 80 }.Contains(Switch))
            {
                if (Direction == 1)
                {
                    nDirection = 0;
                }

                if (Direction == 0)
                {
                    nDirection = 1;
                }
            }

            //1-2
            if (new int[] { 70, 72 }.Contains(Switch))
            {
                if (Direction == 1)
                {
                    nDirection = 2;
                }

                if (Direction == 0 && (EFIS_CPT_VOR_ADF_SELECTOR_L != 0 || EFIS_CPT_VOR_ADF_SELECTOR_R != 0))
                {
                    nDirection = 1;
                }
            }

            //-1 & -14
            if (new int[] { 4, 5, 6, 8, 9, 10, 12, 13, 14, 15, 17, 19, 20, 21, 23, 24, 73, 74, 75, 76, 77, 78, 81, 82, 83, 84, 85, 86, 87 }.Contains(Switch))
            {
                if (Direction == 1)
                {
                    nDirection = MOUSE_FLAG_LEFTSINGLE;
                }

                if (Direction == 0)
                {
                    nDirection = MOUSE_FLAG_LEFTRELEASE;
                }
            }

            switch (Switch)
            {
                //Special for BARO & MINS +-10
                case 1:
                    nMCPCourse_0 = nDirection;
                    break;

                //0-1
                case 22:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_DISENGAGE_BAR, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                //1-0
                case 2:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_FD_SWITCH_L, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 3:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_AT_ARM_SWITCH, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 25:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_FD_SWITCH_R, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 69:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_VOR_ADF_SELECTOR_L, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    EFIS_CPT_VOR_ADF_SELECTOR_L = nDirection;
                    break;

                case 71:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_VOR_ADF_SELECTOR_R, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    EFIS_CPT_VOR_ADF_SELECTOR_R = nDirection;
                    break;

                case 79:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS_RADIO_BARO, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 80:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO_IN_HPA, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                //1-2
                case 70:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_VOR_ADF_SELECTOR_L, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 72:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_VOR_ADF_SELECTOR_R, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                //0-...
                case 26 when Direction == 1:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR, 0, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 27 when Direction == 1:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR, 1, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 28 when Direction == 1:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR, 2, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 29 when Direction == 1:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR, 3, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 30 when Direction == 1:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR, 4, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 65 when Direction == 1:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MODE, 0, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 66 when Direction == 1:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MODE, 1, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 67 when Direction == 1:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MODE, 2, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 68 when Direction == 1:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MODE, 3, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 89 when Direction == 1:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 0, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 90 when Direction == 1:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 1, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 91 when Direction == 1:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 2, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 92 when Direction == 1:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 3, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 93 when Direction == 1:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 4, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 94 when Direction == 1:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 5, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 95 when Direction == 1:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 6, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 96 when Direction == 1:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 7, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                //-1 & -14
                case 4:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_N1_SWITCH, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 5:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_SPEED_SWITCH, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 6:
                    pmdg737MCPIASOverspeedFlashingCancellationTokenSource?.Cancel();
                    pmdg737MCPIASOverspeedFlashing = null;
                    pmdg737MCPIASUnderspeedFlashingCancellationTokenSource?.Cancel();
                    pmdg737MCPIASUnderspeedFlashing = null;
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_CO_SWITCH, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 8:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_SPD_INTV_SWITCH, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 9:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_VNAV_SWITCH, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 10:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_LVL_CHG_SWITCH, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 12:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_HDG_SEL_SWITCH, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 13:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_LNAV_SWITCH, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 14:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_VOR_LOC_SWITCH, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 15:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_APP_SWITCH, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 17:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_ALT_HOLD_SWITCH, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 19:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_VS_SWITCH, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 20:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_CMD_A_SWITCH, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 21:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_CWS_A_SWITCH, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 23:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_CMD_B_SWITCH, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 24:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_CWS_B_SWITCH, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 73:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_FPV, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 74:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MTRS, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 75:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MODE_CTR, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 76:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE_TFC, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 77:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS_RST, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 78:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO_STD, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 81:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_WXR, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 82:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_STA, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 83:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_WPT, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 84:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_ARPT, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 85:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_DATA, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 86:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_POS, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 87:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_TERR, nDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                //-18 & -17
                case 33 when Direction == 1:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_COURSE_SELECTOR_L, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 34 when Direction == 1:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_COURSE_SELECTOR_L, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 35 when Direction == 1:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_SPEED_SELECTOR, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 36 when Direction == 1:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_SPEED_SELECTOR, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 37 when Direction == 1:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_HEADING_SELECTOR, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 38 when Direction == 1:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_HEADING_SELECTOR, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 41 when Direction == 1:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_ALTITUDE_SELECTOR, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 42 when Direction == 1:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_ALTITUDE_SELECTOR, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 43 when Direction == 1:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_VS_SELECTOR, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 44 when Direction == 1:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_VS_SELECTOR, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 45 when Direction == 1:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_COURSE_SELECTOR_R, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 46 when Direction == 1:
                    SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_COURSE_SELECTOR_R, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                //-18 & -17 (-1 & -3)
                case 57 when Direction == 1:
                    if (nMCPCourse_0 == 0)
                    {
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    }
                    if (nMCPCourse_0 == 1)
                    {
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    }
                    break;

                case 58 when Direction == 1:
                    if (nMCPCourse_0 == 0)
                    {
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    }
                    if (nMCPCourse_0 == 1)
                    {
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    }
                    break;

                case 59 when Direction == 1:
                    if (nMCPCourse_0 == 0)
                    {
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    }
                    if (nMCPCourse_0 == 1)
                    {
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    }
                    break;

                case 60 when Direction == 1:
                    if (nMCPCourse_0 == 0)
                    {
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    }
                    if (nMCPCourse_0 == 1)
                    {
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    }
                    break;

                default:
                    break;
            }
            return true;
        }
    }
}