using System;
using System.Linq;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceITAPI_Data;
using static Device_Interface_Manager.MSFSProfiles.PMDG.PMDG_NG3_SDK;

namespace Device_Interface_Manager.MSFSProfiles.PMDG.B737.USB;

public class NG_MCP_777 : NG_MCP_3311
{
    public NG_MCP_777()
    {
        MCP_ATArmSwPos = 0;

        MCP_annunN1Pos = 194;
        MCP_annunSPEEDPos = 146;
        MCP_annunCMD_BPos = 155;
        MCP_annunCMD_APos = 145;
        MCP_annunVSPos = 151;
        MCP_annunALT_HOLDPos = 152;
        MCP_annunAPPPos = 154;
        MCP_annunVOR_LOCPos = 153;
        MCP_annunHDG_SELPos = 150;
        MCP_annunVNAVPos = 148;
        MCP_annunLNAVPos = 147;
        MCP_annunLVL_CHGPos = 149;
        MCP_annunCWS_APos = 0;
        MCP_annunCWS_BPos = 0;
        MCP_annunFD_0Pos = 0;
        MCP_annunFD_1Pos = 0;
        MCP_annunATArmPos = 0;

        MCP_IASMachStartPos = 1;
        MCPSevenSegmentTest = "888 - 888088888880";
        MCPMachSevenSegmentDot = 8;
        MCP_VertSpeedStartPos = 4;

        BackgroundLEDs = Array.Empty<int>();
    }

    private readonly int[] backgroundButtonLEDs = new int[] { 129, 130, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 177, 178, 179, 180 };

    protected override void BackgroundButtonLED(bool status)
    {
        foreach (int ledNumber in backgroundButtonLEDs)
        {
            interfaceIT_LED_Set(Device.Session, ledNumber, status);
        }
    }

    protected override void SetIAS(bool value)
    {
        interfaceIT_LED_Set(Device.Session, 185, value);
    }

    protected override void SetMACH(bool value)
    {
        interfaceIT_LED_Set(Device.Session, 186, value);
    }

    protected override void SetHeadingAltitude(ushort? heading, ushort? altitude)
    {
        if (hdg_rotary_push == 0)
        {
            MCP_Heading = heading;
            heading_Course0 = MCP_Course_0;
        }
        if (hdg_rotary_push == 1)
        {
            MCP_Heading = MCP_Course_0;
            heading_Course0 = heading;
        }
        if (alt_rotary_push == 0)
        {
            MCP_Altitude = altitude;
            altitude_Course1 = MCP_Course_1;
        }
        if (alt_rotary_push == 1)
        {
            MCP_Altitude = MCP_Course_1;
            altitude_Course1 = altitude;
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
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_DISENGAGE_BAR);
                break;

            //1-0
            case 17:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_FD_SWITCH_L);
                break;

            case 18:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_AT_ARM_SWITCH);
                break;

            case 24:
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

            case 81:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_MINIMUMS_RADIO_BARO);
                break;

            case 82:
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
            case 58 when direction == 1:
                simConnectClient.TransmitEvent(0, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR);
                break;

            case 59 when direction == 1:
                simConnectClient.TransmitEvent(1, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR);
                break;

            case 60 when direction == 1:
                simConnectClient.TransmitEvent(2, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR);
                break;

            case 61 when direction == 1:
                simConnectClient.TransmitEvent(3, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR);
                break;

            case 62 when direction == 1:
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

            case 73 when direction == 1:
                simConnectClient.TransmitEvent(0, PMDGEvents.EVT_EFIS_CPT_RANGE);
                break;

            case 74 when direction == 1:
                simConnectClient.TransmitEvent(1, PMDGEvents.EVT_EFIS_CPT_RANGE);
                break;

            case 75 when direction == 1:
                simConnectClient.TransmitEvent(2, PMDGEvents.EVT_EFIS_CPT_RANGE);
                break;

            case 76 when direction == 1:
                simConnectClient.TransmitEvent(3, PMDGEvents.EVT_EFIS_CPT_RANGE);
                break;

            case 77 when direction == 1:
                simConnectClient.TransmitEvent(4, PMDGEvents.EVT_EFIS_CPT_RANGE);
                break;

            case 78 when direction == 1:
                simConnectClient.TransmitEvent(5, PMDGEvents.EVT_EFIS_CPT_RANGE);
                break;

            case 79 when direction == 1:
                simConnectClient.TransmitEvent(6, PMDGEvents.EVT_EFIS_CPT_RANGE);
                break;

            //-3 & -14
            case 20:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_CO_SWITCH);
                break;

            case 22:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_SPD_INTV_SWITCH);
                break;

            case 23:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_ALT_INTV_SWITCH);
                break;

            case 25:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_CMD_A_SWITCH);
                break;

            case 26:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_N1_SWITCH);
                break;

            case 27:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_SPEED_SWITCH);
                break;

            case 28:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_LNAV_SWITCH);
                break;

            case 29:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_VNAV_SWITCH);
                break;

            case 30:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_LVL_CHG_SWITCH);
                break;

            case 31:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_HDG_SEL_SWITCH);
                break;

            case 32:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_VS_SWITCH);
                break;

            case 33:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_ALT_HOLD_SWITCH);
                break;

            case 34:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_VOR_LOC_SWITCH);
                break;

            case 35:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_APP_SWITCH);
                break;

            case 83:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_FPV);
                break;

            case 84:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_MTRS);
                break;

            case 85:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_MINIMUMS_RST);
                break;

            case 86:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_BARO_STD);
                break;

            case 87:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_MODE_CTR);
                break;

            case 88:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_RANGE_TFC);
                break;

            case 89:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_WXR);
                break;

            case 90:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_STA);
                break;

            case 91:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_WPT);
                break;

            case 92:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_ARPT);
                break;

            case 93:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_DATA);
                break;

            case 94:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_POS);
                break;

            case 95:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_CPT_TERR);
                break;

            //-18 & -17
            case 9 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_VS_SELECTOR);
                break;

            case 10 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_VS_SELECTOR);
                break;

            case 11 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_ALTITUDE_SELECTOR);
                break;

            case 12 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_ALTITUDE_SELECTOR);
                break;

            //-18 & -17 SPECIAL
            case 5 when direction == 1:
                if (hdg_rotary_push == 0)
                {
                    simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_SPEED_SELECTOR);
                }
                if (hdg_rotary_push == 1)
                {
                    simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_COURSE_SELECTOR_L);
                }
                break;

            case 6 when direction == 1:
                if (hdg_rotary_push == 0)
                {
                    simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_SPEED_SELECTOR);
                }
                if (hdg_rotary_push == 1)
                {
                    simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_COURSE_SELECTOR_L);
                }
                break;

            case 7 when direction == 1:
                if (alt_rotary_push == 0)
                {
                    simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_HEADING_SELECTOR);
                }
                if (alt_rotary_push == 1)
                {
                    simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_COURSE_SELECTOR_R);
                }
                break;

            case 8 when direction == 1:
                if (alt_rotary_push == 0)
                {
                    simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_HEADING_SELECTOR);
                }
                if (alt_rotary_push == 1)
                {
                    simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_COURSE_SELECTOR_R);
                }
                break;

            //-18 & -17 (-1 & -3)
            case 1 when direction == 1:
                TransmitBAROMINSTenTimes(PMDGEvents.EVT_EFIS_CPT_MINIMUMS, false, speed_rotary_push);
                break;

            case 2 when direction == 1:
                TransmitBAROMINSTenTimes(PMDGEvents.EVT_EFIS_CPT_MINIMUMS, true, speed_rotary_push);
                break;

            case 3 when direction == 1:
                TransmitBAROMINSTenTimes(PMDGEvents.EVT_EFIS_CPT_BARO, false, speed_rotary_push);
                break;

            case 4 when direction == 1:
                TransmitBAROMINSTenTimes(PMDGEvents.EVT_EFIS_CPT_BARO, true, speed_rotary_push);
                break;
        }
    }
}