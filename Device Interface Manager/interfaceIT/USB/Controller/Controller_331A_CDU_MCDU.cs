using static Device_Interface_Manager.interfaceIT.USB.InterfaceITAPI_Data;

namespace Device_Interface_Manager.interfaceIT.USB.Controller
{
    public class Controller_331A_CDU_MCDU
    {
        public int Session { get; set; }

        public INTERFACEIT_KEY_NOTIFY_PROC KeyNotifyCallback;

        public void Controller_331A_CDU_MCDUStart()
        {
            _ = interfaceIT_Dataline_Enable(Session, true);
            _ = interfaceIT_Dataline_Set(Session, 6, true);
            _ = interfaceIT_Brightness_Enable(Session, true);
            _ = interfaceIT_Analog_Enable(Session, true);
            _ = interfaceIT_Switch_Enable_Callback(Session, true, KeyNotifyCallback);
            _ = interfaceIT_LED_Enable(Session, true);
        }

        public void Controller_331A_CDU_MCDUStop()
        {
            _ = interfaceIT_Dataline_Set(Session, 6, false);
            _ = interfaceIT_Dataline_Enable(Session, false);
            _ = interfaceIT_Analog_Enable(Session, false);
            _ = interfaceIT_Brightness_Enable(Session, false);
            _ = interfaceIT_Switch_Enable_Callback(Session, false, KeyNotifyCallback);
            for (int k = 1; k <= 13; k++)
            {
                _ = interfaceIT_LED_Set(Session, k, false);
            }
            _ = interfaceIT_LED_Enable(Session, false);
        }
    }
}