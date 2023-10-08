using System;
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
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_EXEC);
                break;

            case 2:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_PROG);
                break;

            case 3:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_HOLD);
                break;

            case 4:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_CRZ);
                break;

            case 5:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_DEP_ARR);
                break;

            case 6:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_LEGS);
                break;

            case 7:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_MENU);
                break;

            case 8:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_CLB);
                break;

            case 9:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_E);
                break;

            case 10:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_D);
                break;

            case 11:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_C);
                break;

            case 12:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_B);
                break;

            case 13:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_A);
                break;

            case 14:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_FIX);
                break;

            case 15:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_N1_LIMIT);
                break;

            case 16:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_RTE);
                break;

            case 17:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_J);
                break;

            case 18:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_I);
                break;

            case 19:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_H);
                break;

            case 20:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_G);
                break;

            case 21:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_F);
                break;

            case 22:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_NEXT_PAGE);
                break;

            case 23:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_PREV_PAGE);
                break;

            case 25:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_O);
                break;

            case 26:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_N);
                break;

            case 27:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_M);
                break;

            case 28:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_L);
                break;

            case 29:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_K);
                break;

            case 30:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_3);
                break;

            case 31:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_2);
                break;

            case 32:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_1);
                break;

            case 33:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_T);
                break;

            case 34:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_S);
                break;

            case 35:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_R);
                break;

            case 36:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_Q);
                break;

            case 37:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_P);
                break;

            case 38:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_6);
                break;

            case 39:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_5);
                break;

            case 40:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_4);
                break;

            case 41:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_Y);
                break;

            case 42:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_X);
                break;

            case 43:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_W);
                break;

            case 44:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_V);
                break;

            case 45:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_U);
                break;

            case 46:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_9);
                break;

            case 47:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_8);
                break;

            case 48:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_7);
                break;

            case 49:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_CLR);
                break;

            case 50:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_SLASH);
                break;

            case 51:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_DEL);
                break;

            case 52:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_SPACE);
                break;

            case 53:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_Z);
                break;

            case 54:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_PLUS_MINUS);
                break;

            case 55:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_0);
                break;

            case 56:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_DOT);
                break;

            case 57:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_L1);
                break;

            case 58:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_L2);
                break;

            case 59:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_L3);
                break;

            case 60:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_L4);
                break;

            case 61:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_L5);
                break;

            case 62:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_L6);
                break;

            case 63:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_INIT_REF);
                break;

            case 64:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_R1);
                break;

            case 65:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_R2);
                break;

            case 66:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_R3);
                break;

            case 67:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_R4);
                break;

            case 68:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_R5);
                break;

            case 69:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_R6);
                break;

            case 70:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_CDU_R_DES);
                break;
        }
    }
}