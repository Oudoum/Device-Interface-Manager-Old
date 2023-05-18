using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.FlightSimulator.SimConnect;
using static Device_Interface_Manager.MSFSProfiles.PMDG.PMDG_NG3_SDK;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceITAPI_Data;

namespace Device_Interface_Manager.MSFSProfiles.PMDG.B737.USB;

public abstract class NG_CDU_Base : MSFSProfiles.USB
{
    protected int CDU_annunEXECPos { get; set; } = 1;
    private bool _cDU_annunEXEC;
    protected bool CDU_annunEXEC
    {
        set
        {
            if (_cDU_annunEXEC != value)
            {
                interfaceIT_LED_Set(device.Session, CDU_annunEXECPos, _cDU_annunEXEC = value);
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
                interfaceIT_LED_Set(device.Session, CDU_annunCALLPos, _cDU_annunCALL = value);
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
                interfaceIT_LED_Set(device.Session, CDU_annunMSGPos, _cDU_annunMSG = value);
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
                interfaceIT_LED_Set(device.Session, CDU_annunFAILPos, _cDU_annunFAIL = value);
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
                interfaceIT_LED_Set(device.Session, CDU_annunOFSTPos, _cDU_annunOFST = value);
            }
        }
    }

    private byte _lTS_PedPanelKnob;
    private byte LTS_PedPanelKnob
    {
        get => _lTS_PedPanelKnob;
        set
        {
            if (_lTS_PedPanelKnob != value && ELEC_BusPowered_3)
            {
                interfaceIT_Brightness_Set(device.Session, (int)((_lTS_PedPanelKnob = value) * 1.5));
            }
        }
    }

    private bool _eLEC_BusPowered_3;
    private bool ELEC_BusPowered_3
    {
        get => _eLEC_BusPowered_3;
        set
        {
            if (_eLEC_BusPowered_3 != value)
            {
                _eLEC_BusPowered_3 = value;
                if (value)
                {
                    interfaceIT_Brightness_Set(device.Session, (int)(LTS_PedPanelKnob * 1.5));
                }
                else if (!value)
                {
                    interfaceIT_Brightness_Set(device.Session, 0);
                }
            }
        }
    }

    public override void Stop()
    {
        base.Stop();
        startupManager.pMDG737CDU?.Close();
    }

    protected PMDG_NG3_Data pMDG_NG3_Data = new();

    protected DATA_REQUEST_ID CDU_ID { get; set; }

    protected override void Simconnect_OnRecvClientData(SimConnect sender, SIMCONNECT_RECV_CLIENT_DATA data)
    {
        if ((uint)DATA_REQUEST_ID.DATA_REQUEST == data.dwRequestID)
        {
            pMDG_NG3_Data = (PMDG_NG3_Data)data.dwData[0];
            ELEC_BusPowered_3 = pMDG_NG3_Data.ELEC_BusPowered[3];
            LTS_PedPanelKnob = pMDG_NG3_Data.LTS_PedPanelKnob;
            RecvData();
            if (!pMDG_NG3_Data.ELEC_BusPowered[3])
            {
                startupManager.pMDG737CDU?.Dispatcher.BeginInvoke(delegate ()
                {
                    startupManager.pMDG737CDU.ClearPMDGCDUCells();
                });
            }
        }
        if ((uint)CDU_ID == data.dwRequestID)
        {
            startupManager.pMDG737CDU.Dispatcher.BeginInvoke(delegate ()
            {
                startupManager.pMDG737CDU.GetPMDGCDUCells((PMDG_NG3_CDU_Screen)data.dwData[0]);
            });
        }
    }

    protected abstract void RecvData();

    protected PMDG_737_CDU_StartupManager startupManager = new();

    protected async override Task StartSimConnect()
    {
        await base.StartSimConnect();
        await startupManager.PMDG737CDUStartup(simConnectClient);
        _ = Task.Run(GetValues);
    }

    protected override void KeyPressedProc(uint session, int key, uint direction)
    {

    }

    public void GetValues()
    {
        int oldValue = 0;
        int noldValue = 0;
        while (simConnectClient.simConnect is not null)
        {
            interfaceIT_Analog_GetValue(device.Session, 0, out int value);
            if (Math.Abs(value - oldValue) >= 50 || oldValue == 0)
            {
                oldValue = value;
            }
            if (Math.Abs(value - noldValue) > 40 || noldValue == 0)
            {
                startupManager.pMDG737CDU?.Dispatcher.BeginInvoke(() => startupManager.pMDG737CDU.Brightness = noldValue = value);
            }
            Thread.Sleep(50);
        }
    }
}
