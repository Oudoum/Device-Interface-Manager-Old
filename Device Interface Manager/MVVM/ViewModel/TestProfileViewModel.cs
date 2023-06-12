using System;
using System.Linq;
using System.Windows;
using System.Text.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Device_Interface_Manager.MVVM.Model;
using Device_Interface_Manager.interfaceIT.USB;
using Device_Interface_Manager.MSFSProfiles.PMDG;

namespace Device_Interface_Manager.MVVM.ViewModel;
public partial class TestProfileViewModel : ObservableObject
{
    [ObservableProperty]
    private TestCreator _TestCreator;

    [ObservableProperty]
    private ObservableCollection<TestCreator> _testCreatorCollection = new();

    [ObservableProperty]
    private string _boardType;

    partial void OnBoardTypeChanged(string value)
    {
        TestCreator = TestCreatorCollection.FirstOrDefault(item => item.BoardType == value);
    }

    public ObservableCollection<string> BoardTypeCollection { get; set; } = new();

    private ObservableCollection<InterfaceIT_BoardInfo.Device> _deviceList = new();
    public ObservableCollection<InterfaceIT_BoardInfo.Device> DeviceList
    {
        get => _deviceList;
        set
        {
            _deviceList = value;
            foreach (var device in value)
            {
                TestCreator testCreator = new();
                for (int i = device.DeviceInfo.nSwitchFirst; i <= device.DeviceInfo.nSwitchLast; i++)
                {
                    testCreator.AllSwitchItems.Add(i);
                }
                for (int i = device.DeviceInfo.nLEDFirst; i <= device.DeviceInfo.nLEDLast; i++)
                {
                    testCreator.AllLEDItems.Add(i);
                }
                BoardTypeCollection.Add(testCreator.BoardType = device.BoardType);
                TestCreatorCollection.Add(testCreator);
            }
        }
    }

    private List<string> test = new();
    [RelayCommand]
    private void SaveProfiles()
    {
        test?.Clear();
        foreach (var profile in TestCreatorCollection)
        {
            test.Add(JsonSerializer.Serialize(profile, new JsonSerializerOptions { WriteIndented = true }));
        }
    }

    [RelayCommand]
    private void LoadProfiles()
    {
        TestCreatorCollection.Clear();
        if (test is null)
        {
            return;
        }
        foreach (var item in test)
        {
            TestCreatorCollection.Add(JsonSerializer.Deserialize<TestCreator>(item));
        }
        TestCreator = TestCreatorCollection.FirstOrDefault(item => item.BoardType == BoardType);
    }

    [RelayCommand]
    private void AddInput()
    {
        if (TestCreator is null)
        {
            return;
        }

        List<int?> switchItems = new(TestCreator.AllSwitchItems);
        foreach (var item in TestCreator.RemovedSwitchItems)
        {
            switchItems.Remove(item);
        }
        TestCreator.InputCreator.Add(new InputCreator { Switches = new ObservableCollection<int?>(switchItems) });
    }

    [RelayCommand]
    private void AddOutput()
    {
        if (TestCreator is null)
        {
            return;
        }

        List<int?> lEDsItems = new(TestCreator.AllLEDItems);
        foreach (var item in TestCreator.RemovedLEDsItems)
        {
            lEDsItems.Remove(item);
        }
        TestCreator.OutputCreator.Add(new OutputCreator { LEDs = new ObservableCollection<int?>(lEDsItems) });
    }

    [RelayCommand]
    private void DeleteRow(object creator)
    {
        if (creator is InputCreator)
        {
            foreach (var input in TestCreator.InputCreator)
            {
                if (input == creator)
                {
                    TestCreator.InputCreator.Remove(input);
                    if (input.SelectedSwitch is not null)
                    {
                        TestCreator.RemovedSwitchItems.Remove(input.SelectedSwitch);
                        TestCreator.InputCreator
                            .ToList()
                            .ForEach(item =>
                            {
                                List<int?> switches = new(item.Switches)
                                {
                                input.SelectedSwitch
                                };
                                item.Switches = new(switches.OrderBy(i => i));
                            });
                    }
                    return;
                }
            }
        }
        else if (creator is OutputCreator)
        {
            foreach (var output in TestCreator.OutputCreator)
            {
                if (output == creator)
                {
                    TestCreator.OutputCreator.Remove(output);
                    if (output.SelectedLED is not null)
                    {
                        TestCreator.RemovedLEDsItems.Remove(output.SelectedLED);
                        TestCreator.OutputCreator
                            .ToList()
                            .ForEach(item =>
                            {
                                List<int?> switches = new(item.LEDs)
                                {
                                output.SelectedLED
                                };
                                item.LEDs = new(switches.OrderBy(i => i));
                            });
                    }
                    return;
                }
            }
        }

    }

    [RelayCommand]
    private void StartProfiles()
    {
        PMDGProfile pMDGProfile = new(TestCreatorCollection.ToArray(), DeviceList.ToArray());
    }

    [RelayCommand]
    private void ComboBoxGotFocus(RoutedEventArgs e)
    {
        if (e is not null)
        {
            if (e.Source is System.Windows.Controls.ComboBox comboBox)
            {
                comboBox.Dispatcher.BeginInvoke(new Action(() => comboBox.IsDropDownOpen = true));
            }
        }
    }

    [RelayCommand]
    private void SwitchComboBoxSlectionChanged(System.Windows.Controls.SelectionChangedEventArgs e)
    {
        int? addedValue = null;
        if (e.AddedItems.Count > 0)
        {
            addedValue = (int?)e.AddedItems[0];
        }

        int? removedValue = null;
        if (e.RemovedItems.Count > 0)
        {
            removedValue = (int?)e.RemovedItems[0];
        }

        if (addedValue is not null)
        {
            foreach (var item in TestCreator.InputCreator)
            {
                if (item.SelectedSwitch != addedValue)
                {
                    item.Switches.Remove(addedValue);
                }
            }

            if (!TestCreator.RemovedSwitchItems.Contains(addedValue))
            {
                TestCreator.RemovedSwitchItems.Add(addedValue);
            }
            return;
        }

        TestCreator.RemovedSwitchItems.Remove(removedValue);

        TestCreator.InputCreator
            .Where(item => !item.Switches.Contains(removedValue))
            .ToList()
            .ForEach(item =>
            {
                List<int?> switches = new(item.Switches)
                {
                    removedValue
                };
                item.Switches = new(switches.OrderBy(i => i));
            });
    }

    [RelayCommand]
    private void LEDComboBoxSlectionChanged(System.Windows.Controls.SelectionChangedEventArgs e)
    {
        int? addedValue = null;
        if (e.AddedItems.Count > 0)
        {
            addedValue = (int?)e.AddedItems[0];
        }

        int? removedValue = null;
        if (e.RemovedItems.Count > 0)
        {
            removedValue = (int?)e.RemovedItems[0];
        }

        if (addedValue is not null)
        {
            foreach (var item in TestCreator.OutputCreator)
            {
                if (item.SelectedLED != addedValue)
                {
                    item.LEDs.Remove(addedValue);
                }
            }

            if (!TestCreator.RemovedLEDsItems.Contains(addedValue))
            {
                TestCreator.RemovedLEDsItems.Add(addedValue);
            }
            return;
        }

        TestCreator.RemovedLEDsItems.Remove(removedValue);

        TestCreator.OutputCreator
            .Where(item => !item.LEDs.Contains(removedValue))
            .ToList()
            .ForEach(item =>
            {
                List<int?> leds = new(item.LEDs)
                {
                    removedValue
                };
                item.LEDs = new(leds.OrderBy(i => i));
            });
    }

}