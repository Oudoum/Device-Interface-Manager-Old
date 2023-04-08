using System;
using System.Windows.Forms;
using static Device_Interface_Manager.MSFSProfiles.PMDG.B737.NG_MCP_777_USB_MAPPING;
using static Device_Interface_Manager.MSFSProfiles.PMDG.PMDG_NG3_SDK;

namespace Device_Interface_Manager.MSFSProfiles.PMDG.B737;

public class NG_MCP_777_USB_MAPPING_CON
{
    public ConvertedEVENT C_L_INBD { get; init; }
    public ConvertedEVENT C_LWR_CTRL { get; init; }
    public ConvertedEVENT C_R_INBD { get; init; }
    public ConvertedEVENT C_ENG { get; init; }
    public ConvertedEVENT C_STAT { get; init; }
    public ConvertedEVENT C_ELEC { get; init; }
    public ConvertedEVENT C_HYD { get; init; }
    public ConvertedEVENT C_FUEL { get; init; }
    public ConvertedEVENT C_AIR { get; init; }
    public ConvertedEVENT C_DOOR { get; init; }
    public ConvertedEVENT C_GEAR { get; init; }
    public ConvertedEVENT C_FCTL { get; init; }
    public ConvertedEVENT C_CHKL { get; init; }
    public ConvertedEVENT C_COMM { get; init; }
    public ConvertedEVENT C_NAV { get; init; }
    public ConvertedEVENT C_CANC_RCL { get; init; }

    public class ConvertedEVENT
    {
        public PMDGEvents ConvertedControlEvent { get; init; }
        public uint ConvertedMouseEvent { get; init; }
    }

    public NG_MCP_777_USB_MAPPING_CON(NG_MCP_777_USB_MAPPING mAPPING)
    {
        C_L_INBD = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.L_INBD), ConvertedMouseEvent = GetMouseEvent(mAPPING.L_INBD), };
        C_LWR_CTRL = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.LWR_CTRL), ConvertedMouseEvent = GetMouseEvent(mAPPING.LWR_CTRL), };
        C_R_INBD = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.R_INBD), ConvertedMouseEvent = GetMouseEvent(mAPPING.R_INBD), };
        C_ENG = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.ENG), ConvertedMouseEvent = GetMouseEvent(mAPPING.ENG), };
        C_STAT = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.STAT), ConvertedMouseEvent = GetMouseEvent(mAPPING.STAT), };
        C_ELEC = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.ELEC), ConvertedMouseEvent = GetMouseEvent(mAPPING.ELEC), };
        C_HYD = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.HYD), ConvertedMouseEvent = GetMouseEvent(mAPPING.HYD), };
        C_FUEL = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.FUEL), ConvertedMouseEvent = GetMouseEvent(mAPPING.FUEL), };
        C_AIR = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.AIR), ConvertedMouseEvent = GetMouseEvent(mAPPING.AIR), };
        C_DOOR = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.DOOR), ConvertedMouseEvent = GetMouseEvent(mAPPING.DOOR), };
        C_GEAR = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.GEAR), ConvertedMouseEvent = GetMouseEvent(mAPPING.GEAR), };
        C_FCTL = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.FCTL), ConvertedMouseEvent = GetMouseEvent(mAPPING.FCTL), };
        C_CHKL = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.CHKL), ConvertedMouseEvent = GetMouseEvent(mAPPING.CHKL), };
        C_COMM = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.COMM), ConvertedMouseEvent = GetMouseEvent(mAPPING.COMM), };
        C_NAV = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.NAV), ConvertedMouseEvent = GetMouseEvent(mAPPING.NAV), };
        C_CANC_RCL = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.CANC_RCL), ConvertedMouseEvent = GetMouseEvent(mAPPING.CANC_RCL), };

    }

    private static PMDGEvents GetControlEvent(EVENT eVENT)
    {
        try
        {
            return Enum.Parse<PMDGEvents>(eVENT.ControlEvent);
        }
        catch (Exception e)
        {

            MessageBox.Show(e.Message);
            return 0;
        }
    }

    private static uint GetMouseEvent(EVENT eVENT)
    {
        return eVENT.MouseEvent switch
        {
            nameof(MOUSE_FLAG_RIGHTSINGLE) => MOUSE_FLAG_RIGHTSINGLE,
            nameof(MOUSE_FLAG_MIDDLESINGLE) => MOUSE_FLAG_MIDDLESINGLE,
            nameof(MOUSE_FLAG_LEFTSINGLE) => MOUSE_FLAG_LEFTSINGLE,
            nameof(MOUSE_FLAG_RIGHTDOUBLE) => MOUSE_FLAG_RIGHTDOUBLE,
            nameof(MOUSE_FLAG_MIDDLEDOUBLE) => MOUSE_FLAG_MIDDLEDOUBLE,
            nameof(MOUSE_FLAG_LEFTDOUBLE) => MOUSE_FLAG_LEFTDOUBLE,
            nameof(MOUSE_FLAG_RIGHTDRAG) => MOUSE_FLAG_RIGHTDRAG,
            nameof(MOUSE_FLAG_MIDDLEDRAG) => MOUSE_FLAG_MIDDLEDRAG,
            nameof(MOUSE_FLAG_LEFTDRAG) => MOUSE_FLAG_LEFTDRAG,
            nameof(MOUSE_FLAG_MOVE) => MOUSE_FLAG_MOVE,
            nameof(MOUSE_FLAG_DOWN_REPEAT) => MOUSE_FLAG_DOWN_REPEAT,
            nameof(MOUSE_FLAG_RIGHTRELEASE) => MOUSE_FLAG_RIGHTRELEASE,
            nameof(MOUSE_FLAG_MIDDLERELEASE) => MOUSE_FLAG_MIDDLERELEASE,
            nameof(MOUSE_FLAG_LEFTRELEASE) => MOUSE_FLAG_LEFTRELEASE,
            nameof(MOUSE_FLAG_WHEEL_FLIP) => MOUSE_FLAG_WHEEL_FLIP,
            nameof(MOUSE_FLAG_WHEEL_SKIP) => MOUSE_FLAG_WHEEL_SKIP,
            nameof(MOUSE_FLAG_WHEEL_UP) => MOUSE_FLAG_WHEEL_UP,
            nameof(MOUSE_FLAG_WHEEL_DOWN) => MOUSE_FLAG_WHEEL_DOWN,
            _ => 0,
        };
    }
}