using MobiFlight.SimConnectMSFS;
using System.Threading;
using System.Threading.Tasks;

namespace Device_Interface_Manager.Profiles
{
    public abstract class ENETWASM : ENETBase
    {
        protected SimConnectCache MobiFlightSimConnect { get; set; }

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
                GetSimVar();
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

        public async void Start(string ipaddress)
        {
            this.CancellationTokenSource = new CancellationTokenSource();
            await Task.Run(() => this.StartInterfaceITEthernetConnection(this.InterfaceITEthernet = new(), ipaddress));
            if (this.ConnectionStatus == 2)
            {
                this.StartinterfaceITEthernet(this.InterfaceITEthernet);
                await Task.Run(() => this.MobiFlightSimConnectStart());
                this.ReceiveSimConnectDataThread = new Thread(() => this.ReceiveSimConnectData(this.CancellationTokenSource.Token))
                {
                    Name = this.ReceiveSimConnectDataThread?.ToString()
                };
                this.ReceiveSimConnectDataThread?.Start();
                this.ReceiveInterfaceITEthernetDataThread = new Thread(() => this.InterfaceITEthernet.GetinterfaceITEthernetData(this.INTERFACEIT_ETHERNET_KEY_NOTIFY_PROC = new(KeyPressedProcEthernet), this.CancellationTokenSource.Token))
                {
                    Name = this.ReceiveInterfaceITEthernetDataThread?.ToString()
                };
                this.ReceiveInterfaceITEthernetDataThread?.Start();
            }
        }

        public void Stop()
        {
            this.CancellationTokenSource?.Cancel();
            this.MobiFlightSimConnectStop();
        }
    }
}