using Microsoft.FlightSimulator.SimConnect;
using Newtonsoft.Json;
using System.IO;
using System.Threading;
using static Device_Interface_Manager.MSFSProfiles.PMDG.PMDG_NG3_SDK;

namespace Device_Interface_Manager.MSFSProfiles.PMDG.B737
{
    public class NG_CDU_L
    {
        private CancellationTokenSource CancellationTokenSource { get; set; }

        private Thread ReceiveSimConnectDataThread { get; set; }

        private SimConnectClient PMDGSimConnectClient { get; set; }

        private void ReceiveSimConnectData(CancellationToken token)
        {
            while (true)
            {
                this.PMDGSimConnectClient?.ReceiveSimConnectMessage();
                Thread.Sleep(10);
                if (token.IsCancellationRequested)
                {
                    break;
                }
            }
        }

        private void PMDGSimConnectStart()
        {
            this.PMDGSimConnectClient = new SimConnectClient();
            this.PMDGSimConnectClient?.SimConnect_Open();
            Thread.Sleep(500);
            this.PMDGSimConnectClient.Simconnect.OnRecvClientData += Simconnect_OnRecvClientData;
            PMDG737 pMDG737 = new();
            pMDG737.RegisterPMDGDataEvents(this.PMDGSimConnectClient.Simconnect);
            this.pMDG737CDU.Closing += PMDG737CDU_Closing;
            this.pMDG737CDU.Dispatcher.BeginInvoke(delegate ()
            {
                this.GetPMDG737CDUSettings();
                Thread.Sleep(500);
                this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_DOT, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                Thread.Sleep(500);
                this.PMDGSimConnectClient.Simconnect.TransmitClientEvent(0, PMDGEvents.EVT_CDU_L_CLR, MOUSE_FLAG_LEFTSINGLE, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                this.pMDG737CDU?.Show();
            });
        }

        private void PMDGSimConnectStop()
        {
            this.PMDGSimConnectClient?.SimConnect_Close();
        }

        public void Start()
        {
            this.CancellationTokenSource = new CancellationTokenSource();
            this.PMDGSimConnectStart();
            this.ReceiveSimConnectDataThread = new Thread(() => this.ReceiveSimConnectData(this.CancellationTokenSource.Token))
            {
                Name = this.ReceiveSimConnectDataThread?.ToString()
            };
            this.ReceiveSimConnectDataThread?.Start();
        }

        public void Stop()
        {
            this.PMDGSimConnectStop();
            this.pMDG737CDU?.Close();
        }

        private void Simconnect_OnRecvClientData(SimConnect sender, SIMCONNECT_RECV_CLIENT_DATA data)
        {
            if (((uint)DATA_REQUEST_ID.DATA_REQUEST) == data.dwRequestID)
            {
                if (!((PMDG_NG3_Data)data.dwData[0]).ELEC_BusPowered[3])
                {
                    this.pMDG737CDU?.Dispatcher.BeginInvoke(delegate ()
                    {
                        this.pMDG737CDU.ClearPMDGCDUCells();
                    });
                }
            }

            if ((uint)DATA_REQUEST_ID.CDU0_REQUEST == data.dwRequestID)
            {
                this.pMDG737CDU.Dispatcher.BeginInvoke(delegate ()
                {
                    this.pMDG737CDU.GetPMDGCDUCells((PMDG_NG3_CDU_Screen)data.dwData[0]);
                });
            }
        }

        private readonly MVVM.View.PMDG737CDU pMDG737CDU = new();

        private const string settings = @"Profiles\PMDG 737\CDU Screen L.json";
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
    }
}