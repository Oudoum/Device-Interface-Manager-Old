using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Device_Interface_Manager.interfaceIT.ENET;
using Device_Interface_Manager.MSFSProfiles.PMDG;
using Device_Interface_Manager.MSFSProfiles.WASM;
using static Device_Interface_Manager.MVVM.Model.HomeENETModel;

namespace Device_Interface_Manager.MVVM.ViewModel
{
    public partial class HomeENETViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isENETEnabled = true;

        public ObservableCollection<Connection> Connections { get; set; } = new();

        public ObservableCollection<Profile> Profiles { get; set; } = new();

        private InterfaceITEthernet interfaceITEthernet;
        private InterfaceITEthernet.INTERFACEIT_ETHERNET_KEY_NOTIFY_PROC pROC;
        private CancellationTokenSource ethernetCancellationTokenSource;

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
            if (!(this.IsENETEnabled = !this.IsENETEnabled))
            {
                await StartENETProfiles();
                this.ListPMDG.ForEach(o => WeakReferenceMessenger.Default.Send(new ValueChangedMessage<InterfaceITEthernet>(o.InterfaceITEthernet)));
                this.ListWASM.ForEach(o => WeakReferenceMessenger.Default.Send(new ValueChangedMessage<InterfaceITEthernet>(o.InterfaceITEthernet)));
                return;
            }
            this.interfaceITEthernet?.CloseStream();
            this.ethernetCancellationTokenSource?.Cancel();
            this.ListWASM.ForEach(o => o.Stop());
            this.ListPMDG.ForEach(o => o.Stop());
            this.ListWASM.Clear();
            this.ListPMDG.Clear();
            this.SaveENETData();
        }

        private async Task StartENETProfiles()
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

                    case 99:
                         this.interfaceITEthernet = new() { Hostname = connection.IPAddress };
                        await Task.Run(() => this.interfaceITEthernet.InterfaceITEthernetConnection((this.ethernetCancellationTokenSource = new()).Token));
                        if ((connection.Status = this.interfaceITEthernet.ClientStatus) == 2)
                        {
                            this.interfaceITEthernet.GetinterfaceITEthernetDataStart();
                            this.interfaceITEthernet.GetinterfaceITEthernetInfo();
                            this.GetinterfaceITEthernetData();
                            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<InterfaceITEthernet>(this.interfaceITEthernet));
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        private void GetinterfaceITEthernetData()
        {
            Task.Run(() => this.interfaceITEthernet.GetinterfaceITEthernetData(this.pROC = new(this.KeyPressedProcEthernet), this.ethernetCancellationTokenSource.Token));
        }

        private void KeyPressedProcEthernet(int key, string direction) { }

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

        [RelayCommand]
        private void ResetENETScreens()
        {
            this.ListPMDG.ForEach(o => { o.pMDG737CDU.Top = 0; o.pMDG737CDU.Left = 0; });
            this.ListWASM.ForEach(o => { o.fBWA32NXMCDU.Top = 0; o.fBWA32NXMCDU.Left = 0; });
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