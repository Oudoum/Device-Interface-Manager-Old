using System.Threading;

namespace Device_Interface_Manager.MSFSProfiles.PMDG
{
    public abstract class USBPMDG : USBBase
    {
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

        public void Start(interfaceIT.USB.InterfaceIT_BoardInfo.Device device)
        {
            this.CancellationTokenSource = new CancellationTokenSource();
            this.PMDGSimConnectStart();
            this.ReceiveSimConnectDataThread = new Thread(() => this.ReceiveSimConnectData(this.CancellationTokenSource.Token))
            {
                Name = this.ReceiveSimConnectDataThread?.ToString()
            };
            this.ReceiveSimConnectDataThread?.Start();
            this.StartUSBConnection(device);
        }

        public override void Stop()
        {
            base.Stop();
            this.PMDGSimConnectStop();
        }
    }
}