using System;
using System.Runtime.InteropServices;

namespace Device_Interface_Manager.Devices.interfaceIT.USB;

public class InterfaceIT_BoardInfo
{
    public class Device
    {
        public int Id { get; init; }
        public uint Session { get; init; }
        public InterfaceIT_BoardIDs BoardID { get; init; }
        public string BoardName { get; init; }
        public string SerialNumber { get; init; }
        public BoardInfo BoardInfo { get; init; }
    }

    public readonly struct BoardInfo
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

    [Flags]
    public enum Features : uint
    {
        None = 0x00000000,

        InputSwitches = 0x00000001,
        InputRC = 0x00000002,
        InputSPI = 0x00000004,
        InputDataLine = 0x00000008,
        InputIIC = 0x00000010,
        InputReserved6 = 0x00000020,
        InputReserved7 = 0x00000040,
        InputReserved8 = 0x00000080,

        OutputLED = 0x00000100,
        OutputLCD = 0x00000200,
        Output7Segment = 0x00000400,
        OutputSPI = 0x00000800,
        OutputIIC = 0x00001000,
        OutputDataLine = 0x00002000,
        OutputServo = 0x00004000,
        OutputReserved16 = 0x00008000,

        SpecialBrightness = 0x00010000,
        SpecialAnalogInput = 0x00020000,
        SpecialAnalog16Input = 0x00040000,
    }
}