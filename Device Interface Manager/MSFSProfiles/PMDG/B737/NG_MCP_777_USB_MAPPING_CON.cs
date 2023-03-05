using System;
using System.Windows.Forms;
using static Device_Interface_Manager.MSFSProfiles.PMDG.B737.NG_MCP_777_USB_MAPPING;
using static Device_Interface_Manager.MSFSProfiles.PMDG.PMDG_NG3_SDK;

namespace Device_Interface_Manager.MSFSProfiles.PMDG.B737
{
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
            this.C_L_INBD = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.L_INBD), ConvertedMouseEvent = GetMouseEvent(mAPPING.L_INBD), };
            this.C_LWR_CTRL = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.LWR_CTRL), ConvertedMouseEvent = GetMouseEvent(mAPPING.LWR_CTRL), };
            this.C_R_INBD = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.R_INBD), ConvertedMouseEvent = GetMouseEvent(mAPPING.R_INBD), };
            this.C_ENG = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.ENG), ConvertedMouseEvent = GetMouseEvent(mAPPING.ENG), };
            this.C_STAT = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.STAT), ConvertedMouseEvent = GetMouseEvent(mAPPING.STAT), };
            this.C_ELEC = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.ELEC), ConvertedMouseEvent = GetMouseEvent(mAPPING.ELEC), };
            this.C_HYD = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.HYD), ConvertedMouseEvent = GetMouseEvent(mAPPING.HYD), };
            this.C_FUEL = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.FUEL), ConvertedMouseEvent = GetMouseEvent(mAPPING.FUEL), };
            this.C_AIR = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.AIR), ConvertedMouseEvent = GetMouseEvent(mAPPING.AIR), };
            this.C_DOOR = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.DOOR), ConvertedMouseEvent = GetMouseEvent(mAPPING.DOOR), };
            this.C_GEAR = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.GEAR), ConvertedMouseEvent = GetMouseEvent(mAPPING.GEAR), };
            this.C_FCTL = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.FCTL), ConvertedMouseEvent = GetMouseEvent(mAPPING.FCTL), };
            this.C_CHKL = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.CHKL), ConvertedMouseEvent = GetMouseEvent(mAPPING.CHKL), };
            this.C_COMM = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.COMM), ConvertedMouseEvent = GetMouseEvent(mAPPING.COMM), };
            this.C_NAV = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.NAV), ConvertedMouseEvent = GetMouseEvent(mAPPING.NAV), };
            this.C_CANC_RCL = new ConvertedEVENT { ConvertedControlEvent = GetControlEvent(mAPPING.CANC_RCL), ConvertedMouseEvent = GetMouseEvent(mAPPING.CANC_RCL), };

        }

        private PMDGEvents GetControlEvent(EVENT eVENT)
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

        private uint GetMouseEvent(EVENT eVENT)
        {
            switch (eVENT.MouseEvent)
            {
                case nameof(MOUSE_FLAG_RIGHTSINGLE):
                    return MOUSE_FLAG_RIGHTSINGLE;

                case nameof(MOUSE_FLAG_MIDDLESINGLE):
                    return MOUSE_FLAG_MIDDLESINGLE;

                case nameof(MOUSE_FLAG_LEFTSINGLE):
                    return MOUSE_FLAG_LEFTSINGLE;

                case nameof(MOUSE_FLAG_RIGHTDOUBLE):
                    return MOUSE_FLAG_RIGHTDOUBLE;

                case nameof(MOUSE_FLAG_MIDDLEDOUBLE):
                    return MOUSE_FLAG_MIDDLEDOUBLE;

                case nameof(MOUSE_FLAG_LEFTDOUBLE):
                    return MOUSE_FLAG_LEFTDOUBLE;

                case nameof(MOUSE_FLAG_RIGHTDRAG):
                    return MOUSE_FLAG_RIGHTDRAG;

                case nameof(MOUSE_FLAG_MIDDLEDRAG):
                    return MOUSE_FLAG_MIDDLEDRAG;

                case nameof(MOUSE_FLAG_LEFTDRAG):
                    return MOUSE_FLAG_LEFTDRAG;

                case nameof(MOUSE_FLAG_MOVE):
                    return MOUSE_FLAG_MOVE;

                case nameof(MOUSE_FLAG_DOWN_REPEAT):
                    return MOUSE_FLAG_DOWN_REPEAT;

                case nameof(MOUSE_FLAG_RIGHTRELEASE):
                    return MOUSE_FLAG_RIGHTRELEASE;

                case nameof(MOUSE_FLAG_MIDDLERELEASE):
                    return MOUSE_FLAG_MIDDLERELEASE;

                case nameof(MOUSE_FLAG_LEFTRELEASE):
                    return MOUSE_FLAG_LEFTRELEASE;

                case nameof(MOUSE_FLAG_WHEEL_FLIP):
                    return MOUSE_FLAG_WHEEL_FLIP;

                case nameof(MOUSE_FLAG_WHEEL_SKIP):
                    return MOUSE_FLAG_WHEEL_SKIP;

                case nameof(MOUSE_FLAG_WHEEL_UP):
                    return MOUSE_FLAG_WHEEL_UP;

                case nameof(MOUSE_FLAG_WHEEL_DOWN):
                    return MOUSE_FLAG_WHEEL_DOWN;
            }
            return 0;
        }
    }
}