namespace Device_Interface_Manager.MVVM.Model
{
    public class HomeUSBModel
    {
        public class Connection : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
        {
            public int Id { get; set; }
            public string Name { get; set; }
            private string _serial;
            public string Serial
            {
                get => _serial;
                set
                {
                    _serial = value;
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
