using System;
using System.Linq;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Device_Interface_Manager.Devices.interfaceIT.USB;
using Device_Interface_Manager.Models;
using Device_Interface_Manager.SimConnectProfiles.PMDG;
using MahApps.Metro.Controls.Dialogs;
using static Device_Interface_Manager.Models.OutputCreatorModel;

namespace Device_Interface_Manager.ViewModels;
public partial class OutputCreatorViewModel : BaseCreatorViewModel
{
    public OutputCreatorViewModel(OutputCreatorModel outputCreatorModel, object device) : base(device)
    {
        OutputCreatorModel = outputCreatorModel;
    }

    public OutputCreatorModel OutputCreatorModel { get; set; }

    public string OutputType
    {
        get
        {
            Digits ??= CreateDigits();
            return OutputCreatorModel.OutputType;
        }
        set
        {
            if (OutputCreatorModel.OutputType != value)
            {
                OutputCreatorModel.OutputType = value;
                OnPropertyChanged(nameof(OutputType));
                if (value != ProfileCreatorModel.SEVENSEGMENT)
                {
                    IsPadded = null;
                    PaddingCharacterPair = null;
                    DigitCount = null;
                    SubstringStart = null;
                    SubstringEnd = null;
                    OnPropertyChanged(nameof(SubstringStart));
                    OnPropertyChanged(nameof(SubstringEnd));
                }
                else if (value == ProfileCreatorModel.SEVENSEGMENT)
                {
                    IsPadded = false;
                    Operator = null;
                    ComparisonValue = null;
                    TrueValue = null;
                    FalseValue = null;
                    IsInverted = false;
                    OnPropertyChanged(nameof(Operator));
                    OnPropertyChanged(nameof(ComparisonValue));
                    OnPropertyChanged(nameof(TrueValue));
                    OnPropertyChanged(nameof(FalseValue));
                    OnPropertyChanged(nameof(IsInverted));
                }
            }
        }
    }

    public string[] OutputTypes => OutputCreatorModel.OutputTypes;

    public int? Output
    {
        get => OutputCreatorModel.Output;
        set
        {
            if (OutputCreatorModel.Output != value)
            {
                OutputCreatorModel.Output = value;
                OnPropertyChanged(nameof(Output));
            }
        }
    }

    public int?[] LEDs => OutputCreatorModel.LEDs;

    public int?[] Datalines => OutputCreatorModel.Datalines;

    public int?[] SevenSegments => OutputCreatorModel.SevenSegments;

    public string DataType
    {
        get => OutputCreatorModel.DataType;
        set
        {
            if (OutputCreatorModel.DataType != value)
            {
                OutputCreatorModel.DataType = value;
                if (value is not null)
                {
                    if (value == ProfileCreatorModel.MSFSSimConnect)
                    {
                        PMDGData = null;
                    }
                    else if (value == ProfileCreatorModel.PMDG737)
                    {
                        Data = null;
                        Unit = null;
                    }
                }
                Operator = null;
                ComparisonValue = null;
                TrueValue = null;
                FalseValue = null;
                OnPropertyChanged(nameof(DataType));
                OnPropertyChanged(nameof(PMDGDataArrayIndices));
                OnPropertyChanged(nameof(PMDGDataArrayIndex));
                OnPropertyChanged(nameof(IsComparisonEnabled));
                OnPropertyChanged(nameof(Operator));
                OnPropertyChanged(nameof(ComparisonValue));
                OnPropertyChanged(nameof(TrueValue));
                OnPropertyChanged(nameof(FalseValue));
            }
        }
    }

    public string[] DataTypes => OutputCreatorModel.DataTypes;

    public string PMDGData
    {
        get => OutputCreatorModel.PMDGData;
        set
        {
            if (OutputCreatorModel.PMDGData != value)
            {
                OutputCreatorModel.PMDGData = value;
                Operator = null;
                ComparisonValue = null;
                TrueValue = null;
                FalseValue = null;
                OnPropertyChanged(nameof(PMDGData));
                OnPropertyChanged(nameof(PMDGDataArrayIndices));
                OnPropertyChanged(nameof(PMDGDataArrayIndex));
                OnPropertyChanged(nameof(IsComparisonEnabled));
                OnPropertyChanged(nameof(Operator));
                OnPropertyChanged(nameof(ComparisonValue));
                OnPropertyChanged(nameof(TrueValue));
                OnPropertyChanged(nameof(FalseValue));
            }
        }
    }

    public string SearchPMDGData
    {
        get => OutputCreatorModel.SearchPMDGData;
        set
        {
            if (OutputCreatorModel.SearchPMDGData != value)
            {
                OutputCreatorModel.SearchPMDGData = value;
                OnPropertyChanged(nameof(PMDGDataArray));
                OnPropertyChanged(nameof(PMDGData));
                OnPropertyChanged(nameof(PMDGDataArrayIndices));
                OnPropertyChanged(nameof(PMDGDataArrayIndex));
            }
        }
    }

    public string[] PMDGDataArray => OutputCreatorModel.PMDGDataArray;

    public int? PMDGDataArrayIndex
    {
        get => OutputCreatorModel.PMDGDataArrayIndex;
        set
        {
            if (OutputCreatorModel.PMDGDataArrayIndex != value)
            {
                OutputCreatorModel.PMDGDataArrayIndex = value;
                OnPropertyChanged(nameof(PMDGDataArrayIndex));
            }
        }
    }

    public int?[] PMDGDataArrayIndices => OutputCreatorModel.PMDGDataArrayIndices;

    public bool IsComparisonEnabled => !string.IsNullOrEmpty(PMDGData)
    && typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetField(PMDGData)?.FieldType != typeof(bool)
    && typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetField(PMDGData)?.FieldType != typeof(bool[]);

    public char? Operator
    {
        get => OutputCreatorModel.Operator;
        set => OutputCreatorModel.Operator = value;
    }

    public string ComparisonValue
    {
        get => OutputCreatorModel.ComparisonValue;
        set => OutputCreatorModel.ComparisonValue = value;
    }

    public double? TrueValue
    {
        get => OutputCreatorModel.TrueValue;
        set => OutputCreatorModel.TrueValue = value;
    }

    public double? FalseValue
    {
        get => OutputCreatorModel.FalseValue;
        set => OutputCreatorModel.FalseValue = value;
    }

    public string Data
    {
        get => OutputCreatorModel.Data;
        set
        {
            if (OutputCreatorModel.Data != value)
            {
                OutputCreatorModel.Data = value;
                OnPropertyChanged(nameof(Data));
            }
        }
    }

    public string Unit
    {
        get => OutputCreatorModel.Unit;
        set
        {
            if (OutputCreatorModel.Unit != value)
            {
                OutputCreatorModel.Unit = value;
                OnPropertyChanged(nameof(Unit));
            }
        }
    }

    public bool IsInverted
    {
        get => OutputCreatorModel.IsInverted;
        set => OutputCreatorModel.IsInverted = value;
    }


    public bool? IsPadded
    {
        get => OutputCreatorModel.IsPadded;
        set
        {
            if (OutputCreatorModel.IsPadded != value)
            {
                OutputCreatorModel.IsPadded = value;
                OnPropertyChanged(nameof(IsPadded));
            }
        }
    }

    public static Dictionary<string, char?> PaddingCharacters => OutputCreatorModel.PaddingCharacters;

    public KeyValuePair<string, char?>? PaddingCharacterPair
    {
        get
        {
            if (PaddingCharacter is null)
            {
                return null;
            }
            return PaddingCharacters.First(s => s.Value == PaddingCharacter);
        }
        set
        {
            if (value is null)
            {
                OutputCreatorModel.PaddingCharacter = null;
            }
            if (value is not null)
            {
                OutputCreatorModel.PaddingCharacter = value.Value.Value;
            }
            OnPropertyChanged(nameof(PaddingCharacterPair));
        }
    }


    public char? PaddingCharacter
    {
        get => OutputCreatorModel.PaddingCharacter;
        set => OutputCreatorModel.PaddingCharacter = value;
    }

    public byte[] DigitCounts { get; set; } = { 1, 2, 3, 4, 5, 6, 7, 8 };

    public byte? DigitCount
    {
        get => OutputCreatorModel.DigitCount;
        set
        {
            if (OutputCreatorModel.DigitCount != value)
            {
                OutputCreatorModel.DigitCount = value;
                OnPropertyChanged(nameof(DigitCount));
                if (value is null)
                {
                    Digits.Clear();
                    DigitFormatting.DigitCheckedSum = null;
                    DigitFormatting.DecimalPointCheckedSum = null;
                    return;
                }
                else if (Digits.Count > value)
                {
                    for (int i = Digits.Count - 1; i >= value; i--)
                    {
                        Digits[i].IsDigitChecked = false;
                        Digits[i].IsDecimalPointChecked = false;
                        Digits.RemoveAt(i);
                    }
                    return;
                }
                for (int i = Digits.Count; i < value; i++)
                {
                    Digits.Add(new DigitFormatting { Digit = (byte)(i + 1) });
                }
            }
        }
    }

    public byte? DigitCheckedSum => OutputCreatorModel.DigitCheckedSum;

    public byte? DecimalPointCheckedSum => OutputCreatorModel.DecimalPointCheckedSum;

    public ObservableCollection<DigitFormatting> Digits { get; set; }

    private ObservableCollection<DigitFormatting> CreateDigits()
    {
        ObservableCollection<DigitFormatting> digits = new();
        DigitFormatting.DigitCheckedSum = OutputCreatorModel.DigitCheckedSum;
        DigitFormatting.DecimalPointCheckedSum = OutputCreatorModel.DecimalPointCheckedSum;
        for (int i = 0; i < DigitCount; i++)
        {
            DigitFormatting digitFormatting = new()
            {
                Digit = (byte)(i + 1),
                IsDigitChecked = OutputCreatorModel.DigitCheckedSum != null && (DigitCheckedSum & (1 << i)) != 0,
                IsDecimalPointChecked = OutputCreatorModel.DecimalPointCheckedSum != null && (DecimalPointCheckedSum & (1 << i)) != 0
            };
            digits.Add(digitFormatting);
        }
        DigitFormatting.SumDigitCheckedChanged += DigitFormatting_SumDigitCheckedChanged;
        DigitFormatting.SumDecimalPointCheckedChanged += DigitFormatting_SumDecimalPointCheckedChanged;
        return digits;
    }

    private void DigitFormatting_SumDigitCheckedChanged(object sender, EventArgs e)
    {
        OutputCreatorModel.DigitCheckedSum = DigitFormatting.DigitCheckedSum;
    }

    private void DigitFormatting_SumDecimalPointCheckedChanged(object sender, EventArgs e)
    {
        OutputCreatorModel.DecimalPointCheckedSum = DigitFormatting.DecimalPointCheckedSum;
    }

    public byte? SubstringStart
    {
        get => OutputCreatorModel.SubstringStart;
        set => OutputCreatorModel.SubstringStart = value;
    }

    public byte? SubstringEnd
    {
        get => OutputCreatorModel.SubstringEnd;
        set => OutputCreatorModel.SubstringEnd = value;
    }

    public override OutputCreator[] OutputCreator 
    { 
        get => OutputCreatorModel.OutputCreator;
        set => OutputCreatorModel.OutputCreator = value;
    }

    public override ObservableCollection<PreconditionModel> Preconditions
    {
        get => OutputCreatorModel.Preconditions;
        set => OutputCreatorModel.Preconditions = value;
    }

    private int? outputPosition;

    private void SetOutputPosition(int? value)
    {
        //DISABLE FOR DEBUG
        if (outputPosition != value)
        {
            if (outputPosition is not null)
            {
                switch (OutputType)
                {
                    case ProfileCreatorModel.LED:
                        InterfaceITAPI_Data.interfaceIT_LED_Set(Device.Session, outputPosition.Value, false);
                        break;

                    case ProfileCreatorModel.DATALINE:
                        InterfaceITAPI_Data.interfaceIT_Dataline_Set(Device.Session, outputPosition.Value, false);
                        break;

                    case ProfileCreatorModel.SEVENSEGMENT:
                        InterfaceITAPI_Data.interfaceIT_7Segment_Display(Device.Session, " ", outputPosition.Value);
                        break;
                }
            }
            if (value is not null)
            {
                switch (OutputType)
                {
                    case ProfileCreatorModel.LED:
                        InterfaceITAPI_Data.interfaceIT_LED_Set(Device.Session, value.Value, true);
                        break;

                    case ProfileCreatorModel.DATALINE:
                        InterfaceITAPI_Data.interfaceIT_Dataline_Set(Device.Session, value.Value, true);
                        break;

                    case ProfileCreatorModel.SEVENSEGMENT:
                        InterfaceITAPI_Data.interfaceIT_7Segment_Display(Device.Session, "8", value.Value);
                        break;
                }
            }
            outputPosition = value;
        }
        //DISABLE FOR DEBUG
    }

    [RelayCommand]
    private void MouseMoveComboBox(System.Windows.Input.MouseEventArgs e)
    {
        if (e.OriginalSource is TextBlock textBlock)
        {
            string text = textBlock.Text;
            if (text == "r")
            {
                return;
            }
            SetOutputPosition(int.Parse(text));
            return;
        }
        else if (e.OriginalSource is Grid grid)
        {
            SetOutputPosition(((OutputCreatorViewModel)grid.DataContext).Output);
            return;
        }
        SetOutputPosition(null);
    }

    [RelayCommand]
    private void MouseLeaveComboBox(System.Windows.Input.MouseEventArgs e)
    {
        if (e.OriginalSource is ComboBox)
        {
            SetOutputPosition(null);
        }
    }

    public override MessageDialogResult CanCloseAsync()
    {
        DigitFormatting.SumDecimalPointCheckedChanged -= DigitFormatting_SumDecimalPointCheckedChanged;
        DigitFormatting.SumDigitCheckedChanged -= DigitFormatting_SumDigitCheckedChanged;
        return base.CanCloseAsync();
    }
}