using System;
using System.Linq;
using System.Threading.Tasks;
using static Device_Interface_Manager.MSFSProfiles.PMDG.PMDG_NG3_SDK;

namespace Device_Interface_Manager.MSFSProfiles.PMDG.B737.USB;
public class NG_EFIS_R_330C_332E : MSFSProfiles.USB
{
    protected override async Task StartSimConnectAsync()
    {
        await base.StartSimConnectAsync();
        PMDG737.RegisterPMDGDataEvents(simConnectClient.SimConnect);
        NG_MCP_3311.OnBackgroundLEDChanged += NG_MCP_3311_OnBackgroundLEDChanged;
    }

    public int[] BackgroundLEDs { get; init; } = new int[] { 1, 2, 3, 4, 5, 6, 7 };
    private void NG_MCP_3311_OnBackgroundLEDChanged(object sender, bool e)
    {
        foreach (int ledNumber in BackgroundLEDs)
        {
            interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_LED_Set(Device.Session, ledNumber, e);
        }
    }

    private uint eFIS_FO_VOR_ADF_SELECTOR_L;
    private uint eFIS_FO_VOR_ADF_SELECTOR_R;
    private uint aDFR;

    protected override void KeyPressedProc(uint session, int key, uint direction)
    {
        //1-0
        if (new int[] { 1, 3 }.Contains(key))
        {
            if (direction == 1)
            {
                direction = 0;
            }

            else if (direction == 0)
            {
                direction = 1;
            }
        }

        //1-2
        else if (new int[] { 2, 4 }.Contains(key))
        {
            if (direction == 1)
            {
                direction = 2;
            }

            else if (direction == 0 && (eFIS_FO_VOR_ADF_SELECTOR_L != 0 || eFIS_FO_VOR_ADF_SELECTOR_R != 0))
            {
                direction = 1;
            }
        }

        //-3 & -14
        else if (new int[] { 9, 10, 11, 12, 13, 14, 15, 17, 18, 19, 20, 21, 22 }.Contains(key))
        {
            if (direction == 1)
            {
                direction = MOUSE_FLAG_LEFTSINGLE;
            }

            else if (direction == 0)
            {
                direction = MOUSE_FLAG_LEFTRELEASE;
            }
        }

        switch (key)
        {
            //1-0
            case 1:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_FO_VOR_ADF_SELECTOR_L);
                eFIS_FO_VOR_ADF_SELECTOR_L = direction;
                break;

            //1-2
            case 2:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_FO_VOR_ADF_SELECTOR_L);
                break;

            //1-0
            case 3:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_FO_VOR_ADF_SELECTOR_R);
                eFIS_FO_VOR_ADF_SELECTOR_R = direction;
                break;

            //1-2
            case 4:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_FO_VOR_ADF_SELECTOR_R);
                aDFR = direction;
                break;

            //0-...
            case 5 when direction == 1:
                simConnectClient.TransmitEvent(0, PMDGEvents.EVT_EFIS_FO_BARO_IN_HPA);
                break;

            case 6 when direction == 1:
                simConnectClient.TransmitEvent(1, PMDGEvents.EVT_EFIS_FO_BARO_IN_HPA);
                break;

            case 7 when direction == 1:
                simConnectClient.TransmitEvent(0, PMDGEvents.EVT_EFIS_FO_MINIMUMS_RADIO_BARO);
                break;

            case 8 when direction == 1:
                simConnectClient.TransmitEvent(1, PMDGEvents.EVT_EFIS_FO_MINIMUMS_RADIO_BARO);
                break;

            //-3 & -14
            case 9:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_FO_WXR);
                break;

            case 10:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_FO_STA);
                break;

            case 11:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_FO_WPT);
                break;

            case 12:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_FO_ARPT);
                break;

            case 13:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_FO_DATA);
                break;

            case 14:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_FO_POS);
                break;

            case 15:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_FO_TERR);
                break;

            case 17:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_FO_FPV);
                break;

            case 18:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_FO_MTRS);
                break;

            case 19:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_FO_MODE_CTR);
                break;

            case 20:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_FO_RANGE_TFC);
                break;

            case 21:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_FO_MINIMUMS_RST);
                break;

            case 22:
                simConnectClient.TransmitEvent(direction, PMDGEvents.EVT_EFIS_FO_BARO_STD);
                break;

            //0-...
            case 25 when direction == 1:
                simConnectClient.TransmitEvent(0, PMDGEvents.EVT_EFIS_FO_RANGE);
                break;

            case 26 when direction == 1:
                simConnectClient.TransmitEvent(1, PMDGEvents.EVT_EFIS_FO_RANGE);
                break;

            case 27 when direction == 1:
                simConnectClient.TransmitEvent(2, PMDGEvents.EVT_EFIS_FO_RANGE);
                break;

            case 28 when direction == 1:
                simConnectClient.TransmitEvent(3, PMDGEvents.EVT_EFIS_FO_RANGE);
                break;

            case 29 when direction == 1:
                simConnectClient.TransmitEvent(4, PMDGEvents.EVT_EFIS_FO_RANGE);
                break;

            case 30 when direction == 1:
                simConnectClient.TransmitEvent(5, PMDGEvents.EVT_EFIS_FO_RANGE);
                break;

            case 31 when direction == 1:
                simConnectClient.TransmitEvent(6, PMDGEvents.EVT_EFIS_FO_RANGE);
                break;

            case 32 when direction == 1:
                simConnectClient.TransmitEvent(7, PMDGEvents.EVT_EFIS_FO_RANGE);
                break;

            case 33 when direction == 1:
                simConnectClient.TransmitEvent(0, PMDGEvents.EVT_EFIS_FO_MODE);
                break;

            case 34 when direction == 1:
                simConnectClient.TransmitEvent(1, PMDGEvents.EVT_EFIS_FO_MODE);
                break;

            case 35 when direction == 1:
                simConnectClient.TransmitEvent(2, PMDGEvents.EVT_EFIS_FO_MODE);
                break;

            case 36 when direction == 1:
                simConnectClient.TransmitEvent(3, PMDGEvents.EVT_EFIS_FO_MODE);
                break;

            //-18 & -17
            //330C
            case 41 when direction == 1 && Device.DeviceInfo.szBoardType == "330C":
                TransmitBAROMINSTenTimes(PMDGEvents.EVT_EFIS_FO_MINIMUMS, true);
                break;

            case 42 when direction == 1 && Device.DeviceInfo.szBoardType == "330C":
                TransmitBAROMINSTenTimes(PMDGEvents.EVT_EFIS_FO_MINIMUMS, false);
                break;

            case 43 when direction == 1 && Device.DeviceInfo.szBoardType == "330C":
                TransmitBAROMINSTenTimes(PMDGEvents.EVT_EFIS_FO_BARO, true);
                break;

            case 44 when direction == 1 && Device.DeviceInfo.szBoardType == "330C":
                TransmitBAROMINSTenTimes(PMDGEvents.EVT_EFIS_FO_BARO, false);
                break;

            //332E
            case 41 when direction == 1 && Device.DeviceInfo.szBoardType == "332E":
                TransmitBAROMINSTenTimes(PMDGEvents.EVT_EFIS_FO_MINIMUMS, false);
                break;

            case 42 when direction == 1 && Device.DeviceInfo.szBoardType == "332E":
                TransmitBAROMINSTenTimes(PMDGEvents.EVT_EFIS_FO_MINIMUMS, true);
                break;

            case 43 when direction == 1 && Device.DeviceInfo.szBoardType == "332E":
                TransmitBAROMINSTenTimes(PMDGEvents.EVT_EFIS_FO_BARO, false);
                break;

            case 44 when direction == 1 && Device.DeviceInfo.szBoardType == "332E":
                TransmitBAROMINSTenTimes(PMDGEvents.EVT_EFIS_FO_BARO, true);
                break;
        }
    }

    private void TransmitBAROMINSTenTimes(Enum eventID, bool isUp)
    {
        if (aDFR != 2)
        {
            if (isUp)
            {
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_UP, eventID);
            }
            else if (!isUp)
            {
                simConnectClient.TransmitEvent(MOUSE_FLAG_WHEEL_DOWN, eventID);
            }
        }
        else if (aDFR == 2)
        {
            for (int i = 0; i < 10; i++)
            {
                if (isUp)
                {
                    simConnectClient.TransmitEvent(MOUSE_FLAG_RIGHTSINGLE, eventID);
                    simConnectClient.TransmitEvent(MOUSE_FLAG_RIGHTRELEASE, eventID);
                }
                else if (!isUp)
                {
                    simConnectClient.TransmitEvent(MOUSE_FLAG_LEFTSINGLE, eventID);
                    simConnectClient.TransmitEvent(MOUSE_FLAG_LEFTRELEASE, eventID);
                }
            }
        }
    }
}