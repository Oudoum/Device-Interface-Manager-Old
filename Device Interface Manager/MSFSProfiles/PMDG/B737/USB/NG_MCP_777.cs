using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.FlightSimulator.SimConnect;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceITAPI_Data;
using static Device_Interface_Manager.MSFSProfiles.PMDG.PMDG_NG3_SDK;

namespace Device_Interface_Manager.MSFSProfiles.PMDG.B737.USB;

public class NG_MCP_777 : MSFSProfiles.USB
{
    //MCP SETUP
    private CancellationTokenSource pmdg737MCPBlinkingCancellationTokenSource;
    private CancellationTokenSource pmdg737MCPLightCancellationTokenSource;

    //Method for MCP Lights
    private bool pmdg737NotPowered;
    private void PMDG737MCPLight(bool pmdg737MCPLightValue, CancellationToken token)
    {
        if (pmdg737MCPLightValue && !pmdg737NotPowered)
        {
            IAS = false;
            MACH = false;
            Thread.Sleep(1500);
        }
        BackgroundLED(pmdg737MCPLightValue);
        if (ELEC_BusPowered_15)
        {
            pmdg737NotPowered = false;
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
            interfaceIT_7Segment_Display(Device.Session, "888-888088888880", 1);
            Thread.Sleep(2000);
            if (token.IsCancellationRequested)
            {
                return;
            }
            interfaceIT_7Segment_Display(Device.Session, new string(' ', 16), 1);
            Thread.Sleep(1000);
        }
    }

    private void BackgroundLED(bool status)
    {
        interfaceIT_LED_Set(Device.Session, 129, status);
        interfaceIT_LED_Set(Device.Session, 130, status);
        interfaceIT_LED_Set(Device.Session, 132, status);
        interfaceIT_LED_Set(Device.Session, 133, status);
        interfaceIT_LED_Set(Device.Session, 134, status);
        interfaceIT_LED_Set(Device.Session, 135, status);
        interfaceIT_LED_Set(Device.Session, 136, status);
        interfaceIT_LED_Set(Device.Session, 137, status);
        interfaceIT_LED_Set(Device.Session, 138, status);
        interfaceIT_LED_Set(Device.Session, 139, status);
        interfaceIT_LED_Set(Device.Session, 140, status);
        interfaceIT_LED_Set(Device.Session, 141, status);
        interfaceIT_LED_Set(Device.Session, 177, status);
        interfaceIT_LED_Set(Device.Session, 178, status);
        interfaceIT_LED_Set(Device.Session, 179, status);
        interfaceIT_LED_Set(Device.Session, 180, status);
    }

    private bool _eLEC_BusPowered_2;
    private bool ELEC_BusPowered_2
    {
        get => _eLEC_BusPowered_2;
        set
        {
            if (_eLEC_BusPowered_2 != value)
            {
                _eLEC_BusPowered_2 = value;
                if (value && !ELEC_BusPowered_7)
                {
                    BackgroundLED(true);
                }
                else if (!value && MAIN_LightsSelector > 10)
                {
                    BackgroundLED(true);
                }
                else if (!value)
                {
                    BackgroundLED(false);
                }
            }
        }
    }

    private bool _eLEC_BusPowered_7;
    private bool ELEC_BusPowered_7
    {
        get => _eLEC_BusPowered_7;
        set
        {
            if (_eLEC_BusPowered_7 != value)
            {
                _eLEC_BusPowered_7 = value;
                if (value && LTS_MainPanelKnob_0 == 0)
                {
                    pmdg737MCPLightCancellationTokenSource?.Cancel();
                    BackgroundLED(false);
                }
                else if (!value && ELEC_BusPowered_2)
                {
                    Task.Run(() => PMDG737MCPLight(true, (pmdg737MCPLightCancellationTokenSource = new()).Token));
                }
            }
        }
    }

    private bool _eLEC_BusPowered_15;
    private bool ELEC_BusPowered_15
    {
        get => _eLEC_BusPowered_15;
        set
        {
            if (_eLEC_BusPowered_15 != value)
            {
                _eLEC_BusPowered_15 = value;
            }
        }
    }

    private byte _lTS_MainPanelKnob_0;
    private byte LTS_MainPanelKnob_0
    {
        get => _lTS_MainPanelKnob_0;
        set
        {
            if (_lTS_MainPanelKnob_0 != value)
            {
                _lTS_MainPanelKnob_0 = value;
                if (value > 10)
                {
                    BackgroundLED(true);
                    return;
                }
                BackgroundLED(false);
            }
        }
    }

    private bool _mCP_annunN1;
    private bool MCP_annunN1
    {
        set
        {
            if (_mCP_annunN1 != value)
            {
                interfaceIT_LED_Set(Device.Session, 193, _mCP_annunN1 = value);
            }
        }
    }

    private bool _mCP_annunSPEED;
    private bool MCP_annunSPEED
    {
        set
        {
            if (_mCP_annunSPEED != value)
            {
                interfaceIT_LED_Set(Device.Session, 146, _mCP_annunSPEED = value);
            }
        }
    }

    private bool _mCP_annunVNAV;
    private bool MCP_annunVNAV
    {
        set
        {
            if (_mCP_annunVNAV != value)
            {
                interfaceIT_LED_Set(Device.Session, 148, _mCP_annunVNAV = value);
            }
        }
    }

    private bool _mCP_annunLVL_CHG;
    private bool MCP_annunLVL_CHG
    {
        set
        {
            if (_mCP_annunLVL_CHG != value)
            {
                interfaceIT_LED_Set(Device.Session, 149, _mCP_annunLVL_CHG = value);
            }
        }
    }

    private bool _mCP_annunHDG_SEL;
    private bool MCP_annunHDG_SEL
    {
        set
        {
            if (_mCP_annunHDG_SEL != value)
            {
                interfaceIT_LED_Set(Device.Session, 150, _mCP_annunHDG_SEL = value);
            }
        }
    }

    private bool _mCP_annunLNAV;
    private bool MCP_annunLNAV
    {
        set
        {
            if (_mCP_annunLNAV != value)
            {
                interfaceIT_LED_Set(Device.Session, 147, _mCP_annunLNAV = value);
            }
        }
    }

    private bool _mCP_annunVOR_LOC;
    private bool MCP_annunVOR_LOC
    {
        set
        {
            if (_mCP_annunVOR_LOC != value)
            {
                interfaceIT_LED_Set(Device.Session, 153, _mCP_annunVOR_LOC = value);
            }
        }
    }

    private bool _mCP_annunAPP;
    private bool MCP_annunAPP
    {
        set
        {
            if (_mCP_annunAPP != value)
            {
                interfaceIT_LED_Set(Device.Session, 154, _mCP_annunAPP = value);
            }
        }
    }

    private bool _mCP_annunALT_HOLD;
    private bool MCP_annunALT_HOLD
    {
        set
        {
            if (_mCP_annunALT_HOLD != value)
            {
                interfaceIT_LED_Set(Device.Session, 152, _mCP_annunALT_HOLD = value);
            }
        }
    }

    private bool _mCP_annunVS;
    private bool MCP_annunVS
    {
        set
        {
            if (_mCP_annunVS != value)
            {
                interfaceIT_LED_Set(Device.Session, 151, _mCP_annunVS = value);
            }
        }
    }

    private bool _mCP_annunCMD_A;
    private bool MCP_annunCMD_A
    {
        set
        {
            if (_mCP_annunCMD_A != value)
            {
                interfaceIT_LED_Set(Device.Session, 145, _mCP_annunCMD_A = value);
            }
        }
    }

    private bool _mCP_annunCMD_B;
    private bool MCP_annunCMD_B
    {
        set
        {
            if (_mCP_annunCMD_B != value)
            {
                interfaceIT_LED_Set(Device.Session, 155, _mCP_annunCMD_B = value);
            }
        }
    }

    private bool _mCP_indication_powered;
    private bool MCP_indication_powered
    {
        get => _mCP_indication_powered;
        set
        {
            if (_mCP_indication_powered != value)
            {
                _mCP_indication_powered = value;
                if (value)
                {
                    return;
                }
                pmdg737MCPBlinkingCancellationTokenSource?.Cancel();
                _mCP_Altitude = null;
                _mCP_Heading = null;
                _mCP_IASMach = null;
                _mCP_VertSpeed = null;
                IAS = false;
                MACH = false;
                interfaceIT_7Segment_Display(Device.Session, new string(' ', 16), 1);
            }
        }
    }

    private byte? _mAIN_LightsSelector;
    private byte? MAIN_LightsSelector
    {
        get => _mAIN_LightsSelector;
        set
        {
            if (_mAIN_LightsSelector != value && value != 2)
            {
                _mAIN_LightsSelector = value;
                if (value == 1)
                {
                    pmdg737MCPBlinkingCancellationTokenSource?.Cancel();
                    interfaceIT_7Segment_Display(Device.Session, new string(' ', 16), 1);
                    _mCP_Altitude = null;
                    _mCP_Heading = null;
                    _mCP_IASMach = null;
                    _mCP_VertSpeed = null;
                    IAS = false;
                    MACH = false;
                }
                if (value == 0)
                {
                    interfaceIT_7Segment_Display(Device.Session, "888-888088888880", 1);
                    Task.Run(() => PMDG737MCPBlinking((pmdg737MCPBlinkingCancellationTokenSource = new()).Token));
                    IAS = true;
                    MACH = true;
                }
            }
        }
    }

    private float? _mCP_IASMach;
    private float? MCP_IASMach
    {
        set
        {
            if (_mCP_IASMach != value && MAIN_LightsSelector != 0)
            {
                _mCP_IASMach = value;
                if (value < 1)
                {
                    interfaceIT_7Segment_Display(Device.Session, string.Format("{0,3}", value?.ToString("#.00", System.Globalization.CultureInfo.InvariantCulture)).TrimStart('.'), 2);
                    interfaceIT_7Segment_Display(Device.Session, " ", 1);
                    interfaceIT_LED_Set(Device.Session, 8, true);
                    IAS = false;
                    MACH = true;
                }
                if (value >= 100)
                {
                    interfaceIT_7Segment_Display(Device.Session, string.Format("{0,3}", value?.ToString(System.Globalization.CultureInfo.InvariantCulture)), 1);
                    MACH = false;
                    IAS = true;
                }
            }
        }
    }

    private bool _iAS;
    private bool IAS
    {
        set
        {
            if (_iAS != value)
            {
                interfaceIT_LED_Set(Device.Session, 185, _iAS = value);
            }
        }
    }

    private bool _mACH;
    private bool MACH
    {
        set
        {
            if (_mACH != value)
            {
                interfaceIT_LED_Set(Device.Session, 186, _mACH = value);
            }
        }
    }

    private ushort? _mCP_Heading;
    private ushort? MCP_Heading
    {
        set
        {
            if (_mCP_Heading != value && MAIN_LightsSelector != 0)
            {
                interfaceIT_7Segment_Display(Device.Session, (_mCP_Heading = value)?.ToString("D3"), 9);
            }
        }
    }

    private bool _mCP_IASBlank;
    private bool MCP_IASBlank
    {
        set
        {
            if (_mCP_IASBlank != value)
            {
                _mCP_IASBlank = value;
                if (value)
                {
                    interfaceIT_7Segment_Display(Device.Session, "   ", 1);
                    IAS = false;
                    MACH = false;
                }
            }
        }
    }

    private ushort? _mCP_Altitude;
    private ushort? MCP_Altitude
    {
        set
        {
            if (_mCP_Altitude != value && MAIN_LightsSelector != 0)
            {
                _mCP_Altitude = value;
                if (value == 0)
                {
                    interfaceIT_7Segment_Display(Device.Session, "0000", 13);
                    return;
                }
                interfaceIT_7Segment_Display(Device.Session, string.Format("{0,5}", value.ToString()), 12);
            }
        }
    }

    private bool _mCP_VertSpeedBlank;
    private bool MCP_VertSpeedBlank
    {
        get => _mCP_VertSpeedBlank;
        set
        {
            if (_mCP_VertSpeedBlank != value)
            {
                _mCP_VertSpeedBlank = value;
                if (value)
                {
                    interfaceIT_7Segment_Display(Device.Session, "     ", 4);
                }
            }
        }
    }

    private short? _mCP_VertSpeed;
    private short? MCP_VertSpeed
    {
        set
        {
            if (_mCP_VertSpeed != value && !MCP_VertSpeedBlank && MAIN_LightsSelector != 0)
            {
                _mCP_VertSpeed = value;
                if (value == 0)
                {
                    interfaceIT_7Segment_Display(Device.Session, "     ", 4);
                }
                else if (value < 0)
                {
                    interfaceIT_7Segment_Display(Device.Session, "-" + string.Format("{0,4}", value?.ToString("D3").TrimStart('-')), 4);
                }
                else if (value > 0)
                {
                    interfaceIT_7Segment_Display(Device.Session, string.Format("{0,5}", value?.ToString("D3")), 4);
                }
            }
        }
    }

    public override void Stop()
    {
        base.Stop();
        pmdg737MCPBlinkingCancellationTokenSource?.Cancel();
        pmdg737MCPLightCancellationTokenSource?.Cancel();
    }

    protected override async Task StartSimConnectAsync()
    {
        await base.StartSimConnectAsync();
        PMDG737.RegisterPMDGDataEvents(simConnectClient.SimConnect);
    }

    protected override void SimConnect_OnRecvClientData(SimConnect sender, SIMCONNECT_RECV_CLIENT_DATA data)
    {
        if ((uint)DATA_REQUEST_ID.DATA_REQUEST == data.dwRequestID)
        {
            ELEC_BusPowered_2 = ((PMDG_NG3_Data)data.dwData[0]).ELEC_BusPowered[2];
            ELEC_BusPowered_7 = ((PMDG_NG3_Data)data.dwData[0]).ELEC_BusPowered[7];
            ELEC_BusPowered_15 = ((PMDG_NG3_Data)data.dwData[0]).ELEC_BusPowered[15];

            MCP_annunN1 = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunN1;
            MCP_annunSPEED = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunSPEED;
            MCP_annunVNAV = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunVNAV;
            MCP_annunLVL_CHG = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunLVL_CHG;
            MCP_annunHDG_SEL = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunHDG_SEL;
            MCP_annunLNAV = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunLNAV;
            MCP_annunVOR_LOC = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunVOR_LOC;
            MCP_annunAPP = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunAPP;
            MCP_annunALT_HOLD = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunALT_HOLD;
            MCP_annunVS = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunVS;
            MCP_annunCMD_A = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunCMD_A;
            MCP_annunCMD_B = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunCMD_B;

            LTS_MainPanelKnob_0 = ((PMDG_NG3_Data)data.dwData[0]).LTS_MainPanelKnob[0];

            MCP_indication_powered = ((PMDG_NG3_Data)data.dwData[0]).MCP_indication_powered;
            MCP_IASBlank = ((PMDG_NG3_Data)data.dwData[0]).MCP_IASBlank;
            MCP_VertSpeedBlank = ((PMDG_NG3_Data)data.dwData[0]).MCP_VertSpeedBlank;

            if (MCP_indication_powered)
            {
                MAIN_LightsSelector = ((PMDG_NG3_Data)data.dwData[0]).MAIN_LightsSelector;
                MCP_IASMach = ((PMDG_NG3_Data)data.dwData[0]).MCP_IASMach;
                if (hdg_rotary_push == 0)
                {
                    MCP_Heading = ((PMDG_NG3_Data)data.dwData[0]).MCP_Heading;
                    heading_Course0 = ((PMDG_NG3_Data)data.dwData[0]).MCP_Course[0];
                }
                if (hdg_rotary_push == 1)
                {
                    MCP_Heading = ((PMDG_NG3_Data)data.dwData[0]).MCP_Course[0];
                    heading_Course0 = ((PMDG_NG3_Data)data.dwData[0]).MCP_Heading;
                }
                if (alt_rotary_push == 0)
                {
                    MCP_Altitude = ((PMDG_NG3_Data)data.dwData[0]).MCP_Altitude;
                    altitude_Course1 = ((PMDG_NG3_Data)data.dwData[0]).MCP_Course[1];
                }
                if (alt_rotary_push == 1)
                {
                    MCP_Altitude = ((PMDG_NG3_Data)data.dwData[0]).MCP_Course[1];
                    altitude_Course1 = ((PMDG_NG3_Data)data.dwData[0]).MCP_Altitude;
                }
                MCP_VertSpeed = ((PMDG_NG3_Data)data.dwData[0]).MCP_VertSpeed;
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

    protected override void KeyPressedProc(uint session, int key, uint direction)
    {
        //1-0
        if (new int[] { 17, 18, 24, 69, 71, 81, 82 }.Contains(key))
        {
            if (direction == 1)
            {
                direction = 0;
            }

            else if (direction == 0)
            {
                direction = 1;
            }
        }

        //1-2
        else if (new int[] { 70, 72 }.Contains(key))
        {
            if (direction == 1)
            {
                direction = 2;
            }

            else if (direction == 0 && (eFIS_CPT_VOR_ADF_SELECTOR_L != 0 || eFIS_CPT_VOR_ADF_SELECTOR_R != 0))
            {
                direction = 1;
            }
        }

        //-3 & -14
        else if (new int[] { 20, 22, 23, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 83, 84, 85, 85, 87, 88, 89, 90, 91, 92, 93, 94, 95 }.Contains(key))
        {
            if (direction == 1)
            {
                direction = MOUSE_FLAG_LEFTSINGLE;
            }

            else if (direction == 0)
            {
                direction = MOUSE_FLAG_LEFTRELEASE;
            }
        }

        switch (key)
        {
            //Special
            case 39:
                speed_rotary_push = direction;
                break;

            case 63:
                hdg_rotary_push = direction;
                if (heading_Course0 is not null)
                {
                    MCP_Heading = heading_Course0;
                }
                break;

            case 64:
                alt_rotary_push = direction;
                if (altitude_Course1 is not null)
                {
                    MCP_Altitude = altitude_Course1;
                }
                break;

            //0-1
            case 21:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_DISENGAGE_BAR, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            //1-0
            case 17:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_FD_SWITCH_L, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 18:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_AT_ARM_SWITCH, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 24:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_FD_SWITCH_R, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 69:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_VOR_ADF_SELECTOR_L, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                eFIS_CPT_VOR_ADF_SELECTOR_L = direction;
                break;

            case 71:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_VOR_ADF_SELECTOR_R, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                eFIS_CPT_VOR_ADF_SELECTOR_R = direction;
                break;

            case 81:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS_RADIO_BARO, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 82:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO_IN_HPA, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            //1-2
            case 70:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_VOR_ADF_SELECTOR_L, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 72:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_VOR_ADF_SELECTOR_R, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            //0-...
            case 58 when direction == 1:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR, 0, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 59 when direction == 1:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR, 1, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 60 when direction == 1:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR, 2, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 61 when direction == 1:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR, 3, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 62 when direction == 1:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR, 4, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 65 when direction == 1:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MODE, 0, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 66 when direction == 1:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MODE, 1, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 67 when direction == 1:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MODE, 2, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 68 when direction == 1:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MODE, 3, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 73 when direction == 1:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 0, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 74 when direction == 1:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 1, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 75 when direction == 1:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 2, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 76 when direction == 1:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 3, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 77 when direction == 1:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 4, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 78 when direction == 1:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 5, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 79 when direction == 1:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 6, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            //-3 & -14
            case 20:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_CO_SWITCH, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 22:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_SPD_INTV_SWITCH, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 23:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_ALT_INTV_SWITCH, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 25:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_CMD_A_SWITCH, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 26:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_N1_SWITCH, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 27:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_SPEED_SWITCH, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 28:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_LNAV_SWITCH, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 29:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_VNAV_SWITCH, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 30:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_LVL_CHG_SWITCH, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 31:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_HDG_SEL_SWITCH, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 32:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_VS_SWITCH, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 33:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_ALT_HOLD_SWITCH, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 34:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_VOR_LOC_SWITCH, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 35:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_APP_SWITCH, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 83:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_FPV, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 84:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MTRS, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 85:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS_RST, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 86:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO_STD, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 87:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MODE_CTR, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 88:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE_TFC, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 89:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_WXR, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 90:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_STA, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 91:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_WPT, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 92:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_ARPT, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 93:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_DATA, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 94:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_POS, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 95:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_TERR, direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            //-18 & -17
            case 9 when direction == 1:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_VS_SELECTOR, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 10 when direction == 1:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_VS_SELECTOR, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 11 when direction == 1:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_ALTITUDE_SELECTOR, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 12 when direction == 1:
                simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_ALTITUDE_SELECTOR, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            //-18 & -17 SPECIAL
            case 5 when direction == 1:
                if (hdg_rotary_push == 0)
                {
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_SPEED_SELECTOR, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                }
                if (hdg_rotary_push == 1)
                {
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_COURSE_SELECTOR_L, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                }
                break;

            case 6 when direction == 1:
                if (hdg_rotary_push == 0)
                {
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_SPEED_SELECTOR, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                }
                if (hdg_rotary_push == 1)
                {
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_COURSE_SELECTOR_L, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                }
                break;

            case 7 when direction == 1:
                if (alt_rotary_push == 0)
                {
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_HEADING_SELECTOR, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                }
                if (alt_rotary_push == 1)
                {
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_COURSE_SELECTOR_R, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                }
                break;

            case 8 when direction == 1:
                if (alt_rotary_push == 0)
                {
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_HEADING_SELECTOR, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                }
                if (alt_rotary_push == 1)
                {
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_COURSE_SELECTOR_R, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                }
                break;

            //-18 & -17 (-1 & -3)
            case 1 when direction == 1:
                if (speed_rotary_push == 0)
                {
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                }
                if (speed_rotary_push == 1)
                {
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                }
                break;

            case 2 when direction == 1:
                if (speed_rotary_push == 0)
                {
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                }
                if (speed_rotary_push == 1)
                {
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                }
                break;

            case 3 when direction == 1:
                if (speed_rotary_push == 0)
                {
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                }
                if (speed_rotary_push == 1)
                {
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                }
                break;

            case 4 when direction == 1:
                if (speed_rotary_push == 0)
                {
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                }
                if (speed_rotary_push == 1)
                {
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.SimConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                }
                break;

            default:
                break;
        }
    }
}