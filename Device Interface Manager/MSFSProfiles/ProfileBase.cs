using System.Threading;
using System.Threading.Tasks;

namespace Device_Interface_Manager.MSFSProfiles;

public class ProfileBase
{
    public MVVM.View.PMDG737CDU pMDG737CDU;

    public MVVM.View.FBWA32NXMCDU fBWA32NXMCDU;

    protected SimConnectClient simConnectClient = new();

    protected CancellationTokenSource cancellationTokenSource = new();

    protected Task receiveSimConnectDataTask;

    protected void ReceiveSimConnectData(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            simConnectClient?.ReceiveSimConnectMessage();
            Thread.Sleep(10);
        }
    }

    protected async virtual Task StartSimConnect()
    {
        await simConnectClient.SimConnect_OpenAsync(cancellationTokenSource.Token);
        simConnectClient.simConnect.OnRecvClientData += Simconnect_OnRecvClientData;
        simConnectClient.simConnect.OnRecvOpen += Simconnect_OnRecvOpen;
    }

    protected virtual void Simconnect_OnRecvClientData(Microsoft.FlightSimulator.SimConnect.SimConnect sender, Microsoft.FlightSimulator.SimConnect.SIMCONNECT_RECV_CLIENT_DATA data)
    {

    }

    protected virtual void Simconnect_OnRecvOpen(Microsoft.FlightSimulator.SimConnect.SimConnect sender, Microsoft.FlightSimulator.SimConnect.SIMCONNECT_RECV_OPEN data)
    {

    }
}