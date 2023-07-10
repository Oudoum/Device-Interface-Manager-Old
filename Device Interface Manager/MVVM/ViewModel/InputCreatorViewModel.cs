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

    public int? SelectedSwitch
    {
        get => InputCreatorModel.SelectedSwitch;
        set
        { 
            InputCreatorModel.SelectedSwitch = value;
            OnPropertyChanged(nameof(SelectedSwitch));
        }
    }

    public int?[] Switches => InputCreatorModel.Switches;

    public string SelectedEventType
    {
        get => InputCreatorModel.SelectedEventType;
        set
        {
            InputCreatorModel.SelectedEventType = value;
            OnPropertyChanged(nameof(SelectedEventType));
            OnPropertyChanged(nameof(PMDGEvent));
            OnPropertyChanged(nameof(PMDGMouseEventPress));
            OnPropertyChanged(nameof(PMDGMouseEventRelease));
        }
    }

    public string[] EventType => InputCreatorModel.EventType;

    public PMDG_NG3_SDK.PMDGEvents? PMDGEvent
    {
        get => InputCreatorModel.PMDGEvent;
        set
        {
            InputCreatorModel.PMDGEvent = value;
            OnPropertyChanged(nameof(PMDGEvent));
        }
    }

    public string SearchPMDGEventsText
    {
        get => InputCreatorModel.SearchPMDGEventsText;
        set
        {
            InputCreatorModel.SearchPMDGEventsText = value;
            OnPropertyChanged(nameof(PMDGEvents));
            OnPropertyChanged(nameof(PMDGEvent));
        }
    }

    public PMDG_NG3_SDK.PMDGEvents[] PMDGEvents => InputCreatorModel.PMDGEvents;

    public KeyValuePair<string, uint>? PMDGMouseEventPress
    {
        get => InputCreatorModel.PMDGMouseEventPress;
        set
        {
            InputCreatorModel.PMDGMouseEventPress = value;
            OnPropertyChanged(nameof(PMDGMouseEventPress));
        }
    }

    public string SearchPMDGMouseEventsTextPress
    {
        get => InputCreatorModel.SearchPMDGMouseEventsTextPress;
        set
        {
            InputCreatorModel.SearchPMDGMouseEventsTextPress = value;
            OnPropertyChanged(nameof(PMDGMouseEventsPress));
            OnPropertyChanged(nameof(PMDGMouseEventPress));
        }
    }

    public KeyValuePair<string, uint>[] PMDGMouseEventsPress => InputCreatorModel.PMDGMouseEventsPress;

    public KeyValuePair<string, uint>? PMDGMouseEventRelease
    {
        get => InputCreatorModel.PMDGMouseEventRelease;
        set
        {
            InputCreatorModel.PMDGMouseEventRelease = value;
            OnPropertyChanged(nameof(PMDGMouseEventRelease));
        }
    }

    public string SearchPMDGMouseEventsTextRelease
    {
        get => InputCreatorModel.SearchPMDGMouseEventsTextRelease;
        set
        {
            InputCreatorModel.SearchPMDGMouseEventsTextRelease = value;
            OnPropertyChanged(nameof(PMDGMouseEventsRelease));
            OnPropertyChanged(nameof(PMDGMouseEventRelease));
        }
    }

    public KeyValuePair<string, uint>[] PMDGMouseEventsRelease => InputCreatorModel.PMDGMouseEventsRelease;

    public string Event
    {
        get => InputCreatorModel.Event;
        set
        {
            InputCreatorModel.Event = value;
            OnPropertyChanged(nameof(Event));
        }
    }

    public uint? EventDataPress
    {
        get => InputCreatorModel.EventDataPress;
        set
        {
            InputCreatorModel.EventDataPress = value;
            OnPropertyChanged(nameof(EventDataPress));
        }
    }

    public uint? EventDataRelease
    {
        get => InputCreatorModel.EventDataRelease;
        set
        {
            InputCreatorModel.EventDataRelease = value;
            OnPropertyChanged(nameof(EventDataRelease));
        }
    }

    [RelayCommand]
    private void GetSwitch()
    {
        int key;
        while (InterfaceITAPI_Data.interfaceIT_Switch_Get_Item(1, out key, out _) == 0) ;
        //ErrorText = $"{ProfileCreatorModel.DeviceName} | Session: {Devices.FirstOrDefault(k => k.BoardType == ProfileCreatorModel.DeviceName).Session} | Switch: {key} => {direction}";
        SelectedSwitch = key;
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