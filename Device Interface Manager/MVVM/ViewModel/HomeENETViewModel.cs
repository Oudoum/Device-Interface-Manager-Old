using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Device_Interface_Manager.interfaceIT.ENET;
using Device_Interface_Manager.MSFSProfiles.PMDG;
using Device_Interface_Manager.MSFSProfiles.WASM;
using static Device_Interface_Manager.MVVM.Model.HomeENETModel;

namespace Device_Interface_Manager.MVVM.ViewModel
{
    partial class HomeENETViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isENETEnabled = true;

        public ObservableCollection<Connection> Connections { get; set; } = new();

        public ObservableCollection<Profile> Profiles { get; set; } = new();

        private CancellationTokenSource EthernetCancellationTokenSource { get; set; }

        private List<ENETPMDG> ListPMDG { get; set; } = new();
        private List<ENETWASM> ListWASM { get; set; } = new();

        public HomeENETViewModel()
        {
            CreateProfiles();
            LoadENETData();

            if (Properties.Settings.Default.AutoHide && Connections.Count > 0)
            {
                StartENET();
            }
        }

        [RelayCommand]
        private async void StartENET()
        {
            this.IsENETEnabled = !this.IsENETEnabled;

            if (!this.IsENETEnabled)
            {
                foreach (var connection in this.Connections) 
                {
                    switch (connection.Profile.Id)
                    {
                        case 1:
                            MSFSProfiles.WASM.FENIX.A320.MCDU_L_E mCDU_L_E = new();
                            await Task.Run(() => mCDU_L_E.Start(connection.IPAddress));
                            connection.Status = mCDU_L_E.ConnectionStatus;
                            this.ListWASM.Add(mCDU_L_E);
                            break;

                        case 2:
                            MSFSProfiles.WASM.FENIX.A320.MCDU_R_E mCDU_R_E = new();
                            await Task.Run(() => mCDU_R_E.Start(connection.IPAddress));
                            connection.Status = mCDU_R_E.ConnectionStatus;
                            this.ListWASM.Add(mCDU_R_E);
                            break;

                        case 3:
                            MSFSProfiles.WASM.FBW.A32NX.MCDU_L_E mCDU_L_E1 = new();
                            await Task.Run(() => mCDU_L_E1.Start(connection.IPAddress));
                            connection.Status = mCDU_L_E1.ConnectionStatus;
                            this.ListWASM.Add(mCDU_L_E1);
                            break;

                        case 4:
                            MSFSProfiles.WASM.FBW.A32NX.MCDU_R_E mCDU_R_E1 = new();
                            await Task.Run(() => mCDU_R_E1.Start(connection.IPAddress));
                            connection.Status = mCDU_R_E1.ConnectionStatus;
                            this.ListWASM.Add(mCDU_R_E1);
                            break;

                        case 5:
                            MSFSProfiles.PMDG.B737.NG_CDU_L_E nG_CDU_L_E = new();
                            await Task.Run(() => nG_CDU_L_E.Start(connection.IPAddress));
                            connection.Status = nG_CDU_L_E.ConnectionStatus;
                            this.ListPMDG.Add(nG_CDU_L_E);
                            break;

                        case 6:
                            MSFSProfiles.PMDG.B737.NG_CDU_R_E nG_CDU_R_E = new();
                            await Task.Run(() => nG_CDU_R_E.Start(connection.IPAddress));
                            connection.Status = nG_CDU_R_E.ConnectionStatus;
                            this.ListPMDG.Add(nG_CDU_R_E);
                            break;

                        default:
                            break;
                    }
                }

                if (this.Connections.Any(o => o.Profile.Id == 99))
                {
                    InterfaceITEthernet interfaceITEthernet = new();
                    int index = this.Connections.IndexOf(this.Connections.First(o => o.Profile.Id == 99));
                    interfaceITEthernet.Hostname = this.Connections[index].IPAddress;
                    await Task.Run(() => interfaceITEthernet.InterfaceITEthernetConnection(this.EthernetCancellationTokenSource.Token));
                    this.Connections[index].Status = interfaceITEthernet.ClientStatus;
                    if (this.Connections[index].Status == 2)
                    {
                        interfaceITEthernet.GetinterfaceITEthernetDataStart();
                        interfaceITEthernet.GetinterfaceITEthernetInfo();
                        MainViewModel.BoardinfoENETVM.InterfaceITEthernetInfoTextCollection.Add(interfaceITEthernet.InterfaceITEthernetInfoText);
                        MainViewModel.BoardinfoENETVM.InterfaceITEthernetInfoIPCollection.Add(interfaceITEthernet.InterfaceITEthernetInfo.CLIENT);
                        this.DataThread = new(() => interfaceITEthernet.GetinterfaceITEthernetData(INTERFACEIT_ETHERNET_KEY_NOTIFY_PROC = new(KeyPressedProcEthernet), this.EthernetCancellationTokenSource.Token))
                        {
                            Name = "TestDataThread"
                        };
                        this.DataThread.Start();
                    }
                }
            }

            if (this.IsENETEnabled)
            {
                this.ListWASM.ForEach(o => o.Stop());
                this.ListPMDG.ForEach(o => o.Stop());

                this.ListWASM.Clear();
                this.ListPMDG.Clear();

                foreach (var status in this.Connections) 
                {
                    status.Status = 0;
                }
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

        // CDU/MCDU TEST START
        private InterfaceITEthernet.INTERFACEIT_ETHERNET_KEY_NOTIFY_PROC INTERFACEIT_ETHERNET_KEY_NOTIFY_PROC { get; set; }
        private Thread DataThread { get; set; }
        private void KeyPressedProcEthernet(int Switch, string Direction)
        {

        }
        // CDU/MCDU TEST END

        private void CreateProfiles()
        {
            Profiles.Add(new Profile { Id = 0, Name = "-- None --" });

            Profiles.Add(new Profile { Id = 1, Name = "Fenix A320 Left MCDU" });
            Profiles.Add(new Profile { Id = 2, Name = "Fenix A320 Right MCDU" });

            Profiles.Add(new Profile { Id = 3, Name = "FBW A32NX Left MCDU" });
            Profiles.Add(new Profile { Id = 4, Name = "FBW A32NX Right MCDU" });

            Profiles.Add(new Profile { Id = 5, Name = "PMDG 737NG Left CDU" });
            Profiles.Add(new Profile { Id = 6, Name = "PMDG 737NG Right CDU" });

            //Profiles.Add(new Profile { Id = 7, Name = "PMDG 737MAX Left CDU" });
            //Profiles.Add(new Profile { Id = 8, Name = "PMDG 737MAX Right CDU" });

            //Profiles.Add(new Profile { Id = 9, Name = "PMDG 777 Left CDU" });
            //Profiles.Add(new Profile { Id = 10, Name = "PMDG 777 Right CDU" });
            //Profiles.Add(new Profile { Id = 11, Name = "PMDG 777 Center CDU" });

            //Profiles.Add(new Profile { Id = 12, Name = "PMDG 747 Left CDU" });
            //Profiles.Add(new Profile { Id = 13, Name = "PMDG 747 Right CDU" });
            //Profiles.Add(new Profile { Id = 14, Name = "PMDG 747 Center CDU" });

            Profiles.Add(new Profile { Id = 99, Name = "CDU/MCDU Test" });
        }

        private const string enet = @"Profiles\ENET.json";
        private void LoadENETData()
        {
            if (File.Exists(enet))
            {
                this.Connections = JsonConvert.DeserializeObject<ObservableCollection<Connection>>(File.ReadAllText(enet));
            }
        }

        public void SaveENETData()
        {
            if (this.Connections.Count == 0)
            {
                if (Directory.Exists((enet.Remove(8))))
                {
                    File.Delete(enet);
                }
                return;
            }
            foreach (var status in this.Connections)
            {
                status.Status = 0;
            }
            Directory.CreateDirectory(enet.Remove(8));
            string json = JsonConvert.SerializeObject(this.Connections, Formatting.Indented);
            if (File.Exists(enet))
            {
                if (File.ReadAllText(enet) != json)
                {
                    File.WriteAllText(enet, json);
                }
                return;
            }
            File.WriteAllText(enet, json);
        }
    }
}