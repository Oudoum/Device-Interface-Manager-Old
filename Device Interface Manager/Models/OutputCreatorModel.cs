using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Device_Interface_Manager.SimConnectProfiles.PMDG;
using System.Collections.ObjectModel;

namespace Device_Interface_Manager.Models;
public class OutputCreatorModel
{
    public string OutputType { get; set; }

    public string[] OutputTypes { get; set; } = { ProfileCreatorModel.LED, ProfileCreatorModel.DATALINE, ProfileCreatorModel.SEVENSEGMENT };

    public Dictionary<string, string> LEDs { get; set; } = new();

    public Dictionary<string, string> Datalines { get; set; } = new();

    public Dictionary<string, string> SevenSegments { get; set; } = new();

    public KeyValuePair<string, string>? Output { get; set; }

    public string DataType { get; set; } = ProfileCreatorModel.PMDG737;

    public string[] DataTypes { get; set; } = { ProfileCreatorModel.MSFSSimConnect, ProfileCreatorModel.PMDG737 };

    public string PMDGData { get; set; }

    private string _searchPMDGData;
    public string SearchPMDGData
    {
        get => _searchPMDGData;
        set
        {
            if (_searchPMDGData != value)
            {
                _searchPMDGData = value;
                string matchingData = PMDGDataArray.FirstOrDefault(s => s.Equals(value, StringComparison.OrdinalIgnoreCase));
                if (matchingData is not null)
                {
                    PMDGData = matchingData;
                }
            }
        }
    }

    public string[] PMDGDataArray => string.IsNullOrEmpty(SearchPMDGData)
        ? typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetFields().Select(field => field.Name).Take(typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetFields().Length - 1).ToArray()
        : typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetFields().Select(field => field.Name).Where(name => name.Contains(SearchPMDGData, StringComparison.OrdinalIgnoreCase)).Take(typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetFields().Length - 1).ToArray();

    private int? _pMDGDataArrayIndex;
    public int? PMDGDataArrayIndex
    {
        get
        {
            if (_pMDGDataArrayIndex is null)
            {
                return _pMDGDataArrayIndex = PMDGDataArrayIndices?.FirstOrDefault();

            }
            return _pMDGDataArrayIndex;
        }
        set
        {
            if (value != _pMDGDataArrayIndex)
            {
                _pMDGDataArrayIndex = value;
            }
        }
    }

    public int?[] PMDGDataArrayIndices => string.IsNullOrEmpty(PMDGData)
        ? Array.Empty<int?>()
        : typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetField(PMDGData).GetCustomAttribute<MarshalAsAttribute>() is MarshalAsAttribute attribute && attribute.Value != UnmanagedType.ByValTStr && attribute.SizeConst is int size
        ? new int?[size].Select((_, i) => i).Cast<int?>().ToArray()
        : null;

    public static char[] Operators { get; set; } = new char[] { '=', '≠', '<', '>', '≤', '≥' };

    public char? Operator { get; set; }

    private string _comparisonValue;
    public string ComparisonValue
    {
        get => string.IsNullOrEmpty(PMDGData)
            || typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetField(PMDGData)?.FieldType != typeof(bool)
            || typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetField(PMDGData)?.FieldType != typeof(bool[])
            || DataType == ProfileCreatorModel.MSFSSimConnect
            ? _comparisonValue
            : null;
        set => _comparisonValue = value;
    }

    public double? TrueValue { get; set; }

    public double? FalseValue { get; set; }

    public string Data { get; set; }

    public string Unit { get; set; }

    public bool IsInverted { get; set; }

    public bool? IsPadded { get; set; }

    public static Dictionary<string, char?> PaddingCharacters => new() { ["Zero"] = '0', ["Space"] = ' ' };

    public char? PaddingCharacter { get; set; }

    public byte? DigitCount { get; set; }

    public byte? DigitCheckedSum { get; set; }

    public byte? DecimalPointCheckedSum { get; set; }

    public byte? SubstringStart { get; set; }

    public byte? SubstringEnd { get; set; }

    public class DigitFormatting
    {
        public byte Digit { get; init; }

        private bool _isDigitChecked;
        public bool IsDigitChecked
        {
            get => _isDigitChecked;
            set
            {
                if (_isDigitChecked != value)
                {
                    _isDigitChecked = value;
                    if (value)
                    {
                        DigitCheckedSum = (byte)((DigitCheckedSum ?? 0) | (1 << (Digit - 1)));
                        return;
                    }
                    DigitCheckedSum = (byte)((DigitCheckedSum ?? 0) & ~(1 << (Digit - 1)));
                }
            }
        }

        private bool _isDecimalPointChecked;
        public bool IsDecimalPointChecked
        {
            get => _isDecimalPointChecked;
            set
            {
                if (_isDecimalPointChecked != value)
                {
                    _isDecimalPointChecked = value;
                    if (value)
                    {

                        DecimalPointCheckedSum = (byte)((DecimalPointCheckedSum ?? 0) | (1 << (Digit - 1)));
                        return;
                    }
                    DecimalPointCheckedSum = (byte)((DecimalPointCheckedSum ?? 0) & ~(1 << (Digit - 1)));;
                }
            }
        }

        private static byte? _digitCheckedSum;
        public static byte? DigitCheckedSum
        {
            get => _digitCheckedSum;
            set
            {
                if (_digitCheckedSum != value)
                {
                    _digitCheckedSum = value;
                    SumDigitCheckedChanged?.Invoke(null, EventArgs.Empty);
                }
            }
        }

        private static byte? _decimalPointCheckedSum;
        public static byte? DecimalPointCheckedSum
        {
            get => _decimalPointCheckedSum;
            set
            {
                if (_decimalPointCheckedSum != value)
                {
                    _decimalPointCheckedSum = value;
                    SumDecimalPointCheckedChanged?.Invoke(null, EventArgs.Empty);
                }
            }
        }

        public static event EventHandler SumDigitCheckedChanged;

        public static event EventHandler SumDecimalPointCheckedChanged;
    }

    public OutputCreator[] OutputCreator { get; set; }

    public ObservableCollection<PreconditionModel> Preconditions { get; set; }
}