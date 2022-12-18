using Device_Interface_Manager.interfaceIT.ENET;
using Device_Interface_Manager.MVVM.ViewModel;
using System.Threading;

namespace Device_Interface_Manager.Profiles
{
    public abstract class ENETBase
    {
        protected byte ConnectionStatus { get; private set; }

        protected CancellationTokenSource CancellationTokenSource { get; set; }

        protected Thread ReceiveSimConnectDataThread { get; set; }

        protected Thread ReceiveInterfaceITEthernetDataThread { get; set; }

        protected InterfaceITEthernet.INTERFACEIT_ETHERNET_KEY_NOTIFY_PROC INTERFACEIT_ETHERNET_KEY_NOTIFY_PROC { get; set; }

        protected InterfaceITEthernet InterfaceITEthernet { get; set; }

        protected abstract void GetSimVar();

        protected abstract void KeyPressedProcEthernet(int key, string direction);

        protected void StartInterfaceITEthernetConnection(InterfaceITEthernet interfaceITEthernet, string ipaddress)
        {
            interfaceITEthernet.Hostname = ipaddress;
            interfaceITEthernet?.InterfaceITEthernetConnection(this.CancellationTokenSource.Token);
            this.ConnectionStatus = interfaceITEthernet.ClientStatus;
        }

        protected void StartinterfaceITEthernet(InterfaceITEthernet interfaceITEthernet)
        {
            interfaceITEthernet?.GetinterfaceITEthernetDataStart();
            interfaceITEthernet?.GetinterfaceITEthernetInfo();
            MainViewModel.BoardinfoENETVM.InterfaceITEthernetInfoTextCollection.Add(interfaceITEthernet.InterfaceITEthernetInfoText); //change this later
            MainViewModel.BoardinfoENETVM.InterfaceITEthernetInfoIPCollection.Add(interfaceITEthernet.InterfaceITEthernetInfo.CLIENT); //change this later
        }
    }
}