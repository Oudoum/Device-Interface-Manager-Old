using System;
using System.Threading.Tasks;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceIT_BoardInfo;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceITAPI_Data;

namespace Device_Interface_Manager.MSFSProfiles;

public abstract class USB : ProfileBase
{
    protected Device Device;

    protected INTERFACEIT_KEY_NOTIFY_PROC_UINT iNTERFACEIT_KEY_NOTIFY_PROC;

    protected abstract void KeyPressedProc(uint session, int key, uint direction);

    public async Task StartAsync(Device device)
    {
        if (device is not null)
        {
            Device = device;
            InterfaceITEnable(Device);
            await StartSimConnectAsync();
            if (simConnectClient.simConnect is not null)
            {
                simConnectClient.OnCockpitLoaded += SimConnectClient_OnCockpitLoaded;
                receiveSimConnectDataTask = Task.Run(() => ReceiveSimConnectData(cancellationTokenSource.Token));
            }
        }
    }

    private async void SimConnectClient_OnCockpitLoaded(object sender, EventArgs e)
    {
        if (iNTERFACEIT_KEY_NOTIFY_PROC is null)
        {
            await Task.Delay(TimeSpan.FromSeconds(10));
            interfaceIT_Switch_Enable_Callback(Device.Session, true, iNTERFACEIT_KEY_NOTIFY_PROC = new(KeyPressedProc));
        }
    }

    public override void Stop()
    {
        cancellationTokenSource.Cancel();
        if (Device is not null)
        {
            interfaceIT_Switch_Enable_Callback(Device.Session, false, iNTERFACEIT_KEY_NOTIFY_PROC);
            InterfaceITDisable(Device);
        }
        simConnectClient.SimConnect_Close();
    }
}