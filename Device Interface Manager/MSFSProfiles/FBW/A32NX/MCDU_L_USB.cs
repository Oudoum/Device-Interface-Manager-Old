using System;
using System.IO;
using System.Text.Json;
using Microsoft.FlightSimulator.SimConnect;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceITAPI_Data;

namespace Device_Interface_Manager.MSFSProfiles.FBW.A32NX;

public class MCDU_L_USB : USB
{
    public MCDU_L_USB()
    {
        System.Windows.Application.Current.Dispatcher.Invoke(delegate
        {
            fBWA32NXMCDU = new();
        });
        fBWA32NXMCDU.EditormodeOff += FBWA32NXMCDU_EditormodeOff; ;
        fBWA32NXMCDU.Closing += FBWA32NXMCDU_Closing; ;
        fBWA32NXMCDU.Dispatcher.BeginInvoke(delegate ()
        {
            GetPMDG737CDUSettings();
            fBWA32NXMCDU.Show();
            fBWA32NXMCDU.WindowState = (System.Windows.WindowState)fBW_A32NX_MCDU_Screen.Fullscreen;
        });
    }

    public override void Stop()
    {
        base.Stop();
        fBWA32NXMCDU?.Close();
    }

    private void FBWA32NXMCDU_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        SaveScreenProperties();
    }

    private void FBWA32NXMCDU_EditormodeOff(object sender, EventArgs e)
    {
        SaveScreenProperties();
    }

    private const string settings = @"Profiles\FBW A32NX\USB MCDU Screen L.json";
    FBW_A32NX_MCDU_Screen fBW_A32NX_MCDU_Screen = new();
    private void GetPMDG737CDUSettings()
    {
        if (File.Exists(settings))
        {
            fBW_A32NX_MCDU_Screen = JsonSerializer.Deserialize<FBW_A32NX_MCDU_Screen>(File.ReadAllText(settings), new JsonSerializerOptions { NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals });
        }
        fBW_A32NX_MCDU_Screen.Load(fBWA32NXMCDU);
    }

    private void SaveScreenProperties()
    {
        fBW_A32NX_MCDU_Screen.Save(fBWA32NXMCDU);
        string json = JsonSerializer.Serialize(fBW_A32NX_MCDU_Screen, new JsonSerializerOptions { WriteIndented = true, NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals });
        Directory.CreateDirectory(settings.Remove(18));
        if (File.Exists(settings))
        {
            if (File.ReadAllText(settings) != json)
            {
                File.WriteAllText(settings, json);
            }
            return;
        }
        File.WriteAllText(settings, json);
    }

    protected override void Simconnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
    {
        simConnectClient.OnSimVarChanged += SimConnectClient_OnSimVarChanged;

        simConnectClient.RegisterSimVar("L:A32NX_OVHD_INTLT_ANN");
        simConnectClient.RegisterSimVar("L:A32NX_ELEC_AC_ESS_SHED_BUS_IS_POWERED");

        simConnectClient.RegisterSimVar("L:A32NX_MCDU_L_ANNUNC_FAIL");
        simConnectClient.RegisterSimVar("L:A32NX_MCDU_L_ANNUNC_FMGC");
        simConnectClient.RegisterSimVar("L:A32NX_MCDU_L_ANNUNC_FM");
        simConnectClient.RegisterSimVar("L:A32NX_MCDU_L_ANNUNC_MCDU_MENU");
        simConnectClient.RegisterSimVar("L:A32NX_MCDU_L_ANNUNC_FM1");
        simConnectClient.RegisterSimVar("L:A32NX_MCDU_L_ANNUNC_IND");
        simConnectClient.RegisterSimVar("L:A32NX_MCDU_L_ANNUNC_RDY");
        simConnectClient.RegisterSimVar("L:A32NX_MCDU_L_ANNUNC_FM2");

        simConnectClient.RegisterSimVar("LIGHT POTENTIOMETER:85", "percent");
    }

    private void SimConnectClient_OnSimVarChanged(SimConnectClient.SimVar simVar)
    {
        switch (simVar.Name)
        {
            case "L:A32NX_OVHD_INTLT_ANN":
                if (simVar.Data == 0 && simConnectClient.GetSimVar("L:A32NX_ELEC_AC_ESS_SHED_BUS_IS_POWERED") == 1)
                {
                    SetLEDLighttest(true);
                    break;
                }
                SetLEDLighttest(false);
                break;

            case "L:A32NX_ELEC_AC_ESS_SHED_BUS_IS_POWERED":
                if (simVar.Data == 1 && simConnectClient.GetSimVar("L:A32NX_OVHD_INTLT_ANN") == 0)
                {
                    SetLEDLighttest(true);
                    break;
                }
                SetLEDLighttest(false);
                break;

            case "L:A32NX_MCDU_L_ANNUNC_FAIL":
                _ = interfaceIT_LED_Set(Device.Session, 2, simVar.BData());
                break;

            case "L:A32NX_MCDU_L_ANNUNC_FMGC":
                _ = interfaceIT_LED_Set(Device.Session, 3, simVar.BData());
                break;

            case "L:A32NX_MCDU_L_ANNUNC_FM":
                _ = interfaceIT_LED_Set(Device.Session, 4, simVar.BData());
                break;

            case "L:A32NX_MCDU_L_ANNUNC_MCDU_MENU":
                _ = interfaceIT_LED_Set(Device.Session, 5, simVar.BData());
                break;

            case "L:A32NX_MCDU_L_ANNUNC_FM1":
                _ = interfaceIT_LED_Set(Device.Session, 9, simVar.BData());
                break;

            case "L:A32NX_MCDU_L_ANNUNC_IND":
                _ = interfaceIT_LED_Set(Device.Session, 10, simVar.BData());
                break;

            case "L:A32NX_MCDU_L_ANNUNC_RDY":
                _ = interfaceIT_LED_Set(Device.Session, 11, simVar.BData());
                break;

            case "L:A32NX_MCDU_L_ANNUNC_FM2":
                _ = interfaceIT_LED_Set(Device.Session, 13, simVar.BData());
                break;

            case "LIGHT POTENTIOMETER:85":
                _ = interfaceIT_Brightness_Set(Device.Session, (int)(simVar.Data * 2.55));
                break;

            default:
                break;
        }
    }

    private void SetLEDLighttest(bool value)
    {
        _ = interfaceIT_LED_Set(Device.Session, 2, value);
        _ = interfaceIT_LED_Set(Device.Session, 3, value);
        _ = interfaceIT_LED_Set(Device.Session, 4, value);
        _ = interfaceIT_LED_Set(Device.Session, 5, value);
        _ = interfaceIT_LED_Set(Device.Session, 9, value);
        _ = interfaceIT_LED_Set(Device.Session, 10, value);
        _ = interfaceIT_LED_Set(Device.Session, 11, value);
        _ = interfaceIT_LED_Set(Device.Session, 13, value);
    }

    protected override bool KeyPressedProc(int session, int key, int direction)
    {
        if (direction == 0)
        {
            return true;
        }

        switch (key)
        {
            case 1:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_MENU");
                break;

            case 2:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_ATC");
                break;

            case 3:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_SEC");
                break;

            case 4:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_INIT");
                break;

            case 5:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_FUEL");
                break;

            case 6:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_RAD");
                break;

            case 7:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_FPLN");
                break;

            case 8:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_PERF");
                break;

            case 9:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_E");
                break;

            case 10:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_D");
                break;

            case 11:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_C");
                break;

            case 12:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_B");
                break;

            case 13:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_A");
                break;

            case 14:

                break;

            case 15:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_AIRPORT");
                break;

            case 16:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_PROG");
                break;

            case 17:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_J");
                break;

            case 18:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_I");
                break;

            case 19:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_H");
                break;

            case 20:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_G");
                break;

            case 21:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_F");
                break;

            case 22:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_UP");
                break;

            case 23:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_PREVPAGE");
                break;

            case 24:

                break;

            case 25:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_O");
                break;

            case 26:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_N");
                break;

            case 27:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_M");
                break;

            case 28:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_L");
                break;

            case 29:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_K");
                break;

            case 30:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_3");
                break;

            case 31:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_2");
                break;

            case 32:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_1");
                break;

            case 33:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_T");
                break;

            case 34:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_S");
                break;

            case 35:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_R");
                break;

            case 36:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_Q");
                break;

            case 37:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_P");
                break;

            case 38:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_6");
                break;

            case 39:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_5");
                break;

            case 40:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_4");
                break;

            case 41:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_Y");
                break;

            case 42:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_X");
                break;

            case 43:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_W");
                break;

            case 44:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_V");
                break;

            case 45:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_U");
                break;

            case 46:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_9");
                break;

            case 47:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_8");
                break;

            case 48:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_7");
                break;

            case 49:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_CLR");
                break;

            case 50:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_OVFY");
                break;

            case 51:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_SP");
                break;

            case 52:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_DIV");
                break;

            case 53:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_Z");
                break;

            case 54:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_PLUSMINUS");
                break;

            case 55:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_0");
                break;

            case 56:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_DOT");
                break;

            case 57:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_L1");
                break;

            case 58:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_L2");
                break;

            case 59:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_L3");
                break;

            case 60:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_L4");
                break;

            case 61:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_L5");
                break;

            case 62:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_L6");
                break;

            case 63:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_DIR");
                break;

            case 64:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_R1");
                break;

            case 65:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_R2");
                break;

            case 66:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_R3");
                break;

            case 67:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_R4");
                break;

            case 68:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_R5");
                break;

            case 69:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_R6");
                break;

            case 70:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_DATA");
                break;

            case 71:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_DOWN");
                break;

            case 72:
                simConnectClient.SetEventID("A320_Neo_CDU_1_BTN_NEXTPAGE");
                break;

            default:
                break;
        }
        return true;
    }
}