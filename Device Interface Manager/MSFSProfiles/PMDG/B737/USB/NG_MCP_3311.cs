using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.FlightSimulator.SimConnect;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceITAPI_Data;
using static Device_Interface_Manager.MSFSProfiles.PMDG.PMDG_NG3_SDK;

namespace Device_Interface_Manager.MSFSProfiles.PMDG.B737.USB;

public class NG_MCP_3311 : MSFSProfiles.USB
{
    //MCP SETUP
    private CancellationTokenSource pmdg737MCPBlinkingCancellationTokenSource;
    private CancellationTokenSource pmdg737MCPIASOverspeedUnderspeedFlashingCancellationTokenSource;
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

    //Task for IAS/MACH flashing
    public int MCPMachSevenSegmentDot { get; init; } = 48;
    private async Task PMDG737MCPFlashing(string flashingAB, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (MCP_IASMach < 1)
            {
                interfaceIT_7Segment_Display(Device.Session, ' ' + flashingAB, MCP_IASMachStartPos - 1);
                interfaceIT_LED_Set(Device.Session, MCPMachSevenSegmentDot, true);
                await Task.Delay(500, token);
                interfaceIT_7Segment_Display(Device.Session, new string(' ', MCP_IASMachStartPos - 4), MCP_IASMachStartPos - 1);
                interfaceIT_LED_Set(Device.Session, MCPMachSevenSegmentDot, true);
            }
            else if (MCP_IASMach >= 100)
            {
                interfaceIT_7Segment_Display(Device.Session, flashingAB, MCP_IASMachStartPos - 1);
                await Task.Delay(500, token);
                interfaceIT_7Segment_Display(Device.Session, " ", MCP_IASMachStartPos - 1);
            }
            await Task.Delay(500, token);
        }
    }

    //Task for MCP blinking (Annunciator Test)
    public string MCPSevenSegmentTest { get; init; } = "888 888888888880-8880888";
    public int MCPSevenSegmentStartPos { get; init; } = 1;
    private async Task PMDG737MCPBlinkingAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            interfaceIT_7Segment_Display(Device.Session, MCPSevenSegmentTest, MCPSevenSegmentStartPos);
            await Task.Delay(2000, token);
            interfaceIT_7Segment_Display(Device.Session, new string(' ', MCPSevenSegmentTest.Length), MCPSevenSegmentStartPos);
            await Task.Delay(1000, token);
        }
    }

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
                //FIX THIS
            }
        }
    }

    public int MCP_ATArmSwPos { get; init; } = 5;
    private bool _mCP_ATArmSw;
    private bool MCP_ATArmSw
    {
        set
        {
            if (_mCP_ATArmSw != value)
            {
                interfaceIT_Dataline_Set(Device.Session, MCP_ATArmSwPos, _mCP_ATArmSw = value);
            }
        }
    }

    public int MCP_annunFD_0Pos { get; init; } = 225;
    private bool _mCP_annunFD_0;
    private bool MCP_annunFD_0
    {
        set
        {
            if (_mCP_annunFD_0 != value)
            {
                interfaceIT_LED_Set(Device.Session, MCP_annunFD_0Pos, _mCP_annunFD_0 = value);
            }
        }
    }

    public int MCP_annunFD_1Pos { get; init; } = 226;
    private bool _mCP_annunFD_1;
    private bool MCP_annunFD_1
    {
        set
        {
            if (_mCP_annunFD_1 != value)
            {
                interfaceIT_LED_Set(Device.Session, MCP_annunFD_1Pos, _mCP_annunFD_1 = value);
            }
        }
    }

    public int MCP_annunATArmPos { get; init; } = 227;
    private bool _mCP_annunATArm;
    private bool MCP_annunATArm
    {
        set
        {
            if (_mCP_annunATArm != value)
            {
                interfaceIT_LED_Set(Device.Session, MCP_annunATArmPos, _mCP_annunATArm = value);
            }
        }
    }

    public int MCP_annunN1Pos { get; init; } = 193;
    private bool _mCP_annunN1;
    private bool MCP_annunN1
    {
        set
        {
            if (_mCP_annunN1 != value)
            {
                interfaceIT_LED_Set(Device.Session, MCP_annunN1Pos, _mCP_annunN1 = value);
            }
        }
    }

    public int MCP_annunSPEEDPos { get; init; } = 194;
    private bool _mCP_annunSPEED;
    private bool MCP_annunSPEED
    {
        set
        {
            if (_mCP_annunSPEED != value)
            {
                interfaceIT_LED_Set(Device.Session, MCP_annunSPEEDPos, _mCP_annunSPEED = value);
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

    public int MCP_annunLVL_CHGPos { get; init; } = 196;
    private bool _mCP_annunLVL_CHG;
    private bool MCP_annunLVL_CHG
    {
        set
        {
            if (_mCP_annunLVL_CHG != value)
            {
                interfaceIT_LED_Set(Device.Session, MCP_annunLVL_CHGPos, _mCP_annunLVL_CHG = value);
            }
        }
    }

    public int MCP_annunHDG_SELPos { get; init; } = 197;
    private bool _mCP_annunHDG_SEL;
    private bool MCP_annunHDG_SEL
    {
        set
        {
            if (_mCP_annunHDG_SEL != value)
            {
                interfaceIT_LED_Set(Device.Session, MCP_annunHDG_SELPos, _mCP_annunHDG_SEL = value);
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

    public int MCP_annunVOR_LOCPos { get; init; } = 199;
    private bool _mCP_annunVOR_LOC;
    private bool MCP_annunVOR_LOC
    {
        set
        {
            if (_mCP_annunVOR_LOC != value)
            {
                interfaceIT_LED_Set(Device.Session, MCP_annunVOR_LOCPos, _mCP_annunVOR_LOC = value);
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

    public int MCP_annunCMD_APos { get; init; } = 204;
    private bool _mCP_annunCMD_A;
    private bool MCP_annunCMD_A
    {
        set
        {
            if (_mCP_annunCMD_A != value)
            {
                interfaceIT_LED_Set(Device.Session, MCP_annunCMD_APos, _mCP_annunCMD_A = value);
            }
        }
    }

    public int MCP_annunCWS_APos { get; init; } = 205;
    private bool _mCP_annunCWS_A;
    private bool MCP_annunCWS_A
    {
        set
        {
            if (_mCP_annunCWS_A != value)
            {
                interfaceIT_LED_Set(Device.Session, MCP_annunCWS_APos, _mCP_annunCWS_A = value);
            }
        }
    }

    public int MCP_annunCMD_BPos { get; init; } = 206;
    private bool _mCP_annunCMD_B;
    private bool MCP_annunCMD_B
    {
        set
        {
            if (_mCP_annunCMD_B != value)
            {
                interfaceIT_LED_Set(Device.Session, MCP_annunCMD_BPos, _mCP_annunCMD_B = value);
            }
        }
    }

    public int MCP_annunCWS_BPos { get; init; } = 207;
    private bool _mCP_annunCWS_B;
    private bool MCP_annunCWS_B
    {
        set
        {
            if (_mCP_annunCWS_B != value)
            {
                interfaceIT_LED_Set(Device.Session, MCP_annunCWS_BPos, _mCP_annunCWS_B = value);
            }
        }
    }

    protected virtual void BackgroundButtonLED(bool status) { }
    private bool _mCP_indication_powered;
    private bool MCP_indication_powered
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
                pmdg737MCPIASOverspeedUnderspeedFlashingCancellationTokenSource?.Cancel();
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
                    Task.Run(() => PMDG737MCPBlinkingAsync((pmdg737MCPBlinkingCancellationTokenSource = new()).Token));
                    IAS = true;
                    MACH = true;
                }
            }
        }
    }

    private void ResetMCP7SegmentDisplays()
    {
        pmdg737MCPBlinkingCancellationTokenSource?.Cancel();
        _mCP_Altitude = null;
        _mCP_Course_0 = null;
        _mCP_Course_1 = null;
        _mCP_Heading = null;
        _mCP_IASMach = null;
        _mCP_VertSpeed = null;
        IAS = false;
        MACH = false;
        interfaceIT_7Segment_Display(Device.Session, new string(' ', MCPSevenSegmentTest.Length), MCPSevenSegmentStartPos);
    }

    public int MCP_IASMachStartPos { get; init; } = 6;
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
                    interfaceIT_7Segment_Display(Device.Session, string.Format("{0,3}", value?.ToString("#.00", System.Globalization.CultureInfo.InvariantCulture)).TrimStart('.'), MCP_IASMachStartPos + 1);
                    interfaceIT_7Segment_Display(Device.Session, " ", MCP_IASMachStartPos);
                    interfaceIT_LED_Set(Device.Session, MCPMachSevenSegmentDot, true);
                    IAS = false;
                    MACH = true;
                }
                else if (value >= 100)
                {
                    interfaceIT_7Segment_Display(Device.Session, string.Format("{0,3}", value?.ToString(System.Globalization.CultureInfo.InvariantCulture)), MCP_IASMachStartPos);
                    MACH = false;
                    IAS = true;
                }
            }
        }
    }

    public int MCP_Course_0StartPos { get; init; } = 1;
    private ushort? _mCP_Course_0;
    protected ushort? MCP_Course_0
    {
        get => _mCP_Course_0;
        set
        {
            if (_mCP_Course_0 != value && MAIN_LightsSelector != 0)
            {
                interfaceIT_7Segment_Display(Device.Session, (_mCP_Course_0 = value)?.ToString("D3"), MCP_Course_0StartPos);
            }
        }
    }

    public int MCP_Course_1StartPos { get; init; } = 22;
    private ushort? _mCP_Course_1;
    protected ushort? MCP_Course_1
    {
        get => _mCP_Course_1;
        set
        {
            if (_mCP_Course_1 != value && MAIN_LightsSelector != 0)
            {
                interfaceIT_7Segment_Display(Device.Session, (_mCP_Course_1 = value)?.ToString("D3"), MCP_Course_1StartPos);
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

    private bool _mCP_IASOverspeedFlash;
    private bool MCP_IASOverspeedFlash
    {
        set
        {
            if (_mCP_IASOverspeedFlash != value)
            {
                _mCP_IASOverspeedFlash = value;
                if (value)
                {
                    Task.Run(() => PMDG737MCPFlashing("B", (pmdg737MCPIASOverspeedUnderspeedFlashingCancellationTokenSource = new()).Token));
                    return;
                }
                OverSpeedUnderspeedReset();
            }
        }
    }

    private void OverSpeedUnderspeedReset()
    {
        pmdg737MCPIASOverspeedUnderspeedFlashingCancellationTokenSource?.Cancel();
        if (MCP_IASMach < 1)
        {
            interfaceIT_7Segment_Display(Device.Session, new string(' ', 2), MCP_IASMachStartPos - 1);
            interfaceIT_LED_Set(Device.Session, MCPMachSevenSegmentDot, true);
        }
        else if (MCP_IASMach >= 100)
        {
            interfaceIT_7Segment_Display(Device.Session, " ", MCP_IASMachStartPos - 1);
        }
    }

    private bool _mCP_IASUnderspeedFlash;
    private bool MCP_IASUnderspeedFlash
    {
        set
        {
            if (_mCP_IASUnderspeedFlash != value)
            {
                _mCP_IASUnderspeedFlash = value;
                if (value)
                {
                    Task.Run(() => PMDG737MCPFlashing("A", (pmdg737MCPIASOverspeedUnderspeedFlashingCancellationTokenSource = new()).Token));
                    return;
                }
                OverSpeedUnderspeedReset();
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
                    VerticalSpeedBlank();
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

    public override void Stop()
    {
        base.Stop();
        pmdg737MCPBlinkingCancellationTokenSource?.Cancel();
        pmdg737MCPIASOverspeedUnderspeedFlashingCancellationTokenSource?.Cancel();
        pmdg737MCPLightCancellationTokenSource?.Cancel();
    }


    private bool[] _eLEC_BusPowered = new bool[16];
    private void SetELEC_BusPowered(bool[] value)
    {
        if (!_eLEC_BusPowered.SequenceEqual(value))
        {
            if (value[2] && value[3] && !value[7] && value[15] && !MCP_indication_powered)
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

    protected override void SimConnect_OnRecvClientData(SimConnect sender, SIMCONNECT_RECV_CLIENT_DATA data)
    {
        if ((uint)DATA_REQUEST_ID.DATA_REQUEST == data.dwRequestID)
        {
            SetELEC_BusPowered(((PMDG_NG3_Data)data.dwData[0]).ELEC_BusPowered);

            MCP_ATArmSw = ((PMDG_NG3_Data)data.dwData[0]).MCP_ATArmSw;

            MCP_annunFD_0 = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunFD[0];
            MCP_annunFD_1 = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunFD[1];
            MCP_annunATArm = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunATArm;
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
            MCP_annunCWS_A = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunCWS_A;
            MCP_annunCMD_B = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunCMD_B;
            MCP_annunCWS_B = ((PMDG_NG3_Data)data.dwData[0]).MCP_annunCWS_B;

            LTS_MainPanelKnob_0 = ((PMDG_NG3_Data)data.dwData[0]).LTS_MainPanelKnob[0];

            MCP_indication_powered = ((PMDG_NG3_Data)data.dwData[0]).MCP_indication_powered;
            MCP_IASBlank = ((PMDG_NG3_Data)data.dwData[0]).MCP_IASBlank;
            MCP_IASOverspeedFlash = ((PMDG_NG3_Data)data.dwData[0]).MCP_IASOverspeedFlash;
            MCP_IASUnderspeedFlash = ((PMDG_NG3_Data)data.dwData[0]).MCP_IASUnderspeedFlash;
            MCP_VertSpeedBlank = ((PMDG_NG3_Data)data.dwData[0]).MCP_VertSpeedBlank;

            if (MCP_indication_powered)
            {
                MAIN_LightsSelector = ((PMDG_NG3_Data)data.dwData[0]).MAIN_LightsSelector;
                MCP_IASMach = ((PMDG_NG3_Data)data.dwData[0]).MCP_IASMach;
                MCP_Course_0 = ((PMDG_NG3_Data)data.dwData[0]).MCP_Course[0];
                MCP_Course_1 = ((PMDG_NG3_Data)data.dwData[0]).MCP_Course[1];
                MCP_VertSpeed = ((PMDG_NG3_Data)data.dwData[0]).MCP_VertSpeed;
                SetHeadingAltitude(((PMDG_NG3_Data)data.dwData[0]).MCP_Heading, ((PMDG_NG3_Data)data.dwData[0]).MCP_Altitude);
            }
        }
    }

    protected virtual void SetHeadingAltitude(ushort? heading, ushort? altitude)
    {
        MCP_Heading = heading;
        MCP_Altitude = altitude;
    }

    private uint eFIS_CPT_VOR_ADF_SELECTOR_L;
    private uint eFIS_CPT_VOR_ADF_SELECTOR_R;
    private uint nMCPCourse_0;

    protected override void KeyPressedProc(uint session, int key, uint direction)
    {
        //1-0
        if (new int[] { 2, 3, 25, 69, 71, 79, 80 }.Contains(key))
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
        else if (new int[] { 4, 5, 6, 8, 9, 10, 12, 13, 14, 15, 17, 18, 19, 20, 21, 23, 24, 73, 74, 75, 76, 77, 78, 81, 82, 83, 84, 85, 86, 87 }.Contains(key))
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
            //Special for BARO & MINS +-10
            case 1:
                nMCPCourse_0 = direction;
                break;

            //0-1
            case 22:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_DISENGAGE_BAR);
                break;

            //1-0
            case 2:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_FD_SWITCH_L);
                break;

            case 3:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_AT_ARM_SWITCH);
                break;

            case 25:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_FD_SWITCH_R);
                break;

            case 69:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_VOR_ADF_SELECTOR_L);
                eFIS_CPT_VOR_ADF_SELECTOR_L = direction;
                break;

            case 71:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_VOR_ADF_SELECTOR_R);
                eFIS_CPT_VOR_ADF_SELECTOR_R = direction;
                break;

            case 79:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_MINIMUMS_RADIO_BARO);
                break;

            case 80:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_BARO_IN_HPA);
                break;

            //1-2
            case 70:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_VOR_ADF_SELECTOR_L);
                break;

            case 72:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_VOR_ADF_SELECTOR_R);
                break;

            //0-...
            case 26 when direction == 1:
                simConnectClient.TransmitEvent(0, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR);
                break;

            case 27 when direction == 1:
                simConnectClient.TransmitEvent(1, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR);
                break;

            case 28 when direction == 1:
                simConnectClient.TransmitEvent(2, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR);
                break;

            case 29 when direction == 1:
                simConnectClient.TransmitEvent(3, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR);
                break;

            case 30 when direction == 1:
                simConnectClient.TransmitEvent(4, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR);
                break;

            case 65 when direction == 1:
                simConnectClient.TransmitEvent(0, PMDGEvents.EVT_EFIS_CPT_MODE);
                break;

            case 66 when direction == 1:
                simConnectClient.TransmitEvent(1, PMDGEvents.EVT_EFIS_CPT_MODE);
                break;

            case 67 when direction == 1:
                simConnectClient.TransmitEvent(2, PMDGEvents.EVT_EFIS_CPT_MODE);
                break;

            case 68 when direction == 1:
                simConnectClient.TransmitEvent(3, PMDGEvents.EVT_EFIS_CPT_MODE);
                break;

            case 89 when direction == 1:
                simConnectClient.TransmitEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE);
                break;

            case 90 when direction == 1:
                simConnectClient.TransmitEvent(1, PMDGEvents.EVT_EFIS_CPT_RANGE);
                break;

            case 91 when direction == 1:
                simConnectClient.TransmitEvent(2, PMDGEvents.EVT_EFIS_CPT_RANGE);
                break;

            case 92 when direction == 1:
                simConnectClient.TransmitEvent(3, PMDGEvents.EVT_EFIS_CPT_RANGE);
                break;

            case 93 when direction == 1:
                simConnectClient.TransmitEvent(4, PMDGEvents.EVT_EFIS_CPT_RANGE);
                break;

            case 94 when direction == 1:
                simConnectClient.TransmitEvent(5, PMDGEvents.EVT_EFIS_CPT_RANGE);
                break;

            case 95 when direction == 1:
                simConnectClient.TransmitEvent(6, PMDGEvents.EVT_EFIS_CPT_RANGE);
                break;

            case 96 when direction == 1:
                simConnectClient.TransmitEvent(7, PMDGEvents.EVT_EFIS_CPT_RANGE);
                break;

            //-3 & -14
            case 4:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_N1_SWITCH);
                break;

            case 5:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_SPEED_SWITCH);
                break;

            case 6:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_CO_SWITCH);
                break;

            case 8:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_SPD_INTV_SWITCH);
                break;

            case 9:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_VNAV_SWITCH);
                break;

            case 10:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_LVL_CHG_SWITCH);
                break;

            case 12:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_HDG_SEL_SWITCH);
                break;

            case 13:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_LNAV_SWITCH);
                break;

            case 14:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_VOR_LOC_SWITCH);
                break;

            case 15:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_APP_SWITCH);
                break;

            case 17:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_ALT_HOLD_SWITCH);
                break;

            case 18:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_ALT_INTV_SWITCH);
                break;

            case 19:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_VS_SWITCH);
                break;

            case 20:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_CMD_A_SWITCH);
                break;

            case 21:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_CWS_A_SWITCH);
                break;

            case 23:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_CMD_B_SWITCH);
                break;

            case 24:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_CWS_B_SWITCH);
                break;

            case 73:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_FPV);
                break;

            case 74:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_MTRS);
                break;

            case 75:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_MODE_CTR);
                break;

            case 76:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_RANGE_TFC);
                break;

            case 77:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_MINIMUMS_RST);
                break;

            case 78:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_BARO_STD);
                break;

            case 81:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_WXR);
                break;

            case 82:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_STA);
                break;

            case 83:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_WPT);
                break;

            case 84:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_ARPT);
                break;

            case 85:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_DATA);
                break;

            case 86:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_POS);
                break;

            case 87:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_TERR);
                break;

            //-18 & -17
            case 33 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_COURSE_SELECTOR_L);
                break;

            case 34 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_COURSE_SELECTOR_L);
                break;

            case 35 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_SPEED_SELECTOR);
                break;

            case 36 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_SPEED_SELECTOR);
                break;

            case 37 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_HEADING_SELECTOR);
                break;

            case 38 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_HEADING_SELECTOR);
                break;

            case 41 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_ALTITUDE_SELECTOR);
                break;

            case 42 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_ALTITUDE_SELECTOR);
                break;

            case 43 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_VS_SELECTOR);
                break;

            case 44 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_VS_SELECTOR);
                break;

            case 45 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_COURSE_SELECTOR_R);
                break;

            case 46 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_COURSE_SELECTOR_R);
                break;

            //-18 & -17 (-1 & -3)
            case 57 when direction == 1:
                TransmitBAROMINSTenTimes(PMDGEvents.EVT_EFIS_CPT_MINIMUMS, false, nMCPCourse_0);
                break;

            case 58 when direction == 1:
                TransmitBAROMINSTenTimes(PMDGEvents.EVT_EFIS_CPT_MINIMUMS, true, nMCPCourse_0);
                break;

            case 59 when direction == 1:
                TransmitBAROMINSTenTimes(PMDGEvents.EVT_EFIS_CPT_BARO, false, nMCPCourse_0);
                break;

            case 60 when direction == 1:
                TransmitBAROMINSTenTimes(PMDGEvents.EVT_EFIS_CPT_BARO, true, nMCPCourse_0);
                break;
        }
    }

    protected void TransmitBAROMINSTenTimes(Enum eventID, bool isUp, uint special)
    {
        if (special == 0)
        {
            if (isUp)
            {
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, eventID);
            }
            else if (!isUp)
            {
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, eventID);
            }
        }
        else if (special == 1)
        {
            for (int i = 0; i < 10; i++)
            {
                if (isUp)
                {
                    simConnectClient.TransmitEvent(MOUSE_FLAG_RIGHTSINGLE, eventID);
                    simConnectClient.TransmitEvent(MOUSE_FLAG_RIGHTRELEASE, eventID);
                }
                else if (!isUp)
                {
                    simConnectClient.TransmitEvent(MOUSE_FLAG_LEFTSINGLE, eventID);
                    simConnectClient.TransmitEvent(MOUSE_FLAG_LEFTRELEASE, eventID);
                }
            }
        }
    }
}