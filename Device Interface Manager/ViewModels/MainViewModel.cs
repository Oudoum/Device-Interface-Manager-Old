using System;
using System.Linq;
using System.IO;
using System.Text.Json;
using System.Reflection;
using System.Collections.ObjectModel;
using AutoUpdaterDotNET;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Device_Interface_Manager.Devices.interfaceIT.USB;
using Device_Interface_Manager.Core;
using static Device_Interface_Manager.Devices.interfaceIT.USB.InterfaceITAPI_Data;
using Device_Interface_Manager.SimConnectProfiles;
using MahApps.Metro.Controls.Dialogs;
using Device_Interface_Manager.Devices.COM;

namespace Device_Interface_Manager.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private const string updateLink = "https://raw.githubusercontent.com/Oudoum/Device-Interface-Manager-Download/main/Updates/AutoUpdaterDIM.xml";

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
    private bool _isSimConnectOpen;

    [ObservableProperty]
    private bool _radioButtonHomeIsChecked = true;

    [ObservableProperty]
    private bool _radioButtonLEDTestIsChecked;

    [ObservableProperty]
    private bool _radioButtonSwitchTesteIsChecked;

    [ObservableProperty]
    private bool _radioButtonOtherTestIsChecked;

    [ObservableProperty]
    private bool _radioButtonHomeENETIsChecked = true;

    [ObservableProperty]
    private bool _radioButtonBoardinfoENETIsChecked;

    [ObservableProperty]
    private bool _radioButtonTestENETIsChecked;

    [ObservableProperty]
    private bool _settingsIsChecked;

    [ObservableProperty]
    private string _totalControllers;

    [ObservableProperty]
    private bool _enet = Properties.Settings.Default.ENET;
    partial void OnEnetChanged(bool value)
    {
        Properties.Settings.Default.ENET = value;
        Properties.Settings.Default.Save();
    }

    [ObservableProperty]
    private ObservableObject _currentView;

    partial void OnCurrentViewChanged(ObservableObject value)
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
                HomeUSBVM.GetBoardInfo(DeviceList[value]);
                HomeUSBVM.Devices = DeviceList.ToList();
            }
        }
    }

    public Version DIMVersion { get; set; } = Assembly.GetEntryAssembly().GetName().Version;

    private int hidDeviceCount;

    public MainViewModel(ILogger<MainViewModel> logger, ILogger<HomeUSBViewModel> homeUSBLogger, ILogger<SettingsViewModel> settingsLogger)
    {
        HomeUSBVM = new HomeUSBViewModel(homeUSBLogger);
        HomeENETVM = new HomeENETViewModel();
        BoardinfoENETVM = new BoardinfoENETViewModel();
        TestENETVM = new TestENETViewModel();
        SettingsVM = new SettingsViewModel(settingsLogger);

        GetInterfaceITDevices();

        USB_ENET();

        SelectedController = 0;

        Update();
        SimConnectClient.Instance.SimConnectConnectionChanged += Instance_SimConnectConnectionChanged;

        hidDeviceCount = GetSpecificDevicesCount();
        HidSharp.DeviceList.Local.Changed += COMDeviceListChanged;
    }

    private void Instance_SimConnectConnectionChanged(object sender, bool isConnected)
    {
        IsSimConnectOpen = isConnected;
    }

    private int GetSpecificDevicesCount()
    {
        return HidSharp.DeviceList.Local.GetHidDevices(8145, 1002).Where(s =>
        {
            try
            {
                return s.GetFriendlyName() == "Plain I/O";
            }
            catch
            {
                return false;
            }
        }).Count();
    }

    private void COMDeviceListChanged(object sender, HidSharp.DeviceListChangedEventArgs e)
    {
        int hidDeviceCount = GetSpecificDevicesCount();
        if (this.hidDeviceCount != hidDeviceCount)
        {
            this.hidDeviceCount = hidDeviceCount;
            System.Windows.Application.Current.Dispatcher.Invoke(RefreshDeviceList);
        }
    }

    private static void Update()
    {
        AutoUpdater.ShowSkipButton = false;
        AutoUpdater.ShowRemindLaterButton = false;
        AutoUpdater.SetOwner(System.Windows.Application.Current.MainWindow);
        AutoUpdater.Start(updateLink);
    }

    [RelayCommand]
    private void OpenProfileCreator()
    {
        ProfileCreatorViewModel profileCreatorViewModel = new(DialogCoordinator.Instance, DeviceList);
        ViewLocator viewLocator = new(profileCreatorViewModel);
        viewLocator.Closed += () =>
        {
            HomeUSBVM.AddProfiles();
        };
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
        TotalControllers = totalControllers > 0 ? "Total controllers detected: " + totalControllers : "No controllers detected!";
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
                interfaceIT_GetBoardInfo(session, out InterfaceIT_BoardInfo.BoardInfo bOARDCAPS);
                InterfaceIT_BoardIDs boardID = (InterfaceIT_BoardIDs)Convert.ToUInt16(bOARDCAPS.BoardType, 16);
                if (boardID == InterfaceIT_BoardIDs.FDS_CONTROLLER_MCP || boardID == InterfaceIT_BoardIDs.FDS_737_PMX_MCP || boardID == InterfaceIT_BoardIDs.JetMAX_737_RADIO)
                {
                    interfaceIT_SetBoardOptions(session, (uint)BoardOptions.Force64);
                }
                string boardName = boardID.ToString().Replace('_', ' ');
                DeviceList.Add(new InterfaceIT_BoardInfo.Device
                {
                    Id = i++,
                    BoardID = boardID,
                    BoardName = boardName,
                    SerialNumber = device,
                    Session = session,
                    BoardInfo = bOARDCAPS
                });
                LEDTestViewModels.Add(new LEDTestViewModel()
                {
                    Session = session,
                    LEDFirst = bOARDCAPS.LEDFirst,
                    LEDLast = bOARDCAPS.LEDLast,
                    BoardType = boardName 
                });
                SwitchTestViewModels.Add(new SwitchTestViewModel()
                {
                    Session = session 
                });
                OtherTestViewModels.Add(new OtherTestsViewModel()
                {
                    Session = session,
                    Features = bOARDCAPS.Features,
                    DatalineCount = bOARDCAPS.DatalineCount,
                    DatalineFirst = bOARDCAPS.DatalineFirst,
                    DatalineLast = bOARDCAPS.DatalineLast,
                    SevenSegmentCount = bOARDCAPS.SevenSegmentCount,
                    SevenSegmentFirst = bOARDCAPS.SevenSegmentFirst,
                    SevenSegmentLast = bOARDCAPS.SevenSegmentLast
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
            InterfaceITDisable(device);
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
}