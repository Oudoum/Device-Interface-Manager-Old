using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Device_Interface_Manager.interfaceIT.USB;
using System.Collections.ObjectModel;
using static Device_Interface_Manager.MVVM.ViewModel.MainViewModel;

namespace Device_Interface_Manager.MVVM.ViewModel
{
    public partial class SwitchTestViewModel : ObservableObject
    {
        public ObservableCollection<string> SwitchLog { get; set; } = new();

        private InterfaceITAPI_Data.INTERFACEIT_KEY_NOTIFY_PROC keyNotifiyCallback;

        [ObservableProperty]
        private bool _pollModeEnabled;

        [ObservableProperty]
        private bool _callbackModeEnabled;

        [RelayCommand]
        private void CallbackMode()
        {
            this.CallbackModeEnabled = !this.CallbackModeEnabled;
            if (this.keyNotifiyCallback is null)
            {
                _ = InterfaceITAPI_Data.interfaceIT_Switch_Enable_Callback(GetSelectedDeviceSession(), true, this.keyNotifiyCallback = new InterfaceITAPI_Data.INTERFACEIT_KEY_NOTIFY_PROC(this.KeyPressedProc));
                return;
            }
            _ = InterfaceITAPI_Data.interfaceIT_Switch_Enable_Callback(GetSelectedDeviceSession(), false, this.keyNotifiyCallback = null);
        }

        [RelayCommand]
        private void PollMode()
        {
            _ = InterfaceITAPI_Data.interfaceIT_Switch_Enable_Poll(GetSelectedDeviceSession(), this.PollModeEnabled = !this.PollModeEnabled);
        }

        [RelayCommand]
        private void GetSwitch()
        {
            while (InterfaceITAPI_Data.interfaceIT_Switch_Get_Item(GetSelectedDeviceSession(), out int _, out int _) == 0)
            {
                _ = InterfaceITAPI_Data.interfaceIT_Switch_Get_Item(GetSelectedDeviceSession(), out int key, out int direction);
                this.KeyPressedProc(GetSelectedDeviceSession(), key, direction);
            }
        }

        [RelayCommand]
        private void ClearSwitchList()
        {
            this.SwitchLog.Clear();
        }

        private bool KeyPressedProc(int session, int key, int direction)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                this.SwitchLog.Add("Session " + session + ", Switch " + key + " is now " + (direction == InterfaceITAPI_Data.Data.INTERFACEIT_SWITCH_DIR_DOWN ? "on" : "off"));
            });
            return true;
        }
    }
}