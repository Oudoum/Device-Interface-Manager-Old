using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Device_Interface_Manager.SimConnectProfiles.FBW.A32NX.E;

public class MCDU_R : ENET
{
    public MCDU_R()
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

    protected override void Stop()
    {
        base.Stop();
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

    private const string settings = @"Profiles\FBW A32NX\ENET MCDU Screen R.json";
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

    protected override void OnRecvOpen()
    {
        simConnectClient.OnSimVarChanged += SimConnectClient_OnSimVarChanged;

        simConnectClient.RegisterSimVar("L:A32NX_OVHD_INTLT_ANN");
        simConnectClient.RegisterSimVar("L:A32NX_ELEC_AC_ESS_SHED_BUS_IS_POWERED");

        simConnectClient.RegisterSimVar("L:A32NX_MCDU_R_ANNUNC_FAIL");
        simConnectClient.RegisterSimVar("L:A32NX_MCDU_R_ANNUNC_FMGC");
        simConnectClient.RegisterSimVar("L:A32NX_MCDU_R_ANNUNC_FM");
        simConnectClient.RegisterSimVar("L:A32NX_MCDU_R_ANNUNC_MCDU_MENU");
        simConnectClient.RegisterSimVar("L:A32NX_MCDU_R_ANNUNC_FM1");
        simConnectClient.RegisterSimVar("L:A32NX_MCDU_R_ANNUNC_IND");
        simConnectClient.RegisterSimVar("L:A32NX_MCDU_R_ANNUNC_RDY");
        simConnectClient.RegisterSimVar("L:A32NX_MCDU_R_ANNUNC_FM2");
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

            case "L:A32NX_MCDU_R_ANNUNC_FAIL":
                Device.SendinterfaceITEthernetLED(2, simVar.Data);
                break;

            case "L:A32NX_MCDU_R_ANNUNC_FMGC":
                Device.SendinterfaceITEthernetLED(3, simVar.Data);
                break;

            case "L:A32NX_MCDU_R_ANNUNC_FM":
                Device.SendinterfaceITEthernetLED(4, simVar.Data);
                break;

            case "L:A32NX_MCDU_R_ANNUNC_MCDU_MENU":
                Device.SendinterfaceITEthernetLED(5, simVar.Data);
                break;

            case "L:A32NX_MCDU_R_ANNUNC_FM1":
                Device.SendinterfaceITEthernetLED(6, simVar.Data);
                break;

            case "L:A32NX_MCDU_R_ANNUNC_IND":
                Device.SendinterfaceITEthernetLED(7, simVar.Data);
                break;

            case "L:A32NX_MCDU_R_ANNUNC_RDY":
                Device.SendinterfaceITEthernetLED(8, simVar.Data);
                break;

            case "L:A32NX_MCDU_R_ANNUNC_FM2":
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

    private readonly Dictionary<int, string> keyEventMap = new()
    {
        { 1, "(>H:A320_Neo_CDU_2_BTN_DOT)"},
        { 2, "(>H:A320_Neo_CDU_2_BTN_O)"},
        { 3, "(>H:A320_Neo_CDU_2_BTN_PLUSMINUS)"},
        { 4, "(>H:A320_Neo_CDU_2_BTN_Z)"},
        { 5, "(>H:A320_Neo_CDU_2_BTN_DIV)"},
        { 6, "(>H:A320_Neo_CDU_2_BTN_SP)"},
        { 7, "(>H:A320_Neo_CDU_2_BTN_OVFY)"},
        { 8, "(>H:A320_Neo_CDU_2_BTN_CLR)"},
        { 9, "(>H:A320_Neo_CDU_2_BTN_7)"},
        {10, "(>H:A320_Neo_CDU_2_BTN_8)"},
        {11, "(>H:A320_Neo_CDU_2_BTN_9)"},
        {12, "(>H:A320_Neo_CDU_2_BTN_U)"},
        {13, "(>H:A320_Neo_CDU_2_BTN_V)"},
        {14, "(>H:A320_Neo_CDU_2_BTN_W)"},
        {15, "(>H:A320_Neo_CDU_2_BTN_X)"},
        {16, "(>H:A320_Neo_CDU_2_BTN_Y)"},
        {17, "(>H:A320_Neo_CDU_2_BTN_4)"},
        {18, "(>H:A320_Neo_CDU_2_BTN_5)"},
        {19, "(>H:A320_Neo_CDU_2_BTN_6)"},
        {20, "(>H:A320_Neo_CDU_2_BTN_P)"},
        {21, "(>H:A320_Neo_CDU_2_BTN_Q)"},
        {22, "(>H:A320_Neo_CDU_2_BTN_R)"},
        {23, "(>H:A320_Neo_CDU_2_BTN_S)"},
        {24, "(>H:A320_Neo_CDU_2_BTN_T)"},
        {25, "(>H:A320_Neo_CDU_2_BTN_1)"},
        {26, "(>H:A320_Neo_CDU_2_BTN_2)"},
        {27, "(>H:A320_Neo_CDU_2_BTN_3)"},
        {28, "(>H:A320_Neo_CDU_2_BTN_K)"},
        {29, "(>H:A320_Neo_CDU_2_BTN_L)"},
        {30, "(>H:A320_Neo_CDU_2_BTN_M)"},
        {31, "(>H:A320_Neo_CDU_2_BTN_N)"},
        {32, "(>H:A320_Neo_CDU_2_BTN_O)"},
        {33, "(>H:A320_Neo_CDU_2_BTN_PREVPAGE)"},
        {34, "(>H:A320_Neo_CDU_2_BTN_UP)"},
        {35, "(>H:A320_Neo_CDU_2_BTN_L1)"},
        {36, "(>H:A320_Neo_CDU_2_BTN_F)"},
        {37, "(>H:A320_Neo_CDU_2_BTN_G)"},
        {38, "(>H:A320_Neo_CDU_2_BTN_H)"},
        {39, "(>H:A320_Neo_CDU_2_BTN_I)"},
        {40, "(>H:A320_Neo_CDU_2_BTN_J)"},
        {41, "(>H:A320_Neo_CDU_2_BTN_AIRPORT)"},

        {43, "(>H:A320_Neo_CDU_2_BTN_L2)"},
        {44, "(>H:A320_Neo_CDU_2_BTN_A)"},
        {45, "(>H:A320_Neo_CDU_2_BTN_B)"},
        {46, "(>H:A320_Neo_CDU_2_BTN_C)"},
        {47, "(>H:A320_Neo_CDU_2_BTN_D)"},
        {48, "(>H:A320_Neo_CDU_2_BTN_E)"},
        {49, "(>H:A320_Neo_CDU_2_BTN_FPLN)"},
        {50, "(>H:A320_Neo_CDU_2_BTN_RAD)"},
        {51, "(>H:A320_Neo_CDU_2_BTN_L3)"},
        {52, "(>H:A320_Neo_CDU_2_BTN_FUEL)"},
        {53, "(>H:A320_Neo_CDU_2_BTN_FPLN)"},
        {54, "(>H:A320_Neo_CDU_2_BTN_ATC)"},
        {55, "(>H:A320_Neo_CDU_2_BTN_MENU)"},
        {56, "(>H:A320_Neo_CDU_2_BTN_L5)"},
        {57, "(>H:A320_Neo_CDU_2_BTN_DIR)"},
        {58, "(>H:A320_Neo_CDU_2_BTN_PROG)"},
        {59, "(>H:A320_Neo_CDU_2_BTN_L4)"},
        {60, "(>H:A320_Neo_CDU_2_BTN_PERF)"},
        {61, "(>H:A320_Neo_CDU_2_BTN_INIT)"},
        {62, "(>H:A320_Neo_CDU_2_BTN_DATA)"},


        {65, "(>H:A320_Neo_CDU_2_BTN_R1)"},
        {66, "(>H:A320_Neo_CDU_2_BTN_R2)"},
        {67, "(>H:A320_Neo_CDU_2_BTN_R3)"},
        {68, "(>H:A320_Neo_CDU_2_BTN_R4)"},
        {69, "(>H:A320_Neo_CDU_2_BTN_R5)"},
        {70, "(>H:A320_Neo_CDU_2_BTN_R6)"},
        {71, "(>H:A320_Neo_CDU_2_BTN_L6)"},

        {73, "(>H:A320_Neo_CDU_2_BTN_NEXTPAGE)"},
        {74, "(>H:A320_Neo_CDU_2_BTN_DOWN)"},
    };

    protected override void KeyPressedAction(int key, uint direction)
    {
        if (direction == 0 || !keyEventMap.ContainsKey(key))
        {
            return;
        }

        simConnectClient.SendWASMEvent(keyEventMap[key]);
    }
}