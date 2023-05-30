using System.Threading;
using System.Threading.Tasks;

namespace Device_Interface_Manager.MSFSProfiles;

public abstract class ProfileBase
{
    public MVVM.View.PMDG737CDU PMDG737CDU { get; set; }

    public MVVM.View.FBWA32NXMCDU FBWA32NXMCDU { get; set; }

    protected SimConnectClient simConnectClient = SimConnectClient.Instance;

    protected CancellationTokenSource cancellationTokenSource = new();

    protected async virtual Task StartSimConnectAsync()
    {
        simConnectClient.OnSimVarChanged += SimConnectClient_OnSimVarChanged;
        await simConnectClient.SimConnect_OpenAsync(cancellationTokenSource.Token);
        if (simConnectClient.SimConnect is not null)
        {
            simConnectClient.SimConnect.OnRecvOpen += SimConnect_OnRecvOpen;
            simConnectClient.SimConnect.OnRecvQuit += SimConnect_OnRecvQuit;
            simConnectClient.SimConnect.OnRecvClientData += SimConnect_OnRecvClientData;
        }
    }

    public virtual void Stop()
    {

    }

    protected virtual void SimConnectClient_OnSimVarChanged(object sender, SimConnectClient.SimVar e)
    {

    }


    protected virtual void SimConnect_OnRecvQuit(Microsoft.FlightSimulator.SimConnect.SimConnect sender, Microsoft.FlightSimulator.SimConnect.SIMCONNECT_RECV data)
    {
        Stop();
    }

    protected virtual void SimConnect_OnRecvOpen(Microsoft.FlightSimulator.SimConnect.SimConnect sender, Microsoft.FlightSimulator.SimConnect.SIMCONNECT_RECV_OPEN data)
    {

    }

    protected virtual void SimConnect_OnRecvClientData(Microsoft.FlightSimulator.SimConnect.SimConnect sender, Microsoft.FlightSimulator.SimConnect.SIMCONNECT_RECV_CLIENT_DATA data)
    {

    }
}