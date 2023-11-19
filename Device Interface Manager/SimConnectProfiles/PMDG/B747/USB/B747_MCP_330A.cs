using System;
using System.Linq;
using static Device_Interface_Manager.SimConnectProfiles.PMDG.PMDG_747QOTSII_SDK;
using static Device_Interface_Manager.Devices.interfaceIT.USB.InterfaceITAPI_Data;

namespace Device_Interface_Manager.SimConnectProfiles.PMDG.B747.USB;
public class B747_MCP_330A : B747_MCP_3329
{
    public B747_MCP_330A()
    {
        MCP_annunTHRPos = 194;
        MCP_annunSPDPos = 196;
        MCP_annunCMD_CPos = 198;
        MCP_annunCMD_LPos = 200;
        MCP_annunVSPos = 202;
        MCP_annunALT_HOLDPos = 204;
        MCP_annunAPPPos = 206;
        MCP_annunLOCPos = 208;
        MCP_annunHDG_HOLDPos = 210;
        MCP_annunVNAVPos = 212;
        MCP_annunLNAVPos = 214;
        MCP_annunFLCHPos = 216;
        MCP_annunCMD_RPos = 218;

        BackgroundLEDs = Array.Empty<int>();
    }


    private readonly int[] backgroundButtonLEDs = new int[] { 193, 195, 197, 199, 201, 203, 205, 207, 209, 211, 213, 215, 217 };

    protected override void BackgroundButtonLED(bool status)
    {
        foreach (int ledNumber in backgroundButtonLEDs)
        {
            interfaceIT_LED_Set(Device.Session, ledNumber, status);
        }
    }

    protected override void KeyPressedProc(uint session, int key, uint direction)
    {
        //1-0
        if (new int[] { 2, 3, 4 }.Contains(key))
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

        //-3 & -14
        else if (new int[] { 6, 8, 9, 10, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 36, 37, 38, 39, 40, 41, 42, 43, 47, 48 }.Contains(key))
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
            //0-1
            case 1:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_DISENGAGE_BAR);
                break;

            //1-0
            case 2:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_FD_SWITCH_L);
                break;

            case 3:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_FD_SWITCH_R);
                break;

            case 4:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_AT_ARM_SWITCH);
                break;

            //-3 & -14
            case 6:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_THR_SWITCH);
                break;

            case 7:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_THR_SWITCH);
                break;

            case 8:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_AP_C_SWITCH);
                break;

            case 9:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_AP_L_SWITCH);
                break;

            case 10:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_VS_SWITCH);
                break;

            case 11:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_ALT_HOLD_SWITCH);
                break;

            case 12:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_APP_SWITCH);
                break;

            case 13:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_LOC_SWITCH);
                break;

            case 14:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_HDG_HOLD_SWITCH);
                break;

            case 15:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_VNAV_SWITCH);
                break;

            case 16:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_LNAV_SWITCH);
                break;

            case 17:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_LVL_CHG_SWITCH);
                break;

            case 18:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_AP_R_SWITCH);
                break;

            case 19:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_SPEED_PUSH_SWITCH);
                break;

            case 20:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_ALTITUDE_PUSH_SWITCH);
                break;

            case 21:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_IAS_MACH_SWITCH);
                break;

            case 22:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_HDG_HOLD_SWITCH);
                break;

            case 36:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_DSP_ENG_SWITCH);
                break;

            case 37:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_DSP_STAT_SWITCH);
                break;

            case 38:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_DSP_ELEC_SWITCH);
                break;

            case 39:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_DSP_HYD_SWITCH);
                break;

            case 40:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_DSP_FUEL_SWITCH);
                break;

            case 41:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_DSP_AIR_SWITCH);
                break;

            case 42:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_DSP_DOOR_SWITCH);
                break;

            case 43:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_DSP_GEAR_SWITCH);
                break;

            case 47:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_DSP_CANC_SWITCH);
                break;

            case 48:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_DSP_RCL_SWITCH);
                break;

            //0-...
            case 25 when direction == 1:
                simConnectClient.TransmitEvent(0, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR);
                break;

            case 26 when direction == 1:
                simConnectClient.TransmitEvent(1, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR);
                break;

            case 27 when direction == 1:
                simConnectClient.TransmitEvent(2, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR);
                break;

            case 28 when direction == 1:
                simConnectClient.TransmitEvent(3, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR);
                break;

            case 29 when direction == 1:
                simConnectClient.TransmitEvent(4, PMDGEvents.EVT_MCP_BANK_ANGLE_SELECTOR);
                break;

            //-18 & -17
            case 49 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_SPEED_SELECTOR);
                break;

            case 50 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_SPEED_SELECTOR);
                break;

            case 51 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_HEADING_SELECTOR);
                break;

            case 52 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_HEADING_SELECTOR);
                break;

            case 53 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_ALTITUDE_SELECTOR);
                break;

            case 54 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_ALTITUDE_SELECTOR);
                break;

            case 55 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_VS_SELECTOR);
                break;

            case 56 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_VS_SELECTOR);
                break;
        }
    }
}