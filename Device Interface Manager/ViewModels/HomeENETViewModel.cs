using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Device_Interface_Manager.Devices.interfaceIT.ENET;
using Device_Interface_Manager.SimConnectProfiles;
using static Device_Interface_Manager.Models.HomeENETModel;

namespace Device_Interface_Manager.ViewModels;

public partial class HomeENETViewModel : ObservableObject
{
    private const string enet = @"Profiles\ENET.json";

    [ObservableProperty]
    private bool _isENETEnabled = true;

    public ObservableCollection<Connection> Connections { get; set; } = new();

    public ObservableCollection<string> Profiles { get; set; } = new();

    public InterfaceITEthernet interfaceITEthernet;

    private List<ENET> ENETList { get; set; } = new();

    private readonly Dictionary<string, Func<Connection, Task>> profileActions;

    public HomeENETViewModel()
    {
        profileActions = new()
        {
            { "-- None --", null },
            { "Fenix A320 Left MCDU", StartENETProfileAsync<SimConnectProfiles.FENIX.A320.E.MCDU_L> },
            { "Fenix A320 Right MCDU", StartENETProfileAsync<SimConnectProfiles.FENIX.A320.E.MCDU_R> },
            { "FBW A32NX Left MCDU", StartENETProfileAsync<SimConnectProfiles.FBW.A32NX.E.MCDU_L> },
            { "FBW A32NX Right MCDU", StartENETProfileAsync<SimConnectProfiles.FBW.A32NX.E.MCDU_R> },
            { "PMDG 737NG Left CDU", StartENETProfileAsync<SimConnectProfiles.PMDG.B737.E.NG_CDU_L> },
            { "PMDG 737NG Right CDU", StartENETProfileAsync<SimConnectProfiles.PMDG.B737.E.NG_CDU_R> },
            { "PMDG 737NG Left CDU (MAX)", StartENETProfileAsync<SimConnectProfiles.PMDG.B737.E.NG_CDU_MAX_L> },
            { "PMDG 737NG Right CDU (MAX)", StartENETProfileAsync<SimConnectProfiles.PMDG.B737.E.NG_CDU_MAX_R> },
            { "Asobo 747-8I Left CDU", StartENETProfileAsync<SimConnectProfiles.Asobo.B747.E.CDU_L>},
            { "Asobo 747-8I Right CDU", StartENETProfileAsync<SimConnectProfiles.Asobo.B747.E.CDU_R>},
            { "[P3D] PMDG 747 Left CDU", StartENETProfileAsync<SimConnectProfiles.PMDG.B747.E.B747_CDU_L> },
            { "[P3D] PMDG 747 Right CDU", StartENETProfileAsync<SimConnectProfiles.PMDG.B747.E.B747_CDU_R> },
            { "[P3D] PMDG 747 Center CDU", StartENETProfileAsync<SimConnectProfiles.PMDG.B747.E.B747_CDU_C> },
            { "[P3D] PMDG 777 Left CDU", StartENETProfileAsync<SimConnectProfiles.PMDG.B777.E.B777_CDU_L> },
            { "[P3D] PMDG 777 Right CDU", StartENETProfileAsync<SimConnectProfiles.PMDG.B777.E.B777_CDU_R> },
            { "[P3D] PMDG 777 Center CDU", StartENETProfileAsync<SimConnectProfiles.PMDG.B777.E.B777_CDU_C> },
            { "CDU/MCDU Test", StartTestAsync },

        //"PMDG 737MAX Left CDU"
        //"PMDG 737MAX Right CDU"

        };

        CreateProfiles();
        Connections = MainViewModel.LoadConnectionsData<ObservableCollection<Connection>>(enet);

        if (Properties.Settings.Default.AutoHide && Connections.Count > 0)
        {
            _ = StartENETAsync();
        }
    }

    [RelayCommand]
    private async Task StartENETAsync()
    {
        if (!(IsENETEnabled = !IsENETEnabled))
        {
            SaveENETConnections();
            foreach (var connection in Connections)
            {
                await StartENETProfilesAsync(connection);
            }
            return;
        }
        StopENET();
    }

    private void StopENET()
    {
        isActive = false;
        WeakReferenceMessenger.Default.Send(string.Empty);
        ENETList.ForEach(o => o.Close());
        ENETList.Clear();
        foreach (Connection connection in Connections)
        {
            connection.Status = 0;
        }
        IsENETEnabled = true;
    }

    private async Task StartENETProfileAsync<T>(Connection connection) where T : ENET, new()
    {
        T profile = new();
        ENETList.Add(profile);
        connection.Status = await Task.Run(() => profile.StartAsync(new(connection.IPAddress)));
    }

    private bool isActive;
    private async Task StartENETProfilesAsync(Connection connection)
    {
        if (connection.SelectedProfile is null)
        {
            System.Windows.MessageBox.Show("Please reselect the profile for: " + connection.Name);
            StopENET();
            return;
        }

        if (profileActions.TryGetValue(connection.SelectedProfile, out var action))
        {
            if (action is not null)
            {
                await action(connection);
            }
        }
    }

    private async Task StartTestAsync(Connection connection)
    {
        if (!isActive)
        {
            isActive = true;
            WeakReferenceMessenger.Default.Send(connection.IPAddress);
            await Task.Delay(10);
            connection.Status = await WeakReferenceMessenger.Default.Send<TestENETViewModel.StatusRequestMessage>();
        }
    }

    [RelayCommand]
    private void DeleteRow(Connection connection)
    {
        foreach (var conn in Connections)
        {
            if (conn == connection)
            {
                Connections.Remove(conn);
                return;
            }
        }
    }

    [RelayCommand]
    private void AddRow()
    {
        Connections.Add(new Connection() { Name = "NAME", IPAddress = "192.168.1.200", SelectedProfile = Profiles[0] });
    }

    [RelayCommand]
    private async Task SearchDevicesAsync()
    {
        InterfaceITEthernetDiscovery? discovery = await InterfaceITEthernet.ReceiveControllerDiscoveryDataAsync();
        if (discovery is null)
        {
            return;
        }
        foreach (var connection in Connections)
        {
            if (connection.IPAddress == discovery.Value.IPAddress)
            {
                return;
            }
        }
        Connections.Add(new Connection() { Name = discovery.Value.Description, IPAddress = discovery.Value.IPAddress, SelectedProfile = Profiles[0] });
    }

    private void CreateProfiles()
    {
        foreach (var item in profileActions.Keys)
        {
            Profiles.Add(item);
        }
    }

    [RelayCommand]
    private void ResetENETScreens()
    {
        ENETList.ForEach(o =>
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

    public void SaveENETConnections()
    {
        MainViewModel.SaveConnectionsData(enet, Connections);
    }
}