using CommunityToolkit.Mvvm.ComponentModel;

namespace Device_Interface_Manager.Models;

public partial class LEDTestModel
{
    public partial class DeviceLED : ObservableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        [ObservableProperty]
        private bool _isChecked;
    }
}