using System;
using System.Linq;
using static Device_Interface_Manager.MSFSProfiles.PMDG.PMDG_NG3_SDK;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceITAPI_Data;

namespace Device_Interface_Manager.MSFSProfiles.PMDG.B737.USB;
public class NG_MCP_330A_332C : NG_MCP_3311
{
    public NG_MCP_330A_332C()
    {
        MCP_ATArmSwPos = 16;

        MCP_annunN1Pos = 194;
        MCP_annunSPEEDPos = 196;
        MCP_annunCMD_BPos = 198;
        MCP_annunCMD_APos = 200;
        MCP_annunVSPos = 202;
        MCP_annunALT_HOLDPos = 204;
        MCP_annunAPPPos = 206;
        MCP_annunVOR_LOCPos = 208;
        MCP_annunHDG_SELPos = 210;
        MCP_annunVNAVPos = 212;
        MCP_annunLNAVPos = 214;
        MCP_annunLVL_CHGPos = 216;
        MCP_annunCWS_APos = 218;
        MCP_annunCWS_BPos = 220;
        MCP_annunFD_0Pos = 227;
        MCP_annunFD_1Pos = 229;
        MCP_annunATArmPos = 231;

        BackgroundLEDs = Array.Empty<int>();
    }

    private readonly int[] backgroundButtonLEDs = new int[] { 193, 195, 197, 199, 201, 203, 205, 207, 209, 211, 213, 215, 217, 219 };

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
        else if (new int[] { 6, 8, 9, 10, 12, 13, 14, 15, 17, 18, 19, 20, 21, 22, 37, 38 }.Contains(key))
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
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_N1_SWITCH);
                break;

            case 7:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_SPEED_SWITCH);
                break;

            case 8:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_CMD_B_SWITCH);
                break;

            case 9:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_CMD_A_SWITCH);
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
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_VOR_LOC_SWITCH);
                break;

            case 14:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_HDG_SEL_SWITCH);
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
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_CWS_A_SWITCH);
                break;

            //747
            case 19 when Device.DeviceInfo.DatalineCount == 2:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_SPD_INTV_SWITCH);
                break;

            case 19:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_CWS_B_SWITCH);
                break;

            case 20:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_ALT_INTV_SWITCH);
                break;

            case 21:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_CO_SWITCH);
                break;

            //747
            case 22 when Device.DeviceInfo.DatalineCount == 2:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_CWS_B_SWITCH);
                break;

            case 22:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_SPD_INTV_SWITCH);
                break;

            case 37:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_CO_SWITCH);
                break;

            case 38:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_MCP_SPD_INTV_SWITCH);
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

            //SwannSim
            case 51 when direction == 1 && Device.SerialNumber == "03EA00000FE3":
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_HEADING_SELECTOR);
                break;

            case 52 when direction == 1 && Device.SerialNumber == "03EA00000FE3":
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_HEADING_SELECTOR);
                break;
            //

            case 51 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_HEADING_SELECTOR);
                break;

            case 52 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_HEADING_SELECTOR);
                break;

            //SwannSim
            case 53 when direction == 1 && Device.SerialNumber == "03EA00000FE3":
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_ALTITUDE_SELECTOR);
                break;

            case 54 when direction == 1 && Device.SerialNumber == "03EA00000FE3":
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_ALTITUDE_SELECTOR);
                break;
            //

            case 53 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_ALTITUDE_SELECTOR);
                break;

            case 54 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_ALTITUDE_SELECTOR);
                break;

            //330A
            case 55 when direction == 1 && Device.SerialNumber == "03EA00000FE3":
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_VS_SELECTOR);
                break;

            case 56 when direction == 1 && Device.SerialNumber == "03EA00000FE3":
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_VS_SELECTOR);
                break;
            //

            case 55 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_VS_SELECTOR);
                break;

            case 56 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_VS_SELECTOR);
                break;

            case 57 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_COURSE_SELECTOR_R);
                break;

            case 58 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_COURSE_SELECTOR_R);
                break;

            //SwannSim
            case 59 when direction == 1 && Device.SerialNumber == "03EA00000FE3":
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_COURSE_SELECTOR_L);
                break;

            case 60 when direction == 1 && Device.SerialNumber == "03EA00000FE3":
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_COURSE_SELECTOR_L);
                break;
            //

            case 59 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, PMDGEvents.EVT_MCP_COURSE_SELECTOR_L);
                break;

            case 60 when direction == 1:
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, PMDGEvents.EVT_MCP_COURSE_SELECTOR_L);
                break;
        }
    }
}