
namespace Device_Interface_Manager.MVVM.Model
{
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

            private byte _status;
            public byte Status
            {
                get => _status;
                set
                {
                    _status = value;
                    OnPropertyChanged();
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
}