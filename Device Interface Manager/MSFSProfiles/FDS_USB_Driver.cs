using System.Linq;
using Microsoft.FlightSimulator.SimConnect;
using Device_Interface_Manager.MSFSProfiles.PMDG;
using Device_Interface_Manager.MVVM.Model;

namespace Device_Interface_Manager.MSFSProfiles;
public class FDS_USB_Driver : USB
{
    public required ProfileCreatorModel TestCreator { get; init; }

    protected override void SimConnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
    {
        interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_Dataline_Set(Device.Session, Device.DeviceInfo.nDatalineFirst, true);

        PMDGProfile.FieldChanged += PMDGProfile_FieldChanged;

        foreach (var item in TestCreator.OutputCreator)
        {
            if (item.SelectedDataType == ProfileCreatorModel.MSFSSimConnect && !string.IsNullOrEmpty(item.Data))
            {
                simConnectClient.RegisterSimVar(item.Data);
            }
        }
    }

    protected override void SimConnectClient_OnSimVarChanged(object sender, SimConnectClient.SimVar simVar)
    {
        base.SimConnectClient_OnSimVarChanged(sender, simVar);

        foreach (var item in TestCreator.OutputCreator.Where(k => k.Data == simVar.Name))
        {
            if (item.SelectedLED is not null)
            {
                if (item.IsInverted)
                {
                    interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_LED_Set(Device.Session, item.SelectedLED.Value, !simVar.BData());
                    continue;
                }
                interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_LED_Set(Device.Session, item.SelectedLED.Value, simVar.BData());
            }
        }
    }

    private void PMDGProfile_FieldChanged(object sender, PMDGDataFieldChangedEventArgs e)
    {
        foreach (var item in TestCreator.OutputCreator.Where(k => k.SelectedDataType == ProfileCreatorModel.PMDG737))
        {
            if ((item.PMDGStructArrayNum is not null && item.PMDGDataFieldName + '_' + item.PMDGStructArrayNum == e.PMDGDataName) || item.PMDGDataFieldName == e.PMDGDataName)
            {
                if (e.Value is bool valueBool)
                {
                    if (item.IsInverted)
                    {
                        interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_LED_Set(Device.Session, item.SelectedLED.Value, !valueBool);
                        continue;
                    }
                    interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_LED_Set(Device.Session, item.SelectedLED.Value, valueBool);
                }
                else if (e.Value is byte valueByte && item.ComparisonValue is not null && item.ComparisonValue <= byte.MaxValue)
                {
                    if (valueByte == item.ComparisonValue)
                    {
                        interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_LED_Set(Device.Session, item.SelectedLED.Value, true);
                        continue;
                    }
                    interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_LED_Set(Device.Session, item.SelectedLED.Value, false);
                }
            }
        }
    }

    protected override void KeyPressedProc(uint session, int key, uint direction)
    {
        foreach (var item in TestCreator.InputCreator.Where(k => k.SelectedSwitch == key))
        {
            //RPN
            if (item.SelectedEventType == ProfileCreatorModel.RPN && direction == 1)
            {
                simConnectClient.SendWASMEvent(item.Event);
                continue;
            }

            //Direction
            if (item.EventDataRelease is not null && direction == 0)
            {
                direction = item.EventDataRelease.Value;
            }
            else if (item.EventDataPress is not null && direction == 1)
            {
                direction = item.EventDataPress.Value;
            }
            else if (item.PMDGMouseEventRelease is not null && direction == 0)
            {
                direction = item.PMDGMouseEventRelease.Value.Value;
            }
            else if (item.PMDGMouseEventPress is not null && direction == 1)
            {
                direction = item.PMDGMouseEventPress.Value.Value;
            }
            else
            {
                return;
            }

            //MSFS/SimConnect/LVar
            if (item.SelectedEventType == ProfileCreatorModel.MSFSSimConnect && !string.IsNullOrEmpty(item.Event))
            {
                simConnectClient.SetSimVar(direction, item.Event);
            }

            //PMDG 737
            else if (item.SelectedEventType == ProfileCreatorModel.PMDG737 && item.PMDGEvent is not null)
            {
                simConnectClient.TransmitEvent(direction, item.PMDGEvent);
            }
        }
    }
}