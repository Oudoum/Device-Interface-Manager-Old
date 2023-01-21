using System.IO;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceITAPI_Data;

namespace Device_Interface_Manager.MVVM.ViewModel
{
    partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _hubHopUpdateProgress;

        private string iP;
        [ObservableProperty]
        private string _iP;

        private string port;
        [ObservableProperty]
        private string _port;

        [ObservableProperty]
        private bool _minimizedHide = Properties.Settings.Default.MinimizedHide;
        partial void OnMinimizedHideChanged(bool value)
        {
            Properties.Settings.Default.MinimizedHide = value;
            Properties.Settings.Default.Save();
        }

        [ObservableProperty]
        private bool _autoHide = Properties.Settings.Default.AutoHide;
        partial void OnAutoHideChanged(bool value)
        {
            Properties.Settings.Default.AutoHide = value;
            Properties.Settings.Default.Save();
        }

        public string InterfaceITAPIVersion { get; set; }

        public SettingsViewModel() 
        {
            this.GetInterfaceITAPIVersion();
            this.ReadSimConnectCfg();
        }

        [RelayCommand]
        private void Discord()
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("https://discord.gg/MRR9zUMQhM") { UseShellExecute = true });
        }

        [RelayCommand]
        private async void InstallUpdateHubhop()
        {
            MobiFlight.SimConnectMSFS.WasmModuleUpdater wasmModuleUpdater = new();
            wasmModuleUpdater.DownloadAndInstallProgress += WasmModuleUpdater_DownloadAndInstallProgress;
            wasmModuleUpdater.AutoDetectCommunityFolder();
            await Task.Run(() => wasmModuleUpdater.DownloadWasmEvents());
            await Task.Run(() => wasmModuleUpdater.InstallWasmModule());
            wasmModuleUpdater = null;
        }

        [RelayCommand]
        private void IPAndPortCheck()
        {
            if (!System.Net.IPAddress.TryParse(this.IP, out System.Net.IPAddress adress) || !ushort.TryParse(this.Port, out ushort port) || this.Port == "0")
            {
                this.IP = this.iP;
                this.Port = this.port;
                return;
            }
            File.WriteAllText("SimConnect.cfg", File.ReadAllText("SimConnect.cfg").Replace("Address=" + this.iP, "Address=" + (this.IP = this.iP = adress.ToString())));
            File.WriteAllText("SimConnect.cfg", File.ReadAllText("SimConnect.cfg").Replace("Port=" + this.port, "Port=" + (this.Port = this.port = port.ToString())));
        }

        private void WasmModuleUpdater_DownloadAndInstallProgress(object sender, MobiFlight.Base.ProgressUpdateEvent e)
        {
            this.HubHopUpdateProgress = e.Current;
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

        private void GetInterfaceITAPIVersion()
        {
            int intSize = 0;
            _ = interfaceIT_GetAPIVersion(null, ref intSize);
            StringBuilder aPIVersion = new(intSize);
            _ = interfaceIT_GetAPIVersion(aPIVersion, ref intSize);
            this.InterfaceITAPIVersion = "interfaceIT API version " + aPIVersion;
        }
    }
}