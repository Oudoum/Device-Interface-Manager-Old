using System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Device_Interface_Manager.MVVM.Model;

namespace Device_Interface_Manager.MVVM.ViewModel;

public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private string _wasmModuleUpdaterMessage;

    [ObservableProperty]
    private ConnectionStatus _Status;

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
            if (_iP != value)
            {
                if (string.IsNullOrEmpty(_iP))
                {
                    _iP = value;
                }
                else if (System.Net.IPAddress.TryParse(value, out System.Net.IPAddress adress))
                {
                    if (System.Net.IPAddress.Any.Equals(adress))
                    {
                        adress = System.Net.IPAddress.Loopback;
                        if (_iP == adress.ToString())
                        {
                            return;
                        }
                    }
                    UpdateSimConnectConfigFile("Address", adress.ToString());
                    _iP = adress.ToString();
                }
            }
        }
    }

    private string _port;
    public string Port
    {
        get => _port;
        set
        {
            if (_port != value)
            {
                if (string.IsNullOrEmpty(_port))
                {
                    _port = value;
                }
                else if (ushort.TryParse(value, out ushort port) && value != "0")
                {
                    UpdateSimConnectConfigFile("Port", port.ToString());
                    _port = port.ToString();
                }
            }
        }
    }

    private void UpdateSimConnectConfigFile(string key, string newValue)
    {
        string oldValue = key == "Address" ? _iP : _port;
        File.WriteAllText("SimConnect.cfg", File.ReadAllText("SimConnect.cfg").Replace(key + "=" + oldValue, key + "=" + newValue));
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
    private async Task InstallUpdateDIMWASModule()
    {
        WasmModuleUpdater wasmModuleUpdater = new();
        WasmModuleUpdaterMessage = await Task.Run(wasmModuleUpdater.InstallWasmModule);
        wasmModuleUpdater = null;
    }

    [RelayCommand]
    private async Task CheckSimConnectConnection()
    {
        if (!await PingHostAsync())
        {
            Status = ConnectionStatus.NotConnected;
            return;
        }
        if (!await ConnectToHostAsync())
        {
            Status = ConnectionStatus.PingSuccessful;
            return;
        }
        Status = ConnectionStatus.Connected;
    }

    [RelayCommand]
    private void GotIPPortTextBoxFocus()
    {
        Status = ConnectionStatus.Default;
    }

    private async Task<bool> PingHostAsync()
    {
        using Ping ping = new();
        return (await ping.SendPingAsync(IP)).Status == IPStatus.Success;
    }

    private async Task<bool> ConnectToHostAsync()
    {
        try
        {
            using TcpClient tcpClient = new();
            await tcpClient.ConnectAsync(IP, Convert.ToInt32(Port)).WaitAsync(TimeSpan.FromSeconds(1));
            return true;
        }
        catch (OperationCanceledException)
        {
            return false;
        }
        catch (ArgumentNullException)
        {
            return false;
        }
        catch (SocketException)
        {
            return false;
        }
        catch (TimeoutException)
        {
            return false;
        }
    }

    public enum ConnectionStatus
    {
        Default,
        NotConnected,
        PingSuccessful,
        Connected
    }

    private void GetInterfaceITAPIVersion()
    {
        InterfaceITAPIVersion = "interfaceIT API version " + interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_GetAPIVersion();
    }

    private void ReadSimConnectCfg()
    {
        if (File.Exists("SimConnect.cfg"))
        {
            foreach (string line in File.ReadLines("SimConnect.cfg"))
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