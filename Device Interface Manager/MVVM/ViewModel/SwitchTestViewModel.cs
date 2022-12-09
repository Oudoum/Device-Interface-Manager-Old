using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Device_Interface_Manager.interfaceIT.USB;
using System.Collections.ObjectModel;
using static Device_Interface_Manager.MVVM.ViewModel.MainViewModel;

namespace Device_Interface_Manager.MVVM.ViewModel
{
    [INotifyPropertyChanged]
    partial class SwitchTestViewModel
    {
        public ObservableCollection<string> SwitchLog { get; set; } = new ObservableCollection<string>();

        public InterfaceITAPI_Data.INTERFACEIT_KEY_NOTIFY_PROC KeyNotifiyCallback { get; set; }

        [ObservableProperty]
        private bool _pollModeEnabled;

        [ObservableProperty]
        private bool _callbackModeEnabled;

        [RelayCommand]
        private void CallbackMode()
        {
            if (this.KeyNotifiyCallback == null)
            {
                this.CallbackModeEnabled = true;
                _ = InterfaceITAPI_Data.interfaceIT_Switch_Enable_Callback(GetSelectedDeviceSession(), true, this.KeyNotifiyCallback = new InterfaceITAPI_Data.INTERFACEIT_KEY_NOTIFY_PROC(this.KeyPressedProc));
            }
            else
            {
                _ = InterfaceITAPI_Data.interfaceIT_Switch_Enable_Callback(GetSelectedDeviceSession(), false, this.KeyNotifiyCallback = null);
                this.CallbackModeEnabled = false;
            }
        }

        [RelayCommand]
        private void PollMode()
        {
            this.PollModeEnabled = !this.PollModeEnabled;
            _ = InterfaceITAPI_Data.interfaceIT_Switch_Enable_Poll(GetSelectedDeviceSession(), this.PollModeEnabled);
        }

        [RelayCommand]
        private void GetSwitch()
        {
            while (InterfaceITAPI_Data.interfaceIT_Switch_Get_Item(GetSelectedDeviceSession(), out int _, out int _) == 0)
            {
                _ = InterfaceITAPI_Data.interfaceIT_Switch_Get_Item(GetSelectedDeviceSession(), out int Switch, out int Direction);
                this.KeyPressedProc(GetSelectedDeviceSession(), Switch, Direction);
            }
        }

        [RelayCommand]
        private void ClearSwitchList()
        {
            this.SwitchLog.Clear();
        }

        private bool KeyPressedProc(int Session, int Switch, int Direction)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                this.SwitchLog.Add("Session " + Session + ", Switch " + Switch + " is now " + (Direction == InterfaceITAPI_Data.Data.INTERFACEIT_SWITCH_DIR_DOWN ? "on" : "off"));
            });
            return true;
        }
    }
}