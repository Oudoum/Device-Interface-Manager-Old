using System;

namespace Device_Interface_Manager.SimConnectMSFS;

public interface ICacheInterface
{
    /// <summary>
    /// Gets raised whenever connection is close
    /// </summary>
    public event EventHandler Closed;
    /// <summary>
    /// Gets raised whenever connection is established
    /// </summary>
    public event EventHandler Connected;
    /// <summary>
    /// Gets raised whenever connection is lost
    /// </summary>
    public event EventHandler ConnectionLost;

    public void Clear();

    /// <summary>
    /// indicates the status of the fsuipc connection
    /// </summary>
    /// <returns>true if connected, false if not</returns>
    public bool IsConnected();

    /// <summary>
    /// initializes and opens connection to fsuipc
    /// </summary>
    public bool Connect();

    /// <summary>
    /// disconnects from fsuipc
    /// </summary>        
    public bool Disconnect();
}