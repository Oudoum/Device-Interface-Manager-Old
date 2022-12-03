using System.Runtime.InteropServices;
using System.Text;
using static Device_Interface_Manager.InterfaceIT_BoardInfo.BoardInformationStructure;

namespace Device_Interface_Manager
{
    public class InterfaceITAPI_Data
    {

        //Switch Callback Definition
        public delegate bool INTERFACEIT_KEY_NOTIFY_PROC(int hSession, int nSwitch, int nDirection);

        //Device Change Notification
        public delegate bool INTERFACEIT_DEVICE_CHANGE_NOTIFY_PROC(int nAction);

        public class Data
        {
            //Device Notification
            public const int INTERFACEIT_DEVICE_REMOVAL = 0x01;
            public const int INTERFACEIT_DEVICE_ARRIVAL = 0x02;
            //Switch Direction Information
            public const int INTERFACEIT_SWITCH_DIR_UNKNOWN = 0xFF;
            public const int INTERFACEIT_SWITCH_DIR_UP = 0x0;
            public const int INTERFACEIT_SWITCH_DIR_DOWN = 0x1;
            //Board Options
            public const int INTERFACEIT_BOARD_OPTION_NONE = 0x0;
            public const int INTERFACEIT_BOARD_OPTION_CDUKEYS = 0x1;     // CDU v9 does not need this.  Used for v7 CDU
            public const int INTERFACEIT_BOARD_OPTION_FORCE64 = 0x2;     // Required for the new FDS-CONTROLLER-MCP boards for relay to function
            public const int INTERFACEIT_BOARD_OPTION_RESERVED3 = 0x4;
        }

        public class ErrorCodes
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
        [DllImport("interfaceITAPI x64.dll")]
        public static extern int interfaceIT_OpenControllers();  //OK

        [DllImport("interfaceITAPI x64.dll")]
        public static extern int interfaceIT_GetDeviceList([Out] byte[] pBuffer, ref int dwSize, string pBoardType);  //OK

        [DllImport("interfaceITAPI x64.dll")]
        public static extern int interfaceIT_CloseControllers();  //OK


        //Controller Functions
        [DllImport("interfaceITAPI x64.dll")]
        public static extern int interfaceIT_Bind(string pController, ref int phSession); //OK

        [DllImport("interfaceITAPI x64.dll")]
        public static extern int interfaceIT_UnBind(int hSession);  //OK

        [DllImport("interfaceITAPI x64.dll")]
        public static extern int interfaceIT_GetBoardInfo(int hSession, ref BOARDCAPS pbc);  //OK

        [DllImport("interfaceITAPI x64.dll")]
        public static extern int interfaceIT_SetBoardOptions(int hSession, int dwOptions);

        [DllImport("interfaceITAPI x64.dll")]
        public static extern int interfaceIT_GetTotalControllers(ref int pnControllerCount);  //OK


        //LED Functions
        [DllImport("interfaceITAPI x64.dll")]
        public static extern int interfaceIT_LED_Enable(int hSession, bool bEnable);  //OK

        [DllImport("interfaceITAPI x64.dll")]
        public static extern int interfaceIT_LED_Set(int hSession, int nLED, int bOn);  //OK

        [DllImport("interfaceITAPI x64.dll")]
        public static extern int interfaceIT_LED_Test(int hSession, bool bEnable);  //OK


        //Switch Functions
        [DllImport("interfaceITAPI x64.dll")]
        public static extern int interfaceIT_Switch_Enable_Callback(int hSession, bool bEnable, INTERFACEIT_KEY_NOTIFY_PROC pProc);  //OK

        [DllImport("interfaceITAPI x64.dll")]
        public static extern int interfaceIT_Switch_Enable_Poll(int hSession, bool bEnable); //OK

        [DllImport("interfaceITAPI x64.dll")]
        public static extern int interfaceIT_Switch_Get_Item(int hSession, out int pnSwitch, out int pnDirection); //OK

        [DllImport("interfaceITAPI x64.dll")]
        public static extern int interfaceIT_Switch_Get_State(int hSession, int nSwitch, int pnState);


        //7 Segment Functions
        [DllImport("interfaceITAPI x64.dll")]
        public static extern int interfaceIT_7Segment_Display(int hSession, string pszData, int nStart); //OK

        [DllImport("interfaceITAPI x64.dll")]
        public static extern int interfaceIT_7Segment_Enable(int hSession, bool bEnable);  //OK


        //Dataline Functions
        [DllImport("interfaceITAPI x64.dll")]
        public static extern int interfaceIT_Dataline_Enable(int hSession, bool bEnable);   //OK

        [DllImport("interfaceITAPI x64.dll")]
        public static extern int interfaceIT_Dataline_Set(int hSession, int nDataline, bool bOn);   //OK


        //Brightness Functions
        [DllImport("interfaceITAPI x64.dll")]
        public static extern int interfaceIT_Brightness_Enable(int hSession, bool bEnable);  //OK

        [DllImport("interfaceITAPI x64.dll")]
        public static extern int interfaceIT_Brightness_Set(int hSession, int nBrightness);  //OK


        //Analog Input Functions
        [DllImport("interfaceITAPI x64.dll")]
        public static extern int interfaceIT_Analog_Enable(int hSession, bool bEnable);  //OK

        [DllImport("interfaceITAPI x64.dll")]
        public static extern int interfaceIT_Analog_GetValue(int hSession, int nReserved, out int pnPos);  //OK

        [DllImport("interfaceITAPI x64.dll", CharSet = CharSet.Unicode)]
        public static extern int interfaceIT_Analog_GetValues(int hSession, StringBuilder pbValues, ref int nValuesSize);


        //Device Change Notification
        [DllImport("interfaceITAPI x64.dll")]
        public static extern int interfaceIT_Enable_DeviceChange_Notification_Callback(bool bEnable, INTERFACEIT_DEVICE_CHANGE_NOTIFY_PROC nAction);  //NOT WORKING!!!


        //Misc Functions
        [DllImport("interfaceITAPI x64.dll")]
        public static extern int interfaceIT_GetAPIVersion(StringBuilder pBuffer, ref int dwSize);  //OK

        [DllImport("interfaceITAPI x64.dll")]
        public static extern int interfaceIT_EnableLogging(bool bEnable);  //OK
    }
}