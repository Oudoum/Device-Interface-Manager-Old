using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Device_Interface_Manager.interfaceIT.USB;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using static Device_Interface_Manager.MVVM.ViewModel.MainViewModel;

namespace Device_Interface_Manager.MVVM.ViewModel
{
    public partial class OtherTestsViewModel : ObservableObject
    {
        private CancellationTokenSource valuesCancellationTokenSource;

        public ObservableCollection<int> SevenSegmentPositions { get; set; } = new();

        [ObservableProperty]
        private bool _sevenSegmentEnabled;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SevenSegmentText))]
        private int _sevenSegmentSelectedPosition;
        partial void OnSevenSegmentSelectedPositionChanged(int value)
        {
            Reset7SegmentDisplay();
            this.SevenSegmentText = this._sevenSegmentText;
        }

        private string _sevenSegmentText = string.Empty;
        public string SevenSegmentText
        {
            get => this._sevenSegmentText;
            set
            {
                if (this.SevenSegmentText.Length > value.Length)
                {
                    Reset7SegmentDisplay();
                }
                if (value.Length > DeviceList[GetSeletedController()].DeviceInfo.n7SegmentCount - this.SevenSegmentSelectedPosition + 1)
                {
                    this._sevenSegmentText = value.Remove(DeviceList[GetSeletedController()].DeviceInfo.n7SegmentCount - this.SevenSegmentSelectedPosition + 1);
                    _ = InterfaceITAPI_Data.interfaceIT_7Segment_Display(GetSelectedDeviceSession(), this._sevenSegmentText, this.SevenSegmentSelectedPosition);
                    return;
                }
                _ = InterfaceITAPI_Data.interfaceIT_7Segment_Display(GetSelectedDeviceSession(), this._sevenSegmentText = value, this.SevenSegmentSelectedPosition);
            }
        }

        [ObservableProperty]
        private bool _datalineEnabled;

        [ObservableProperty]
        private int _brightnessValue;
        partial void OnBrightnessValueChanged(int value)
        {
            _ = InterfaceITAPI_Data.interfaceIT_Brightness_Set(GetSelectedDeviceSession(), value);
        }

        [ObservableProperty]
        private bool _brightnessEnabled;

        [ObservableProperty]
        private int _analogValue;

        [ObservableProperty]
        private bool _analogEnabled;

        [ObservableProperty]
        private bool _loggingEnabled;

        [ObservableProperty]
        private string _featureNotSupported;

        [RelayCommand]
        private void SevenSegmentEnable()
        {
            if ((DeviceList[GetSeletedController()].DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_OUTPUT_7SEGMENT) == 0)
            {
                this.FeatureNotSupported = "7 Segment not supported!";
                return;
            }
            
            this.FeatureNotSupported = string.Empty;
            _ = InterfaceITAPI_Data.interfaceIT_7Segment_Enable(GetSelectedDeviceSession(), this.SevenSegmentEnabled = !this.SevenSegmentEnabled);
            if (this.SevenSegmentEnabled)
            {
                this.SevenSegmentPositions.Clear();
                for (int i = DeviceList[GetSeletedController()].DeviceInfo.n7SegmentFirst; i <= DeviceList[GetSeletedController()].DeviceInfo.n7SegmentLast; i++)
                {
                    this.SevenSegmentSelectedPosition = 1;
                    this.SevenSegmentPositions.Add(i);
                }
            }
        }

        [RelayCommand]
        private void DatalineEnable()
        {
            if ((DeviceList[GetSeletedController()].DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_OUTPUT_DATALINE) == 0)
            {
                this.FeatureNotSupported = "Dataline not supported!";
                return;
            }
            this.DatalineEnabled = !this.DatalineEnabled;
            this.FeatureNotSupported = string.Empty;
            if (!this.DatalineEnabled)
            {
                _ = InterfaceITAPI_Data.interfaceIT_Dataline_Set(GetSelectedDeviceSession(), DeviceList[GetSeletedController()].DeviceInfo.nDatalineFirst, this.DatalineEnabled);
                _ = InterfaceITAPI_Data.interfaceIT_Dataline_Enable(GetSelectedDeviceSession(), this.DatalineEnabled);
                return;
            }
            _ = InterfaceITAPI_Data.interfaceIT_Dataline_Enable(GetSelectedDeviceSession(), this.DatalineEnabled);
            _ = InterfaceITAPI_Data.interfaceIT_Dataline_Set(GetSelectedDeviceSession(), DeviceList[GetSeletedController()].DeviceInfo.nDatalineFirst, this.DatalineEnabled);
        }

        [RelayCommand]
        private void BrightnessEnable()
        {
            this.BrightnessValue = 0;
            if ((DeviceList[GetSeletedController()].DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_SPECIAL_BRIGHTNESS) == 0)
            {
                this.FeatureNotSupported = "Brightness not supported!";
                return;
            }
            this.FeatureNotSupported = string.Empty;
            _ = InterfaceITAPI_Data.interfaceIT_Brightness_Enable(GetSelectedDeviceSession(), this.BrightnessEnabled = !this.BrightnessEnabled);
        }

        [RelayCommand]
        private void AnalogEnable()
        {
            if ((DeviceList[GetSeletedController()].DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_SPECIAL_ANALOG_INPUT) == 0 &&
                (DeviceList[GetSeletedController()].DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_SPECIAL_ANALOG16_INPUT) == 0)
            {
                this.FeatureNotSupported = "Analog input not supported!";
                return;
            }
            this.FeatureNotSupported = string.Empty;
            _ = InterfaceITAPI_Data.interfaceIT_Analog_Enable(GetSelectedDeviceSession(), this.AnalogEnabled = !this.AnalogEnabled);
            if (!this.AnalogEnabled)
            {
                this.valuesCancellationTokenSource.Cancel();
                return;
            }
            Task.Run(() => this.GetValues((this.valuesCancellationTokenSource = new CancellationTokenSource()).Token));
        }

        [RelayCommand]
        private void LoggingEnable()
        {
            _ = InterfaceITAPI_Data.interfaceIT_EnableLogging(this.LoggingEnabled = !this.LoggingEnabled);
            if (this.LoggingEnabled)
            {
                System.Diagnostics.Process.Start("explorer.exe", Environment.ExpandEnvironmentVariables(@"%appdata%\TEKWorx Limited\interfaceIT API"));
            }
        }

        public static void Reset7SegmentDisplay()
        {
            _ = InterfaceITAPI_Data.interfaceIT_7Segment_Display(GetSelectedDeviceSession(), new string(' ', DeviceList[GetSeletedController()].DeviceInfo.n7SegmentCount), 1);
        }

        public void GetValues(CancellationToken token)
        {
            int oldValue = 0;
            int noldValue = 0;
            while (this.AnalogEnabled)
            {
                _ = InterfaceITAPI_Data.interfaceIT_Analog_GetValue(GetSelectedDeviceSession(), 0, out int value);
                if (value - oldValue >= 50 | value - oldValue <= -50 | oldValue == 0)
                {
                    oldValue = value;
                }
                if (value - noldValue > 25 | value - noldValue < -25 | noldValue == 0)
                {
                    this.AnalogValue = noldValue = value;
                }
                Thread.Sleep(200);
                if (token.IsCancellationRequested)
                {
                    return;
                }
            }
        }
    }
}