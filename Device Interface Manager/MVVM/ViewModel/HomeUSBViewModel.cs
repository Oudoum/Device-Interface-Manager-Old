using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Device_Interface_Manager.interfaceIT.USB;
using Device_Interface_Manager.MSFSProfiles;
using static Device_Interface_Manager.MVVM.Model.HomeUSBModel;

namespace Device_Interface_Manager.MVVM.ViewModel;

public partial class HomeUSBViewModel : ObservableObject
{
    private const string usb = @"Profiles\USB.json";

    [ObservableProperty]
    private bool _isUSBEnabled = true;

    public ObservableCollection<string> BoardInfo { get; set; } = new();

    public ObservableCollection<string> BoardType { get; set; } = new();

    public ObservableCollection<Connection> Connections { get; set; }

    public ObservableCollection<Profile> Profiles { get; set; } = new();

    public List<InterfaceIT_BoardInfo.Device> Devices { get; set; } = new();

    private List<USB> USBList { get; set; } = new();

    private readonly Dictionary<string, Func<Connection, Task>> profileActions;

    public HomeUSBViewModel()
    {
        profileActions = new()
        {
            { "-- None --", null },
            { "Fenix A320 Left MCDU", StartUSBProfile<MSFSProfiles.FENIX.A320.USB.MCDU_L> },
            { "Fenix A320 Right MCDU", StartUSBProfile<MSFSProfiles.FENIX.A320.USB.MCDU_R> },
            { "FBW A32NX Left MCDU", StartUSBProfile<MSFSProfiles.FBW.A32NX.USB.MCDU_L> },
            { "FBW A32NX Right MCDU", StartUSBProfile<MSFSProfiles.FBW.A32NX.USB.MCDU_R> },
            { "PMDG 737NG Left CDU", StartUSBProfile<MSFSProfiles.PMDG.B737.USB.NG_CDU_L> },
            { "PMDG 737NG Right CDU", StartUSBProfile<MSFSProfiles.PMDG.B737.USB.NG_CDU_R> },
            { "PMDG 737NG MCP (3311)", StartUSBProfile<MSFSProfiles.PMDG.B737.USB.NG_MCP_3311> },
            { "PMDG 737NG MCP (3327)", StartUSBProfile<MSFSProfiles.PMDG.B737.USB.NG_MCP_777> },
            { "PMDG 737NG MCP (330A | 332C)", StartUSBProfile<MSFSProfiles.PMDG.B737.USB.NG_MCP_330A_332C> },
            { "PMDG 737NG EFIS L (330B | 332D)", StartUSBProfile<MSFSProfiles.PMDG.B737.USB.NG_EFIS_L_330B_332D> },
            { "PMDG 737NG EFIS R (330C | 332E)", StartUSBProfile<MSFSProfiles.PMDG.B737.USB.NG_EFIS_R_330C_332E> },

        //"PMDG 737MAX Left CDU"
        //"PMDG 737MAX Right CDU"

        //"PMDG 777 Left CDU"
        //"PMDG 777 Right CDU"
        //"PMDG 777 Center CDU"

        //"PMDG 747 Left CDU"
        //"PMDG 747 Right CDU"
        //"PMDG 747 Center CDU"

        };

        CreateProfiles();
        Connections = MainViewModel.LoadConnectionsData<ObservableCollection<Connection>>(usb);
        GetBoardInfo();

        if (Properties.Settings.Default.AutoHide && Connections.Count > 0)
        {
            StartUSB();
        }
    }

    public void GetBoardInfo()
    {
        BoardInfo.Clear();
        BoardType.Clear();
        for (int i = 0; i < 4; i++)
        {
            BoardInfo.Add(string.Empty);
        }
        BoardInfo.Add("                                          No USB device is found!");
    }

    private void AddFeatureInfo(bool hasFeature, string featureInfo)
    {
        BoardInfo.Add(hasFeature ? featureInfo : string.Empty);
    }

    public void GetBoardInfo(InterfaceIT_BoardInfo.BOARDCAPS bOARDCAPS)
    {
        BoardInfo.Clear();
        BoardType.Clear();

        BoardInfo.Add($"Board {bOARDCAPS.szBoardType} manufactured on {bOARDCAPS.szManufactureDate} has the following features: ");

        AddFeatureInfo(bOARDCAPS.dwFeatures == InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_NONE, "No features programmed. Please obtain the update patch for this board.");

        AddFeatureInfo((bOARDCAPS.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_OUTPUT_LED) != 0, $"{bOARDCAPS.nLEDCount} | LEDs ( {bOARDCAPS.nLEDFirst} - {bOARDCAPS.nLEDLast} )");

        AddFeatureInfo((bOARDCAPS.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_INPUT_SWITCHES) != 0, $"{bOARDCAPS.nSwitchCount} | Switches ( {bOARDCAPS.nSwitchFirst} - {bOARDCAPS.nSwitchLast} )");

        AddFeatureInfo((bOARDCAPS.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_OUTPUT_7SEGMENT) != 0, $"{bOARDCAPS.n7SegmentCount} | 7 Segments ( {bOARDCAPS.n7SegmentFirst} - {bOARDCAPS.n7SegmentLast} )");

        AddFeatureInfo((bOARDCAPS.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_OUTPUT_DATALINE) != 0, $"{bOARDCAPS.nDatalineCount} | Datalines ( {bOARDCAPS.nDatalineFirst} - {bOARDCAPS.nDatalineLast} )");

        //Not available
        AddFeatureInfo((bOARDCAPS.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_OUTPUT_SERVO) != 0, $"{bOARDCAPS.nServoController} | Servos ( {bOARDCAPS.nServoControllerFirst} - {bOARDCAPS.nServoControllerLast} )");

        AddFeatureInfo((bOARDCAPS.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_SPECIAL_BRIGHTNESS) != 0, "Brightness control supported");

        AddFeatureInfo((bOARDCAPS.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_SPECIAL_ANALOG_INPUT) != 0, "Analog input supported (Single Channel)");

        AddFeatureInfo((bOARDCAPS.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_SPECIAL_ANALOG16_INPUT) != 0, "Analog input supported (16 Channels)");

        foreach (var field in typeof(InterfaceIT_BoardIDs).GetFields())
        {
            if ((string)field.GetValue(null) == bOARDCAPS.szBoardType)
                BoardType.Add(field.Name.ToString().Replace('_', ' '));
        }
    }

    [RelayCommand]
    private async void StartUSB()
    {
        if (!(IsUSBEnabled = !IsUSBEnabled))
        {
            SaveUSBConnections();
            foreach (var connection in Connections)
            {
                await StartUSBProfiles(connection);
            }
            return;
        }
        USBList.ForEach(o => o.Stop());
        USBList.Clear();
    }

    private async Task StartUSBProfile<T>(Connection connection) where T : USB, new()
    {
        T profile = new();
        USBList.Add(profile);
        await Task.Run(() => profile.StartAsync(Devices.FirstOrDefault(o => o.SerialNumber == connection.Serial)));
    }

    private async Task StartUSBProfiles(Connection connection)
    {
        if (profileActions.TryGetValue(connection.Profile.Name, out var action))
        {
            if (action is not null)
            {
                await action(connection);
            }
        }
    }

    [RelayCommand]
    private void DeleteRow(Connection connection)
    {
        Connections.Remove(connection);
    }

    [RelayCommand]
    private void AddRow()
    {
        Connections.Add(new Connection() { Name = "NAME", Serial = "SERIALNUMBER", Profile = Profiles[0] });
    }

    private void CreateProfiles()
    {
        foreach (var item in profileActions.Keys)
        {
            Profiles.Add(new Profile { Name = item });
        }
    }

    [RelayCommand]
    private void ResetUSBScreens()
    {
        USBList.ForEach(o =>
        {
            if (o.pMDG737CDU is not null)
            {
                o.pMDG737CDU.Top = 0;
                o.pMDG737CDU.Left = 0;
            }
            if (o.fBWA32NXMCDU is not null)
            {
                o.fBWA32NXMCDU.Top = 0;
                o.fBWA32NXMCDU.Left = 0;
            }
        });
    }

    public void SaveUSBConnections()
    {
        MainViewModel.SaveConnectionsData(usb, Connections);
    }
}