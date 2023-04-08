using System.Threading;

namespace Device_Interface_Manager.MSFSProfiles;

public abstract class ENET : ENETBase
{
    public MVVM.View.PMDG737CDU pMDG737CDU;

    public MVVM.View.FBWA32NXMCDU fBWA32NXMCDU;

    protected SimConnectClient simConnectClient;

    protected void ReceiveSimConnectData(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            simConnectClient?.ReceiveSimConnectMessage();
            Thread.Sleep(10);
        }
    }

    protected virtual void StartSimConnect()
    {
        simConnectClient = new SimConnectClient();
        simConnectClient?.SimConnect_Open();
        simConnectClient.simConnect.OnRecvClientData += Simconnect_OnRecvClientData;
        simConnectClient.simConnect.OnRecvOpen += Simconnect_OnRecvOpen;
    }

    protected virtual void Simconnect_OnRecvClientData(Microsoft.FlightSimulator.SimConnect.SimConnect sender, Microsoft.FlightSimulator.SimConnect.SIMCONNECT_RECV_CLIENT_DATA data)
    {

    }

    protected virtual void Simconnect_OnRecvOpen(Microsoft.FlightSimulator.SimConnect.SimConnect sender, Microsoft.FlightSimulator.SimConnect.SIMCONNECT_RECV_OPEN data)
    {

    }

    private void StopSimConnect()
    {
        simConnectClient?.SimConnect_Close();
    }

    public void Start(string ipaddress)
    {
        CancellationTokenSource = new CancellationTokenSource();
        StartInterfaceITEthernetConnection(ipaddress);
        if (ConnectionStatus == 2)
        {
            StartinterfaceITEthernet();
            StartSimConnect();
            ReceiveSimConnectDataThread = new Thread(() => ReceiveSimConnectData(CancellationTokenSource.Token))
            {
                Name = ReceiveSimConnectDataThread?.ToString()
            };
            ReceiveSimConnectDataThread?.Start();
            ReceiveInterfaceITEthernetDataThread = new Thread(() => InterfaceITEthernet.GetinterfaceITEthernetData(INTERFACEIT_ETHERNET_KEY_NOTIFY_PROC = new(KeyPressedProcEthernet), CancellationTokenSource.Token))
            {
                Name = ReceiveInterfaceITEthernetDataThread?.ToString()
            };
            ReceiveInterfaceITEthernetDataThread?.Start();
        }
    }

    public override void Stop()
    {
        base.Stop();
        StopSimConnect();
    }
}