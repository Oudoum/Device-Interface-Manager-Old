using System.Threading.Tasks;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceIT_BoardInfo;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceITAPI_Data;

namespace Device_Interface_Manager.MSFSProfiles;

public abstract class USB : ProfileBase
{
    protected Device device;

    protected INTERFACEIT_KEY_NOTIFY_PROC_UINT iNTERFACEIT_KEY_NOTIFY_PROC;

    protected abstract void KeyPressedProc(uint session, int key, uint direction);

    public async Task Start(Device device)
    {
        if (device is not null)
        {
            this.device = device;
            await StartSimConnect();
            receiveSimConnectDataTask = Task.Run(() => ReceiveSimConnectData(cancellationTokenSource.Token));
            StartUSBConnection();
        }
    }

    public virtual void Stop()
    {
        cancellationTokenSource.Cancel();
        StopUSBConnection();
        simConnectClient.SimConnect_Close();
    }

    protected void StartUSBConnection()
    {

        interfaceIT_Dataline_Enable(device.Session, true);
        interfaceIT_Dataline_Set(device.Session, device.DeviceInfo.nDatalineFirst, true);
        interfaceIT_Brightness_Enable(device.Session, true);
        interfaceIT_Analog_Enable(device.Session, true);
        interfaceIT_7Segment_Enable(device.Session, true);
        interfaceIT_Switch_Enable_Callback(device.Session, true, iNTERFACEIT_KEY_NOTIFY_PROC = new(KeyPressedProc));
        interfaceIT_LED_Enable(device.Session, true);
    }

    protected void StopUSBConnection()
    {
        if (device is not null)
        {
            interfaceIT_Dataline_Set(device.Session, device.DeviceInfo.nDatalineFirst, false);
            interfaceIT_Dataline_Enable(device.Session, false);
            interfaceIT_Analog_Enable(device.Session, false);
            interfaceIT_Brightness_Enable(device.Session, false);
            interfaceIT_7Segment_Enable(device.Session, false);
            interfaceIT_Switch_Enable_Callback(device.Session, false, iNTERFACEIT_KEY_NOTIFY_PROC);
            for (int i = device.DeviceInfo.nLEDFirst; i <= device.DeviceInfo.nLEDLast; i++)
            {
                interfaceIT_LED_Set(device.Session, i, false);
            }
            interfaceIT_LED_Enable(device.Session, false);
        }
    }
}