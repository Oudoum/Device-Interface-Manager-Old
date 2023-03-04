using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Device_Interface_Manager.interfaceIT.USB;
using Device_Interface_Manager.MVVM.Model;
using System.Collections.ObjectModel;
using System.Linq;
using static Device_Interface_Manager.MVVM.ViewModel.MainViewModel;

namespace Device_Interface_Manager.MVVM.ViewModel
{
    public partial class LEDTestViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isEnabled;

        public ObservableCollection<LEDTestModel.DeviceLED> LEDs { get; set; } = new();

        [RelayCommand]
        private void LEDEnable()
        {
            _ = InterfaceITAPI_Data.interfaceIT_LED_Enable(GetSelectedDeviceSession(), this.IsEnabled = !this.IsEnabled);
            for (int i = DeviceList[GetSeletedController()].DeviceInfo.nLEDFirst; i <= DeviceList[GetSeletedController()].DeviceInfo.nLEDLast; i++)
            {
                this.LEDs.Add(new LEDTestModel.DeviceLED
                {
                    Id = i - DeviceList[GetSeletedController()].DeviceInfo.nLEDFirst + 1,
                    Name = "Board: " + DeviceList[GetSeletedController()].DeviceInfo.szBoardType + " [LED]",
                    Position = i,
                });
            }
        }

        [RelayCommand]
        private void IsChecked(object posLED)
        {
            _ = InterfaceITAPI_Data.interfaceIT_LED_Set(GetSelectedDeviceSession(), (int)posLED, this.LEDs.FirstOrDefault(x => x.Position == int.Parse(posLED.ToString())).IsChecked);
        }

        [RelayCommand]
        private void AllLEDOnOff(string direction)
        {
            if (direction is "On")
            {
                foreach (var led in this.LEDs)
                {
                    _ = InterfaceITAPI_Data.interfaceIT_LED_Set(GetSelectedDeviceSession(), led.Position, led.IsChecked = true);
                }
                return;
            }
            foreach (var led in this.LEDs)
            {
                _ = InterfaceITAPI_Data.interfaceIT_LED_Set(GetSelectedDeviceSession(), led.Position, led.IsChecked = false);
            }
        }
    }
}