using System;
using static Device_Interface_Manager.SimConnectProfiles.PMDG.PMDG_777X_SDK;

namespace Device_Interface_Manager.SimConnectProfiles.PMDG.B777.E;

public class B777_CDU_C : CDU_Base_E<PMDG_777X_Data>
{
    protected override string SettingsLocation => @"Profiles\PMDG 777\ENET CDU Screen C.json";

    protected override PMDG.DATA_REQUEST_ID CDU_ID => PMDG.DATA_REQUEST_ID.CDU2_REQUEST;

    protected override Enum CDUBrightnessButton => PMDGEvents.EVT_CDU_C_BRITENESS;

    protected override void RecvData()
    {
        CDU_annunEXEC = PMDG_Data.CDU_annunEXEC[2];
        CDU_annunCALL = PMDG_Data.CDU_annunDSPY[2];
        CDU_annunMSG = PMDG_Data.CDU_annunMSG[2];
        CDU_annunFAIL = PMDG_Data.CDU_annunFAIL[2];
        CDU_annunOFST = PMDG_Data.CDU_annunOFST[2];
    }

    protected override void KeyPressedAction(int key, uint direction)
    {
        switch (direction)
        {
            case 0:
                if (key == 8)
                {
                    direction = MOUSE_FLAG_LEFTRELEASE;
                    break;
                }
                if (key == 63 || key == 64)
                {
                    simConnectClient.TransmitEvent(1, PMDGEvents.EVT_CDU_C_BRITENESS);
                    return;
                }
                return;

            case 1:
                direction = MOUSE_FLAG_LEFTSINGLE;
                break;
        }

        switch (key)
        {
            case 1:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_DOT);
                break;

            case 2:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_0);
                break;

            case 3:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_PLUS_MINUS);
                break;

            case 4:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_Z);
                break;

            case 5:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_SPACE);
                break;

            case 6:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_DEL);
                break;

            case 7:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_SLASH);
                break;

            case 8:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_CLR);
                break;

            case 9:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_7);
                break;

            case 10:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_8);
                break;

            case 11:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_9);
                break;

            case 12:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_U);
                break;

            case 13:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_V);
                break;

            case 14:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_W);
                break;

            case 15:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_X);
                break;

            case 16:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_Y);
                break;

            case 17:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_4);
                break;

            case 18:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_5);
                break;

            case 19:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_6);
                break;

            case 20:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_P);
                break;

            case 21:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_Q);
                break;

            case 22:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_R);
                break;

            case 23:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_S);
                break;

            case 24:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_T);
                break;

            case 25:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_1);
                break;

            case 26:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_2);
                break;

            case 27:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_3);
                break;

            case 28:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_K);
                break;

            case 29:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_L);
                break;

            case 30:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_M);
                break;

            case 31:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_N);
                break;

            case 32:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_O);
                break;

            case 33:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_PREV_PAGE);
                break;

            case 34:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_NEXT_PAGE);
                break;

            case 35:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_L1);
                break;

            case 36:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_F);
                break;

            case 37:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_G);
                break;

            case 38:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_H);
                break;

            case 39:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_I);
                break;

            case 40:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_J);
                break;

            case 41:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_MENU);
                break;

            case 42:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_NAV_RAD);
                break;

            case 43:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_L2);
                break;

            case 44:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_A);
                break;

            case 45:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_B);
                break;

            case 46:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_C);
                break;

            case 47:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_D);
                break;

            case 48:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_E);
                break;

            case 49:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_FIX);
                break;

            case 50:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_LEGS);
                break;

            case 51:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_L3);
                break;

            case 52:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_HOLD);
                break;

            case 53:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_FMCCOMM);
                break;

            case 54:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_PROG);
                break;

            case 55:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_EXEC);
                break;

            case 56:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_L5);
                break;

            case 57:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_INIT_REF);
                break;

            case 58:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_RTE);
                break;

            case 59:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_L4);
                break;

            case 60:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_DEP_ARR);
                break;

            case 61:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_ALTN);
                break;

            case 62:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_VNAV);
                break;

            case 63:
                simConnectClient.TransmitEvent(2, PMDGEvents.EVT_CDU_C_BRITENESS);
                break;

            case 64:
                simConnectClient.TransmitEvent(0, PMDGEvents.EVT_CDU_C_BRITENESS);
                break;

            case 65:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_R1);
                break;

            case 66:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_R2);
                break;

            case 67:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_R3);
                break;

            case 68:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_R4);
                break;

            case 69:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_R5);
                break;

            case 70:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_R6);
                break;

            case 71:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_C_L6);
                break;
        }
    }
}