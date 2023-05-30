﻿using System.Threading.Tasks;
using Microsoft.FlightSimulator.SimConnect;
using static Device_Interface_Manager.MSFSProfiles.PMDG.PMDG_NG3_SDK;

namespace Device_Interface_Manager.MSFSProfiles.PMDG.B737.E;
public abstract class NG_CDU_Base : ENET
{
    protected int CDU_annunEXECPos { get; set; } = 1;
    private bool _cDU_annunEXEC;
    protected bool CDU_annunEXEC
    {
        set
        {
            if (_cDU_annunEXEC != value)
            {
                interfaceITEthernet.SendinterfaceITEthernetLED(CDU_annunEXECPos, _cDU_annunEXEC = value);
            }
        }
    }

    protected int CDU_annunCALLPos { get; set; } = 2;
    private bool _cDU_annunCALL;
    protected bool CDU_annunCALL
    {
        set
        {
            if (_cDU_annunCALL != value)
            {
                interfaceITEthernet.SendinterfaceITEthernetLED(CDU_annunCALLPos, _cDU_annunCALL = value);
            }
        }
    }

    protected int CDU_annunMSGPos { get; set; } = 3;
    private bool _cDU_annunMSG;
    protected bool CDU_annunMSG
    {
        set
        {
            if (_cDU_annunMSG != value)
            {
                interfaceITEthernet.SendinterfaceITEthernetLED(CDU_annunMSGPos, _cDU_annunMSG = value);
            }
        }
    }

    protected int CDU_annunFAILPos { get; set; } = 4;
    private bool _cDU_annunFAIL;
    protected bool CDU_annunFAIL
    {
        set
        {
            if (_cDU_annunFAIL != value)
            {
                interfaceITEthernet.SendinterfaceITEthernetLED(CDU_annunFAILPos, _cDU_annunFAIL = value);
            }
        }
    }

    protected int CDU_annunOFSTPos { get; set; } = 5;
    private bool _cDU_annunOFST;
    protected bool CDU_annunOFST
    {
        set
        {
            if (_cDU_annunOFST != value)
            {
                interfaceITEthernet.SendinterfaceITEthernetLED(CDU_annunOFSTPos, _cDU_annunOFST = value);
            }
        }
    }

    private bool _eLEC_BusPowered_15;
    private void SetELEC_BusPowered_15(bool value)
    {
        if (_eLEC_BusPowered_15 != value)
        {
            _eLEC_BusPowered_15 = value;
            if (!value)
            {
                startupManager.PMDG737CDU.ClearPMDGCDUCells();
            }
        }
    }

    public override void Stop()
    {
        base.Stop();
        startupManager.PMDG737CDU?.Close();
    }

    protected PMDG_NG3_Data pMDG_NG3_Data = new();

    protected DATA_REQUEST_ID CDU_ID { get; set; }

    protected override async void SimConnect_OnRecvClientData(SimConnect sender, SIMCONNECT_RECV_CLIENT_DATA data)
    {
        if ((uint)DATA_REQUEST_ID.DATA_REQUEST == data.dwRequestID)
        {
            pMDG_NG3_Data = (PMDG_NG3_Data)data.dwData[0];
            SetELEC_BusPowered_15(pMDG_NG3_Data.ELEC_BusPowered[15]);
            RecvData();
        }
        if ((uint)CDU_ID == data.dwRequestID)
        {
            if (!((PMDG_NG3_CDU_Screen)data.dwData[0]).Powered)
            {
                await Task.Delay(500);
                startupManager.PMDG737CDU.ClearPMDGCDUCells();
                return;
            }
            startupManager.PMDG737CDU.GetPMDGCDUCells((PMDG_NG3_CDU_Screen)data.dwData[0]);
        }
    }

    protected abstract void RecvData();

    protected PMDG_737_CDU_StartupManager startupManager = new();

    protected async override Task StartSimConnectAsync()
    {
        await base.StartSimConnectAsync();
        if (simConnectClient.SimConnect is not null)
        {
            await startupManager.PMDG737CDUStartup(simConnectClient);
            PMDG737CDU = startupManager.PMDG737CDU;
        }
    }

    protected override void KeyPressedAction(int key, uint direction)
    {

    }
}