using Device_Interface_Manager.MVVM.Model;

namespace Device_Interface_Manager.interfaceIT.USB
{
    internal class InterfaceIT_BoardIDs
    {
        //Board type identifiers
        public const string INTERFACEIT_BOARD_ALL = null;

        // Misc
        // FDS-MFP - Origional version
        public const string FDS_MFP = "0201";

        // FDS-SYS boards - Older origional boards
        public const string FDSSYS1 = "32E1";

        public const string FDSSYS2 = "32E2";

        public const string FDSSYS3 = "32E0";

        public const string FDSSYS4 = "32E3";

        //Radios
        // Base
        public const string RADIO_CODE = "4C";

        // 737 NAV v1
        public const string FDS_737NG_NAV = "4C55";
        public const string FDS_737NG_NAV_ID = "55";

        // 737 COMM v1
        public const string FDS_737NG_COMM = "4C56";
        public const string FDS_737NG_COMM_ID = "56";

        // A320 Multi v1
        public const string FDS_A320_MULTI = "4C57";
        public const string FDS_A320_MULTI_ID = "57";

        // 737 NAV v2
        public const string FDS_737NG_NAV_V2 = "4C58";
        public const string FDS_737NG_NAV_V2_ID = "58";

        // 737 COMM v2
        public const string FDS_737NG_COMM_V2 = "4C59";
        public const string FDS_737NG_COMM_V2_ID = "59";

        // 737 NAV / COMM
        public const string FDS_737NG_NAVCOMM = "4C5A";
        public const string FDS_737NG_NAVCOMM_ID = "5A";

        // FDS-CDU
        public const string FDS_CDU = "3302";

        // FDS-A-ACP
        public const string FDS_A_ACP = "3303";

        // FDS-A-RMP
        public const string FDS_A_RMP = "3304";

        // FDS-XPNDR
        public const string FDS_XPNDR = "3305";

        // FDS-737NG-ELECT
        public const string FDS_737_ELECT = "3306";

        // FS-MFP v2
        public const string MFP_V2 = "3307";

        // FDS-CONTROLLER-MCP
        public const string FDS_CONTROLLER_MCP = "330A";

        // FDS-CONTROLLER-EFIS-CA
        public const string FDS_CONTROLLER_EFIS_CA = "330B";

        // FDS-CONTROLLER-EFIS-FO
        public const string FDS_CONTROLLER_EFIS_FO = "330C";

        // FDS-737NG-MCP-ELVL - Not production
        public const string FDS_737NG_MCP_ELVL = "3310";

        // FDS-737-EL-MCP
        public const string FDS_737_EL_MCP = "3311";

        public const string FDS_737_MX_MCP = "3311";

        // FDS-787-MCP
        public const string FDS_787NG_MCP = "3319";

        // FDS-A320-FCU
        public const string FDS_A320_FCU = "3316";

        // FDS_747_RADIO (MULTI_COMM)
        public const string FDS_747_RMP = "3318";

        public const string FDS_7X7_MCOMM = "3318";

        // FDS-CDU v9

        public const string FDS_CDU_V9 = "331A";

        // FDS-A-TCAS V1

        public const string FDS_A_TCAS = "331B";

        // FDS-A-ECAM v1

        public const string FDS_A_ECAM = "331C";

        // FDS-A-CLOCK v1
        public const string FDS_A_CLOCK = "331D";

        public const string FDS_A_RMP_V2 = "331E";

        public const string FDS_A_ACP_V2 = "331F";

        public const string A320_PEDESTAL = "3320";

        public const string A320_OH_ELECT_DISP = "3321";

        public const string FDS_A320_35VU = "3321";

        public const string FDS_IRS = "3322";

        public const string FDS_OM1 = "3323";

        public const string FDS_OE1 = "3324";

        public const string FDS_GM1 = "3325";

        public const string FDS_NDF_GDS_NCP = "3326";

        public const string FDS_777_MX_MCP = "3327";

        public const string FDS_DCP_EFIS = "3328";

        public const string FDS_747_MX_MCP = "3329";

        // 5 Position EFIS
        public const string FDS_737_PMX_EFIS_5_CA = "332A";

        // 5 Position EFIS
        public const string FDS_737_PMX_EFIS_5_FO = "332B";

        // 737 Pro MX MCP
        public const string FDS_737_PMX_MCP = "332C";

        // 737 Pro MX EFIS (Encoder) - CA
        public const string FDS_737_PMX_EFIS_E_CA = "332D";

        // 737 Pro MX EFIS (Encoder) - FO
        public const string FDS_737_PMX_EFIS_E_FO = "332E";

        // 737 MAX 
        public const string FDS_737_MAX_ABRAKE_EFIS = "332F";

        // 787 Tuning and Control Panel
        public const string FDS_787_TCP = "3330";

        // C17 AFCSP
        public const string FDS_C17_AFCSP = "33EF";

        // JetMAX Boards
        public const string JetMAX_737_MCP = "330F";

        public const string JetMAX_737_RADIO = "3401";

        public const string JetMAX_737_XPNDR = "3402";

        public const string JetMAX_777_MCP = "3403";

        public const string JetMAX_737_MCP_V2 = "3404";

        // interfaceIT™ Boards
        public const string IIT_HIO_32_64 = "4101";

        public const string IIT_HIO_64_128 = "4102";

        public const string IIT_HIO_128_256 = "4103";

        public const string IIT_HI_128 = "4105";

        public const string IIT_HRI_8 = "4106";

        public const string IIT_RELAY_8 = "4107";

        public const string IIT_DEV = "4108";

        public const string HIO_RELAY_8 = "4109";
    }
}