﻿using static Device_Interface_Manager.MSFSProfiles.PMDG.PMDG_NG3_SDK;

namespace Device_Interface_Manager.MSFSProfiles.PMDG.B737.USB;

public class NG_CDU_L : NG_CDU_Base
{
    public NG_CDU_L()
    {
        startupManager.Settings = @"Profiles\PMDG 737\USB CDU Screen L.json";
        CDU_ID = DATA_REQUEST_ID.CDU0_REQUEST;
    }

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
        CDU_annunEXEC = pMDG_NG3_Data.CDU_annunEXEC[0];
        CDU_annunCALL = pMDG_NG3_Data.CDU_annunCALL[0];
        CDU_annunMSG = pMDG_NG3_Data.CDU_annunMSG[0];
        CDU_annunFAIL = pMDG_NG3_Data.CDU_annunFAIL[0];
        CDU_annunOFST = pMDG_NG3_Data.CDU_annunOFST[0];
    }

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

        switch (key)
        {
            case 1:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_EXEC);
                break;

            case 2:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_PROG);
                break;

            case 3:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_HOLD);
                break;

            case 4:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_CRZ);
                break;

            case 5:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_DEP_ARR);
                break;

            case 6:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_LEGS);
                break;

            case 7:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_MENU);
                break;

            case 8:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_CLB);
                break;

            case 9:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_E);
                break;

            case 10:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_D);
                break;

            case 11:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_C);
                break;

            case 12:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_B);
                break;

            case 13:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_A);
                break;

            case 14:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_FIX);
                break;

            case 15:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_N1_LIMIT);
                break;

            case 16:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_RTE);
                break;

            case 17:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_J);
                break;

            case 18:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_I);
                break;

            case 19:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_H);
                break;

            case 20:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_G);
                break;

            case 21:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_F);
                break;

            case 22:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_NEXT_PAGE);
                break;

            case 23:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_PREV_PAGE);
                break;

            case 25:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_O);
                break;

            case 26:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_N);
                break;

            case 27:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_M);
                break;

            case 28:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_L);
                break;

            case 29:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_K);
                break;

            case 30:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_3);
                break;

            case 31:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_2);
                break;

            case 32:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_1);
                break;

            case 33:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_T);
                break;

            case 34:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_S);
                break;

            case 35:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_R);
                break;

            case 36:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_Q);
                break;

            case 37:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_P);
                break;

            case 38:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_6);
                break;

            case 39:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_5);
                break;

            case 40:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_4);
                break;

            case 41:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_Y);
                break;

            case 42:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_X);
                break;

            case 43:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_W);
                break;

            case 44:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_V);
                break;

            case 45:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_U);
                break;

            case 46:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_9);
                break;

            case 47:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_8);
                break;

            case 48:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_7);
                break;

            case 49:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_CLR);
                break;

            case 50:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_SLASH);
                break;

            case 51:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_DEL);
                break;

            case 52:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_SPACE);
                break;

            case 53:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_Z);
                break;

            case 54:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_PLUS_MINUS);
                break;

            case 55:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_0);
                break;

            case 56:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_DOT);
                break;

            case 57:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_L1);
                break;

            case 58:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_L2);
                break;

            case 59:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_L3);
                break;

            case 60:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_L4);
                break;

            case 61:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_L5);
                break;

            case 62:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_L6);
                break;

            case 63:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_INIT_REF);
                break;

            case 64:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_R1);
                break;

            case 65:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_R2);
                break;

            case 66:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_R3);
                break;

            case 67:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_R4);
                break;

            case 68:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_R5);
                break;

            case 69:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_R6);
                break;

            case 70:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_L_DES);
                break;
        }
    }
}