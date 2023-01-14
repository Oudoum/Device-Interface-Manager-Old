﻿using System;

namespace Device_Interface_Manager.MSFSProfiles.WASM.FBW.A32NX
{
    public class MCDU_L_E : ENETWASM
    {
        private bool _mCDU_L_ANNUNC_FAIL;
        private bool MCDU_L_ANNUNC_FAIL
        {
            set
            {
                if (this._mCDU_L_ANNUNC_FAIL != value)
                {
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(2, this._mCDU_L_ANNUNC_FAIL = value);
                }
            }
        }

        private bool _mCDU_L_ANNUNC_FMGC;
        private bool MCDU_L_ANNUNC_FMGC
        {
            set
            {
                if (this._mCDU_L_ANNUNC_FMGC != value)
                {
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(3, this._mCDU_L_ANNUNC_FMGC = value);
                }
            }
        }

        private bool _mCDU_L_ANNUNC_FM;
        private bool MCDU_L_ANNUNC_FM
        {
            set
            {
                if (this._mCDU_L_ANNUNC_FM != value)
                {
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(4, this._mCDU_L_ANNUNC_FM = value);
                }
            }
        }

        private bool _mCDU_L_ANNUNC_MCDU_MENU;
        private bool MCDU_L_ANNUNC_MCDU_MENU
        {
            set
            {
                if (this._mCDU_L_ANNUNC_MCDU_MENU != value)
                {
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(5, this._mCDU_L_ANNUNC_MCDU_MENU = value);
                }
            }
        }

        private bool _mCDU_L_ANNUNC_FM1;
        private bool MCDU_L_ANNUNC_FM1
        {
            set
            {
                if (this._mCDU_L_ANNUNC_FM1 != value)
                {
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(6, this._mCDU_L_ANNUNC_FM1 = value);
                }
            }
        }

        private bool _mCDU_L_ANNUNC_IND;
        private bool MCDU_L_ANNUNC_IND
        {
            set
            {
                if (this._mCDU_L_ANNUNC_IND != value)
                {
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(7, this._mCDU_L_ANNUNC_IND = value);
                }
            }
        }

        private bool _mCDU_L_ANNUNC_RDY;
        private bool MCDU_L_ANNUNC_RDY
        {
            set
            {
                if (this._mCDU_L_ANNUNC_RDY != value)
                {
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(8, this._mCDU_L_ANNUNC_RDY = value);
                }
            }
        }

        private bool _mCDU_L_ANNUNC_FM2;
        private bool MCDU_L_ANNUNC_FM2
        {
            set
            {
                if (this._mCDU_L_ANNUNC_FM2 != value)
                {
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(10, this._mCDU_L_ANNUNC_FM2 = value);
                }
            }
        }

        private bool _mCDU_L_LIGHTTEST;
        private bool MCDU_L_LIGHTTEST
        {
            set
            {
                if (this._mCDU_L_LIGHTTEST != value)
                {
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(2, this._mCDU_L_LIGHTTEST = value);
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(3, this._mCDU_L_LIGHTTEST);
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(4, this._mCDU_L_LIGHTTEST);
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(5, this._mCDU_L_LIGHTTEST);
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(6, this._mCDU_L_LIGHTTEST);
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(7, this._mCDU_L_LIGHTTEST);
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(8, this._mCDU_L_LIGHTTEST);
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(10, this._mCDU_L_LIGHTTEST);
                }
            }
        }

        protected override void GetSimVar()
        {
            if (this.MobiFlightSimConnect.GetSimVar("(L:A32NX_OVHD_INTLT_ANN)") == 0 &&
                this.MobiFlightSimConnect.GetSimVar("(L:A32NX_ELEC_AC_ESS_SHED_BUS_IS_POWERED)") == 1)
            {
                this.MCDU_L_LIGHTTEST = true;
                return;
            }

            this.MCDU_L_LIGHTTEST = false;
            this.MCDU_L_ANNUNC_FAIL = Convert.ToBoolean(this.MobiFlightSimConnect.GetSimVar("(L:A32NX_MCDU_L_ANNUNC_FAIL)"));
            this.MCDU_L_ANNUNC_FMGC = Convert.ToBoolean(this.MobiFlightSimConnect.GetSimVar("(L:A32NX_MCDU_L_ANNUNC_FMGC)"));
            this.MCDU_L_ANNUNC_FM = Convert.ToBoolean(this.MobiFlightSimConnect.GetSimVar("(L:A32NX_MCDU_L_ANNUNC_FM)"));
            this.MCDU_L_ANNUNC_MCDU_MENU = Convert.ToBoolean(this.MobiFlightSimConnect.GetSimVar("(L:A32NX_MCDU_L_ANNUNC_MCDU_MENU)"));
            this.MCDU_L_ANNUNC_FM1 = Convert.ToBoolean(this.MobiFlightSimConnect.GetSimVar("(L:A32NX_MCDU_L_ANNUNC_FM1)"));
            this.MCDU_L_ANNUNC_IND = Convert.ToBoolean(this.MobiFlightSimConnect.GetSimVar("(L:A32NX_MCDU_L_ANNUNC_IND)"));
            this.MCDU_L_ANNUNC_RDY = Convert.ToBoolean(this.MobiFlightSimConnect.GetSimVar("(L:A32NX_MCDU_L_ANNUNC_RDY)"));
            this.MCDU_L_ANNUNC_FM2 = Convert.ToBoolean(this.MobiFlightSimConnect.GetSimVar("(L:A32NX_MCDU_L_ANNUNC_FM2)"));
        }

        protected override void KeyPressedProcEthernet(int key, string direction)
        {
            if (direction == "OFF")
            {
                return;
            }

            switch (key)
            {
                case 1:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_DOT");
                    break;

                case 2:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_O");
                    break;

                case 3:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_PLUSMINUS");
                    break;

                case 4:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_Z");
                    break;

                case 5:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_DIV");
                    break;

                case 6:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_SP");
                    break;

                case 7:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_OVFY");
                    break;

                case 8:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_CLR");
                    break;

                case 9:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_7");
                    break;

                case 10:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_8");
                    break;

                case 11:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_9");
                    break;

                case 12:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_U");
                    break;

                case 13:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_V");
                    break;

                case 14:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_W");
                    break;

                case 15:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_X");
                    break;

                case 16:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_Y");
                    break;

                case 17:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_4");
                    break;

                case 18:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_5");
                    break;

                case 19:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_6");
                    break;

                case 20:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_P");
                    break;

                case 21:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_Q");
                    break;

                case 22:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_R");
                    break;

                case 23:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_S");
                    break;

                case 24:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_T");
                    break;

                case 25:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_1");
                    break;

                case 26:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_2");
                    break;

                case 27:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_3");
                    break;

                case 28:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_K");
                    break;

                case 29:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_L");
                    break;

                case 30:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_M");
                    break;

                case 31:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_N");
                    break;

                case 32:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_O");
                    break;

                case 33:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_PREVPAGE");
                    break;

                case 34:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_UP");
                    break;

                case 35:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_L1");
                    break;

                case 36:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_F");
                    break;

                case 37:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_G");
                    break;

                case 38:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_H");
                    break;

                case 39:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_I");
                    break;

                case 40:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_J");
                    break;

                case 41:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_AIRPORT");
                    break;

                case 42:

                    break;

                case 43:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_L2");
                    break;

                case 44:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_A");
                    break;

                case 45:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_B");
                    break;

                case 46:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_C");
                    break;

                case 47:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_D");
                    break;

                case 48:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_E");
                    break;

                case 49:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_FPLN");
                    break;

                case 50:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_RAD");
                    break;

                case 51:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_L3");
                    break;

                case 52:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_FUEL");
                    break;

                case 53:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_FPLN");
                    break;

                case 54:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_ATC");
                    break;

                case 55:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_MENU");
                    break;

                case 56:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_L5");
                    break;

                case 57:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_DIR");
                    break;

                case 58:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_PROG");
                    break;

                case 59:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_L4");
                    break;

                case 60:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_PERF");
                    break;

                case 61:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_INIT");
                    break;

                case 62:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_DATA");
                    break;

                case 63:
                    //BRIGHTNESS_DOWN
                    break;

                case 64:
                    //BRIGHTNESS_UP
                    break;

                case 65:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_R1");
                    break;

                case 66:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_R2");
                    break;

                case 67:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_R3");
                    break;

                case 68:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_R4");
                    break;

                case 69:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_R5");
                    break;

                case 70:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_R6");
                    break;

                case 71:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_L6");
                    break;

                case 72:

                    break;

                case 73:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_NEXTPAGE");
                    break;

                case 74:
                    this.MobiFlightSimConnect.SetSimVar("A320_Neo_CDU_1_BTN_DOWN");
                    break;

                default:
                    break;
            }
        }
    }
}