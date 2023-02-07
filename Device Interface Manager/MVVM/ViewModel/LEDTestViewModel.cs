using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Device_Interface_Manager.interfaceIT.USB;
using Device_Interface_Manager.MVVM.Model;
using System.Collections.ObjectModel;
using System.Linq;
using static Device_Interface_Manager.MVVM.ViewModel.MainViewModel;

namespace Device_Interface_Manager.MVVM.ViewModel
{
    partial class LEDTestViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isEnabled;

        public ObservableCollection<LEDTestModel.DeviceLED> LED { get; set; } = new();


        [RelayCommand]
        private void LEDEnable()
        {
            this.IsEnabled = !this.IsEnabled;
            _ = InterfaceITAPI_Data.interfaceIT_LED_Enable(GetSelectedDeviceSession(), this.IsEnabled);

            if (LED.Count == 0)
            {
                for (int i = DeviceList[GetSeletedController()].DeviceInfo.nLEDFirst; i <= DeviceList[GetSeletedController()].DeviceInfo.nLEDLast; i++)
                {
                    this.LED.Add(new LEDTestModel.DeviceLED
                    {
                        Id = i - DeviceList[GetSeletedController()].DeviceInfo.nLEDFirst + 1,
                        Name = "Board: " + DeviceList[GetSeletedController()].DeviceInfo.szBoardType + " [LED]",
                        Position = i,
                    });
                }
                //for (int i = DeviceList[GetSeletedController()].DeviceInfo.n7SegmentFirst; i <= DeviceList[GetSeletedController()].DeviceInfo.nLEDLast; i++)
                //{
                //    this.LED.Add(new LEDTestModel.DeviceLED
                //    {
                //        Id = i - DeviceList[GetSeletedController()].DeviceInfo.n7SegmentFirst + 1,
                //        Name = "Board: " + DeviceList[GetSeletedController()].DeviceInfo.szBoardType + " [LED]",
                //        Position = i,
                //    });
                //}
            }
        }

        [RelayCommand]
        private void IsChecked(object posLED)
        {
            _ = InterfaceITAPI_Data.interfaceIT_LED_Set(GetSelectedDeviceSession(), (int)posLED, this.LED.FirstOrDefault(x => x.Position == int.Parse(posLED.ToString())).IsChecked);
        }

        [RelayCommand]
        private void AllLEDOnOff(string direction)
        {
            if (direction is "On")
            {
                foreach (var led in this.LED)
                {
                    led.IsChecked = true;
                    _ = InterfaceITAPI_Data.interfaceIT_LED_Set(GetSelectedDeviceSession(), led.Position, true);
                }
            }
            else if (direction is "Off")
            {
                foreach (var led in this.LED)
                {
                    led.IsChecked = false;
                    _ = InterfaceITAPI_Data.interfaceIT_LED_Set(GetSelectedDeviceSession(), led.Position, false);
                }
            }
        }
    }
}