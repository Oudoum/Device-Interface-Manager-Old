namespace Device_Interface_Manager.Models;

public partial class HomeUSBModel
{
    public partial class Connection : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {
        public string Name { get; set; }

        public string Serial { get; set; }

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