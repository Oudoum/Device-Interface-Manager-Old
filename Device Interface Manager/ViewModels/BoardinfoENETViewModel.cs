using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Device_Interface_Manager.Devices.interfaceIT.ENET;

namespace Device_Interface_Manager.ViewModels;

public partial class BoardinfoENETViewModel : ObservableObject, IRecipient<InterfaceITEthernetInfo>
{
    public ObservableCollection<string> InterfaceITEthernetInfoTextCollection { get; set; }

    [ObservableProperty]
    private string _interfaceITEthernetInfoIP;

    public BoardinfoENETViewModel()
    {
        WeakReferenceMessenger.Default.Register(this);
    }

    public void Receive(InterfaceITEthernetInfo message)
    {
        InterfaceITEthernetInfoIP = message.HostIpAddress;
        if (string.IsNullOrEmpty(message.Id))
        {
            InterfaceITEthernetInfoTextCollection = null;
            OnPropertyChanged(nameof(InterfaceITEthernetInfoTextCollection));
            return;
        }
        GetIITEthernetInfos(message);
        OnPropertyChanged(nameof(InterfaceITEthernetInfoTextCollection));
    }

    private void GetIITEthernetInfos(InterfaceITEthernetInfo interfaceITEthernetInfo)
    {
        InterfaceITEthernetInfoTextCollection = new()
        {
            "Board ID: " + interfaceITEthernetInfo.Id,
            "Name: " + interfaceITEthernetInfo.Name,
            "Serial: " + interfaceITEthernetInfo.SerialNumber,
            "Description: " + interfaceITEthernetInfo.Description,
            "Version: " + interfaceITEthernetInfo.Version,
            "Firmware: " + interfaceITEthernetInfo.Firmware,
            "Location: " + interfaceITEthernetInfo.Location,
            "Usage: " + interfaceITEthernetInfo.Usage,
            "Hostname: " + interfaceITEthernetInfo.HostName,
            "Client: " + interfaceITEthernetInfo.Client,
            "Board " + interfaceITEthernetInfo.Id + " has the flollowing features:",
        };

        string[] componentTypes = { "LedsConfig", "SwitchesConfig", "SevenSegmentsConfig", "DataLinesConfig", "EncodersConfig", "AnalogInputsConfig", "PulseWidthsConfig" };
        foreach (string componentType in componentTypes)
        {
            var component = interfaceITEthernetInfo.Boards[0].GetType().GetProperty(componentType).GetValue(interfaceITEthernetInfo.Boards[0], null);
            if (component is not null)
            {
                InterfaceITEthernetInfoTextCollection.Add(((InterfaceITEthernetBoardConfig)component).TotalCount + $" | {componentType} ( " + ((InterfaceITEthernetBoardConfig)component).StartIndex + " - " + ((InterfaceITEthernetBoardConfig)component).StopIndex + " )");
            }
        }
    }
}