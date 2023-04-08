using System;
using System.IO;
using System.Windows;
using System.Threading;
using System.Text.Json;
using Microsoft.FlightSimulator.SimConnect;
using static Device_Interface_Manager.MSFSProfiles.PMDG.PMDG_NG3_SDK;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceITAPI_Data;

namespace Device_Interface_Manager.MSFSProfiles.PMDG.B737;

public class NG_CDU_L_USB : USB
{
    private bool _cDU_annunEXEC0;
    private bool CDU_annunEXEC0
    {
        set
        {
            if (_cDU_annunEXEC0 != value)
            {
                _ = interfaceIT_LED_Set(Device.Session, 1, _cDU_annunEXEC0 = value);
            }
        }
    }

    private bool _cDU_annunCALL0;
    private bool CDU_annunCALL0
    {
        set
        {
            if (_cDU_annunCALL0 != value)
            {
                _ = interfaceIT_LED_Set(Device.Session, 2, _cDU_annunCALL0 = value);
            }
        }
    }

    private bool _cDU_annunMSG0;
    private bool CDU_annunMSG0
    {
        set
        {
            if (_cDU_annunMSG0 != value)
            {
                _ = interfaceIT_LED_Set(Device.Session, 3, _cDU_annunMSG0 = value);
            }
        }
    }

    private bool _cDU_annunFAIL0;
    private bool CDU_annunFAIL0
    {
        set
        {
            if (_cDU_annunFAIL0 != value)
            {
                _ = interfaceIT_LED_Set(Device.Session, 4, _cDU_annunFAIL0 = value);
            }
        }
    }

    private bool _cDU_annunOFST0;
    private bool CDU_annunOFST0
    {
        set
        {
            if (_cDU_annunOFST0 != value)
            {
                _ = interfaceIT_LED_Set(Device.Session, 5, _cDU_annunOFST0 = value);
            }
        }
    }

    private byte _lTS_PedPanelKnob;
    private byte LTS_PedPanelKnob
    {
        get => _lTS_PedPanelKnob;
        set
        {
            if (_lTS_PedPanelKnob != value && ELEC_BusPowered_3)
            {
                _ = interfaceIT_Brightness_Set(Device.Session, (int)((_lTS_PedPanelKnob = value) * 1.5));
            }
        }
    }

    private bool _eLEC_BusPowered_3;
    private bool ELEC_BusPowered_3
    {
        get => _eLEC_BusPowered_3;
        set
        {
            if (_eLEC_BusPowered_3 != value)
            {
                _eLEC_BusPowered_3 = value;
                if (value)
                {
                    _ = interfaceIT_Brightness_Set(Device.Session, (int)(LTS_PedPanelKnob * 1.5));
                }
                else if (!value)
                {
                    _ = interfaceIT_Brightness_Set(Device.Session, 0);
                }
            }
        }
    }

    public override void Stop()
    {
        base.Stop();
        pMDG737CDU?.Close();
    }

    protected override void StartSimConnect()
    {
        base.StartSimConnect();
        PMDG737.RegisterPMDGDataEvents(simConnectClient.simConnect);
        Application.Current.Dispatcher.Invoke((Action)delegate
        {
            pMDG737CDU = new();
        });
        pMDG737CDU.EditormodeOff += PMDG737CDU_EditormodeOff;
        pMDG737CDU.Closing += PMDG737CDU_Closing;
        pMDG737CDU.Dispatcher.BeginInvoke(delegate ()
        {
            GetPMDG737CDUSettings();
            Thread.Sleep(500);
            simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_DOT, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
            Thread.Sleep(500);
            simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_CLR, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
            pMDG737CDU.Show();
            pMDG737CDU.WindowState = (WindowState)pMDG_737_CDU_Screen.Fullscreen;
        });
    }

    private void PMDG737CDU_EditormodeOff(object sender, System.EventArgs e)
    {
        SaveScreenProperties();
    }

    protected override void Simconnect_OnRecvClientData(SimConnect sender, SIMCONNECT_RECV_CLIENT_DATA data)
    {
        if (((uint)DATA_REQUEST_ID.DATA_REQUEST) == data.dwRequestID)
        {
            CDU_annunEXEC0 = ((PMDG_NG3_Data)data.dwData[0]).CDU_annunEXEC[0];
            CDU_annunCALL0 = ((PMDG_NG3_Data)data.dwData[0]).CDU_annunCALL[0];
            CDU_annunMSG0 = ((PMDG_NG3_Data)data.dwData[0]).CDU_annunMSG[0];
            CDU_annunFAIL0 = ((PMDG_NG3_Data)data.dwData[0]).CDU_annunFAIL[0];
            CDU_annunOFST0 = ((PMDG_NG3_Data)data.dwData[0]).CDU_annunOFST[0];
            LTS_PedPanelKnob = ((PMDG_NG3_Data)data.dwData[0]).LTS_PedPanelKnob;
            ELEC_BusPowered_3 = ((PMDG_NG3_Data)data.dwData[0]).ELEC_BusPowered[3];
            if (!ELEC_BusPowered_3)
            {
                pMDG737CDU?.Dispatcher.BeginInvoke(delegate ()
                {
                    pMDG737CDU.ClearPMDGCDUCells();
                });
            }
        }

        if ((uint)DATA_REQUEST_ID.CDU0_REQUEST == data.dwRequestID)
        {
            pMDG737CDU.Dispatcher.BeginInvoke(delegate ()
            {
                pMDG737CDU.GetPMDGCDUCells((PMDG_NG3_CDU_Screen)data.dwData[0]);
            });
        }
    }

    private const string settings = @"Profiles\PMDG 737\USB CDU Screen L.json";
    PMDG_737_CDU_Screen pMDG_737_CDU_Screen = new();
    private void GetPMDG737CDUSettings()
    {
        if (File.Exists(settings))
        {
            pMDG_737_CDU_Screen = JsonSerializer.Deserialize<PMDG_737_CDU_Screen>(File.ReadAllText(settings), new JsonSerializerOptions { NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals});
        }
        pMDG_737_CDU_Screen.Load(pMDG737CDU);
    }

    private void PMDG737CDU_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        SaveScreenProperties();
    }

    private void SaveScreenProperties()
    {
        pMDG_737_CDU_Screen.Save(pMDG737CDU);
        string json = JsonSerializer.Serialize(pMDG_737_CDU_Screen, new JsonSerializerOptions { WriteIndented = true, NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals });
        Directory.CreateDirectory(settings.Remove(17));
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

    protected override bool KeyPressedProc(int session, int key, int direction)
    {
        uint Direction = (uint)direction;

        if (Direction == 1 || key == 49)
        {
            if (Direction == 1)
            {
                Direction = MOUSE_FLAG_LEFTSINGLE;
            }

            else if (Direction == 0)
            {
                Direction = MOUSE_FLAG_LEFTRELEASE;
            }

            switch (key)
            {
                case 1:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_EXEC, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 2:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_PROG, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 3:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_HOLD, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 4:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_CRZ, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 5:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_DEP_ARR, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 6:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_LEGS, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 7:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_MENU, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 8:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_CLB, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 9:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_E, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 10:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_D, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 11:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_C, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 12:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_B, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 13:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_A, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 14:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_FIX, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 15:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_N1_LIMIT, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 16:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_RTE, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 17:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_J, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 18:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_I, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 19:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_H, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 20:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_G, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 21:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_F, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 22:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_NEXT_PAGE, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 23:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_PREV_PAGE, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 25:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_O, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 26:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_N, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 27:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_M, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 28:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_L, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 29:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_K, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 30:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_3, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 31:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_2, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 32:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_1, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 33:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_T, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 34:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_S, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 35:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_R, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 36:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_Q, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 37:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_P, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 38:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_6, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 39:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_5, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 40:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_4, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 41:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_Y, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 42:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_X, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 43:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_W, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 44:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_V, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 45:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_U, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 46:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_9, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 47:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_8, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 48:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_7, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 49:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_CLR, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 50:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_SLASH, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 51:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_DEL, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 52:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_SPACE, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 53:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_Z, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 54:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_PLUS_MINUS, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 55:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_0, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 56:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_DOT, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 57:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_L1, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 58:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_L2, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 59:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_L3, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 60:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_L4, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 61:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_L5, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 62:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_L6, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 63:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_INIT_REF, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 64:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_R1, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 65:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_R2, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 66:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_R3, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 67:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_R4, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 68:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_R5, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 69:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_R6, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 70:
                    simConnectClient.simConnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_DES, Direction, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                default:
                    break;
            }
        }
        return true;
    }
}