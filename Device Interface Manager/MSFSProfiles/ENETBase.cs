﻿using Device_Interface_Manager.interfaceIT.ENET;
using Device_Interface_Manager.MVVM.ViewModel;
using System.Threading;

namespace Device_Interface_Manager.MSFSProfiles
{
    public abstract class ENETBase : ProfileBase
    {
        public byte ConnectionStatus { get; set; }

        protected Thread ReceiveInterfaceITEthernetDataThread { get; set; }

        protected InterfaceITEthernet.INTERFACEIT_ETHERNET_KEY_NOTIFY_PROC INTERFACEIT_ETHERNET_KEY_NOTIFY_PROC { get; set; }

        protected InterfaceITEthernet InterfaceITEthernet { get; set; }

        protected abstract void KeyPressedProcEthernet(int key, string direction);

        public virtual void Stop()
        {
            this.CancellationTokenSource?.Cancel();
            this.InterfaceITEthernet?.CloseStream();
        }

        protected void StartInterfaceITEthernetConnection(string ipaddress)
        {
            this.InterfaceITEthernet = new();
            this.InterfaceITEthernet.Hostname = ipaddress;
            this.InterfaceITEthernet?.InterfaceITEthernetConnection(this.CancellationTokenSource.Token);
            this.ConnectionStatus = this.InterfaceITEthernet.ClientStatus;
        }

        protected void StartinterfaceITEthernet()
        {
            this.InterfaceITEthernet?.GetinterfaceITEthernetDataStart();
            this.InterfaceITEthernet?.GetinterfaceITEthernetInfo();
            MainViewModel.BoardinfoENETVM.InterfaceITEthernetInfoTextCollection.Add(this.InterfaceITEthernet.InterfaceITEthernetInfoText); //change this later
            MainViewModel.BoardinfoENETVM.InterfaceITEthernetInfoIPCollection.Add(this.InterfaceITEthernet.InterfaceITEthernetInfo.CLIENT); //change this later
        }
    }
}