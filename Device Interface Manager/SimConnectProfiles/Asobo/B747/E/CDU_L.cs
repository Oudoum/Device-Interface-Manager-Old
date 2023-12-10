using System.Collections.Generic;

namespace Device_Interface_Manager.SimConnectProfiles.Asobo.B747.E;

public class CDU_L : ENET
{
    protected override void OnRecvOpen()
    {
        simConnectClient.RegisterSimVar("L:FMC_EXEC_ACTIVE");
    }

    protected override void SimConnectClient_OnSimVarChanged(object sender, SimConnectClient.SimVar simVar)
    {
        base.SimConnectClient_OnSimVarChanged(sender, simVar);

        if (simVar.Name == "L:FMC_EXEC_ACTIVE")
        {
            Device.SendinterfaceITEthernetLED(1, simVar.Data);
        }
    }

    private readonly Dictionary<int, string> keyEventMap = new()
    {
        { 1, "(>H:B747_8_FMC_1_BTN_DOT)"},
        { 2, "(>H:B747_8_FMC_1_BTN_0)"},
        { 3, "(>H:B747_8_FMC_1_BTN_PLUSMINUS)"},
        { 4, "(>H:B747_8_FMC_1_BTN_Z)"},
        { 5, "(>H:B747_8_FMC_1_BTN_SP)"},
        { 6, "(>H:B747_8_FMC_1_BTN_DEL)"},
        { 7, "(>H:B747_8_FMC_1_BTN_DIV)"},
        { 8, "(>H:B747_8_FMC_1_BTN_CLR)"},
        { 9, "(>H:B747_8_FMC_1_BTN_7)"},
        {10, "(>H:B747_8_FMC_1_BTN_8)"},
        {11, "(>H:B747_8_FMC_1_BTN_9)"},
        {12, "(>H:B747_8_FMC_1_BTN_U)"},
        {13, "(>H:B747_8_FMC_1_BTN_V)"},
        {14, "(>H:B747_8_FMC_1_BTN_W)"},
        {15, "(>H:B747_8_FMC_1_BTN_X)"},
        {16, "(>H:B747_8_FMC_1_BTN_Y)"},
        {17, "(>H:B747_8_FMC_1_BTN_4)"},
        {18, "(>H:B747_8_FMC_1_BTN_5)"},
        {19, "(>H:B747_8_FMC_1_BTN_6)"},
        {20, "(>H:B747_8_FMC_1_BTN_P)"},
        {21, "(>H:B747_8_FMC_1_BTN_Q)"},
        {22, "(>H:B747_8_FMC_1_BTN_R)"},
        {23, "(>H:B747_8_FMC_1_BTN_S)"},
        {24, "(>H:B747_8_FMC_1_BTN_T)"},
        {25, "(>H:B747_8_FMC_1_BTN_1)"},
        {26, "(>H:B747_8_FMC_1_BTN_2)"},
        {27, "(>H:B747_8_FMC_1_BTN_3)"},
        {28, "(>H:B747_8_FMC_1_BTN_K)"},
        {29, "(>H:B747_8_FMC_1_BTN_L)"},
        {30, "(>H:B747_8_FMC_1_BTN_M)"},
        {31, "(>H:B747_8_FMC_1_BTN_N)"},
        {32, "(>H:B747_8_FMC_1_BTN_O)"},
        {33, "(>H:B747_8_FMC_1_BTN_PREVPAGE)"},
        {34, "(>H:B747_8_FMC_1_BTN_NEXTPAGE)"},
        {35, "(>H:B747_8_FMC_1_BTN_L1)"},
        {36, "(>H:B747_8_FMC_1_BTN_F)"},
        {37, "(>H:B747_8_FMC_1_BTN_G)"},
        {38, "(>H:B747_8_FMC_1_BTN_H)"},
        {39, "(>H:B747_8_FMC_1_BTN_I)"},
        {40, "(>H:B747_8_FMC_1_BTN_J)"},
        {41, "(>H:B747_8_FMC_1_BTN_MENU)"},
        {42, "(>H:B747_8_FMC_1_BTN_NAVRAD)"},
        {43, "(>H:B747_8_FMC_1_BTN_L2)"},
        {44, "(>H:B747_8_FMC_1_BTN_A)"},
        {45, "(>H:B747_8_FMC_1_BTN_B)"},
        {46, "(>H:B747_8_FMC_1_BTN_C)"},
        {47, "(>H:B747_8_FMC_1_BTN_D)"},
        {48, "(>H:B747_8_FMC_1_BTN_E)"},
        {49, "(>H:B747_8_FMC_1_BTN_FIX)"},
        {50, "(>H:B747_8_FMC_1_BTN_LEGS)"},
        {51, "(>H:B747_8_FMC_1_BTN_L3)"},
        {52, "(>H:B747_8_FMC_1_BTN_HOLD)"},
        {53, "(>H:B747_8_FMC_1_BTN_FMCCOMM)"},
        {54, "(>H:B747_8_FMC_1_BTN_PROG)"},
        {55, "(>H:B747_8_FMC_1_BTN_EXEC)"},
        {56, "(>H:B747_8_FMC_1_BTN_L5)"},
        {57, "(>H:B747_8_FMC_1_BTN_INIT)"},
        {58, "(>H:B747_8_FMC_1_BTN_RTE)"},
        {59, "(>H:B747_8_FMC_1_BTN_L4)"},
        {60, "(>H:B747_8_FMC_1_BTN_DEPARR)"},
        {61, "(>H:B747_8_FMC_1_BTN_ATC)"},
        {62, "(>H:B747_8_FMC_1_BTN_VNAV)"},
        {63, "(>B:FMC_B747_1_Button_BRT_DIM_Inc)"},
        {64, "(>B:FMC_B747_1_Button_BRT_DIM_Dec)"},
        {65, "(>H:B747_8_FMC_1_BTN_R1)"},
        {66, "(>H:B747_8_FMC_1_BTN_R2)"},
        {67, "(>H:B747_8_FMC_1_BTN_R3)"},
        {68, "(>H:B747_8_FMC_1_BTN_R4)"},
        {69, "(>H:B747_8_FMC_1_BTN_R5)"},
        {70, "(>H:B747_8_FMC_1_BTN_R6)"},
        {71, "(>H:B747_8_FMC_1_BTN_L6)"},
    };

    protected override void KeyPressedAction(int key, uint direction)
    {
        if (direction == 0 || !keyEventMap.TryGetValue(key, out string value))
        {
            return;
        }

        simConnectClient.SendWASMEvent(value);
    }
}