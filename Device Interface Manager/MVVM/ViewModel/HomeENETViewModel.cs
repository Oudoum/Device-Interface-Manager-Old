using Device_Interface_Manager.Profiles.PMDG.B737;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Device_Interface_Manager.interfaceIT.ENET;
using static Device_Interface_Manager.MVVM.Model.HomeENETModel;
using static Device_Interface_Manager.MVVM.Model.HomeModel;

namespace Device_Interface_Manager.MVVM.ViewModel
{
    class HomeENETViewModel : ObservableObject
    {

        private bool isENETEnabled = true;
        public bool IsENETEnabled
        {
            get => this.isENETEnabled;
            set
            {
                if (this.isENETEnabled != value)
                {
                    this.isENETEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public RelayCommand StartENET { get; set; }

        public RelayCommand<Connection> DeleteRow { get; set; }

        public RelayCommand AddRow { get; set; }

        public ObservableCollection<Connection> Connections { get; set; } = new();

        public ObservableCollection<Profile> Profiles { get; set; } = new();

        private CancellationTokenSource EthernetCancellationTokenSource { get; set; }


        public HomeENETViewModel()
        {
            this.StartENET = new RelayCommand(async () =>
            {
                this.IsENETEnabled = !this.IsENETEnabled;

                if (!this.IsENETEnabled)
                {
                    if (this.Connections.Any(o => o.Profile.Id == 1))
                    {
                        InterfaceITEthernet interfaceITEthernet = new();
                        int index = this.Connections.IndexOf(this.Connections.First(o => o.Profile.Id == 1));
                        await Task.Run(() => StartiterfaceITEthernetConnection(index, interfaceITEthernet));
                        if (this.Connections[index].Status == 2)
                        {
                            await Task.Run(() => SimConnectStart());
                            MainViewModel.HomeVM.MobiFlightWASMProfilesEnabled.Add(true);
                            await Task.Run(() => simConnectCache.IsSimConnectConnected() == true);
                            this.EthernetCancellationTokenSource = new();
                            Device_Interface_Manager.Profiles.FENIX.A320.MSFS_FENIX_A320_MCDU_E.MSFS_FENIX_A320_Captain_MCDU_Data.ReceivedDataThread = new Thread(() => Device_Interface_Manager.Profiles.FENIX.A320.MSFS_FENIX_A320_MCDU_E.MSFS_FENIX_A320_Captain_MCDU_Data.ReceiveDataThread(this.EthernetCancellationTokenSource.Token))
                            {
                                Name = "MSFS_FENIX_A320_MCDU_E.MSFS_FENIX_A320_Captain_MCDU_Data"
                            };
                            Device_Interface_Manager.Profiles.FENIX.A320.MSFS_FENIX_A320_MCDU_E.MSFS_FENIX_A320_Captain_MCDU_Data.ReceivedDataThread.Start();
                            Device_Interface_Manager.Profiles.FENIX.A320.MSFS_FENIX_A320_MCDU_E.MSFS_FENIX_A320_Captain_Events.ReceivedDataThread = new Thread(() => interfaceITEthernet.GetinterfaceITEthernetData(Device_Interface_Manager.Profiles.FENIX.A320.MSFS_FENIX_A320_MCDU_E.MSFS_FENIX_A320_Captain_Events.EthernetKeyNotifyCallback, this.EthernetCancellationTokenSource.Token))
                            {
                                Name = "MSFS_FENIX_A320_MCDU_E.MSFS_FENIX_A320_Captain_MCDU_Events"
                            };
                            Device_Interface_Manager.Profiles.FENIX.A320.MSFS_FENIX_A320_MCDU_E.MSFS_FENIX_A320_Captain_Events.ReceivedDataThread.Start();
                        }
                    }

                    if (this.Connections.Any(o => o.Profile.Id == 5))
                    {
                        MSFS_PMDG_737_CDU_E.InterfaceITEthernet = new();
                        int index = this.Connections.IndexOf(this.Connections.First(o => o.Profile.Id == 5));
                        await Task.Run(() => StartiterfaceITEthernetConnection(index, MSFS_PMDG_737_CDU_E.InterfaceITEthernet));
                        if (this.Connections[index].Status == 2)
                        {
                            StartinterfaceITEthernet(MSFS_PMDG_737_CDU_E.InterfaceITEthernet);
                            MainViewModel.HomeVM.SimConnectProfilesEnabled.Add(true);
                            await MainViewModel.HomeVM.StartSimConnect();
                            this.EthernetCancellationTokenSource = new();
                            MSFS_PMDG_737_CDU_E.MSFS_PMDG_737_Captain_Events.ReceivedDataThread = new Thread(() => MSFS_PMDG_737_CDU_E.InterfaceITEthernet.GetinterfaceITEthernetData(MSFS_PMDG_737_CDU_E.MSFS_PMDG_737_Captain_Events.EthernetKeyNotifyCallback, this.EthernetCancellationTokenSource.Token))
                            {
                                Name = "MSFS_PMDG_737_CDU_E.MSFS_PMDG_737_Captain_Events"
                            };
                            MSFS_PMDG_737_CDU_E.MSFS_PMDG_737_Captain_Events.ReceivedDataThread.Start();
                        }

                    }
                    if (this.Connections.Any(o => o.Profile.Id == 99))
                    {
                        InterfaceITEthernet interfaceITEthernet = new();
                        int index = this.Connections.IndexOf(this.Connections.First(o => o.Profile.Id == 99));
                        await Task.Run(() => StartiterfaceITEthernetConnection(index, interfaceITEthernet));
                        if (this.Connections[index].Status == 2)
                        {
                            StartinterfaceITEthernet(interfaceITEthernet);
                            this.EthernetCancellationTokenSource = new();
                            DataThread = new(() => interfaceITEthernet.GetinterfaceITEthernetData(INTERFACEIT_ETHERNET_KEY_NOTIFY_PROC = new(KeyPressedProcEthernet), this.EthernetCancellationTokenSource.Token))
                            {
                                Name = "TestDataThread"
                            };
                            DataThread.Start();
                        }
                    }
                }
                if (this.IsENETEnabled)
                {
                    MainViewModel.BoardinfoENETVM.InterfaceITEthernetInfoTextCollection.Clear();
                    MainViewModel.BoardinfoENETVM.InterfaceITEthernetInfoIPCollection.Clear();
                    MainViewModel.BoardinfoENETVM.InterfaceITEthernetInfoText.Clear();
                    this.EthernetCancellationTokenSource?.Cancel();
                    SimConnectStop();
                    simConnectCache = null;
                    MainViewModel.HomeVM.StopSimConnect();
                    foreach (var status in this.Connections)
                    {
                        if (status.Status == 2)
                        {
                            switch (status.Id)
                            {
                                case 1:
                                    MainViewModel.HomeVM.SimConnectProfilesEnabled.Remove(true);
                                    break;

                                case 2:
                                    MainViewModel.HomeVM.SimConnectProfilesEnabled.Remove(true);
                                    break;

                                case 3:
                                    MainViewModel.HomeVM.SimConnectProfilesEnabled.Remove(true);
                                    break;

                                case 4:
                                    MainViewModel.HomeVM.SimConnectProfilesEnabled.Remove(true);
                                    break;

                                default:
                                    break;
                            }

                            switch (status.Id)
                            {
                                case 5:
                                    MainViewModel.HomeVM.SimConnectProfilesEnabled.Remove(true);
                                    break;

                                case 6:
                                    MainViewModel.HomeVM.SimConnectProfilesEnabled.Remove(true);
                                    break;

                                case 7:
                                    MainViewModel.HomeVM.SimConnectProfilesEnabled.Remove(true);
                                    break;

                                case 8:
                                    MainViewModel.HomeVM.SimConnectProfilesEnabled.Remove(true);
                                    break;

                                case 9:
                                    MainViewModel.HomeVM.SimConnectProfilesEnabled.Remove(true);
                                    break;

                                case 10:
                                    MainViewModel.HomeVM.SimConnectProfilesEnabled.Remove(true);
                                    break;

                                case 11:
                                    MainViewModel.HomeVM.SimConnectProfilesEnabled.Remove(true);
                                    break;

                                case 12:
                                    MainViewModel.HomeVM.SimConnectProfilesEnabled.Remove(true);
                                    break;

                                case 13:
                                    MainViewModel.HomeVM.SimConnectProfilesEnabled.Remove(true);
                                    break;

                                case 14:
                                    MainViewModel.HomeVM.SimConnectProfilesEnabled.Remove(true);
                                    break;

                                default:
                                    break;
                            }
                        }
                        status.Status = 0;
                    }
                }
            });

            this.DeleteRow = new RelayCommand<Connection>(o =>
            {
                foreach (var conn in Connections)
                {
                    if (conn == o)
                    {
                        Connections.Remove(conn);
                        return;
                    }
                }
            });

            this.AddRow = new RelayCommand(() => Connections.Add(new Connection() { Id = 0, Name = "NAME", IPAddress = "192.168.1.200", Profile = Profiles[0] }));

            CreateProfiles();

            LoadENETData();
        }

        // CDU/MCDU TEST START
        private InterfaceITEthernet.INTERFACEIT_ETHERNET_KEY_NOTIFY_PROC INTERFACEIT_ETHERNET_KEY_NOTIFY_PROC { get; set; }
        private Thread DataThread { get; set; }
        private void KeyPressedProcEthernet(int Switch, string Direction)
        {

        }
        // CDU/MCDU TEST END

        private void StartiterfaceITEthernetConnection(int i, InterfaceITEthernet interfaceITEthernet)
        {
            interfaceITEthernet.Hostname = this.Connections[i].IPAddress;
            interfaceITEthernet.InterfaceITEthernetConnection();
            this.Connections[i].Status = interfaceITEthernet.ClientStatus;
        }

        private void StartinterfaceITEthernet(InterfaceITEthernet interfaceITEthernet)
        {
            interfaceITEthernet.GetinterfaceITEthernetDataStart();
            interfaceITEthernet.GetinterfaceITEthernetInfo();
            MainViewModel.BoardinfoENETVM.InterfaceITEthernetInfoTextCollection.Add(interfaceITEthernet.InterfaceITEthernetInfoText);
            MainViewModel.BoardinfoENETVM.InterfaceITEthernetInfoIPCollection.Add(interfaceITEthernet.InterfaceITEthernetInfo.CLIENT);
        }

        private void CreateProfiles()
        {
            Profiles.Add(new Profile { Id = 0, Name = "-- None --" });

            Profiles.Add(new Profile { Id = 1, Name = "Fenix A320 Captain MCDU" });
            //Profiles.Add(new Profile { Id = 2, Name = "Fenix A320 Firstofficer MCDU" });

            //Profiles.Add(new Profile { Id = 3, Name = "FBW A32NX Captain MCDU" });
            //Profiles.Add(new Profile { Id = 4, Name = "FBW A32NX Firstofficer MCDU" });

            Profiles.Add(new Profile { Id = 5, Name = "PMDG 737NG Captain CDU" });
            //Profiles.Add(new Profile { Id = 6, Name = "PMDG 737NG Firstofficer CDU" });

            //Profiles.Add(new Profile { Id = 7, Name = "PMDG 737MAX Captain CDU" });
            //Profiles.Add(new Profile { Id = 8, Name = "PMDG 737MAX Firstofficer CDU" });

            //Profiles.Add(new Profile { Id = 9, Name = "PMDG 777 Captain CDU" });
            //Profiles.Add(new Profile { Id = 10, Name = "PMDG 777 Firstofficer CDU" });
            //Profiles.Add(new Profile { Id = 11, Name = "PMDG 777 Center CDU" });

            //Profiles.Add(new Profile { Id = 12, Name = "PMDG 747 Captain CDU" });
            //Profiles.Add(new Profile { Id = 13, Name = "PMDG 747 Firstofficer CDU" });
            //Profiles.Add(new Profile { Id = 14, Name = "PMDG 747 Center CDU" });

            Profiles.Add(new Profile { Id = 99, Name = "CDU/MCDU Test" });
        }

        private void LoadENETData()
        {
            if (File.Exists("enet.json"))
            {
                this.Connections = JsonConvert.DeserializeObject<ObservableCollection<Connection>>(File.ReadAllText("enet.json"));
            }
        }

        public void SaveENETData()
        {
            foreach (var status in this.Connections)
            {
                status.Status = 0;
            }
            File.WriteAllText("enet.json", JsonConvert.SerializeObject(this.Connections, Formatting.Indented));
        }
    }
}