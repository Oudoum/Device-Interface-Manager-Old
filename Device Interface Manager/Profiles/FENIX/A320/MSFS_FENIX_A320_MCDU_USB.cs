using Device_Interface_Manager.MVVM.Model;
using System;
using System.Threading;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceITAPI_Data;

namespace Device_Interface_Manager.Profiles.FENIX.A320
{
    public class MSFS_FENIX_A320_MCDU_USB
    {
        public static class MSFS_FENIX_A320_Captain_MCDU_Data
        {
            public static Thread ReceivedDataThread { get; set; }

            public static void ReceiveDataThread(CancellationToken token)
            {
                while (true)
                {
                    HomeModel.simConnectCache.ReceiveSimConnectMessage();
                    Thread.Sleep(10);
                    GetSimVar();
                    Thread.Sleep(10);
                    if (token.IsCancellationRequested) 
                    {
                        return;
                    }
                }
            }

            private static int n_PED_LIGHTING_PEDESTAL;
            public static int N_PED_LIGHTING_PEDESTAL
            {
                get
                {
                    return n_PED_LIGHTING_PEDESTAL;
                }
                set
                {
                    if (value != n_PED_LIGHTING_PEDESTAL)
                    {
                        n_PED_LIGHTING_PEDESTAL = value;
                        interfaceIT_Brightness_Set(HomeModel.Profile5_MSFS_FENIX_A320_MCDU.Session, (int)(n_PED_LIGHTING_PEDESTAL * 50 * 2.55));
                    }
                }
            }

            private static bool i_CDU1_FAIL;
            public static bool I_CDU1_FAIL
            {
                get
                {
                    return i_CDU1_FAIL;
                }
                set
                {
                    if (value != i_CDU1_FAIL)
                    {
                        i_CDU1_FAIL = value;
                        interfaceIT_LED_Set(HomeModel.Profile5_MSFS_FENIX_A320_MCDU.Session, 2, i_CDU1_FAIL);
                    }
                }
            }

            private static bool i_CDU1_MCDU_MENU;
            public static bool I_CDU1_MCDU_MENU
            {
                get
                {
                    return i_CDU1_MCDU_MENU;
                }
                set
                {
                    if (value != i_CDU1_MCDU_MENU)
                    {
                        i_CDU1_MCDU_MENU = value;
                        interfaceIT_LED_Set(HomeModel.Profile5_MSFS_FENIX_A320_MCDU.Session, 3, i_CDU1_MCDU_MENU);
                        interfaceIT_LED_Set(HomeModel.Profile5_MSFS_FENIX_A320_MCDU.Session, 5, i_CDU1_MCDU_MENU);
                    }
                }
            }

            private static bool i_CDU1_FM;
            public static bool I_CDU1_FM
            {
                get
                {
                    return i_CDU1_FM;
                }
                set
                {
                    if (value != i_CDU1_FM)
                    {
                        i_CDU1_FM = value;
                        interfaceIT_LED_Set(HomeModel.Profile5_MSFS_FENIX_A320_MCDU.Session, 4, i_CDU1_FM);
                    }
                }
            }

            private static bool i_CDU1_FM1;
            public static bool I_CDU1_FM1
            {
                get
                {
                    return i_CDU1_FM1;
                }
                set
                {
                    if (value != i_CDU1_FM1)
                    {
                        i_CDU1_FM1 = value;
                        interfaceIT_LED_Set(HomeModel.Profile5_MSFS_FENIX_A320_MCDU.Session, 9, i_CDU1_FM1);
                    }
                }
            }

            private static bool i_CDU1_IND;
            public static bool I_CDU1_IND
            {
                get
                {
                    return i_CDU1_IND;
                }
                set
                {
                    if (value != i_CDU1_IND)
                    {
                        i_CDU1_IND = value;
                        interfaceIT_LED_Set(HomeModel.Profile5_MSFS_FENIX_A320_MCDU.Session, 10, i_CDU1_IND);
                    }
                }
            }

            private static bool i_CDU1_RDY;
            public static bool I_CDU1_RDY
            {
                get
                {
                    return i_CDU1_RDY;
                }
                set
                {
                    if (value != i_CDU1_RDY)
                    {
                        i_CDU1_RDY = value;
                        interfaceIT_LED_Set(HomeModel.Profile5_MSFS_FENIX_A320_MCDU.Session, 11, i_CDU1_RDY);
                    }
                }
            }

            private static bool i_CDU1_DASH;
            public static bool I_CDU1_DASH
            {
                get
                {
                    return i_CDU1_DASH;
                }
                set
                {
                    if (value != i_CDU1_DASH)
                    {
                        i_CDU1_DASH = value;
                        interfaceIT_LED_Set(HomeModel.Profile5_MSFS_FENIX_A320_MCDU.Session, 12, i_CDU1_DASH);
                    }
                }
            }

            private static bool i_CDU1_FM2;
            public static bool I_CDU1_FM2
            {
                get
                {
                    return i_CDU1_FM2;
                }
                set
                {
                    if (value != i_CDU1_FM2)
                    {
                        i_CDU1_FM2 = value;
                        interfaceIT_LED_Set(HomeModel.Profile5_MSFS_FENIX_A320_MCDU.Session, 13, i_CDU1_FM2);
                    }
                }
            }

            //Get SimVars
            private static void GetSimVar()
            {
                // Background light
                N_PED_LIGHTING_PEDESTAL = (int)HomeModel.simConnectCache.GetSimVar("(L:N_PED_LIGHTING_PEDESTAL)");

                // Output LEDs
                I_CDU1_FAIL = Convert.ToBoolean(HomeModel.simConnectCache.GetSimVar("(L:I_CDU1_FAIL)"));

                I_CDU1_MCDU_MENU = Convert.ToBoolean(HomeModel.simConnectCache.GetSimVar("(L:I_CDU1_MCDU_MENU)"));

                I_CDU1_FM = Convert.ToBoolean(HomeModel.simConnectCache.GetSimVar("(L:I_CDU1_FM)"));

                I_CDU1_FM1 = Convert.ToBoolean(HomeModel.simConnectCache.GetSimVar("(L:I_CDU1_FM1)"));

                I_CDU1_IND = Convert.ToBoolean(HomeModel.simConnectCache.GetSimVar("(L:I_CDU1_IND)"));

                I_CDU1_RDY = Convert.ToBoolean(HomeModel.simConnectCache.GetSimVar("(L:I_CDU1_RDY)"));

                I_CDU1_DASH = Convert.ToBoolean(HomeModel.simConnectCache.GetSimVar("(L:I_CDU1_DASH)"));

                I_CDU1_FM2 = Convert.ToBoolean(HomeModel.simConnectCache.GetSimVar("(L:I_CDU1_FM2)"));
            }
        }

        public static class MSFS_FENIX_A320_Captain_Events
        {
            public static INTERFACEIT_KEY_NOTIFY_PROC KeyNotifyCallback { get; set; } = new(KeyPressedProc);

            //Delegate for button presses
            private static bool KeyPressedProc(int Session, int Switch, int Direction)
            {

                if (Direction == 1)
                {
                }

                if (Direction == 0)
                {
                }

                switch (Switch)
                {
                    case 1:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_MENU)");
                        break;

                    case 2:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_ATC_COM)");
                        break;

                    case 3:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_SEC_FPLN)");
                        break;

                    case 4:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_INIT)");
                        break;

                    case 5:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_FUEL_PRED)");
                        break;

                    case 6:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_RAD_NAV)");
                        break;

                    case 7:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_FPLN)");
                        break;

                    case 8:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_PERF)");
                        break;

                    case 9:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_E)");
                        break;

                    case 10:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_D)");
                        break;

                    case 11:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_C)");
                        break;

                    case 12:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_B)");
                        break;

                    case 13:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_A)");
                        break;

                    case 14:

                        break;

                    case 15:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_AIRPORT)");
                        break;

                    case 16:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_PROG)");
                        break;

                    case 17:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_J)");
                        break;

                    case 18:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_I)");
                        break;

                    case 19:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_H)");
                        break;

                    case 20:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_G)");
                        break;

                    case 21:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_F)");
                        break;

                    case 22:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_ARROW_UP)");
                        break;

                    case 23:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_ARROW_LEFT)");
                        break;

                    case 24:

                        break;

                    case 25:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_O)");
                        break;

                    case 26:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_N)");
                        break;

                    case 27:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_M)");
                        break;

                    case 28:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_L)");
                        break;

                    case 29:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_K)");
                        break;

                    case 30:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_3)");
                        break;

                    case 31:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_2)");
                        break;

                    case 32:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_1)");
                        break;

                    case 33:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_T)");
                        break;

                    case 34:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_S)");
                        break;

                    case 35:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_R)");
                        break;

                    case 36:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_Q)");
                        break;

                    case 37:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_P)");
                        break;

                    case 38:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_6)");
                        break;

                    case 39:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_5)");
                        break;

                    case 40:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_4)");
                        break;

                    case 41:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_Y)");
                        break;

                    case 42:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_X)");
                        break;

                    case 43:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_W)");
                        break;

                    case 44:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_V)");
                        break;

                    case 45:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_U)");
                        break;

                    case 46:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_9)");
                        break;

                    case 47:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_8)");
                        break;

                    case 48:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_7)");
                        break;

                    case 49:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_CLEAR)");
                        break;

                    case 50:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_OVFLY)");
                        break;

                    case 51:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_SPACE)");
                        break;

                    case 52:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_SLASH)");
                        break;

                    case 53:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_Z)");
                        break;

                    case 54:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_MINUS)");
                        break;

                    case 55:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_0)");
                        break;

                    case 56:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_DOT)");
                        break;

                    case 57:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_LSK1L)");
                        break;

                    case 58:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_LSK2L)");
                        break;

                    case 59:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_LSK3L)");
                        break;

                    case 60:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_LSK4L)");
                        break;

                    case 61:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_LSK5L)");
                        break;

                    case 62:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_LSK6L)");
                        break;

                    case 63:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_DIR)");
                        break;

                    case 64:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_LSK1R)");
                        break;

                    case 65:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_LSK2R)");
                        break;

                    case 66:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_LSK3R)");
                        break;

                    case 67:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_LSK4R)");
                        break;

                    case 68:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_LSK5R)");
                        break;

                    case 69:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_LSK6R)");
                        break;

                    case 70:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_DATA)");
                        break;

                    case 71:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_ARROW_DOWN)");
                        break;

                    case 72:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_ARROW_RIGHT)");
                        break;

                    default:
                        break;
                }
                return true;
            }
        }
    }
}