using System;
using System.Threading.Tasks;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceIT_BoardInfo;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceITAPI_Data;

namespace Device_Interface_Manager.MSFSProfiles;

public abstract class USB : IITProfileBase
{
    protected Device Device;

    protected INTERFACEIT_KEY_NOTIFY_PROC_UINT interfaceItKeyNotifyProc;

    protected abstract void KeyPressedProc(uint session, int key, uint direction);

    public async Task StartAsync(Device device)
    {
        if (device is not null)
        {
            Device = device;
            InterfaceITEnable(Device);
            await StartSimConnectAsync();
            interfaceItKeyNotifyProc = KeyPressedProc;
            if ((oldCameraState = simConnectClient.GetSimVar("CAMERA STATE")) == 2)
            {
                interfaceIT_Switch_Enable_Callback(Device.Session, true, interfaceItKeyNotifyProc);
            }
        }
    }

    private double? oldCameraState;
    protected override async void SimConnectClient_OnSimVarChanged(object sender, SimConnectClient.SimVar simVar)
    {
        if (simVar.Name == "CAMERA STATE")
        {
            if (simVar.Data == 2 && oldCameraState != 2)
            {
                if (oldCameraState == 11 || oldCameraState == 15)
                {
                    await Task.Delay(TimeSpan.FromSeconds(15));
                }
                interfaceIT_Switch_Enable_Callback(Device.Session, true, interfaceItKeyNotifyProc);
            }
            if (simVar.Data == 15 && oldCameraState != 15)
            {
                interfaceIT_Switch_Enable_Callback(Device.Session, false, interfaceItKeyNotifyProc);
            }
            oldCameraState = simVar.Data;
        }
    }

    public override void Stop()
    {
        cancellationTokenSource.Cancel();
        if (Device is not null)
        {
            interfaceIT_Switch_Enable_Callback(Device.Session, false, interfaceItKeyNotifyProc);
            InterfaceITDisable(Device);
        }
        simConnectClient.SimConnect_Close();
    }
}