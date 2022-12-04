using Device_Interface_Manager.MVVM.View;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoUpdaterDotNET;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceIT_BoardInfo.BoardInformationStructure;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceITAPI_Data;
using Device_Interface_Manager.interfaceIT.USB;

namespace Device_Interface_Manager.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public static int session;
        private const string updateLink = "https://raw.githubusercontent.com/Oudoum/Device-Interface-Manager-Download/main/Updates/AutoUpdaterDIM.xml";
        private const string version = "0.0.9.1";

        public RelayCommand HomeViewCommand { get; set; }

        public RelayCommand LEDTestViewCommand { get; set; }

        public RelayCommand SwitchTestViewCommand { get; set; }

        public RelayCommand TestViewCommand { get; set; }

        public RelayCommand HomeENETViewToggleCommand { get; set; }

        public RelayCommand HomeENETViewCommand { get; set; }

        public RelayCommand BoardinfoENETViewCommand { get; set; }

        public RelayCommand TestENETViewCommand { get; set; }

        public RelayCommand ProfileCreatorCommand { get; set; }

        public RelayCommand WindowMaximizedCommand { get; set; }

        public RelayCommand DeviceCountRefreshCommand { get; set; }

        public RelayCommand DIMUpdaterCommand { get; set; }


        public static HomeViewModel HomeVM { get; set; }

        public static HomeENETViewModel HomeENETVM { get; set; }

        public static ObservableCollection<LEDTestViewModel> LEDTestViewModels { get; set; } = new ObservableCollection<LEDTestViewModel>();

        public static ObservableCollection<SwitchTestViewModel> SwitchTestViewModels { get; set; } = new ObservableCollection<SwitchTestViewModel>();

        public static ObservableCollection<OtherTestsViewModel> OtherTestViewModels { get; set; } = new ObservableCollection<OtherTestsViewModel>();

        public static ObservableCollection<InterfaceIT_BoardInfo.Device> DeviceList { get; set; } = new ObservableCollection<InterfaceIT_BoardInfo.Device>();

        public static BoardinfoENETViewModel BoardinfoENETVM { get; set; }

        public static TestENETViewModel TestENETVM { get; set; }


        public bool RadioButtonHomeIsChecked { get; set; } = true;
        public bool RadioButtonLEDTestIsChecked { get; set; }
        public bool RadioButtonSwitchTesteIsChecked { get; set; }
        public bool RadioButtonOtherTestIsChecked { get; set; }
        public bool RadioButtonHomeENETIsChecked { get; set; } = true;
        public bool RadioButtonBoardinfoENETIsChecked { get; set; }
        public bool RadioButtonTestENETIsChecked { get; set; }

        public static string DIMVersion { get; } = "DIM Version " + version;

        public static string InterfaceITAPIVersion { get; set; }

        private bool _minimizedHide = Properties.Settings.Default.MinimizedHide;
        public bool MinimizedHide
        {
            get => _minimizedHide;
            set
            {
                Properties.Settings.Default.MinimizedHide = this._minimizedHide = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged();
            }
        }

        private bool _autoHide = Properties.Settings.Default.AutoHide;
        public bool AutoHide
        {
            get => this._autoHide;
            set
            {
                Properties.Settings.Default.AutoHide = this._autoHide = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged();
            }
        }

        private bool _enet = Properties.Settings.Default.ENET;
        public bool Enet
        {
            get => this._enet;
            set
            {
                this._enet = value;
                Properties.Settings.Default.ENET = this._enet = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged();
            }
        }

        private object _currentView;
        public object CurrentView
        {
            get => this._currentView;
            set
            {
                this._currentView = value;
                OnPropertyChanged();
            }
        }

        public string TotalControllers { get; set; }

        public static int controllerCount;

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
                HomeVM?.GetBoardInfo();
            }
        }


        public MainViewModel()
        {
            AutoUpdater.InstalledVersion = new Version(version);

            GetInterfaceITAPIVersion();
            GetInterfaceITDevices();

            HomeVM = new HomeViewModel();
            HomeENETVM = new HomeENETViewModel();
            BoardinfoENETVM = new BoardinfoENETViewModel();
            TestENETVM = new TestENETViewModel();

            USB_ENET();

            this.DIMUpdaterCommand = new RelayCommand(() => AutoUpdater.Start(updateLink));

            this.HomeViewCommand = new RelayCommand(() => this.CurrentView = HomeVM);

            this.LEDTestViewCommand = new RelayCommand(() => this.CurrentView = LEDTestViewModels.ElementAtOrDefault(SelectedController));

            this.SwitchTestViewCommand = new RelayCommand(() => this.CurrentView = SwitchTestViewModels.ElementAtOrDefault(SelectedController));

            this.TestViewCommand = new RelayCommand(() => this.CurrentView = OtherTestViewModels.ElementAtOrDefault(SelectedController));

            this.HomeENETViewToggleCommand = new RelayCommand(() =>
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
                    this.CurrentView = HomeVM;
                    this.Enet = !this.Enet;
                }
                OnPropertyChanged(nameof(Enet));
            });

            this.HomeENETViewCommand = new RelayCommand(() => this.CurrentView = HomeENETVM);

            this.BoardinfoENETViewCommand = new RelayCommand(() => this.CurrentView = BoardinfoENETVM);

            this.TestENETViewCommand = new RelayCommand(() => this.CurrentView = TestENETVM);

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

            this.DeviceCountRefreshCommand = new RelayCommand(() =>
            {
                CloseInterfaceITDevices();
                GetInterfaceITDevices();
                while (this._selectedController > DeviceList.Count - 1)
                {
                    this._selectedController--;
                }
                if (this._selectedController == -1)
                {
                    this.SelectedController = 0;
                }
                else 
                {
                    this.SelectedController = this._selectedController;
                }
            });
            this.SelectedController = 0;

            AutoUpdater.Start(updateLink);
        }

        private void USB_ENET()
        {
            if (this.Enet)
            {
                this.CurrentView = HomeVM;
            }
            else if (!this.Enet)
            {
                this.CurrentView = HomeENETVM;
            }
        }

        private void GetInterfaceITAPIVersion()
        {
            int intSize = 0;
            interfaceIT_GetAPIVersion(null, ref intSize);
            StringBuilder aPIVersion = new(intSize);
            interfaceIT_GetAPIVersion(aPIVersion, ref intSize);
            InterfaceITAPIVersion = "interfaceIT API version " + aPIVersion;
        }

        private void GetInterfaceITDevices()
        {
            interfaceIT_OpenControllers();
            GetTotalControllers();
            GetDeviceList();
        }

        private void GetTotalControllers()
        {
            controllerCount = -1;
            interfaceIT_GetTotalControllers(ref controllerCount);
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

        private void GetDeviceList()
        {
            int intSize = 0;
            interfaceIT_GetDeviceList(null, ref intSize, null);
            byte[] byteDeviceList = new byte[intSize];
            interfaceIT_GetDeviceList(byteDeviceList, ref intSize, null);
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
                    interfaceIT_Bind(device, ref session);
                    interfaceIT_GetBoardInfo(session, ref bOARDCAPS);
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
                        interfaceIT_LED_Set(device.Session, i, false);
                    }
                    interfaceIT_LED_Enable(device.Session, false);
                }

                if ((device.DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_INPUT_SWITCHES) != 0)
                {
                    interfaceIT_Switch_Enable_Poll(device.Session, false);
                }


                if ((device.DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_OUTPUT_7SEGMENT) != 0)
                {
                    for (int i = device.DeviceInfo.n7SegmentFirst; i <= device.DeviceInfo.n7SegmentLast; i++)
                    {
                        interfaceIT_7Segment_Display(device.Session, null , i);
                    }
                    interfaceIT_7Segment_Enable(device.Session, false);
                }


                if ((device.DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_OUTPUT_DATALINE) != 0)
                {
                    for (int i = device.DeviceInfo.nDatalineFirst; i <= device.DeviceInfo.nDatalineLast; i++)
                    {
                        interfaceIT_Dataline_Set(device.Session, i , false);
                    }
                    interfaceIT_Dataline_Enable(device.Session, false);
                }


                if ((device.DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_OUTPUT_SERVO) != 0)
                {
                    //Not available
                }


                if ((device.DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_SPECIAL_BRIGHTNESS) != 0)
                {
                    interfaceIT_Brightness_Set(device.Session, 0);
                    interfaceIT_Brightness_Enable(device.Session, false);
                }


                if ((device.DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_SPECIAL_ANALOG_INPUT) != 0)
                {
                    interfaceIT_Analog_Enable(device.Session, false);
                }


                if ((device.DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_SPECIAL_ANALOG16_INPUT) != 0)
                {
                    interfaceIT_Analog_Enable(device.Session, false);
                }
                interfaceIT_UnBind(device.Session);
            }
            interfaceIT_CloseControllers();
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