using System.Runtime.InteropServices;

namespace Device_Interface_Manager.Devices.interfaceIT.USB;

public class InterfaceIT_BoardInfo
{
    public class Device
    {
        public int Id { get; init; }
        public uint Session { get; init; }
        public string BoardType { get; init; }
        public string SerialNumber { get; init; }
        public BOARDCAPS DeviceInfo { get; init; }
    }

    public readonly struct BOARDCAPS
    {
        public int LEDCount { get; }              // Total number of LED outputs
        public int LEDFirst { get; }              // First LED output number
        public int LEDLast { get; }               // Last LED output number
        public int SwitchCount { get; }           // Total number of switches
        public int SwitchFirst { get; }           // First switch input number
        public int SwitchLast { get; }            // Last switch input number
        public int SevenSegmentCount { get; }     // Total number of 7 Segment output (future)
        public int SevenSegmentFirst { get; }     // First 7 Segment output number (future)
        public int SevenSegmentLast { get; }      // Last 7 Segment output number (future)
        public int DatalineCount { get; }         // Total datalines
        public int DatalineFirst { get; }         // First dataline number
        public int DatalineLast { get; }          // Last dataline number
        public int ServoController { get; }       // Total servo controllers
        public int ServoControllerFirst { get; }  // First servo controller
        public int ServoControllerLast { get; }   // Last servo controller
        
        #pragma warning disable IDE0051 // Remove unused private members
        private readonly int reserved4;
        private readonly int reserved5;
        private readonly int reserved6;
        private readonly int reserved7;
        private readonly int reserved8;
        private readonly int reserved9;
        #pragma warning restore IDE0051 // Remove unused private members

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        private readonly string _boardType;       // string containing the board type identifier
        public readonly string BoardType => _boardType;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        private readonly string _manufactureDate; // string containing the manufacture date of the board
        public readonly string ManufactureDate => _manufactureDate;

        public Features Features { get; }         // Features that are currently enabled on the board
        public int UpdateLevel { get; }           // Update level that is currently applied to this board
    }

    public enum Features
    {
        INTERFACEIT_FEATURE_NONE,

        INTERFACEIT_FEATURE_INPUT_SWITCHES = 0x00000001,
        INTERFACEIT_FEATURE_INPUT_RC = 0x00000002,
        INTERFACEIT_FEATURE_INPUT_SPI = 0x00000004,
        INTERFACEIT_FEATURE_INPUT_DATALINE = 0x00000008,
        INTERFACEIT_FEATURE_INPUT_IIC = 0x00000010,
        INTERFACEIT_FEATURE_INPUT_RESERVED6 = 0x00000020,
        INTERFACEIT_FEATURE_INPUT_RESERVED7 = 0x00000040,
        INTERFACEIT_FEATURE_INPUT_RESERVED8 = 0x00000080,

        INTERFACEIT_FEATURE_OUTPUT_LED = 0x00000100,
        INTERFACEIT_FEATURE_OUTPUT_LCD = 0x00000200,
        INTERFACEIT_FEATURE_OUTPUT_7SEGMENT = 0x00000400,
        INTERFACEIT_FEATURE_OUTPUT_SPI = 0x00000800,
        INTERFACEIT_FEATURE_OUTPUT_IIC = 0x00001000,
        INTERFACEIT_FEATURE_OUTPUT_DATALINE = 0x00002000,
        INTERFACEIT_FEATURE_OUTPUT_SERVO = 0x00004000,
        INTERFACEIT_FEATURE_OUTPUT_RESERVED16 = 0x00008000,

        INTERFACEIT_FEATURE_SPECIAL_BRIGHTNESS = 0x00010000,
        INTERFACEIT_FEATURE_SPECIAL_ANALOG_INPUT = 0x00020000,
        INTERFACEIT_FEATURE_SPECIAL_ANALOG16_INPUT = 0x00040000,
    }
}