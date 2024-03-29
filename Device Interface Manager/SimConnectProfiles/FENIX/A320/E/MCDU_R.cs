﻿namespace Device_Interface_Manager.SimConnectProfiles.FENIX.A320.E;

public class MCDU_R : ENET
{
    protected override void OnRecvOpen()
    {
        simConnectClient.RegisterSimVar("L:I_CDU2_FAIL");
        simConnectClient.RegisterSimVar("L:I_CDU2_MCDU_MENU");
        simConnectClient.RegisterSimVar("L:I_CDU2_FM");
        simConnectClient.RegisterSimVar("L:I_CDU2_FM1");
        simConnectClient.RegisterSimVar("L:I_CDU2_IND");
        simConnectClient.RegisterSimVar("L:I_CDU2_RDY");
        simConnectClient.RegisterSimVar("L:I_CDU2_DASH");
        simConnectClient.RegisterSimVar("L:I_CDU2_FM2");
    }

    protected override void SimConnectClient_OnSimVarChanged(object sender, SimConnectClient.SimVar simVar)
    {
        base.SimConnectClient_OnSimVarChanged(sender, simVar);

        switch (simVar.Name)
        {
            case "L:I_CDU2_FAIL":
                Device.SendinterfaceITEthernetLED(2, simVar.Data);
                break;

            case "L:I_CDU2_MCDU_MENU":
                Device.SendinterfaceITEthernetLED(3, simVar.Data);
                Device.SendinterfaceITEthernetLED(5, simVar.Data);
                break;

            case "L:I_CDU2_FM":
                Device.SendinterfaceITEthernetLED(4, simVar.Data);
                break;

            case "L:I_CDU2_FM1":
                Device.SendinterfaceITEthernetLED(6, simVar.Data);
                break;

            case "L:I_CDU2_IND":
                Device.SendinterfaceITEthernetLED(7, simVar.Data);
                break;

            case "L:I_CDU2_RDY":
                Device.SendinterfaceITEthernetLED(8, simVar.Data);
                break;

            case "L:I_CDU2_DASH":
                Device.SendinterfaceITEthernetLED(9, simVar.Data);
                break;

            case "L:I_CDU2_FM2":
                Device.SendinterfaceITEthernetLED(10, simVar.Data);
                break;

            default:
                break;
        }
    }

    protected override void KeyPressedAction(int key, uint direction)
    {
        switch (key)
        {
            case 1:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_DOT");
                break;

            case 2:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_0");
                break;

            case 3:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_MINUS");
                break;

            case 4:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_Z");
                break;

            case 5:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_SLASH");
                break;

            case 6:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_SPACE");
                break;

            case 7:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_OVFLY");
                break;

            case 8:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_CLEAR");
                break;

            case 9:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_7");
                break;

            case 10:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_8");
                break;

            case 11:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_9");
                break;

            case 12:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_U");
                break;

            case 13:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_V");
                break;

            case 14:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_W");
                break;

            case 15:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_X");
                break;

            case 16:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_Y");
                break;

            case 17:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_4");
                break;

            case 18:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_5");
                break;

            case 19:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_6");
                break;

            case 20:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_P");
                break;

            case 21:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_Q");
                break;

            case 22:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_R");
                break;

            case 23:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_S");
                break;

            case 24:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_T");
                break;

            case 25:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_1");
                break;

            case 26:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_2");
                break;

            case 27:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_3");
                break;

            case 28:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_K");
                break;

            case 29:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_L");
                break;

            case 30:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_M");
                break;

            case 31:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_N");
                break;

            case 32:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_O");
                break;

            case 33:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_ARROW_LEFT");
                break;

            case 34:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_ARROW_UP");
                break;

            case 35:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_LSK1L");
                break;

            case 36:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_F");
                break;

            case 37:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_G");
                break;

            case 38:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_H");
                break;

            case 39:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_I");
                break;

            case 40:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_J");
                break;

            case 41:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_AIRPORT");
                break;

            case 42:

                break;

            case 43:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_LSK2L");
                break;

            case 44:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_A");
                break;

            case 45:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_B");
                break;

            case 46:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_C");
                break;

            case 47:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_D");
                break;

            case 48:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_E");
                break;

            case 49:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_FPLN");
                break;

            case 50:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_RAD_NAV");
                break;

            case 51:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_LSK3L");
                break;

            case 52:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_FUEL_PRED");
                break;

            case 53:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_SEC_FPLN");
                break;

            case 54:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_ATC_COM");
                break;

            case 55:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_MENU");
                break;

            case 56:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_LSK5L");
                break;

            case 57:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_DIR");
                break;

            case 58:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_PROG");
                break;

            case 59:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_LSK4L");
                break;

            case 60:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_PERF");
                break;

            case 61:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_INIT");
                break;

            case 62:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_DATA");
                break;

            case 63:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_BRIGHTNESS_DOWN");
                break;

            case 64:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_BRIGHTNESS_UP");
                break;

            case 65:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_LSK1R");
                break;

            case 66:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_LSK2R");
                break;

            case 67:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_LSK3R");
                break;

            case 68:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_LSK4R");
                break;

            case 69:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_LSK5R");
                break;

            case 70:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_LSK6R");
                break;

            case 71:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_LSK6L");
                break;

            case 72:

                break;

            case 73:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_ARROW_RIGHT");
                break;

            case 74:
                simConnectClient.SetSimVar(direction, "L:S_CDU2_KEY_ARROW_DOWN");
                break;

            default:
                break;
        }
    }
}