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
using Device_Interface_Manager.MVVM.View;
using Device_Interface_Manager.Core;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceITAPI_Data;

namespace Device_Interface_Manager.MVVM.ViewModel;

public partial class MainViewModel : ObservableObject, IRecipient<SimConnectStausMessage>
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


    public string DIMVersionText { get; set; } = "DIM Dev Version: ";
    public string DIMVersionDate { get; set; } = "2023-07-13";
    //public string DIMVersion { get; } = "DIM Dev Version " + Assembly.GetEntryAssembly().GetName().Version;

    public MainViewModel()
    {
        HomeUSBVM = new HomeUSBViewModel();
        HomeENETVM = new HomeENETViewModel();
        BoardinfoENETVM = new BoardinfoENETViewModel();
        TestENETVM = new TestENETViewModel();
        SettingsVM = new SettingsViewModel();

        GetInterfaceITDevices();

        USB_ENET();

        SelectedController = 0;

        Update();
        StrongReferenceMessenger.Default.Register(this);
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
        if (!System.Windows.Application.Current.Windows.OfType<ProfileCreatorView>().Any())
        {
            NavigationService navigationService = new();
            navigationService.NavigateTo<ProfileCreatorView>(DeviceList.ToArray());

            //Dirty
            System.Windows.Application.Current.Windows.OfType<ProfileCreatorView>().First().Closed += ProfileCreatorView_Closed;
        }
    }

    //Dirty
    private void ProfileCreatorView_Closed(object sender, EventArgs e)
    {
        HomeUSBVM.AddProfiles();
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
                interfaceIT_GetBoardInfo(session, out InterfaceIT_BoardInfo.BOARDCAPS bOARDCAPS);
                if (bOARDCAPS.szBoardType == "330A" || bOARDCAPS.szBoardType == "332C")
                {
                    interfaceIT_SetBoardOptions(session, BoardOptions.INTERFACEIT_BOARD_OPTION_FORCE64);
                }
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