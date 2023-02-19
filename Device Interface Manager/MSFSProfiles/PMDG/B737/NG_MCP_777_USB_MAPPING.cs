namespace Device_Interface_Manager.MSFSProfiles.PMDG.B737
{
    public class NG_MCP_777_USB_MAPPING
    {
        public EVENT L_INBD { get; init; } = new EVENT();
        public EVENT LWR_CTRL { get; init; } = new EVENT();
        public EVENT R_INBD { get; init; } = new EVENT();
        public EVENT ENG { get; init; } = new EVENT();
        public EVENT STAT { get; init; } = new EVENT();
        public EVENT ELEC { get; init; } = new EVENT();
        public EVENT HYD { get; init; } = new EVENT();
        public EVENT FUEL { get; init; } = new EVENT();
        public EVENT AIR { get; init; } = new EVENT();
        public EVENT DOOR { get; init; } = new EVENT();
        public EVENT GEAR { get; init; } = new EVENT();
        public EVENT FCTL { get; init; } = new EVENT();
        public EVENT CHKL { get; init; } = new EVENT();
        public EVENT COMM { get; init; } = new EVENT();
        public EVENT NAV { get; init; } = new EVENT();
        public EVENT CANC_RCL { get; init; } = new EVENT();

        public class EVENT
        {
            public string ControlEvent { get; set; } = "ControlEvent_COMES_IN_HERE";
            public string MouseEvent { get; set; } = "MouseEvent_COMES_IN_HERE";
        }
    }
}