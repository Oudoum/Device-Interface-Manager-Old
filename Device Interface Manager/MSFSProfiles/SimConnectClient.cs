using System;
using Microsoft.FlightSimulator.SimConnect;
using System.Runtime.InteropServices;
using Device_Interface_Manager.MVVM.ViewModel;
using static Device_Interface_Manager.MSFSProfiles.PMDG.PMDG_NG3_SDK;

namespace Device_Interface_Manager.MSFSProfiles
{
    public class SimConnectClient
    {
        public SimConnect Simconnect { get; set; }
        private readonly IntPtr m_hWnd = new(0);
        private const int WM_USER_SIMCONNECT = 0x0402;

        //The SimConnect_Open function is used to send a request to the Microsoft Flight Simulator server to open up communications with a new client.
        public void SimConnect_Open()
        {
            while (MainViewModel.HomeUSBVM.IsSimConnectOpen == false)
            {
                try
                {
                    /// The constructor is similar to SimConnect_Open in the native API
                    Simconnect = new SimConnect("DeviceInterfaceManager", m_hWnd, WM_USER_SIMCONNECT, null, 0);

                    /// Listen to connect and quit msgs
                    Simconnect.OnRecvOpen += new SimConnect.RecvOpenEventHandler(SimConnect_OnRecvOpen);
                    Simconnect.OnRecvQuit += new SimConnect.RecvQuitEventHandler(SimConnect_OnRecvQuit);

                    /// Listen to exceptions
                    Simconnect.OnRecvException += new SimConnect.RecvExceptionEventHandler(SimConnect_OnRecvException);

                    //request current aircraft file path
                    Simconnect.RequestSystemState(DATA_REQUEST_ID.AIR_PATH_REQUEST, "AircraftLoaded");

                    break;
                }
                catch (COMException)
                {

                }
            }
        }

        public void ReceiveSimConnectMessage()
        {
            try
            {
                Simconnect?.ReceiveMessage();
            }
            catch (Exception)
            {
                MainViewModel.HomeUSBVM.IsSimConnectOpen = false;
            }
        }

        //The SimConnect_Close function is used to request that the communication with the server is ended.
        public void SimConnect_Close()
        {
            Simconnect?.Dispose();
            Simconnect = null;
            MainViewModel.HomeUSBVM.IsSimConnectOpen = false;
        }

        //The SIMCONNECT_RECV_OPEN structure is used to return information to the client, after a successful call to SimConnect_Open.
        private void SimConnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        {
            MainViewModel.HomeUSBVM.IsSimConnectOpen = true;
        }

        //The SIMCONNECT_RECV_QUIT is an identical structure to the SIMCONNECT_RECV structure.
        //The SIMCONNECT_RECV structure is used with the SIMCONNECT_RECV_ID enumeration to indicate which type of structure has been returned.
        private void SimConnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
        {
            MainViewModel.HomeUSBVM.IsSimConnectOpen = false;
        }

        //The SIMCONNECT_RECV_EXCEPTION structure is used with the SIMCONNECT_EXCEPTION enumeration type to return information on an error that has occurred.
        private void SimConnect_OnRecvException(SimConnect sender, SIMCONNECT_RECV_EXCEPTION data)
        {
            if (data.dwException != 9)
            {

            }
        }
    }
}