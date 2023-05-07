namespace Device_Interface_Manager.MVVM.Model;

public class HomeENETModel
{
    public class Connection : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private string _iPAddress;
        public string IPAddress
        {
            get => _iPAddress;
            set
            {
                if (System.Net.IPAddress.TryParse(value, out System.Net.IPAddress ipAddress))
                {
                    _iPAddress = ipAddress.ToString();
                }
            }
        }
        private interfaceIT.ENET.InterfaceITEthernet.ConnectionStatus _status;
        [System.Text.Json.Serialization.JsonIgnore]
        public interfaceIT.ENET.InterfaceITEthernet.ConnectionStatus Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }
        public Profile Profile { get; set; }
    }

    public class Profile
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}