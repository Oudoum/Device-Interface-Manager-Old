using MobiFlight.SimConnectMSFS;
using System.Threading;

namespace Device_Interface_Manager.MSFSProfiles.WASM
{
    public abstract class USBWASM : USBBase
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

        public void Start(interfaceIT.USB.InterfaceIT_BoardInfo.Device device)
        {
            this.CancellationTokenSource = new CancellationTokenSource();
            this.MobiFlightSimConnectStart();
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
            this.MobiFlightSimConnectStop();
        }
    }
}