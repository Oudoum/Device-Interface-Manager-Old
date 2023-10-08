using System;
using System.Threading;
using System.Threading.Tasks;

namespace Device_Interface_Manager.SimConnectProfiles;

public abstract class ProfileBase<T>
{
    public Views.PMDGCDU PMDGCDU { get; set; }

    public Views.FBWA32NXMCDU FBWA32NXMCDU { get; set; }

    protected SimConnectClient simConnectClient = SimConnectClient.Instance;

    protected CancellationTokenSource cancellationTokenSource = new();

    protected Devices.interfaceIT.ENET.InterfaceITEthernet.ConnectionStatus ConnectionStatus { get; set; }

    protected abstract T Device { get; set; }

    protected async virtual Task StartSimConnectAsync()
    {
        simConnectClient.OnSimVarChanged += SimConnectClient_OnSimVarChanged;
        await simConnectClient.SimConnect_OpenAsync(cancellationTokenSource.Token);
        if (simConnectClient.SimConnectMSFS is not null)
        {
            simConnectClient.SimConnectMSFS.OnRecvOpen += SimConnect_OnRecvOpenMSFS;
            simConnectClient.SimConnectMSFS.OnRecvQuit += SimConnect_OnRecvQuitMSFS;
            simConnectClient.SimConnectMSFS.OnRecvClientData += SimConnect_OnRecvClientDataMSFS;
        }
        else if (simConnectClient.SimConnectP3D is not null)
        {
            simConnectClient.SimConnectP3D.OnRecvOpen += SimConnect_OnRecvOpenP3D;
            simConnectClient.SimConnectP3D.OnRecvQuit += SimConnect_OnRecvQuitP3D;
            simConnectClient.SimConnectP3D.OnRecvClientData += SimConnect_OnRecvClientDataP3D;
        }
    }

    public async Task<Devices.interfaceIT.ENET.InterfaceITEthernet.ConnectionStatus> StartAsync(T device)
    {
        ConnectionStatus = Devices.interfaceIT.ENET.InterfaceITEthernet.ConnectionStatus.NotConnected;
        if (device is not null)
        {
            Device = device;
            ConnectionStatus = await StartDevice();
            await StartSimConnectAsync();
        }
        return ConnectionStatus;
    }

    protected abstract Task<Devices.interfaceIT.ENET.InterfaceITEthernet.ConnectionStatus> StartDevice();

    public void Stop()
    {
        if (Device is not null)
        {
            if (simConnectClient.SimConnectMSFS is not null)
            {
                simConnectClient.SimConnectMSFS.OnRecvClientData -= SimConnect_OnRecvClientDataMSFS;
                simConnectClient.SimConnectMSFS.OnRecvQuit -= SimConnect_OnRecvQuitMSFS;
                simConnectClient.SimConnectMSFS.OnRecvOpen -= SimConnect_OnRecvOpenMSFS;
            }
            else if (simConnectClient.SimConnectP3D is not null)
            {
                simConnectClient.SimConnectP3D.OnRecvClientData -= SimConnect_OnRecvClientDataP3D;
                simConnectClient.SimConnectP3D.OnRecvQuit -= SimConnect_OnRecvQuitP3D;
                simConnectClient.SimConnectP3D.OnRecvOpen -= SimConnect_OnRecvOpenP3D;
            }
            simConnectClient.OnSimVarChanged -= SimConnectClient_OnSimVarChanged;
            cancellationTokenSource.Cancel();
            StopDevice();
            simConnectClient.SimConnect_Close();
        }
    }

    protected abstract void StopDevice();

    protected abstract void CockpitLoaded(bool isLoaded);

    protected double? OldCameraState { get; set; }

    protected virtual async void SimConnectClient_OnSimVarChanged(object sender, SimConnectClient.SimVar simVar)
    {
        if (simVar.Name == "CAMERA STATE" && OldCameraState != simVar.Data)
        {
            if (simVar.Data == (double)SimConnectClient.CameraState.Cockpit && OldCameraState != (double)SimConnectClient.CameraState.Cockpit)
            {
                if (OldCameraState == (double)SimConnectClient.CameraState.Waiting || OldCameraState == (double)SimConnectClient.CameraState.MenuRTC)
                {
                    await Task.Delay(TimeSpan.FromSeconds(15));
                    CockpitLoaded(true);
                }
            }
            if (simVar.Data == (double)SimConnectClient.CameraState.Cockpit && OldCameraState == null)
            {
                CockpitLoaded(true);
            }
            if (simVar.Data == (double)SimConnectClient.CameraState.MenuRTC && OldCameraState != (double)SimConnectClient.CameraState.MenuRTC)
            {
                CockpitLoaded(false);
            }
            OldCameraState = simVar.Data;
        }
    }

    private void SimConnect_OnRecvOpenMSFS(Microsoft.FlightSimulator.SimConnect.SimConnect sender, Microsoft.FlightSimulator.SimConnect.SIMCONNECT_RECV_OPEN data)
    {
        SimConnect_OnRecvOpen();
    }

    private void SimConnect_OnRecvQuitMSFS(Microsoft.FlightSimulator.SimConnect.SimConnect sender, Microsoft.FlightSimulator.SimConnect.SIMCONNECT_RECV data)
    {
        SimConnect_OnRecvQuit();
    }

    private void SimConnect_OnRecvClientDataMSFS(Microsoft.FlightSimulator.SimConnect.SimConnect sender, Microsoft.FlightSimulator.SimConnect.SIMCONNECT_RECV_CLIENT_DATA data)
    {
        SimConnect_OnRecvClientData(data.dwRequestID, data.dwData[0]);
    }

    private void SimConnect_OnRecvOpenP3D(LockheedMartin.Prepar3D.SimConnect.SimConnect sender, LockheedMartin.Prepar3D.SimConnect.SIMCONNECT_RECV_OPEN data)
    {
        SimConnect_OnRecvOpen();
    }

    private void SimConnect_OnRecvQuitP3D(LockheedMartin.Prepar3D.SimConnect.SimConnect sender, LockheedMartin.Prepar3D.SimConnect.SIMCONNECT_RECV data)
    {
        SimConnect_OnRecvQuit();
    }

    private void SimConnect_OnRecvClientDataP3D(LockheedMartin.Prepar3D.SimConnect.SimConnect sender, LockheedMartin.Prepar3D.SimConnect.SIMCONNECT_RECV_CLIENT_DATA data)
    {
        SimConnect_OnRecvClientData(data.dwRequestID, data.dwData[0]);
    }

    protected virtual void SimConnect_OnRecvOpen()
    {

    }

    protected virtual void SimConnect_OnRecvQuit()
    {
        Stop();
    }

    protected virtual void SimConnect_OnRecvClientData(uint dwRequestID, object dwData)
    {

    }
}