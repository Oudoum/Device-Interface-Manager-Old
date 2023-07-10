using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Device_Interface_Manager.MSFSProfiles.PMDG;

namespace Device_Interface_Manager.MVVM.Model;
public class OutputCreatorModel
{
    public int? SelectedLED { get; set; }

    public int?[] LEDs { get; set; }

    private string _selectedDataType = ProfileCreatorModel.PMDG737;
    public string SelectedDataType
    {
        get => _selectedDataType;
        set
        {
            if (_selectedDataType != value && value is not null)
            {
                _selectedDataType = value;

                if (value == ProfileCreatorModel.MSFSSimConnect)
                {
                    PMDGDataFieldName = null;
                }
            }
        }
    }

    public string[] DataType { get; set; } = new string[] { ProfileCreatorModel.MSFSSimConnect, ProfileCreatorModel.PMDG737 };

    public string PMDGDataFieldName { get; set; }

    private string _searchPMDGDataText;
    public string SearchPMDGDataText
    {
        get => _searchPMDGDataText;
        set
        {
            if (_searchPMDGDataText != value)
            {
                _searchPMDGDataText = value;

                string matchingData = PMDGDataFieldNames.FirstOrDefault(s => s.Equals(value, StringComparison.OrdinalIgnoreCase));
                if (matchingData is not null)
                {
                    PMDGDataFieldName = matchingData;
                }
            }
        }
    }

    public string[] PMDGDataFieldNames => string.IsNullOrEmpty(SearchPMDGDataText)
        ? typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetFields().Select(field => field.Name).Take(typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetFields().Length - 1).ToArray()
        : typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetFields().Select(field => field.Name).Where(name => name.Contains(SearchPMDGDataText, StringComparison.OrdinalIgnoreCase)).Take(typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetFields().Length - 1).ToArray();

    private int? _pMDGStructArrayNum;
    public int? PMDGStructArrayNum
    {
        get
        {
            if (_pMDGStructArrayNum is null)
            {
                return _pMDGStructArrayNum = PMDGStructArrayNums?.FirstOrDefault();

            }
            return _pMDGStructArrayNum;
        }
        set
        {
            if (value != _pMDGStructArrayNum)
            {
                _pMDGStructArrayNum = value;
            }
        }
    }

    public int?[] PMDGStructArrayNums => string.IsNullOrEmpty(PMDGDataFieldName)
        ? Array.Empty<int?>()
        : typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetField(PMDGDataFieldName).GetCustomAttribute<MarshalAsAttribute>()?.SizeConst is int size
        ? new int?[size].Select((_, i) => i).Cast<int?>().ToArray()
        : null;

    public bool IsComparisonValueEnabled => !string.IsNullOrEmpty(PMDGDataFieldName)
        && typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetField(PMDGDataFieldName)?.FieldType != typeof(bool)
        && typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetField(PMDGDataFieldName)?.FieldType != typeof(bool[]);


    private int? _comparisonValue;
    public int? ComparisonValue
    {
        get => string.IsNullOrEmpty(PMDGDataFieldName)
            || typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetField(PMDGDataFieldName)?.FieldType == typeof(bool)
            || typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetField(PMDGDataFieldName)?.FieldType == typeof(bool[])
            ? null
            : _comparisonValue;
        set => _comparisonValue = value;
    }

    public string Data { get; set; }

    public bool IsInverted { get; set; }
}