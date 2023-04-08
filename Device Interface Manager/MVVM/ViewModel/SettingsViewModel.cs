using System.IO;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Device_Interface_Manager.MVVM.ViewModel;

public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private int _hubHopUpdateProgress;

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

    private string _iP;
    public string IP
    {
        get => _iP;
        set
        {
            if (_iP is null) 
            {
                _iP = value;
                return;
            }
            if (_iP != value && System.Net.IPAddress.TryParse(value, out System.Net.IPAddress adress))
            {
                File.WriteAllText("SimConnect.cfg", File.ReadAllText("SimConnect.cfg").Replace("Address=" + _iP, "Address=" + (_iP = adress.ToString())));
            }
        }
    }

    private string _port;
    public string Port
    {
        get => _port;
        set
        {
            if (_port is null)
            {
                _port = value;
                return;
            }
            if (_iP != value && ushort.TryParse(value, out ushort port) && value != "0")
            {
                File.WriteAllText("SimConnect.cfg", File.ReadAllText("SimConnect.cfg").Replace("Port=" + _port, "Port=" + (_port = port.ToString())));
            }
        }
    }

    public SettingsViewModel() 
    {
        GetInterfaceITAPIVersion();
        ReadSimConnectCfg();
    }

    [RelayCommand]
    private static void Discord()
    {
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("https://discord.gg/MRR9zUMQhM") { UseShellExecute = true });
    }

    [RelayCommand]
    private async void InstallUpdateHubhop()
    {
        SimConnectMSFS.WasmModuleUpdater wasmModuleUpdater = new();
        wasmModuleUpdater.DownloadAndInstallProgress += WasmModuleUpdater_DownloadAndInstallProgress;
        wasmModuleUpdater.AutoDetectCommunityFolder();
        await Task.Run(() => wasmModuleUpdater.DownloadWasmEvents());
        await Task.Run(() => wasmModuleUpdater.InstallWasmModule());
        wasmModuleUpdater = null;
    }

    private void WasmModuleUpdater_DownloadAndInstallProgress(object sender, SimConnectMSFS.ProgressUpdateEvent e)
    {
        HubHopUpdateProgress = e.Current;
    }

    private void GetInterfaceITAPIVersion()
    {
        int intSize = 0;
        _ = interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_GetAPIVersion(null, ref intSize);
        StringBuilder aPIVersion = new(intSize);
        _ = interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_GetAPIVersion(aPIVersion, ref intSize);
        InterfaceITAPIVersion = "interfaceIT API version " + aPIVersion;
    }

    private void ReadSimConnectCfg()
    {
        if (File.Exists("SimConnect.cfg"))
        {
            foreach (var line in File.ReadLines("SimConnect.cfg"))
            {
                if (line.StartsWith("Address="))
                {
                    IP = line[8..];
                }
                if (line.StartsWith("Port="))
                {
                    Port = line[5..];
                }
            }
        }
    }
}