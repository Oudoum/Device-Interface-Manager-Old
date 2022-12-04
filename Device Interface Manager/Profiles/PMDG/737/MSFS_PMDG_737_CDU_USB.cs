using Device_Interface_Manager.MVVM.ViewModel;
using Device_Interface_Manager.Profiles;
using Microsoft.FlightSimulator.SimConnect;
using System;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceITAPI_Data;
using static Device_Interface_Manager.Profiles.PMDG.PMDG_NG3_SDK;

namespace Device_Interface_Manager.MVVM.Model
{
    public class MSFS_PMDG_737_CDU_USB
    {
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
                        interfaceIT_LED_Set(HomeModel.Profile0_MSFS_PMDG_737_CDU.Session, 1, cDU_annunEXEC0);
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
                        interfaceIT_LED_Set(HomeModel.Profile0_MSFS_PMDG_737_CDU.Session, 2, cDU_annunCALL0);
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
                        interfaceIT_LED_Set(HomeModel.Profile0_MSFS_PMDG_737_CDU.Session, 3, cDU_annunMSG0);
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
                        interfaceIT_LED_Set(HomeModel.Profile0_MSFS_PMDG_737_CDU.Session, 4, cDU_annunFAIL0);
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
                        interfaceIT_LED_Set(HomeModel.Profile0_MSFS_PMDG_737_CDU.Session, 5, cDU_annunOFST0);
                    }
                }
            }

            private static byte lTS_PedPanelKnob;
            public static byte LTS_PedPanelKnob
            {
                get
                {
                    return lTS_PedPanelKnob;
                }
                set
                {
                    if (value != lTS_PedPanelKnob)
                    {
                        lTS_PedPanelKnob = value;
                        interfaceIT_Brightness_Set(HomeModel.Profile0_MSFS_PMDG_737_CDU.Session, (int)(lTS_PedPanelKnob * 1.5));
                    }
                }
            }

            //Method for sending PMDG data to the FDS-CDU
            public static void HandleVariableReceivedMSFS_PMDG_737_CDU()
            {
                CDU_annunEXEC0 = HomeViewModel.SimConnectClient.PMDGData.CDU_annunEXEC[0];

                CDU_annunCALL0 = HomeViewModel.SimConnectClient.PMDGData.CDU_annunCALL[0];

                CDU_annunMSG0 = HomeViewModel.SimConnectClient.PMDGData.CDU_annunMSG[0];

                CDU_annunFAIL0 = HomeViewModel.SimConnectClient.PMDGData.CDU_annunFAIL[0];

                CDU_annunOFST0 = HomeViewModel.SimConnectClient.PMDGData.CDU_annunOFST[0];

                LTS_PedPanelKnob = HomeViewModel.SimConnectClient.PMDGData.LTS_PedPanelKnob;
            }
        }

        public static class MSFS_PMDG_737_Captain_CDU_Events
        {
            public static INTERFACEIT_KEY_NOTIFY_PROC KeyNotifyCallback { get; set; } = new(KeyPressedProc);

            //Delegate for button presses
            private static bool KeyPressedProc(int Session, int Switch, int oDirection)
            {
                uint Direction = (uint)oDirection;

                if (Direction == 1 || Switch == 49)
                {
                    if (Direction == 1)
                    {
                        Direction = MOUSE_FLAG_LEFTSINGLE;
                    }

                    else if (Direction == 0)
                    {
                        Direction = MOUSE_FLAG_LEFTRELEASE;
                    }

                    switch (Switch)
                    {
                        case 1:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_EXEC, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 2:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_PROG, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 3:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_HOLD, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 4:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_CRZ, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 5:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_DEP_ARR, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 6:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_LEGS, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 7:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_MENU, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 8:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_CLB, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 9:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_E, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 10:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_D, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 11:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_C, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 12:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_B, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 13:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_A, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 14:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_FIX, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 15:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_N1_LIMIT, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 16:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_RTE, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 17:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_J, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 18:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_I, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 19:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_H, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 20:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_G, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 21:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_F, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 22:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_NEXT_PAGE, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 23:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_PREV_PAGE, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 25:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_O, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 26:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_N, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 27:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_M, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 28:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_L, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 29:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_K, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 30:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_3, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 31:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_2, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 32:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_1, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 33:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_T, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 34:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_S, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 35:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_R, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 36:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_Q, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 37:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_P, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 38:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_6, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 39:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_5, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 40:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_4, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 41:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_Y, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 42:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_X, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 43:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_W, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 44:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_V, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 45:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_U, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 46:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_9, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 47:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_8, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 48:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_7, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 49:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_CLR, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 50:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_SLASH, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 51:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_DEL, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 52:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_SPACE, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 53:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_Z, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 54:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_PLUS_MINUS, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 55:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_0, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 56:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_DOT, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 57:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_L1, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 58:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_L2, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 59:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_L3, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 60:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_L4, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 61:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_L5, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 62:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_L6, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 63:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_INIT_REF, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 64:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_R1, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 65:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_R2, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 66:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_R3, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 67:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_R4, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 68:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_R5, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 69:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_R6, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 70:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_DES, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        default:
                            break;
                    }
                }
                return true;
            }
        }

        public static class MSFS_PMDG_737_Firstofficer_CDU_Data
        {
            private static bool cDU_annunEXEC1;
            public static bool CDU_annunEXEC1
            {
                get
                {
                    return cDU_annunEXEC1;
                }
                set
                {
                    if (value != cDU_annunEXEC1)
                    {
                        cDU_annunEXEC1 = value;
                        interfaceIT_LED_Set(HomeModel.Profile0_MSFS_PMDG_737_CDU.Session, 1, cDU_annunEXEC1);
                    }
                }
            }

            private static bool cDU_annunCALL1;
            public static bool CDU_annunCALL1
            {
                get
                {
                    return cDU_annunCALL1;
                }
                set
                {
                    if (value != cDU_annunCALL1)
                    {
                        cDU_annunCALL1 = value;
                        interfaceIT_LED_Set(HomeModel.Profile0_MSFS_PMDG_737_CDU.Session, 2, cDU_annunCALL1);
                    }
                }
            }

            private static bool cDU_annunFAIL1;
            public static bool CDU_annunFAIL1
            {
                get
                {
                    return cDU_annunFAIL1;
                }
                set
                {
                    if (value != cDU_annunFAIL1)
                    {
                        cDU_annunFAIL1 = value;
                        interfaceIT_LED_Set(HomeModel.Profile0_MSFS_PMDG_737_CDU.Session, 4, cDU_annunFAIL1);
                    }
                }
            }

            private static bool cDU_annunMSG1;
            public static bool CDU_annunMSG1
            {
                get
                {
                    return cDU_annunMSG1;
                }
                set
                {
                    if (value != cDU_annunMSG1)
                    {
                        cDU_annunMSG1 = value;
                        interfaceIT_LED_Set(HomeModel.Profile0_MSFS_PMDG_737_CDU.Session, 3, cDU_annunMSG1);
                    }
                }
            }

            private static bool cDU_annunOFST1;
            public static bool CDU_annunOFST1
            {
                get
                {
                    return cDU_annunOFST1;
                }
                set
                {
                    if (value != cDU_annunOFST1)
                    {
                        cDU_annunOFST1 = value;
                        interfaceIT_LED_Set(HomeModel.Profile0_MSFS_PMDG_737_CDU.Session, 5, cDU_annunOFST1);
                    }
                }
            }

            private static byte lTS_PedPanelKnob;
            public static byte LTS_PedPanelKnob
            {
                get
                {
                    return lTS_PedPanelKnob;
                }
                set
                {
                    if (value != lTS_PedPanelKnob)
                    {
                        lTS_PedPanelKnob = value;
                        interfaceIT_Brightness_Set(HomeModel.Profile0_MSFS_PMDG_737_CDU.Session, (int)(lTS_PedPanelKnob * 1.5));
                    }
                }
            }

            //Method for sending PMDG data to the FDS-CDU
            public static void HandleVariableReceivedMSFS_PMDG_737_CDU()
            {
                CDU_annunEXEC1 = HomeViewModel.SimConnectClient.PMDGData.CDU_annunEXEC[1];

                CDU_annunCALL1 = HomeViewModel.SimConnectClient.PMDGData.CDU_annunCALL[1];

                CDU_annunFAIL1 = HomeViewModel.SimConnectClient.PMDGData.CDU_annunFAIL[1];

                CDU_annunMSG1 = HomeViewModel.SimConnectClient.PMDGData.CDU_annunMSG[1];

                CDU_annunOFST1 = HomeViewModel.SimConnectClient.PMDGData.CDU_annunOFST[1];

                LTS_PedPanelKnob = HomeViewModel.SimConnectClient.PMDGData.LTS_PedPanelKnob;
            }
        }
        public static class MSFS_PMDG_737_Firstofficer_CDU_Events
        {
            public static INTERFACEIT_KEY_NOTIFY_PROC KeyNotifyCallback { get; set; } = new(KeyPressedProc);

            //Delegate for button presses
            private static bool KeyPressedProc(int Session, int Switch, int oDirection)
            {
                uint Direction = (uint)oDirection;

                if (Direction == 1 | Switch == 49)
                {
                    if (Direction == 1)
                    {
                        Direction = MOUSE_FLAG_LEFTSINGLE;
                    }

                    if (Direction == 0)
                    {
                        Direction = MOUSE_FLAG_LEFTRELEASE;
                    }

                    switch (Switch)
                    {
                        case 1:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_EXEC, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 2:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_PROG, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 3:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_HOLD, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 4:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_CRZ, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 5:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_DEP_ARR, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 6:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_LEGS, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 7:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_MENU, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 8:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_CLB, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 9:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_E, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 10:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_D, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 11:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_C, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 12:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_B, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 13:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_A, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 14:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_FIX, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 15:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_N1_LIMIT, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 16:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_RTE, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 17:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_J, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 18:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_I, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 19:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_H, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 20:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_G, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 21:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_F, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 22:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_NEXT_PAGE, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 23:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_PREV_PAGE, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 25:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_O, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 26:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_N, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 27:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_M, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 28:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_L, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 29:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_K, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 30:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_3, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 31:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_2, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 32:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_1, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 33:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_T, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 34:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_S, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 35:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_R, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 36:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_Q, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 37:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_P, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 38:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_6, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 39:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_5, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 40:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_4, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 41:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_Y, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 42:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_X, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 43:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_W, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 44:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_V, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 45:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_U, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 46:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_9, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 47:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_8, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 48:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_7, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 49:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_CLR, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 50:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_SLASH, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 51:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_DEL, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 52:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_SPACE, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 53:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_Z, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 54:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_PLUS_MINUS, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 55:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_0, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 56:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_DOT, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 57:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_L1, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 58:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_L2, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 59:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_L3, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 60:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_L4, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 61:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_L5, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 62:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_L6, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 63:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_INIT_REF, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 64:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_R1, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 65:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_R2, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 66:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_R3, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 67:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_R4, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 68:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_R5, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 69:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_R6, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        case 70:
                            SimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_DES, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                            break;

                        default:
                            break;
                    }
                }
                return true;
            }
        }
    }
}