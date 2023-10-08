using System.Threading.Tasks;
using static Device_Interface_Manager.Devices.interfaceIT.USB.InterfaceIT_BoardInfo;
using static Device_Interface_Manager.Devices.interfaceIT.USB.InterfaceITAPI_Data;

namespace Device_Interface_Manager.SimConnectProfiles;

public abstract class USB : ProfileBase<Device>
{
    protected KeyNotificationCallbackUint interfaceItKeyNotifyProc;

    protected override Device Device { get; set; }

    protected abstract void KeyPressedProc(uint session, int key, uint direction);

    protected override Task<Devices.interfaceIT.ENET.InterfaceITEthernet.ConnectionStatus> StartDevice()
    {
        InterfaceITEnable(Device);
        interfaceItKeyNotifyProc = KeyPressedProc;
        return Task.FromResult(Devices.interfaceIT.ENET.InterfaceITEthernet.ConnectionStatus.Connected);
    }

    protected override void StopDevice()
    {
        interfaceIT_Switch_Enable_Callback(Device.Session, false, interfaceItKeyNotifyProc);
        InterfaceITDisable(Device);
    }

    protected override void CockpitLoaded(bool isLoaded)
    {
        interfaceIT_Switch_Enable_Callback(Device.Session, isLoaded, interfaceItKeyNotifyProc);
    }
}