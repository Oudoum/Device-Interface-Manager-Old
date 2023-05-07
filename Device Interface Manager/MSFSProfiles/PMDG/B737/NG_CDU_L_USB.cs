using System.IO;
using System.Windows;
using System.Threading;
using System.Text.Json;
using Microsoft.FlightSimulator.SimConnect;
using static Device_Interface_Manager.MSFSProfiles.PMDG.PMDG_NG3_SDK;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceITAPI_Data;
using System.Threading.Tasks;

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
                interfaceIT_LED_Set(device.Session, 1, _cDU_annunEXEC0 = value);
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
                interfaceIT_LED_Set(device.Session, 2, _cDU_annunCALL0 = value);
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
                interfaceIT_LED_Set(device.Session, 3, _cDU_annunMSG0 = value);
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
                interfaceIT_LED_Set(device.Session, 4, _cDU_annunFAIL0 = value);
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
                interfaceIT_LED_Set(device.Session, 5, _cDU_annunOFST0 = value);
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
                interfaceIT_Brightness_Set(device.Session, (int)((_lTS_PedPanelKnob = value) * 1.5));
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
                    interfaceIT_Brightness_Set(device.Session, (int)(LTS_PedPanelKnob * 1.5));
                }
                else if (!value)
                {
                    interfaceIT_Brightness_Set(device.Session, 0);
                }
            }
        }
    }

    public override void Stop()
    {
        base.Stop();
        pMDG737CDU?.Close();
    }

    protected override async Task StartSimConnect()
    {
        await base.StartSimConnect();
        PMDG737.RegisterPMDGDataEvents(simConnectClient.simConnect);
        Application.Current.Dispatcher.Invoke(delegate
        {
            pMDG737CDU = new();
        });
        pMDG737CDU.OnEditormodeOff += PMDG737CDU_EditormodeOff;
        pMDG737CDU.Closing += PMDG737CDU_Closing;
        await pMDG737CDU.Dispatcher.BeginInvoke(delegate ()
        {
            GetPMDG737CDUSettings();
            Thread.Sleep(500);
            simConnectClient.TransmitEvent(MOUSE_FLAG_LEFTSINGLE, PMDGEvents.EVT_CDU_L_DOT);
            Thread.Sleep(500);
            simConnectClient.TransmitEvent(MOUSE_FLAG_LEFTSINGLE, PMDGEvents.EVT_CDU_L_CLR);
            pMDG737CDU.Show();
            pMDG737CDU.WindowState = (WindowState)pMDG_737_CDU_Screen.Fullscreen;
            pMDG737CDU.CreatePMDGCDUCells();
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
    NG_CDU_Base pMDG_737_CDU_Screen = new();
    private void GetPMDG737CDUSettings()
    {
        if (File.Exists(settings))
        {
            pMDG_737_CDU_Screen = JsonSerializer.Deserialize<NG_CDU_Base>(File.ReadAllText(settings), new JsonSerializerOptions { NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals});
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

    protected override void KeyPressedProc(uint session, int key, uint direction)
    {
        if (direction == 1 || key == 49)
        {
            if (direction == 1)
            {
                direction = MOUSE_FLAG_LEFTSINGLE;
            }

            else if (direction == 0)
            {
                direction = MOUSE_FLAG_LEFTRELEASE;
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
}