using System;
using System.Linq;
using System.Collections.Generic;
using Device_Interface_Manager.MSFSProfiles.PMDG;

namespace Device_Interface_Manager.MVVM.Model;
public class InputCreatorModel
{
    public string InputType { get; set; }

    public string[] InputTypes { get; set; } = { ProfileCreatorModel.SWITCH};

    public int? Input { get; set; }

    public int?[] Switches { get; set; }

    public string EventType { get; set; }

    public string[] EventTypes { get; set; } = { ProfileCreatorModel.MSFSSimConnect, ProfileCreatorModel.RPN, ProfileCreatorModel.PMDG737 };

    public PMDG_NG3_SDK.PMDGEvents? PMDGEvent { get; set; }

    private string _searchPMDGEvent;
    public string SearchPMDGEvent
    {
        get => _searchPMDGEvent;
        set
        {
            if (_searchPMDGEvent != value)
            {
                _searchPMDGEvent = value;

                if (Enum.TryParse<PMDG_NG3_SDK.PMDGEvents>(value, true, out var result))
                {
                    PMDGEvent = result;
                }
            }
        }
    }

    public PMDG_NG3_SDK.PMDGEvents[] PMDGEvents => string.IsNullOrEmpty(SearchPMDGEvent)
        ? (PMDG_NG3_SDK.PMDGEvents[])Enum.GetValues(typeof(PMDG_NG3_SDK.PMDGEvents))
        : ((PMDG_NG3_SDK.PMDGEvents[])Enum.GetValues(typeof(PMDG_NG3_SDK.PMDGEvents))).Where(name => name.ToString().Contains(SearchPMDGEvent, StringComparison.OrdinalIgnoreCase)).ToArray();

    public KeyValuePair<string, uint>? PMDGMousePress { get; set; }

    private string _searchPMDGMousePress;
    public string SearchPMDGMousePress
    {
        get => _searchPMDGMousePress;
        set
        {
            if (_searchPMDGMousePress != value)
            {
                _searchPMDGMousePress = value;

                KeyValuePair<string, uint> matchingEvent = PMDGMouseFlags.FirstOrDefault(pair => pair.Key.Equals(value, StringComparison.OrdinalIgnoreCase));
                if (matchingEvent.Key is not null)
                {
                    PMDGMousePress = matchingEvent;
                }
            }
        }
    }

    public KeyValuePair<string, uint>[] PMDGMousePressArray => string.IsNullOrEmpty(SearchPMDGMousePress)
    ? PMDGMouseFlags.ToArray()
    : PMDGMouseFlags.Where(pair => pair.Key.Contains(SearchPMDGMousePress, StringComparison.OrdinalIgnoreCase)).ToArray();

    public KeyValuePair<string, uint>? PMDGMouseRelease { get; set; }

    private string _searchPMDGMouseRelease;
    public string SearchPMDGMouseRelease
    {
        get => _searchPMDGMouseRelease;
        set
        {
            if (_searchPMDGMouseRelease != value)
            {
                _searchPMDGMouseRelease = value;

                KeyValuePair<string, uint> matchingEvent = PMDGMouseFlags.FirstOrDefault(pair => pair.Key.Equals(value, StringComparison.OrdinalIgnoreCase));
                if (matchingEvent.Key is not null)
                {
                    PMDGMouseRelease = matchingEvent;
                }
            }
        }
    }

    public KeyValuePair<string, uint>[] PMDGMouseReleaseArray => string.IsNullOrEmpty(SearchPMDGMouseRelease)
    ? PMDGMouseFlags.ToArray()
    : PMDGMouseFlags.Where(pair => pair.Key.Contains(SearchPMDGMouseRelease, StringComparison.OrdinalIgnoreCase)).ToArray();

    public string Event { get; set; }

    public uint? DataPress { get; set; }

    public uint? DataRelease { get; set; }

    private static Dictionary<string, uint> PMDGMouseFlags => new()
    {
        { "LeftSingle", PMDG_NG3_SDK.MOUSE_FLAG_LEFTSINGLE },
        { "LeftRelease", PMDG_NG3_SDK.MOUSE_FLAG_LEFTRELEASE },
        { "RightSingle", PMDG_NG3_SDK.MOUSE_FLAG_RIGHTSINGLE },
        { "RightRelease", PMDG_NG3_SDK.MOUSE_FLAG_RIGHTRELEASE },
        { "WheelUp", PMDG_NG3_SDK.MOUSE_FLAG_WHEEL_UP },
        { "WheelDown", PMDG_NG3_SDK.MOUSE_FLAG_WHEEL_DOWN }
        //{ "MiddleSingle", PMDG_NG3_SDK.MOUSE_FLAG_MIDDLESINGLE },
        //{ "RightDouble", PMDG_NG3_SDK.MOUSE_FLAG_RIGHTDOUBLE },
        //{ "MiddleDouble", PMDG_NG3_SDK.MOUSE_FLAG_MIDDLEDOUBLE },
        //{ "LeftDouble", PMDG_NG3_SDK.MOUSE_FLAG_LEFTDOUBLE },
        //{ "RightDrag", PMDG_NG3_SDK.MOUSE_FLAG_RIGHTDRAG },
        //{ "MiddleDrag", PMDG_NG3_SDK.MOUSE_FLAG_MIDDLEDRAG },
        //{ "LeftDrag", PMDG_NG3_SDK.MOUSE_FLAG_LEFTDRAG },
        //{ "Move", PMDG_NG3_SDK.MOUSE_FLAG_MOVE },
        //{ "DownRepeat", PMDG_NG3_SDK.MOUSE_FLAG_DOWN_REPEAT },
        //{ "MiddleRelease", PMDG_NG3_SDK.MOUSE_FLAG_MIDDLERELEASE },
        //{ "WheelFlip", PMDG_NG3_SDK.MOUSE_FLAG_WHEEL_FLIP },
        //{ "WheelSkip", PMDG_NG3_SDK.MOUSE_FLAG_WHEEL_SKIP },
    };
}