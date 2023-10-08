using System.Runtime.Serialization;

namespace Device_Interface_Manager.HubHop;

public enum HubHopType
{
    [EnumMember(Value = "Output")]
    Output = 1,
    [EnumMember(Value = "Input")]
    Input = 2,
    [EnumMember(Value = "InputOutput")]
    InputOutput = 3,
    [EnumMember(Value = "Input (Potentiometer)")]
    InputPotentiometer = 4,
    [EnumMember(Value = "AllInputs")]
    AllInputs = 6
}

public enum HubHopAction
{
    [EnumMember(Value = "Code")]
    Code = 1,
    [EnumMember(Value = "DataRef")]
    DataRef = 2,
    [EnumMember(Value = "Command")]
    Command = 4,
}