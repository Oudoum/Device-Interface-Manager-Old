using CommunityToolkit.Mvvm.ComponentModel;

namespace Device_Interface_Manager.MVVM.Model
{
    public class HomeUSBModel
    {
        public class Connection : ObservableObject
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
            public Profile Profile { get; set; }
        }

        public class Profile
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}