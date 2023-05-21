using System;
using System.Runtime.InteropServices;
using System.Text;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceIT_BoardInfo;

namespace Device_Interface_Manager.interfaceIT.USB;

public partial class InterfaceITAPI_Data
{

    //Switch Callback Definition
    public delegate void INTERFACEIT_KEY_NOTIFY_PROC(uint hSession, int nSwitch, int nDirection);
    public delegate void INTERFACEIT_KEY_NOTIFY_PROC_UINT(uint hSession, int nSwitch, uint nDirection);

    //Device Change Notification
    public delegate void INTERFACEIT_DEVICE_CHANGE_NOTIFY_PROC(int nAction);

    public struct DeviceNotification
    {
        //Device Notification
        public const int INTERFACEIT_DEVICE_REMOVAL = 0x01;
        public const int INTERFACEIT_DEVICE_ARRIVAL = 0x02;
    }

    public struct SwitchDirectionInfo
    {
        //Switch Direction Information
        public const int INTERFACEIT_SWITCH_DIR_UNKNOWN = 0xFF;
        public const int INTERFACEIT_SWITCH_DIR_UP = 0x0;
        public const int INTERFACEIT_SWITCH_DIR_DOWN = 0x1;
    }

    public struct BoardOptions
    {
        //Board Options
        public const int INTERFACEIT_BOARD_OPTION_NONE = 0x0;
        public const int INTERFACEIT_BOARD_OPTION_CDUKEYS = 0x1;     // CDU v9 does not need this.  Used for v7 CDU
        public const int INTERFACEIT_BOARD_OPTION_FORCE64 = 0x2;     // Required for the new FDS-CONTROLLER-MCP boards for relay to function
        public const int INTERFACEIT_BOARD_OPTION_RESERVED3 = 0x4;
    }

    public struct ErrorCodes
    {
        //Error Codes
        const int IITAPI_ERR_BASE = 0;

        public const int IITAPI_ERR_OK = IITAPI_ERR_BASE - 0;
        public const int IITAPI_ERR_CONTROLLERS_OPEN_FAILED = IITAPI_ERR_BASE - 1;
        public const int IITAPI_ERR_CONTROLLERS_ALREADY_OPENED = IITAPI_ERR_BASE - 2;
        public const int IITAPI_ERR_CONTROLLERS_NOT_OPENED = IITAPI_ERR_BASE - 3;
        public const int IITAPI_ERR_INVALID_HANDLE = IITAPI_ERR_BASE - 4;
        public const int IITAPI_ERR_INVALID_POINTER = IITAPI_ERR_BASE - 5;
        public const int IITAPI_ERR_INVALID_CONTROLLER_NAME = IITAPI_ERR_BASE - 6;
        public const int IITAPI_ERR_FAILED = IITAPI_ERR_BASE - 7;
        public const int IITAPI_ERR_INVALID_CONTROLLER_POINTER = IITAPI_ERR_BASE - 8;
        public const int IITAPI_ERR_INVALID_CALLBACK = IITAPI_ERR_BASE - 9;
        public const int IITAPI_ERR_RETRIEVING_CONTROLLER = IITAPI_ERR_BASE - 10;
        public const int IITAPI_ERR_NOT_ENABLED = IITAPI_ERR_BASE - 11;
        public const int IITAPI_ERR_BUFFER_NOT_LARGE_ENOUGH = IITAPI_ERR_BASE - 12;
        public const int IITAPI_ERR_BUFFER_NOT_LARGE_ENOUGHT = IITAPI_ERR_BASE - 12;   // Previous release typo
        public const int IITAPI_ERR_PARAMETER_LENGTH_INCORRECT = IITAPI_ERR_BASE - 13;
        public const int IITAPI_ERR_PARAMETER_OUT_OF_RANGE = IITAPI_ERR_BASE - 14;
        public const int IITAPI_ERR_FEATURE_NOT_AVAILABLE = IITAPI_ERR_BASE - 15;
        public const int IITAPI_ERR_ALREADY_ENABLED = IITAPI_ERR_BASE - 16;
        public const int IITAPI_ERR_NO_ITEMS = IITAPI_ERR_BASE - 17;
        public const int IITAPI_ERR_CONTROLLER_ALREADY_BOUND = IITAPI_ERR_BASE - 18;
        public const int IITAPI_ERR_NO_CONTROLLERS_FOUND = IITAPI_ERR_BASE - 19;
        public const int IITAPI_ERR_UNKNOWN = IITAPI_ERR_BASE - 20;
        public const int IITAPI_ERR_NOT_LICENSED = IITAPI_ERR_BASE - 21;
        public const int IITAPI_ERR_INVALID_LICENSE = IITAPI_ERR_BASE - 22;
        public const int IITAPI_ERR_ALREADY_LICENSED = IITAPI_ERR_BASE - 23;
        public const int IITAPI_ERR_GENERATING_ACTIVATIONID = IITAPI_ERR_BASE - 24;
        public const int IITAPI_ERR_EXPIRED_LICENSE = IITAPI_ERR_BASE - 25;
        public const int IITAPI_ERR_EXPIRED_TRIAL = IITAPI_ERR_BASE - 26;

        public static string GetErrorCodes(int code)
        {
            foreach (var field in typeof(ErrorCodes).GetFields())
            {
                if ((int)field.GetValue(null) == code)
                    return field.Name.ToString();
            }
            return "Error";
        }
    }


    //Main Functions
    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_OpenControllers();

    [LibraryImport("interfaceITAPI x64.dll", StringMarshalling = StringMarshalling.Utf8)]
    private static partial void interfaceIT_GetDeviceList(byte[] pBuffer, ref uint dwSize, string pBoardType);

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    internal static string[] interfaceIT_GetDeviceList()
    {
        uint bufferSize = 0;
        interfaceIT_GetDeviceList(null, ref bufferSize, null);
        byte[] deviceList = new byte[bufferSize];
        interfaceIT_GetDeviceList(deviceList, ref bufferSize, null);
        return Encoding.UTF8.GetString(deviceList).TrimEnd('\0').Split('\0');
    }

    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_CloseControllers();


    //Controller Functions
    [LibraryImport("interfaceITAPI x64.dll", StringMarshalling = StringMarshalling.Utf8)]
    internal static partial void interfaceIT_Bind(string pController, out uint phSession);

    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_UnBind(uint hSession);

    [DllImport("interfaceITAPI x64.dll")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time", Justification = "<Pending>")]
    internal static extern void interfaceIT_GetBoardInfo(uint hSession, out BOARDCAPS pbc);

    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_SetBoardOptions(uint hSession, uint dwOptions);

    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_GetTotalControllers(ref int pnControllerCount);


    //LED Functions
    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_LED_Enable(uint hSession, [MarshalAs(UnmanagedType.Bool)] bool bEnable);

    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_LED_Test(uint hSession, [MarshalAs(UnmanagedType.Bool)] bool bEnable);

    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_LED_Set(uint hSession, int nLED, [MarshalAs(UnmanagedType.Bool)] bool bOn);

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    internal static void interfaceIT_LED_Set(uint hSession, int nLED, double bOn)
    {
        interfaceIT_LED_Set(hSession, nLED, Convert.ToBoolean(bOn));
    }


    //Switch Functions
    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_Switch_Enable_Callback(uint hSession, [MarshalAs(UnmanagedType.Bool)] bool bEnable, INTERFACEIT_KEY_NOTIFY_PROC pROC);

    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_Switch_Enable_Callback(uint hSession, [MarshalAs(UnmanagedType.Bool)] bool bEnable, INTERFACEIT_KEY_NOTIFY_PROC_UINT pROC);

    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_Switch_Enable_Poll(uint hSession, [MarshalAs(UnmanagedType.Bool)] bool bEnable);

    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial int interfaceIT_Switch_Get_Item(uint hSession, out int pnSwitch, out int pnDirection);

    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_Switch_Get_State(uint hSession, out int nSwitch,out int pnState);  //Not tested


    //7 Segment Functions
    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_7Segment_Enable(uint hSession, [MarshalAs(UnmanagedType.Bool)] bool bEnable);

    [LibraryImport("interfaceITAPI x64.dll", StringMarshalling = StringMarshalling.Utf8)]
    internal static partial void interfaceIT_7Segment_Display(uint hSession, string pszData, int nStart);


    //Dataline Functions
    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_Dataline_Enable(uint hSession, [MarshalAs(UnmanagedType.Bool)] bool bEnable);

    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_Dataline_Set(uint hSession, int nDataline, [MarshalAs(UnmanagedType.Bool)] bool bOn);


    //Brightness Functions
    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_Brightness_Enable(uint hSession, [MarshalAs(UnmanagedType.Bool)] bool bEnable);

    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_Brightness_Set(uint hSession, int nBrightness);


    //Analog Input Functions
    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_Analog_Enable(uint hSession, [MarshalAs(UnmanagedType.Bool)] bool bEnable);

    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_Analog_GetValue(uint hSession, int nReserved, out int pnPos);

    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_Analog_GetValues(uint hSession, byte[] pbValues, ref int nValuesSize);  //Not tested


    //Device Change Notification
    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_Enable_DeviceChange_Notification_Callback([MarshalAs(UnmanagedType.Bool)] bool bEnable, INTERFACEIT_DEVICE_CHANGE_NOTIFY_PROC nAction);  //NOT WORKING!!!


    //Misc Functions
    [LibraryImport("interfaceITAPI x64.dll")]
    private static partial void interfaceIT_GetAPIVersion(byte[] pBuffer, ref uint dwSize);

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    internal static string interfaceIT_GetAPIVersion()
    {
        uint bufferSize = 0;
        interfaceIT_GetAPIVersion(null, ref bufferSize);
        byte[] aPIversion = new byte[bufferSize];
        interfaceIT_GetAPIVersion(aPIversion, ref bufferSize);
        return Encoding.UTF8.GetString(aPIversion);
    }


    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_EnableLogging([MarshalAs(UnmanagedType.Bool)] bool bEnable);


    internal static void InterfaceITEnable(Device device)
    {
        if ((device.DeviceInfo.dwFeatures & Features.INTERFACEIT_FEATURE_SPECIAL_ANALOG16_INPUT) != 0)
        {
            interfaceIT_Analog_Enable(device.Session, true);
        }

        if ((device.DeviceInfo.dwFeatures & Features.INTERFACEIT_FEATURE_SPECIAL_ANALOG_INPUT) != 0)
        {
            interfaceIT_Analog_Enable(device.Session, true);
        }

        if ((device.DeviceInfo.dwFeatures & Features.INTERFACEIT_FEATURE_SPECIAL_BRIGHTNESS) != 0)
        {
            interfaceIT_Brightness_Enable(device.Session, true);
        }

        if ((device.DeviceInfo.dwFeatures & Features.INTERFACEIT_FEATURE_OUTPUT_SERVO) != 0)
        {
            //Not available
        }

        if ((device.DeviceInfo.dwFeatures & Features.INTERFACEIT_FEATURE_OUTPUT_DATALINE) != 0)
        {
            interfaceIT_Dataline_Enable(device.Session, true);
        }

        if ((device.DeviceInfo.dwFeatures & Features.INTERFACEIT_FEATURE_OUTPUT_7SEGMENT) != 0)
        {
            interfaceIT_7Segment_Enable(device.Session, true);
        }

        if ((device.DeviceInfo.dwFeatures & Features.INTERFACEIT_FEATURE_OUTPUT_LED) != 0)
        {
            interfaceIT_LED_Enable(device.Session, true);
        }
    }


    internal static void InterfaceITDisable(Device device)
    {
        if ((device.DeviceInfo.dwFeatures & Features.INTERFACEIT_FEATURE_OUTPUT_LED) != 0)
        {
            for (int i = device.DeviceInfo.nLEDFirst; i <= device.DeviceInfo.nLEDLast; i++)
            {
                interfaceIT_LED_Set(device.Session, i, false);
            }
            interfaceIT_LED_Enable(device.Session, false);
        }

        if ((device.DeviceInfo.dwFeatures & Features.INTERFACEIT_FEATURE_INPUT_SWITCHES) != 0)
        {
            interfaceIT_Switch_Enable_Poll(device.Session, false);
        }

        if ((device.DeviceInfo.dwFeatures & Features.INTERFACEIT_FEATURE_OUTPUT_7SEGMENT) != 0)
        {
            for (int i = device.DeviceInfo.n7SegmentFirst; i <= device.DeviceInfo.n7SegmentLast; i++)
            {
                interfaceIT_7Segment_Display(device.Session, null, i);
            }
            interfaceIT_7Segment_Enable(device.Session, false);
        }

        if ((device.DeviceInfo.dwFeatures & Features.INTERFACEIT_FEATURE_OUTPUT_DATALINE) != 0)
        {
            for (int i = device.DeviceInfo.nDatalineFirst; i <= device.DeviceInfo.nDatalineLast; i++)
            {
                interfaceIT_Dataline_Set(device.Session, i, false);
            }
            interfaceIT_Dataline_Enable(device.Session, false);
        }

        if ((device.DeviceInfo.dwFeatures & Features.INTERFACEIT_FEATURE_OUTPUT_SERVO) != 0)
        {
            //Not available
        }

        if ((device.DeviceInfo.dwFeatures & Features.INTERFACEIT_FEATURE_SPECIAL_BRIGHTNESS) != 0)
        {
            interfaceIT_Brightness_Set(device.Session, 0);
            interfaceIT_Brightness_Enable(device.Session, false);
        }

        if ((device.DeviceInfo.dwFeatures & Features.INTERFACEIT_FEATURE_SPECIAL_ANALOG_INPUT) != 0)
        {
            interfaceIT_Analog_Enable(device.Session, false);
        }

        if ((device.DeviceInfo.dwFeatures & Features.INTERFACEIT_FEATURE_SPECIAL_ANALOG16_INPUT) != 0)
        {
            interfaceIT_Analog_Enable(device.Session, false);
        }
    }
}