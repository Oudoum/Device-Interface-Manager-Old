using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.FlightSimulator.SimConnect;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceITAPI_Data;
using static Device_Interface_Manager.MSFSProfiles.PMDG.PMDG_NG3_SDK;

namespace Device_Interface_Manager.MSFSProfiles.PMDG.B737;

public class NG_MCP_USB : USB
{
    //MCP SETUP
    private CancellationTokenSource pmdg737MCPBlinkingCancellationTokenSource;
    private CancellationTokenSource pmdg737MCPIASOverspeedFlashingCancellationTokenSource;
    private CancellationTokenSource pmdg737MCPIASUnderspeedFlashingCancellationTokenSource;
    private CancellationTokenSource pmdg737MCPLightCancellationTokenSource;

    //Method for MCP Lights
    private bool pmdg737NotPowered;
    private void PMDG737MCPLight(bool pmdg737MCPLightValue, CancellationToken token)
    {
        if (pmdg737MCPLightValue && !pmdg737NotPowered)
        {
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

    //Thread Method for IAS/MACH flashing
    private void PMDG737MCPFlashing(string flashingAB, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (MCP_IASMach < 1)
            {
                _ = interfaceIT_7Segment_Display(Device.Session, " " + flashingAB, 5);
                _ = interfaceIT_LED_Set(Device.Session, 48, true);
                Thread.Sleep(500);
            }
            if (MCP_IASMach < 1)
            {
                _ = interfaceIT_7Segment_Display(Device.Session, "  ", 5);
                _ = interfaceIT_LED_Set(Device.Session, 48, true);
            }
            if (MCP_IASMach >= 100)
            {
                _ = interfaceIT_7Segment_Display(Device.Session, flashingAB, 5);
                Thread.Sleep(500);
            }
            if (MCP_IASMach >= 100)
            {
                _ = interfaceIT_7Segment_Display(Device.Session, " ", 5);
            }
            Thread.Sleep(500);
        }
    }

    //Thread Method for MCP blinking
    private void PMDG737MCPBlinking(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            _ = interfaceIT_7Segment_Display(Device.Session, "888 888888888880-8880888", 1);
            Thread.Sleep(2000);
            if (token.IsCancellationRequested)
            {
                return;
            }
            _ = interfaceIT_7Segment_Display(Device.Session, new string(' ', 24), 1);
            Thread.Sleep(1000);
        }
    }

    private void BackgroundLED(bool status)
    {
        _ = interfaceIT_LED_Set(Device.Session, 209, status);
        _ = interfaceIT_LED_Set(Device.Session, 210, status);
        _ = interfaceIT_LED_Set(Device.Session, 211, status);
        _ = interfaceIT_LED_Set(Device.Session, 212, status);
        _ = interfaceIT_LED_Set(Device.Session, 213, status);
        _ = interfaceIT_LED_Set(Device.Session, 214, status);
        _ = interfaceIT_LED_Set(Device.Session, 215, status);
        _ = interfaceIT_LED_Set(Device.Session, 217, status);
        _ = interfaceIT_LED_Set(Device.Session, 218, status);
        _ = interfaceIT_LED_Set(Device.Session, 219, status);
        _ = interfaceIT_LED_Set(Device.Session, 220, status);
        _ = interfaceIT_LED_Set(Device.Session, 221, status);
        _ = interfaceIT_LED_Set(Device.Session, 222, status);
        _ = interfaceIT_LED_Set(Device.Session, 223, status);
        _ = interfaceIT_LED_Set(Device.Session, 233, status);
        _ = interfaceIT_LED_Set(Device.Session, 234, status);
        _ = interfaceIT_LED_Set(Device.Session, 235, status);
        _ = interfaceIT_LED_Set(Device.Session, 236, status);
        _ = interfaceIT_LED_Set(Device.Session, 237, status);
        _ = interfaceIT_LED_Set(Device.Session, 238, status);
        _ = interfaceIT_LED_Set(Device.Session, 239, status);
        _ = interfaceIT_LED_Set(Device.Session, 240, status);
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

    private bool _mCP_ATArmSw;
    private bool MCP_ATArmSw
    {
        set
        {
            if (_mCP_ATArmSw != value)
            {
                _ = interfaceIT_Dataline_Set(Device.Session, 5, _mCP_ATArmSw = value);
            }
        }
    }

    private bool _mCP_annunFD_0;
    private bool MCP_annunFD_0
    {
        set
        {
            if (_mCP_annunFD_0 != value)
            {
                _ = interfaceIT_LED_Set(Device.Session, 225, _mCP_annunFD_0 = value);
            }
        }
    }

    private bool _mCP_annunFD_1;
    private bool MCP_annunFD_1
    {
        set
        {
            if (_mCP_annunFD_1 != value)
            {
                _ = interfaceIT_LED_Set(Device.Session, 226, _mCP_annunFD_1 = value);
            }
        }
    }

    private bool _mCP_annunATArm;
    private bool MCP_annunATArm
    {
        set
        {
            if (_mCP_annunATArm != value)
            {
                _ = interfaceIT_LED_Set(Device.Session, 227, _mCP_annunATArm = value);
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
                _ = interfaceIT_LED_Set(Device.Session, 193, _mCP_annunN1 = value);
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
                _ = interfaceIT_LED_Set(Device.Session, 194, _mCP_annunSPEED = value);
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
                _ = interfaceIT_LED_Set(Device.Session, 195, _mCP_annunVNAV = value);
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
                _ = interfaceIT_LED_Set(Device.Session, 196, _mCP_annunLVL_CHG = value);
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
                _ = interfaceIT_LED_Set(Device.Session, 197, _mCP_annunHDG_SEL = value);
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
                _ = interfaceIT_LED_Set(Device.Session, 198, _mCP_annunLNAV = value);
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
                _ = interfaceIT_LED_Set(Device.Session, 199, _mCP_annunVOR_LOC = value);
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
                _ = interfaceIT_LED_Set(Device.Session, 201, _mCP_annunAPP = value);
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
                _ = interfaceIT_LED_Set(Device.Session, 202, _mCP_annunALT_HOLD = value);
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
                _ = interfaceIT_LED_Set(Device.Session, 203, _mCP_annunVS = value);
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
                _ = interfaceIT_LED_Set(Device.Session, 204, _mCP_annunCMD_A = value);
            }
        }
    }

    private bool _mCP_annunCWS_A;
    private bool MCP_annunCWS_A
    {
        set
        {
            if (_mCP_annunCWS_A != value)
            {
                _ = interfaceIT_LED_Set(Device.Session, 205, _mCP_annunCWS_A = value);
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
                _ = interfaceIT_LED_Set(Device.Session, 206, _mCP_annunCMD_B = value);
            }
        }
    }

    private bool _mCP_annunCWS_B;
    private bool MCP_annunCWS_B
    {
        set
        {
            if (_mCP_annunCWS_B != value)
            {
                _ = interfaceIT_LED_Set(Device.Session, 207, _mCP_annunCWS_B = value);
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
                pmdg737MCPIASOverspeedFlashingCancellationTokenSource?.Cancel();
                pmdg737MCPIASUnderspeedFlashingCancellationTokenSource?.Cancel();
                pmdg737MCPBlinkingCancellationTokenSource?.Cancel();
                _mCP_Altitude = null;
                _mCP_Course_0 = null;
                _mCP_Course_1 = null;
                _mCP_Heading = null;
                _mCP_IASMach = null;
                _mCP_VertSpeed = null;
                _ = interfaceIT_7Segment_Display(Device.Session, new string(' ', 24), 1);
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
                    _ = interfaceIT_7Segment_Display(Device.Session, new string(' ', 24), 1);
                    _mCP_Altitude = null;
                    _mCP_Course_0 = null;
                    _mCP_Course_1 = null;
                    _mCP_Heading = null;
                    _mCP_IASMach = null;
                    _mCP_VertSpeed = null;
                }
                if (value == 0)
                {
                    _ = interfaceIT_7Segment_Display(Device.Session, "888 888888888880-8880888", 1);
                    Task.Run(() => PMDG737MCPBlinking((pmdg737MCPBlinkingCancellationTokenSource = new()).Token));
                }
            }
        }
    }

    private float? _mCP_IASMach;
    private float? MCP_IASMach
    {
        get => _mCP_IASMach;
        set
        {
            if (_mCP_IASMach != value && MAIN_LightsSelector != 0)
            {
                _mCP_IASMach = value;
                if (value < 1)
                {
                    _ = interfaceIT_7Segment_Display(Device.Session, string.Format("{0,3}", value?.ToString("#.00", System.Globalization.CultureInfo.InvariantCulture)).TrimStart('.'), 7);
                    _ = interfaceIT_7Segment_Display(Device.Session, " ", 6);
                    _ = interfaceIT_LED_Set(Device.Session, 48, true);
                }
                if (value >= 100)
                {
                    _ = interfaceIT_7Segment_Display(Device.Session, string.Format("{0,3}", value?.ToString(System.Globalization.CultureInfo.InvariantCulture)), 6);
                }
            }
        }
    }

    private ushort? _mCP_Course_0;
    private ushort? MCP_Course_0
    {
        set
        {
            if (_mCP_Course_0 != value && MAIN_LightsSelector != 0)
            {
                _ = interfaceIT_7Segment_Display(Device.Session, (_mCP_Course_0 = value)?.ToString("D3"), 1);
            }
        }
    }

    private ushort? _mCP_Course_1;
    private ushort? MCP_Course_1
    {
        set
        {
            if (_mCP_Course_1 != value && MAIN_LightsSelector != 0)
            {
                _ = interfaceIT_7Segment_Display(Device.Session, (_mCP_Course_1 = value)?.ToString("D3"), 22);
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
                _ = interfaceIT_7Segment_Display(Device.Session, (_mCP_Heading = value)?.ToString("D3"), 9);
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
                    _ = interfaceIT_7Segment_Display(Device.Session, "   ", 6);
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
                    Task.Run(() => PMDG737MCPFlashing("B", (pmdg737MCPIASOverspeedFlashingCancellationTokenSource = new()).Token));
                    return;
                }
                pmdg737MCPIASOverspeedFlashingCancellationTokenSource?.Cancel();
            }
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
                    Task.Run(() => PMDG737MCPFlashing("A", (pmdg737MCPIASUnderspeedFlashingCancellationTokenSource = new()).Token));
                    return;
                }
                pmdg737MCPIASUnderspeedFlashingCancellationTokenSource?.Cancel();
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
                    _ = interfaceIT_7Segment_Display(Device.Session, "0000", 13);
                    return;
                }
                _ = interfaceIT_7Segment_Display(Device.Session, string.Format("{0,5}", value.ToString()), 12);
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
                    _ = interfaceIT_7Segment_Display(Device.Session, "     ", 17);
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
                    _ = interfaceIT_7Segment_Display(Device.Session, "     ", 17);
                }
                else if (value < 0) 
                {
                    _ = interfaceIT_7Segment_Display(Device.Session, "-" + string.Format("{0,4}", value?.ToString("D3").TrimStart('-')), 17);
                }
                else if (value > 0) 
                {
                    _ = interfaceIT_7Segment_Display(Device.Session, string.Format("{0,5}", value?.ToString("D3")), 17);
                }
            }
        }
    }

    public override void Stop()
    {
        base.Stop();
        pmdg737MCPBlinkingCancellationTokenSource?.Cancel();
        pmdg737MCPIASOverspeedFlashingCancellationTokenSource?.Cancel();
        pmdg737MCPIASUnderspeedFlashingCancellationTokenSource?.Cancel();
        pmdg737MCPLightCancellationTokenSource?.Cancel();
    }

    protected override void StartSimConnect()
    {
        base.StartSimConnect();
        PMDG737.RegisterPMDGDataEvents(simConnectClient.simConnect);
    }

    protected override void Simconnect_OnRecvClientData(SimConnect sender, SIMCONNECT_RECV_CLIENT_DATA data)
    {
        if (((uint)DATA_REQUEST_ID.DATA_REQUEST) == data.dwRequestID)
        {
            ELEC_BusPowered_2 = ((PMDG_NG3_Data)data.dwData[0]).ELEC_BusPowered[2];
            ELEC_BusPowered_7 = ((PMDG_NG3_Data)data.dwData[0]).ELEC_BusPowered[7];
            ELEC_BusPowered_15 = ((PMDG_NG3_Data)data.dwData[0]).ELEC_BusPowered[15];

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
                MCP_Heading = ((PMDG_NG3_Data)data.dwData[0]).MCP_Heading;
                MCP_Course_0 = ((PMDG_NG3_Data)data.dwData[0]).MCP_Course[0];
                MCP_Course_1 = ((PMDG_NG3_Data)data.dwData[0]).MCP_Course[1];
                MCP_Altitude = ((PMDG_NG3_Data)data.dwData[0]).MCP_Altitude;
                MCP_VertSpeed = ((PMDG_NG3_Data)data.dwData[0]).MCP_VertSpeed;
            }
        }
    }

    private uint eFIS_CPT_VOR_ADF_SELECTOR_R;
    private uint eFIS_CPT_VOR_ADF_SELECTOR_L;
    private uint nMCPCourse_0;

    protected override bool KeyPressedProc(int session, int key, int direction)
    {
        uint ndirection = (uint)direction;

        //1-0
        if (new int[] { 2, 3, 25, 69, 71, 79, 80 }.Contains(key))
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

            if (direction == 0 && (eFIS_CPT_VOR_ADF_SELECTOR_L != 0 || eFIS_CPT_VOR_ADF_SELECTOR_R != 0))
            {
                ndirection = 1;
            }
        }

        //-3 & -14
        if (new int[] { 4, 5, 6, 8, 9, 10, 12, 13, 14, 15, 17, 18, 19, 20, 21, 23, 24, 73, 74, 75, 76, 77, 78, 81, 82, 83, 84, 85, 86, 87 }.Contains(key))
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
            //Special for BARO & MINS +-10
            case 1:
                nMCPCourse_0 = ndirection;
                break;

            //0-1
            case 22:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_DISENGAGE_BAR, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            //1-0
            case 2:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_FD_SWITCH_L, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 3:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_AT_ARM_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 25:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_FD_SWITCH_R, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 69:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_VOR_ADF_SELECTOR_L, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                eFIS_CPT_VOR_ADF_SELECTOR_L = ndirection;
                break;

            case 71:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_VOR_ADF_SELECTOR_R, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                eFIS_CPT_VOR_ADF_SELECTOR_R = ndirection;
                break;

            case 79:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS_RADIO_BARO, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 80:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO_IN_HPA, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            //1-2
            case 70:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_VOR_ADF_SELECTOR_L, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 72:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_VOR_ADF_SELECTOR_R, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            //0-...
            case 26 when direction == 1:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR, 0, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 27 when direction == 1:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR, 1, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 28 when direction == 1:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR, 2, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 29 when direction == 1:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR, 3, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 30 when direction == 1:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR, 4, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 65 when direction == 1:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MODE, 0, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 66 when direction == 1:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MODE, 1, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 67 when direction == 1:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MODE, 2, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 68 when direction == 1:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MODE, 3, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 89 when direction == 1:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 0, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 90 when direction == 1:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 1, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 91 when direction == 1:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 2, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 92 when direction == 1:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 3, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 93 when direction == 1:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 4, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 94 when direction == 1:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 5, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 95 when direction == 1:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 6, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 96 when direction == 1:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE, 7, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            //-3 & -14
            case 4:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_N1_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 5:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_SPEED_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 6:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_CO_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 8:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_SPD_INTV_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 9:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_VNAV_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 10:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_LVL_CHG_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 12:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_HDG_SEL_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 13:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_LNAV_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 14:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_VOR_LOC_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 15:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_APP_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 17:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_ALT_HOLD_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 18:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_ALT_INTV_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 19:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_VS_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 20:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_CMD_A_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 21:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_CWS_A_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 23:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_CMD_B_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 24:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_CWS_B_SWITCH, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 73:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_FPV, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 74:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MTRS, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 75:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MODE_CTR, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 76:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE_TFC, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 77:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS_RST, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 78:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO_STD, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 81:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_WXR, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 82:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_STA, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 83:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_WPT, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 84:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_ARPT, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 85:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_DATA, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 86:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_POS, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 87:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_TERR, ndirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            //-18 & -17
            case 33 when direction == 1:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_COURSE_SELECTOR_L, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 34 when direction == 1:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_COURSE_SELECTOR_L, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 35 when direction == 1:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_SPEED_SELECTOR, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 36 when direction == 1:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_SPEED_SELECTOR, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 37 when direction == 1:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_HEADING_SELECTOR, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 38 when direction == 1:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_HEADING_SELECTOR, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 41 when direction == 1:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_ALTITUDE_SELECTOR, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 42 when direction == 1:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_ALTITUDE_SELECTOR, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 43 when direction == 1:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_VS_SELECTOR, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 44 when direction == 1:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_VS_SELECTOR, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 45 when direction == 1:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_COURSE_SELECTOR_R, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            case 46 when direction == 1:
                simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_MCP_COURSE_SELECTOR_R, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                break;

            //-18 & -17 (-1 & -3)
            case 57 when direction == 1:
                if (nMCPCourse_0 == 0)
                {
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                }
                if (nMCPCourse_0 == 1)
                {
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                }
                break;

            case 58 when direction == 1:
                if (nMCPCourse_0 == 0)
                {
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                }
                if (nMCPCourse_0 == 1)
                {
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_MINIMUMS, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                }
                break;

            case 59 when direction == 1:
                if (nMCPCourse_0 == 0)
                {
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_WHEEL_DOWN, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                }
                if (nMCPCourse_0 == 1)
                {
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_LEFTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                }
                break;

            case 60 when direction == 1:
                if (nMCPCourse_0 == 0)
                {
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_WHEEL_UP, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                }
                if (nMCPCourse_0 == 1)
                {
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_EFIS_CPT_BARO, MOUSE_FLAG_RIGHTRELEASE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                }
                break;

            default:
                break;
        }
        return true;
    }
}