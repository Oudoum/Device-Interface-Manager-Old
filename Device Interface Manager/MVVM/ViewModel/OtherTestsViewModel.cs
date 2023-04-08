using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Device_Interface_Manager.interfaceIT.USB;

namespace Device_Interface_Manager.MVVM.ViewModel;

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
        SevenSegmentText = _sevenSegmentText;
    }

    private string _sevenSegmentText = string.Empty;
    public string SevenSegmentText
    {
        get => _sevenSegmentText;
        set
        {
            if (SevenSegmentText.Length > value.Length)
            {
                Reset7SegmentDisplay();
            }
            if (value.Length > SevenSegmentCount - SevenSegmentSelectedPosition + 1)
            {
                _sevenSegmentText = value.Remove(SevenSegmentCount - SevenSegmentSelectedPosition + 1);
                _ = InterfaceITAPI_Data.interfaceIT_7Segment_Display(Session, _sevenSegmentText, SevenSegmentSelectedPosition);
                return;
            }
            _ = InterfaceITAPI_Data.interfaceIT_7Segment_Display(Session, _sevenSegmentText = value, SevenSegmentSelectedPosition);
        }
    }

    [ObservableProperty]
    private bool _datalineEnabled;

    [ObservableProperty]
    private int _brightnessValue;
    partial void OnBrightnessValueChanged(int value)
    {
        _ = InterfaceITAPI_Data.interfaceIT_Brightness_Set(Session, value);
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


    public required int Session { get; init; }
    public required int Features { get; init; }
    public required int DatalineFirst { get; init; }
    public required int SevenSegmentCount { get; init; }
    public required int SevenSegmentFirst { get; init; }
    public required int SevenSegmentLast { get; init; }
    public OtherTestsViewModel() { }

    [RelayCommand]
    private void SevenSegmentEnable()
    {
        if ((Features & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_OUTPUT_7SEGMENT) == 0)
        {
            FeatureNotSupported = "7 Segment not supported!";
            return;
        }
        
        FeatureNotSupported = string.Empty;
        _ = InterfaceITAPI_Data.interfaceIT_7Segment_Enable(Session, SevenSegmentEnabled = !SevenSegmentEnabled);
        if (SevenSegmentEnabled)
        {
            SevenSegmentPositions.Clear();
            for (int i = SevenSegmentFirst; i <= SevenSegmentLast; i++)
            {
                SevenSegmentSelectedPosition = 1;
                SevenSegmentPositions.Add(i);
            }
        }
    }

    [RelayCommand]
    private void DatalineEnable()
    {
        if ((Features & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_OUTPUT_DATALINE) == 0)
        {
            FeatureNotSupported = "Dataline not supported!";
            return;
        }
        DatalineEnabled = !DatalineEnabled;
        FeatureNotSupported = string.Empty;
        if (!DatalineEnabled)
        {
            _ = InterfaceITAPI_Data.interfaceIT_Dataline_Set(Session, DatalineFirst, DatalineEnabled);
            _ = InterfaceITAPI_Data.interfaceIT_Dataline_Enable(Session, DatalineEnabled);
            return;
        }
        _ = InterfaceITAPI_Data.interfaceIT_Dataline_Enable(Session, DatalineEnabled);
        _ = InterfaceITAPI_Data.interfaceIT_Dataline_Set(Session, DatalineFirst, DatalineEnabled);
    }

    [RelayCommand]
    private void BrightnessEnable()
    {
        BrightnessValue = 0;
        if ((Features & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_SPECIAL_BRIGHTNESS) == 0)
        {
            FeatureNotSupported = "Brightness not supported!";
            return;
        }
        FeatureNotSupported = string.Empty;
        _ = InterfaceITAPI_Data.interfaceIT_Brightness_Enable(Session, BrightnessEnabled = !BrightnessEnabled);
    }

    [RelayCommand]
    private void AnalogEnable()
    {
        if ((Features & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_SPECIAL_ANALOG_INPUT) == 0 &&
            (Features & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_SPECIAL_ANALOG16_INPUT) == 0)
        {
            FeatureNotSupported = "Analog input not supported!";
            return;
        }
        FeatureNotSupported = string.Empty;
        _ = InterfaceITAPI_Data.interfaceIT_Analog_Enable(Session, AnalogEnabled = !AnalogEnabled);
        if (!AnalogEnabled)
        {
            valuesCancellationTokenSource.Cancel();
            return;
        }
        Task.Run(() => GetValues((valuesCancellationTokenSource = new CancellationTokenSource()).Token));
    }

    [RelayCommand]
    private void LoggingEnable()
    {
        _ = InterfaceITAPI_Data.interfaceIT_EnableLogging(LoggingEnabled = !LoggingEnabled);
        if (LoggingEnabled)
        {
            System.Diagnostics.Process.Start("explorer.exe", Environment.ExpandEnvironmentVariables(@"%appdata%\TEKWorx Limited\interfaceIT API"));
        }
    }

    public void Reset7SegmentDisplay()
    {
        _ = InterfaceITAPI_Data.interfaceIT_7Segment_Display(Session, new string(' ', SevenSegmentCount), 1);
    }

    public void GetValues(CancellationToken token)
    {
        int oldValue = 0;
        int noldValue = 0;
        while (AnalogEnabled)
        {
            _ = InterfaceITAPI_Data.interfaceIT_Analog_GetValue(Session, 0, out int value);
            if (value - oldValue >= 50 | value - oldValue <= -50 | oldValue == 0)
            {
                oldValue = value;
            }
            if (value - noldValue > 25 | value - noldValue < -25 | noldValue == 0)
            {
                AnalogValue = noldValue = value;
            }
            Thread.Sleep(200);
            if (token.IsCancellationRequested)
            {
                return;
            }
        }
    }
}