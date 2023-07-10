using System;
using System.Linq;
using System.Collections.Generic;
using Device_Interface_Manager.MSFSProfiles.PMDG;

namespace Device_Interface_Manager.MVVM.Model;
public class InputCreatorModel
{
    public int? SelectedSwitch { get; set; }

    public int?[] Switches { get; set; }

    private string _selectedEventType = ProfileCreatorModel.PMDG737;
    public string SelectedEventType
    {
        get => _selectedEventType;
        set
        {
            if (_selectedEventType != value && value is not null)
            {
                _selectedEventType = value;

                if (value == ProfileCreatorModel.MSFSSimConnect || value == ProfileCreatorModel.RPN)
                {
                    PMDGEvent = null;
                    PMDGMouseEventPress = null;
                    PMDGMouseEventRelease = null;
                }
            }
        }
    }

    public string[] EventType { get; set; } = new string[] { ProfileCreatorModel.MSFSSimConnect, ProfileCreatorModel.RPN, ProfileCreatorModel.PMDG737 };

    public PMDG_NG3_SDK.PMDGEvents? PMDGEvent { get; set; }

    private string _searchPMDGEventsText;
    public string SearchPMDGEventsText
    {
        get => _searchPMDGEventsText;
        set
        {
            if (_searchPMDGEventsText != value)
            {
                _searchPMDGEventsText = value;

                if (Enum.TryParse<PMDG_NG3_SDK.PMDGEvents>(value, true, out var result))
                {
                    PMDGEvent = result;
                }
            }
        }
    }

    public PMDG_NG3_SDK.PMDGEvents[] PMDGEvents => string.IsNullOrEmpty(SearchPMDGEventsText)
        ? (PMDG_NG3_SDK.PMDGEvents[])Enum.GetValues(typeof(PMDG_NG3_SDK.PMDGEvents))
        : ((PMDG_NG3_SDK.PMDGEvents[])Enum.GetValues(typeof(PMDG_NG3_SDK.PMDGEvents))).Where(name => name.ToString().Contains(SearchPMDGEventsText, StringComparison.OrdinalIgnoreCase)).ToArray();

    public KeyValuePair<string, uint>? PMDGMouseEventPress { get; set; }

    private string _searchPMDGMouseEventsTextPress;
    public string SearchPMDGMouseEventsTextPress
    {
        get => _searchPMDGMouseEventsTextPress;
        set
        {
            if (_searchPMDGMouseEventsTextPress != value)
            {
                _searchPMDGMouseEventsTextPress = value;

                KeyValuePair<string, uint> matchingEvent = PMDGMouseFlags.FirstOrDefault(pair => pair.Key.Equals(value, StringComparison.OrdinalIgnoreCase));
                if (matchingEvent.Key is not null)
                {
                    PMDGMouseEventPress = matchingEvent;
                }
            }
        }
    }

    public KeyValuePair<string, uint>[] PMDGMouseEventsPress => string.IsNullOrEmpty(SearchPMDGMouseEventsTextPress)
    ? PMDGMouseFlags.ToArray()
    : PMDGMouseFlags.Where(pair => pair.Key.Contains(SearchPMDGMouseEventsTextPress, StringComparison.OrdinalIgnoreCase)).ToArray();

    public KeyValuePair<string, uint>? PMDGMouseEventRelease { get; set; }

    private string _searchPMDGMouseEventsTextRelease;
    public string SearchPMDGMouseEventsTextRelease
    {
        get => _searchPMDGMouseEventsTextRelease;
        set
        {
            if (_searchPMDGMouseEventsTextRelease != value)
            {
                _searchPMDGMouseEventsTextRelease = value;

                KeyValuePair<string, uint> matchingEvent = PMDGMouseFlags.FirstOrDefault(pair => pair.Key.Equals(value, StringComparison.OrdinalIgnoreCase));
                if (matchingEvent.Key is not null)
                {
                    PMDGMouseEventRelease = matchingEvent;
                }
            }
        }
    }

    public KeyValuePair<string, uint>[] PMDGMouseEventsRelease => string.IsNullOrEmpty(SearchPMDGMouseEventsTextRelease)
    ? PMDGMouseFlags.ToArray()
    : PMDGMouseFlags.Where(pair => pair.Key.Contains(SearchPMDGMouseEventsTextRelease, StringComparison.OrdinalIgnoreCase)).ToArray();

    public string Event { get; set; }

    public uint? EventDataPress { get; set; }

    public uint? EventDataRelease { get; set; }

    private static Dictionary<string, uint> PMDGMouseFlags => new()
    {
        { "RightSingle", PMDG_NG3_SDK.MOUSE_FLAG_RIGHTSINGLE },
        { "MiddleSingle", PMDG_NG3_SDK.MOUSE_FLAG_MIDDLESINGLE },
        { "LeftSingle", PMDG_NG3_SDK.MOUSE_FLAG_LEFTSINGLE },
        { "RightDouble", PMDG_NG3_SDK.MOUSE_FLAG_RIGHTDOUBLE },
        { "MiddleDouble", PMDG_NG3_SDK.MOUSE_FLAG_MIDDLEDOUBLE },
        { "LeftDouble", PMDG_NG3_SDK.MOUSE_FLAG_LEFTDOUBLE },
        { "RightDrag", PMDG_NG3_SDK.MOUSE_FLAG_RIGHTDRAG },
        { "MiddleDrag", PMDG_NG3_SDK.MOUSE_FLAG_MIDDLEDRAG },
        { "LeftDrag", PMDG_NG3_SDK.MOUSE_FLAG_LEFTDRAG },
        { "Move", PMDG_NG3_SDK.MOUSE_FLAG_MOVE },
        { "DownRepeat", PMDG_NG3_SDK.MOUSE_FLAG_DOWN_REPEAT },
        { "RightRelease", PMDG_NG3_SDK.MOUSE_FLAG_RIGHTRELEASE },
        { "MiddleRelease", PMDG_NG3_SDK.MOUSE_FLAG_MIDDLERELEASE },
        { "LeftRelease", PMDG_NG3_SDK.MOUSE_FLAG_LEFTRELEASE },
        { "WheelFlip", PMDG_NG3_SDK.MOUSE_FLAG_WHEEL_FLIP },
        { "WheelSkip", PMDG_NG3_SDK.MOUSE_FLAG_WHEEL_SKIP },
        { "WheelUp", PMDG_NG3_SDK.MOUSE_FLAG_WHEEL_UP },
        { "WheelDown", PMDG_NG3_SDK.MOUSE_FLAG_WHEEL_DOWN }
    };
}