using System;
using System.Collections.Generic;
using static Device_Interface_Manager.SimConnectProfiles.PMDG.PMDG_NG3_SDK;

namespace Device_Interface_Manager.SimConnectProfiles.PMDG.B737.USB;

public class NG_CDU_R : CDU_Base_USB<PMDG_NG3_Data>
{
    protected override string SettingsLocation => @"Profiles\PMDG 737\USB CDU Screen R.json";

    protected override PMDG.DATA_REQUEST_ID CDU_ID => PMDG.DATA_REQUEST_ID.CDU1_REQUEST;

    protected override Enum CDUBrightnessButton => PMDGEvents.EVT_CDU_R_BRITENESS;

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
        CDU_annunEXEC = PMDG_Data.CDU_annunEXEC[1];
        CDU_annunCALL = PMDG_Data.CDU_annunCALL[1];
        CDU_annunMSG = PMDG_Data.CDU_annunMSG[1];
        CDU_annunFAIL = PMDG_Data.CDU_annunFAIL[1];
        CDU_annunOFST = PMDG_Data.CDU_annunOFST[1];
    }

    private readonly Dictionary<int, PMDGEvents> keyEventMap = new()
    {
        { 1, PMDGEvents.EVT_CDU_R_EXEC},
        { 2, PMDGEvents.EVT_CDU_R_PROG},
        { 3, PMDGEvents.EVT_CDU_R_HOLD},
        { 4, PMDGEvents.EVT_CDU_R_CRZ},
        { 5, PMDGEvents.EVT_CDU_R_DEP_ARR},
        { 6, PMDGEvents.EVT_CDU_R_LEGS},
        { 7, PMDGEvents.EVT_CDU_R_MENU},
        { 8, PMDGEvents.EVT_CDU_R_CLB},
        { 9, PMDGEvents.EVT_CDU_R_E},
        {10, PMDGEvents.EVT_CDU_R_D},
        {11, PMDGEvents.EVT_CDU_R_C},
        {12, PMDGEvents.EVT_CDU_R_B},
        {13, PMDGEvents.EVT_CDU_R_A},
        {14, PMDGEvents.EVT_CDU_R_FIX},
        {15, PMDGEvents.EVT_CDU_R_N1_LIMIT},
        {16, PMDGEvents.EVT_CDU_R_RTE},
        {17, PMDGEvents.EVT_CDU_R_J},
        {18, PMDGEvents.EVT_CDU_R_I},
        {19, PMDGEvents.EVT_CDU_R_H},
        {20, PMDGEvents.EVT_CDU_R_G},
        {21, PMDGEvents.EVT_CDU_R_F},
        {22, PMDGEvents.EVT_CDU_R_NEXT_PAGE},
        {23, PMDGEvents.EVT_CDU_R_PREV_PAGE},

        {25, PMDGEvents.EVT_CDU_R_O},
        {26, PMDGEvents.EVT_CDU_R_N},
        {27, PMDGEvents.EVT_CDU_R_M},
        {28, PMDGEvents.EVT_CDU_R_L},
        {29, PMDGEvents.EVT_CDU_R_K},
        {30, PMDGEvents.EVT_CDU_R_3},
        {31, PMDGEvents.EVT_CDU_R_2},
        {32, PMDGEvents.EVT_CDU_R_1},
        {33, PMDGEvents.EVT_CDU_R_T},
        {34, PMDGEvents.EVT_CDU_R_S},
        {35, PMDGEvents.EVT_CDU_R_R},
        {36, PMDGEvents.EVT_CDU_R_Q},
        {37, PMDGEvents.EVT_CDU_R_P},
        {38, PMDGEvents.EVT_CDU_R_6},
        {39, PMDGEvents.EVT_CDU_R_5},
        {40, PMDGEvents.EVT_CDU_R_4},
        {41, PMDGEvents.EVT_CDU_R_Y},
        {42, PMDGEvents.EVT_CDU_R_X},
        {43, PMDGEvents.EVT_CDU_R_W},
        {44, PMDGEvents.EVT_CDU_R_V},
        {45, PMDGEvents.EVT_CDU_R_U},
        {46, PMDGEvents.EVT_CDU_R_9},
        {47, PMDGEvents.EVT_CDU_R_8},
        {48, PMDGEvents.EVT_CDU_R_7},
        {49, PMDGEvents.EVT_CDU_R_CLR},
        {50, PMDGEvents.EVT_CDU_R_SLASH},
        {51, PMDGEvents.EVT_CDU_R_DEL},
        {52, PMDGEvents.EVT_CDU_R_SPACE},
        {53, PMDGEvents.EVT_CDU_R_Z},
        {54, PMDGEvents.EVT_CDU_R_PLUS_MINUS},
        {55, PMDGEvents.EVT_CDU_R_0},
        {56, PMDGEvents.EVT_CDU_R_DOT},
        {57, PMDGEvents.EVT_CDU_R_L1},
        {58, PMDGEvents.EVT_CDU_R_L2},
        {59, PMDGEvents.EVT_CDU_R_L3},
        {60, PMDGEvents.EVT_CDU_R_L4},
        {61, PMDGEvents.EVT_CDU_R_L5},
        {62, PMDGEvents.EVT_CDU_R_L6},
        {63, PMDGEvents.EVT_CDU_R_INIT_REF},
        {64, PMDGEvents.EVT_CDU_R_R1},
        {65, PMDGEvents.EVT_CDU_R_R2},
        {66, PMDGEvents.EVT_CDU_R_R3},
        {67, PMDGEvents.EVT_CDU_R_R4},
        {68, PMDGEvents.EVT_CDU_R_R5},
        {69, PMDGEvents.EVT_CDU_R_R6},
        {70, PMDGEvents.EVT_CDU_R_DES},
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