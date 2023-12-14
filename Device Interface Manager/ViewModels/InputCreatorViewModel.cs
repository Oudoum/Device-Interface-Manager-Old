using CommunityToolkit.Mvvm.Input;
using Device_Interface_Manager.Devices.interfaceIT.ENET;
using Device_Interface_Manager.Devices.interfaceIT.USB;
using Device_Interface_Manager.Models;
using Device_Interface_Manager.SimConnectProfiles.PMDG;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Device_Interface_Manager.ViewModels;
public partial class InputCreatorViewModel : BaseCreatorViewModel
{
    public InputCreatorViewModel(InputCreatorModel inputCreatorModel, object device) : base(device)
    {
        InputCreatorModel = inputCreatorModel;
        if (device is InterfaceIT_BoardInfo.Device iitdevice)
        {
            for (int i = iitdevice.BoardInfo.SwitchFirst; i <= iitdevice.BoardInfo.SwitchLast; i++)
            {
                string position = i.ToString();
                Inputs.Add(position, position);
            }
            InterfaceITAPI_Data.interfaceIT_Switch_Enable_Poll(iitdevice.Session, true);
        }
        else if (device is InterfaceITEthernet iitENETdevice && iitENETdevice.InterfaceITEthernetInfo.Boards.Count == 1)
        {
            for (int i = iitENETdevice.InterfaceITEthernetInfo.Boards[0].SwitchesConfig.StartIndex; i <= iitENETdevice.InterfaceITEthernetInfo.Boards[0].SwitchesConfig.StopIndex; i++)
            {
                string position = i.ToString();
                Inputs.Add(position, position);
            }
            iitENETdevice.IsPolling = true;
        }
        else if (device is Tuple<uint, uint> arduinoDevice)
        {
            for (uint i = arduinoDevice.Item1; i <= arduinoDevice.Item2; i++)
            {
                string position = i.ToString();
                Inputs.Add(position, position);
            }
        }
        else if (device is Devices.CPflight.Device cpflightdevice)
        {
            Inputs = cpflightdevice.SwitchInformations;
        }
    }

    public InputCreatorModel InputCreatorModel { get; set; }

    public string InputType
    {
        get => InputCreatorModel.InputType;
        set
        {
            if (InputCreatorModel.InputType != value)
            {
                InputCreatorModel.InputType = value;
                OnPropertyChanged(nameof(InputType));
            }
        }
    }

    public string[] InputTypes => InputCreatorModel.InputTypes;

    public KeyValuePair<string, string>? Input
    {
        get => InputCreatorModel.Input;
        set
        {
            if (!InputCreatorModel.Input.Equals(value))
            {
                InputCreatorModel.Input = value;
                OnPropertyChanged(nameof(Input));
            }
        }
    }

    public Dictionary<string, string> Inputs
    {
        get => InputCreatorModel.Inputs;
        set => InputCreatorModel.Inputs = value;
    }

    public string EventType
    {
        get => InputCreatorModel.EventType;
        set
        {
            if (InputCreatorModel.EventType != value)
            {
                InputCreatorModel.EventType = value;
                if (value is not null)
                {
                    switch (value)
                    {
                        case ProfileCreatorModel.MSFSSimConnect:
                        case ProfileCreatorModel.RPN:
                        case ProfileCreatorModel.KEVENT:
                            PMDGEvent = null;
                            PMDGMousePress = null;
                            PMDGMouseRelease = null;
                            OnRelease = false;
                            if (value == ProfileCreatorModel.RPN)
                            {
                                DataPress = null;
                                DataRelease = null;
                            }
                            break;
                        case ProfileCreatorModel.PMDG737:
                            Event = null;
                            OnRelease = false;
                            break;
                    }
                }
                OnPropertyChanged(nameof(EventType));
            }
        }
    }

    public string[] EventTypes => InputCreatorModel.EventTypes;

    public PMDG_NG3_SDK.PMDGEvents? PMDGEvent
    {
        get => InputCreatorModel.PMDGEvent;
        set
        {
            if (InputCreatorModel.PMDGEvent != value)
            {
                InputCreatorModel.PMDGEvent = value;
                OnPropertyChanged(nameof(PMDGEvent));
            }
        }
    }

    public string SearchPMDGEvent
    {
        get => InputCreatorModel.SearchPMDGEvent;
        set
        {
            if (InputCreatorModel.SearchPMDGEvent != value)
            {
                InputCreatorModel.SearchPMDGEvent = value;
                OnPropertyChanged(nameof(PMDGEvents));
                OnPropertyChanged(nameof(PMDGEvent));
            }
        }
    }

    public PMDG_NG3_SDK.PMDGEvents[] PMDGEvents => InputCreatorModel.PMDGEvents;

    public KeyValuePair<string, uint>? PMDGMousePress
    {
        get => InputCreatorModel.PMDGMousePress;
        set
        {
            if (!InputCreatorModel.PMDGMousePress.Equals(value))
            {
                InputCreatorModel.PMDGMousePress = value;
                OnPropertyChanged(nameof(PMDGMousePress));

            }
        }
    }

    public string SearchPMDGMousePress
    {
        get => InputCreatorModel.SearchPMDGMousePress;
        set
        {
            if (InputCreatorModel.SearchPMDGMousePress != value)
            {
                InputCreatorModel.SearchPMDGMousePress = value;
                OnPropertyChanged(nameof(PMDGMousePressArray));
                OnPropertyChanged(nameof(PMDGMousePress));
            }
        }
    }

    public KeyValuePair<string, uint>[] PMDGMousePressArray => InputCreatorModel.PMDGMousePressArray;

    public KeyValuePair<string, uint>? PMDGMouseRelease
    {
        get => InputCreatorModel.PMDGMouseRelease;
        set
        {
            if (!InputCreatorModel.PMDGMouseRelease.Equals(value))
            {
                InputCreatorModel.PMDGMouseRelease = value;
                OnPropertyChanged(nameof(PMDGMouseRelease));
            }
        }
    }

    public string SearchPMDGMouseRelease
    {
        get => InputCreatorModel.SearchPMDGMouseRelease;
        set
        {
            if (InputCreatorModel.SearchPMDGMouseRelease != value)
            {
                InputCreatorModel.SearchPMDGMouseRelease = value;
                OnPropertyChanged(nameof(PMDGMouseReleaseArray));
                OnPropertyChanged(nameof(PMDGMouseRelease));
            }
        }
    }

    public KeyValuePair<string, uint>[] PMDGMouseReleaseArray => InputCreatorModel.PMDGMouseReleaseArray;

    public string Event
    {
        get => InputCreatorModel.Event;
        set
        {
            if (InputCreatorModel.Event != value)
            {
                InputCreatorModel.Event = value;
                OnPropertyChanged(nameof(Event));
            }
        }
    }

    public bool OnRelease
    {
        get => InputCreatorModel.OnRelease;
        set
        {
            if (InputCreatorModel.OnRelease != value)
            {
                InputCreatorModel.OnRelease = value;
                OnPropertyChanged(nameof(OnRelease));
            }
        }
    }

    public uint? DataPress
    {
        get => InputCreatorModel.DataPress;
        set
        {
            if (InputCreatorModel.DataPress != value)
            {
                InputCreatorModel.DataPress = value;
                OnPropertyChanged(nameof(DataPress));
            }
        }
    }

    public uint? DataRelease
    {
        get => InputCreatorModel.DataRelease;
        set
        {
            if (InputCreatorModel.DataRelease != value)
            {
                InputCreatorModel.DataRelease = value;
                OnPropertyChanged(nameof(DataRelease));
            }
        }
    }

    public override ObservableCollection<OutputCreator> OutputCreator
    {
        get => InputCreatorModel.OutputCreator;
        set => InputCreatorModel.OutputCreator = value;
    }

    public override ObservableCollection<PreconditionModel> Preconditions
    {
        get => InputCreatorModel.Preconditions;
        set => InputCreatorModel.Preconditions = value;
    }

    public override MessageDialogResult CanClose()
    {
        if (Device is not null)
        {
            if (Device is InterfaceIT_BoardInfo.Device iitdevice)
            {
                InterfaceITAPI_Data.interfaceIT_Switch_Enable_Poll(iitdevice.Session, false);
            }
            else if (Device is InterfaceITEthernet iitENETdevice)
            {
                iitENETdevice.IsPolling = false;
            }
        }
        return base.CanClose();
    }

    [RelayCommand]
    private void GetSwitch()
    {
        switch (Device)
        {
            case InterfaceIT_BoardInfo.Device iitdevice:
                while (InterfaceITAPI_Data.interfaceIT_Switch_Get_Item(iitdevice.Session, out int key, out int direction) == 0)
                {
                    if (direction == 1)
                    {
                        Input = Inputs.FirstOrDefault(i => i.Key == key.ToString());
                    }
                }
                break;

            case InterfaceITEthernet iitENETdevice:
                while (iitENETdevice.GetSwitch(out int key, out uint direction))
                {
                    if (direction == 1)
                    {
                        Input = Inputs.FirstOrDefault(i => i.Key == key.ToString());
                    }
                }
                break;
        }
    }
}