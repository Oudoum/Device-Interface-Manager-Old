using Device_Interface_Manager.interfaceIT.ENET;
using Device_Interface_Manager.MVVM.Model;
using Device_Interface_Manager.MVVM.View;
using System.Threading;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceITAPI_Data;

namespace Device_Interface_Manager.Profiles.FENIX.A320
{
    public class MSFS_FENIX_A320_MCDU_E
    {
        public class MSFS_FENIX_A320_Captain_MCDU_Data
        {
            public Thread ReceivedDataThread { get; set; }

            public void ReceiveDataThread(CancellationToken token)
            {
                I_CDU1_FAIL = 9999;
                I_CDU1_MCDU_MENU = 9999;
                I_CDU1_FM = 9999;
                I_CDU1_FM1 = 9999;
                I_CDU1_IND = 9999;
                I_CDU1_RDY = 9999;
                I_CDU1_DASH = 9999;
                I_CDU1_FM2 = 9999;

                while (true)
                {
                    HomeModel.simConnectCache.ReceiveSimConnectMessage();
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }
                    Thread.Sleep(10);
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }
                    GetSimVar();
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }
                    Thread.Sleep(10);
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }
                }
            }

            //Get SimVars
            int I_CDU1_FAIL;
            int I_CDU1_MCDU_MENU;
            int I_CDU1_FM;
            int I_CDU1_FM1;
            int I_CDU1_IND;
            int I_CDU1_RDY;
            int I_CDU1_DASH;
            int I_CDU1_FM2;
            private void GetSimVar()
            {
                // Output LEDs

                if (I_CDU1_FAIL != (I_CDU1_FAIL = (int)HomeModel.simConnectCache.GetSimVar("(L:I_CDU1_FAIL)")))
                {
                    ProfileCreator.Test.SendintefaceITEthernetLED(2, I_CDU1_FAIL);
                }

                if (I_CDU1_MCDU_MENU != (I_CDU1_MCDU_MENU = (int)HomeModel.simConnectCache.GetSimVar("(L:I_CDU1_MCDU_MENU)")))
                {
                    ProfileCreator.Test.SendintefaceITEthernetLED(3, I_CDU1_MCDU_MENU);
                    ProfileCreator.Test.SendintefaceITEthernetLED(5, I_CDU1_MCDU_MENU);
                }

                if (I_CDU1_FM != (I_CDU1_FM = (int)HomeModel.simConnectCache.GetSimVar("(L:I_CDU1_FM)")))
                {
                    ProfileCreator.Test.SendintefaceITEthernetLED(4, I_CDU1_FM);
                }

                if (I_CDU1_FM1 != (I_CDU1_FM1 = (int)HomeModel.simConnectCache.GetSimVar("(L:I_CDU1_FM1)")))
                {
                    ProfileCreator.Test.SendintefaceITEthernetLED(6, I_CDU1_FM1);
                }

                if (I_CDU1_IND != (I_CDU1_IND = (int)HomeModel.simConnectCache.GetSimVar("(L:I_CDU1_IND)")))
                {
                    ProfileCreator.Test.SendintefaceITEthernetLED(7, I_CDU1_IND);
                }

                if (I_CDU1_RDY != (I_CDU1_RDY = (int)HomeModel.simConnectCache.GetSimVar("(L:I_CDU1_RDY)")))
                {
                    ProfileCreator.Test.SendintefaceITEthernetLED(8, I_CDU1_RDY);
                }

                if (I_CDU1_DASH != (I_CDU1_DASH = (int)HomeModel.simConnectCache.GetSimVar("(L:I_CDU1_DASH)")))
                {
                    ProfileCreator.Test.SendintefaceITEthernetLED(9, I_CDU1_DASH);
                }

                if (I_CDU1_FM2 != (I_CDU1_FM2 = (int)HomeModel.simConnectCache.GetSimVar("(L:I_CDU1_FM2)")))
                {
                    ProfileCreator.Test.SendintefaceITEthernetLED(10, I_CDU1_FM2);
                }
            }
        }


        public class MSFS_FENIX_A320_Captain_Events
        {
            public Thread ReceivedDataThread { get; set; }

            public InterfaceITEthernet.INTERFACEIT_ETHERNET_KEY_NOTIFY_PROC EthernetKeyNotifyCallback { get; set; } = new(KeyPressedProcEthernet);

            private static void KeyPressedProcEthernet(int Switch, string Direction)
            {

                if (Direction == "ON")
                {
                    Direction = "1";
                }

                if (Direction == "OFF")
                {
                    Direction= "0";
                }

                switch (Switch)
                {
                    case 1:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_DOT)");
                        break;

                    case 2:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_0)");
                        break;

                    case 3:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_MINUS)");
                        break;

                    case 4:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_Z)");
                        break;

                    case 5:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_SLASH)");
                        break;

                    case 6:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_SPACE)");
                        break;

                    case 7:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_OVFLY)");
                        break;

                    case 8:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_CLEAR)");
                        break;

                    case 9:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_7)");
                        break;

                    case 10:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_8)");
                        break;

                    case 11:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_9)");
                        break;

                    case 12:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_U)");
                        break;

                    case 13:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_V)");
                        break;

                    case 14:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_W)");
                        break;

                    case 15:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_X)");
                        break;

                    case 16:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_Y)");
                        break;

                    case 17:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_4)");
                        break;

                    case 18:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_5)");
                        break;

                    case 19:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_6)");
                        break;

                    case 20:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_P)");
                        break;

                    case 21:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_Q)");
                        break;

                    case 22:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_R)");
                        break;

                    case 23:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_S)");
                        break;

                    case 24:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_T)");
                        break;

                    case 25:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_1)");
                        break;

                    case 26:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_2)");
                        break;

                    case 27:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_3)");
                        break;

                    case 28:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_K)");
                        break;

                    case 29:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_L)");
                        break;

                    case 30:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_M)");
                        break;

                    case 31:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_N)");
                        break;

                    case 32:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_O)");
                        break;

                    case 33:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_ARROW_LEFT)");
                        break;

                    case 34:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_ARROW_UP)");
                        break;

                    case 35:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_LSK1L)");
                        break;

                    case 36:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_F)");
                        break;

                    case 37:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_G)");
                        break;

                    case 38:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_H)");
                        break;

                    case 39:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_I)");
                        break;

                    case 40:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_J)");
                        break;

                    case 41:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_AIRPORT)");
                        break;

                    case 42:

                        break;

                    case 43:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_LSK2L)");
                        break;

                    case 44:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_A)");
                        break;

                    case 45:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_B)");
                        break;

                    case 46:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_C)");
                        break;

                    case 47:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_D)");
                        break;

                    case 48:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_E)");
                        break;

                    case 49:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_FPLN)");
                        break;

                    case 50:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_RAD_NAV)");
                        break;

                    case 51:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_LSK3L)");
                        break;

                    case 52:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_FUEL_PRED)");
                        break;

                    case 53:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_SEC_FPLN)");
                        break;

                    case 54:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_ATC_COM)");
                        break;

                    case 55:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_MENU)");
                        break;

                    case 56:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_LSK5L)");
                        break;

                    case 57:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_DIR)");
                        break;

                    case 58:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_PROG)");
                        break;

                    case 59:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_LSK4L)");
                        break;

                    case 60:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_PERF)");
                        break;

                    case 61:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_INIT)");
                        break;

                    case 62:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_DATA)");
                        break;

                    case 63:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_BRIGHTNESS_DOWN)");
                        break;

                    case 64:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_BRIGHTNESS_UP)");
                        break;

                    case 65:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_LSK1R)");
                        break;

                    case 66:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_LSK2R)");
                        break;

                    case 67:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_LSK3R)");
                        break;

                    case 68:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_LSK4R)");
                        break;

                    case 69:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_LSK5R)");
                        break;

                    case 70:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_LSK6R)");
                        break;

                    case 71:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_LSK6L)");
                        break;

                    case 72:

                        break;

                    case 73:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_ARROW_RIGHT)");
                        break;

                    case 74:
                        HomeModel.simConnectCache.SetSimVar(Direction + " (>L:S_CDU1_KEY_ARROW_DOWN)");
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
