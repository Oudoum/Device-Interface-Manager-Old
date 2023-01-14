using System;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceITAPI_Data;

namespace Device_Interface_Manager.MSFSProfiles.WASM.FENIX.A320
{
    public class MCDU_R_USB : USBWASM
    {
        private bool _cDU2_FAIL;
        private bool CDU2_FAIL
        {
            set
            {
                if (this._cDU2_FAIL != value)
                {
                    _ = interfaceIT_LED_Set(this.Device.Session, 2, this._cDU2_FAIL = value);
                }
            }
        }

        private bool _cDU2_MCDU_MENU;
        private bool CDU2_MCDU_MENU
        {
            set
            {
                if (this._cDU2_MCDU_MENU != value)
                {
                    _ = interfaceIT_LED_Set(this.Device.Session, 3, this._cDU2_MCDU_MENU = value);
                    _ = interfaceIT_LED_Set(this.Device.Session, 5, value);
                }
            }
        }

        private bool _cDU2_FM;
        private bool CDU2_FM
        {
            set
            {
                if (this._cDU2_FM != value)
                {
                    _ = interfaceIT_LED_Set(this.Device.Session, 4, this._cDU2_FM = value);
                }
            }
        }

        private bool _cDU2_FM1;
        private bool CDU2_FM1
        {
            set
            {
                if (this._cDU2_FM1 != value)
                {
                    _ = interfaceIT_LED_Set(this.Device.Session, 9, this._cDU2_FM1 = value);
                }
            }
        }

        private bool _cDU2_IND;
        private bool CDU2_IND
        {
            set
            {
                if (this._cDU2_IND != value)
                {
                    _ = interfaceIT_LED_Set(this.Device.Session, 10, this._cDU2_IND = value);
                }
            }
        }

        private bool _cDU2_RDY;
        private bool CDU2_RDY
        {
            set
            {
                if (this._cDU2_RDY != value)
                {
                    _ = interfaceIT_LED_Set(this.Device.Session, 11, this._cDU2_RDY = value);
                }
            }
        }

        private bool _cDU2_DASH;
        private bool CDU2_DASH
        {
            set
            {
                if (this._cDU2_DASH != value)
                {
                    _ = interfaceIT_LED_Set(this.Device.Session, 12, this._cDU2_DASH = value);
                }
            }
        }

        private bool _cDU2_FM2;
        private bool CDU2_FM2
        {
            set
            {
                if (this._cDU2_FM2 != value)
                {
                    _ = interfaceIT_LED_Set(this.Device.Session, 13, this._cDU2_FM2 = value);
                }
            }
        }

        private float _pED_LIGHTING_PEDESTAL;
        private float PED_LIGHTING_PEDESTAL
        {
            set
            {
                if (this._pED_LIGHTING_PEDESTAL != value)
                {
                    _ = interfaceIT_Brightness_Set(this.Device.Session, (int)((this._pED_LIGHTING_PEDESTAL = value) * 50 * 2.55));
                }
            }
        }

        protected override void GetSimVar()
        {
            this.CDU2_FAIL = Convert.ToBoolean(this.MobiFlightSimConnect.GetSimVar("(L:I_CDU2_FAIL)"));
            this.CDU2_MCDU_MENU = Convert.ToBoolean(this.MobiFlightSimConnect.GetSimVar("(L:I_CDU2_MCDU_MENU)"));
            this.CDU2_FM = Convert.ToBoolean(this.MobiFlightSimConnect.GetSimVar("(L:I_CDU2_FM)"));
            this.CDU2_FM1 = Convert.ToBoolean(this.MobiFlightSimConnect.GetSimVar("(L:I_CDU2_FM1)"));
            this.CDU2_IND = Convert.ToBoolean(this.MobiFlightSimConnect.GetSimVar("(L:I_CDU2_IND)"));
            this.CDU2_RDY = Convert.ToBoolean(this.MobiFlightSimConnect.GetSimVar("(L:I_CDU2_RDY)"));
            this.CDU2_DASH = Convert.ToBoolean(this.MobiFlightSimConnect.GetSimVar("(L:I_CDU2_DASH)"));
            this.CDU2_FM2 = Convert.ToBoolean(this.MobiFlightSimConnect.GetSimVar("(L:I_CDU2_FM2)"));

            this.PED_LIGHTING_PEDESTAL = this.MobiFlightSimConnect.GetSimVar("(L:N_PED_LIGHTING_PEDESTAL)");
        }

        protected override bool KeyPressedProc(int session, int key, int direction)
        {
            switch (key)
            {
                case 1:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_MENU)");
                    break;

                case 2:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_ATC_COM)");
                    break;

                case 3:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_SEC_FPLN)");
                    break;

                case 4:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_INIT)");
                    break;

                case 5:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_FUEL_PRED)");
                    break;

                case 6:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_RAD_NAV)");
                    break;

                case 7:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_FPLN)");
                    break;

                case 8:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_PERF)");
                    break;

                case 9:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_E)");
                    break;

                case 10:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_D)");
                    break;

                case 11:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_C)");
                    break;

                case 12:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_B)");
                    break;

                case 13:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_A)");
                    break;

                case 14:

                    break;

                case 15:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_AIRPORT)");
                    break;

                case 16:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_PROG)");
                    break;

                case 17:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_J)");
                    break;

                case 18:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_I)");
                    break;

                case 19:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_H)");
                    break;

                case 20:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_G)");
                    break;

                case 21:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_F)");
                    break;

                case 22:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_ARROW_UP)");
                    break;

                case 23:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_ARROW_LEFT)");
                    break;

                case 24:

                    break;

                case 25:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_O)");
                    break;

                case 26:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_N)");
                    break;

                case 27:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_M)");
                    break;

                case 28:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_L)");
                    break;

                case 29:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_K)");
                    break;

                case 30:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_3)");
                    break;

                case 31:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_2)");
                    break;

                case 32:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_1)");
                    break;

                case 33:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_T)");
                    break;

                case 34:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_S)");
                    break;

                case 35:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_R)");
                    break;

                case 36:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_Q)");
                    break;

                case 37:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_P)");
                    break;

                case 38:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_6)");
                    break;

                case 39:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_5)");
                    break;

                case 40:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_4)");
                    break;

                case 41:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_Y)");
                    break;

                case 42:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_X)");
                    break;

                case 43:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_W)");
                    break;

                case 44:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_V)");
                    break;

                case 45:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_U)");
                    break;

                case 46:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_9)");
                    break;

                case 47:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_8)");
                    break;

                case 48:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_7)");
                    break;

                case 49:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_CLEAR)");
                    break;

                case 50:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_OVFLY)");
                    break;

                case 51:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_SPACE)");
                    break;

                case 52:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_SLASH)");
                    break;

                case 53:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_Z)");
                    break;

                case 54:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_MINUS)");
                    break;

                case 55:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_0)");
                    break;

                case 56:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_DOT)");
                    break;

                case 57:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_LSK1L)");
                    break;

                case 58:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_LSK2L)");
                    break;

                case 59:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_LSK3L)");
                    break;

                case 60:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_LSK4L)");
                    break;

                case 61:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_LSK5L)");
                    break;

                case 62:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_LSK6L)");
                    break;

                case 63:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_DIR)");
                    break;

                case 64:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_LSK1R)");
                    break;

                case 65:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_LSK2R)");
                    break;

                case 66:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_LSK3R)");
                    break;

                case 67:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_LSK4R)");
                    break;

                case 68:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_LSK5R)");
                    break;

                case 69:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_LSK6R)");
                    break;

                case 70:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_DATA)");
                    break;

                case 71:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_ARROW_DOWN)");
                    break;

                case 72:
                    this.MobiFlightSimConnect.SetSimVar(direction + " (>L:S_CDU2_KEY_ARROW_RIGHT)");
                    break;

                default:
                    break;
            }
            return true;
        }
    }
}