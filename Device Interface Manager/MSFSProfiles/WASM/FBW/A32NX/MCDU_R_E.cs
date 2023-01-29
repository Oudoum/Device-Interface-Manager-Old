using Device_Interface_Manager.MSFSProfiles.FBW.A32NX;
using Device_Interface_Manager.MVVM.View;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Device_Interface_Manager.MSFSProfiles.WASM.FBW.A32NX
{
    public class MCDU_R_E : ENETWASM
    {
        private bool _mCDU_R_ANNUNC_FAIL;
        private bool MCDU_R_ANNUNC_FAIL
        {
            set
            {
                if (this._mCDU_R_ANNUNC_FAIL != value)
                {
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(2, this._mCDU_R_ANNUNC_FAIL = value);
                }
            }
        }

        private bool _mCDU_R_ANNUNC_FMGC;
        private bool MCDU_R_ANNUNC_FMGC
        {
            set
            {
                if (this._mCDU_R_ANNUNC_FMGC != value)
                {
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(3, this._mCDU_R_ANNUNC_FMGC = value);
                }
            }
        }

        private bool _mCDU_R_ANNUNC_FM;
        private bool MCDU_R_ANNUNC_FM
        {
            set
            {
                if (this._mCDU_R_ANNUNC_FM != value)
                {
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(4, this._mCDU_R_ANNUNC_FM = value);
                }
            }
        }

        private bool _mCDU_R_ANNUNC_MCDU_MENU;
        private bool MCDU_R_ANNUNC_MCDU_MENU
        {
            set
            {
                if (this._mCDU_R_ANNUNC_MCDU_MENU != value)
                {
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(5, this._mCDU_R_ANNUNC_MCDU_MENU = value);
                }
            }
        }

        private bool _mCDU_R_ANNUNC_FM1;
        private bool MCDU_R_ANNUNC_FM1
        {
            set
            {
                if (this._mCDU_R_ANNUNC_FM1 != value)
                {
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(6, this._mCDU_R_ANNUNC_FM1 = value);
                }
            }
        }

        private bool _mCDU_R_ANNUNC_IND;
        private bool MCDU_R_ANNUNC_IND
        {
            set
            {
                if (this._mCDU_R_ANNUNC_IND != value)
                {
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(7, this._mCDU_R_ANNUNC_IND = value);
                }
            }
        }

        private bool _mCDU_R_ANNUNC_RDY;
        private bool MCDU_R_ANNUNC_RDY
        {
            set
            {
                if (this._mCDU_R_ANNUNC_RDY != value)
                {
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(8, this._mCDU_R_ANNUNC_RDY = value);
                }
            }
        }

        private bool _mCDU_R_ANNUNC_FM2;
        private bool MCDU_R_ANNUNC_FM2
        {
            set
            {
                if (this._mCDU_R_ANNUNC_FM2 != value)
                {
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(10, this._mCDU_R_ANNUNC_FM2 = value);
                }
            }
        }

        private bool _mCDU_R_LIGHTTEST;
        private bool MCDU_R_LIGHTTEST
        {
            set
            {
                if (this._mCDU_R_LIGHTTEST != value)
                {
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(2, this._mCDU_R_LIGHTTEST = value);
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(3, this._mCDU_R_LIGHTTEST);
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(4, this._mCDU_R_LIGHTTEST);
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(5, this._mCDU_R_LIGHTTEST);
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(6, this._mCDU_R_LIGHTTEST);
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(7, this._mCDU_R_LIGHTTEST);
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(8, this._mCDU_R_LIGHTTEST);
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(10, this._mCDU_R_LIGHTTEST);
                }
            }
        }

        private readonly FBWA32NXMCDU fBWA32NXMCDU = new();

        public MCDU_R_E()
        {
            this.fBWA32NXMCDU.EditormodeOff += FBWA32NXMCDU_EditormodeOff; ;
            this.fBWA32NXMCDU.Closing += FBWA32NXMCDU_Closing; ;
            this.fBWA32NXMCDU.Dispatcher.BeginInvoke(delegate ()
            {
                this.GetPMDG737CDUSettings();
                this.fBWA32NXMCDU.Show();
                this.fBWA32NXMCDU.WindowState = (System.Windows.WindowState)this.fBW_A32NX_MCDU_Screen.Fullscreen;
            });
        }

        public override void Stop()
        {
            base.Stop();
            this.fBWA32NXMCDU?.Close();
        }

        private void FBWA32NXMCDU_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.SaveScreenProperties();
        }

        private void FBWA32NXMCDU_EditormodeOff(object sender, EventArgs e)
        {
            this.SaveScreenProperties();
        }

        private const string settings = @"Profiles\FBW A32NX\ENET MCDU Screen R.json";
        FBW_A32NX_MCDU_Screen fBW_A32NX_MCDU_Screen = new();
        private void GetPMDG737CDUSettings()
        {
            if (File.Exists(settings))
            {
                this.fBW_A32NX_MCDU_Screen = JsonConvert.DeserializeObject<FBW_A32NX_MCDU_Screen>(File.ReadAllText(settings));
            }
            this.fBW_A32NX_MCDU_Screen.Load(this.fBWA32NXMCDU);
        }

        private void SaveScreenProperties()
        {
            this.fBW_A32NX_MCDU_Screen.Save(this.fBWA32NXMCDU);
            string json = JsonConvert.SerializeObject(fBW_A32NX_MCDU_Screen, Formatting.Indented);
            Directory.CreateDirectory(settings.Remove(18));
            if (File.Exists(settings))
            {
                if (File.ReadAllText(settings) != json)
                {
                    File.WriteAllText(settings, json);
                }
                return;
            }
            File.WriteAllText(settings, json);
        }

        protected override void GetSimVar()
        {
            if (this.MobiFlightSimConnect.GetSimVar("(L:A32NX_OVHD_INTLT_ANN)") == 0 &&
                this.MobiFlightSimConnect.GetSimVar("(L:A32NX_ELEC_AC_ESS_SHED_BUS_IS_POWERED)") == 1)
            {
                this.MCDU_R_LIGHTTEST = true;
                return;
            }

            this.MCDU_R_LIGHTTEST = false;
            this.MCDU_R_ANNUNC_FAIL = Convert.ToBoolean(this.MobiFlightSimConnect.GetSimVar("(L:A32NX_MCDU_R_ANNUNC_FAIL)"));
            this.MCDU_R_ANNUNC_FMGC = Convert.ToBoolean(this.MobiFlightSimConnect.GetSimVar("(L:A32NX_MCDU_R_ANNUNC_FMGC)"));
            this.MCDU_R_ANNUNC_FM = Convert.ToBoolean(this.MobiFlightSimConnect.GetSimVar("(L:A32NX_MCDU_R_ANNUNC_FM)"));
            this.MCDU_R_ANNUNC_MCDU_MENU = Convert.ToBoolean(this.MobiFlightSimConnect.GetSimVar("(L:A32NX_MCDU_R_ANNUNC_MCDU_MENU)"));
            this.MCDU_R_ANNUNC_FM1 = Convert.ToBoolean(this.MobiFlightSimConnect.GetSimVar("(L:A32NX_MCDU_R_ANNUNC_FM1)"));
            this.MCDU_R_ANNUNC_IND = Convert.ToBoolean(this.MobiFlightSimConnect.GetSimVar("(L:A32NX_MCDU_R_ANNUNC_IND)"));
            this.MCDU_R_ANNUNC_RDY = Convert.ToBoolean(this.MobiFlightSimConnect.GetSimVar("(L:A32NX_MCDU_R_ANNUNC_RDY)"));
            this.MCDU_R_ANNUNC_FM2 = Convert.ToBoolean(this.MobiFlightSimConnect.GetSimVar("(L:A32NX_MCDU_R_ANNUNC_FM2)"));
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
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_DOT");
                    break;

                case 2:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_O");
                    break;

                case 3:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_PLUSMINUS");
                    break;

                case 4:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_Z");
                    break;

                case 5:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_DIV");
                    break;

                case 6:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_SP");
                    break;

                case 7:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_OVFY");
                    break;

                case 8:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_CLR");
                    break;

                case 9:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_7");
                    break;

                case 10:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_8");
                    break;

                case 11:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_9");
                    break;

                case 12:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_U");
                    break;

                case 13:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_V");
                    break;

                case 14:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_W");
                    break;

                case 15:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_X");
                    break;

                case 16:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_Y");
                    break;

                case 17:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_4");
                    break;

                case 18:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_5");
                    break;

                case 19:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_6");
                    break;

                case 20:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_P");
                    break;

                case 21:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_Q");
                    break;

                case 22:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_R");
                    break;

                case 23:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_S");
                    break;

                case 24:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_T");
                    break;

                case 25:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_1");
                    break;

                case 26:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_2");
                    break;

                case 27:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_3");
                    break;

                case 28:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_K");
                    break;

                case 29:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_L");
                    break;

                case 30:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_M");
                    break;

                case 31:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_N");
                    break;

                case 32:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_O");
                    break;

                case 33:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_PREVPAGE");
                    break;

                case 34:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_UP");
                    break;

                case 35:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_L1");
                    break;

                case 36:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_F");
                    break;

                case 37:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_G");
                    break;

                case 38:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_H");
                    break;

                case 39:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_I");
                    break;

                case 40:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_J");
                    break;

                case 41:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_AIRPORT");
                    break;

                case 42:

                    break;

                case 43:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_L2");
                    break;

                case 44:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_A");
                    break;

                case 45:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_B");
                    break;

                case 46:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_C");
                    break;

                case 47:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_D");
                    break;

                case 48:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_E");
                    break;

                case 49:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_FPLN");
                    break;

                case 50:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_RAD");
                    break;

                case 51:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_L3");
                    break;

                case 52:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_FUEL");
                    break;

                case 53:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_FPLN");
                    break;

                case 54:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_ATC");
                    break;

                case 55:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_MENU");
                    break;

                case 56:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_L5");
                    break;

                case 57:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_DIR");
                    break;

                case 58:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_PROG");
                    break;

                case 59:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_L4");
                    break;

                case 60:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_PERF");
                    break;

                case 61:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_INIT");
                    break;

                case 62:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_DATA");
                    break;

                case 63:
                    //BRIGHTNESS_DOWN
                    break;

                case 64:
                    //BRIGHTNESS_UP
                    break;

                case 65:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_R1");
                    break;

                case 66:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_R2");
                    break;

                case 67:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_R3");
                    break;

                case 68:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_R4");
                    break;

                case 69:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_R5");
                    break;

                case 70:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_R6");
                    break;

                case 71:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_L6");
                    break;

                case 72:

                    break;

                case 73:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_NEXTPAGE");
                    break;

                case 74:
                    this.MobiFlightSimConnect.SetEventID("A320_Neo_CDU_2_BTN_DOWN");
                    break;

                default:
                    break;
            }
        }
    }
}