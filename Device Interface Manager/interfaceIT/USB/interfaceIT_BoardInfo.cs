using System.Runtime.InteropServices;

namespace Device_Interface_Manager.interfaceIT.USB;

public class InterfaceIT_BoardInfo
{
    public class Device
    {
        public int Id { get; init; }
        public uint Session { get; init; }
        public string BoardType { get; init; }
        public string SerialNumber { get; init; }
        public BoardInformationStructure.BOARDCAPS DeviceInfo { get; init; }
    }

    public class BoardInformationStructure
    {
        public struct BOARDCAPS
        {
            public int nLEDCount;              // Total number of LED outputs
            public int nLEDFirst;              // First LED output number
            public int nLEDLast;               // Last LED output number
            public int nSwitchCount;           // Total number of switches
            public int nSwitchFirst;           // First switch input number
            public int nSwitchLast;            // Last switch input number
            public int n7SegmentCount;         // Total number of 7 Segment output (future)
            public int n7SegmentFirst;         // First 7 Segment output number (future)
            public int n7SegmentLast;          // Last 7 Segment output number (future)
            public int nDatalineCount;         // Total datalines
            public int nDatalineFirst;         // First dataline number
            public int nDatalineLast;          // Last dataline number
            public int nServoController;       // Total servo controllers
            public int nServoControllerFirst;  // First servo controller
            public int nServoControllerLast;   // Last servo controller
            public int nReserved4;
            public int nReserved5;
            public int nReserved6;
            public int nReserved7;
            public int nReserved8;
            public int nReserved9;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string szBoardType;         // Null-terminated string containing the board type identifier
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
            public string szManufactureDate;   // Null-terminated string containing the manufacture date of the board
            public int dwFeatures;             // Features that are currently enabled on the board
            public int nUpdateLevel;           // Update level that is currently applied to this board
        }
    }

    public class Features
    {
        //Features
        public const int INTERFACEIT_FEATURE_NONE = 0x00000000;

        public const int INTERFACEIT_FEATURE_INPUT_SWITCHES = 0x00000001;
        public const int INTERFACEIT_FEATURE_INPUT_RC = 0x00000002;
        public const int INTERFACEIT_FEATURE_INPUT_SPI = 0x00000004;
        public const int INTERFACEIT_FEATURE_INPUT_DATALINE = 0x00000008;
        public const int INTERFACEIT_FEATURE_INPUT_IIC = 0x00000010;
        public const int INTERFACEIT_FEATURE_INPUT_RESERVED6 = 0x00000020;
        public const int INTERFACEIT_FEATURE_INPUT_RESERVED7 = 0x00000040;
        public const int INTERFACEIT_FEATURE_INPUT_RESERVED8 = 0x00000080;

        public const int INTERFACEIT_FEATURE_OUTPUT_LED = 0x00000100;
        public const int INTERFACEIT_FEATURE_OUTPUT_LCD = 0x00000200;
        public const int INTERFACEIT_FEATURE_OUTPUT_7SEGMENT = 0x00000400;
        public const int INTERFACEIT_FEATURE_OUTPUT_SPI = 0x00000800;
        public const int INTERFACEIT_FEATURE_OUTPUT_IIC = 0x00001000;
        public const int INTERFACEIT_FEATURE_OUTPUT_DATALINE = 0x00002000;
        public const int INTERFACEIT_FEATURE_OUTPUT_SERVO = 0x00004000;
        public const int INTERFACEIT_FEATURE_OUTPUT_RESERVED16 = 0x00008000;

        public const int INTERFACEIT_FEATURE_SPECIAL_BRIGHTNESS = 0x00010000;
        public const int INTERFACEIT_FEATURE_SPECIAL_ANALOG_INPUT = 0x00020000;
        public const int INTERFACEIT_FEATURE_SPECIAL_ANALOG16_INPUT = 0x00040000;
    }
}