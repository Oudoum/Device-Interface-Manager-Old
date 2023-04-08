using System;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using AutoUpdaterDotNET;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using Device_Interface_Manager.interfaceIT.USB;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceITAPI_Data;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceIT_BoardInfo.BoardInformationStructure;
using Device_Interface_Manager.MSFSProfiles;
using System.Threading.Tasks;

namespace Device_Interface_Manager.MVVM.ViewModel;

public partial class MainViewModel : ObservableObject, IRecipient<SimConnectStausMessage>
{
    public int session;
    private const string updateLink = "https://raw.githubusercontent.com/Oudoum/Device-Interface-Manager-Download/main/Updates/AutoUpdaterDIM.xml";
    private const string version = "1.1.5";

    public HomeUSBViewModel HomeUSBVM { get; set; }

    public HomeENETViewModel HomeENETVM { get; set; }

    public ObservableCollection<LEDTestViewModel> LEDTestViewModels { get; set; } = new();

    public ObservableCollection<SwitchTestViewModel> SwitchTestViewModels { get; set; } = new();

    public ObservableCollection<OtherTestsViewModel> OtherTestViewModels { get; set; } = new();

    public ObservableCollection<InterfaceIT_BoardInfo.Device> DeviceList { get; set; } = new();

    public BoardinfoENETViewModel BoardinfoENETVM { get; set; }

    public TestENETViewModel TestENETVM { get; set; }

    public SettingsViewModel SettingsVM { get; set; }

    public bool IsSimConnectOpen { get; set; }

    public bool RadioButtonHomeIsChecked { get; set; } = true;
    public bool RadioButtonLEDTestIsChecked { get; set; }
    public bool RadioButtonSwitchTesteIsChecked { get; set; }
    public bool RadioButtonOtherTestIsChecked { get; set; }
    public bool RadioButtonHomeENETIsChecked { get; set; } = true;
    public bool RadioButtonBoardinfoENETIsChecked { get; set; }
    public bool RadioButtonTestENETIsChecked { get; set; }

    public bool SettingsIsChecked { get; set; }

    public int controllerCount;

    public string DIMVersion { get; } = "DIM Version " + version;

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

    partial void OnCurrentViewChanged(object value)
    {
        this.SettingsIsChecked = (value?.GetType() == typeof(SettingsViewModel));
        this.OnPropertyChanged(nameof(this.SettingsIsChecked));
    }

    private int _selectedController;
    public int SelectedController
    {
        get => _selectedController;
        set
        {
            _selectedController = value;
            GetCurrentView();
            OnPropertyChanged();
            if (DeviceList.Count > 0)
            {
                HomeUSBVM.GetBoardInfo(DeviceList[value].DeviceInfo);
                HomeUSBVM.Devices = DeviceList.ToList();
            }
        }
    }

    public MainViewModel()
    {
        AutoUpdater.InstalledVersion = new Version(version);

        HomeUSBVM = new HomeUSBViewModel();
        HomeENETVM = new HomeENETViewModel();
        BoardinfoENETVM = new BoardinfoENETViewModel();
        TestENETVM = new TestENETViewModel();
        SettingsVM = new SettingsViewModel();

        GetInterfaceITDevices();

        USB_ENET();

        SelectedController = 0;

        AutoUpdater.Start(updateLink);
        StrongReferenceMessenger.Default.Register(this);
    }

    [RelayCommand]
    private void OnWindowClosing()
    {
        CloseInterfaceITDevices();
        HomeENETVM.SaveENETData();
        HomeUSBVM.SaveUSBData();
        Environment.Exit(Environment.ExitCode);
    }

    [RelayCommand]
    private void HomeUSBView()
    {
        CurrentView = HomeUSBVM;
    }

    [RelayCommand]
    private void LEDTestView()
    {
        CurrentView = LEDTestViewModels.ElementAtOrDefault(SelectedController);
    }

    [RelayCommand]
    private void SwitchTestView()
    {
        CurrentView = SwitchTestViewModels.ElementAtOrDefault(SelectedController);
    }

    [RelayCommand]
    private void TestView()
    {
        CurrentView = OtherTestViewModels.ElementAtOrDefault(SelectedController);
    }

    [RelayCommand]
    private void HomeENETViewToggle()
    {
        if (Enet)
        {
            RadioButtonHomeENETIsChecked = true;
            OnPropertyChanged(nameof(RadioButtonHomeENETIsChecked));
            CurrentView = HomeENETVM;
            Enet = !Enet;
        }
        else if (!Enet)
        {
            RadioButtonHomeIsChecked = true;
            OnPropertyChanged(nameof(RadioButtonHomeIsChecked));
            CurrentView = HomeUSBVM;
            Enet = !Enet;
        }
        OnPropertyChanged(nameof(Enet));
    }

    [RelayCommand]
    private void HomeENETView()
    {
        CurrentView = HomeENETVM;
    }

    [RelayCommand]
    private void BoardinfoENETView()
    {
        CurrentView = BoardinfoENETVM;
    }

    [RelayCommand]
    private void TestENETView()
    {
        CurrentView = TestENETVM;
    }

    [RelayCommand]
    private void SettingsView()
    {
        CurrentView = SettingsVM;
        RadioButtonBoardinfoENETIsChecked = false;
        RadioButtonHomeENETIsChecked = false;
        RadioButtonHomeIsChecked = false;
        RadioButtonLEDTestIsChecked = false;
        RadioButtonOtherTestIsChecked = false;
        RadioButtonSwitchTesteIsChecked = false;
        RadioButtonTestENETIsChecked = false;
        OnPropertyChanged(nameof(RadioButtonBoardinfoENETIsChecked));
        OnPropertyChanged(nameof(RadioButtonHomeENETIsChecked));
        OnPropertyChanged(nameof(RadioButtonHomeIsChecked));
        OnPropertyChanged(nameof(RadioButtonLEDTestIsChecked));
        OnPropertyChanged(nameof(RadioButtonOtherTestIsChecked));
        OnPropertyChanged(nameof(RadioButtonSwitchTesteIsChecked));
        OnPropertyChanged(nameof(RadioButtonTestENETIsChecked));
    }

    [RelayCommand]
    private void DeviceCountRefresh()
    {
        CloseInterfaceITDevices();
        GetInterfaceITDevices();
        while (_selectedController > DeviceList.Count - 1)
        {
            _selectedController--;
        }
        if (_selectedController != -1)
        {
            SelectedController = _selectedController;
            return;
        }
        SelectedController = 0;
    }

    private void USB_ENET()
    {
        if (Enet)
        {
            CurrentView = HomeUSBVM;
        }
        else if (!Enet)
        {
            CurrentView = HomeENETVM;
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

    private void GetDeviceList()
    {
        int intSize = 0;
        _ = interfaceIT_GetDeviceList(null, ref intSize, null);
        byte[] byteDeviceList = new byte[intSize];
        _ = interfaceIT_GetDeviceList(byteDeviceList, ref intSize, null);
        DeviceList.Clear();
        HomeUSBVM.GetBoardInfo();
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
                string boardType = string.Empty;
                foreach (var field in typeof(InterfaceIT_BoardIDs).GetFields())
                {
                    if ((string)field.GetValue(null) == bOARDCAPS.szBoardType)
                        boardType = field.Name.ToString().Replace('_', ' ');
                }
                DeviceList.Add(new InterfaceIT_BoardInfo.Device
                {
                    Id = i++,
                    BoardType = boardType,
                    SerialNumber = device,
                    Session = session,
                    DeviceInfo = bOARDCAPS
                });
                LEDTestViewModels.Add(new LEDTestViewModel()
                {
                    Session = session,
                    LEDFirst = bOARDCAPS.nLEDFirst,
                    LEDLast = bOARDCAPS.nLEDLast,
                    BoardType = boardType 
                });
                SwitchTestViewModels.Add(new SwitchTestViewModel()
                {
                    Session = session 
                });
                OtherTestViewModels.Add(new OtherTestsViewModel()
                {
                    Session = session,
                    Features = bOARDCAPS.dwFeatures,
                    DatalineFirst = bOARDCAPS.nDatalineFirst,
                    SevenSegmentCount = bOARDCAPS.n7SegmentCount,
                    SevenSegmentFirst = bOARDCAPS.n7SegmentFirst,
                    SevenSegmentLast = bOARDCAPS.n7SegmentLast
                });
            }
        }
    }

    private void GetCurrentView()
    {
        if (DeviceList.Count != 0)
        {
            if (RadioButtonLEDTestIsChecked)
            {
                CurrentView = LEDTestViewModels.ElementAtOrDefault(SelectedController);
            }
            else if (RadioButtonSwitchTesteIsChecked)
            {
                CurrentView = SwitchTestViewModels.ElementAtOrDefault(SelectedController);
            }
            else if (RadioButtonOtherTestIsChecked)
            {
                CurrentView = OtherTestViewModels.ElementAtOrDefault(SelectedController);
            }
        }
        else if (!RadioButtonHomeIsChecked)
        {
            CurrentView = null;
        }
    }

    public void CloseInterfaceITDevices()
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
                    _ = interfaceIT_7Segment_Display(device.Session, null, i);
                }
                _ = interfaceIT_7Segment_Enable(device.Session, false);
            }


            if ((device.DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_OUTPUT_DATALINE) != 0)
            {
                for (int i = device.DeviceInfo.nDatalineFirst; i <= device.DeviceInfo.nDatalineLast; i++)
                {
                    _ = interfaceIT_Dataline_Set(device.Session, i, false);
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

    public void Receive(SimConnectStausMessage message)
    {
        IsSimConnectOpen = message.Value;
        OnPropertyChanged(nameof(IsSimConnectOpen));
    }
}

public class SimConnectStausMessage : ValueChangedMessage<bool>
{
    public SimConnectStausMessage(bool value) : base(value)
    {
    }
}