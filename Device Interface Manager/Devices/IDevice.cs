using System;
using System.Data;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Device_Interface_Manager.Devices;
public interface IDevice
{
    Task<ConnectionState> ConnectAsync();
    void Disconnect();
}

public interface IInputDevice : IDevice
{
    event EventHandler<InputChangeEventArgs> InputChange;
}

public interface IOutputDevice : IDevice
{
    Task SendDataAsync(string data);
}

public interface IInputOutputDevice : IInputDevice, IOutputDevice
{

}

public class InputChangeEventArgs : EventArgs
{
    public int ButtonId { get; set; }
    public bool IsPressed { get; set; }
}

public abstract class COMDeviceBase : IInputOutputDevice
{
    public COMDeviceBase()
    {
        
    }

    public abstract event EventHandler<InputChangeEventArgs> InputChange;

    public abstract Task<ConnectionState> ConnectAsync();
    public abstract void Disconnect();
    public abstract Task SendDataAsync(string data);
}

public abstract class EthernetDeviceBase : IInputOutputDevice
{
    protected TcpClient client;
    protected NetworkStream stream;

    public abstract event EventHandler<InputChangeEventArgs> InputChange;

    public string HostIPAddress { get; private set; }
    public int TCPPort { get; private set; }

    public EthernetDeviceBase(string hostIPAddress, int tCPport)
    {
        HostIPAddress = hostIPAddress;
        TCPPort = tCPport;
    }

    public abstract Task SendDataAsync(string data);
    public abstract Task<ConnectionState> ConnectAsync();
    public abstract void Disconnect();
}

public class Test : EthernetDeviceBase
{
    public Test(string hostIPAddress, int tCPport) : base(hostIPAddress, tCPport)
    {
    }

    public override event EventHandler<InputChangeEventArgs> InputChange;

    public override Task<ConnectionState> ConnectAsync()
    {
        throw new NotImplementedException();
    }

    public override void Disconnect()
    {
        throw new NotImplementedException();
    }

    public override Task SendDataAsync(string data)
    {
        throw new NotImplementedException();
    }
}