namespace Device_Interface_Manager.MVVM.Model;

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
        public Profile Profile { get; set; }
    }

    public class Profile
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}