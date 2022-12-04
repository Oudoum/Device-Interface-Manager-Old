using System;
using System.Threading;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceITAPI_Data;

namespace Device_Interface_Manager.MVVM.Model
{
    public class MSFS_FBW_A32NX_MCDU_USB
    {
        public static class MSFS_FBW_A32NX_Captain_MCDU_Data
        {
            public static Thread ReceivedDataThread { get; set; }

            public static void ReceiveDataThread(int Session, CancellationToken token)
            {
                while (true)
                {
                    HomeModel.simConnectCache.ReceiveSimConnectMessage();
                    Thread.Sleep(10);
                    GetSimVar(Session);
                    Thread.Sleep(10);
                    if (token.IsCancellationRequested) 
                    {
                        return;
                    }
                }
            }

            //Get SimVars
            private static void GetSimVar(int Session)
            {
                // Background light
                interfaceIT_Brightness_Set(Session, (int)(HomeModel.simConnectCache.GetSimVar("(A:LIGHT POTENTIOMETER:85, Percent)") * 2.55));

                // Brightness Test
                if (HomeModel.simConnectCache.GetSimVar("(L:A32NX_OVHD_INTLT_ANN)") == 0 &&
                    HomeModel.simConnectCache.GetSimVar("(L:A32NX_ELEC_AC_ESS_SHED_BUS_IS_POWERED)") == 1)
                {
                    interfaceIT_LED_Set(Session, 2, true);
                    interfaceIT_LED_Set(Session, 3, true);
                    interfaceIT_LED_Set(Session, 4, true);
                    interfaceIT_LED_Set(Session, 5, true);
                    interfaceIT_LED_Set(Session, 9, true);
                    interfaceIT_LED_Set(Session, 10, true);
                    interfaceIT_LED_Set(Session, 11, true);
                    interfaceIT_LED_Set(Session, 13, true);
                    return;
                }

                // Output LEDs
                interfaceIT_LED_Set(Session, 2, Convert.ToBoolean(HomeModel.simConnectCache.GetSimVar("(L:A32NX_MCDU_L_ANNUNC_FAIL)")));

                interfaceIT_LED_Set(Session, 3, Convert.ToBoolean(HomeModel.simConnectCache.GetSimVar("(L:A32NX_MCDU_L_ANNUNC_FMGC)")));

                interfaceIT_LED_Set(Session, 4, Convert.ToBoolean(HomeModel.simConnectCache.GetSimVar("(L:A32NX_MCDU_L_ANNUNC_FM)")));

                interfaceIT_LED_Set(Session, 5, Convert.ToBoolean(HomeModel.simConnectCache.GetSimVar("(L:A32NX_MCDU_L_ANNUNC_MCDU_MENU)")));

                interfaceIT_LED_Set(Session, 9, Convert.ToBoolean(HomeModel.simConnectCache.GetSimVar("(L:A32NX_MCDU_L_ANNUNC_FM1)")));

                interfaceIT_LED_Set(Session, 10, Convert.ToBoolean(HomeModel.simConnectCache.GetSimVar("(L:A32NX_MCDU_L_ANNUNC_IND)")));

                interfaceIT_LED_Set(Session, 11, Convert.ToBoolean(HomeModel.simConnectCache.GetSimVar("(L:A32NX_MCDU_L_ANNUNC_RDY)")));

                interfaceIT_LED_Set(Session, 13, Convert.ToBoolean(HomeModel.simConnectCache.GetSimVar("(L:A32NX_MCDU_L_ANNUNC_FM2)")));
            }
        }

        public static class MSFS_FBW_A32NX_Captain_MCDU_Events
        {
            public static INTERFACEIT_KEY_NOTIFY_PROC KeyNotifyCallback { get; set; } = new(KeyPressedProc);

            //Delegate for button presses
            private static bool KeyPressedProc(int Session, int Switch, int Direction)
            {

                if (Direction == 1)
                {
                }

                if (Direction == 0)
                {
                    return true;
                }

                switch (Switch)
                {
                    case 1:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_MENU");
                        break;

                    case 2:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_ATC");
                        break;

                    case 3:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_SEC");
                        break;

                    case 4:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_INIT");
                        break;

                    case 5:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_FUEL");
                        break;

                    case 6:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_RAD");
                        break;

                    case 7:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_FPLN");
                        break;

                    case 8:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_PERF");
                        break;

                    case 9:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_E");
                        break;

                    case 10:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_D");
                        break;

                    case 11:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_C");
                        break;

                    case 12:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_B");
                        break;

                    case 13:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_A");
                        break;

                    case 14:

                        break;

                    case 15:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_AIRPORT");
                        break;

                    case 16:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_PROG");
                        break;

                    case 17:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_J");
                        break;

                    case 18:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_I");
                        break;

                    case 19:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_H");
                        break;

                    case 20:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_G");
                        break;

                    case 21:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_F");
                        break;

                    case 22:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_UP");
                        break;

                    case 23:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_PREVPAGE");
                        break;

                    case 24:

                        break;

                    case 25:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_O");
                        break;

                    case 26:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_N");
                        break;

                    case 27:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_M");
                        break;

                    case 28:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_L");
                        break;

                    case 29:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_K");
                        break;

                    case 30:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_3");
                        break;

                    case 31:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_2");
                        break;

                    case 32:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_1");
                        break;

                    case 33:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_T");
                        break;

                    case 34:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_S");
                        break;

                    case 35:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_R");
                        break;

                    case 36:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_Q");
                        break;

                    case 37:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_P");
                        break;

                    case 38:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_6");
                        break;

                    case 39:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_5");
                        break;

                    case 40:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_4");
                        break;

                    case 41:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_Y");
                        break;

                    case 42:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_X");
                        break;

                    case 43:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_W");
                        break;

                    case 44:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_V");
                        break;

                    case 45:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_U");
                        break;

                    case 46:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_9");
                        break;

                    case 47:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_8");
                        break;

                    case 48:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_7");
                        break;

                    case 49:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_CLR");
                        break;

                    case 50:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_OVFY");
                        break;

                    case 51:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_SP");
                        break;

                    case 52:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_DIV");
                        break;

                    case 53:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_Z");
                        break;

                    case 54:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_PLUSMINUS");
                        break;

                    case 55:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_0");
                        break;

                    case 56:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_DOT");
                        break;

                    case 57:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_L1");
                        break;

                    case 58:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_L2");
                        break;

                    case 59:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_L3");
                        break;

                    case 60:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_L4");
                        break;

                    case 61:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_L5");
                        break;

                    case 62:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_L6");
                        break;

                    case 63:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_DIR");
                        break;

                    case 64:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_R1");
                        break;

                    case 65:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_R2");
                        break;

                    case 66:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_R3");
                        break;

                    case 67:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_R4");
                        break;

                    case 68:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_R5");
                        break;

                    case 69:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_R6");
                        break;

                    case 70:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_DATA");
                        break;

                    case 71:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_DOWN");
                        break;

                    case 72:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_1_BTN_NEXTPAGE");
                        break;

                    default:
                        break;
                }
                return true;
            }
        }

        public static class MSFS_FBW_A32NX_Firstofficer_MCDU_Data
        {
            public static Thread ReceivedDataThread { get; set; }

            public static void ReceiveDataThread(int Session, CancellationToken token)
            {
                while (true)
                {
                    HomeModel.simConnectCache.ReceiveSimConnectMessage();
                    Thread.Sleep(10);
                    GetSimVar(Session);
                    Thread.Sleep(10);
                    if (token.IsCancellationRequested) 
                    {
                        return;
                    }
                }
            }

            //Get SimVars
            private static void GetSimVar(int Session)
            {
                // Background light
                interfaceIT_Brightness_Set(Session, (int)(HomeModel.simConnectCache.GetSimVar("(A:LIGHT POTENTIOMETER:85, Percent)") * 2.55));

                // Brightness Test
                if (HomeModel.simConnectCache.GetSimVar("(L:A32NX_OVHD_INTLT_ANN)") == 0 &&
                    HomeModel.simConnectCache.GetSimVar("(L:A32NX_ELEC_AC_ESS_SHED_BUS_IS_POWERED)") == 1)
                {
                    interfaceIT_LED_Set(Session, 2, true);
                    interfaceIT_LED_Set(Session, 3, true);
                    interfaceIT_LED_Set(Session, 4, true);
                    interfaceIT_LED_Set(Session, 5, true);
                    interfaceIT_LED_Set(Session, 9, true);
                    interfaceIT_LED_Set(Session, 10, true);
                    interfaceIT_LED_Set(Session, 11, true);
                    interfaceIT_LED_Set(Session, 13, true);
                    return;
                }

                // Output LEDs
                interfaceIT_LED_Set(Session, 2, Convert.ToBoolean(HomeModel.simConnectCache.GetSimVar("(L:A32NX_MCDU_R_ANNUNC_FAIL)")));

                interfaceIT_LED_Set(Session, 3, Convert.ToBoolean(HomeModel.simConnectCache.GetSimVar("(L:A32NX_MCDU_R_ANNUNC_FMGC)")));

                interfaceIT_LED_Set(Session, 4, Convert.ToBoolean(HomeModel.simConnectCache.GetSimVar("(L:A32NX_MCDU_R_ANNUNC_FM)")));

                interfaceIT_LED_Set(Session, 5, Convert.ToBoolean(HomeModel.simConnectCache.GetSimVar("(L:A32NX_MCDU_R_ANNUNC_MCDU_MENU)")));

                interfaceIT_LED_Set(Session, 9, Convert.ToBoolean(HomeModel.simConnectCache.GetSimVar("(L:A32NX_MCDU_R_ANNUNC_FM1)")));

                interfaceIT_LED_Set(Session, 10, Convert.ToBoolean(HomeModel.simConnectCache.GetSimVar("(L:A32NX_MCDU_R_ANNUNC_IND)")));

                interfaceIT_LED_Set(Session, 11, Convert.ToBoolean(HomeModel.simConnectCache.GetSimVar("(L:A32NX_MCDU_R_ANNUNC_RDY)")));

                interfaceIT_LED_Set(Session, 13, Convert.ToBoolean(HomeModel.simConnectCache.GetSimVar("(L:A32NX_MCDU_R_ANNUNC_FM2)")));
            }
        }

        public static class MSFS_FBW_A32NX_Firstofficer_MCDU_Events
        {
            public static INTERFACEIT_KEY_NOTIFY_PROC KeyNotifyCallback { get; set; } = new(KeyPressedProc);

            //Delegate for button presses
            private static bool KeyPressedProc(int Session, int Switch, int Direction)
            {

                if (Direction == 1)
                {
                }

                if (Direction == 0)
                {
                    return true;
                }

                switch (Switch)
                {
                    case 1:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_MENU");
                        break;

                    case 2:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_ATC");
                        break;

                    case 3:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_SEC");
                        break;

                    case 4:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_INIT");
                        break;

                    case 5:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_FUEL");
                        break;

                    case 6:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_RAD");
                        break;

                    case 7:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_FPLN");
                        break;

                    case 8:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_PERF");
                        break;

                    case 9:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_E");
                        break;

                    case 10:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_D");
                        break;

                    case 11:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_C");
                        break;

                    case 12:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_B");
                        break;

                    case 13:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_A");
                        break;

                    case 14:

                        break;

                    case 15:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_AIRPORT");
                        break;

                    case 16:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_PROG");
                        break;

                    case 17:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_J");
                        break;

                    case 18:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_I");
                        break;

                    case 19:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_H");
                        break;

                    case 20:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_G");
                        break;

                    case 21:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_F");
                        break;

                    case 22:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_UP");
                        break;

                    case 23:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_PREVPAGE");
                        break;

                    case 24:

                        break;

                    case 25:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_O");
                        break;

                    case 26:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_N");
                        break;

                    case 27:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_M");
                        break;

                    case 28:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_L");
                        break;

                    case 29:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_K");
                        break;

                    case 30:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_3");
                        break;

                    case 31:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_2");
                        break;

                    case 32:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_1");
                        break;

                    case 33:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_T");
                        break;

                    case 34:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_S");
                        break;

                    case 35:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_R");
                        break;

                    case 36:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_Q");
                        break;

                    case 37:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_P");
                        break;

                    case 38:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_6");
                        break;

                    case 39:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_5");
                        break;

                    case 40:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_4");
                        break;

                    case 41:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_Y");
                        break;

                    case 42:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_X");
                        break;

                    case 43:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_W");
                        break;

                    case 44:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_V");
                        break;

                    case 45:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_U");
                        break;

                    case 46:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_9");
                        break;

                    case 47:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_8");
                        break;

                    case 48:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_7");
                        break;

                    case 49:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_CLR");
                        break;

                    case 50:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_OVFY");
                        break;

                    case 51:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_SP");
                        break;

                    case 52:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_DIV");
                        break;

                    case 53:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_Z");
                        break;

                    case 54:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_PLUSMINUS");
                        break;

                    case 55:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_0");
                        break;

                    case 56:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_DOT");
                        break;

                    case 57:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_L1");
                        break;

                    case 58:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_L2");
                        break;

                    case 59:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_L3");
                        break;

                    case 60:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_L4");
                        break;

                    case 61:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_L5");
                        break;

                    case 62:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_L6");
                        break;

                    case 63:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_DIR");
                        break;

                    case 64:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_R1");
                        break;

                    case 65:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_R2");
                        break;

                    case 66:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_R3");
                        break;

                    case 67:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_R4");
                        break;

                    case 68:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_R5");
                        break;

                    case 69:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_R6");
                        break;

                    case 70:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_DATA");
                        break;

                    case 71:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_DOWN");
                        break;

                    case 72:
                        HomeModel.simConnectCache.SetEventID("A320_Neo_CDU_2_BTN_NEXTPAGE");
                        break;

                    default:
                        break;
                }
                return true;
            }
        }
    }
}