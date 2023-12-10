using System;
using System.Collections.Generic;
using static Device_Interface_Manager.SimConnectProfiles.PMDG.PMDG_NG3_SDK;

namespace Device_Interface_Manager.SimConnectProfiles.PMDG.B737.USB;

public class NG_CDU_L : CDU_Base_USB<PMDG_NG3_Data>
{
    protected override string SettingsLocation => @"Profiles\PMDG 737\USB CDU Screen L.json";

    protected override PMDG.DATA_REQUEST_ID CDU_ID => PMDG.DATA_REQUEST_ID.CDU0_REQUEST;

    protected override Enum CDUBrightnessButton => PMDGEvents.EVT_CDU_L_BRITENESS;

    protected override void GetCDUAnalogValues()
    {
        //Task.Run(() => GetValues(cancellationTokenSource.Token));
    }

    //public void GetValues(CancellationToken cancellationToken)
    //{
    //    int oldValue = 0;
    //    int noldValue = 0;
    //    while (!cancellationToken.IsCancellationRequested)
    //    {
    //        interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_Analog_GetValue(Device.Session, 0, out int value);
    //        if (Math.Abs(value - oldValue) >= 50 || oldValue == 0)
    //        {
    //            oldValue = value;
    //        }
    //        if (Math.Abs(value - noldValue) > 40 || noldValue == 0)
    //        {
    //            if (PMDG737CDU is not null)
    //            {
    //                noldValue = value;
    //                PMDG737CDU.Dispatcher.BeginInvoke(() => PMDG737CDU.Brightness = value);
    //            }
    //        }
    //        Thread.Sleep(50);
    //    }
    //}

    protected override void RecvData()
    {
        CDU_annunEXEC = PMDG_Data.CDU_annunEXEC[0];
        CDU_annunCALL = PMDG_Data.CDU_annunCALL[0];
        CDU_annunMSG = PMDG_Data.CDU_annunMSG[0];
        CDU_annunFAIL = PMDG_Data.CDU_annunFAIL[0];
        CDU_annunOFST = PMDG_Data.CDU_annunOFST[0];
    }

    private readonly Dictionary<int, PMDGEvents> keyEventMap = new()
    {
        { 1, PMDGEvents.EVT_CDU_L_EXEC},
        { 2, PMDGEvents.EVT_CDU_L_PROG},
        { 3, PMDGEvents.EVT_CDU_L_HOLD},
        { 4, PMDGEvents.EVT_CDU_L_CRZ},
        { 5, PMDGEvents.EVT_CDU_L_DEP_ARR},
        { 6, PMDGEvents.EVT_CDU_L_LEGS},
        { 7, PMDGEvents.EVT_CDU_L_MENU},
        { 8, PMDGEvents.EVT_CDU_L_CLB},
        { 9, PMDGEvents.EVT_CDU_L_E},
        {10, PMDGEvents.EVT_CDU_L_D},
        {11, PMDGEvents.EVT_CDU_L_C},
        {12, PMDGEvents.EVT_CDU_L_B},
        {13, PMDGEvents.EVT_CDU_L_A},
        {14, PMDGEvents.EVT_CDU_L_FIX},
        {15, PMDGEvents.EVT_CDU_L_N1_LIMIT},
        {16, PMDGEvents.EVT_CDU_L_RTE},
        {17, PMDGEvents.EVT_CDU_L_J},
        {18, PMDGEvents.EVT_CDU_L_I},
        {19, PMDGEvents.EVT_CDU_L_H},
        {20, PMDGEvents.EVT_CDU_L_G},
        {21, PMDGEvents.EVT_CDU_L_F},
        {22, PMDGEvents.EVT_CDU_L_NEXT_PAGE},
        {23, PMDGEvents.EVT_CDU_L_PREV_PAGE},

        {25, PMDGEvents.EVT_CDU_L_O},
        {26, PMDGEvents.EVT_CDU_L_N},
        {27, PMDGEvents.EVT_CDU_L_M},
        {28, PMDGEvents.EVT_CDU_L_L},
        {29, PMDGEvents.EVT_CDU_L_K},
        {30, PMDGEvents.EVT_CDU_L_3},
        {31, PMDGEvents.EVT_CDU_L_2},
        {32, PMDGEvents.EVT_CDU_L_1},
        {33, PMDGEvents.EVT_CDU_L_T},
        {34, PMDGEvents.EVT_CDU_L_S},
        {35, PMDGEvents.EVT_CDU_L_R},
        {36, PMDGEvents.EVT_CDU_L_Q},
        {37, PMDGEvents.EVT_CDU_L_P},
        {38, PMDGEvents.EVT_CDU_L_6},
        {39, PMDGEvents.EVT_CDU_L_5},
        {40, PMDGEvents.EVT_CDU_L_4},
        {41, PMDGEvents.EVT_CDU_L_Y},
        {42, PMDGEvents.EVT_CDU_L_X},
        {43, PMDGEvents.EVT_CDU_L_W},
        {44, PMDGEvents.EVT_CDU_L_V},
        {45, PMDGEvents.EVT_CDU_L_U},
        {46, PMDGEvents.EVT_CDU_L_9},
        {47, PMDGEvents.EVT_CDU_L_8},
        {48, PMDGEvents.EVT_CDU_L_7},
        {49, PMDGEvents.EVT_CDU_L_CLR},
        {50, PMDGEvents.EVT_CDU_L_SLASH},
        {51, PMDGEvents.EVT_CDU_L_DEL},
        {52, PMDGEvents.EVT_CDU_L_SPACE},
        {53, PMDGEvents.EVT_CDU_L_Z},
        {54, PMDGEvents.EVT_CDU_L_PLUS_MINUS},
        {55, PMDGEvents.EVT_CDU_L_0},
        {56, PMDGEvents.EVT_CDU_L_DOT},
        {57, PMDGEvents.EVT_CDU_L_L1},
        {58, PMDGEvents.EVT_CDU_L_L2},
        {59, PMDGEvents.EVT_CDU_L_L3},
        {60, PMDGEvents.EVT_CDU_L_L4},
        {61, PMDGEvents.EVT_CDU_L_L5},
        {62, PMDGEvents.EVT_CDU_L_L6},
        {63, PMDGEvents.EVT_CDU_L_INIT_REF},
        {64, PMDGEvents.EVT_CDU_L_R1},
        {65, PMDGEvents.EVT_CDU_L_R2},
        {66, PMDGEvents.EVT_CDU_L_R3},
        {67, PMDGEvents.EVT_CDU_L_R4},
        {68, PMDGEvents.EVT_CDU_L_R5},
        {69, PMDGEvents.EVT_CDU_L_R6},
        {70, PMDGEvents.EVT_CDU_L_DES},
    };

    protected override void KeyPressedProc(uint session, int key, uint direction)
    {
        switch (direction)
        {
            case 0:
                if (key == 49)
                {
                    direction = MOUSE_FLAG_LEFTRELEASE;
                    break;
                }
                return;

            case 1:
                direction = MOUSE_FLAG_LEFTSINGLE;
                break;
        }

        if (keyEventMap.TryGetValue(key, out PMDGEvents value))
        {
            simConnectClient.TransmitEvent(direction, value);
        }
    }
}