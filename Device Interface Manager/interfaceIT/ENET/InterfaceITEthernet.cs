using System;
using System.Text;
using System.Windows;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Net.Http;

namespace Device_Interface_Manager.interfaceIT.ENET;

public class InterfaceITEthernet : ObservableObject
{
    private TcpClient client;
    private NetworkStream stream;

    //private const string FDS_737_PRO_MX_MCP_E = "0E04";
    //private const string FDS_A320_CDU_E = "0E09";
    //private const string FDS_737_CDU_E = "0E08";
    //private const string IIT_E_HIO_128_256_6 = "0E07";
    //private const string IIT_E_HIO_64_128_3 = "0E06";
    //private const string IIT_OEM_128_128_3_E = "0E0A";
    //private const string IIT_OEM_128_256_6_E = "0E05";
    //private const string IIT_OEM_256_256X_6_E = "0E0B";

    public string HostIPAddress { get; set; }
    public int TCPPort { get; set; } = 10346;
    public InterfaceITEthernetInfo InterfaceITEthernetInfo { get; set; }
    public ObservableCollection<string> InterfaceITEthernetInfoText { get; set; }

    public static async Task<string[]> ReceiveControllerDiscoveryData()
    {
        UdpClient client = new() { EnableBroadcast = true };
        client.Send(Encoding.ASCII.GetBytes("D"), new IPEndPoint(IPAddress.Broadcast, 30303));
        try
        {
            UdpReceiveResult result = await client.ReceiveAsync().WaitAsync(TimeSpan.FromSeconds(1));
            return new string[] { Encoding.ASCII.GetString(result.Buffer).Split("\r\n")[5], result.RemoteEndPoint.Address.ToString() };
        }
        catch (Exception)
        {
            return null;
        }
    }

    public enum ConnectionStatus
    {
        Default,
        NotConnected,
        PingSuccessful,
        Connected
    }

    public async Task<ConnectionStatus> InterfaceITEthernetConnectionAsync(CancellationToken cancellationToken)
    {
        if (!await PingHostAsync())
        {
            return ConnectionStatus.NotConnected;
        }
        if (!await ConnectToHostAsync(cancellationToken))
        {
            return ConnectionStatus.PingSuccessful;
        }
        return ConnectionStatus.Connected;
    }

    private async Task<bool> PingHostAsync()
    {
        using Ping ping = new();
        return (await ping.SendPingAsync(HostIPAddress)).Status == IPStatus.Success;
    }

    private async Task<bool> ConnectToHostAsync(CancellationToken cancellationToken)
    {
        try
        {
            client = new();
            await client.ConnectAsync(HostIPAddress, TCPPort, cancellationToken);
            stream = client.GetStream();
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
    }

    public async Task<InterfaceITEthernetInfo> GetInterfaceITEthernetDataAsync(Action<int, uint> interfacITKeyAction, CancellationToken cancellationToken)
    {
        TaskCompletionSource<InterfaceITEthernetInfo> tcs = new();
        _ = Task.Run(async () =>
        {
            StringBuilder sb = new();
            byte[] buffer = new byte[8192];
            bool isInitializing = false;
            bool isSwitchIdentifying = false;
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    int bytesRead = await stream.ReadAsync(buffer, cancellationToken);
                    sb.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));
                    if (buffer[bytesRead - 1] != 10)
                    {
                        continue;
                    }
                }
                catch (IOException)
                {
                    return;
                }
                catch (OperationCanceledException)
                {

                }

                foreach (string ethernetData in sb.ToString().Split("\r\n", StringSplitOptions.RemoveEmptyEntries))
                {
                    switch (ethernetData)
                    {
                        case "STATE=2":
                            isInitializing = true;
                            InterfaceITEthernetInfo = new()
                            {
                                HOSTIPADDRESS = HostIPAddress,
                            };
                            break;

                        case "STATE=3":
                            isSwitchIdentifying = true;
                            tcs.SetResult(InterfaceITEthernetInfo);
                            break;

                        case "STATE=4":
                            isInitializing = false;
                            isSwitchIdentifying = false;
                            break;

                        default:
                            if (isSwitchIdentifying || !isInitializing)
                            {
                                ProcessSwitchData(interfacITKeyAction, ethernetData);
                            }
                            else if (isInitializing && !isSwitchIdentifying)
                            {
                                GetInterfaceITEthernetInfo(ethernetData);
                            }
                            break;
                    }
                }
                sb.Clear();
            }
        }, cancellationToken);
        return await tcs.Task;
    }

    private static void ProcessSwitchData(Action<int, uint> interfacITKeyAction, string ethernetData)
    {
        if (ethernetData.StartsWith("B1="))
        {
            string[] splittedSwitchData = ethernetData.Replace("B1=SW:", string.Empty).Split(':');
            if (int.TryParse(splittedSwitchData[0], out int ledNumber))
            {
                uint direction = 0;
                if (splittedSwitchData[1] == "ON")
                {
                    direction = 1;
                }
                interfacITKeyAction(ledNumber, direction);
            }
        }
        File.AppendAllText("Log.txt", ethernetData + Environment.NewLine);
    }

    private void GetInterfaceITEthernetInfo(string ethernetData)
    {
        int index = ethernetData.IndexOf('=');
        if (index < 0)
        {
            return;
        }

        string value = ethernetData[(index + 1)..];
        switch (ethernetData[..index])
        {
            case "ID":
                InterfaceITEthernetInfo.ID = value;
                break;

            case "NAME":
                InterfaceITEthernetInfo.NAME = value;
                break;

            case "SERIAL":
                InterfaceITEthernetInfo.SERIAL = value;
                break;

            case "DESC":
                InterfaceITEthernetInfo.DESC = value;
                break;

            case "COPYRIGHT":
                InterfaceITEthernetInfo.COPYRIGHT = value;
                break;

            case "VERSION":
                InterfaceITEthernetInfo.VERSION = value;
                break;

            case "FIRMWARE":
                InterfaceITEthernetInfo.FIRMWARE = value;
                break;

            case "LOCATION":
                InterfaceITEthernetInfo.LOCATION = Convert.ToInt32(value);
                break;

            case "USAGE":
                InterfaceITEthernetInfo.USAGE = Convert.ToInt32(value);
                InterfaceITEthernetInfo.BOARDS = new InterfaceITEthernetInfoBoard[InterfaceITEthernetInfo.USAGE];
                break;

            case "HOSTNAME":
                InterfaceITEthernetInfo.HOSTNAME = value;
                break;

            case "CLIENT":
                InterfaceITEthernetInfo.CLIENT = value;
                break;

            case "BOARD":
                string[] board = value.Split(':');
                int boardNumber = Convert.ToInt32(board[0]);
                InterfaceITEthernetInfo.BOARDS[boardNumber - 1] = new() { BOARDNUMBER = boardNumber, ID = board[1], DESC = board[2] };
                break;

            case "CONFIG":
                GetConfigData(value);
                break;
        }
    }

    private void GetConfigData(string value)
    {
        string[] config = value.Split(":");
        int boardNumberMinusOne = Convert.ToInt32(config[0]) -1;
        switch (config[1])
        {
            case "LED":
                InterfaceITEthernetInfo.BOARDS[boardNumberMinusOne].LEDS = GetConfigData(config);
                break;

            case "SWITCH":
                InterfaceITEthernetInfo.BOARDS[boardNumberMinusOne].SWITCHES = GetConfigData(config);
                break;

            case "7 SEGMENT":
                InterfaceITEthernetInfo.BOARDS[boardNumberMinusOne].SEVENSEGMENTS = GetConfigData(config);
                break;

            case "DATALINE":
                InterfaceITEthernetInfo.BOARDS[boardNumberMinusOne].DATALINES = GetConfigData(config);
                break;

            case "ENCODER":
                InterfaceITEthernetInfo.BOARDS[boardNumberMinusOne].ENCODERS = GetConfigData(config);
                break;

            case "ANALOG IN":
                InterfaceITEthernetInfo.BOARDS[boardNumberMinusOne].ANALOGINS = GetConfigData(config);
                break;

            case "PULSE WIDTH":
                InterfaceITEthernetInfo.BOARDS[boardNumberMinusOne].PULSEWIDTHS = GetConfigData(config);
                break;
        }
    }

    private static InterfaceITEthernetInfoBoardConfig GetConfigData(string[] config)
    {
        return new InterfaceITEthernetInfoBoardConfig
        {
            Start = Convert.ToInt32(config[3]),
            Stop = Convert.ToInt32(config[5]),
            Total = Convert.ToInt32(config[7])
        };
    }

    public void CloseStream()
    {
        try
        {
            SendinterfaceITEthernetLEDAllOff();
            stream?.Write(Encoding.ASCII.GetBytes("DISCONNECT" + "\r\n"));
            client?.Close();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    public void SendinterfaceITEthernetLEDAllOff()
    {
        stream?.Write(Encoding.ASCII.GetBytes("B1:CLEAR" + "\r\n"));
    }

    public void SendinterfaceITEthernetLED(int nLED, bool bOn)
    {
        SendinterfaceITEthernetLED<bool>(nLED, bOn);
    }

    public void SendinterfaceITEthernetLED(int nLED, int bOn)
    {
        SendinterfaceITEthernetLED<int>(nLED, bOn);
    }

    public void SendinterfaceITEthernetLED(int nLED, double bOn)
    {
        SendinterfaceITEthernetLED<double>(nLED, bOn);
    }

    bool hasErrorBeenShown;
    private void SendinterfaceITEthernetLED<T>(int nLED, T bOn)
    {
        if (nLED < 0)
        {
            throw new ArgumentException("nLED must be a non-negative integer.");
        }

        if (bOn is not bool && bOn is not int && bOn is not double)
        {
            throw new ArgumentException("bOn must be of type bool, int, or double.");
        }

        Reconnect();

        try
        {
            stream?.Write(Encoding.ASCII.GetBytes("B1:LED:" + nLED + ":" + Convert.ToUInt16(bOn) + "\r\n"));
        }
        catch (Exception e)
        {
            Reconnect();
            if (!hasErrorBeenShown)
            {
                hasErrorBeenShown = true;
                MessageBox.Show(e.Message);
            }
        }
    }

    private void Reconnect()
    {
        if (hasErrorBeenShown)
        {
            try
            {
                client = new();
                client.Connect(HostIPAddress, TCPPort);
                stream = client.GetStream();
                hasErrorBeenShown = false;

            }
            catch (OperationCanceledException)
            {

            }
            catch (ArgumentNullException)
            {

            }
            catch (SocketException)
            {
            }
        }
    }
}

public class InterfaceITEthernetInfo
{
    public string HOSTIPADDRESS { get; set; }
    public string ID { get; set; }
    public string NAME { get; set; }
    public string SERIAL { get; set; }
    public string DESC { get; set; }
    public string COPYRIGHT { get; set; }
    public string VERSION { get; set; }
    public string FIRMWARE { get; set; }
    public int LOCATION { get; set; }
    public int USAGE { get; set; }
    public string HOSTNAME { get; set; }
    public string CLIENT { get; set; }
    public InterfaceITEthernetInfoBoard[] BOARDS { get; set; }
}

public class InterfaceITEthernetInfoBoard
{
    public int BOARDNUMBER { get; set; }
    public string ID { get; set; }
    public string DESC { get; set; }
    public InterfaceITEthernetInfoBoardConfig LEDS { get; set; }
    public InterfaceITEthernetInfoBoardConfig SWITCHES { get; set; }
    public InterfaceITEthernetInfoBoardConfig SEVENSEGMENTS { get; set; }
    public InterfaceITEthernetInfoBoardConfig DATALINES { get; set; }
    public InterfaceITEthernetInfoBoardConfig ENCODERS { get; set; }
    public InterfaceITEthernetInfoBoardConfig ANALOGINS { get; set; }
    public InterfaceITEthernetInfoBoardConfig PULSEWIDTHS { get; set; }
}

public class InterfaceITEthernetInfoBoardConfig
{
    public int Start { get; set; }
    public int Stop { get; set; }
    public int Total { get; set; }
}