using Device_Interface_Manager.MVVM.Model;
using Device_Interface_Manager.MVVM.ViewModel;
using Microsoft.FlightSimulator.SimConnect;
using System;
using static PMDG_SDK.PMDG_NG3_SDK;

namespace Device_Interface_Manager.Profiles.PMDG.B737
{
    public class MSFS_PMDG_737_CDU_E
    {
        public static InterfaceITEthernet InterfaceITEthernet { get; set; }

        public static class MSFS_PMDG_737_Captain_CDU_Data
        {
            private static bool cDU_annunEXEC0;
            public static bool CDU_annunEXEC0
            {
                get
                {
                    return cDU_annunEXEC0;
                }
                set
                {
                    if (value != cDU_annunEXEC0)
                    {
                        cDU_annunEXEC0 = value;
                        InterfaceITEthernet.SendintefaceITEthernetLED(1, Convert.ToInt32(cDU_annunEXEC0));
                    }
                }
            }

            private static bool cDU_annunCALL0;
            public static bool CDU_annunCALL0
            {
                get
                {
                    return cDU_annunCALL0;
                }
                set
                {
                    if (value != cDU_annunCALL0)
                    {
                        cDU_annunCALL0 = value;
                        InterfaceITEthernet.SendintefaceITEthernetLED(2, Convert.ToInt32(cDU_annunCALL0));
                    }
                }
            }

            private static bool cDU_annunMSG0;
            public static bool CDU_annunMSG0
            {
                get
                {
                    return cDU_annunMSG0;
                }
                set
                {
                    if (value != cDU_annunMSG0)
                    {
                        cDU_annunMSG0 = value;
                        InterfaceITEthernet.SendintefaceITEthernetLED(3, Convert.ToInt32(cDU_annunMSG0));
                    }
                }
            }

            private static bool cDU_annunFAIL0;
            public static bool CDU_annunFAIL0
            {
                get
                {
                    return cDU_annunFAIL0;
                }
                set
                {
                    if (value != cDU_annunFAIL0)
                    {
                        cDU_annunFAIL0 = value;
                        InterfaceITEthernet.SendintefaceITEthernetLED(4, Convert.ToInt32(cDU_annunFAIL0));
                    }
                }
            }

            private static bool cDU_annunOFST0;
            public static bool CDU_annunOFST0
            {
                get
                {
                    return cDU_annunOFST0;
                }
                set
                {
                    if (value != cDU_annunOFST0)
                    {
                        cDU_annunOFST0 = value;
                        InterfaceITEthernet.SendintefaceITEthernetLED(5, Convert.ToInt32(cDU_annunOFST0));
                    }
                }
            }

            public static void HandleVariableReceivedMSFS_PMDG_737_CDU()
            {
                CDU_annunEXEC0 = HomeViewModel.SimConnectClient.PMDGData.CDU_annunEXEC[0];

                CDU_annunCALL0 = HomeViewModel.SimConnectClient.PMDGData.CDU_annunCALL[0];

                CDU_annunMSG0 = HomeViewModel.SimConnectClient.PMDGData.CDU_annunMSG[0];

                CDU_annunFAIL0 = HomeViewModel.SimConnectClient.PMDGData.CDU_annunFAIL[0];

                CDU_annunOFST0 = HomeViewModel.SimConnectClient.PMDGData.CDU_annunOFST[0];
            }
        }


        public static class MSFS_PMDG_737_Captain_Events
        {
            public static System.Threading.Thread ReceivedDataThread { get; set; }

            //Delegate for button presses
            public static InterfaceITEthernet.INTERFACEIT_ETHERNET_KEY_NOTIFY_PROC EthernetKeyNotifyCallback { get; set; } = new(KeyPressedProcEthernet);

            private static void KeyPressedProcEthernet(int Switch, string Direction)
            {
                uint oDirection;
                if (Direction == "ON")
                {
                    oDirection = MOUSE_FLAG_LEFTSINGLE;
                }

                else if (Direction == "OFF" && Switch == 8)
                {
                    oDirection = MOUSE_FLAG_LEFTRELEASE;
                }

                else if (Direction == "OFF" && Switch == 63)
                {
                    oDirection = MOUSE_FLAG_LEFTRELEASE;
                }
                else if (Direction == "OFF" && Switch == 64)
                {
                    oDirection = MOUSE_FLAG_LEFTRELEASE;
                }

                else return;

                switch (Switch)
                {
                    case 1:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_DOT, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 2:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_0, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 3:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_PLUS_MINUS, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 4:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_Z, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 5:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_SPACE, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 6:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_DEL, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 7:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_SLASH, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 8:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_CLR, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 9:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_7, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 10:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_8, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 11:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_9, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 12:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_U, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 13:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_V, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 14:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_W, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 15:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_X, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 16:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_Y, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 17:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_4, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 18:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_5, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 19:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_6, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 20:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_P, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 21:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_Q, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 22:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_R, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 23:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_S, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 24:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_T, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 25:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_1, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 26:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_2, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 27:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_3, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 28:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_K, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 29:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_L, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 30:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_M, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 31:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_N, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 32:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_O, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 33:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_PREV_PAGE, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 34:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_NEXT_PAGE, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 35:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_L1, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 36:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_F, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 37:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_G, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 38:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_H, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 39:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_I, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 40:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_J, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 41:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_N1_LIMIT, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 42:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_FIX, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 43:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_L2, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 44:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_A, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 45:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_B, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 46:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_C, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 47:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_D, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 48:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_E, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 49:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_MENU, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 50:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_LEGS, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 51:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_L3, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 52:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_DEP_ARR, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 53:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_HOLD, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 54:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_PROG, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 55:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_EXEC, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 56:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_L5, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 57:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_INIT_REF, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 58:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_RTE, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 59:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_L4, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 60:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_CLB, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 61:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_CRZ, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 62:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_DES, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 63:
                        //SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_BRITENESS, 2, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 64:
                        //SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_BRITENESS, 0 , SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 65:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_R1, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 66:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_R2, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 67:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_R3, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 68:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_R4, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 69:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_R5, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 70:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_R6, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    case 71:
                        SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_L6, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        break;

                    default:
                        break;
                }
            }
        }
    }
}