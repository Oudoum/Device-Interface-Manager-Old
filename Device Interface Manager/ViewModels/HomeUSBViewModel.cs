using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Device_Interface_Manager.Devices.interfaceIT.USB;
using Device_Interface_Manager.SimConnectProfiles;
using Device_Interface_Manager.Models;
using static Device_Interface_Manager.Models.HomeUSBModel;

namespace Device_Interface_Manager.ViewModels;

public partial class HomeUSBViewModel : ObservableObject
{
    private const string usb = @"Profiles\USB.json";

    [ObservableProperty]
    private bool _isUSBEnabled = true;

    public ObservableCollection<string> BoardInfo { get; set; } = new();

    public ObservableCollection<Connection> Connections { get; set; }

    public ObservableCollection<string> Profiles { get; set; } = new();

    public List<InterfaceIT_BoardInfo.Device> Devices { get; set; } = new();

    private List<USB> USBList { get; set; } = new();

    private readonly Dictionary<string, Func<Connection, Task>> profileActions;

    private List<ProfileCreatorModel> profileCreatorModels;

    [ObservableProperty]
    public ObservableCollection<string> _logMessages;

    //Devices.COM.SerialDevice SerialDevice { get; set; } = new();

    public HomeUSBViewModel(ILogger<HomeUSBViewModel> logger)
    {
        profileActions = new()
        {
            { "-- None --", null },
            { "Fenix A320 Left MCDU", StartUSBProfile<SimConnectProfiles.FENIX.A320.USB.MCDU_L> },
            { "Fenix A320 Right MCDU", StartUSBProfile<SimConnectProfiles.FENIX.A320.USB.MCDU_R> },
            { "FBW A32NX Left MCDU", StartUSBProfile<SimConnectProfiles.FBW.A32NX.USB.MCDU_L> },
            { "FBW A32NX Right MCDU", StartUSBProfile<SimConnectProfiles.FBW.A32NX.USB.MCDU_R> },
            { "PMDG 737NG Left CDU", StartUSBProfile<SimConnectProfiles.PMDG.B737.USB.NG_CDU_L> },
            { "PMDG 737NG Right CDU", StartUSBProfile<SimConnectProfiles.PMDG.B737.USB.NG_CDU_R> },
            { "PMDG 737NG MCP (330F | 3311)", StartUSBProfile<SimConnectProfiles.PMDG.B737.USB.NG_MCP_3311> },
            { "PMDG 737NG MCP (3327)", StartUSBProfile<SimConnectProfiles.PMDG.B737.USB.NG_MCP_3327> },
            { "PMDG 737NG MCP (330A | 332C)", StartUSBProfile<SimConnectProfiles.PMDG.B737.USB.NG_MCP_330A_332C> },
            { "PMDG 737NG EFIS L (330B | 332D)", StartUSBProfile<SimConnectProfiles.PMDG.B737.USB.NG_EFIS_L_330B_332D> },
            { "PMDG 737NG EFIS R (330C | 332E)", StartUSBProfile<SimConnectProfiles.PMDG.B737.USB.NG_EFIS_R_330C_332E> },
            //{ "Asobo 747-8I Left CDU", StartUSBProfile<SimConnectProfiles.Asobo.B747.USB.CDU_L>},
            //{ "Asobo 747-8I Right CDU", StartUSBProfile<SimConnectProfiles.Asobo.B747.USB.CDU_R>},
            { "[P3D] PMDG 747 Left CDU", StartUSBProfile<SimConnectProfiles.PMDG.B747.USB.B747_CDU_L> },
            { "[P3D] PMDG 747 Right CDU", StartUSBProfile<SimConnectProfiles.PMDG.B747.USB.B747_CDU_R> },
            { "[P3D] PMDG 747 Center CDU", StartUSBProfile<SimConnectProfiles.PMDG.B747.USB.B747_CDU_C> },
            { "[P3D] PMDG 747 MCP (330A)", StartUSBProfile<SimConnectProfiles.PMDG.B747.USB.B747_MCP_330A> },
            { "[P3D] PMDG 747 EFIS L (330B)", StartUSBProfile<SimConnectProfiles.PMDG.B747.USB.B747_EFIS_L_330B> },
            { "[P3D] PMDG 747 EFIS R (330C)", StartUSBProfile<SimConnectProfiles.PMDG.B747.USB.B747_EFIS_R_330C> },
            //{ "[P3D] PMDG 777 Left CDU", StartUSBProfile<SimConnectProfiles.PMDG.B777.USB.B777_CDU_L> },
            //{ "[P3D] PMDG 777 Right CDU", StartUSBProfile<SimConnectProfiles.PMDG.B777.USB.B777_CDU_R> },
            //{ "[P3D] PMDG 777 Center CDU", StartUSBProfile<SimConnectProfiles.PMDG.B777.USB.B777_CDU_C> },

        //"PMDG 737MAX Left CDU"
        //"PMDG 737MAX Right CDU"

        //"PMDG 777 Left CDU"
        //"PMDG 777 Right CDU"
        //"PMDG 777 Center CDU"

        //"PMDG 747 Left CDU"
        //"PMDG 747 Right CDU"
        //"PMDG 747 Center CDU"

        };

        foreach (var item in profileActions.Keys)
        {
            Profiles.Add(item);
        }

        AddProfiles();

        Connections = MainViewModel.LoadConnectionsData<ObservableCollection<Connection>>(usb);
        GetBoardInfo();

        if (Properties.Settings.Default.AutoHide && Connections.Count > 0)
        {
            _ = StartUSB();
        }
    }

    public void AddProfiles()
    {
        profileCreatorModels = new();
        string folderPath = Path.Combine("Profiles", "Creator");
        if (Directory.Exists(folderPath))
        {
            foreach (string filePath in Directory.GetFiles(folderPath))
            {
                try
                {
                    profileCreatorModels.Add(JsonSerializer.Deserialize<ProfileCreatorModel>(File.ReadAllText(filePath)));
                }
                catch (Exception)
                {

                }
            }
        }

        List<string> profileKeys = new();

        foreach (var item in profileCreatorModels)
        {
            if (item.Driver == ProfileCreatorModel.FDSUSB)
            {
                string profileKey = '#' + item.ProfileName;
                profileKeys.Add(profileKey);
                if (profileActions.ContainsKey(profileKey))
                {
                    continue;
                }
                profileActions.Add(profileKey, StartUSBCustomProfile);
                Profiles.Add(profileKey);
            }
        }

        List<string> keys = profileActions.Keys.Where(key => key.StartsWith("#")).ToList();

        foreach (var item in profileKeys)
        {
            if (keys.Contains(item))
            {
                continue;
            }
            profileActions.Remove(item);
            Profiles.Remove(item);
        }
    }

    public void GetBoardInfo()
    {
        BoardInfo.Clear();
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

    public void GetBoardInfo(InterfaceIT_BoardInfo.Device device)
    {
        InterfaceIT_BoardInfo.BoardInfo bOARDCAPS = device.BoardInfo;

        BoardInfo.Clear();

        BoardInfo.Add($"Board \"{device.BoardName}\" ({bOARDCAPS.BoardType}) manufactured on {bOARDCAPS.ManufactureDate} has the following features: ");

        AddFeatureInfo(bOARDCAPS.Features == InterfaceIT_BoardInfo.Features.None, "No features programmed. Please obtain the update patch for this board.");

        AddFeatureInfo((bOARDCAPS.Features & InterfaceIT_BoardInfo.Features.OutputLED) != 0, $"{bOARDCAPS.LEDCount} | LEDs ( {bOARDCAPS.LEDFirst} - {bOARDCAPS.LEDLast} )");

        AddFeatureInfo((bOARDCAPS.Features & InterfaceIT_BoardInfo.Features.InputSwitches) != 0, $"{bOARDCAPS.SwitchCount} | Switches ( {bOARDCAPS.SwitchFirst} - {bOARDCAPS.SwitchLast} )");

        AddFeatureInfo((bOARDCAPS.Features & InterfaceIT_BoardInfo.Features.Output7Segment) != 0, $"{bOARDCAPS.SevenSegmentCount} | 7 Segments ( {bOARDCAPS.SevenSegmentFirst} - {bOARDCAPS.SevenSegmentLast} )");

        AddFeatureInfo((bOARDCAPS.Features & InterfaceIT_BoardInfo.Features.OutputDataLine) != 0, $"{bOARDCAPS.DatalineCount} | Datalines ( {bOARDCAPS.DatalineFirst} - {bOARDCAPS.DatalineLast} )");

        //Not available
        AddFeatureInfo((bOARDCAPS.Features & InterfaceIT_BoardInfo.Features.OutputServo) != 0, $"{bOARDCAPS.ServoController} | Servos ( {bOARDCAPS.ServoControllerFirst} - {bOARDCAPS.ServoControllerLast} )");

        AddFeatureInfo((bOARDCAPS.Features & InterfaceIT_BoardInfo.Features.SpecialBrightness) != 0, "Brightness control supported");

        AddFeatureInfo((bOARDCAPS.Features & InterfaceIT_BoardInfo.Features.SpecialAnalogInput) != 0, "Analog input supported (Single Channel)");

        AddFeatureInfo((bOARDCAPS.Features & InterfaceIT_BoardInfo.Features.SpecialAnalog16Input) != 0, "Analog input supported (16 Channels)");
    }

    [RelayCommand]
    private async Task StartUSB()
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
        StopUSB();
    }

    private void StopUSB()
    {
        USBList.ForEach(o => o.Close());
        USBList.Clear();
        Device_Interface_Manager.SimConnectProfiles.Profiles.Instance.Stop();
        IsUSBEnabled = true;
    }

    private async Task StartUSBProfile<T>(Connection connection) where T : USB, new()
    {
        T profile = new();
        USBList.Add(profile);
        await Task.Run(() => profile.StartAsync(Devices.FirstOrDefault(o => o.SerialNumber == connection.Serial)));
    }

    private async Task StartUSBCustomProfile(Connection connection)
    {
        await Task.Run(() => Device_Interface_Manager.SimConnectProfiles.Profiles.Instance.StartAsync(profileCreatorModels.FirstOrDefault(s => '#' + s.ProfileName == connection.SelectedProfile), Devices.FirstOrDefault(o => o.SerialNumber == connection.Serial)));
    }

    private async Task StartUSBProfiles(Connection connection)
    {
        if (connection.SelectedProfile is null)
        {
            System.Windows.MessageBox.Show("Please reselect the profile for: " + connection.Name);
            StopUSB();
            return;
        }

        else if (connection.Serial.Length != 12 || connection.Serial == "SERIALNUMBER")
        {
            System.Windows.MessageBox.Show("Please Drag & Drop or enter the serial number for: " + connection.Name);
            StopUSB();
            return;
        }

        else if (profileActions.TryGetValue(connection.SelectedProfile, out var action))
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
        Connections.Add(new Connection() { Name = "NAME", Serial = "SERIALNUMBER", SelectedProfile = Profiles[0] });
    }

    [RelayCommand]
    private void ResetUSBScreens()
    {
        USBList.ForEach(o =>
        {
            if (o.PMDGCDU is not null)
            {
                o.PMDGCDU.Top = 0;
                o.PMDGCDU.Left = 0;
            }
            if (o.FBWA32NXMCDU is not null)
            {
                o.FBWA32NXMCDU.Top = 0;
                o.FBWA32NXMCDU.Left = 0;
            }
        });
    }

    public void SaveUSBConnections()
    {
        MainViewModel.SaveConnectionsData(usb, Connections);
    }
}