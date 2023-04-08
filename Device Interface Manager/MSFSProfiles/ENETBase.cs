using System.Threading;
using Device_Interface_Manager.interfaceIT.ENET;

namespace Device_Interface_Manager.MSFSProfiles;

public abstract class ENETBase : ProfileBase
{
    public byte ConnectionStatus { get; set; }

    protected Thread ReceiveInterfaceITEthernetDataThread { get; set; }

    protected InterfaceITEthernet.INTERFACEIT_ETHERNET_KEY_NOTIFY_PROC INTERFACEIT_ETHERNET_KEY_NOTIFY_PROC { get; set; }

    public InterfaceITEthernet InterfaceITEthernet { get; set; }

    protected abstract void KeyPressedProcEthernet(int key, string direction);

    public virtual void Stop()
    {
        CancellationTokenSource?.Cancel();
        InterfaceITEthernet?.CloseStream();
    }

    protected void StartInterfaceITEthernetConnection(string ipaddress)
    {
        InterfaceITEthernet = new()
        {
            Hostname = ipaddress
        };
        InterfaceITEthernet?.InterfaceITEthernetConnection(CancellationTokenSource.Token);
        ConnectionStatus = InterfaceITEthernet.ClientStatus;
    }

    protected void StartinterfaceITEthernet()
    {
        InterfaceITEthernet?.GetinterfaceITEthernetDataStart();
        InterfaceITEthernet?.GetinterfaceITEthernetInfo();
    }
}