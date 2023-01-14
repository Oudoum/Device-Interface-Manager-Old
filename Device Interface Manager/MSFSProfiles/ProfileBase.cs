using System.Threading;

namespace Device_Interface_Manager.MSFSProfiles
{
    public class ProfileBase
    {
        protected CancellationTokenSource CancellationTokenSource { get; set; }

        protected Thread ReceiveSimConnectDataThread { get; set; }
    }
}