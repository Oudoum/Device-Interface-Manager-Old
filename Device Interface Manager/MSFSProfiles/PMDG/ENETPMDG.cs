using System.Threading;

namespace Device_Interface_Manager.MSFSProfiles.PMDG
{
    public abstract class ENETPMDG : ENETBase
    {
        public MVVM.View.PMDG737CDU pMDG737CDU;

        protected abstract void Simconnect_OnRecvClientData(Microsoft.FlightSimulator.SimConnect.SimConnect sender, Microsoft.FlightSimulator.SimConnect.SIMCONNECT_RECV_CLIENT_DATA data);

        protected SimConnectClient PMDGSimConnectClient { get; set; }

        protected void ReceiveSimConnectData(CancellationToken token)
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

        protected virtual void PMDGSimConnectStart()
        {
            this.PMDGSimConnectClient = new SimConnectClient();
            this.PMDGSimConnectClient?.SimConnect_Open();
            this.PMDGSimConnectClient.Simconnect.OnRecvClientData += Simconnect_OnRecvClientData;
        }

        private void PMDGSimConnectStop()
        {
            this.PMDGSimConnectClient?.SimConnect_Close();
        }

        public void Start(string ipaddress)
        {
            this.CancellationTokenSource = new CancellationTokenSource();
            this.StartInterfaceITEthernetConnection(ipaddress);
            if (this.ConnectionStatus == 2)
            {
                this.StartinterfaceITEthernet();
                this.PMDGSimConnectStart();
                this.ReceiveSimConnectDataThread = new Thread(() => this.ReceiveSimConnectData(this.CancellationTokenSource.Token))
                {
                    Name = this.ReceiveSimConnectDataThread?.ToString()
                };
                this.ReceiveSimConnectDataThread?.Start();
                this.ReceiveInterfaceITEthernetDataThread = new Thread(() => this.InterfaceITEthernet.GetinterfaceITEthernetData(this.INTERFACEIT_ETHERNET_KEY_NOTIFY_PROC = new(this.KeyPressedProcEthernet), this.CancellationTokenSource.Token))
                {
                    Name = this.ReceiveInterfaceITEthernetDataThread?.ToString()
                };
                this.ReceiveInterfaceITEthernetDataThread?.Start();
            }
        }

        public override void Stop()
        {
            base.Stop();
            this.PMDGSimConnectStop();
        }
    }
}