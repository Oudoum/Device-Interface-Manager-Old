using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using static Device_Interface_Manager.MVVM.ViewModel.MainViewModel;

namespace Device_Interface_Manager.MVVM.ViewModel
{
    class OtherTestsViewModel : ObservableObject
    {

        public CancellationTokenSource GetValuesCancellationTokenSource { get; set; }

        public RelayCommand SevenSegmentEnableCommand { get; set; }

        public RelayCommand DatalineEnableCommand { get; set; }

        public RelayCommand BrightnessEnableCommand { get; set; }

        public RelayCommand AnalogEnableCommand { get; set; }

        public RelayCommand LoggingEnableCommand { get; set; }



        public Thread AnalogValueThread { get; set; }


        public ObservableCollection<int> SevenSegmentPositions { get; set; } = new ObservableCollection<int>();


        private bool _sevenSegmentEnabled;
        public bool SevenSegmentEnabled
        {
            get => this._sevenSegmentEnabled;
            set
            {
                if (value == true)
                {
                    this.SevenSegmentPositions.Clear();
                    for (int i = DeviceList[GetSeletedController()].DeviceInfo.n7SegmentFirst; i <= DeviceList[GetSeletedController()].DeviceInfo.n7SegmentLast; i++)
                    {
                        this.SevenSegmentSelectedPosition = 1;
                        this.SevenSegmentPositions.Add(i);
                    }
                }
                InterfaceITAPI_Data.interfaceIT_7Segment_Enable(GetSelectedDeviceSession(), value);
                this._sevenSegmentEnabled = value;
                OnPropertyChanged();
            }
        }

        private int _sevenSegmentSelectedPosition;
        public int SevenSegmentSelectedPosition
        {
            get => this._sevenSegmentSelectedPosition;
            set
            {
                this._sevenSegmentSelectedPosition = value;
                Reset7SegmentDisplay();
                this.SevenSegmentText = this.SevenSegmentText;
                OnPropertyChanged();
            }
        }

        private string _sevenSegmentText = string.Empty;
        public string SevenSegmentText
        {
            get => this._sevenSegmentText;
            set
            {
                if (value.Length <= DeviceList[GetSeletedController()].DeviceInfo.n7SegmentCount - this.SevenSegmentSelectedPosition + 1)
                {
                    if (_sevenSegmentText.Length > value.Length)
                    {
                        Reset7SegmentDisplay();
                    }
                    this._sevenSegmentText = value;
                }
                else
                {
                    this._sevenSegmentText = value.Remove(DeviceList[GetSeletedController()].DeviceInfo.n7SegmentCount - this.SevenSegmentSelectedPosition + 1);
                    OnPropertyChanged();
                }
                    InterfaceITAPI_Data.interfaceIT_7Segment_Display(GetSelectedDeviceSession(), this.SevenSegmentText, this.SevenSegmentSelectedPosition);
                    OnPropertyChanged();
            }
        }

        public static void Reset7SegmentDisplay()
        {
            InterfaceITAPI_Data.interfaceIT_7Segment_Display(GetSelectedDeviceSession(), new string(' ', DeviceList[GetSeletedController()].DeviceInfo.n7SegmentCount), 1);
        }

        private bool _datalineEnabled;
        public bool DatalineEnabled
        {
            get => this._datalineEnabled;
            set
            {
                if (value == true)
                {
                    InterfaceITAPI_Data.interfaceIT_Dataline_Enable(GetSelectedDeviceSession(), value);
                    InterfaceITAPI_Data.interfaceIT_Dataline_Set(GetSelectedDeviceSession(), DeviceList[GetSeletedController()].DeviceInfo.nDatalineFirst, value);
                }
                else
                {
                    InterfaceITAPI_Data.interfaceIT_Dataline_Set(GetSelectedDeviceSession(), DeviceList[GetSeletedController()].DeviceInfo.nDatalineFirst, value);
                    InterfaceITAPI_Data.interfaceIT_Dataline_Enable(GetSelectedDeviceSession(), value);
                }
                this._datalineEnabled = value;
                OnPropertyChanged();
            }
        }

        private int _brightnessValue;
        public int BrightnessValue
        {
            get => this._brightnessValue;
            set
            {
                this._brightnessValue = value;
                InterfaceITAPI_Data.interfaceIT_Brightness_Set(GetSelectedDeviceSession(), value);
                OnPropertyChanged();
            }
        }

        private bool _brightnessEnabled;
        public bool BrightnessEnabled
        {
            get => this._brightnessEnabled;
            set
            {
                this.BrightnessValue = 0;
                InterfaceITAPI_Data.interfaceIT_Brightness_Enable(GetSelectedDeviceSession(), value);
                this._brightnessEnabled = value;
                OnPropertyChanged();
            }
        }

        private int _analogValue;
        public int AnalogValue
        {
            get => this._analogValue;
            set
            {
                this._analogValue = value;
                OnPropertyChanged();
            }
        }

        private bool _analogEnabled;
        public bool AnalogEnabled
        {
            get => this._analogEnabled;
            set
            {
                this.GetValuesCancellationTokenSource = new CancellationTokenSource();
                InterfaceITAPI_Data.interfaceIT_Analog_Enable(GetSelectedDeviceSession(), value);
                if (value == true)
                {
                    this.AnalogValueThread = new Thread(o => this.GetValues(GetValuesCancellationTokenSource.Token));
                    this.AnalogValueThread.Start();
                }
                else
                {
                    this.GetValuesCancellationTokenSource?.Cancel();
                    this.AnalogValueThread = null;
                    this.AnalogValue = 0;
                }
                this._analogEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _loggingEnabled;
        public bool LoggingEnabled
        {
            get => this._loggingEnabled;
            set
            {
                InterfaceITAPI_Data.interfaceIT_EnableLogging(value);
                if (value == true)
                {
                    System.Diagnostics.Process.Start("explorer.exe", Environment.ExpandEnvironmentVariables(@"%appdata%\TEKWorx Limited\interfaceIT API"));
                }
                this._loggingEnabled = value;
                OnPropertyChanged();
            }
        }

        private string _featureNotSupported;
        public string FeatureNotSupported
        {
            get => this._featureNotSupported;
            set
            {
                this._featureNotSupported = value;
                OnPropertyChanged();
            }
        }


        public OtherTestsViewModel()
        {
            this.SevenSegmentEnableCommand = new RelayCommand(() =>
            {
                if ((DeviceList[GetSeletedController()].DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_OUTPUT_7SEGMENT) != 0)
                {
                    this.SevenSegmentEnabled = !this.SevenSegmentEnabled;
                    this.FeatureNotSupported = string.Empty;
                }
                else
                {
                    this.FeatureNotSupported = "7 Segment not supported!";
                }
            });

            this.DatalineEnableCommand = new RelayCommand(() =>
            {
                if ((DeviceList[GetSeletedController()].DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_OUTPUT_DATALINE) != 0)
                {
                    this.DatalineEnabled = !this.DatalineEnabled;
                    this.FeatureNotSupported = string.Empty;
                }
                else
                {
                    this.FeatureNotSupported = "Dataline not supported!";
                }
            });

            this.BrightnessEnableCommand = new RelayCommand(() =>
            {
                if ((DeviceList[GetSeletedController()].DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_SPECIAL_BRIGHTNESS) != 0)
                {
                    this.BrightnessEnabled = !this.BrightnessEnabled;
                    this.FeatureNotSupported = string.Empty;
                }
                else
                {
                    this.FeatureNotSupported = "Brightness not supported!";
                }
            });

            this.AnalogEnableCommand = new RelayCommand(() =>
            {
                if ((DeviceList[GetSeletedController()].DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_SPECIAL_ANALOG_INPUT) != 0 ||
                    (DeviceList[GetSeletedController()].DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_SPECIAL_ANALOG16_INPUT) != 0)
                {
                    this.AnalogEnabled = !this.AnalogEnabled;
                    this.FeatureNotSupported = string.Empty;
                }
                else
                {
                    this.FeatureNotSupported = "Analog input not supported!";
                }
            });

            this.LoggingEnableCommand = new RelayCommand(() =>
            {
                this.LoggingEnabled = !this.LoggingEnabled;
            });
        }

        //Get Analog Values Method
        public void GetValues(CancellationToken token)
        {
            int oldValue = 0;
            int noldValue = 0;
            while (this.AnalogEnabled)
            {
                InterfaceITAPI_Data.interfaceIT_Analog_GetValue(GetSelectedDeviceSession(), 0, out int value);
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