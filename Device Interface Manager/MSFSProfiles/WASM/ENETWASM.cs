using MobiFlight.SimConnectMSFS;
using System.Threading;

namespace Device_Interface_Manager.MSFSProfiles.WASM
{
    public abstract class ENETWASM : ENETBase
    {
        protected SimConnectCache MobiFlightSimConnect { get; set; }

        protected abstract void GetSimVar();

        protected void ReceiveSimConnectData(CancellationToken token)
        {
            while (true)
            {
                this.MobiFlightSimConnect?.ReceiveSimConnectMessage();
                Thread.Sleep(10);
                if (token.IsCancellationRequested)
                {
                    break;
                }
                this.GetSimVar();
                Thread.Sleep(10);
                if (token.IsCancellationRequested)
                {
                    break;
                }
            }
        }

        private void MobiFlightSimConnectStart()
        {
            this.MobiFlightSimConnect = new SimConnectCache();
            this.MobiFlightSimConnect?.Start();
            this.MobiFlightSimConnect?.Connect();
        }

        private void MobiFlightSimConnectStop()
        {
            this.MobiFlightSimConnect?.Disconnect();
            this.MobiFlightSimConnect?.Stop();
        }

        public void Start(string ipaddress)
        {
            this.CancellationTokenSource = new CancellationTokenSource();
            this.StartInterfaceITEthernetConnection(ipaddress);
            if (this.ConnectionStatus == 2)
            {
                this.StartinterfaceITEthernet();
                this.MobiFlightSimConnectStart();
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
            this.MobiFlightSimConnectStop();
        }
    }
}