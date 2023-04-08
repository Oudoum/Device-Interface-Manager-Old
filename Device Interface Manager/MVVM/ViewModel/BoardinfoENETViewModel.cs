using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Device_Interface_Manager.interfaceIT.ENET;

namespace Device_Interface_Manager.MVVM.ViewModel;

public partial class BoardinfoENETViewModel : ObservableObject, IRecipient<ValueChangedMessage<InterfaceITEthernet>>
{
    public ObservableCollection<ObservableCollection<string>> InterfaceITEthernetInfoTextCollection { get; set; } = new();
    public ObservableCollection<string> InterfaceITEthernetInfoText { get; set; } = new();

    public ObservableCollection<string> InterfaceITEthernetInfoIPCollection { get; set; } = new();
    [ObservableProperty]
    private string _interfaceITEthernetInfoIP;

    partial void OnInterfaceITEthernetInfoIPChanged(string value)
    {
        if (value is not null)
        {
            this.InterfaceITEthernetInfoText = this.InterfaceITEthernetInfoTextCollection[this.InterfaceITEthernetInfoIPCollection.IndexOf(value)];
        }
    }

    public BoardinfoENETViewModel()
    {
        WeakReferenceMessenger.Default.Register(this);
    }

    public void Receive(ValueChangedMessage<InterfaceITEthernet> message)
    {
        if (InterfaceITEthernetInfoIPCollection.Contains(message.Value.Hostname))
        {
            InterfaceITEthernetInfoIPCollection.Clear();
            InterfaceITEthernetInfoTextCollection.Clear();
        }
        InterfaceITEthernetInfoIPCollection.Add(message.Value.Hostname);
        InterfaceITEthernetInfoTextCollection.Add(message.Value.InterfaceITEthernetInfoText);
        InterfaceITEthernetInfoIP = message.Value.Hostname;
    }
}