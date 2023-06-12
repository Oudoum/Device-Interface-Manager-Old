namespace Device_Interface_Manager.MVVM.Model;

public class HomeENETModel
{
    public class Connection : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {
        public string Name { get; set; }
        private string _iPAddress;
        public string IPAddress
        {
            get => _iPAddress;
            set
            {
                if (string.IsNullOrEmpty(value))
                { 
                    _iPAddress = value;
                }

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
        private string _selectedProfile;
        public string SelectedProfile
        {
            get => _selectedProfile;
            set
            {
                if (_selectedProfile != value && value is not null)
                {
                    _selectedProfile = value;
                }
            }
        }
    }
}