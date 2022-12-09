using CommunityToolkit.Mvvm.ComponentModel;

namespace Device_Interface_Manager.MVVM.Model
{
    public partial class LEDTestModel
    {
        [INotifyPropertyChanged]
        public partial class DeviceLED
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Position { get; set; }
            [ObservableProperty]
            private bool _isChecked;
        }
    }
}