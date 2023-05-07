using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Device_Interface_Manager.MVVM.Model;
public partial class TestCreator : ObservableObject
{
    [ObservableProperty]
    public string _profileName;

    [ObservableProperty]
    public string _boardType;

    public ObservableCollection<InputCreator> InputCreator { get; set; }

    public ObservableCollection<OutputCreator> OutputCreator { get; set; }
}

public class InputCreator
{
    public ObservableCollection<Switch> Switches { get; set; }
    public EventType EventType { get; set; }
    public string Event { get; set; } // MSFS/SimConnect...
    //PMDG Events (ENUM)
    //PMDG Mouse Events (ENUM)
    //PMDG Event Data if no Mouse Event
    //Negated
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
    //PMDG Data struct
    //PMDG Array
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