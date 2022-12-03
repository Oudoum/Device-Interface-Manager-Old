using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using static Device_Interface_Manager.MVVM.ViewModel.MainViewModel;

namespace Device_Interface_Manager.MVVM.ViewModel
{
    class SwitchTestViewModel : ObservableObject
    {

        public RelayCommand CallbackmodeEnableCommand { get; set; }

        public RelayCommand PollmodeEnableCommand { get; set; }

        public RelayCommand GetSwitchCommand { get; set; }

        public RelayCommand ClearSwitchListCommand { get; set; }


        public ObservableCollection<string> SwitchLog { get; set; } = new ObservableCollection<string>();

        public InterfaceITAPI_Data.INTERFACEIT_KEY_NOTIFY_PROC KeyNotifiyCallback { get; set; }


        private bool _pollModeEnabled;
        public bool PollmodeEnabled 
        {
            get => this._pollModeEnabled;
            set
            {
                this._pollModeEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _callbackModeEnabled;
        public bool CallbackmodeEnabled
        {
            get => this._callbackModeEnabled;
            set
            {
                this._callbackModeEnabled = value;
                OnPropertyChanged();
            }
        }


        public SwitchTestViewModel() 
        {
            this.CallbackmodeEnableCommand = new RelayCommand(() =>
            {
                if (this.KeyNotifiyCallback == null)
                {
                    this.CallbackmodeEnabled = true;
                    InterfaceITAPI_Data.interfaceIT_Switch_Enable_Callback(GetSelectedDeviceSession(), true, this.KeyNotifiyCallback = new InterfaceITAPI_Data.INTERFACEIT_KEY_NOTIFY_PROC(this.KeyPressedProc));
                }
                else
                {
                    InterfaceITAPI_Data.interfaceIT_Switch_Enable_Callback(GetSelectedDeviceSession(), false, this.KeyNotifiyCallback = null);
                    this.CallbackmodeEnabled = false;
                }
            });

            this.PollmodeEnableCommand = new RelayCommand(() =>
            {
                this.PollmodeEnabled = !this.PollmodeEnabled;
                InterfaceITAPI_Data.interfaceIT_Switch_Enable_Poll(GetSelectedDeviceSession(), this.PollmodeEnabled);
            });

            this.GetSwitchCommand = new RelayCommand(() =>
            {
                while (InterfaceITAPI_Data.interfaceIT_Switch_Get_Item(GetSelectedDeviceSession(), out int Switch, out int Direction) == 0)
                {
                    InterfaceITAPI_Data.interfaceIT_Switch_Get_Item(GetSelectedDeviceSession(), out Switch, out Direction);
                    this.KeyPressedProc(GetSelectedDeviceSession(), Switch, Direction);
                }
            });

            this.ClearSwitchListCommand = new RelayCommand(() => { this.SwitchLog.Clear(); });
        }

        public bool KeyPressedProc(int Session, int Switch, int Direction)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                this.SwitchLog.Add("Session " + Session + ", Switch " + Switch + " is now " + (Direction == InterfaceITAPI_Data.Data.INTERFACEIT_SWITCH_DIR_DOWN ? "on" : "off"));
            });
            return true;
        }
    }
}