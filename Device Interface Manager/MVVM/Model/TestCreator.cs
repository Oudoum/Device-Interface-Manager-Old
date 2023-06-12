using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using System.Runtime.InteropServices;
using CommunityToolkit.Mvvm.ComponentModel;
using Device_Interface_Manager.MSFSProfiles.PMDG;

namespace Device_Interface_Manager.MVVM.Model;
public partial class TestCreator : ObservableObject
{
    public const string PMDG737 = "PMDG737";
    public const string H = "H";
    public const string MSFSSimConnect = "MSFS/SimConnect/LVar";

    public string ProfileName { get; set; }

    public string BoardType { get; set; }

    [ObservableProperty]
    private ObservableCollection<InputCreator> _inputCreator = new();

    public List<int?> AllSwitchItems { get; set; } = new();
    public List<int?> RemovedSwitchItems { get; set; } = new();

    [ObservableProperty]
    private ObservableCollection<OutputCreator> _outputCreator = new();

    public List<int?> AllLEDItems { get; set; } = new();
    public List<int?> RemovedLEDsItems { get; set; } = new();
}

public partial class InputCreator : ObservableObject
{
    public int? SelectedSwitch { get; set; }

    [ObservableProperty]
    private ObservableCollection<int?> _switches;

    [ObservableProperty]
    private string _selectedEventType = TestCreator.PMDG737;

    [JsonIgnore]
    public string[] EventType { get; set; } = new string[] { TestCreator.MSFSSimConnect, TestCreator.H, TestCreator.PMDG737 };

    [ObservableProperty]
    private string _event; // MSFS/SimConnect...

    [ObservableProperty]
    private PMDG_NG3_SDK.PMDGEvents? _pMDGEvent;

    [ObservableProperty]
    [property: JsonIgnore]
    [NotifyPropertyChangedFor(nameof(PMDGEvents))]
    private string _searchPMDGEventsText;

    [JsonIgnore]
    public PMDG_NG3_SDK.PMDGEvents[] PMDGEvents => string.IsNullOrEmpty(SearchPMDGEventsText)
        ? (PMDG_NG3_SDK.PMDGEvents[])Enum.GetValues(typeof(PMDG_NG3_SDK.PMDGEvents))
        : ((PMDG_NG3_SDK.PMDGEvents[])Enum.GetValues(typeof(PMDG_NG3_SDK.PMDGEvents))).Where(name => name.ToString().Contains(SearchPMDGEventsText, StringComparison.OrdinalIgnoreCase)).ToArray();

    [ObservableProperty]
    private KeyValuePair<string, uint>? _pMDGMouseEventPress;

    [ObservableProperty]
    [property: JsonIgnore]
    [NotifyPropertyChangedFor(nameof(PMDGMouseEventsPress))]
    private string _searchPMDGMouseEventsTextPress;

    [JsonIgnore]
    public KeyValuePair<string, uint>[] PMDGMouseEventsPress => string.IsNullOrEmpty(SearchPMDGMouseEventsTextPress)
    ? PMDGMouseFlags.ToArray()
    : PMDGMouseFlags.Where(pair => pair.Key.Contains(SearchPMDGMouseEventsTextPress, StringComparison.OrdinalIgnoreCase)).ToArray();

    [ObservableProperty]
    private KeyValuePair<string, uint>? _pMDGMouseEventRelease;

    [ObservableProperty]
    [property: JsonIgnore]
    [NotifyPropertyChangedFor(nameof(PMDGMouseEventsRelease))]
    private string _searchPMDGMouseEventsTextRelease;

    [JsonIgnore]
    public KeyValuePair<string, uint>[] PMDGMouseEventsRelease => string.IsNullOrEmpty(SearchPMDGMouseEventsTextRelease)
    ? PMDGMouseFlags.ToArray()
    : PMDGMouseFlags.Where(pair => pair.Key.Contains(SearchPMDGMouseEventsTextRelease, StringComparison.OrdinalIgnoreCase)).ToArray();

    [ObservableProperty]
    private uint? _pMDGEventDataPress;

    [ObservableProperty]
    private uint? _pMDGEventDataRelease;

    [JsonIgnore]
    private static Dictionary<string, uint> PMDGMouseFlags => new()
    {
        { "RIGHTSINGLE", PMDG_NG3_SDK.MOUSE_FLAG_RIGHTSINGLE },
        { "MIDDLESINGLE", PMDG_NG3_SDK.MOUSE_FLAG_MIDDLESINGLE },
        { "LEFTSINGLE", PMDG_NG3_SDK.MOUSE_FLAG_LEFTSINGLE },
        { "RIGHTDOUBLE", PMDG_NG3_SDK.MOUSE_FLAG_RIGHTDOUBLE },
        { "MIDDLEDOUBLE", PMDG_NG3_SDK.MOUSE_FLAG_MIDDLEDOUBLE },
        { "LEFTDOUBLE", PMDG_NG3_SDK.MOUSE_FLAG_LEFTDOUBLE },
        { "RIGHTDRAG", PMDG_NG3_SDK.MOUSE_FLAG_RIGHTDRAG },
        { "MIDDLEDRAG", PMDG_NG3_SDK.MOUSE_FLAG_MIDDLEDRAG },
        { "LEFTDRAG", PMDG_NG3_SDK.MOUSE_FLAG_LEFTDRAG },
        { "MOVE", PMDG_NG3_SDK.MOUSE_FLAG_MOVE },
        { "DOWN_REPEAT", PMDG_NG3_SDK.MOUSE_FLAG_DOWN_REPEAT },
        { "RIGHTRELEASE", PMDG_NG3_SDK.MOUSE_FLAG_RIGHTRELEASE },
        { "MIDDLERELEASE", PMDG_NG3_SDK.MOUSE_FLAG_MIDDLERELEASE },
        { "LEFTRELEASE", PMDG_NG3_SDK.MOUSE_FLAG_LEFTRELEASE },
        { "WHEEL_FLIP", PMDG_NG3_SDK.MOUSE_FLAG_WHEEL_FLIP },
        { "WHEEL_SKIP", PMDG_NG3_SDK.MOUSE_FLAG_WHEEL_SKIP },
        { "WHEEL_UP", PMDG_NG3_SDK.MOUSE_FLAG_WHEEL_UP },
        { "WHEEL_DOWN", PMDG_NG3_SDK.MOUSE_FLAG_WHEEL_DOWN }
    };
}

public partial class OutputCreator : ObservableObject
{
    public int? SelectedLED { get; set; }

    [ObservableProperty]
    private ObservableCollection<int?> _lEDs;

    [ObservableProperty]
    private string _selectedDataType = TestCreator.PMDG737;

    [JsonIgnore]
    public string[] DataType { get; set; } = new string[] { TestCreator.MSFSSimConnect, TestCreator.PMDG737 };

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PMDGStructArrayNums))]
    [NotifyPropertyChangedFor(nameof(PMDGStructArrayNum))]
    public string _pMDGDataFieldName;

    [ObservableProperty]
    [property: JsonIgnore]
    [NotifyPropertyChangedFor(nameof(PMDGDataFieldNames))]
    private string _searchPMDGDataText;

    [JsonIgnore]
    public string[] PMDGDataFieldNames => string.IsNullOrEmpty(SearchPMDGDataText)
    ? typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetFields().Select(field => field.Name).Take(typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetFields().Length - 1).ToArray()
    : typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetFields().Select(field => field.Name).Where(name => name.Contains(SearchPMDGDataText, StringComparison.OrdinalIgnoreCase)).Take(typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetFields().Length - 1).ToArray();

    private int? _pMDGStructArrayNum;
    public int? PMDGStructArrayNum
    {
        get
        {
            if (_pMDGStructArrayNum is null)
            {
                return _pMDGStructArrayNum = PMDGStructArrayNums?.FirstOrDefault();

            }
            return _pMDGStructArrayNum;
        }
        set
        {
            if (value != _pMDGStructArrayNum)
            { 
                _pMDGStructArrayNum = value;
            }
        }
    }

    [JsonIgnore]
    public int?[] PMDGStructArrayNums => string.IsNullOrEmpty(PMDGDataFieldName)
    ? Array.Empty<int?>()
    : typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetField(PMDGDataFieldName).GetCustomAttribute<MarshalAsAttribute>()?.SizeConst is int size
    ? new int?[size].Select((_, i) => i).Cast<int?>().ToArray()
    : null;

    [ObservableProperty]
    private string _data;

    public bool IsInverted { get; set; }
}

//7Segement

//Brightness

//Analog