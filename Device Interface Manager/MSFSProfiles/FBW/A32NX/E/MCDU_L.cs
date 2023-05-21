using System;
using System.IO;
using System.Text.Json;
using Microsoft.FlightSimulator.SimConnect;

namespace Device_Interface_Manager.MSFSProfiles.FBW.A32NX.E;

public class MCDU_L : ENET
{
    public MCDU_L()
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

    private const string settings = @"Profiles\FBW A32NX\ENET MCDU Screen L.json";
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
    }

    private void SimConnectClient_OnSimVarChanged(object sender, SimConnectClient.SimVar simVar)
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
                interfaceITEthernet.SendinterfaceITEthernetLED(2, simVar.Data);
                break;

            case "L:A32NX_MCDU_L_ANNUNC_FMGC":
                interfaceITEthernet.SendinterfaceITEthernetLED(3, simVar.Data);
                break;

            case "L:A32NX_MCDU_L_ANNUNC_FM":
                interfaceITEthernet.SendinterfaceITEthernetLED(4, simVar.Data);
                break;

            case "L:A32NX_MCDU_L_ANNUNC_MCDU_MENU":
                interfaceITEthernet.SendinterfaceITEthernetLED(5, simVar.Data);
                break;

            case "L:A32NX_MCDU_L_ANNUNC_FM1":
                interfaceITEthernet.SendinterfaceITEthernetLED(6, simVar.Data);
                break;

            case "L:A32NX_MCDU_L_ANNUNC_IND":
                interfaceITEthernet.SendinterfaceITEthernetLED(7, simVar.Data);
                break;

            case "L:A32NX_MCDU_L_ANNUNC_RDY":
                interfaceITEthernet.SendinterfaceITEthernetLED(8, simVar.Data);
                break;

            case "L:A32NX_MCDU_L_ANNUNC_FM2":
                interfaceITEthernet.SendinterfaceITEthernetLED(10, simVar.Data);
                break;

            default:
                break;
        }
    }

    private void SetLEDLighttest(bool value)
    {
        interfaceITEthernet.SendinterfaceITEthernetLED(2, value);
        interfaceITEthernet.SendinterfaceITEthernetLED(3, value);
        interfaceITEthernet.SendinterfaceITEthernetLED(4, value);
        interfaceITEthernet.SendinterfaceITEthernetLED(5, value);
        interfaceITEthernet.SendinterfaceITEthernetLED(6, value);
        interfaceITEthernet.SendinterfaceITEthernetLED(7, value);
        interfaceITEthernet.SendinterfaceITEthernetLED(8, value);
        interfaceITEthernet.SendinterfaceITEthernetLED(10, value);
    }

    protected override void KeyPressedAction(int key, uint direction)
    {
        if (direction == 0)
        {
            return;
        }

        switch (key)
        {
            case 1:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_DOT");
                break;

            case 2:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_O");
                break;

            case 3:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_PLUSMINUS");
                break;

            case 4:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_Z");
                break;

            case 5:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_DIV");
                break;

            case 6:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_SP");
                break;

            case 7:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_OVFY");
                break;

            case 8:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_CLR");
                break;

            case 9:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_7");
                break;

            case 10:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_8");
                break;

            case 11:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_9");
                break;

            case 12:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_U");
                break;

            case 13:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_V");
                break;

            case 14:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_W");
                break;

            case 15:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_X");
                break;

            case 16:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_Y");
                break;

            case 17:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_4");
                break;

            case 18:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_5");
                break;

            case 19:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_6");
                break;

            case 20:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_P");
                break;

            case 21:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_Q");
                break;

            case 22:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_R");
                break;

            case 23:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_S");
                break;

            case 24:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_T");
                break;

            case 25:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_1");
                break;

            case 26:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_2");
                break;

            case 27:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_3");
                break;

            case 28:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_K");
                break;

            case 29:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_L");
                break;

            case 30:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_M");
                break;

            case 31:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_N");
                break;

            case 32:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_O");
                break;

            case 33:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_PREVPAGE");
                break;

            case 34:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_UP");
                break;

            case 35:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_L1");
                break;

            case 36:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_F");
                break;

            case 37:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_G");
                break;

            case 38:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_H");
                break;

            case 39:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_I");
                break;

            case 40:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_J");
                break;

            case 41:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_AIRPORT");
                break;

            case 42:

                break;

            case 43:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_L2");
                break;

            case 44:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_A");
                break;

            case 45:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_B");
                break;

            case 46:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_C");
                break;

            case 47:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_D");
                break;

            case 48:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_E");
                break;

            case 49:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_FPLN");
                break;

            case 50:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_RAD");
                break;

            case 51:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_L3");
                break;

            case 52:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_FUEL");
                break;

            case 53:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_FPLN");
                break;

            case 54:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_ATC");
                break;

            case 55:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_MENU");
                break;

            case 56:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_L5");
                break;

            case 57:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_DIR");
                break;

            case 58:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_PROG");
                break;

            case 59:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_L4");
                break;

            case 60:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_PERF");
                break;

            case 61:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_INIT");
                break;

            case 62:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_DATA");
                break;

            case 63:
                //BRIGHTNESS_DOWN
                break;

            case 64:
                //BRIGHTNESS_UP
                break;

            case 65:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_R1");
                break;

            case 66:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_R2");
                break;

            case 67:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_R3");
                break;

            case 68:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_R4");
                break;

            case 69:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_R5");
                break;

            case 70:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_R6");
                break;

            case 71:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_L6");
                break;

            case 72:

                break;

            case 73:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_NEXTPAGE");
                break;

            case 74:
                simConnectClient.SendEvent("H:A320_Neo_CDU_1_BTN_DOWN");
                break;

            default:
                break;
        }
    }
}