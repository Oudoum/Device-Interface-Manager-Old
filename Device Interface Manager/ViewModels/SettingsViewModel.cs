using System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Device_Interface_Manager.Models;
using Microsoft.Extensions.Logging;

namespace Device_Interface_Manager.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly ILogger<SettingsViewModel> logger;

    [ObservableProperty]
    private string _wasmModuleUpdaterMessage;

    [ObservableProperty]
    private ConnectionStatus _Status;

    public bool MinimizedHide
    {
        get => Properties.Settings.Default.MinimizedHide;
        set
        {
            if (MinimizedHide != value)
            {
                Properties.Settings.Default.MinimizedHide = value;
                logger.LogInformation("MinimizedHide: " + value);
                Properties.Settings.Default.Save();
            }
        }
    }

    public bool AutoHide
    {
        get => Properties.Settings.Default.AutoHide;
        set
        {
            if (AutoHide != value)
            {
                Properties.Settings.Default.AutoHide = value;
                logger.LogInformation("AutoHide: " + value);
                Properties.Settings.Default.Save();
            }
        }
    }

    public bool IsP3D
    {
        get => Properties.Settings.Default.P3D;
        set
        {
            if (IsP3D != value)
            { 
                Properties.Settings.Default.P3D = value;
                logger.LogInformation("IsP3D: " + value);
                Properties.Settings.Default.Save();
            }
        }
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

    public SettingsViewModel(ILogger<SettingsViewModel> logger) 
    {
        this.logger = logger;
        GetInterfaceITAPIVersion();
        ReadSimConnectCfg();
    }

    [RelayCommand]
    private static void Discord()
    {
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("https://discord.gg/MRR9zUMQhM") { UseShellExecute = true });
    }

    [RelayCommand]
    private async Task InstallUpdateDIMWASModuleAsync()
    {
        logger.LogInformation("InstallUpdateDIMWASModule started");
        WasmModuleUpdater wasmModuleUpdater = new();
        WasmModuleUpdaterMessage = await Task.Run(wasmModuleUpdater.InstallWasmModule);
        wasmModuleUpdater = null;
        logger.LogInformation("InstallUpdateDIMWASModule finished");
    }

    [RelayCommand]
    private async Task CheckSimConnectConnectionAsync()
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
        IPStatus status = (await ping.SendPingAsync(IP)).Status;
        logger.LogInformation($"Ping result for IP {IP}: {status}");
        return status == IPStatus.Success;
    }

    private async Task<bool> ConnectToHostAsync()
    {
        try
        {
            using TcpClient tcpClient = new();
            await tcpClient.ConnectAsync(IP, Convert.ToInt32(Port)).WaitAsync(TimeSpan.FromSeconds(1));
            logger.LogInformation("SimConnect is available");
            return true;
        }
        catch (OperationCanceledException ex)
        {
            logger.LogCritical(ex, "ConnectToHostAsync OperationCanceledException");
            return false;
        }
        catch (ArgumentNullException ex)
        {
            logger.LogCritical(ex, "ConnectToHostAsync ArgumentNullException");
            return false;
        }
        catch (SocketException ex)
        {
            logger.LogCritical(ex, "ConnectToHostAsync SocketException");
            return false;
        }
        catch (TimeoutException ex)
        {
            logger.LogCritical(ex, "ConnectToHostAsync TimeoutException");
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
        InterfaceITAPIVersion = "interfaceIT API version " + Devices.interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_GetAPIVersion();
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