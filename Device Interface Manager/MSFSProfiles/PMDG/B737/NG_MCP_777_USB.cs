using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.FlightSimulator.SimConnect;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceITAPI_Data;
using static Device_Interface_Manager.MSFSProfiles.PMDG.PMDG_NG3_SDK;

namespace Device_Interface_Manager.MSFSProfiles.PMDG.B737
{
    public class NG_MCP_777_USB : USBPMDG
    {
        //MCP SETUP
        private CancellationTokenSource pmdg737MCPBlinkingCancellationTokenSource;
        private CancellationTokenSource pmdg737MCPLightCancellationTokenSource;

        //Method for MCP Lights
        private bool pmdg737NotPowered;
        private void PMDG737MCPLight(bool pmdg737MCPLightValue, CancellationToken token)
        {
            if (pmdg737MCPLightValue && !this.pmdg737NotPowered)
            {
                this.IAS = false;
                this.MACH = false;
                Thread.Sleep(1500);
            }
            this.BackgroundLED(pmdg737MCPLightValue);
            if (this.ELEC_BusPowered_15)
            {
                this.pmdg737NotPowered = false;
            }
            if (token.IsCancellationRequested)
            {
                return;
            }
        }

        //Thread Method for MCP blinking
        private void PMDG737MCPBlinking(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                _ = interfaceIT_7Segment_Display(this.Device.Session, "888-888088888880", 1);
                Thread.Sleep(2000);
                if (token.IsCancellationRequested)
                {
                    return;
                }
                _ = interfaceIT_7Segment_Display(this.Device.Session, new string(' ', 16), 1);
                Thread.Sleep(1000);
            }
        }

        private void BackgroundLED(bool status)
        {
            _ = interfaceIT_LED_Set(this.Device.Session, 129, status);
            _ = interfaceIT_LED_Set(this.Device.Session, 130, status);
            _ = interfaceIT_LED_Set(this.Device.Session, 132, status);
            _ = interfaceIT_LED_Set(this.Device.Session, 133, status);
            _ = interfaceIT_LED_Set(this.Device.Session, 134, status);
            _ = interfaceIT_LED_Set(this.Device.Session, 135, status);
            _ = interfaceIT_LED_Set(this.Device.Session, 136, status);
            _ = interfaceIT_LED_Set(this.Device.Session, 137, status);
            _ = interfaceIT_LED_Set(this.Device.Session, 138, status);
            _ = interfaceIT_LED_Set(this.Device.Session, 139, status);
            _ = interfaceIT_LED_Set(this.Device.Session, 140, status);
            _ = interfaceIT_LED_Set(this.Device.Session, 141, status);
            _ = interfaceIT_LED_Set(this.Device.Session, 177, status);
            _ = interfaceIT_LED_Set(this.Device.Session, 178, status);
            _ = interfaceIT_LED_Set(this.Device.Session, 179, status);
            _ = interfaceIT_LED_Set(this.Device.Session, 180, status);
        }

        private bool _eLEC_BusPowered_2;
        private bool ELEC_BusPowered_2
        {
            get => this._eLEC_BusPowered_2;
            set
            {
                if (this._eLEC_BusPowered_2 != value)
                {
                    this._eLEC_BusPowered_2 = value;
                    if (value && !this.ELEC_BusPowered_7)
                    {
                        this.BackgroundLED(true);
                    }
                    else if (!value && this.MAIN_LightsSelector > 10)
                    {
                        this.BackgroundLED(true);
                    }
                    else if (!value)
                    {
                        this.BackgroundLED(false);
                    }
                }
            }
        }

        private bool _eLEC_BusPowered_7;
        private bool ELEC_BusPowered_7
        {
            get => this._eLEC_BusPowered_7;
            set
            {
                if (this._eLEC_BusPowered_7 != value)
                {
                    this._eLEC_BusPowered_7 = value;
                    if (value && this.LTS_MainPanelKnob_0 == 0)
                    {
                        this.pmdg737MCPLightCancellationTokenSource?.Cancel();
                        this.BackgroundLED(false);
                    }
                    else if (!value && this.ELEC_BusPowered_2)
                    {
                        Task.Run(() => this.PMDG737MCPLight(true, (this.pmdg737MCPLightCancellationTokenSource = new()).Token));
                    }
                }
            }
        }

        private bool _eLEC_BusPowered_15;
        private bool ELEC_BusPowered_15
        {
            get => this._eLEC_BusPowered_15;
            set
            {
                if (this._eLEC_BusPowered_15 != value)
                {
                    this._eLEC_BusPowered_15 = value;
                }
            }
        }

        private byte _lTS_MainPanelKnob_0;
        private byte LTS_MainPanelKnob_0
        {
            get => this._lTS_MainPanelKnob_0;
            set
            {
                if (this._lTS_MainPanelKnob_0 != value)
                {
                    this._lTS_MainPanelKnob_0 = value;
                    if (value > 10)
                    {
                        this.BackgroundLED(true);
                        return;
                    }
                    this.BackgroundLED(false);
                }
            }
        }

        private bool _mCP_annunN1;
        private bool MCP_annunN1
        {
            set
            {
                if (this._mCP_annunN1 != value)
                {
                    _ = interfaceIT_LED_Set(this.Device.Session, 193, this._mCP_annunN1 = value);
                }
            }
        }

        private bool _mCP_annunSPEED;
        private bool MCP_annunSPEED
        {
            set
            {
                if (this._mCP_annunSPEED != value)
                {
                    _ = interfaceIT_LED_Set(this.Device.Session, 146, this._mCP_annunSPEED = value);
                }
            }
        }

        private bool _mCP_annunVNAV;
        private bool MCP_annunVNAV
        {
            set
            {
                if (this._mCP_annunVNAV != value)
                {
                    _ = interfaceIT_LED_Set(this.Device.Session, 148, this._mCP_annunVNAV = value);
                }
            }
        }

        private bool _mCP_annunLVL_CHG;
        private bool MCP_annunLVL_CHG
        {
            set
            {
                if (this._mCP_annunLVL_CHG != value)
                {
                    _ = interfaceIT_LED_Set(this.Device.Session, 149, this._mCP_annunLVL_CHG = value);
                }
            }
        }

        private bool _mCP_annunHDG_SEL;
        private bool MCP_annunHDG_SEL
        {
            set
            {
                if (this._mCP_annunHDG_SEL != value)
                {
                    _ = interfaceIT_LED_Set(this.Device.Session, 150, this._mCP_annunHDG_SEL = value);
                }
            }
        }

        private bool _mCP_annunLNAV;
        private bool MCP_annunLNAV
        {
            set
            {
                if (this._mCP_annunLNAV != value)
                {
                    _ = interfaceIT_LED_Set(this.Device.Session, 147, this._mCP_annunLNAV = value);
                }
            }
        }

        private bool _mCP_annunVOR_LOC;
        private bool MCP_annunVOR_LOC
        {
            set
            {
                if (this._mCP_annunVOR_LOC != value)
                {
                    _ = interfaceIT_LED_Set(this.Device.Session, 153, this._mCP_annunVOR_LOC = value);
                }
            }
        }

        private bool _mCP_annunAPP;
        private bool MCP_annunAPP
        {
            set
            {
                if (this._mCP_annunAPP != value)
                {
                    _ = interfaceIT_LED_Set(this.Device.Session, 154, this._mCP_annunAPP = value);
                }
            }
        }

        private bool _mCP_annunALT_HOLD;
        private bool MCP_annunALT_HOLD
        {
            set
            {
                if (this._mCP_annunALT_HOLD != value)
                {
                    _ = interfaceIT_LED_Set(this.Device.Session, 152, this._mCP_annunALT_HOLD = value);
                }
            }
        }

        private bool _mCP_annunVS;
        private bool MCP_annunVS
        {
            set
            {
                if (this._mCP_annunVS != value)
                {
                    _ = interfaceIT_LED_Set(this.Device.Session, 151, this._mCP_annunVS = value);
                }
            }
        }

        private bool _mCP_annunCMD_A;
        private bool MCP_annunCMD_A
        {
            set
            {
                if (this._mCP_annunCMD_A != value)
                {
                    _ = interfaceIT_LED_Set(this.Device.Session, 145, this._mCP_annunCMD_A = value);
                }
            }
        }

        private bool _mCP_annunCMD_B;
        private bool MCP_annunCMD_B
        {
            set
            {
                if (this._mCP_annunCMD_B != value)
                {
                    _ = interfaceIT_LED_Set(this.Device.Session, 155, this._mCP_annunCMD_B = value);
                }
            }
        }

        private bool _mCP_indication_powered;
        private bool MCP_indication_powered
        {
            get => this._mCP_indication_powered;
            set
            {
                if (this._mCP_indication_powered != value)
                {
                    this._mCP_indication_powered = value;
                    if (value)
                    {
                        return;
                    }
                    this.pmdg737MCPBlinkingCancellationTokenSource?.Cancel();
                    this._mCP_Altitude = null;
                    this._mCP_Heading = null;
                    this._mCP_IASMach = null;
                    this._mCP_VertSpeed = null;
                    this.IAS = false;
                    this.MACH = false;
                    _ = interfaceIT_7Segment_Display(this.Device.Session, new string(' ', 16), 1);
                }
            }
        }

        private byte? _mAIN_LightsSelector;
        private byte? MAIN_LightsSelector
        {
            get => this._mAIN_LightsSelector;
            set
            {
                if (this._mAIN_LightsSelector != value && value != 2)
                {
                    this._mAIN_LightsSelector = value;
                    if (value == 1)
                    {
                        this.pmdg737MCPBlinkingCancellationTokenSource?.Cancel();
                        _ = interfaceIT_7Segment_Display(this.Device.Session, new string(' ', 16), 1);
                        this._mCP_Altitude = null;
                        this._mCP_Heading = null;
                        this._mCP_IASMach = null;
                        this._mCP_VertSpeed = null;
                        this.IAS = false;
                        this.MACH = false;
                    }
                    if (value == 0)
                    {
                        _ = interfaceIT_7Segment_Display(this.Device.Session, "888-888088888880", 1);
                        Task.Run(() => this.PMDG737MCPBlinking((this.pmdg737MCPBlinkingCancellationTokenSource = new()).Token));
                        this.IAS = true;
                        this.MACH = true;
                    }
                }
            }
        }

        private float? _mCP_IASMach;
        private float? MCP_IASMach
        {
            set
            {
                if (this._mCP_IASMach != value && this.MAIN_LightsSelector != 0)
                {
                    this._mCP_IASMach = value;
                    if (value < 1)
                    {
                        _ = interfaceIT_7Segment_Display(this.Device.Session, string.Format("{0,3}", value?.ToString("#.00", System.Globalization.CultureInfo.InvariantCulture)).TrimStart('.'), 2);
                        _ = interfaceIT_7Segment_Display(this.Device.Session, " ", 1);
                        _ = interfaceIT_LED_Set(this.Device.Session, 8, true);
                        this.IAS = false;
                        this.MACH = true;
                    }
                    if (value >= 100)
                    {
                        _ = interfaceIT_7Segment_Display(this.Device.Session, string.Format("{0,3}", value?.ToString(System.Globalization.CultureInfo.InvariantCulture)), 1);
                        this.MACH = false;
                        this.IAS = true;
                    }
                }
            }
        }

        private bool _iAS;
        private bool IAS
        {
            set
            {
                if (this._iAS != value)
                {
                    _ = interfaceIT_LED_Set(this.Device.Session, 185, this._iAS = value);
                }
            }
        }

        private bool _mACH;
        private bool MACH
        {
            set
            {
                if (this._mACH != value)
                {
                    _ = interfaceIT_LED_Set(this.Device.Session, 186, this._mACH = value);
                }
            }
        }

        private ushort? _mCP_Heading;
        private ushort? MCP_Heading
        {
            set
            {
                if (this._mCP_Heading != value && this.MAIN_LightsSelector != 0)
                {
                    _ = interfaceIT_7Segment_Display(this.Device.Session, (this._mCP_Heading = value)?.ToString("D3"), 9);
                }
            }
        }

        private bool _mCP_IASBlank;
        private bool MCP_IASBlank
        {
            set
            {
                if (this._mCP_IASBlank != value)
                {
                    this._mCP_IASBlank = value;
                    if (value)
                    {
                        _ = interfaceIT_7Segment_Display(this.Device.Session, "   ", 1);
                        this.IAS = false;
                        this.MACH = false;
                    }
                }
            }
        }

        private ushort? _mCP_Altitude;
        private ushort? MCP_Altitude
        {
            set
            {
                if (this._mCP_Altitude != value && this.MAIN_LightsSelector != 0)
                {
                    this._mCP_Altitude = value;
                    if (value == 0)
                    {
                        _ = interfaceIT_7Segment_Display(this.Device.Session, "00000", 12);
                        return;
                    }
                    _ = interfaceIT_7Segment_Display(this.Device.Session, string.Format("{0,5}", value.ToString()), 12);
                }
            }
        }

        private bool _mCP_VertSpeedBlank;
        private bool MCP_VertSpeedBlank
        {
            get => this._mCP_VertSpeedBlank;
            set
            {
                if (this._mCP_VertSpeedBlank != value)
                {
                    this._mCP_VertSpeedBlank = value;
                    if (value)
                    {
                        _ = interfaceIT_7Segment_Display(this.Device.Session, "     ", 4);
                    }
                }
            }
        }

        private short? _mCP_VertSpeed;
        private short? MCP_VertSpeed
        {
            set
            {
                if (this._mCP_VertSpeed != value && !this.MCP_VertSpeedBlank && this.MAIN_LightsSelector != 0)
                {
                    this._mCP_VertSpeed = value;
                    if (value == 0)
                    {
                        _ = interfaceIT_7Segment_Display(this.Device.Session, "     ", 4);
                    }
                    else if (value < 0)
                    {
                        _ = interfaceIT_7Segment_Display(this.Device.Session, "-" + string.Format("{0,4}", value?.ToString("D3").TrimStart('-')), 4);
                    }
                    else if (value > 0)
                    {
                        _ = interfaceIT_7Segment_Display(this.Device.Session, string.Format("{0,5}", value?.ToString("D3")), 4);
                    }
                }
            }
        }

        public override void Stop()
        {
            base.Stop();
            this.pmdg737MCPBlinkingCancellationTokenSource?.Cancel();
            this.pmdg737MCPLightCancellationTokenSource?.Cancel();
        }

        protected override void PMDGSimConnectStart()
        {
            base.PMDGSimConnectStart();
            PMDG737.RegisterPMDGDataEvents(this.PMDGSimConnectClient.Simconnect);
        }

        protected override void Simconnect_OnRecvClientData(SimConnect sender, SIMCONNECT_RECV_CLIENT_DATA data)
        {
            if (((uint)DATA_REQUEST_ID.DATA_REQUEST) == data.dwRequestID)
            {
                this.ELEC_BusPowered_2 = ((PMDG_NG3_Data)data.dwData[0]).ELEC_BusPowered[2];
                this.ELEC_BusPowered_7 = ((PMDG_NG3_Data)data.dwData[0]).ELEC_BusPowered[7];
                this.ELEC_BusPowered_15 = ((PMDG_NG3_Data)data.dwData[0]).ELEC_BusPowered[15];

                this.MCP_annunN1 = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunN1;
                this.MCP_annunSPEED = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunSPEED;
                this.MCP_annunVNAV = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunVNAV;
                this.MCP_annunLVL_CHG = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunLVL_CHG;
                this.MCP_annunHDG_SEL = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunHDG_SEL;
                this.MCP_annunLNAV = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunLNAV;
                this.MCP_annunVOR_LOC = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunVOR_LOC;
                this.MCP_annunAPP = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunAPP;
                this.MCP_annunALT_HOLD = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunALT_HOLD;
                this.MCP_annunVS = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunVS;
                this.MCP_annunCMD_A = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunCMD_A;
                this.MCP_annunCMD_B = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunCMD_B;

                this.LTS_MainPanelKnob_0 = ((PMDG_NG3_Data)data.dwData[0]).LTS_MainPanelKnob[0];

                this.MCP_indication_powered = ((PMDG_NG3_Data)data.dwData[0]).MCP_indication_powered;
                this.MCP_IASBlank = ((PMDG_NG3_Data)data.dwData[0]).MCP_IASBlank;
                this.MCP_VertSpeedBlank = ((PMDG_NG3_Data)data.dwData[0]).MCP_VertSpeedBlank;

                if (this.MCP_indication_powered)
                {
                    this.MAIN_LightsSelector = ((PMDG_NG3_Data)data.dwData[0]).MAIN_LightsSelector;
                    this.MCP_IASMach = ((PMDG_NG3_Data)data.dwData[0]).MCP_IASMach;
                    if (this.hdg_rotary_push == 0)
                    {
                        this.MCP_Heading = ((PMDG_NG3_Data)data.dwData[0]).MCP_Heading;
                        this.heading_Course0 = ((PMDG_NG3_Data)data.dwData[0]).MCP_Course[0];
                    }
                    if (this.hdg_rotary_push == 1)
                    {
                        this.MCP_Heading = ((PMDG_NG3_Data)data.dwData[0]).MCP_Course[0];
                        this.heading_Course0 = ((PMDG_NG3_Data)data.dwData[0]).MCP_Heading;
                    }
                    if (this.alt_rotary_push == 0)
                    {
                        this.MCP_Altitude = ((PMDG_NG3_Data)data.dwData[0]).MCP_Altitude;
                        this.altitude_Course1 = ((PMDG_NG3_Data)data.dwData[0]).MCP_Course[1];
                    }
                    if (this.alt_rotary_push == 1)
                    {
                        this.MCP_Altitude = ((PMDG_NG3_Data)data.dwData[0]).MCP_Course[1];
                        this.altitude_Course1 = ((PMDG_NG3_Data)data.dwData[0]).MCP_Altitude;
                    }
                    this.MCP_VertSpeed = ((PMDG_NG3_Data)data.dwData[0]).MCP_VertSpeed;
                }
            }
        }

        private uint eFIS_CPT_VOR_ADF_SELECTOR_R;
        private uint eFIS_CPT_VOR_ADF_SELECTOR_L;
        private uint speed_rotary_push;
        private uint hdg_rotary_push;
        private uint alt_rotary_push;
        private ushort? heading_Course0;
        private ushort? altitude_Course1;

        protected override bool KeyPressedProc(int session, int key, int direction)
        {
            uint ndirection = (uint)direction;

            //1-0
            if (new int[] { 17, 18, 24, 69, 71, 81, 82 }.Contains(key))
            {
                if (direction == 1)
                {
                    ndirection = 0;
                }

                if (direction == 0)
                {
                    ndirection = 1;
                }
            }

            //1-2
            if (new int[] { 70, 72 }.Contains(key))
            {
                if (direction == 1)
                {
                    ndirection = 2;
                }

                if (direction == 0 && (this.eFIS_CPT_VOR_ADF_SELECTOR_L != 0 || this.eFIS_CPT_VOR_ADF_SELECTOR_R != 0))
                {
                    ndirection = 1;
                }
            }

            //-3 & -14
            if (new int[] { 20, 22, 23, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 83, 84, 85, 85, 87, 88, 89, 90, 91, 92, 93, 94, 95 }.Contains(key))
            {
                if (direction == 1)
                {
                    ndirection = MOUSE_FLAG_LEFTSINGLE;
                }

                if (direction == 0)
                {
                    ndirection = MOUSE_FLAG_LEFTRELEASE;
                }
            }

            switch (key)
            {
                //Special
                case 39:
                    this.speed_rotary_push = ndirection;
                    break;

                case 63:
                    this.hdg_rotary_push = ndirection;
                    if (this.heading_Course0 is not null)
                    {
                        this.MCP_Heading = this.heading_Course0;
                    }
                    break;

                case 64:
                    this.alt_rotary_push = ndirection;
                    if (this.altitude_Course1 is not null)
                    {
                        this.MCP_Altitude = this.altitude_Course1;
                    }
                    break;

                //0-1
                case 21:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_DISENGAGE_BAR, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                //1-0
                case 17:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_FD_SWITCH_L, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 18:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_AT_ARM_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 24:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_FD_SWITCH_R, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 69:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_VOR_ADF_SELECTOR_L, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    this.eFIS_CPT_VOR_ADF_SELECTOR_L = ndirection;
                    break;

                case 71:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_VOR_ADF_SELECTOR_R, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    this.eFIS_CPT_VOR_ADF_SELECTOR_R = ndirection;
                    break;

                case 81:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS_RADIO_BARO, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 82:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO_IN_HPA, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                //1-2
                case 70:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_VOR_ADF_SELECTOR_L, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 72:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_VOR_ADF_SELECTOR_R, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                //0-...
                case 58 when direction == 1:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR, 0, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 59 when direction == 1:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR, 1, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 60 when direction == 1:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR, 2, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 61 when direction == 1:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR, 3, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 62 when direction == 1:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR, 4, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 65 when direction == 1:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MODE, 0, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 66 when direction == 1:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MODE, 1, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 67 when direction == 1:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MODE, 2, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 68 when direction == 1:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MODE, 3, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 73 when direction == 1:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 0, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 74 when direction == 1:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 1, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 75 when direction == 1:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 2, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 76 when direction == 1:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 3, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 77 when direction == 1:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 4, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 78 when direction == 1:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 5, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 79 when direction == 1:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 6, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                //-3 & -14
                case 20:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_CO_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 22:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_SPD_INTV_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 23:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_ALT_INTV_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 25:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_CMD_A_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 26:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_N1_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 27:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_SPEED_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 28:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_LNAV_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 29:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_VNAV_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 30:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_LVL_CHG_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 31:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_HDG_SEL_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 32:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_VS_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 33:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_ALT_HOLD_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 34:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_VOR_LOC_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 35:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_APP_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 83:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_FPV, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 84:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MTRS, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 85:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS_RST, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 86:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO_STD, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 87:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MODE_CTR, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 88:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE_TFC, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 89:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_WXR, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 90:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_STA, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 91:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_WPT, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 92:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_ARPT, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 93:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_DATA, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 94:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_POS, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 95:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_TERR, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                //-18 & -17
                case 9 when direction == 1:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_VS_SELECTOR, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 10 when direction == 1:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_VS_SELECTOR, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 11 when direction == 1:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_ALTITUDE_SELECTOR, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 12 when direction == 1:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_ALTITUDE_SELECTOR, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                //-18 & -17 SPECIAL
                case 5 when direction == 1:
                    if (hdg_rotary_push == 0)
                    {
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_SPEED_SELECTOR, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    }
                    if (hdg_rotary_push == 1)
                    {
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_COURSE_SELECTOR_L, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    }
                    break;

                case 6 when direction == 1:
                    if (hdg_rotary_push == 0)
                    {
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_SPEED_SELECTOR, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    }
                    if (hdg_rotary_push == 1)
                    {
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_COURSE_SELECTOR_L, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    }
                    break;

                case 7 when direction == 1:
                    if (alt_rotary_push == 0)
                    {
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_HEADING_SELECTOR, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    }
                    if (alt_rotary_push == 1)
                    {
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_COURSE_SELECTOR_R, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    }
                    break;

                case 8 when direction == 1:
                    if (alt_rotary_push == 0)
                    {
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_HEADING_SELECTOR, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    }
                    if (alt_rotary_push == 1)
                    {
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_COURSE_SELECTOR_R, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    }
                    break;

                //-18 & -17 (-1 & -3)
                case 1 when direction == 1:
                    if (this.speed_rotary_push == 0)
                    {
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    }
                    if (this.speed_rotary_push == 1)
                    {
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    }
                    break;

                case 2 when direction == 1:
                    if (this.speed_rotary_push == 0)
                    {
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    }
                    if (this.speed_rotary_push == 1)
                    {
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    }
                    break;

                case 3 when direction == 1:
                    if (this.speed_rotary_push == 0)
                    {
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    }
                    if (this.speed_rotary_push == 1)
                    {
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    }
                    break;

                case 4 when direction == 1:
                    if (this.speed_rotary_push == 0)
                    {
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    }
                    if (this.speed_rotary_push == 1)
                    {
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    }
                    break;

                default:
                    break;
            }
            return true;
        }
    }
}