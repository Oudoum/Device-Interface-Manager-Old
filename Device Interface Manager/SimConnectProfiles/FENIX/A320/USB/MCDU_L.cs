using Microsoft.FlightSimulator.SimConnect;
using static Device_Interface_Manager.Devices.interfaceIT.USB.InterfaceITAPI_Data;

namespace Device_Interface_Manager.SimConnectProfiles.FENIX.A320.USB;

public class MCDU_L : Device_Interface_Manager.SimConnectProfiles.USB
{
    protected override void SimConnect_OnRecvOpen()
    {
        interfaceIT_Dataline_Set(Device.Session, Device.DeviceInfo.DatalineFirst, true);

        simConnectClient.RegisterSimVar("L:I_CDU1_FAIL");
        simConnectClient.RegisterSimVar("L:I_CDU1_MCDU_MENU");
        simConnectClient.RegisterSimVar("L:I_CDU1_FM");
        simConnectClient.RegisterSimVar("L:I_CDU1_FM1");
        simConnectClient.RegisterSimVar("L:I_CDU1_IND");
        simConnectClient.RegisterSimVar("L:I_CDU1_RDY");
        simConnectClient.RegisterSimVar("L:I_CDU1_DASH");
        simConnectClient.RegisterSimVar("L:I_CDU1_FM2");

        simConnectClient.RegisterSimVar("L:N_PED_LIGHTING_PEDESTAL");
    }

    protected override void SimConnectClient_OnSimVarChanged(object sender, SimConnectClient.SimVar simVar)
    {
        base.SimConnectClient_OnSimVarChanged(sender, simVar);

        switch (simVar.Name)
        {
            case "L:I_CDU1_FAIL":
                interfaceIT_LED_Set(Device.Session, 2, simVar.BData());
                break;

            case "L:I_CDU1_MCDU_MENU":
                bool value = simVar.BData();
                interfaceIT_LED_Set(Device.Session, 3, value);
                interfaceIT_LED_Set(Device.Session, 5, value);
                break;

            case "L:I_CDU1_FM":
                interfaceIT_LED_Set(Device.Session, 4, simVar.BData());
                break;

            case "L:I_CDU1_FM1":
                interfaceIT_LED_Set(Device.Session, 9, simVar.BData());
                break;

            case "L:I_CDU1_IND":
                interfaceIT_LED_Set(Device.Session, 10, simVar.BData());
                break;

            case "L:I_CDU1_RDY":
                interfaceIT_LED_Set(Device.Session, 11, simVar.BData());
                break;

            case "L:I_CDU1_DASH":
                interfaceIT_LED_Set(Device.Session, 12, simVar.BData());
                break;

            case "L:I_CDU1_FM2":
                interfaceIT_LED_Set(Device.Session, 13, simVar.BData());
                break;

            case "L:N_PED_LIGHTING_PEDESTAL":
                interfaceIT_Brightness_Set(Device.Session, (int)(simVar.Data * 50 * 2.55));
                break;

            default:
                break;
        }
    }

    protected override void KeyPressedProc(uint session, int key, uint direction)
    {
        switch (key)
        {
            case 1:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_MENU");
                break;

            case 2:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_ATC_COM");
                break;

            case 3:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_SEC_FPLN");
                break;

            case 4:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_INIT");
                break;

            case 5:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_FUEL_PRED");
                break;

            case 6:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_RAD_NAV");
                break;

            case 7:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_FPLN");
                break;

            case 8:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_PERF");
                break;

            case 9:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_E");
                break;

            case 10:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_D");
                break;

            case 11:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_C");
                break;

            case 12:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_B");
                break;

            case 13:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_A");
                break;

            case 14:

                break;

            case 15:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_AIRPORT");
                break;

            case 16:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_PROG");
                break;

            case 17:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_J");
                break;

            case 18:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_I");
                break;

            case 19:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_H");
                break;

            case 20:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_G");
                break;

            case 21:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_F");
                break;

            case 22:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_ARROW_UP");
                break;

            case 23:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_ARROW_LEFT");
                break;

            case 24:

                break;

            case 25:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_O");
                break;

            case 26:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_N");
                break;

            case 27:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_M");
                break;

            case 28:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_L");
                break;

            case 29:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_K");
                break;

            case 30:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_3");
                break;

            case 31:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_2");
                break;

            case 32:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_1");
                break;

            case 33:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_T");
                break;

            case 34:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_S");
                break;

            case 35:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_R");
                break;

            case 36:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_Q");
                break;

            case 37:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_P");
                break;

            case 38:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_6");
                break;

            case 39:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_5");
                break;

            case 40:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_4");
                break;

            case 41:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_Y");
                break;

            case 42:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_X");
                break;

            case 43:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_W");
                break;

            case 44:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_V");
                break;

            case 45:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_U");
                break;

            case 46:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_9");
                break;

            case 47:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_8");
                break;

            case 48:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_7");
                break;

            case 49:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_CLEAR");
                break;

            case 50:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_OVFLY");
                break;

            case 51:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_SPACE");
                break;

            case 52:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_SLASH");
                break;

            case 53:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_Z");
                break;

            case 54:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_MINUS");
                break;

            case 55:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_0");
                break;

            case 56:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_DOT");
                break;

            case 57:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_LSK1L");
                break;

            case 58:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_LSK2L");
                break;

            case 59:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_LSK3L");
                break;

            case 60:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_LSK4L");
                break;

            case 61:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_LSK5L");
                break;

            case 62:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_LSK6L");
                break;

            case 63:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_DIR");
                break;

            case 64:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_LSK1R");
                break;

            case 65:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_LSK2R");
                break;

            case 66:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_LSK3R");
                break;

            case 67:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_LSK4R");
                break;

            case 68:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_LSK5R");
                break;

            case 69:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_LSK6R");
                break;

            case 70:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_DATA");
                break;

            case 71:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_ARROW_DOWN");
                break;

            case 72:
                simConnectClient.SetSimVar(direction, "L:S_CDU1_KEY_ARROW_RIGHT");
                break;

            default:
                break;
        }
    }
}