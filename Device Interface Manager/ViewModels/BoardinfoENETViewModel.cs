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
        InterfaceITEthernetInfoIP = message.HOSTIPADDRESS;
        if (message.ID is null)
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
            "Board ID: " + interfaceITEthernetInfo.ID,
            "Name: " + interfaceITEthernetInfo.NAME,
            "Serial: " + interfaceITEthernetInfo.SERIAL,
            "Description: " + interfaceITEthernetInfo.DESC,
            "Version: " + interfaceITEthernetInfo.VERSION,
            "Firmware: " + interfaceITEthernetInfo.FIRMWARE,
            "Location: " + interfaceITEthernetInfo.LOCATION,
            "Usage: " + interfaceITEthernetInfo.USAGE,
            "Hostname: " + interfaceITEthernetInfo.HOSTNAME,
            "Client: " + interfaceITEthernetInfo.CLIENT,
            "Board " + interfaceITEthernetInfo.ID + " has the flollowing features:",
        };

        string[] componentTypes = { "LEDS", "SWITCHES", "SEVENSEGMENTS", "DATALINES", "ENCODERS", "ANALOGINS", "PULSEWIDTHS" };
        foreach (string componentType in componentTypes)
        {
            var component = interfaceITEthernetInfo.BOARDS[0].GetType().GetProperty(componentType).GetValue(interfaceITEthernetInfo.BOARDS[0], null);
            if (component is not null)
            {
                InterfaceITEthernetInfoTextCollection.Add(((InterfaceITEthernetInfoBoardConfig)component).Total + $" | {componentType} ( " + ((InterfaceITEthernetInfoBoardConfig)component).Start + " - " + ((InterfaceITEthernetInfoBoardConfig)component).Stop + " )");
            }
        }
    }
}