using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Device_Interface_Manager.Core;
using Device_Interface_Manager.interfaceIT.USB;
using Device_Interface_Manager.MSFSProfiles.PMDG;
using Device_Interface_Manager.MVVM.Model;

namespace Device_Interface_Manager.MVVM.ViewModel;
public partial class InputCreatorViewModel : ObservableObject, ICloseWindows
{
    public InputCreatorModel InputCreatorModel { get; set; }

    public Action Close { get; set; }

    public InterfaceIT_BoardInfo.Device Device { get; set; }

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

    public int? Input
    {
        get => InputCreatorModel.Input;
        set
        {
            if (InputCreatorModel.Input != value)
            {
                InputCreatorModel.Input = value;
                OnPropertyChanged(nameof(Input));
            }
        }
    }

    public int?[] Switches => InputCreatorModel.Switches;

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
                    if (value == ProfileCreatorModel.MSFSSimConnect || value == ProfileCreatorModel.RPN)
                    {
                        PMDGEvent = null;
                        PMDGMousePress = null;
                        PMDGMouseRelease = null;
                    }
                    if (value == ProfileCreatorModel.RPN)
                    {
                        DataPress = null;
                        DataRelease = null;
                    }
                    else if (value == ProfileCreatorModel.PMDG737)
                    {
                        Event = null;
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

    [RelayCommand]
    private void GetSwitch()
    {
        while (InterfaceITAPI_Data.interfaceIT_Switch_Get_Item(Device.Session, out int key, out int direction) == 0)
        {
            if (direction == 1)
            {
                Input = key;
            }
        }
    }

    [RelayCommand]
    private void SwitchComboBoxSlectionChanged(SelectionChangedEventArgs e)
    {
        //int? addedValue = null;
        //if (e.AddedItems.Count > 0)
        //{
        //    addedValue = (int?)e.AddedItems[0];
        //}

        //int? removedValue = null;
        //if (e.RemovedItems.Count > 0)
        //{
        //    removedValue = (int?)e.RemovedItems[0];
        //}

        //if (addedValue is not null)
        //{
        //    foreach (var item in TestCreator.InputCreator)
        //    {
        //        if (item.SelectedSwitch != addedValue)
        //        {
        //            item.Switches.Remove(addedValue);
        //        }
        //    }

        //    if (!TestCreator.RemovedSwitchItems.Contains(addedValue))
        //    {
        //        TestCreator.RemovedSwitchItems.Add(addedValue);
        //    }
        //    return;
        //}

        //TestCreator.RemovedSwitchItems.Remove(removedValue);

        //TestCreator.InputCreator
        //    .Where(item => !item.Switches.Contains(removedValue))
        //    .ToList()
        //    .ForEach(item =>
        //    {
        //        List<int?> switches = new(item.Switches)
        //        {
        //            removedValue
        //        };
        //        item.Switches = new(switches.OrderBy(i => i));
        //    });
    }

    public bool Save { get; set; }

    [RelayCommand]
    private void Ok()
    {
        Save = true;
        Close?.Invoke();
    }

    [RelayCommand]
    private static void ComboboxMouseEnterLeave(RoutedEventArgs e)
    {
        if (e.RoutedEvent == UIElement.GotFocusEvent)
        {
            ((ComboBox)e.Source).IsDropDownOpen = true;
        }
        else if (e.RoutedEvent == UIElement.LostFocusEvent)
        {
            ((ComboBox)e.Source).IsDropDownOpen = false;
        }
    }

}