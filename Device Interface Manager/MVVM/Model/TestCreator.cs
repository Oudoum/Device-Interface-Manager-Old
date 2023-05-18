using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Device_Interface_Manager.MSFSProfiles.PMDG;
using System.Collections.Generic;

namespace Device_Interface_Manager.MVVM.Model;
public partial class TestCreator : ObservableObject
{
    public string _profileName;

    public string _boardType;

    [ObservableProperty]
    private ObservableCollection<InputCreator> _inputCreator;

    [ObservableProperty]
    private ObservableCollection<OutputCreator> _outputCreator;
}

public partial class InputCreator : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<string> _switches;

    [ObservableProperty]
    private string _selectedSwitch = "--None--";


    public EventType EventType { get; set; }
    public string Event { get; set; } // MSFS/SimConnect...
    public PMDG_NG3_SDK.PMDGEvents PMDGEvent { get; set; }
    public KeyValuePair<string, uint>[] PMDGMouseEvent { get; set; }
    public int PMDGEventData { get; set; }
    public bool IsInverted { get; set; }
}

public class EventType
{
    public string Name1 { get; private set; } = "MSFS/SimConnect/LVar";
    public string Name2 { get; private set; } = "H";
    public string Name3 { get; private set; } = "PMDG737";
}

public class OutputCreator
{
    public ObservableCollection<string> LEDs { get; set; }
    public DataType DataType { get; set; }
    public string Data { get; set; }
    public string[] PMDGStructData { get; set; }
    public int PMDGStructArrayNum { get; set; }
}

public class DataType
{
    public string Name1 { get; private set; } = "MSFS/SimConnect";
    public string Name2 { get; private set; } = "PMDG737";
}

//7Segement

//Brightness

//Analog