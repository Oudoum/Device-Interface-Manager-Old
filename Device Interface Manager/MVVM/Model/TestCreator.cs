using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Device_Interface_Manager.MSFSProfiles.PMDG;
using System.Collections.Generic;

namespace Device_Interface_Manager.MVVM.Model;
public partial class TestCreator : ObservableObject
{
    public string _profileName;

    public string _boardType;

    public ObservableCollection<InputCreator> InputCreator { get; set; }

    public ObservableCollection<OutputCreator> OutputCreator { get; set; }
}

public class InputCreator
{
    public ObservableCollection<Switch> Switches { get; set; }
    public EventType EventType { get; set; }
    public string Event { get; set; } // MSFS/SimConnect...
    public PMDG_NG3_SDK.PMDGEvents PMDGEvent { get; set; }
    public KeyValuePair<string, uint>[] PMDGMouseEvent { get; set; }
    public int PMDGEventData { get; set; }
    public bool IsInverted { get; set; }
}

public class Switch
{
    public int Id { get; set; }
    public int Position { get; set; }
}

public class EventType
{
    public string Name1 { get; private set; } = "MSFS/SimConnect/LVar";
    public string Name2 { get; private set; } = "H";
    public string Name3 { get; private set; } = "PMDG737";
}

public class OutputCreator
{
    public ObservableCollection<LED> LEDs { get; set; }
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

public class LED
{
    public int Id { get; set; }
    public int Position { get; set; }
}

//7Segement

//Brightness

//Analog