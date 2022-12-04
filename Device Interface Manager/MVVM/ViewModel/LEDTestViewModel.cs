using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Device_Interface_Manager.interfaceIT.USB;
using Device_Interface_Manager.MVVM.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using static Device_Interface_Manager.MVVM.ViewModel.MainViewModel;

namespace Device_Interface_Manager.MVVM.ViewModel
{
    class LEDTestViewModel : ObservableObject
    {
        public RelayCommand LEDEnableCommand { get; set; }
        public RelayCommand<object> IsCheckedCommand { get; set; }
        public RelayCommand<string> AllLEDOnOffCommand { get; set; }


        private bool _isEnabled;
        public bool IsEnabled
        {
            get => this._isEnabled;
            set
            {
                this._isEnabled = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<LEDTestModel.DeviceLED> LED { get; set; } = new ObservableCollection<LEDTestModel.DeviceLED>();

        public LEDTestViewModel()
        {
            this.LEDEnableCommand = new RelayCommand(() =>
            {
                this.IsEnabled = !this.IsEnabled;
                InterfaceITAPI_Data.interfaceIT_LED_Enable(GetSelectedDeviceSession(), this.IsEnabled);

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
                }
            });

            this.AllLEDOnOffCommand = new RelayCommand<string>(o =>
            {
                if (o is "On")
                {
                    foreach (var led in this.LED)
                    {
                        led.IsChecked = true;
                        InterfaceITAPI_Data.interfaceIT_LED_Set(GetSelectedDeviceSession(), led.Position, true);
                    }
                }
                else if (o is "Off")
                {
                    foreach (var led in this.LED)
                    {
                        led.IsChecked = false;
                        InterfaceITAPI_Data.interfaceIT_LED_Set(GetSelectedDeviceSession(), led.Position, false);
                    }
                }
            });

            this.IsCheckedCommand = new RelayCommand<object>(o =>
            {
                InterfaceITAPI_Data.interfaceIT_LED_Set(GetSelectedDeviceSession(), (int)o, this.LED.FirstOrDefault(x => x.Position == int.Parse(o.ToString())).IsChecked);
            });
        }
    }
}