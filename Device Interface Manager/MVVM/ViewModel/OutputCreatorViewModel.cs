using System;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Device_Interface_Manager.Core;
using Device_Interface_Manager.interfaceIT.USB;
using Device_Interface_Manager.MVVM.Model;

namespace Device_Interface_Manager.MVVM.ViewModel;
public partial class OutputCreatorViewModel : ObservableObject, ICloseWindows
{
    public OutputCreatorModel OutputCreatorModel { get; set; }

    public Action Close { get; set; }

    public InterfaceIT_BoardInfo.Device Device { get; set; }

    public int? SelectedLED
    {
        get => OutputCreatorModel.SelectedLED;
        set
        {
            OutputCreatorModel.SelectedLED = value;
            OnPropertyChanged(nameof(SelectedLED));
        }
    }

    public int?[] LEDs => OutputCreatorModel.LEDs;

    public string SelectedDataType
    {
        get => OutputCreatorModel.SelectedDataType;
        set
        {
            OutputCreatorModel.SelectedDataType = value;
            OnPropertyChanged(nameof(SelectedDataType));
            OnPropertyChanged(nameof(PMDGDataFieldName));
            OnPropertyChanged(nameof(PMDGStructArrayNums));
            OnPropertyChanged(nameof(PMDGStructArrayNum));
            OnPropertyChanged(nameof(IsComparisonValueEnabled));
            OnPropertyChanged(nameof(ComparisonValue));
        }
    }

    public string[] DataType => OutputCreatorModel.DataType;

    public string PMDGDataFieldName
    {
        get => OutputCreatorModel.PMDGDataFieldName;
        set
        {
            OutputCreatorModel.PMDGDataFieldName = value;
            OnPropertyChanged(nameof(PMDGDataFieldName));
            OnPropertyChanged(nameof(PMDGStructArrayNums));
            OnPropertyChanged(nameof(PMDGStructArrayNum));
            OnPropertyChanged(nameof(IsComparisonValueEnabled));
            OnPropertyChanged(nameof(ComparisonValue));
        }
    }

    public string SearchPMDGDataText
    {
        get => OutputCreatorModel.SearchPMDGDataText;
        set
        {
            OutputCreatorModel.SearchPMDGDataText = value;
            OnPropertyChanged(nameof(PMDGDataFieldNames));
            OnPropertyChanged(nameof(PMDGDataFieldName));
        }
    }

    public string[] PMDGDataFieldNames => OutputCreatorModel.PMDGDataFieldNames;

    public int? PMDGStructArrayNum
    {
        get => OutputCreatorModel.PMDGStructArrayNum;
        set
        {
            OutputCreatorModel.PMDGStructArrayNum = value;
            OnPropertyChanged(nameof(PMDGStructArrayNum));
        }
    }

    public int?[] PMDGStructArrayNums => OutputCreatorModel.PMDGStructArrayNums;

    public bool IsComparisonValueEnabled => OutputCreatorModel.IsComparisonValueEnabled;

    public int? ComparisonValue
    {
        get => OutputCreatorModel.ComparisonValue;
        set => OutputCreatorModel.ComparisonValue = value;
    }

    public string Data
    {
        get => OutputCreatorModel.Data;
        set
        {
            OutputCreatorModel.Data = value;
            OnPropertyChanged(nameof(Data));
        }
    }

    public bool IsInverted
    {
        get => OutputCreatorModel.IsInverted;
        set => OutputCreatorModel.IsInverted = value;
    }




    private int? lEDPosition;

    private void SetLEDPosition(int? value)
    {
        //DISABLE FOR DEBUG
        if (lEDPosition != value)
        {
            if (lEDPosition is not null)
            {
                InterfaceITAPI_Data.interfaceIT_LED_Set(Device.Session, lEDPosition.Value, false);
                //ErrorText = $"{ProfileCreatorModel.DeviceName} | Session: {Devices.FirstOrDefault(k => k.BoardType == ProfileCreatorModel.DeviceName).Session} | LED: {_lEDPosition.Value} => false";
            }
            if (value is not null)
            {
                InterfaceITAPI_Data.interfaceIT_LED_Set(Device.Session, value.Value, true);
                //ErrorText = $"{ProfileCreatorModel.DeviceName} | Session: {Devices.FirstOrDefault(k => k.BoardType == ProfileCreatorModel.DeviceName).Session} | LED: {value.Value} => true";
            }
            lEDPosition = value;
        }
        //DISABLE FOR DEBUG
    }

    [RelayCommand]
    private void MouseMoveLEDComboBox(System.Windows.Input.MouseEventArgs e)
    {
        if (e.OriginalSource is TextBlock textBlock)
        {
            string text = textBlock.Text;
            if (text == "r")
            {
                return;
            }
            SetLEDPosition(int.Parse(text));
            return;
        }
        else if (e.OriginalSource is Grid grid)
        {
            SetLEDPosition(((OutputCreatorViewModel)grid.DataContext).SelectedLED);
            return;
        }
        SetLEDPosition(null);
    }

    [RelayCommand]
    private void MouseLeaveLEDComboBox(System.Windows.Input.MouseEventArgs e)
    {
        if (e.OriginalSource is ComboBox)
        {
            SetLEDPosition(null);
        }
    }

    [RelayCommand]
    private void LEDComboBoxSlectionChanged(SelectionChangedEventArgs e)
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
        //    foreach (var item in TestCreator.OutputCreator)
        //    {
        //        if (item.SelectedLED != addedValue)
        //        {
        //            item.LEDs.Remove(addedValue);
        //        }
        //    }

        //    if (!TestCreator.RemovedLEDsItems.Contains(addedValue))
        //    {
        //        TestCreator.RemovedLEDsItems.Add(addedValue);
        //    }
        //    return;
        //}

        //TestCreator.RemovedLEDsItems.Remove(removedValue);

        //TestCreator.OutputCreator
        //    .Where(item => !item.LEDs.Contains(removedValue))
        //    .ToList()
        //    .ForEach(item =>
        //    {
        //        List<int?> leds = new(item.LEDs)
        //        {
        //            removedValue
        //        };
        //        item.LEDs = new(leds.OrderBy(i => i));
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