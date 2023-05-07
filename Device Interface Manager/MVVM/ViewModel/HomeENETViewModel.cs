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

    public HomeENETViewModel()
    {
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
        connection.Status = await Task.Run(() => profile.Start(connection.IPAddress));
        ENETList.Add(profile);
    }

    private bool isActive;
    private async Task StartENETProfiles(Connection connection)
    {
        switch (connection.Profile.Id)
        {
            case 1:
                await StartENETProfile<MSFSProfiles.FENIX.A320.MCDU_L_E>(connection);
                break;

            case 2:
                await StartENETProfile<MSFSProfiles.FENIX.A320.MCDU_R_E>(connection);
                break;

            case 3:
                await StartENETProfile<MSFSProfiles.FBW.A32NX.MCDU_L_E>(connection);
                break;

            case 4:
                await StartENETProfile<MSFSProfiles.FBW.A32NX.MCDU_R_E>(connection);
                break;

            case 5:
                await StartENETProfile<MSFSProfiles.PMDG.B737.NG_CDU_L_E>(connection);
                break;

            case 6:
                await StartENETProfile<MSFSProfiles.PMDG.B737.NG_CDU_R_E>(connection);
                break;

            case 7:
                await StartENETProfile<MSFSProfiles.PMDG.B737.NG_CDU_MAX_L_E>(connection);
                break;

            case 8:
                await StartENETProfile<MSFSProfiles.PMDG.B737.NG_CDU_MAX_R_E>(connection);
                break;

            case 9:
                if (!isActive)
                {
                    isActive = true;
                    WeakReferenceMessenger.Default.Send(connection.IPAddress);
                    await Task.Delay(10);
                    connection.Status = await WeakReferenceMessenger.Default.Send<TestENETViewModel.StatusRequestMessage>();
                }
                break;

            default:
                break;
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
        Connections.Add(new Connection() { Id = 0, Name = "NAME", IPAddress = "192.168.1.200", Profile = Profiles[0] });
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
        Connections.Add(new Connection() { Id = 0, Name = result[0], IPAddress = result[1], Profile = Profiles[0] });
    }

    private void CreateProfiles()
    {
        string[] profileNames = new string[]
       {
           "-- None --",
           "Fenix A320 Left MCDU",
           "Fenix A320 Right MCDU",
           "FBW A32NX Left MCDU",
           "FBW A32NX Right MCDU",
           "PMDG 737NG Left CDU",
           "PMDG 737NG Right CDU",
           "PMDG 737NG Left CDU (MAX)",
           "PMDG 737NG Right CDU (MAX)",
           "CDU/MCDU Test"
       };

        //"PMDG 737MAX Left CDU"
        //"PMDG 737MAX Right CDU

        //"PMDG 777 Left CDU"
        //"PMDG 777 Right CDU"
        //"PMDG 777 Center CDU"

        //"PMDG 747 Left CDU"
        //"PMDG 747 Right CDU"
        //"PMDG 747 Center CDU"

        for (int i = 0; i < profileNames.Length; i++)
        {
            Profiles.Add(new Profile { Id = i, Name = profileNames[i] });
        }
    }

    [RelayCommand]
    private void ResetENETScreens()
    {
        ENETList.ForEach(o => { o.pMDG737CDU.Top = 0; o.pMDG737CDU.Left = 0; o.fBWA32NXMCDU.Top = 0; o.fBWA32NXMCDU.Left = 0; });
    }

    public void SaveENETConnections()
    {
        MainViewModel.SaveConnectionsData(enet, Connections);
    }
}