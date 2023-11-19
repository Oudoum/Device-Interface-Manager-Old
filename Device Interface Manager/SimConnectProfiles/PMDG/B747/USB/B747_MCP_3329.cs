using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Device_Interface_Manager.Devices.interfaceIT.USB.InterfaceITAPI_Data;
using static Device_Interface_Manager.SimConnectProfiles.PMDG.PMDG_747QOTSII_SDK;

namespace Device_Interface_Manager.SimConnectProfiles.PMDG.B747.USB;
public class B747_MCP_3329 : SimConnectProfiles.USB
{
    //MCP SETUP
    private CancellationTokenSource pmdg737MCPLightCancellationTokenSource;

    //Task for MCP lights delay
    private async Task PMDG737MCPLightAsync(bool pmdg737MCPLightValue, CancellationToken token)
    {
        if (pmdg737MCPLightValue)
        {
            await Task.Delay(1000, token);
            IAS = false;
            MACH = false;
        }
        BackgroundLED(pmdg737MCPLightValue);
    }

    public string MCPSevenSegmentTest { get; init; } = ".888888888888888";
    public int MCPSevenSegmentStartPos { get; init; } = 1;
    public int MCPMachSevenSegmentDot { get; init; } = 48;


    public static event EventHandler<bool> OnBackgroundLEDChanged;
    public int[] BackgroundLEDs { get; init; } = new int[] { 209, 210, 211, 212, 213, 214, 215, 217, 218, 219, 220, 221, 222, 223, 233, 234, 235, 236, 237, 238, 239, 240 };
    private bool backgroundLEDStatus;
    private void BackgroundLED(bool status)
    {
        foreach (int ledNumber in BackgroundLEDs)
        {
            interfaceIT_LED_Set(Device.Session, ledNumber, status);
        }
        OnBackgroundLEDChanged?.Invoke(this, status);
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
                }
                else if (value <= 10 && !backgroundLEDStatus)
                {
                    BackgroundLED(false);
                }
            }
        }
    }

    public int MCP_ATArm_Sw_OnPos { get; init; } = 5;
    private bool _mCP_ATArm_Sw_On;
    private bool MCP_ATArm_Sw_On
    {
        set
        {
            if (_mCP_ATArm_Sw_On != value)
            {
                interfaceIT_Dataline_Set(Device.Session, MCP_ATArm_Sw_OnPos, _mCP_ATArm_Sw_On = value);
            }
        }
    }

    public int MCP_annunTHRPos { get; init; } = 193;
    private bool _mCP_annunTHR;
    private bool MCP_annunTHR
    {
        set
        {
            if (_mCP_annunTHR != value)
            {
                interfaceIT_LED_Set(Device.Session, MCP_annunTHRPos, _mCP_annunTHR = value);
            }
        }
    }

    public int MCP_annunSPDPos { get; init; } = 194;
    private bool _mCP_annunSPD;
    private bool MCP_annunSPD
    {
        set
        {
            if (_mCP_annunSPD != value)
            {
                interfaceIT_LED_Set(Device.Session, MCP_annunSPDPos, _mCP_annunSPD = value);
            }
        }
    }

    public int MCP_annunVNAVPos { get; init; } = 195;
    private bool _mCP_annunVNAV;
    private bool MCP_annunVNAV
    {
        set
        {
            if (_mCP_annunVNAV != value)
            {
                interfaceIT_LED_Set(Device.Session, MCP_annunVNAVPos, _mCP_annunVNAV = value);
            }
        }
    }

    public int MCP_annunFLCHPos { get; init; } = 196;
    private bool _mCP_annunFLCH;
    private bool MCP_annunFLCH
    {
        set
        {
            if (_mCP_annunFLCH != value)
            {
                interfaceIT_LED_Set(Device.Session, MCP_annunFLCHPos, _mCP_annunFLCH = value);
            }
        }
    }

    public int MCP_annunHDG_HOLDPos { get; init; } = 197;
    private bool _mCP_annunHDG_HOLD;
    private bool MCP_annunHDG_HOLD
    {
        set
        {
            if (_mCP_annunHDG_HOLD != value)
            {
                interfaceIT_LED_Set(Device.Session, MCP_annunHDG_HOLDPos, _mCP_annunHDG_HOLD = value);
            }
        }
    }

    public int MCP_annunLNAVPos { get; init; } = 198;
    private bool _mCP_annunLNAV;
    private bool MCP_annunLNAV
    {
        set
        {
            if (_mCP_annunLNAV != value)
            {
                interfaceIT_LED_Set(Device.Session, MCP_annunLNAVPos, _mCP_annunLNAV = value);
            }
        }
    }

    public int MCP_annunLOCPos { get; init; } = 199;
    private bool _mCP_annunLOC;
    private bool MCP_annunLOC
    {
        set
        {
            if (_mCP_annunLOC != value)
            {
                interfaceIT_LED_Set(Device.Session, MCP_annunLOCPos, _mCP_annunLOC = value);
            }
        }
    }

    public int MCP_annunAPPPos { get; init; } = 201;
    private bool _mCP_annunAPP;
    private bool MCP_annunAPP
    {
        set
        {
            if (_mCP_annunAPP != value)
            {
                interfaceIT_LED_Set(Device.Session, MCP_annunAPPPos, _mCP_annunAPP = value);
            }
        }
    }

    public int MCP_annunALT_HOLDPos { get; init; } = 202;
    private bool _mCP_annunALT_HOLD;
    private bool MCP_annunALT_HOLD
    {
        set
        {
            if (_mCP_annunALT_HOLD != value)
            {
                interfaceIT_LED_Set(Device.Session, MCP_annunALT_HOLDPos, _mCP_annunALT_HOLD = value);
            }
        }
    }

    public int MCP_annunVSPos { get; init; } = 203;
    private bool _mCP_annunVS;
    private bool MCP_annunVS
    {
        set
        {
            if (_mCP_annunVS != value)
            {
                interfaceIT_LED_Set(Device.Session, MCP_annunVSPos, _mCP_annunVS = value);
            }
        }
    }

    public int MCP_annunCMD_LPos { get; init; } = 204;
    private bool _mCP_annunCMD_L;
    private bool MCP_annunCMD_L
    {
        set
        {
            if (_mCP_annunCMD_L != value)
            {
                interfaceIT_LED_Set(Device.Session, MCP_annunCMD_LPos, _mCP_annunCMD_L = value);
            }
        }
    }

    public int MCP_annunCMD_CPos { get; init; } = 206;
    private bool _mCP_annunCMD_C;
    private bool MCP_annunCMD_C
    {
        set
        {
            if (_mCP_annunCMD_C != value)
            {
                interfaceIT_LED_Set(Device.Session, MCP_annunCMD_CPos, _mCP_annunCMD_C = value);
            }
        }
    }

    public int MCP_annunCMD_RPos { get; init; } = 208;
    private bool _mCP_annunCMD_R;
    private bool MCP_annunCMD_R
    {
        set
        {
            if (_mCP_annunCMD_R != value)
            {
                interfaceIT_LED_Set(Device.Session, MCP_annunCMD_RPos, _mCP_annunCMD_R = value);
            }
        }
    }

    protected virtual void BackgroundButtonLED(bool status) { }
    private bool _mCP_indication_powered;
    private bool MCP_panelPowered
    {
        get => _mCP_indication_powered;
        set
        {
            if (_mCP_indication_powered != value)
            {
                _mCP_indication_powered = value;
                BackgroundButtonLED(value);
                if (value)
                {
                    return;
                }
                ResetMCP7SegmentDisplays();
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
                    ResetMCP7SegmentDisplays();
                }
                else if (value == 0)
                {
                    interfaceIT_7Segment_Display(Device.Session, MCPSevenSegmentTest, MCPSevenSegmentStartPos);
                    IAS = true;
                    MACH = true;
                }
            }
        }
    }

    private void ResetMCP7SegmentDisplays()
    {
        _mCP_Altitude = null;
        _mCP_Heading = null;
        _mCP_IASMach = null;
        _mCP_VertSpeed = null;
        IAS = false;
        MACH = false;
        interfaceIT_7Segment_Display(Device.Session, new string(' ', MCPSevenSegmentTest.Length), MCPSevenSegmentStartPos);
    }

    public int MCP_IASMachStartPos { get; init; } = 5;
    private float? _mCP_IASMach;
    private float? MCP_IASMach
    {
        get => _mCP_IASMach;
        set
        {
            if (_mCP_IASMach != value && MAIN_LightsSelector != 0)
            {
                _mCP_IASMach = value;
                if (value < 10.0)
                {
                    interfaceIT_7Segment_Display(Device.Session, (value - Math.Truncate((double)value))?.ToString(".000", System.Globalization.CultureInfo.InvariantCulture).TrimStart('0'), MCP_IASMachStartPos);
                    IAS = false;
                    MACH = true;
                }
                else if (value >= 100)
                {
                    interfaceIT_7Segment_Display(Device.Session, " " + string.Format("{0,3}", value?.ToString(System.Globalization.CultureInfo.InvariantCulture)), MCP_IASMachStartPos);
                    MACH = false;
                    IAS = true;
                }
            }
        }
    }

    public int MCP_HeadingStartPos { get; init; } = 9;
    private ushort? _mCP_Heading;
    protected ushort? MCP_Heading
    {
        set
        {
            if (_mCP_Heading != value && MAIN_LightsSelector != 0)
            {
                interfaceIT_7Segment_Display(Device.Session, (_mCP_Heading = value)?.ToString("D3"), MCP_HeadingStartPos);
            }
        }
    }

    //777 MCP
    private bool _iAS;
    private bool IAS
    {
        set
        {
            if (_iAS != value)
            {
                SetIAS(value);
                _iAS = value;
            }
        }
    }

    protected virtual void SetIAS(bool value) { }

    //777 MCP
    private bool _mACH;
    private bool MACH
    {
        set
        {
            if (_mACH != value)
            {
                SetMACH(value);
                _mACH = value;
            }
        }
    }

    protected virtual void SetMACH(bool value) { }

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
                    interfaceIT_7Segment_Display(Device.Session, new string(' ', 3), MCP_IASMachStartPos);
                    IAS = false;
                    MACH = false;
                }
            }
        }
    }

    public int MCP_AltitudeStartPos { get; init; } = 12;
    private ushort? _mCP_Altitude;
    protected ushort? MCP_Altitude
    {
        set
        {
            if (_mCP_Altitude != value && MAIN_LightsSelector != 0)
            {
                _mCP_Altitude = value;
                if (value == 0)
                {
                    interfaceIT_7Segment_Display(Device.Session, " 0000", MCP_AltitudeStartPos);
                    return;
                }
                interfaceIT_7Segment_Display(Device.Session, string.Format("{0,5}", value.ToString()), MCP_AltitudeStartPos);
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
                    VerticalSpeedBlank();
                }
            }
        }
    }

    private void VerticalSpeedBlank()
    {
        interfaceIT_7Segment_Display(Device.Session, new string(' ', 5), MCP_VertSpeedStartPos);
    }

    public int MCP_VertSpeedStartPos { get; init; } = 17;
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
                    interfaceIT_7Segment_Display(Device.Session, "0000", MCP_VertSpeedStartPos + 1);
                }
                else if (value < 0)
                {
                    interfaceIT_7Segment_Display(Device.Session, "-" + string.Format("{0,4}", value?.ToString("D3").TrimStart('-')), MCP_VertSpeedStartPos);
                }
                else if (value > 0)
                {
                    interfaceIT_7Segment_Display(Device.Session, string.Format("{0,5}", value?.ToString("D3")), MCP_VertSpeedStartPos);
                }
            }
        }
    }

    protected override async Task StartSimConnectAsync()
    {
        await base.StartSimConnectAsync();
        interfaceIT_Switch_Enable_Callback(Device.Session, true, interfaceItKeyNotifyProc);
    }

    protected override void Stop()
    {
        base.Stop();
        pmdg737MCPLightCancellationTokenSource?.Cancel();
    }

    private bool[] _eLEC_BusPowered = new bool[16];
    private void SetELEC_BusPowered(bool[] value)
    {
        if (!_eLEC_BusPowered.SequenceEqual(value))
        {
            if (value[2] && value[3] && !value[7] && value[15] && !MCP_panelPowered)
            {
                BackgroundLED(true);
                backgroundLEDStatus = true;
            }
            else if (value[2] && value[3] && !value[4] && !value[5] && !value[6] && !value[7] && !value[8] && !value[9] && !value[10] && !value[11] && !value[12] && value[15])
            {
                Task.Run(() => PMDG737MCPLightAsync(true, (pmdg737MCPLightCancellationTokenSource = new()).Token));
                backgroundLEDStatus = true;
            }
            else if (!value[15] && value[2] && LTS_MainPanelKnob_0 <= 10)
            {
                BackgroundLED(false);
                backgroundLEDStatus = false;
            }
            else if (LTS_MainPanelKnob_0 <= 10 && value[0] && value[1] && value[2] && value[3] && value[4] && value[5] && value[6] && value[7] && value[8] && value[9] && value[10] && value[11] && value[12] && value[15])
            {
                pmdg737MCPLightCancellationTokenSource?.Cancel();
                BackgroundLED(false);
                backgroundLEDStatus = false;
            }
            else if (!value[1] && !value[2] && !value[3] && !value[4] && !value[5] && !value[6] && !value[7] && !value[8] && !value[9] && !value[10] && !value[11] && !value[12] && !value[13] && !value[14] && !value[15])
            {
                BackgroundLED(false);
                backgroundLEDStatus = false;
            }
            _eLEC_BusPowered = value;
        }
    }

    protected override void SimConnect_OnRecvClientData(uint dwRequestID, object dwData)
    {
        if ((uint)PMDG.DATA_REQUEST_ID.DATA_REQUEST == dwRequestID)
        {
            //SetELEC_BusPowered(((PMDG_747QOTSII_Data)dwData).ELEC_BusPowered);

            MCP_ATArm_Sw_On = ((PMDG_747QOTSII_Data)dwData).MCP_ATArm_Sw_On;

            MCP_annunTHR = ((PMDG_747QOTSII_Data)dwData).MCP_annunTHR;
            MCP_annunSPD = ((PMDG_747QOTSII_Data)dwData).MCP_annunSPD;
            MCP_annunVNAV = ((PMDG_747QOTSII_Data)dwData).MCP_annunVNAV;
            MCP_annunFLCH = ((PMDG_747QOTSII_Data)dwData).MCP_annunFLCH;
            MCP_annunHDG_HOLD = ((PMDG_747QOTSII_Data)dwData).MCP_annunHDG_HOLD;
            MCP_annunLNAV = ((PMDG_747QOTSII_Data)dwData).MCP_annunLNAV;
            MCP_annunLOC = ((PMDG_747QOTSII_Data)dwData).MCP_annunLOC;
            MCP_annunAPP = ((PMDG_747QOTSII_Data)dwData).MCP_annunAPP;
            MCP_annunALT_HOLD = ((PMDG_747QOTSII_Data)dwData).MCP_annunALT_HOLD;
            MCP_annunVS = ((PMDG_747QOTSII_Data)dwData).MCP_annunVS;

            MCP_annunCMD_L = ((PMDG_747QOTSII_Data)dwData).MCP_annunAP[0];
            MCP_annunCMD_C = ((PMDG_747QOTSII_Data)dwData).MCP_annunAP[1];
            MCP_annunCMD_R = ((PMDG_747QOTSII_Data)dwData).MCP_annunAP[2];

            //LTS_MainPanelKnob_0 = ((PMDG_747QOTSII_Data)dwData).LTS_MainPanelKnob[0];

            MCP_panelPowered = ((PMDG_747QOTSII_Data)dwData).MCP_panelPowered;
            MCP_IASBlank = ((PMDG_747QOTSII_Data)dwData).MCP_IASBlank;
            MCP_VertSpeedBlank = ((PMDG_747QOTSII_Data)dwData).MCP_VertSpeedBlank;

            if (MCP_panelPowered)
            {
                //MAIN_LightsSelector = ((PMDG_747QOTSII_Data)dwData).MAIN_LightsSelector;
                MCP_IASMach = ((PMDG_747QOTSII_Data)dwData).MCP_IASMach;
                MCP_VertSpeed = ((PMDG_747QOTSII_Data)dwData).MCP_VertSpeed;
                SetHeadingAltitude(((PMDG_747QOTSII_Data)dwData).MCP_Heading, ((PMDG_747QOTSII_Data)dwData).MCP_Altitude);
            }
        }
    }

    protected virtual void SetHeadingAltitude(ushort? heading, ushort? altitude)
    {
        MCP_Heading = heading;
        MCP_Altitude = altitude;
    }

    //private uint eFIS_CPT_VOR_ADF_SELECTOR_L;
    //private uint eFIS_CPT_VOR_ADF_SELECTOR_R;
    //private uint nMCPCourse_0;

    protected override void KeyPressedProc(uint session, int key, uint direction)
    {
    //    //1-0
    //    if (new int[] { 2, 3, 25, 69, 71, 79, 80 }.Contains(key))
    //    {
    //        if (direction == 1)
    //        {
    //            direction = 0;
    //        }

    //        else if (direction == 0)
    //        {
    //            direction = 1;
    //        }
    //    }

    //    //1-2
    //    else if (new int[] { 70, 72 }.Contains(key))
    //    {
    //        if (direction == 1)
    //        {
    //            direction = 2;
    //        }

    //        else if (direction == 0 && (eFIS_CPT_VOR_ADF_SELECTOR_L != 0 || eFIS_CPT_VOR_ADF_SELECTOR_R != 0))
    //        {
    //            direction = 1;
    //        }
    //    }

    //    //-3 & -14
    //    else if (new int[] { 4, 5, 6, 8, 9, 10, 12, 13, 14, 15, 17, 18, 19, 20, 21, 23, 24, 73, 74, 75, 76, 77, 78, 81, 82, 83, 84, 85, 86, 87 }.Contains(key))
    //    {
    //        if (direction == 1)
    //        {
    //            direction = MOUSE_FLAG_LEFTSINGLE;
    //        }

    //        else if (direction == 0)
    //        {
    //            direction = MOUSE_FLAG_LEFTRELEASE;
    //        }
    //    }

    //    switch (key)
    //    {
    //        //Special for BARO & MINS +-10
    //        case 1:
    //            nMCPCourse_0 = direction;
    //            break;

    //        //0-1
    //        case 22:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_DISENGAGE_BAR);
    //            break;

    //        //1-0
    //        case 2:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_FD_SWITCH_L);
    //            break;

    //        case 3:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_AT_ARM_SWITCH);
    //            break;

    //        case 25:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_FD_SWITCH_R);
    //            break;

    //        case 69:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_VOR_ADF_SELECTOR_L);
    //            eFIS_CPT_VOR_ADF_SELECTOR_L = direction;
    //            break;

    //        case 71:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_VOR_ADF_SELECTOR_R);
    //            eFIS_CPT_VOR_ADF_SELECTOR_R = direction;
    //            break;

    //        case 79:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_MINIMUMS_RADIO_BARO);
    //            break;

    //        case 80:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_BARO_IN_HPA);
    //            break;

    //        //1-2
    //        case 70:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_VOR_ADF_SELECTOR_L);
    //            break;

    //        case 72:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_VOR_ADF_SELECTOR_R);
    //            break;

    //        //0-...
    //        case 26 when direction == 1:
    //            simConnectClient.TransmitEvent(0, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR);
    //            break;

    //        case 27 when direction == 1:
    //            simConnectClient.TransmitEvent(1, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR);
    //            break;

    //        case 28 when direction == 1:
    //            simConnectClient.TransmitEvent(2, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR);
    //            break;

    //        case 29 when direction == 1:
    //            simConnectClient.TransmitEvent(3, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR);
    //            break;

    //        case 30 when direction == 1:
    //            simConnectClient.TransmitEvent(4, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR);
    //            break;

    //        case 65 when direction == 1:
    //            simConnectClient.TransmitEvent(0, PMDGEvents.EVT_EFIS_CPT_MODE);
    //            break;

    //        case 66 when direction == 1:
    //            simConnectClient.TransmitEvent(1, PMDGEvents.EVT_EFIS_CPT_MODE);
    //            break;

    //        case 67 when direction == 1:
    //            simConnectClient.TransmitEvent(2, PMDGEvents.EVT_EFIS_CPT_MODE);
    //            break;

    //        case 68 when direction == 1:
    //            simConnectClient.TransmitEvent(3, PMDGEvents.EVT_EFIS_CPT_MODE);
    //            break;

    //        case 89 when direction == 1:
    //            simConnectClient.TransmitEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE);
    //            break;

    //        case 90 when direction == 1:
    //            simConnectClient.TransmitEvent(1, PMDGEvents.EVT_EFIS_CPT_RANGE);
    //            break;

    //        case 91 when direction == 1:
    //            simConnectClient.TransmitEvent(2, PMDGEvents.EVT_EFIS_CPT_RANGE);
    //            break;

    //        case 92 when direction == 1:
    //            simConnectClient.TransmitEvent(3, PMDGEvents.EVT_EFIS_CPT_RANGE);
    //            break;

    //        case 93 when direction == 1:
    //            simConnectClient.TransmitEvent(4, PMDGEvents.EVT_EFIS_CPT_RANGE);
    //            break;

    //        case 94 when direction == 1:
    //            simConnectClient.TransmitEvent(5, PMDGEvents.EVT_EFIS_CPT_RANGE);
    //            break;

    //        case 95 when direction == 1:
    //            simConnectClient.TransmitEvent(6, PMDGEvents.EVT_EFIS_CPT_RANGE);
    //            break;

    //        case 96 when direction == 1:
    //            simConnectClient.TransmitEvent(7, PMDGEvents.EVT_EFIS_CPT_RANGE);
    //            break;

    //        //-3 & -14
    //        case 4:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_N1_SWITCH);
    //            break;

    //        case 5:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_SPEED_SWITCH);
    //            break;

    //        case 6:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_CO_SWITCH);
    //            break;

    //        case 8:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_SPD_INTV_SWITCH);
    //            break;

    //        case 9:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_VNAV_SWITCH);
    //            break;

    //        case 10:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_LVL_CHG_SWITCH);
    //            break;

    //        case 12:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_HDG_SEL_SWITCH);
    //            break;

    //        case 13:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_LNAV_SWITCH);
    //            break;

    //        case 14:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_VOR_LOC_SWITCH);
    //            break;

    //        case 15:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_APP_SWITCH);
    //            break;

    //        case 17:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_ALT_HOLD_SWITCH);
    //            break;

    //        case 18:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_ALT_INTV_SWITCH);
    //            break;

    //        case 19:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_VS_SWITCH);
    //            break;

    //        case 20:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_CMD_A_SWITCH);
    //            break;

    //        case 21:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_CWS_A_SWITCH);
    //            break;

    //        case 23:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_CMD_B_SWITCH);
    //            break;

    //        case 24:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_CWS_B_SWITCH);
    //            break;

    //        case 73:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_FPV);
    //            break;

    //        case 74:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_MTRS);
    //            break;

    //        case 75:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_MODE_CTR);
    //            break;

    //        case 76:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_RANGE_TFC);
    //            break;

    //        case 77:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_MINIMUMS_RST);
    //            break;

    //        case 78:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_BARO_STD);
    //            break;

    //        case 81:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_WXR);
    //            break;

    //        case 82:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_STA);
    //            break;

    //        case 83:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_WPT);
    //            break;

    //        case 84:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_ARPT);
    //            break;

    //        case 85:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_DATA);
    //            break;

    //        case 86:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_POS);
    //            break;

    //        case 87:
    //            simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_TERR);
    //            break;

    //        //-18 & -17
    //        case 33 when direction == 1:
    //            simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_COURSE_SELECTOR_L);
    //            break;

    //        case 34 when direction == 1:
    //            simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_COURSE_SELECTOR_L);
    //            break;

    //        case 35 when direction == 1:
    //            simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_SPEED_SELECTOR);
    //            break;

    //        case 36 when direction == 1:
    //            simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_SPEED_SELECTOR);
    //            break;

    //        case 37 when direction == 1:
    //            simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_HEADING_SELECTOR);
    //            break;

    //        case 38 when direction == 1:
    //            simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_HEADING_SELECTOR);
    //            break;

    //        case 41 when direction == 1:
    //            simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_ALTITUDE_SELECTOR);
    //            break;

    //        case 42 when direction == 1:
    //            simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_ALTITUDE_SELECTOR);
    //            break;

    //        case 43 when direction == 1:
    //            simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_VS_SELECTOR);
    //            break;

    //        case 44 when direction == 1:
    //            simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_VS_SELECTOR);
    //            break;

    //        case 45 when direction == 1:
    //            simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_COURSE_SELECTOR_R);
    //            break;

    //        case 46 when direction == 1:
    //            simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_COURSE_SELECTOR_R);
    //            break;

    //        //-18 & -17 (-1 & -3)
    //        case 57 when direction == 1:
    //            TransmitBAROMINSTenTimes(PMDGEvents.EVT_EFIS_CPT_MINIMUMS, false, nMCPCourse_0);
    //            break;

    //        case 58 when direction == 1:
    //            TransmitBAROMINSTenTimes(PMDGEvents.EVT_EFIS_CPT_MINIMUMS, true, nMCPCourse_0);
    //            break;

    //        case 59 when direction == 1:
    //            TransmitBAROMINSTenTimes(PMDGEvents.EVT_EFIS_CPT_BARO, false, nMCPCourse_0);
    //            break;

    //        case 60 when direction == 1:
    //            TransmitBAROMINSTenTimes(PMDGEvents.EVT_EFIS_CPT_BARO, true, nMCPCourse_0);
    //            break;
    //    }
    //}

    //protected void TransmitBAROMINSTenTimes(Enum eventID, bool isUp, uint special)
    //{
    //    if (special == 0)
    //    {
    //        if (isUp)
    //        {
    //            simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, eventID);
    //        }
    //        else if (!isUp)
    //        {
    //            simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, eventID);
    //        }
    //    }
    //    else if (special == 1)
    //    {
    //        for (int i = 0; i < 10; i++)
    //        {
    //            if (isUp)
    //            {
    //                simConnectClient.TransmitEvent(MOUSE_FLAG_RIGHTSINGLE, eventID);
    //                simConnectClient.TransmitEvent(MOUSE_FLAG_RIGHTRELEASE, eventID);
    //            }
    //            else if (!isUp)
    //            {
    //                simConnectClient.TransmitEvent(MOUSE_FLAG_LEFTSINGLE, eventID);
    //                simConnectClient.TransmitEvent(MOUSE_FLAG_LEFTRELEASE, eventID);
    //            }
    //        }
    //    }
    }
}