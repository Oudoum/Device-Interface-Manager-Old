using System;
using System.Linq;
using System.IO;
using System.Text.Json;
using System.Reflection;
using System.Collections.ObjectModel;
using AutoUpdaterDotNET;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using Device_Interface_Manager.interfaceIT.USB;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceITAPI_Data;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceIT_BoardInfo.BoardInformationStructure;
using Device_Interface_Manager.MVVM.View;

namespace Device_Interface_Manager.MVVM.ViewModel;

public partial class MainViewModel : ObservableObject, IRecipient<SimConnectStausMessage>
{
    private const string updateLink = "https://raw.githubusercontent.com/Oudoum/Device-Interface-Manager-Download/main/Updates/AutoUpdaterDIM.xml";
    private const string version = "1.1.7";

    public HomeUSBViewModel HomeUSBVM { get; set; }

    public HomeENETViewModel HomeENETVM { get; set; }

    public ObservableCollection<LEDTestViewModel> LEDTestViewModels { get; set; } = new();

    public ObservableCollection<SwitchTestViewModel> SwitchTestViewModels { get; set; } = new();

    public ObservableCollection<OtherTestsViewModel> OtherTestViewModels { get; set; } = new();

    public ObservableCollection<InterfaceIT_BoardInfo.Device> DeviceList { get; set; } = new();

    public BoardinfoENETViewModel BoardinfoENETVM { get; set; }

    public TestENETViewModel TestENETVM { get; set; }

    public SettingsViewModel SettingsVM { get; set; }

    [ObservableProperty]
    public bool _isSimConnectOpen;

    [ObservableProperty]
    public bool _radioButtonHomeIsChecked = true;

    [ObservableProperty]
    public bool _radioButtonLEDTestIsChecked;

    [ObservableProperty]
    public bool _radioButtonSwitchTesteIsChecked;

    [ObservableProperty]
    public bool _radioButtonOtherTestIsChecked;

    [ObservableProperty]
    public bool _radioButtonHomeENETIsChecked = true;

    [ObservableProperty]
    public bool _radioButtonBoardinfoENETIsChecked;

    [ObservableProperty]
    public bool _radioButtonTestENETIsChecked;

    [ObservableProperty]
    public bool _settingsIsChecked;

    [ObservableProperty]
    public string _totalControllers;

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
        SettingsIsChecked = (value?.GetType() == typeof(SettingsViewModel));
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

    public string DIMVersion { get; } = "DIM Version " + version;

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
    private void OpenProfileCreator()
    {
        if (!System.Windows.Application.Current.Windows.OfType<TestProfileView>().Any())
        {
            TestProfileView profileCreator = new();
            profileCreator.Show();
        }
    }

    [RelayCommand]
    private void OnWindowClosing()
    {
        CloseInterfaceITDevices();
        HomeENETVM.SaveENETConnections();
        HomeUSBVM.SaveUSBConnections();
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
        if (Enet = !Enet)
        {
            RadioButtonHomeIsChecked = true;
            CurrentView = HomeUSBVM;
            return;
        }
        RadioButtonHomeENETIsChecked = true;
        CurrentView = HomeENETVM;
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
    }

    [RelayCommand]
    private void RefreshDeviceList()
    {
        CloseInterfaceITDevices();
        GetInterfaceITDevices();
        if (DeviceList.Count > 0)
        {
            SelectedController = Math.Min(_selectedController, DeviceList.Count - 1);
            return;
        }
        SelectedController = 0;
    }

    private void USB_ENET()
    {
        if (Enet)
        {
            CurrentView = HomeUSBVM;
            return;
        }
        CurrentView = HomeENETVM;
    }

    private void GetInterfaceITDevices()
    {
        interfaceIT_OpenControllers();
        GetTotalControllers();
        GetDeviceList();
    }

    private void GetTotalControllers()
    {
        int totalControllers = -1;
        interfaceIT_GetTotalControllers(ref totalControllers);
        if (totalControllers > 0)
        {
            TotalControllers = $"Total controllers detected: {totalControllers}\nClick to refresh!";
            return;
        }
        TotalControllers = "No controllers detected!\nClick to refresh!";
    }

    private void GetDeviceList()
    {
        DeviceList.Clear();
        HomeUSBVM.GetBoardInfo();
        LEDTestViewModels.Clear();
        SwitchTestViewModels.Clear();
        OtherTestViewModels.Clear();
        int i = 0;
        foreach (string device in interfaceIT_GetDeviceList())
        {
            if (!string.IsNullOrEmpty(device))
            {
                interfaceIT_Bind(device, out uint session);
                interfaceIT_GetBoardInfo(session, out BOARDCAPS bOARDCAPS);
                string boardType = string.Empty;
                foreach (FieldInfo field in typeof(InterfaceIT_BoardIDs).GetFields())
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
            switch (true)
            {
                case bool _ when RadioButtonLEDTestIsChecked:
                    CurrentView = LEDTestViewModels.ElementAtOrDefault(SelectedController);
                    break;

                case bool _ when RadioButtonSwitchTesteIsChecked:
                    CurrentView = SwitchTestViewModels.ElementAtOrDefault(SelectedController);
                    break;

                case bool _ when RadioButtonOtherTestIsChecked:
                    CurrentView = OtherTestViewModels.ElementAtOrDefault(SelectedController);
                    break;
            }
        }
        else if (!RadioButtonHomeIsChecked)
        {
            CurrentView = null;
        }
    }

    public void CloseInterfaceITDevices()
    {
        foreach (InterfaceIT_BoardInfo.Device device in DeviceList)
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
                    interfaceIT_7Segment_Display(device.Session, null, i);
                }
                interfaceIT_7Segment_Enable(device.Session, false);
            }


            if ((device.DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_OUTPUT_DATALINE) != 0)
            {
                for (int i = device.DeviceInfo.nDatalineFirst; i <= device.DeviceInfo.nDatalineLast; i++)
                {
                    interfaceIT_Dataline_Set(device.Session, i, false);
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

    public static T LoadConnectionsData<T>(string fullPath) where T : new()
    {
        if (File.Exists(fullPath))
        {
            return JsonSerializer.Deserialize<T>(File.ReadAllText(fullPath));
        }
        return new T();
    }

    public static void SaveConnectionsData<T>(string fullPath, T connections) where T : System.Collections.ICollection
    {
        string directory = Path.GetDirectoryName(fullPath);
        if (connections.Count == 0)
        {
            if (Directory.Exists(directory))
            {
                File.Delete(fullPath);
            }
            return;
        }
        Directory.CreateDirectory(directory);
        string json = JsonSerializer.Serialize(connections, new JsonSerializerOptions { WriteIndented = true });
        if (File.Exists(fullPath) && File.ReadAllText(fullPath) == json)
        {
            return;
        }
        File.WriteAllText(fullPath, json);
    }

    public void Receive(SimConnectStausMessage message)
    {
        IsSimConnectOpen = message.Value;
    }
}

public class SimConnectStausMessage : ValueChangedMessage<bool>
{
    public SimConnectStausMessage(bool value) : base(value)
    {
    }
}