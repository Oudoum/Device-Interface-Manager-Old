using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using Device_Interface_Manager.MSFSProfiles.PMDG;

namespace Device_Interface_Manager.MVVM.Model;

public partial class ProfileCreatorModel : ObservableObject
{
    //Drivers
    public const string FDSUSB = "FDS USB";
    public static string[] Drivers => new string[] { FDSUSB };

    //Data-/EventTypes
    public const string PMDG737 = "PMDG737";
    public const string MSFSSimConnect = "MSFS/SimConnect/LVar";
    public const string RPN = "RPN/H-Events";

    public string ProfileName { get; set; }

    public string SelectedDriver { get; set; }

    public string DeviceName { get; set; }

    [ObservableProperty]
    private ObservableCollection<InputCreator> _inputCreator = new();

    [JsonIgnore]
    public List<int?> Switches { get; set; } = new();

    [JsonIgnore]
    public List<int?> UsedSwitches { get; set; } = new();

    [ObservableProperty]
    private ObservableCollection<OutputCreator> _outputCreator = new();

    [JsonIgnore]
    public List<int?> LEDs { get; set; } = new();

    [JsonIgnore]
    public List<int?> UsedLEDs { get; set; } = new();
}

public partial class InputCreator : ObservableObject
{
    [ObservableProperty]
    public int? _selectedSwitch;

    [ObservableProperty]
    private string _selectedEventType;

    [ObservableProperty]
    private PMDG_NG3_SDK.PMDGEvents? _pMDGEvent;

    [ObservableProperty]
    private string _event;

    [ObservableProperty]
    private KeyValuePair<string, uint>? _pMDGMouseEventPress;

    [ObservableProperty]
    private KeyValuePair<string, uint>? _pMDGMouseEventRelease;

    [ObservableProperty]
    private uint? _EventDataPress;

    [ObservableProperty]
    private uint? _EventDataRelease;

    public InputCreator Clone()
    {
        return MemberwiseClone() as InputCreator;
    }
}

public partial class OutputCreator : ObservableObject
{
    [ObservableProperty]
    private int? _selectedLED;

    [ObservableProperty]
    private string _selectedDataType;

    [ObservableProperty]
    public string _pMDGDataFieldName;

    [ObservableProperty]
    private string _data;

    [ObservableProperty]
    private int? _pMDGStructArrayNum;

    [ObservableProperty]
    private int? _comparisonValue;

    [ObservableProperty]
    private bool _isInverted;

    public OutputCreator Clone()
    {
        return MemberwiseClone() as OutputCreator;
    }
}

//7Segement

//Brightness

//Dataline

//Analog