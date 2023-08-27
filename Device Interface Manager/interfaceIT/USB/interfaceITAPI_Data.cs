using System;
using System.Runtime.InteropServices;
using System.Text;
using static Device_Interface_Manager.interfaceIT.USB.InterfaceIT_BoardInfo;

namespace Device_Interface_Manager.interfaceIT.USB;

public partial class InterfaceITAPI_Data
{
    public delegate void KeyNotificationCallback(uint session, int key, int direction);
    public delegate void KeyNotificationCallbackUint(uint session, int key, uint direction);

    public delegate void DeviceChangeCallback(int action);

    public enum DeviceNotification
    {
        INTERFACEIT_DEVICE_REMOVAL = 0x01,
        INTERFACEIT_DEVICE_ARRIVAL = 0x02,
    }

    public enum SwitchDirectionInfo
    {
        INTERFACEIT_SWITCH_DIR_UNKNOWN = 0xFF,
        INTERFACEIT_SWITCH_DIR_UP = 0x0,
        INTERFACEIT_SWITCH_DIR_DOWN = 0x1,
    }

    public enum BoardOptions
    {
       INTERFACEIT_BOARD_OPTION_NONE = 0x0,
       INTERFACEIT_BOARD_OPTION_CDUKEYS = 0x1,     // CDU v9 does not need this.  Used for v7 CDU
       INTERFACEIT_BOARD_OPTION_FORCE64 = 0x2,     // Required for the new FDS-CONTROLLER-MCP boards for relay to function
       INTERFACEIT_BOARD_OPTION_RESERVED3 = 0x4,
    }

    public enum ErrorCodes
    {
        IITAPI_ERR_OK = -0,
        IITAPI_ERR_CONTROLLERS_OPEN_FAILED = -1,
        IITAPI_ERR_CONTROLLERS_ALREADY_OPENED = -2,
        IITAPI_ERR_CONTROLLERS_NOT_OPENED = -3,
        IITAPI_ERR_INVALID_HANDLE = -4,
        IITAPI_ERR_INVALID_POINTER = -5,
        IITAPI_ERR_INVALID_CONTROLLER_NAME = -6,
        IITAPI_ERR_FAILED = -7,
        IITAPI_ERR_INVALID_CONTROLLER_POINTER = -8,
        IITAPI_ERR_INVALID_CALLBACK = -9,
        IITAPI_ERR_RETRIEVING_CONTROLLER = -10,
        IITAPI_ERR_NOT_ENABLED = -11,
        IITAPI_ERR_BUFFER_NOT_LARGE_ENOUGH = -12,
        IITAPI_ERR_PARAMETER_LENGTH_INCORRECT = -13,
        IITAPI_ERR_PARAMETER_OUT_OF_RANGE = -14,
        IITAPI_ERR_FEATURE_NOT_AVAILABLE = -15,
        IITAPI_ERR_ALREADY_ENABLED = -16,
        IITAPI_ERR_NO_ITEMS = -17,
        IITAPI_ERR_CONTROLLER_ALREADY_BOUND = -18,
        IITAPI_ERR_NO_CONTROLLERS_FOUND = -19,
        IITAPI_ERR_UNKNOWN = -20,
        IITAPI_ERR_NOT_LICENSED = -21,
        IITAPI_ERR_INVALID_LICENSE = -22,
        IITAPI_ERR_ALREADY_LICENSED = -23,
        IITAPI_ERR_GENERATING_ACTIVATIONID = -24,
        IITAPI_ERR_EXPIRED_LICENSE = -25,
        IITAPI_ERR_EXPIRED_TRIAL = -26,
    }

    //Main Functions
    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_OpenControllers();

    [LibraryImport("interfaceITAPI x64.dll", StringMarshalling = StringMarshalling.Utf8)]
    private static partial void interfaceIT_GetDeviceList(byte[] buffer, ref uint bufferSize, string boardType);

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
    internal static partial void interfaceIT_Bind(string controller, out uint session);

    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_UnBind(uint session);

    [DllImport("interfaceITAPI x64.dll")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time", Justification = "<Pending>")]
    internal static extern void interfaceIT_GetBoardInfo(uint session, out BOARDCAPS pbc);

    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_SetBoardOptions(uint session, uint options);

    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_GetTotalControllers(ref int controllerCount);


    //LED Functions
    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_LED_Enable(uint session, [MarshalAs(UnmanagedType.Bool)] bool enable);

    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_LED_Test(uint session, [MarshalAs(UnmanagedType.Bool)] bool enable);

    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_LED_Set(uint session, int lED, [MarshalAs(UnmanagedType.Bool)] bool on);

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    internal static void interfaceIT_LED_Set(uint session, int lED, double on)
    {
        interfaceIT_LED_Set(session, lED, Convert.ToBoolean(on));
    }


    //Switch Functions
    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_Switch_Enable_Callback(uint session, [MarshalAs(UnmanagedType.Bool)] bool enable, KeyNotificationCallback pROC);

    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_Switch_Enable_Callback(uint session, [MarshalAs(UnmanagedType.Bool)] bool enable, KeyNotificationCallbackUint pROC);

    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_Switch_Enable_Poll(uint session, [MarshalAs(UnmanagedType.Bool)] bool enable);

    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial int interfaceIT_Switch_Get_Item(uint session, out int key, out int direction);

    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_Switch_Get_State(uint session, out int key,out int state);  //Not tested


    //7 Segment Functions
    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_7Segment_Enable(uint session, [MarshalAs(UnmanagedType.Bool)] bool enable);

    [LibraryImport("interfaceITAPI x64.dll", StringMarshalling = StringMarshalling.Utf8)]
    internal static partial void interfaceIT_7Segment_Display(uint session, string data, int start);


    //Dataline Functions
    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_Dataline_Enable(uint session, [MarshalAs(UnmanagedType.Bool)] bool enable);

    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_Dataline_Set(uint session, int dataline, [MarshalAs(UnmanagedType.Bool)] bool on);


    //Brightness Functions
    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_Brightness_Enable(uint session, [MarshalAs(UnmanagedType.Bool)] bool enable);

    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_Brightness_Set(uint session, int brightness);


    //Analog Input Functions
    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_Analog_Enable(uint session, [MarshalAs(UnmanagedType.Bool)] bool enable);

    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_Analog_GetValue(uint session, int reserved, out int pos);

    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_Analog_GetValues(uint session, byte[] values, ref int valuesSize);  //Not tested


    //Device Change Notification
    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_Enable_DeviceChange_Notification_Callback([MarshalAs(UnmanagedType.Bool)] bool enable, DeviceChangeCallback pProc);  //NOT WORKING!!!


    //Misc Functions
    [LibraryImport("interfaceITAPI x64.dll")]
    private static partial void interfaceIT_GetAPIVersion(byte[] buffer, ref uint bufferSize);

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
    internal static partial void interfaceIT_EnableLogging([MarshalAs(UnmanagedType.Bool)] bool enable);


    internal static void InterfaceITEnable(Device device)
    {
        if ((device.DeviceInfo.Features & Features.INTERFACEIT_FEATURE_SPECIAL_ANALOG16_INPUT) != 0)
        {
            interfaceIT_Analog_Enable(device.Session, true);
        }

        if ((device.DeviceInfo.Features & Features.INTERFACEIT_FEATURE_SPECIAL_ANALOG_INPUT) != 0)
        {
            interfaceIT_Analog_Enable(device.Session, true);
        }

        if ((device.DeviceInfo.Features & Features.INTERFACEIT_FEATURE_SPECIAL_BRIGHTNESS) != 0)
        {
            interfaceIT_Brightness_Enable(device.Session, true);
        }

        if ((device.DeviceInfo.Features & Features.INTERFACEIT_FEATURE_OUTPUT_SERVO) != 0)
        {
            //Not available
        }

        if ((device.DeviceInfo.Features & Features.INTERFACEIT_FEATURE_OUTPUT_DATALINE) != 0)
        {
            interfaceIT_Dataline_Enable(device.Session, true);
        }

        if ((device.DeviceInfo.Features & Features.INTERFACEIT_FEATURE_OUTPUT_7SEGMENT) != 0)
        {
            interfaceIT_7Segment_Enable(device.Session, true);
        }

        if ((device.DeviceInfo.Features & Features.INTERFACEIT_FEATURE_OUTPUT_LED) != 0)
        {
            interfaceIT_LED_Enable(device.Session, true);
        }
    }

    internal static void InterfaceITDisable(Device device)
    {
        if ((device.DeviceInfo.Features & Features.INTERFACEIT_FEATURE_OUTPUT_LED) != 0)
        {
            for (int i = device.DeviceInfo.LEDFirst; i <= device.DeviceInfo.LEDLast; i++)
            {
                interfaceIT_LED_Set(device.Session, i, false);
            }
            interfaceIT_LED_Enable(device.Session, false);
        }

        if ((device.DeviceInfo.Features & Features.INTERFACEIT_FEATURE_INPUT_SWITCHES) != 0)
        {
            interfaceIT_Switch_Enable_Poll(device.Session, false);
        }

        if ((device.DeviceInfo.Features & Features.INTERFACEIT_FEATURE_OUTPUT_7SEGMENT) != 0)
        {
            for (int i = device.DeviceInfo.SevenSegmentFirst; i <= device.DeviceInfo.SevenSegmentLast; i++)
            {
                interfaceIT_7Segment_Display(device.Session, null, i);
            }
            interfaceIT_7Segment_Enable(device.Session, false);
        }

        if ((device.DeviceInfo.Features & Features.INTERFACEIT_FEATURE_OUTPUT_DATALINE) != 0)
        {
            for (int i = device.DeviceInfo.DatalineFirst; i <= device.DeviceInfo.DatalineLast; i++)
            {
                interfaceIT_Dataline_Set(device.Session, i, false);
            }
            interfaceIT_Dataline_Enable(device.Session, false);
        }

        if ((device.DeviceInfo.Features & Features.INTERFACEIT_FEATURE_OUTPUT_SERVO) != 0)
        {
            //Not available
        }

        if ((device.DeviceInfo.Features & Features.INTERFACEIT_FEATURE_SPECIAL_BRIGHTNESS) != 0)
        {
            interfaceIT_Brightness_Set(device.Session, 0);
            interfaceIT_Brightness_Enable(device.Session, false);
        }

        if ((device.DeviceInfo.Features & Features.INTERFACEIT_FEATURE_SPECIAL_ANALOG_INPUT) != 0)
        {
            interfaceIT_Analog_Enable(device.Session, false);
        }

        if ((device.DeviceInfo.Features & Features.INTERFACEIT_FEATURE_SPECIAL_ANALOG16_INPUT) != 0)
        {
            interfaceIT_Analog_Enable(device.Session, false);
        }
    }
}