﻿using System;
using System.IO;
using System.Threading;
using Microsoft.FlightSimulator.SimConnect;
using Newtonsoft.Json;
using static Device_Interface_Manager.MSFSProfiles.PMDG.PMDG_NG3_SDK;

namespace Device_Interface_Manager.MSFSProfiles.PMDG.B737
{
    public class NG_CDU_R_E : ENETPMDG
    {
        private bool _cDU_annunEXEC1;
        private bool CDU_annunEXEC1
        {
            set
            {
                if (this._cDU_annunEXEC1 != value)
                {
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(1, Convert.ToInt32(this._cDU_annunEXEC1 = value));
                }
            }
        }

        private bool _cDU_annunCALL1;
        private bool CDU_annunCALL1
        {
            set
            {
                if (this._cDU_annunCALL1 != value)
                {
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(2, Convert.ToInt32(this._cDU_annunCALL1 = value));
                }
            }
        }

        private bool _cDU_annunMSG1;
        private bool CDU_annunMSG1
        {
            set
            {
                if (this._cDU_annunMSG1 != value)
                {
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(3, Convert.ToInt32(this._cDU_annunMSG1 = value));
                }
            }
        }

        private bool _cDU_annunFAIL1;
        private bool CDU_annunFAIL1
        {
            set
            {
                if (this._cDU_annunFAIL1 != value)
                {
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(4, Convert.ToInt32(this._cDU_annunFAIL1 = value));
                }
            }
        }

        private bool _cDU_annunOFST1;
        private bool CDU_annunOFST1
        {
            set
            {
                if (this._cDU_annunOFST1 != value)
                {
                    this.InterfaceITEthernet.SendintefaceITEthernetLED(5, Convert.ToInt32(this._cDU_annunOFST1 = value));
                }
            }
        }

        public override void Stop()
        {
            base.Stop();
            this.pMDG737CDU?.Close();
        }

        protected override void PMDGSimConnectStart()
        {
            base.PMDGSimConnectStart();
            PMDG737 pMDG737 = new();
            pMDG737.RegisterPMDGDataEvents(this.PMDGSimConnectClient.Simconnect);
            this.pMDG737CDU.Closing += PMDG737CDU_Closing;
            this.pMDG737CDU.Dispatcher.BeginInvoke(delegate ()
            {
                this.GetPMDG737CDUSettings();
                Thread.Sleep(500);
                this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_DOT, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                Thread.Sleep(500);
                this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_CLR, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                this.pMDG737CDU?.Show();
            });
        }

        private readonly MVVM.View.PMDG737CDU pMDG737CDU = new();

        protected override void Simconnect_OnRecvClientData(SimConnect sender, SIMCONNECT_RECV_CLIENT_DATA data)
        {
            if (((uint)DATA_REQUEST_ID.DATA_REQUEST) == data.dwRequestID)
            {
                this.CDU_annunEXEC1 = ((PMDG_NG3_Data)data.dwData[0]).CDU_annunEXEC[1];
                this.CDU_annunCALL1 = ((PMDG_NG3_Data)data.dwData[0]).CDU_annunCALL[1];
                this.CDU_annunMSG1 = ((PMDG_NG3_Data)data.dwData[0]).CDU_annunMSG[1];
                this.CDU_annunFAIL1 = ((PMDG_NG3_Data)data.dwData[0]).CDU_annunFAIL[1];
                this.CDU_annunOFST1 = ((PMDG_NG3_Data)data.dwData[0]).CDU_annunOFST[1];
                if (!((PMDG_NG3_Data)data.dwData[0]).ELEC_BusPowered[3])
                {
                    this.pMDG737CDU?.Dispatcher.BeginInvoke(delegate ()
                    {
                        this.pMDG737CDU.ClearPMDGCDUCells();
                    });
                }
            }

            if ((uint)DATA_REQUEST_ID.CDU1_REQUEST == data.dwRequestID)
            {
                this.pMDG737CDU.Dispatcher.BeginInvoke(delegate ()
                {
                    this.pMDG737CDU.GetPMDGCDUCells((PMDG_NG3_CDU_Screen)data.dwData[0]);
                });
            }
        }

        private const string settings = @"Profiles\PMDG 737\ENET CDU Screen R.json";
        PMDG_737_CDU_Screen pMDG_737_CDU_Screen = new();
        private void GetPMDG737CDUSettings()
        {
            if (File.Exists(settings))
            {
                this.pMDG_737_CDU_Screen = JsonConvert.DeserializeObject<PMDG_737_CDU_Screen>(File.ReadAllText(settings));
            }
            this.pMDG_737_CDU_Screen.Load(this.pMDG737CDU);
        }

        private void PMDG737CDU_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.pMDG_737_CDU_Screen.Save(this.pMDG737CDU);
            string json = JsonConvert.SerializeObject(pMDG_737_CDU_Screen, Formatting.Indented);
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

        protected override void KeyPressedProcEthernet(int Switch, string Direction)
        {
            uint oDirection = 0;
            if (Direction == "ON")
            {
                oDirection = MOUSE_FLAG_LEFTSINGLE;
            }

            else if (Direction == "OFF" && Switch == 8)
            {
                oDirection = MOUSE_FLAG_LEFTRELEASE;
            }

            else if (Direction == "OFF" && Switch == 63)
            {
                this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_BRITENESS, 1, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
            }
            else if (Direction == "OFF" && Switch == 64)
            {
                this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_BRITENESS, 1, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
            }

            if (oDirection == 0)
            {
                return;
            }

            switch (Switch)
            {
                case 1:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_DOT, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 2:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_0, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 3:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_PLUS_MINUS, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 4:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_Z, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 5:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_SPACE, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 6:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_DEL, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 7:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_SLASH, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 8:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_CLR, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 9:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_7, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 10:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_8, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 11:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_9, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 12:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_U, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 13:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_V, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 14:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_W, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 15:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_X, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 16:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_Y, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 17:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_4, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 18:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_5, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 19:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_6, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 20:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_P, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 21:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_Q, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 22:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_R, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 23:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_S, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 24:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_T, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 25:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_1, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 26:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_2, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 27:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_3, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 28:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_K, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 29:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_L, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 30:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_M, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 31:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_N, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 32:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_O, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 33:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_PREV_PAGE, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 34:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_NEXT_PAGE, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 35:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_L1, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 36:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_F, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 37:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_G, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 38:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_H, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 39:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_I, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 40:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_J, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 41:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_N1_LIMIT, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 42:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_FIX, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 43:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_L2, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 44:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_A, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 45:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_B, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 46:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_C, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 47:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_D, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 48:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_E, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 49:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_MENU, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 50:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_LEGS, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 51:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_L3, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 52:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_DEP_ARR, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 53:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_HOLD, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 54:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_PROG, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 55:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_EXEC, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 56:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_L5, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 57:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_INIT_REF, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 58:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_RTE, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 59:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_L4, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 60:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_CLB, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 61:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_CRZ, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 62:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_DES, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 63 when Direction == "ON":
                    if (Direction == "ON")
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_BRITENESS, 2, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 64 when Direction == "ON":
                    if (Direction == "ON")
                        this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_BRITENESS, 0, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 65:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_R1, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 66:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_R2, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 67:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_R3, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 68:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_R4, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 69:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_R5, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 70:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_R6, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                case 71:
                    this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_R_L6, oDirection, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                    break;

                default:
                    break;

            }
        }
    }
}