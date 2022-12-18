using CommunityToolkit.Mvvm.ComponentModel;

namespace Device_Interface_Manager.Profiles.FENIX.A320
{
    [ObservableObject]
    partial class MSFS_FENIX_A320_MCDU_En : ENETWASM
    {
        [ObservableProperty]
        int _cDU1_FAIL;
        [ObservableProperty]
        int _cDU1_MCDU_MENU;
        [ObservableProperty]
        int _cDU1_FM;
        [ObservableProperty]
        int _cDU1_FM1;
        [ObservableProperty]
        int _cDU1_IND;
        [ObservableProperty]
        int _cDU1_RDY;
        [ObservableProperty]
        int _cDU1_DASH;
        [ObservableProperty]
        int _cDU1_FM2;

        protected override void GetSimVar()
        {
            this.CDU1_FAIL = (int)this.MobiFlightSimConnect.GetSimVar("(L:I_CDU1_FAIL)");
            this.CDU1_MCDU_MENU = (int)this.MobiFlightSimConnect.GetSimVar("(L:I_CDU1_MCDU_MENU)");
            this.CDU1_FM = (int)this.MobiFlightSimConnect.GetSimVar("(L:I_CDU1_FM)");
            this.CDU1_FM1 = (int)this.MobiFlightSimConnect.GetSimVar("(L:I_CDU1_FM1)");
            this.CDU1_IND = (int)this.MobiFlightSimConnect.GetSimVar("(L:I_CDU1_IND)");
            this.CDU1_RDY = (int)this.MobiFlightSimConnect.GetSimVar("(L:I_CDU1_RDY)");
            this.CDU1_DASH = (int)this.MobiFlightSimConnect.GetSimVar("(L:I_CDU1_DASH)");
            this.CDU1_FM2 = (int)this.MobiFlightSimConnect.GetSimVar("(L:I_CDU1_FM2)");
        }

        partial void OnCDU1_FAILChanged(int value)
        {
            this.InterfaceITEthernet.SendintefaceITEthernetLED(2, value);
        }

        partial void OnCDU1_MCDU_MENUChanged(int value)
        {
            this.InterfaceITEthernet.SendintefaceITEthernetLED(3, value);
            this.InterfaceITEthernet.SendintefaceITEthernetLED(5, value);
        }

        partial void OnCDU1_FMChanged(int value)
        {
            this.InterfaceITEthernet.SendintefaceITEthernetLED(4, value);
        }

        partial void OnCDU1_FM1Changed(int value)
        {
            this.InterfaceITEthernet.SendintefaceITEthernetLED(6, value);
        }

        partial void OnCDU1_INDChanged(int value)
        {
            this.InterfaceITEthernet.SendintefaceITEthernetLED(7, value);
        }

        partial void OnCDU1_RDYChanged(int value)
        {
            this.InterfaceITEthernet.SendintefaceITEthernetLED(8, value);
        }

        partial void OnCDU1_DASHChanged(int value)
        {
            this.InterfaceITEthernet.SendintefaceITEthernetLED(9, value);
        }

        partial void OnCDU1_FM2Changed(int value)
        {
            this.InterfaceITEthernet.SendintefaceITEthernetLED(10, value);
        }

        protected override void KeyPressedProcEthernet(int key, string direction)
        {
            if (direction == "ON")
            {
                direction = "1";
            }

            else if (direction == "OFF")
            {
                direction = "0";
            }

            switch (key)
            {
                case 1:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_DOT)");
                    break;

                case 2:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_0)");
                    break;

                case 3:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_MINUS)");
                    break;

                case 4:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_Z)");
                    break;

                case 5:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_SLASH)");
                    break;

                case 6:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_SPACE)");
                    break;

                case 7:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_OVFLY)");
                    break;

                case 8:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_CLEAR)");
                    break;

                case 9:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_7)");
                    break;

                case 10:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_8)");
                    break;

                case 11:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_9)");
                    break;

                case 12:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_U)");
                    break;

                case 13:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_V)");
                    break;

                case 14:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_W)");
                    break;

                case 15:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_X)");
                    break;

                case 16:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_Y)");
                    break;

                case 17:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_4)");
                    break;

                case 18:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_5)");
                    break;

                case 19:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_6)");
                    break;

                case 20:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_P)");
                    break;

                case 21:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_Q)");
                    break;

                case 22:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_R)");
                    break;

                case 23:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_S)");
                    break;

                case 24:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_T)");
                    break;

                case 25:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_1)");
                    break;

                case 26:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_2)");
                    break;

                case 27:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_3)");
                    break;

                case 28:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_K)");
                    break;

                case 29:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_L)");
                    break;

                case 30:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_M)");
                    break;

                case 31:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_N)");
                    break;

                case 32:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_O)");
                    break;

                case 33:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_ARROW_LEFT)");
                    break;

                case 34:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_ARROW_UP)");
                    break;

                case 35:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_LSK1L)");
                    break;

                case 36:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_F)");
                    break;

                case 37:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_G)");
                    break;

                case 38:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_H)");
                    break;

                case 39:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_I)");
                    break;

                case 40:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_J)");
                    break;

                case 41:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_AIRPORT)");
                    break;

                case 42:

                    break;

                case 43:    
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_LSK2L)");
                    break;

                case 44:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_A)");
                    break;

                case 45:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_B)");
                    break;

                case 46:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_C)");
                    break;

                case 47:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_D)");
                    break;

                case 48:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_E)");
                    break;

                case 49:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_FPLN)");
                    break;

                case 50:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_RAD_NAV)");
                    break;

                case 51:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_LSK3L)");
                    break;

                case 52:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_FUEL_PRED)");
                    break;

                case 53:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_SEC_FPLN)");
                    break;

                case 54:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_ATC_COM)");
                    break;

                case 55:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_MENU)");
                    break;

                case 56:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_LSK5L)");
                    break;

                case 57:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_DIR)");
                    break;

                case 58:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_PROG)");
                    break;

                case 59:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_LSK4L)");
                    break;

                case 60:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_PERF)");
                    break;

                case 61:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_INIT)");
                    break;

                case 62:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_DATA)");
                    break;

                case 63:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_BRIGHTNESS_DOWN)");
                    break;

                case 64:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_BRIGHTNESS_UP)");
                    break;

                case 65:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_LSK1R)");
                    break;

                case 66:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_LSK2R)");
                    break;

                case 67:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_LSK3R)");
                    break;

                case 68:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_LSK4R)");
                    break;

                case 69:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_LSK5R)");
                    break;

                case 70:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_LSK6R)");
                    break;

                case 71:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_LSK6L)");
                    break;

                case 72:

                    break;

                case 73:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_ARROW_RIGHT)");
                    break;

                case 74:
                    MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU1_KEY_ARROW_DOWN)");
                    break;

                default:
                    break;
            }
        }
    }
}