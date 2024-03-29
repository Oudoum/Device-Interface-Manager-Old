﻿using System;
using System.Threading.Tasks;
using Device_Interface_Manager.Models;
using static Device_Interface_Manager.Devices.interfaceIT.USB.InterfaceIT_BoardInfo;
using static Device_Interface_Manager.Devices.interfaceIT.USB.InterfaceITAPI_Data;

namespace Device_Interface_Manager.SimConnectProfiles;
public class ProfileCreator_FDS_USB : ProfileCreatorBase<Device>
{
    private KeyNotifyCallbackUint interfaceItKeyNotifyProc;

    protected override Device Device { get; set; }

    private void KeyPressedProc(uint session, int key, uint direction)
    {
        ButtonIteration(key.ToString(), direction);
    }

    protected override Task<Devices.interfaceIT.ENET.InterfaceITEthernet.ConnectionStatus> StartDevice()
    {
        EnableDeviceFeatures(Device);
        interfaceItKeyNotifyProc = KeyPressedProc;
        return Task.FromResult(Devices.interfaceIT.ENET.InterfaceITEthernet.ConnectionStatus.Connected);
    }

    protected override void StopDevice()
    {
        interfaceIT_Switch_Enable_Callback(Device.Session, false, interfaceItKeyNotifyProc);
        InterfaceITDisable(Device);
    }

    protected override void CockpitLoaded(bool isLoaded)
    {
        interfaceIT_Switch_Enable_Callback(Device.Session, isLoaded, interfaceItKeyNotifyProc);
    }

    protected override void SetBooleanOuput(OutputCreator item, bool valueBool)
    {
        if (item.OutputType == ProfileCreatorModel.LED)
        {
            interfaceIT_LED_Set(Device.Session, Convert.ToInt32(item.Output.Value.Key), valueBool);
        }
        else if (item.OutputType == ProfileCreatorModel.DATALINE)
        {
            interfaceIT_Dataline_Set(Device.Session, Convert.ToInt32(item.Output.Value.Key), valueBool);
        }
    }

    protected override void SetDisplayOutput(OutputCreator item, string outputValue)
    {
        interfaceIT_7Segment_Display(Device.Session, outputValue, Convert.ToInt32(item.Output.Value.Key));
    }
}