using System;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.FlightSimulator.SimConnect;
using static Device_Interface_Manager.MSFSProfiles.PMDG.PMDG_NG3_SDK;

namespace Device_Interface_Manager.MSFSProfiles.PMDG.B737;

public class PMDG737
{
    public static void RegisterPMDGDataEvents(SimConnect simConnect)
    {
        CreatePMDG737NG3Events(simConnect);
        AssociatePMDG737NG3Data(simConnect);
        AssociatePMDG737NG3CDU0Data(simConnect);
        AssociatePMDG737NG3CDU1Data(simConnect);
    }

    private static void CreatePMDG737NG3Events(SimConnect simConnect)
    {
        // Map the PMDG Events to SimConnect
        foreach (PMDGEvents eventid in Enum.GetValues(typeof(PMDGEvents)))
        {
            simConnect.MapClientEventToSimEvent(eventid, "#" + Convert.ChangeType(eventid, eventid.GetTypeCode()).ToString());
        }
    }

    private static void AssociatePMDG737NG3Data(SimConnect simConnect)
    {
        // Associate an ID with the PMDG data area name
        simConnect.MapClientDataNameToID(PMDG_NG3_DATA_NAME, PMDG_NG3.DATA_ID);
        // Define the data area structure - this is a required step
        simConnect.AddToClientDataDefinition(PMDG_NG3.DATA_DEFINITION, 0, (uint)Marshal.SizeOf<PMDG_NG3_Data>(), 0, 0);
        // Register the data area structure
        simConnect.RegisterStruct<SIMCONNECT_RECV_CLIENT_DATA, PMDG_NG3_Data>(PMDG_NG3.DATA_DEFINITION);
        // Sign up for notification of data change
        simConnect.RequestClientData(
            PMDG_NG3.DATA_ID,
            DATA_REQUEST_ID.DATA_REQUEST,
            PMDG_NG3.DATA_DEFINITION,
            SIMCONNECT_CLIENT_DATA_PERIOD.ON_SET,
            SIMCONNECT_CLIENT_DATA_REQUEST_FLAG.CHANGED,
            0,
            0,
            0);
    }

    private static void AssociatePMDG737NG3CDU0Data(SimConnect simConnect)
    {
        // Associate an ID with the PMDG data CDU0 area name
        simConnect.MapClientDataNameToID(PMDG_NG3_CDU_0_NAME, PMDG_NG3.CDU_0_ID);
        // Define the data area CDU0 structure - this is a required step
        simConnect.AddToClientDataDefinition(PMDG_NG3.CDU_0_DEFINITION, 0, (uint)Marshal.SizeOf<PMDG_NG3_CDU_Screen>(), 0, 0);
        // Register the data area CDU0 structure
        simConnect.RegisterStruct<SIMCONNECT_RECV_CLIENT_DATA, PMDG_NG3_CDU_Screen>(PMDG_NG3.CDU_0_DEFINITION);
        // Sign up for notification of CDU0 data change
        simConnect.RequestClientData(
            PMDG_NG3.CDU_0_ID,
            DATA_REQUEST_ID.CDU0_REQUEST,
            PMDG_NG3.CDU_0_DEFINITION,
            SIMCONNECT_CLIENT_DATA_PERIOD.ON_SET,
            SIMCONNECT_CLIENT_DATA_REQUEST_FLAG.CHANGED,
            0,
            0,
            0);
    }

    private static void AssociatePMDG737NG3CDU1Data(SimConnect simConnect)
    {
        // Associate an ID with the PMDG data CDU1 area name
        simConnect.MapClientDataNameToID(PMDG_NG3_CDU_1_NAME, PMDG_NG3.CDU_1_ID);
        // Define the data area CDU1 structure - this is a required step
        simConnect.AddToClientDataDefinition(PMDG_NG3.CDU_1_DEFINITION, 0, (uint)Marshal.SizeOf<PMDG_NG3_CDU_Screen>(), 0, 0);
        // Register the data area CDU1 structure
        simConnect.RegisterStruct<SIMCONNECT_RECV_CLIENT_DATA, PMDG_NG3_CDU_Screen>(PMDG_NG3.CDU_1_DEFINITION);
        // Sign up for notification of CDU1 data change
        simConnect.RequestClientData(
            PMDG_NG3.CDU_1_ID,
            DATA_REQUEST_ID.CDU1_REQUEST,
            PMDG_NG3.CDU_1_DEFINITION,
            SIMCONNECT_CLIENT_DATA_PERIOD.ON_SET,
            SIMCONNECT_CLIENT_DATA_REQUEST_FLAG.CHANGED,
            0,
            0,
            0);
    }

    public static void SetPMDG737MCP(ref StringBuilder value, string name)
    {
        switch (name)
        {
            case "MCP_Altitude" when value[0] != '0':
                value.Insert(0, " ", 5 - value.Length);
                return;

            case "MCP_Altitude" when value[0] == '0':
                value.Insert(0, " 000");
                return;

            case "MCP_VertSpeed" when value[0] == '0' || value.ToString() == "-16960":
                value.Clear();
                value.Append(' ', 5);
                return;

            case "FMC_LandingAltitude" when value.ToString() == "-32767":
                value.Clear();
                value.Append(' ', 5);
                return;

            case "ADF_StandbyFrequency":
                value.Insert(value.Length - 1, ".");
                value.Insert(0, " ", 6 - value.Length);
                return;
        }

        if (name == "MCP_VertSpeed" && int.TryParse(value.ToString(), out int intValue))
        {
            if (intValue < 0)
            {
                value.Clear();
                value.Append('-');
                value.AppendFormat("{0,4}", intValue.ToString("D3").TrimStart('-'));
            }
            else if (intValue > 0)
            {
                value.Clear();
                value.Append('+');
                value.AppendFormat("{0,4}", intValue.ToString("D3"));
            }
        }
    }

    public static void SetPMDG737IRSDisplay(ref StringBuilder value, string name)
    {
        switch (name)
        {
            case "IRS_DisplayLeft" when value.Length > 0 && value[0] == 'w':
                value[0] = '8';
                break;

            case "IRS_DisplayLeft":
                value.Append(' ', 6 - value.Length);
                break;

            case "IRS_DisplayRight" when value.Length > 0 && value[0] == 'n':
                value[0] = '8';
                break;

            case "IRS_DisplayRight":
                value.Append(' ', 7 - value.Length);
                break;
        }
    }

    //private static void PMDGEventsNotUsed(SimConnect simConnect)
    //{
    //    //Methode 1
    //    // Associate an ID with the PMDG control area name
    //    simConnect.MapClientDataNameToID(PMDG_NG3_CONTROL_NAME, PMDG_NG3.CONTROL_ID);
    //    // Define the control area - this is a required step
    //    simConnect.AddToClientDataDefinition(PMDG_NG3.CONTROL_DEFINITION, 0, (uint)Marshal.SizeOf<PMDG_NG3_Control>(), 0, 0);
    //    // Sign up for notification of control change
    //    simConnect.RequestClientData(
    //        PMDG_NG3.CONTROL_ID,
    //        DATA_REQUEST_ID.CONTROL_REQUEST,
    //        PMDG_NG3.CONTROL_DEFINITION,
    //        SIMCONNECT_CLIENT_DATA_PERIOD.ON_SET,
    //        (SIMCONNECT_CLIENT_DATA_REQUEST_FLAG)SIMCONNECT_DATA_REQUEST_FLAG.CHANGED,
    //        0,
    //        0,
    //        0);

    //    // Create new PMDG_NG3_Control object
    //    PMDG_NG3_SDK.PMDG_NG3_Control pmdg_NG3_Control = new PMDG_NG3_Control
    //    {
    //        Event = (uint)PMDGEvents.EVT_OH_ELEC_BATTERY_SWITCH,
    //        Parameter = 1
    //    };

    //    // Set PMDG Event to Sim
    //    simConnect.SetClientData(PMDG_NG3.CONTROL_ID, PMDG_NG3.CONTROL_DEFINITION, 0, (uint)Marshal.SizeOf<PMDG_NG3_Control>(), pmdg_NG3_Control);
    //    simConnect.ReceiveMessage();
    //}
}