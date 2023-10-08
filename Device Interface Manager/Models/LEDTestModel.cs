namespace Device_Interface_Manager.Models;

public partial class LEDTestModel
{
    public partial class DeviceLED : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
        private bool _isChecked;
    }
}