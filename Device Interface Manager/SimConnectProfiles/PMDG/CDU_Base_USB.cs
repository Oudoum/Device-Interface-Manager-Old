using System.Threading.Tasks;
using MahApps.Metro.Controls;
using static Device_Interface_Manager.Devices.interfaceIT.USB.InterfaceITAPI_Data;

namespace Device_Interface_Manager.SimConnectProfiles.PMDG;

public abstract class CDU_Base_USB<T> : USB where T : struct
{
    protected int CDU_annunEXECPos { get; set; } = 1;
    private bool _cDU_annunEXEC;
    protected bool CDU_annunEXEC
    {
        set
        {
            if (_cDU_annunEXEC != value)
            {
                interfaceIT_LED_Set(Device.Session, CDU_annunEXECPos, _cDU_annunEXEC = value);
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
                interfaceIT_LED_Set(Device.Session, CDU_annunCALLPos, _cDU_annunCALL = value);
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
                interfaceIT_LED_Set(Device.Session, CDU_annunMSGPos, _cDU_annunMSG = value);
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
                interfaceIT_LED_Set(Device.Session, CDU_annunFAILPos, _cDU_annunFAIL = value);
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
                interfaceIT_LED_Set(Device.Session, CDU_annunOFSTPos, _cDU_annunOFST = value);
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
                interfaceIT_Brightness_Set(Device.Session, (int)((_lTS_PedPanelKnob = value) * 1.5));
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
                    interfaceIT_Brightness_Set(Device.Session, (int)(LTS_PedPanelKnob * 1.5));
                }
                else if (!value)
                {
                    interfaceIT_Brightness_Set(Device.Session, 0);
                }
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
                PMDGCDU?.ClearPMDGCDUCells();
            }
        }
    }

    protected override void StopDevice()
    {
        base.StopDevice();
        startupManager.PMDGCDU?.Close();
    }

    protected T PMDG_Data { get; private set; } = new();

    protected abstract string SettingsLocation { get; }

    protected abstract PMDG.DATA_REQUEST_ID CDU_ID { get; }

    protected abstract System.Enum CDUBrightnessButton { get; }

    protected override async void SimConnect_OnRecvClientData(uint dwRequestID, object dwData)
    {
        if ((uint)PMDG.DATA_REQUEST_ID.DATA_REQUEST == dwRequestID)
        {
            PMDG_Data = (T)dwData;
            if (PMDG_Data is PMDG_NG3_SDK.PMDG_NG3_Data pMDGNG3)
            {
                SetELEC_BusPowered_15(pMDGNG3.ELEC_BusPowered[15]);
                LTS_PedPanelKnob = pMDGNG3.LTS_PedPanelKnob;
            }
            else if (PMDG_Data is PMDG_777X_SDK.PMDG_777X_Data || PMDG_Data is PMDG_747QOTSII_SDK.PMDG_747QOTSII_Data)
            {
                SetELEC_BusPowered_15(true);
            }
            RecvData();
        }
        else if ((uint)CDU_ID == dwRequestID)
        {
            startupManager.PMDGCDU.GetPMDGCDUCells((PMDG_CDU_SDK.ICDU_Screen)dwData);
            if (!((PMDG_CDU_SDK.ICDU_Screen)dwData).CDU_Screen.Powered)
            {
                await Task.Delay(500);
                startupManager.PMDGCDU.ClearPMDGCDUCells();
                return;
            }
        }
    }

    protected abstract void RecvData();

    private PMDG_CDU_StartupManager startupManager;

    protected async override Task StartSimConnectAsync()
    {
        await base.StartSimConnectAsync();
        if (simConnectClient.SimConnectMSFS is not null || simConnectClient.SimConnectP3D is not null)
        {
            startupManager = new(SettingsLocation);
            await startupManager.PMDG737CDUStartup(simConnectClient, CDUBrightnessButton);
            PMDGCDU = startupManager.PMDGCDU;
            PMDGCDU.Invoke(() => PMDGCDU.Brightness = 4095);
            GetCDUAnalogValues();
            interfaceIT_Dataline_Set(Device.Session, Device.DeviceInfo.DatalineFirst, true);
        }
    }

    protected abstract void GetCDUAnalogValues();
}