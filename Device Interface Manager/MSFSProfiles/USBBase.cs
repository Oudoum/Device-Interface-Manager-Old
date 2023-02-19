using static Device_Interface_Manager.interfaceIT.USB.InterfaceIT_BoardInfo;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceITAPI_Data;

namespace Device_Interface_Manager.MSFSProfiles
{
    public abstract class USBBase : ProfileBase
    {
        protected INTERFACEIT_KEY_NOTIFY_PROC INTERFACEIT_KEY_NOTIFY_PROC { get; set; }

        protected Device Device { get; set; }

        protected abstract bool KeyPressedProc(int session, int key, int direction);

        public virtual void Stop()
        {
            this.CancellationTokenSource?.Cancel();
            this.StopUSBConnection();
        }

        protected void StartUSBConnection(Device device)
        {
            this.Device = device;
            _ = interfaceIT_Dataline_Enable(this.Device.Session, true);
            _ = interfaceIT_Dataline_Set(this.Device.Session, device.DeviceInfo.nDatalineFirst, true);
            _ = interfaceIT_Brightness_Enable(this.Device.Session, true);
            _ = interfaceIT_Analog_Enable(this.Device.Session, true);
            _ = interfaceIT_7Segment_Enable(this.Device.Session, true);
            this.INTERFACEIT_KEY_NOTIFY_PROC = new(KeyPressedProc);
            _ = interfaceIT_Switch_Enable_Callback(this.Device.Session, true, this.INTERFACEIT_KEY_NOTIFY_PROC);
            _ = interfaceIT_LED_Enable(this.Device.Session, true);
        }

        protected void StopUSBConnection()
        {
            _ = interfaceIT_Dataline_Set(this.Device.Session, this.Device.DeviceInfo.nDatalineFirst, false);
            _ = interfaceIT_Dataline_Enable(this.Device.Session, false);
            _ = interfaceIT_Analog_Enable(this.Device.Session, false);
            _ = interfaceIT_Brightness_Enable(this.Device.Session, false);
            _ = interfaceIT_7Segment_Enable(this.Device.Session, false);
            _ = interfaceIT_Switch_Enable_Callback(this.Device.Session, false, this.INTERFACEIT_KEY_NOTIFY_PROC);
            for (int i = this.Device.DeviceInfo.nLEDFirst; i <= this.Device.DeviceInfo.nLEDLast; i++)
            {
                _ = interfaceIT_LED_Set(this.Device.Session, i, false);
            }
            _ = interfaceIT_LED_Enable(this.Device.Session, false);
        }
    }
}