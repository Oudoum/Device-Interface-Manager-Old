using System.Threading.Tasks;
using System.Threading;
using System.Collections.ObjectModel;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Device_Interface_Manager.MVVM.Model;
using Device_Interface_Manager.MVVM.View;
using Device_Interface_Manager.Profiles.FENIX.A320;
using static Device_Interface_Manager.MVVM.Model.HomeModel;
using static Device_Interface_Manager.MVVM.ViewModel.MainViewModel;
using Device_Interface_Manager.interfaceIT.USB.Controller;
using Device_Interface_Manager.interfaceIT.USB;
using Device_Interface_Manager.Profiles;

namespace Device_Interface_Manager.MVVM.ViewModel
{
    class HomeViewModel : ObservableObject
    {
        public static SimConnectClient SimConnectClient { get; set; }



        public Thread SimConnectMessageThread;

        public CancellationTokenSource SimConnectMessageCancellationTokenSource { get; set; }


        public RelayCommand DiscordCommand { get; set; }

        public RelayCommand IPAndPortCheckCommand { get; set; }

        public RelayCommand<string> ProfileStartStopCommand { get; set; }

        public RelayCommand InstallUpdateHubhopCommand { get; set; }



        public ObservableCollection<string> BoardInfo { get; set; } = new ObservableCollection<string>();

        public ObservableCollection<string> BoardType { get; set; } = new ObservableCollection<string>();

        public List<bool> SimConnectProfilesEnabled { get; set; } = new List<bool>();

        public List<bool> MobiFlightWASMProfilesEnabled { get; set; } = new List<bool>();

        private bool _areProfilesNotActive = true;
        public bool AreProfilesNotActive
        {
            get => this._areProfilesNotActive;
            set
            {
                this._areProfilesNotActive = value;
                OnPropertyChanged();
            }
        }


        private string iP;
        private string _iP;
        public string IP
        {
            get => this._iP;
            set
            {
                this._iP = value;
                OnPropertyChanged();
            }
        }

        private string port;
        private string _port;
        public string Port
        {
            get => this._port;
            set
            {
                this._port = value;
                OnPropertyChanged();
            }
        }


        private byte _mSFS_PMDG_B737_Captain_CDU_Enabled;
        public byte MSFS_PMDG_B737_Captain_CDU_Enabled
        {
            get => this._mSFS_PMDG_B737_Captain_CDU_Enabled;
            set
            {
                this._mSFS_PMDG_B737_Captain_CDU_Enabled = value;
                OnPropertyChanged();
            }
        }

        private string _mSFS_PMDG_B737_Captain_CDU_SN = Properties.Settings.Default.MSFS_PMDG_B737_Captain_CDU_SN;
        public string MSFS_PMDG_B737_Captain_CDU_SN
        {
            get => this._mSFS_PMDG_B737_Captain_CDU_SN;
            set
            {
                Properties.Settings.Default.MSFS_PMDG_B737_Captain_CDU_SN = this._mSFS_PMDG_B737_Captain_CDU_SN = GetAssignedProfiles(value);
                Properties.Settings.Default.Save();
                OnPropertyChanged();
            }
        }

        private byte _mSFS_PMDG_B737_Firstofficer_CDU_Enabled;
        public byte MSFS_PMDG_B737_Firstofficer_CDU_Enabled
        {
            get => this._mSFS_PMDG_B737_Firstofficer_CDU_Enabled;
            set
            {
                this._mSFS_PMDG_B737_Firstofficer_CDU_Enabled = value;
                OnPropertyChanged();
            }
        }

        private string _mSFS_PMDG_B737_Firstofficer_CDU_SN = Properties.Settings.Default.MSFS_PMDG_B737_Firstofficer_CDU_SN;
        public string MSFS_PMDG_B737_Firstofficer_CDU_SN
        {
            get => this._mSFS_PMDG_B737_Firstofficer_CDU_SN;
            set
            {
                Properties.Settings.Default.MSFS_PMDG_B737_Firstofficer_CDU_SN = this._mSFS_PMDG_B737_Firstofficer_CDU_SN = GetAssignedProfiles(value);
                Properties.Settings.Default.Save();
                OnPropertyChanged();
            }
        }

        private byte _mSFS_PMDG_B737_MCP_Enabled;
        public byte MSFS_PMDG_B737_MCP_Enabled
        {
            get => this._mSFS_PMDG_B737_MCP_Enabled;
            set
            {
                this._mSFS_PMDG_B737_MCP_Enabled = value;
                OnPropertyChanged();
            }
        }

        private string _mSFS_PMDG_B737_MCP_SN = Properties.Settings.Default.MSFS_PMDG_B737_MCP_SN;
        public string MSFS_PMDG_B737_MCP_SN
        {
            get => this._mSFS_PMDG_B737_MCP_SN;
            set
            {
                Properties.Settings.Default.MSFS_PMDG_B737_MCP_SN = this._mSFS_PMDG_B737_MCP_SN = GetAssignedProfiles(value);
                Properties.Settings.Default.Save();
                OnPropertyChanged();
            }
        }

        private byte _mSFS_FBW_A32NX_Captain_MCDU_Enabled;
        public byte MSFS_FBW_A32NX_Captain_MCDU_Enabled
        {
            get => this._mSFS_FBW_A32NX_Captain_MCDU_Enabled;
            set
            {
                this._mSFS_FBW_A32NX_Captain_MCDU_Enabled = value;
                OnPropertyChanged();
            }
        }

        private string _mSFS_FBW_A32NX_Captain_MCDU_SN = Properties.Settings.Default.MSFS_FBW_A32NX_Captain_MCDU_SN;
        public string MSFS_FBW_A32NX_Captain_MCDU_SN
        {
            get => this._mSFS_FBW_A32NX_Captain_MCDU_SN;
            set
            {
                Properties.Settings.Default.MSFS_FBW_A32NX_Captain_MCDU_SN = this._mSFS_FBW_A32NX_Captain_MCDU_SN = GetAssignedProfiles(value);
                Properties.Settings.Default.Save();
                OnPropertyChanged();
            }
        }

        private byte _mSFS_FBW_A32NX_Firstofficer_MCDU_Enabled;
        public byte MSFS_FBW_A32NX_Firstofficer_MCDU_Enabled
        {
            get => this._mSFS_FBW_A32NX_Firstofficer_MCDU_Enabled;
            set
            {
                this._mSFS_FBW_A32NX_Firstofficer_MCDU_Enabled = value;
                OnPropertyChanged();
            }
        }

        private string _mSFS_FBW_A32NX_Firstofficer_MCDU_SN = Properties.Settings.Default.MSFS_FBW_A32NX_Firstofficer_MCDU_SN;
        public string MSFS_FBW_A32NX_Firstofficer_MCDU_SN
        {
            get => this._mSFS_FBW_A32NX_Firstofficer_MCDU_SN;
            set
            {
                Properties.Settings.Default.MSFS_FBW_A32NX_Firstofficer_MCDU_SN = this._mSFS_FBW_A32NX_Firstofficer_MCDU_SN = GetAssignedProfiles(value);
                Properties.Settings.Default.Save();
                OnPropertyChanged();
            }
        }

        private byte _mSFS_FENIX_A320_Captain_MCDU_Enabled;
        public byte MSFS_FENIX_A320_Captain_MCDU_Enabled
        {
            get => this._mSFS_FENIX_A320_Captain_MCDU_Enabled;
            set
            {
                this._mSFS_FENIX_A320_Captain_MCDU_Enabled = value;
                OnPropertyChanged();
            }
        }

        private string _mSFS_FENIX_A320_Captain_MCDU_SN = Properties.Settings.Default.MSFS_FENIX_A320_Captain_MCDU_SN;
        public string MSFS_FENIX_A320_Captain_MCDU_SN
        {
            get => this._mSFS_FENIX_A320_Captain_MCDU_SN;
            set
            {
                Properties.Settings.Default.MSFS_FENIX_A320_Captain_MCDU_SN = this._mSFS_FENIX_A320_Captain_MCDU_SN = GetAssignedProfiles(value);
                Properties.Settings.Default.Save();
                OnPropertyChanged();
            }
        }

        private bool _isSimConnectOpen;
        public bool IsSimConnectOpen
        {
            get => this._isSimConnectOpen;
            set
            {
                this._isSimConnectOpen = value;
                OnPropertyChanged();
            }
        }

        private int _hubHopUpdateProgress;
        public int HubHopUpdateProgress
        {
            get => this._hubHopUpdateProgress;
            set
            {
                this._hubHopUpdateProgress = value;
                OnPropertyChanged();
            }
        }


        public HomeViewModel()
        {
            ReadSimConnectCfg();

            this.DiscordCommand = new RelayCommand(() =>
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("https://discord.gg/MRR9zUMQhM") { UseShellExecute = true });
            });

            this.IPAndPortCheckCommand = new RelayCommand(() =>
            {
                if (System.Net.IPAddress.TryParse(this.IP, out System.Net.IPAddress adress))
                {
                    File.WriteAllText("SimConnect.cfg", File.ReadAllText("SimConnect.cfg").Replace("Address=" + this.iP, "Address=" + (this.IP = this.iP = adress.ToString())));
                }
                else
                {
                    this.IP = this.port;
                }

                if (ushort.TryParse(this.Port, out ushort port) && this.Port != "0")
                {
                    File.WriteAllText("SimConnect.cfg", File.ReadAllText("SimConnect.cfg").Replace("Port=" + this.port, "Port=" + (this.Port = this.port = port.ToString())));
                }
                else
                {
                    this.Port = this.port;
                }
            });

            this.ProfileStartStopCommand = new RelayCommand<string>(o =>
            {
                switch (o)
                {
                    case "MSFS_PMDG_B737_Captain_CDU":
                        StartMSFS_PMDG_B737_Captain_CDU();
                        break;

                    case "MSFS_PMDG_B737_Firstofficer_CDU":
                        StartMSFS_PMDG_B737_Firstofficer_CDU();
                        break;

                    case "MSFS_PMDG_B737_MCP":
                        StartMSFS_PMDG_B737_MCP();
                        break;

                    case "MSFS_FBW_A32NX_Captain_CDU":
                        StartMSFS_FBW_A32NX_Captain_CDU();
                        break;

                    case "MSFS_FBW_A32NX_Firstofficer_CDU":
                        StartMSFS_FBW_A32NX_Firstofficer_CDU();
                        break;

                    case "MSFS_FENIX_A320_Captain_CDU":
                        StartMSFS_FENIX_A320_Captain_CDU();
                        break;

                    default:
                        break;
                }
                StopSimConnect();

            });

            this.InstallUpdateHubhopCommand = new RelayCommand(async () =>
            {
                MobiFlight.SimConnectMSFS.WasmModuleUpdater wasmModuleUpdater = new();
                wasmModuleUpdater.DownloadAndInstallProgress += WasmModuleUpdater_DownloadAndInstallProgress;
                wasmModuleUpdater.AutoDetectCommunityFolder();
                await Task.Run(() => wasmModuleUpdater.DownloadWasmEvents());
                await Task.Run(() => wasmModuleUpdater.InstallWasmModule());
                wasmModuleUpdater = null;
            });
        }

        private void WasmModuleUpdater_DownloadAndInstallProgress(object sender, MobiFlight.Base.ProgressUpdateEvent e)
        {
            this.HubHopUpdateProgress = e.Current;
        }

        private async void StartMSFS_PMDG_B737_Captain_CDU()
        {
            if (this.MSFS_PMDG_B737_Captain_CDU_Enabled == 0 && !string.IsNullOrEmpty(this.MSFS_PMDG_B737_Captain_CDU_SN))
            {
                this.SimConnectProfilesEnabled.Add(true);
                this.MSFS_PMDG_B737_Captain_CDU_Enabled = 1;
                await StartSimConnect();
                SimConnectClient.PMDG737CDU0 = new PMDG737CDU();
                Profile0_MSFS_PMDG_737_CDU = new Controller_331A_CDU_MCDU
                {
                    Session = GetProfileSession(Properties.Settings.Default.MSFS_PMDG_B737_Captain_CDU_SN),
                    KeyNotifyCallback = MSFS_PMDG_737_CDU_USB.MSFS_PMDG_737_Captain_CDU_Events.KeyNotifyCallback,
                };
                Profile0_MSFS_PMDG_737_CDU.Controller_331A_CDU_MCDUStart();
                this.MSFS_PMDG_B737_Captain_CDU_Enabled = 2;
            }
            else if (this.MSFS_PMDG_B737_Captain_CDU_Enabled != 0)
            {
                Profile0_MSFS_PMDG_737_CDU?.Controller_331A_CDU_MCDUStop();
                Profile0_MSFS_PMDG_737_CDU = null;
                SimConnectClient.PMDG737CDU0?.Close();
                SimConnectClient.PMDG737CDU0 = null;
                this.SimConnectProfilesEnabled.Remove(true);
                this.MSFS_PMDG_B737_Captain_CDU_Enabled = 0;
            }
        }

        private async void StartMSFS_PMDG_B737_Firstofficer_CDU()
        {
            if (this.MSFS_PMDG_B737_Firstofficer_CDU_Enabled == 0 && !string.IsNullOrEmpty(this.MSFS_PMDG_B737_Firstofficer_CDU_SN))
            {
                this.SimConnectProfilesEnabled.Add(true);
                this.MSFS_PMDG_B737_Firstofficer_CDU_Enabled = 1;
                await StartSimConnect();
                SimConnectClient.PMDG737CDU1 = new PMDG737CDU();
                Profile1_MSFS_PMDG_737_CDU = new Controller_331A_CDU_MCDU
                {
                    Session = GetProfileSession(Properties.Settings.Default.MSFS_PMDG_B737_Firstofficer_CDU_SN),
                    KeyNotifyCallback = MSFS_PMDG_737_CDU_USB.MSFS_PMDG_737_Firstofficer_CDU_Events.KeyNotifyCallback,
                };
                Profile1_MSFS_PMDG_737_CDU.Controller_331A_CDU_MCDUStart();
                this.MSFS_PMDG_B737_Firstofficer_CDU_Enabled = 2;
            }
            else if (this.MSFS_PMDG_B737_Firstofficer_CDU_Enabled != 0)
            {
                Profile1_MSFS_PMDG_737_CDU?.Controller_331A_CDU_MCDUStop();
                Profile1_MSFS_PMDG_737_CDU = null;
                SimConnectClient.PMDG737CDU1?.Close();
                SimConnectClient.PMDG737CDU1 = null;
                this.SimConnectProfilesEnabled.Remove(true);
                this.MSFS_PMDG_B737_Firstofficer_CDU_Enabled = 0;
            }
        }

        private async void StartMSFS_PMDG_B737_MCP()
        {
            if (this.MSFS_PMDG_B737_MCP_Enabled == 0 && !string.IsNullOrEmpty(this.MSFS_PMDG_B737_MCP_SN))
            {
                this.SimConnectProfilesEnabled.Add(true);
                this.MSFS_PMDG_B737_MCP_Enabled = 1;
                await StartSimConnect();
                MSFS_PMDG_737_MCP_USB.MSFSPMDG737MCPSession = GetProfileSession(Properties.Settings.Default.MSFS_PMDG_B737_MCP_SN);
                MSFS_PMDG_737_MCP_USB.Flaggone = true;
                this.MSFS_PMDG_B737_MCP_Enabled = 2;
            }
            else if (this.MSFS_PMDG_B737_MCP_Enabled != 0)
            {
                MSFS_PMDG_737_MCP_USB.MSFSPMDG737MCPClose();
                this.SimConnectProfilesEnabled.Remove(true);
                MSFS_PMDG_737_MCP_USB.Flaggone = false;
                SimConnectClient.PMDGMCPIsStarted = false;
                this.MSFS_PMDG_B737_MCP_Enabled = 0;
            }
        }

        private async void StartMSFS_FBW_A32NX_Captain_CDU()
        {
            CancellationTokenSource cancellationTokenSource = new();
            if (this.MSFS_FBW_A32NX_Captain_MCDU_Enabled == 0 && !string.IsNullOrEmpty(this.MSFS_FBW_A32NX_Captain_MCDU_SN))
            {
                this.MobiFlightWASMProfilesEnabled.Add(true);
                this.MSFS_FBW_A32NX_Captain_MCDU_Enabled = 1;
                SimConnectStart();
                await Task.Run(() => simConnectCache.IsSimConnectConnected() == true);
                Profile3_MSFS_FBW_A32NX_MCDU = new Controller_331A_CDU_MCDU
                {
                    Session = GetProfileSession(Properties.Settings.Default.MSFS_FBW_A32NX_Captain_MCDU_SN),
                    KeyNotifyCallback = MSFS_FBW_A32NX_MCDU_USB.MSFS_FBW_A32NX_Captain_MCDU_Events.KeyNotifyCallback,
                };
                Profile3_MSFS_FBW_A32NX_MCDU.Controller_331A_CDU_MCDUStart();
                MSFS_FBW_A32NX_MCDU_USB.MSFS_FBW_A32NX_Captain_MCDU_Data.ReceivedDataThread = new Thread(() => MSFS_FBW_A32NX_MCDU_USB.MSFS_FBW_A32NX_Captain_MCDU_Data.ReceiveDataThread(Profile3_MSFS_FBW_A32NX_MCDU.Session, cancellationTokenSource.Token));
                MSFS_FBW_A32NX_MCDU_USB.MSFS_FBW_A32NX_Captain_MCDU_Data.ReceivedDataThread.Start();
                //CreateWebpage();
                //FBW_A32NX_MCDU_Window?.Show();
                //FBW_A32NX_MCDU_Window.WindowState = WindowState.Maximized;
                this.MSFS_FBW_A32NX_Captain_MCDU_Enabled = 2;
            }
            else if (this.MSFS_FBW_A32NX_Captain_MCDU_Enabled != 0)
            {
                cancellationTokenSource?.Cancel();
                Profile3_MSFS_FBW_A32NX_MCDU?.Controller_331A_CDU_MCDUStop();
                SimConnectStop();
                Profile3_MSFS_FBW_A32NX_MCDU = null;
                FBW_A32NX_MCDU_Window = null;
                this.MobiFlightWASMProfilesEnabled.Remove(true);
                this.MSFS_FBW_A32NX_Captain_MCDU_Enabled = 0;
            }
        }

        private async void StartMSFS_FBW_A32NX_Firstofficer_CDU()
        {
            CancellationTokenSource cancellationTokenSource = new();
            if (this.MSFS_FBW_A32NX_Firstofficer_MCDU_Enabled == 0 && !string.IsNullOrEmpty(this.MSFS_FBW_A32NX_Firstofficer_MCDU_SN))
            {
                this.MobiFlightWASMProfilesEnabled.Add(true);
                this.MSFS_FBW_A32NX_Firstofficer_MCDU_Enabled = 1;
                SimConnectStart();
                await Task.Run(() => simConnectCache.IsSimConnectConnected() == true);
                Profile4_MSFS_FBW_A32NX_MCDU = new Controller_331A_CDU_MCDU
                {
                    Session = GetProfileSession(Properties.Settings.Default.MSFS_FBW_A32NX_Firstofficer_MCDU_SN),
                    KeyNotifyCallback = MSFS_FBW_A32NX_MCDU_USB.MSFS_FBW_A32NX_Firstofficer_MCDU_Events.KeyNotifyCallback,

                };
                Profile4_MSFS_FBW_A32NX_MCDU.Controller_331A_CDU_MCDUStart();
                MSFS_FBW_A32NX_MCDU_USB.MSFS_FBW_A32NX_Firstofficer_MCDU_Data.ReceivedDataThread = new Thread(() => MSFS_FBW_A32NX_MCDU_USB.MSFS_FBW_A32NX_Firstofficer_MCDU_Data.ReceiveDataThread(Profile4_MSFS_FBW_A32NX_MCDU.Session, cancellationTokenSource.Token));
                MSFS_FBW_A32NX_MCDU_USB.MSFS_FBW_A32NX_Firstofficer_MCDU_Data.ReceivedDataThread.Start();
                //CreateWebpage();
                //FBW_A32NX_MCDU_Window?.Show();
                //FBW_A32NX_MCDU_Window.WindowState = WindowState.Maximized;
                this.MSFS_FBW_A32NX_Firstofficer_MCDU_Enabled = 2;
            }
            else if (this.MSFS_FBW_A32NX_Firstofficer_MCDU_Enabled != 0)
            {
                cancellationTokenSource?.Cancel();
                Profile4_MSFS_FBW_A32NX_MCDU?.Controller_331A_CDU_MCDUStop();
                SimConnectStop();
                Profile4_MSFS_FBW_A32NX_MCDU = null;
                FBW_A32NX_MCDU_Window = null;
                this.MobiFlightWASMProfilesEnabled.Remove(true);
                this.MSFS_FBW_A32NX_Firstofficer_MCDU_Enabled = 0;
            }
        }

        private async void StartMSFS_FENIX_A320_Captain_CDU()
        {
            CancellationTokenSource cancellationTokenSource = new();
            if (this.MSFS_FENIX_A320_Captain_MCDU_Enabled == 0 && !string.IsNullOrEmpty(this.MSFS_FENIX_A320_Captain_MCDU_SN))
            {
                this.MobiFlightWASMProfilesEnabled.Add(true);
                this.MSFS_FENIX_A320_Captain_MCDU_Enabled = 1;
                SimConnectStart();
                await Task.Run(() => simConnectCache.IsSimConnectConnected() == true);
                Profile5_MSFS_FENIX_A320_MCDU = new Controller_331A_CDU_MCDU
                {
                    Session = GetProfileSession(Properties.Settings.Default.MSFS_FENIX_A320_Captain_MCDU_SN),
                    KeyNotifyCallback = MSFS_FENIX_A320_MCDU_USB.MSFS_FENIX_A320_Captain_Events.KeyNotifyCallback,
                };
                Profile5_MSFS_FENIX_A320_MCDU.Controller_331A_CDU_MCDUStart();
                MSFS_FENIX_A320_MCDU_USB.MSFS_FENIX_A320_Captain_MCDU_Data.ReceivedDataThread = new Thread(() => MSFS_FENIX_A320_MCDU_USB.MSFS_FENIX_A320_Captain_MCDU_Data.ReceiveDataThread(cancellationTokenSource.Token));
                MSFS_FENIX_A320_MCDU_USB.MSFS_FENIX_A320_Captain_MCDU_Data.ReceivedDataThread.Start();
                this.MSFS_FENIX_A320_Captain_MCDU_Enabled = 2;
            }
            else if (this.MSFS_FENIX_A320_Captain_MCDU_Enabled != 0)
            {
                cancellationTokenSource?.Cancel();
                Profile5_MSFS_FENIX_A320_MCDU?.Controller_331A_CDU_MCDUStop();
                SimConnectStop();
                Profile5_MSFS_FENIX_A320_MCDU = null;
                this.MobiFlightWASMProfilesEnabled.Remove(true);
                this.MSFS_FENIX_A320_Captain_MCDU_Enabled = 0;
            }
        }

        public void StopSimConnect()
        {
            if (!this.SimConnectProfilesEnabled.Any())
            {
                this.SimConnectMessageCancellationTokenSource?.Cancel();
                SimConnectClient?.SimConnect_Close();
                SimConnectClient = null;
            }
            if (!this.MobiFlightWASMProfilesEnabled.Any())
            {
                simConnectCache = null;
            }
            if (!this.SimConnectProfilesEnabled.Any() && !this.MobiFlightWASMProfilesEnabled.Any())
            {
                this.AreProfilesNotActive = true;
            }
            else
            {
                this.AreProfilesNotActive = false;
            }
        }

        public void GetBoardInfo()
        {
            if (DeviceList.Count == 0)
            {
                this.BoardInfo.Clear();
                this.BoardType.Clear();
                return;
            }

            this.BoardInfo.Clear();

            this.BoardInfo.Add("Board " + DeviceList[GetSeletedController()].DeviceInfo.szBoardType + " manufactured on " + DeviceList[GetSeletedController()].DeviceInfo.szManufactureDate + " has the following features: ");


            if (DeviceList[GetSeletedController()].DeviceInfo.dwFeatures == InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_NONE)
                this.BoardInfo.Add("No features programmed. Please obtain the update patch for this board.");
            else this.BoardInfo.Add(string.Empty);

            if ((DeviceList[GetSeletedController()].DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_OUTPUT_LED) != 0)
                this.BoardInfo.Add(DeviceList[GetSeletedController()].DeviceInfo.nLEDCount + " | LEDs ( " + DeviceList[GetSeletedController()].DeviceInfo.nLEDFirst + " - " + DeviceList[GetSeletedController()].DeviceInfo.nLEDLast + " )");
            else this.BoardInfo.Add(string.Empty);


            if ((DeviceList[GetSeletedController()].DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_INPUT_SWITCHES) != 0)
                this.BoardInfo.Add(DeviceList[GetSeletedController()].DeviceInfo.nSwitchCount + " | Switches ( " + DeviceList[GetSeletedController()].DeviceInfo.nSwitchFirst + " - " + DeviceList[GetSeletedController()].DeviceInfo.nSwitchLast + " )");
            else this.BoardInfo.Add(string.Empty);


            if ((DeviceList[GetSeletedController()].DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_OUTPUT_7SEGMENT) != 0)
                this.BoardInfo.Add(DeviceList[GetSeletedController()].DeviceInfo.n7SegmentCount + " | 7 Segments ( " + DeviceList[GetSeletedController()].DeviceInfo.n7SegmentFirst + " - " + DeviceList[GetSeletedController()].DeviceInfo.n7SegmentLast + " )");
            else this.BoardInfo.Add(string.Empty);


            if ((DeviceList[GetSeletedController()].DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_OUTPUT_DATALINE) != 0)
                this.BoardInfo.Add(DeviceList[GetSeletedController()].DeviceInfo.nDatalineCount + " | Datalines ( " + DeviceList[GetSeletedController()].DeviceInfo.nDatalineFirst + " - " + DeviceList[GetSeletedController()].DeviceInfo.nDatalineLast + " )");
            else this.BoardInfo.Add(string.Empty);


            //Not available
            if ((DeviceList[GetSeletedController()].DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_OUTPUT_SERVO) != 0)
                this.BoardInfo.Add(DeviceList[GetSeletedController()].DeviceInfo.nServoController + " | Servos ( " + DeviceList[GetSeletedController()].DeviceInfo.nServoControllerFirst + " - " + DeviceList[GetSeletedController()].DeviceInfo.nServoControllerLast + " )");
            else this.BoardInfo.Add(string.Empty);


            if ((DeviceList[GetSeletedController()].DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_SPECIAL_BRIGHTNESS) != 0)
                this.BoardInfo.Add("Brightness control supported");
            else this.BoardInfo.Add(string.Empty);


            if ((DeviceList[GetSeletedController()].DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_SPECIAL_ANALOG_INPUT) != 0)
                this.BoardInfo.Add("Analog input supported (Single Channel)");
            else this.BoardInfo.Add(string.Empty);


            if ((DeviceList[GetSeletedController()].DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_SPECIAL_ANALOG16_INPUT) != 0)
                this.BoardInfo.Add("Analog input supported (16 Channels)");
            else this.BoardInfo.Add(string.Empty);


            this.BoardType.Clear();

            foreach (var field in typeof(InterfaceIT_BoardIDs).GetFields())
            {
                if ((string)field.GetValue(null) == DeviceList[GetSeletedController()].DeviceInfo.szBoardType)
                    this.BoardType.Add(field.Name.ToString().Replace('_', ' '));
            }
        }

        private static int GetProfileSession(string profilestring)
        {
            foreach (var device in DeviceList)
            {
                if (device.SerialNumber == profilestring)
                {
                    return device.Session;
                }
            }
            return 0;
        }

        private string GetAssignedProfiles(string profile)
        {
            if (string.IsNullOrEmpty(profile))
            {
                return null;
            }
            else if (profile == this.MSFS_PMDG_B737_Captain_CDU_SN)
            {
                return null;
            }
            else if (profile == this.MSFS_PMDG_B737_Firstofficer_CDU_SN)
            {
                return null;
            }
            else if (profile == this.MSFS_PMDG_B737_MCP_SN)
            {
                return null;
            }
            else if (profile == this.MSFS_FBW_A32NX_Captain_MCDU_SN)
            {
                return null;
            }
            else if (profile == this.MSFS_FBW_A32NX_Firstofficer_MCDU_SN)
            {
                return null;
            }
            else if (profile == this.MSFS_FENIX_A320_Captain_MCDU_SN)
            {
                return null;
            }
            else
                return profile;
        }

        private void ReadSimConnectCfg()
        {
            if (File.Exists("SimConnect.cfg"))
            {
                foreach (var line in File.ReadLines("SimConnect.cfg"))
                {
                    if (line.StartsWith("Address="))
                    {
                        this.iP = this._iP = line.Replace("Address=", null);
                    }
                    if (line.StartsWith("Port="))
                    {
                        this.port = this._port = line.Replace("Port=", null);
                    }
                }
            }
        }

        public async Task StartSimConnect()
        {
            if (SimConnectClient is null)
            {
                this.SimConnectMessageCancellationTokenSource = new CancellationTokenSource();
                SimConnectClient = new SimConnectClient();
                await Task.Run(() => SimConnectClient.SimConnect_Open());
                this.SimConnectMessageThread = new Thread(() => SimConnectClient.ReceiveSimConnectMessage(this.SimConnectMessageCancellationTokenSource.Token))
                {
                    Name = "SimConnectClient"
                };
                SimConnectMessageThread.Name = SimConnectMessageThread.ToString();
                SimConnectMessageThread.Start();
            }
        }
    }
}