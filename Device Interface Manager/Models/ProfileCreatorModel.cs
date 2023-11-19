using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using Device_Interface_Manager.SimConnectProfiles.PMDG;

namespace Device_Interface_Manager.Models;

public partial class ProfileCreatorModel : ObservableObject
{
    //Drivers
    public const string FDSUSB = "FDS USB";
    public const string FDSENET = "FDS E-Series";
    public const string CPflightUSB = "CPflight USB";
    public const string CPflightENET = "CPflight ENET";
    public const string HID = "HID";
    public const string Arduino = "Arduino";
    public const string SIOC = "SIOC";

    public static string[] Drivers => new string[] { FDSUSB, FDSENET, CPflightUSB, CPflightENET, HID, Arduino, SIOC };

    //Data-/EventTypes
    public const string PMDG737 = "PMDG737";
    public const string KEVENT = "K:Event";
    public const string MSFSSimConnect = "MSFS/SimConnect/LVar";
    public const string RPN = "RPN/H-Events";

    //Inputs
    public const string SWITCH = "Switch";

    //Outputs
    public const string LED = "LED";
    public const string SEVENSEGMENT = "7 Segment";
    public const string DATALINE = "Dataline";

    public string ProfileName { get; set; }

    public string Driver { get; set; }

    public string DeviceName { get; set; }

    [ObservableProperty]
    private ObservableCollection<InputCreator> _inputCreator = new();

    [ObservableProperty]
    private ObservableCollection<OutputCreator> _outputCreator = new();
}

public partial class InputCreator : ObservableObject
{
    [ObservableProperty]
    private Guid _id;

    [ObservableProperty]
    private bool _isActive;

    [ObservableProperty]
    private string _description;

    [ObservableProperty]
    private string _inputType;

    [ObservableProperty]
    private KeyValuePair<string, string>? _input;

    [ObservableProperty]
    private string _eventType;

    [ObservableProperty]
    private PMDG_NG3_SDK.PMDGEvents? _pMDGEvent;

    [ObservableProperty]
    private string _event;

    [ObservableProperty]
    private bool _onRelease;

    [ObservableProperty]
    private KeyValuePair<string, uint>? _pMDGMousePress;

    [ObservableProperty]
    private KeyValuePair<string, uint>? _pMDGMouseRelease;

    [ObservableProperty]
    private uint? _dataPress;

    [ObservableProperty]
    private uint? _dataRelease;

    [ObservableProperty]
    private Precondition[] _preconditions = Array.Empty<Precondition>();

    public InputCreator Clone()
    {
        var clone = MemberwiseClone() as InputCreator;
        clone.Id = Guid.NewGuid();
        return clone;
    }
}

public partial class OutputCreator : ObservableObject
{
    [ObservableProperty]
    private Guid _id;

    [ObservableProperty]
    private bool _isActive;

    [ObservableProperty]
    private string _description;

    [ObservableProperty]
    private string _outputType;

    [ObservableProperty]
    private KeyValuePair<string, string>? _output;

    [ObservableProperty]
    private string _dataType;

    [ObservableProperty]
    private string _pMDGData;

    [ObservableProperty]
    private string _data;

    [ObservableProperty]
    private string _unit;

    [ObservableProperty]
    private int? _pMDGDataArrayIndex;

    [ObservableProperty]
    private char? _operator;

    [ObservableProperty]
    private string _comparisonValue;

    [ObservableProperty]
    private double? _trueValue;

    [ObservableProperty]
    private double? _falseValue;

    [ObservableProperty]
    private bool _isInverted;

    [ObservableProperty]
    private bool? _isPadded;

    [ObservableProperty]
    private char? _paddingCharacter;

    [ObservableProperty]
    private byte? _digitCount;

    [ObservableProperty]
    private byte? _digitCheckedSum;

    [ObservableProperty]
    private byte? _decimalPointCheckedSum;

    [ObservableProperty]
    private byte? _substringStart;

    [ObservableProperty]
    private byte? _substringEnd;

    [ObservableProperty]
    [property: JsonIgnore]
    private object _flightSimValue;

    [ObservableProperty]
    [property: JsonIgnore]
    private object _outputValue;

    [ObservableProperty]
    private Precondition[] _preconditions = Array.Empty<Precondition>();

    public OutputCreator Clone()
    {
        var clone = MemberwiseClone() as OutputCreator;
        clone.Id = Guid.NewGuid();
        return clone;
    }
}

public partial class Precondition : ObservableObject
{
    [ObservableProperty]
    private bool _isActive;

    [ObservableProperty]
    private Guid _referenceId;

    [ObservableProperty]
    private char? _operator;

    [ObservableProperty]
    private string _comparisonValue;

    [ObservableProperty]
    private bool _isOrOperator;
}