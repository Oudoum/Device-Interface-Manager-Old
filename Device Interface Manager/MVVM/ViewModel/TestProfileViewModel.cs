using System;
using System.Windows;
using System.Linq;
using System.Dynamic;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Device_Interface_Manager.MSFSProfiles.PMDG;
using Device_Interface_Manager.MVVM.Model;

namespace Device_Interface_Manager.MVVM.ViewModel;
public partial class TestProfileViewModel : ObservableObject
{

    public ObservableCollection<TestCreator> TestProfiles { get; set; } = new();

    public ObservableCollection<InputCreator> InputProfiles { get; set; } = new();
    public ObservableCollection<OutputCreator> OutputProfiles { get; set; } = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PMDGEvents))]
    private string _searchPMDGEventsText;

    public PMDG_NG3_SDK.PMDGEvents[] PMDGEvents => string.IsNullOrEmpty(SearchPMDGEventsText)
        ? (PMDG_NG3_SDK.PMDGEvents[])Enum.GetValues(typeof(PMDG_NG3_SDK.PMDGEvents))
        : ((PMDG_NG3_SDK.PMDGEvents[])Enum.GetValues(typeof(PMDG_NG3_SDK.PMDGEvents))).Where(name => name.ToString().Contains(SearchPMDGEventsText, StringComparison.OrdinalIgnoreCase)).ToArray();


    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PMDGMouseEvents))]
    private string _searchPMDGMouseEventsText;

    public static Dictionary<string, uint> PMDGMouseFlags => new()
        {
        { nameof(PMDG_NG3_SDK.MOUSE_FLAG_RIGHTSINGLE), PMDG_NG3_SDK.MOUSE_FLAG_RIGHTSINGLE },
        { nameof(PMDG_NG3_SDK.MOUSE_FLAG_MIDDLESINGLE), PMDG_NG3_SDK.MOUSE_FLAG_MIDDLESINGLE },
        { nameof(PMDG_NG3_SDK.MOUSE_FLAG_LEFTSINGLE), PMDG_NG3_SDK.MOUSE_FLAG_LEFTSINGLE },
        { nameof(PMDG_NG3_SDK.MOUSE_FLAG_RIGHTDOUBLE), PMDG_NG3_SDK.MOUSE_FLAG_RIGHTDOUBLE },
        { nameof(PMDG_NG3_SDK.MOUSE_FLAG_MIDDLEDOUBLE), PMDG_NG3_SDK.MOUSE_FLAG_MIDDLEDOUBLE },
        { nameof(PMDG_NG3_SDK.MOUSE_FLAG_LEFTDOUBLE), PMDG_NG3_SDK.MOUSE_FLAG_LEFTDOUBLE },
        { nameof(PMDG_NG3_SDK.MOUSE_FLAG_RIGHTDRAG), PMDG_NG3_SDK.MOUSE_FLAG_RIGHTDRAG },
        { nameof(PMDG_NG3_SDK.MOUSE_FLAG_MIDDLEDRAG), PMDG_NG3_SDK.MOUSE_FLAG_MIDDLEDRAG },
        { nameof(PMDG_NG3_SDK.MOUSE_FLAG_LEFTDRAG), PMDG_NG3_SDK.MOUSE_FLAG_LEFTDRAG },
        { nameof(PMDG_NG3_SDK.MOUSE_FLAG_MOVE), PMDG_NG3_SDK.MOUSE_FLAG_MOVE },
        { nameof(PMDG_NG3_SDK.MOUSE_FLAG_DOWN_REPEAT), PMDG_NG3_SDK.MOUSE_FLAG_DOWN_REPEAT },
        { nameof(PMDG_NG3_SDK.MOUSE_FLAG_RIGHTRELEASE), PMDG_NG3_SDK.MOUSE_FLAG_RIGHTRELEASE },
        { nameof(PMDG_NG3_SDK.MOUSE_FLAG_MIDDLERELEASE), PMDG_NG3_SDK.MOUSE_FLAG_MIDDLERELEASE },
        { nameof(PMDG_NG3_SDK.MOUSE_FLAG_LEFTRELEASE), PMDG_NG3_SDK.MOUSE_FLAG_LEFTRELEASE },
        { nameof(PMDG_NG3_SDK.MOUSE_FLAG_WHEEL_FLIP), PMDG_NG3_SDK.MOUSE_FLAG_WHEEL_FLIP },
        { nameof(PMDG_NG3_SDK.MOUSE_FLAG_WHEEL_SKIP), PMDG_NG3_SDK.MOUSE_FLAG_WHEEL_SKIP },
        { nameof(PMDG_NG3_SDK.MOUSE_FLAG_WHEEL_UP), PMDG_NG3_SDK.MOUSE_FLAG_WHEEL_UP },
        { nameof(PMDG_NG3_SDK.MOUSE_FLAG_WHEEL_DOWN), PMDG_NG3_SDK.MOUSE_FLAG_WHEEL_DOWN }
        };

    public KeyValuePair<string, uint>[] PMDGMouseEvents => string.IsNullOrEmpty(SearchPMDGMouseEventsText)
    ? PMDGMouseFlags.ToArray()
    : PMDGMouseFlags.Where(pair => pair.Key.Contains(SearchPMDGMouseEventsText, StringComparison.OrdinalIgnoreCase)).ToArray();


    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PMDGDataFieldNames))]
    private string _searchPMDGDataText;

    public string[] PMDGDataFieldNames => string.IsNullOrEmpty(SearchPMDGDataText)
    ? typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetFields().Select(field => field.Name).Take(typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetFields().Length - 1).ToArray()
    : typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetFields().Select(field => field.Name).Where(name => name.Contains(SearchPMDGDataText, StringComparison.OrdinalIgnoreCase)).Take(typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetFields().Length - 1).ToArray();


    [RelayCommand]
    private void AddInput()
    {
        InputProfiles.Add(new InputCreator { Switches = new ObservableCollection<Switch>() { new Switch { Id = 0, Position = 1 }, new Switch { Id = 1, Position = 2 } }, Event = "TEST", EventType = new EventType(), PMDGEvent = PMDG_NG3_SDK.PMDGEvents.EVT_OH_ELEC_BATTERY_SWITCH, PMDGMouseEvent = PMDGMouseEvents });
    }

    [RelayCommand]
    private void AddOutput()
    {
        OutputProfiles.Add(new OutputCreator { LEDs = new ObservableCollection<LED>() { new LED { Id = 0, Position = 1 }, new LED { Id = 1, Position = 2 } }, Data = "TEST", DataType = new DataType(), PMDGStructArrayNum = 0, PMDGStructData = PMDGDataFieldNames });
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


    public event EventHandler<PMDGDataFieldChangedEventArgs> FieldChanged;

    public List<string> WatchedFields { get; set; } = new() { "ADF_StandbyFrequency", "IRS_annunALIGN_0" };

    public TestProfileViewModel()
    {
        FieldChanged += Instance_FieldChanged;
        //Initialize
        PMDG_NG3_SDK.PMDG_NG3_Data data = new();

        dynamic dynObject = new ExpandoObject();
        IDictionary<string, object> dynDict = (IDictionary<string, object>)dynObject;

        foreach (var field in typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetFields())
        {
            if (field.Name == "reserved")
            {
                continue;
            }

            if (field.FieldType.IsArray)
            {
                Array array = (Array)field.GetValue(data);
                if (array is null)
                {
                    if (field.GetCustomAttributes(typeof(MarshalAsAttribute), false).FirstOrDefault() is MarshalAsAttribute marshalAsAttribute)
                    {
                        array = Array.CreateInstance(field.FieldType.GetElementType(), marshalAsAttribute.SizeConst);
                        field.SetValue(data, array);
                    }
                }

                if (array is not null)
                {
                    int i = 0;
                    foreach (var item in array)
                    {
                        dynDict[field.Name + '_' + i] = item;
                        i++;
                    }
                }
            }

            dynDict[field.Name] = field.GetValue(data);
        }


        //Update Data
        PMDG_NG3_SDK.PMDG_NG3_Data newData = new()
        {
            ADF_StandbyFrequency = 20,
            IRS_annunALIGN = new bool[2] { true, false}
        };

        foreach (var field in typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetFields())
        {
            if (field.Name == "reserved")
            {
                continue;
            }

            string propertyName;
            object oldValue;
            object newValue;
            if (field.FieldType.IsArray)
            {
                if (field.GetValue(newData) is Array array)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        propertyName = field.Name + '_' + i;
                        if (!dynDict.TryGetValue(propertyName, out oldValue))
                        {
                            oldValue = null;
                        }
                        newValue = array.GetValue(i);
                        if (!Equals(oldValue, newValue) && WatchedFields.Contains(propertyName))
                        {
                            dynDict[propertyName] = newValue;
                            // Value has changed, raise an event or call a method here
                            FieldChanged?.Invoke(null, new PMDGDataFieldChangedEventArgs(propertyName, newValue));
                        }
                    }
                }
                continue;
            }

            propertyName = field.Name;
            if (!dynDict.TryGetValue(propertyName, out oldValue))
            {
                oldValue = null;
            }

            newValue = field.GetValue(newData);
            if (!Equals(oldValue, newValue) && WatchedFields.Contains(propertyName))
            {
                dynDict[propertyName] = newValue;
                // Value has changed, raise an event or call a method here
                FieldChanged?.Invoke(null, new PMDGDataFieldChangedEventArgs(propertyName, newValue));
            }
        }
    }

    private void Instance_FieldChanged(object sender, PMDGDataFieldChangedEventArgs e)
    {
        switch (e.Value)
        {
            case bool boolValue:
                Logic(Convert.ToInt32(boolValue), e.PMDGDataName);
                break;

            case byte byteValue:
                Logic(Convert.ToInt32(byteValue), e.PMDGDataName);
                break;

            case ushort ushortValue:
                Logic(Convert.ToInt32(ushortValue), e.PMDGDataName);
                break;

            case short shortValue:
                Logic(Convert.ToInt32(shortValue), e.PMDGDataName);
                break;

            case uint uintValue:
                Logic(Convert.ToInt32(uintValue), e.PMDGDataName);
                break;

            case int intValue:
                Logic(intValue, e.PMDGDataName);
                break;

            case string stringValue:
                Logic(stringValue, e.PMDGDataName);
                break;

            case float floatValue:
                Logic(Convert.ToSingle(floatValue), e.PMDGDataName);
                break;

            default:
                throw new Exception(e.PMDGDataName);
        }
    }

    private void Logic<T>(T value, string pMDGDataName)
    {

    }
}

public class PMDGDataFieldChangedEventArgs : EventArgs
{
    public string PMDGDataName { get; }
    public object Value { get; }

    public PMDGDataFieldChangedEventArgs(string propertyName, object newValue)
    {
        PMDGDataName = propertyName;
        Value = newValue;
    }
}