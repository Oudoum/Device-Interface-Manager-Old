using System;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Device_Interface_Manager.MSFSProfiles.PMDG;

namespace Device_Interface_Manager.MVVM.ViewModel;
public partial class TestProfileViewModel : ObservableObject
{

    [ObservableProperty]
    private bool _isComboBoxOpen;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PMDGEvents))]
    private string _searchPMDGEventsText;

    public PMDG_NG3_SDK.PMDGEvents[] PMDGEvents => string.IsNullOrEmpty(SearchPMDGEventsText)
    ? (PMDG_NG3_SDK.PMDGEvents[])Enum.GetValues(typeof(PMDG_NG3_SDK.PMDGEvents))
    : ((PMDG_NG3_SDK.PMDGEvents[])Enum.GetValues(typeof(PMDG_NG3_SDK.PMDGEvents))).Where(name => name.ToString().Contains(SearchPMDGEventsText)).OrderBy(name => name).ToArray();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PMDGDataFieldNames))]
    private string _searchPMDGDataText;

    public string[] PMDGDataFieldNames => string.IsNullOrEmpty(SearchPMDGDataText)
        ? typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetFields().Select(field => field.Name).Take(typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetFields().Length - 1).ToArray()
        : typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetFields().Select(field => field.Name).Where(name => name.Contains(SearchPMDGDataText)).OrderBy(name => name).Take(typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetFields().Length - 1).ToArray();

    public TestProfileViewModel()
    {

    }
}