using System;
using System.Threading.Tasks;
using Device_Interface_Manager.Devices.interfaceIT.ENET;

namespace Device_Interface_Manager.SimConnectProfiles;

public abstract class ENET : ProfileBase<InterfaceITEthernet>
{
    protected Action<int, uint> interfacITKeyAction;

    protected Task receiveInterfaceITEthernetDataTask;

    protected override InterfaceITEthernet Device { get; set; }

    protected abstract void KeyPressedAction(int key, uint direction);

    protected override async Task<InterfaceITEthernet.ConnectionStatus> StartDevice()
    {
        InterfaceITEthernet.ConnectionStatus connectionStatus = await Device.InterfaceITEthernetConnectionAsync(cancellationTokenSource.Token);
        receiveInterfaceITEthernetDataTask = Task.Run(async () => await Device.GetInterfaceITEthernetDataAsync(interfacITKeyAction = KeyPressedAction, cancellationTokenSource.Token));
        return connectionStatus;
    }

    protected override void Stop()
    {
        Device.CloseStream();
    }

    protected override void CockpitLoaded(bool isLoaded)
    {
        if (ConnectionStatus == InterfaceITEthernet.ConnectionStatus.Connected)
        {
            //if (isLoaded)
            //{ 
            //    receiveInterfaceITEthernetDataTask = Task.Run(async () => await Device.GetInterfaceITEthernetDataAsync(interfacITKeyAction = KeyPressedAction, cancellationTokenSource.Token));
            //    return;
            //}
            //receiveInterfaceITEthernetDataTask.Dispose();
        }
    }
}