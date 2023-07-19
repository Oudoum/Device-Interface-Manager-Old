using System;
using System.Threading.Tasks;
using Device_Interface_Manager.interfaceIT.ENET;

namespace Device_Interface_Manager.MSFSProfiles;

public abstract class ENET : IITProfileBase
{
    public InterfaceITEthernet.ConnectionStatus ConnectionStatus { get; set; }

    public InterfaceITEthernet interfaceITEthernet;

    public Action<int, uint> interfacITKeyAction;

    protected Task receiveInterfaceITEthernetDataTask;

    protected abstract void KeyPressedAction(int key, uint direction);

    public async Task<InterfaceITEthernet.ConnectionStatus> Start(string ipaddress)
    {
        interfaceITEthernet = new() { HostIPAddress = ipaddress };
        ConnectionStatus = interfaceITEthernet.InterfaceITEthernetConnectionAsync(cancellationTokenSource.Token).Result;
        if (ConnectionStatus == InterfaceITEthernet.ConnectionStatus.Connected)
        {
            await StartSimConnectAsync();
            receiveInterfaceITEthernetDataTask = Task.Run(async () => await interfaceITEthernet.GetInterfaceITEthernetDataAsync(interfacITKeyAction = KeyPressedAction, cancellationTokenSource.Token));
        }
        return ConnectionStatus;
    }

    public override void Stop()
    {
        base.Stop();
        simConnectClient.SimConnect_Close();
        cancellationTokenSource.Cancel();
        interfaceITEthernet.CloseStream();
    }
}