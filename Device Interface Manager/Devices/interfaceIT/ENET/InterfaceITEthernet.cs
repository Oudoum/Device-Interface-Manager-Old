﻿using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Device_Interface_Manager.Devices.interfaceIT.ENET;

public class InterfaceITEthernet
{
    private TcpClient client;
    private NetworkStream stream;

    public string HostIPAddress { get; set; }
    public int TCPPort { get; set; } = 10346;
    public InterfaceITEthernetInfo InterfaceITEthernetInfo { get; set; }
    public bool IsPolling { get; set; }

    public InterfaceITEthernet(string iPAddress)
    {
        HostIPAddress = iPAddress;
    }

    public static async Task<InterfaceITEthernetDiscovery?> ReceiveControllerDiscoveryDataAsync()
    {
        UdpClient client = new() { EnableBroadcast = true };
        client.Send(Encoding.ASCII.GetBytes("D"), new IPEndPoint(IPAddress.Broadcast, 30303));
        try
        {
            UdpReceiveResult result = await client.ReceiveAsync().WaitAsync(TimeSpan.FromSeconds(1));
            string[] sResult = Encoding.ASCII.GetString(result.Buffer).Split("\r\n");
            InterfaceITEthernetDiscovery discovery = new()
            {
                IPAddress = result.RemoteEndPoint.Address.ToString(),
                HostName = sResult[0],
                MACAddress = sResult[1],
                Message = sResult[2],
                Id = sResult[3],
                Name = sResult[4],
                Description = sResult[5]
            };
            return discovery;
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
        while (!cancellationToken.IsCancellationRequested)
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

            }
            catch (ArgumentNullException)
            {
                CloseStream();
            }
            catch (SocketException)
            {
                CloseStream();
            }
        }
        return false;
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
                                HostIpAddress = HostIPAddress,
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
                                GetInterfaceITEthernetInfoData(ethernetData);
                            }
                            break;
                    }
                }
                sb.Clear();
            }
        }, cancellationToken);
        return await tcs.Task;
    }

    private void ProcessSwitchData(Action<int, uint> interfacITKeyAction, string ethernetData)
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

                if (IsPolling)
                {
                    polling.Push(new(ledNumber, direction));
                }
            }
        }
        File.AppendAllText("Log.txt", ethernetData + Environment.NewLine);
    }

    private readonly Stack<KeyValuePair<int, uint>> polling = new();

    public bool GetSwitch(out int ledNumber, out uint direction)
    {
        ledNumber = 0;
        direction = 0;
        if (polling.Count == 0)
        {
            return false;
        }
        KeyValuePair<int, uint> keyValuePair = polling.Pop();
        ledNumber = keyValuePair.Key;
        direction = keyValuePair.Value;
        return true;
    }

    private void GetInterfaceITEthernetInfoData(string ethernetData)
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
                InterfaceITEthernetInfo.Id = value;
                break;

            case "NAME":
                InterfaceITEthernetInfo.Name = value;
                break;

            case "SERIAL":
                InterfaceITEthernetInfo.SerialNumber = value;
                break;

            case "DESC":
                InterfaceITEthernetInfo.Description = value;
                break;

            case "COPYRIGHT":
                InterfaceITEthernetInfo.Copyright = value;
                break;

            case "VERSION":
                InterfaceITEthernetInfo.Version = value;
                break;

            case "FIRMWARE":
                InterfaceITEthernetInfo.Firmware = value;
                break;

            case "LOCATION":
                InterfaceITEthernetInfo.Location = Convert.ToByte(value);
                break;

            case "USAGE":
                InterfaceITEthernetInfo.Usage = Convert.ToByte(value);
                break;

            case "HOSTNAME":
                InterfaceITEthernetInfo.HostName = value;
                break;

            case "CLIENT":
                InterfaceITEthernetInfo.Client = value;
                break;

            case "BOARD":
                string[] board = value.Split(':');
                InterfaceITEthernetInfo.Boards ??= new();
                InterfaceITEthernetInfo.Boards.Add(new() { BoardNumber = Convert.ToByte(board[0]), Id = board[1], Description = board[2] });
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
                InterfaceITEthernetInfo.Boards[boardNumberMinusOne].LedsConfig = GetConfigData(config);
                break;

            case "SWITCH":
                InterfaceITEthernetInfo.Boards[boardNumberMinusOne].SwitchesConfig = GetConfigData(config);
                break;

            case "7 SEGMENT":
                InterfaceITEthernetInfo.Boards[boardNumberMinusOne].SevenSegmentsConfig = GetConfigData(config);
                break;

            case "DATALINE":
                InterfaceITEthernetInfo.Boards[boardNumberMinusOne].DataLinesConfig = GetConfigData(config);
                break;

            case "ENCODER":
                InterfaceITEthernetInfo.Boards[boardNumberMinusOne].EncodersConfig = GetConfigData(config);
                break;

            case "ANALOG IN":
                InterfaceITEthernetInfo.Boards[boardNumberMinusOne].AnalogInputsConfig = GetConfigData(config);
                break;

            case "PULSE WIDTH":
                InterfaceITEthernetInfo.Boards[boardNumberMinusOne].PulseWidthsConfig = GetConfigData(config);
                break;
        }
    }

    private static InterfaceITEthernetBoardConfig GetConfigData(string[] config)
    {
        return new InterfaceITEthernetBoardConfig
        {
            StartIndex = Convert.ToInt32(config[3]),
            StopIndex = Convert.ToInt32(config[5]),
            TotalCount = Convert.ToInt32(config[7])
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
        if (InterfaceITEthernetInfo.Boards is not null)
        {
            for (int i = InterfaceITEthernetInfo.Boards[0].LedsConfig.StartIndex; i <= InterfaceITEthernetInfo.Boards[0].LedsConfig.TotalCount; i++)
            {
                stream?.Write(Encoding.ASCII.GetBytes("B1:LED:" + i + ":" + 0 + "\r\n"));
            }
            //stream?.Write(Encoding.ASCII.GetBytes("B1:CLEAR" + "\r\n"));
        }
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

public readonly struct InterfaceITEthernetDiscovery
{
    public string HostName { get; init; }
    public string MACAddress { get; init; }
    public string IPAddress { get; init; }
    public string Message { get; init; }
    public string Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
}

public class InterfaceITEthernetInfo
{
    public string HostIpAddress { get; set; }
    public string Id { get; set; }
    public string Name { get; set; }
    public string SerialNumber { get; set; }
    public string Description { get; set; }
    public string Copyright { get; set; }
    public string Version { get; set; }
    public string Firmware { get; set; }
    public byte Location { get; set; }
    public byte Usage { get; set; }
    public string HostName { get; set; }
    public string Client { get; set; }
    public List<InterfaceITEthernetBoardInfo> Boards { get; set; }
}

public class InterfaceITEthernetBoardInfo
{
    public byte BoardNumber { get; set; }
    public string Id { get; set; }
    public string Description { get; set; }
    public InterfaceITEthernetBoardConfig LedsConfig { get; set; }
    public InterfaceITEthernetBoardConfig SwitchesConfig { get; set; }
    public InterfaceITEthernetBoardConfig SevenSegmentsConfig { get; set; }
    public InterfaceITEthernetBoardConfig DataLinesConfig { get; set; }
    public InterfaceITEthernetBoardConfig EncodersConfig { get; set; }
    public InterfaceITEthernetBoardConfig AnalogInputsConfig { get; set; }
    public InterfaceITEthernetBoardConfig PulseWidthsConfig { get; set; }
}

public class InterfaceITEthernetBoardConfig
{
    public int StartIndex { get; set; }
    public int StopIndex { get; set; }
    public int TotalCount { get; set; }
}