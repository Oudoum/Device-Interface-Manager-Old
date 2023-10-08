using System;
using System.IO;
using System.Text.Json;
using Microsoft.FlightSimulator.SimConnect;

namespace Device_Interface_Manager.SimConnectProfiles.FBW.A32NX.E;

public class MCDU_L : ENET
{
    public MCDU_L()
    {
        System.Windows.Application.Current.Dispatcher.Invoke(delegate
        {
            FBWA32NXMCDU = new();
        });
        FBWA32NXMCDU.EditormodeOff += FBWA32NXMCDU_EditormodeOff; ;
        FBWA32NXMCDU.Closing += FBWA32NXMCDU_Closing; ;
        FBWA32NXMCDU.Dispatcher.BeginInvoke(delegate ()
        {
            GetPMDG737CDUSettings();
            FBWA32NXMCDU.Show();
            FBWA32NXMCDU.WindowState = (System.Windows.WindowState)fBW_A32NX_MCDU_Screen.Fullscreen;
        });
    }

    protected override void StopDevice()
    {
        base.StopDevice();
        FBWA32NXMCDU?.Close();
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
        fBW_A32NX_MCDU_Screen.Load(FBWA32NXMCDU);
    }

    private void SaveScreenProperties()
    {
        fBW_A32NX_MCDU_Screen.Save(FBWA32NXMCDU);
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

    protected override void SimConnect_OnRecvOpen()
    {
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

    protected override void SimConnectClient_OnSimVarChanged(object sender, SimConnectClient.SimVar simVar)
    {
        base.SimConnectClient_OnSimVarChanged(sender, simVar);

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
                Device.SendinterfaceITEthernetLED(2, simVar.Data);
                break;

            case "L:A32NX_MCDU_L_ANNUNC_FMGC":
                Device.SendinterfaceITEthernetLED(3, simVar.Data);
                break;

            case "L:A32NX_MCDU_L_ANNUNC_FM":
                Device.SendinterfaceITEthernetLED(4, simVar.Data);
                break;

            case "L:A32NX_MCDU_L_ANNUNC_MCDU_MENU":
                Device.SendinterfaceITEthernetLED(5, simVar.Data);
                break;

            case "L:A32NX_MCDU_L_ANNUNC_FM1":
                Device.SendinterfaceITEthernetLED(6, simVar.Data);
                break;

            case "L:A32NX_MCDU_L_ANNUNC_IND":
                Device.SendinterfaceITEthernetLED(7, simVar.Data);
                break;

            case "L:A32NX_MCDU_L_ANNUNC_RDY":
                Device.SendinterfaceITEthernetLED(8, simVar.Data);
                break;

            case "L:A32NX_MCDU_L_ANNUNC_FM2":
                Device.SendinterfaceITEthernetLED(10, simVar.Data);
                break;

            default:
                break;
        }
    }

    private void SetLEDLighttest(bool value)
    {
        Device.SendinterfaceITEthernetLED(2, value);
        Device.SendinterfaceITEthernetLED(3, value);
        Device.SendinterfaceITEthernetLED(4, value);
        Device.SendinterfaceITEthernetLED(5, value);
        Device.SendinterfaceITEthernetLED(6, value);
        Device.SendinterfaceITEthernetLED(7, value);
        Device.SendinterfaceITEthernetLED(8, value);
        Device.SendinterfaceITEthernetLED(10, value);
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
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_DOT)");
                break;

            case 2:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_O)");
                break;

            case 3:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_PLUSMINUS)");
                break;

            case 4:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_Z)");
                break;

            case 5:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_DIV)");
                break;

            case 6:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_SP)");
                break;

            case 7:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_OVFY)");
                break;

            case 8:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_CLR)");
                break;

            case 9:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_7)");
                break;

            case 10:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_8)");
                break;

            case 11:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_9)");
                break;

            case 12:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_U)");
                break;

            case 13:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_V)");
                break;

            case 14:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_W)");
                break;

            case 15:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_X)");
                break;

            case 16:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_Y)");
                break;

            case 17:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_4)");
                break;

            case 18:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_5)");
                break;

            case 19:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_6)");
                break;

            case 20:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_P)");
                break;

            case 21:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_Q)");
                break;

            case 22:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_R)");
                break;

            case 23:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_S)");
                break;

            case 24:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_T)");
                break;

            case 25:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_1)");
                break;

            case 26:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_2)");
                break;

            case 27:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_3)");
                break;

            case 28:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_K)");
                break;

            case 29:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_L)");
                break;

            case 30:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_M)");
                break;

            case 31:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_N)");
                break;

            case 32:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_O)");
                break;

            case 33:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_PREVPAGE)");
                break;

            case 34:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_UP)");
                break;

            case 35:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_L1)");
                break;

            case 36:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_F)");
                break;

            case 37:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_G)");
                break;

            case 38:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_H)");
                break;

            case 39:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_I)");
                break;

            case 40:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_J)");
                break;

            case 41:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_AIRPORT)");
                break;

            case 42:

                break;

            case 43:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_L2)");
                break;

            case 44:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_A)");
                break;

            case 45:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_B)");
                break;

            case 46:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_C)");
                break;

            case 47:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_D)");
                break;

            case 48:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_E)");
                break;

            case 49:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_FPLN)");
                break;

            case 50:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_RAD)");
                break;

            case 51:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_L3)");
                break;

            case 52:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_FUEL)");
                break;

            case 53:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_FPLN)");
                break;

            case 54:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_ATC)");
                break;

            case 55:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_MENU)");
                break;

            case 56:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_L5)");
                break;

            case 57:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_DIR)");
                break;

            case 58:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_PROG)");
                break;

            case 59:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_L4)");
                break;

            case 60:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_PERF)");
                break;

            case 61:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_INIT)");
                break;

            case 62:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_DATA)");
                break;

            case 63:
                //BRIGHTNESS_DOWN
                break;

            case 64:
                //BRIGHTNESS_UP
                break;

            case 65:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_R1)");
                break;

            case 66:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_R2)");
                break;

            case 67:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_R3)");
                break;

            case 68:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_R4)");
                break;

            case 69:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_R5)");
                break;

            case 70:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_R6)");
                break;

            case 71:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_L6)");
                break;

            case 72:

                break;

            case 73:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_NEXTPAGE)");
                break;

            case 74:
                simConnectClient.SendWASMEvent("(>H:A320_Neo_CDU_1_BTN_DOWN)");
                break;

            default:
                break;
        }
    }
}