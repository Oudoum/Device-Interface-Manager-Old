using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Device_Interface_Manager.interfaceIT.ENET;
using Device_Interface_Manager.MSFSProfiles;
using static Device_Interface_Manager.MVVM.Model.HomeENETModel;

namespace Device_Interface_Manager.MVVM.ViewModel;

public partial class HomeENETViewModel : ObservableObject
{
    private const string enet = @"Profiles\ENET.json";

    [ObservableProperty]
    private bool _isENETEnabled = true;

    public ObservableCollection<Connection> Connections { get; set; } = new();

    public ObservableCollection<Profile> Profiles { get; set; } = new();

    public InterfaceITEthernet interfaceITEthernet;

    private List<ENET> ENETList { get; set; } = new();

    private readonly Dictionary<string, Func<Connection, Task>> profileActions;

    public HomeENETViewModel()
    {
        profileActions = new()
        {
            { "-- None --", null },
            { "Fenix A320 Left MCDU", StartENETProfile<MSFSProfiles.FENIX.A320.E.MCDU_L> },
            { "Fenix A320 Right MCDU", StartENETProfile<MSFSProfiles.FENIX.A320.E.MCDU_R> },
            { "FBW A32NX Left MCDU", StartENETProfile<MSFSProfiles.FBW.A32NX.E.MCDU_L> },
            { "FBW A32NX Right MCDU", StartENETProfile<MSFSProfiles.FBW.A32NX.E.MCDU_R> },
            { "PMDG 737NG Left CDU", StartENETProfile<MSFSProfiles.PMDG.B737.E.NG_CDU_L> },
            { "PMDG 737NG Right CDU", StartENETProfile<MSFSProfiles.PMDG.B737.E.NG_CDU_R> },
            { "PMDG 737NG Left CDU (MAX)", StartENETProfile<MSFSProfiles.PMDG.B737.E.NG_CDU_MAX_L> },
            { "PMDG 737NG Right CDU (MAX)", StartENETProfile<MSFSProfiles.PMDG.B737.E.NG_CDU_MAX_R> },
            { "CDU/MCDU Test", StartTest },

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
        Connections = MainViewModel.LoadConnectionsData<ObservableCollection<Connection>>(enet);

        if (Properties.Settings.Default.AutoHide && Connections.Count > 0)
        {
            _ = StartENET();
        }
    }

    [RelayCommand]
    private async Task StartENET()
    {
        if (!(IsENETEnabled = !IsENETEnabled))
        {
            SaveENETConnections();
            foreach (var connection in Connections)
            {
                await StartENETProfiles(connection);
            }
            return;
        }
        StopENET();
    }

    private void StopENET()
    {
        isActive = false;
        WeakReferenceMessenger.Default.Send(string.Empty);
        ENETList.ForEach(o => o.Stop());
        ENETList.Clear();
        foreach (Connection connection in Connections)
        {
            connection.Status = 0;
        }
    }

    private async Task StartENETProfile<T>(Connection connection) where T : ENET, new()
    {
        T profile = new();
        ENETList.Add(profile);
        connection.Status = await Task.Run(() => profile.Start(connection.IPAddress));
    }

    private bool isActive;
    private async Task StartENETProfiles(Connection connection)
    {
        if (profileActions.TryGetValue(connection.Profile.Name, out var action))
        {
            if (action is not null)
            {
                await action(connection);
            }
        }
    }

    private async Task StartTest(Connection connection)
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
        Connections.Add(new Connection() { Name = "NAME", IPAddress = "192.168.1.200", Profile = Profiles[0] });
    }

    [RelayCommand]
    private async Task SearchDevices()
    {
        string[] result = await InterfaceITEthernet.ReceiveControllerDiscoveryData();
        if (result is null)
        {
            return;
        }
        foreach (var connection in Connections)
        {
            if (connection.IPAddress == result[1])
            {
                return;
            }
        }
        Connections.Add(new Connection() { Name = result[0], IPAddress = result[1], Profile = Profiles[0] });
    }

    private void CreateProfiles()
    {
        foreach (var item in profileActions.Keys)
        {
            Profiles.Add(new Profile { Name = item });
        }
    }

    [RelayCommand]
    private void ResetENETScreens()
    {
        ENETList.ForEach(o =>
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

    public void SaveENETConnections()
    {
        MainViewModel.SaveConnectionsData(enet, Connections);
    }
}