using static Device_Interface_Manager.InterfaceITAPI_Data;

namespace Device_Interface_Manager.MVVM.Model
{
    public class Controller_331A_CDU_MCDU
    {
        public int Session { get; set; }

        public INTERFACEIT_KEY_NOTIFY_PROC KeyNotifyCallback;

        public void Controller_331A_CDU_MCDUStart()
        {
                interfaceIT_Dataline_Enable(Session, true);
                interfaceIT_Dataline_Set(Session, 6, true);
                interfaceIT_Brightness_Enable(Session, true);
                interfaceIT_Analog_Enable(Session, true);
                interfaceIT_Switch_Enable_Callback(Session, true, KeyNotifyCallback);
                interfaceIT_LED_Enable(Session, true);
        }

        public void Controller_331A_CDU_MCDUStop()
        {
            interfaceIT_Dataline_Set(Session, 6, false);
            interfaceIT_Dataline_Enable(Session, false);
            interfaceIT_Analog_Enable(Session, false);
            interfaceIT_Brightness_Enable(Session, false);
            interfaceIT_Switch_Enable_Callback(Session, false, KeyNotifyCallback);
            for (int k = 1; k <= 13; k++)
            {
                interfaceIT_LED_Set(Session, k, 0);
            }
            interfaceIT_LED_Enable(Session, false);
        }
    }
}