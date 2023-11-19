using System;
using System.Text;
using System.Runtime.InteropServices;
using static Device_Interface_Manager.Devices.interfaceIT.USB.InterfaceIT_BoardInfo;

namespace Device_Interface_Manager.Devices.interfaceIT.USB;

public partial class InterfaceITAPI_Data
{
    public delegate void KeyNotifyCallback(uint session, int key, int direction);
    public delegate void KeyNotifyCallbackUint(uint session, int key, uint direction);

    public delegate void DeviceChangeCallback(int action);

    public enum DeviceNotification : byte
    {
        Removal = 0x01,
        Arrival = 0x02,
    }

    public enum SwitchDirection : byte
    {
        Unknown = 0xFF,
        Up = 0x0,
        Down = 0x1,
    }

    [Flags]
    public enum BoardOptions : byte
    {
       None = 0x0,
       CDUKeys = 0x1,     // CDU v9 does not need this.  Used for v7 CDU
       Force64 = 0x2,     // Required for the new FDS-CONTROLLER-MCP boards for relay to function
       Reserved3 = 0x4,
    }

    public enum ErrorCodes : short
    {
        OK = 0,
        ControllersOpenFailed = -1,
        ControllersAlreadyOpened = -2,
        ControllersNotOpened = -3,
        InvalidHandle = -4,
        InvalidPointer = -5,
        InvalidControllerName = -6,
        Failed = -7,
        InvalidControllerPointer = -8,
        InvalidCallback = -9,
        RetrievingController = -10,
        NotEnabled = -11,
        BufferNotLargeEnough = -12,
        ParameterLengthIncorrect = -13,
        ParameterOutOfRange = -14,
        FeatureNotAvailable = -15,
        AlreadyEnabled = -16,
        NoItems = -17,
        ControllerAlreadyBound = -18,
        NoControllersFound = -19,
        UnknownError = -20,
        NotLicensed = -21,
        InvalidLicense = -22,
        AlreadyLicensed = -23,
        GeneratingActivationIDFailed = -24,
        ExpiredLicense = -25,
        ExpiredTrial = -26,
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
    internal static extern void interfaceIT_GetBoardInfo(uint session, out BoardInfo pbc);

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
    internal static partial void interfaceIT_Switch_Enable_Callback(uint session, [MarshalAs(UnmanagedType.Bool)] bool enable, KeyNotifyCallback pROC);

    [LibraryImport("interfaceITAPI x64.dll")]
    internal static partial void interfaceIT_Switch_Enable_Callback(uint session, [MarshalAs(UnmanagedType.Bool)] bool enable, KeyNotifyCallbackUint pROC);

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


    internal static void EnableDeviceFeatures(Device device)
    {
        Features features = device.BoardInfo.Features;
        uint session = device.Session;

        if (HasFeature(features, Features.SpecialAnalog16Input))
        {
            interfaceIT_Analog_Enable(session, true);
        }

        if (HasFeature(features, Features.SpecialAnalogInput))
        {
            interfaceIT_Analog_Enable(session, true);
        }

        if (HasFeature(features, Features.SpecialBrightness))
        {
            interfaceIT_Brightness_Enable(session, true);
        }

        if (HasFeature(features, Features.OutputDataLine))
        {
            interfaceIT_Dataline_Enable(session, true);
        }

        if (HasFeature(features, Features.Output7Segment))
        {
            interfaceIT_7Segment_Enable(session, true);
        }

        if (HasFeature(features, Features.OutputLED))
        {
            interfaceIT_LED_Enable(session, true);
        }
    }

    internal static void InterfaceITDisable(Device device)
    {
        Features features = device.BoardInfo.Features;
        uint session = device.Session;

        if (HasFeature(features, Features.OutputLED))
        {
            for (int i = device.BoardInfo.LEDFirst; i <= device.BoardInfo.LEDLast; i++)
            {
                interfaceIT_LED_Set(session, i, false);
            }
            interfaceIT_LED_Enable(session, false);
        }

        if (HasFeature(features, Features.InputSwitches))
        {
            interfaceIT_Switch_Enable_Poll(session, false);
        }

        if (HasFeature(features, Features.Output7Segment))
        {
            for (int i = device.BoardInfo.SevenSegmentFirst; i <= device.BoardInfo.SevenSegmentLast; i++)
            {
                interfaceIT_7Segment_Display(session, null, i);
            }
            interfaceIT_7Segment_Enable(session, false);
        }

        if (HasFeature(features, Features.OutputDataLine))
        {
            for (int i = device.BoardInfo.DatalineFirst; i <= device.BoardInfo.DatalineLast; i++)
            {
                interfaceIT_Dataline_Set(session, i, false);
            }
            interfaceIT_Dataline_Enable(session, false);
        }

        if (HasFeature(features, Features.SpecialBrightness))
        {
            interfaceIT_Brightness_Set(session, 0);
            interfaceIT_Brightness_Enable(session, false);
        }

        if (HasFeature(features, Features.SpecialAnalogInput) || HasFeature(features, Features.SpecialAnalog16Input))
        {
            interfaceIT_Analog_Enable(session, false);
        }
    }

    private static bool HasFeature(Features featuresToCheck, Features feature)
    {
        return (featuresToCheck & feature) != 0;
    }
}