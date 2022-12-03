
namespace Device_Interface_Manager.MVVM.Model
{
    public class LEDTestModel
    {
        public class DeviceLED : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Position { get; set; }
            private bool _isChecked;
            public bool IsChecked 
            {
                get => _isChecked;
                set
                {
                    _isChecked = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}