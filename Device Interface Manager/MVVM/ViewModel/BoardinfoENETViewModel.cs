using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Device_Interface_Manager.interfaceIT.ENET;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Device_Interface_Manager.MVVM.ViewModel
{
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
            if (this.InterfaceITEthernetInfoIPCollection.Contains(message.Value.Hostname))
            {
                this.InterfaceITEthernetInfoIPCollection.Clear();
                this.InterfaceITEthernetInfoTextCollection.Clear();
            }
            this.InterfaceITEthernetInfoIPCollection.Add(message.Value.Hostname);
            this.InterfaceITEthernetInfoTextCollection.Add(message.Value.InterfaceITEthernetInfoText);
            this.InterfaceITEthernetInfoIP = message.Value.Hostname;
        }
    }
}