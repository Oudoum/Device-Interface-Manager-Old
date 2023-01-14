using Device_Interface_Manager.MVVM.View;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using AutoUpdaterDotNET;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceIT_BoardInfo.BoardInformationStructure;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceITAPI_Data;
using Device_Interface_Manager.interfaceIT.USB;

namespace Device_Interface_Manager.MVVM.ViewModel
{
    [INotifyPropertyChanged]
    partial class MainViewModel
    {
        public static int session;
        private const string updateLink = "https://raw.githubusercontent.com/Oudoum/Device-Interface-Manager-Download/main/Updates/AutoUpdaterDIM.xml";
        private const string version = "1.0.0";


        public RelayCommand ProfileCreatorCommand { get; set; }


        public static HomeUSBViewModel HomeUSBVM { get; set; }

        public static HomeENETViewModel HomeENETVM { get; set; }

        public static ObservableCollection<LEDTestViewModel> LEDTestViewModels { get; set; } = new();

        public static ObservableCollection<SwitchTestViewModel> SwitchTestViewModels { get; set; } = new();

        public static ObservableCollection<OtherTestsViewModel> OtherTestViewModels { get; set; } = new();

        public static ObservableCollection<InterfaceIT_BoardInfo.Device> DeviceList { get; set; } = new();

        public static BoardinfoENETViewModel BoardinfoENETVM { get; set; }

        public static TestENETViewModel TestENETVM { get; set; }

        public static SettingsViewModel SettingsVM { get; set; }


        public bool RadioButtonHomeIsChecked { get; set; } = true;
        public bool RadioButtonLEDTestIsChecked { get; set; }
        public bool RadioButtonSwitchTesteIsChecked { get; set; }
        public bool RadioButtonOtherTestIsChecked { get; set; }
        public bool RadioButtonHomeENETIsChecked { get; set; } = true;
        public bool RadioButtonBoardinfoENETIsChecked { get; set; }
        public bool RadioButtonTestENETIsChecked { get; set; }

        public static int controllerCount;

        public static string DIMVersion { get; } = "DIM Version " + version;

        public string TotalControllers { get; set; }

        [ObservableProperty]
        private bool _enet = Properties.Settings.Default.ENET;
        partial void OnEnetChanged(bool value)
        {
            Properties.Settings.Default.ENET = value;
            Properties.Settings.Default.Save();
        }

        [ObservableProperty]
        private object _currentView;



        public static int selectedController;
        private int _selectedController;
        public int SelectedController
        {
            get => _selectedController;
            set
            {
                this._selectedController = selectedController = value;
                GetCurrentView();
                OnPropertyChanged();
                HomeUSBVM?.GetBoardInfo();
            }
        }

        public MainViewModel()
        {
            AutoUpdater.InstalledVersion = new Version(version);

            this.GetInterfaceITDevices();

            HomeUSBVM = new HomeUSBViewModel();
            HomeENETVM = new HomeENETViewModel();
            BoardinfoENETVM = new BoardinfoENETViewModel();
            TestENETVM = new TestENETViewModel();
            SettingsVM = new SettingsViewModel();

            this.USB_ENET();

            //Opens ProfileCreator
            //MOVE THIS TO VIEW!
            this.ProfileCreatorCommand = new RelayCommand(() =>
            {
                if (System.Windows.Application.Current.Windows.OfType<ProfileCreator>().Any() == false)
                {
                    ProfileCreator profileCreator = new();
                    profileCreator.Show();
                }
            });

            this.SelectedController = 0;

            this.DIMUpdater();
        }

        [RelayCommand]
        private void DIMUpdater()
        {
            AutoUpdater.Start(updateLink);
        }

        [RelayCommand]
        private void HomeUSBView()
        {
            this.CurrentView = HomeUSBVM;
        }

        [RelayCommand]
        private void LEDTestView()
        {
            this.CurrentView = LEDTestViewModels.ElementAtOrDefault(SelectedController);
        }

        [RelayCommand]
        private void SwitchTestView()
        {
            this.CurrentView = SwitchTestViewModels.ElementAtOrDefault(SelectedController);
        }

        [RelayCommand]
        private void TestView()
        {
            this.CurrentView = OtherTestViewModels.ElementAtOrDefault(SelectedController);
        }

        [RelayCommand]
        private void HomeENETViewToggle()
        {
            if (this.Enet)
            {
                this.RadioButtonHomeENETIsChecked = true;
                OnPropertyChanged(nameof(RadioButtonHomeENETIsChecked));
                this.CurrentView = HomeENETVM;
                this.Enet = !this.Enet;
            }
            else if (!this.Enet)
            {
                this.RadioButtonHomeIsChecked = true;
                OnPropertyChanged(nameof(RadioButtonHomeIsChecked));
                this.CurrentView = HomeUSBVM;
                this.Enet = !this.Enet;
            }
            OnPropertyChanged(nameof(Enet));
        }

        [RelayCommand]
        private void HomeENETView()
        {
            this.CurrentView = HomeENETVM;
        }

        [RelayCommand]
        private void BoardinfoENETView()
        {
            this.CurrentView = BoardinfoENETVM;
        }

        [RelayCommand]
        private void TestENETView()
        {
            this.CurrentView = TestENETVM;
        }

        [RelayCommand]
        private void SettingsView()
        {
            this.CurrentView = SettingsVM;
            this.RadioButtonBoardinfoENETIsChecked = false;
            this.RadioButtonHomeENETIsChecked = false;
            this.RadioButtonHomeIsChecked = false;
            this.RadioButtonLEDTestIsChecked = false;
            this.RadioButtonOtherTestIsChecked = false;
            this.RadioButtonSwitchTesteIsChecked = false;
            this.RadioButtonTestENETIsChecked = false;
            OnPropertyChanged(nameof(this.RadioButtonBoardinfoENETIsChecked));
            OnPropertyChanged(nameof(this.RadioButtonHomeENETIsChecked));
            OnPropertyChanged(nameof(this.RadioButtonHomeIsChecked));
            OnPropertyChanged(nameof(this.RadioButtonLEDTestIsChecked));
            OnPropertyChanged(nameof(this.RadioButtonOtherTestIsChecked));
            OnPropertyChanged(nameof(this.RadioButtonSwitchTesteIsChecked));
            OnPropertyChanged(nameof(this.RadioButtonTestENETIsChecked));
        }

        [RelayCommand]
        private void DeviceCountRefresh()
        {
            CloseInterfaceITDevices();
            GetInterfaceITDevices();
            while (this._selectedController > DeviceList.Count - 1)
            {
                this._selectedController--;
            }
            if (this._selectedController != -1)
            {
                this.SelectedController = this._selectedController;
                return;
            }
            this.SelectedController = 0;
        }

        private void USB_ENET()
        {
            if (this.Enet)
            {
                this.CurrentView = HomeUSBVM;
            }
            else if (!this.Enet)
            {
                this.CurrentView = HomeENETVM;
            }
        }

        private void GetInterfaceITDevices()
        {
            _ = interfaceIT_OpenControllers();
            GetTotalControllers();
            GetDeviceList();
        }

        private void GetTotalControllers()
        {
            controllerCount = -1;
            _ = interfaceIT_GetTotalControllers(ref controllerCount);
            if (controllerCount > 0)
            {
                TotalControllers = "Total controllers detected: " + controllerCount + Environment.NewLine + "Click to refresh!";
            }
            else
            {
                TotalControllers = "No controllers detected!" + Environment.NewLine + "Click to refresh!";
            }
            OnPropertyChanged(nameof(TotalControllers));
        }

        private static void GetDeviceList()
        {
            int intSize = 0;
            _ = interfaceIT_GetDeviceList(null, ref intSize, null);
            byte[] byteDeviceList = new byte[intSize];
            _ = interfaceIT_GetDeviceList(byteDeviceList, ref intSize, null);
            DeviceList.Clear();
            LEDTestViewModels.Clear();
            SwitchTestViewModels.Clear();
            OtherTestViewModels.Clear();
            int i = 0;
            BOARDCAPS bOARDCAPS = new();
            foreach (var device in Encoding.Default.GetString(byteDeviceList).TrimEnd('\0').Split('\0'))
            {
                if (!string.IsNullOrEmpty(device))
                {
                    _ = interfaceIT_Bind(device, ref session);
                    _ = interfaceIT_GetBoardInfo(session, ref bOARDCAPS);
                    DeviceList.Add(new InterfaceIT_BoardInfo.Device { Id = i++, SerialNumber = device, Session = session, DeviceInfo = bOARDCAPS});
                    LEDTestViewModels.Add(new LEDTestViewModel());
                    SwitchTestViewModels.Add(new SwitchTestViewModel());
                    OtherTestViewModels.Add(new OtherTestsViewModel());
                }
            }
        }

        private void GetCurrentView()
        {
            if (DeviceList.Count != 0)
            {
                if (RadioButtonLEDTestIsChecked)
                {
                    this.CurrentView = LEDTestViewModels.ElementAtOrDefault(SelectedController);
                }
                else if (RadioButtonSwitchTesteIsChecked)
                {
                    this.CurrentView = SwitchTestViewModels.ElementAtOrDefault(SelectedController);
                }
                else if (RadioButtonOtherTestIsChecked)
                {
                    this.CurrentView = OtherTestViewModels.ElementAtOrDefault(SelectedController);
                }
            }
            else if (!RadioButtonHomeIsChecked)
            {
                CurrentView = null;
            }
        }

        public static void CloseInterfaceITDevices()
        {
            foreach (var device in DeviceList)
            {
                if ((device.DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_OUTPUT_LED) != 0)
                {
                    for (int i = device.DeviceInfo.nSwitchFirst; i <= device.DeviceInfo.nLEDLast; i++)
                    {
                        _ = interfaceIT_LED_Set(device.Session, i, false);
                    }
                    _ = interfaceIT_LED_Enable(device.Session, false);
                }

                if ((device.DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_INPUT_SWITCHES) != 0)
                {
                    _ = interfaceIT_Switch_Enable_Poll(device.Session, false);
                }


                if ((device.DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_OUTPUT_7SEGMENT) != 0)
                {
                    for (int i = device.DeviceInfo.n7SegmentFirst; i <= device.DeviceInfo.n7SegmentLast; i++)
                    {
                        _ = interfaceIT_7Segment_Display(device.Session, null , i);
                    }
                    _ = interfaceIT_7Segment_Enable(device.Session, false);
                }


                if ((device.DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_OUTPUT_DATALINE) != 0)
                {
                    for (int i = device.DeviceInfo.nDatalineFirst; i <= device.DeviceInfo.nDatalineLast; i++)
                    {
                        _ = interfaceIT_Dataline_Set(device.Session, i , false);
                    }
                    _ = interfaceIT_Dataline_Enable(device.Session, false);
                }


                if ((device.DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_OUTPUT_SERVO) != 0)
                {
                    //Not available
                }


                if ((device.DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_SPECIAL_BRIGHTNESS) != 0)
                {
                    _ = interfaceIT_Brightness_Set(device.Session, 0);
                    _ = interfaceIT_Brightness_Enable(device.Session, false);
                }


                if ((device.DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_SPECIAL_ANALOG_INPUT) != 0)
                {
                    _ = interfaceIT_Analog_Enable(device.Session, false);
                }


                if ((device.DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_SPECIAL_ANALOG16_INPUT) != 0)
                {
                    _ = interfaceIT_Analog_Enable(device.Session, false);
                }
                _ = interfaceIT_UnBind(device.Session);
            }
            _ = interfaceIT_CloseControllers();
        }

        public static int GetSeletedController()
        {
            return selectedController;
        }

        public static int GetSelectedDeviceSession()
        {
            return DeviceList.Count != 0 ? DeviceList[selectedController].Session : 0;
        }
    }
}