using System;
using System.Text;
using System.Runtime.InteropServices;
using MSFS = Microsoft.FlightSimulator.SimConnect;
using P3D = LockheedMartin.Prepar3D.SimConnect;

namespace Device_Interface_Manager.SimConnectProfiles.PMDG;

public static class PMDG
{
    public enum DATA_REQUEST_ID
    {
        AIR_PATH_REQUEST,
        CONTROL_REQUEST,
        DATA_REQUEST,
        CDU0_REQUEST,
        CDU1_REQUEST,
        CDU2_REQUEST
    }

    public static void RegisterPMDGNG3DataEvents(dynamic simConnect)
    {
        CreatePMDGEvents<PMDG_NG3_SDK.PMDGEvents>(simConnect);

        AssociatePMDGData<PMDG_NG3_SDK.PMDG_NG3_Data      >(simConnect, PMDG_NG3_SDK.DATA_NAME,  PMDG_NG3_SDK.PMDG_NG3.DATA_ID,  PMDG_NG3_SDK.PMDG_NG3.DATA_DEFINITION,  DATA_REQUEST_ID.DATA_REQUEST);
        AssociatePMDGData<PMDG_NG3_SDK.PMDG_NG3_CDU_Screen>(simConnect, PMDG_NG3_SDK.CDU_0_NAME, PMDG_NG3_SDK.PMDG_NG3.CDU_0_ID, PMDG_NG3_SDK.PMDG_NG3.CDU_0_DEFINITION, DATA_REQUEST_ID.CDU0_REQUEST);
        AssociatePMDGData<PMDG_NG3_SDK.PMDG_NG3_CDU_Screen>(simConnect, PMDG_NG3_SDK.CDU_1_NAME, PMDG_NG3_SDK.PMDG_NG3.CDU_1_ID, PMDG_NG3_SDK.PMDG_NG3.CDU_1_DEFINITION, DATA_REQUEST_ID.CDU1_REQUEST);
    }

    public static void RegisterPMDG747DataEvents(dynamic simConnect)
    {
        CreatePMDGEvents<PMDG_747QOTSII_SDK.PMDGEvents>(simConnect);

        AssociatePMDGData<PMDG_747QOTSII_SDK.PMDG_747QOTSII_Data      >(simConnect, PMDG_747QOTSII_SDK.DATA_NAME,  PMDG_747QOTSII_SDK.PMDG_747QOTSII.DATA_ID,  PMDG_747QOTSII_SDK.PMDG_747QOTSII.DATA_DEFINITION,  DATA_REQUEST_ID.DATA_REQUEST);
        AssociatePMDGData<PMDG_747QOTSII_SDK.PMDG_747QOTSII_CDU_Screen>(simConnect, PMDG_747QOTSII_SDK.CDU_0_NAME, PMDG_747QOTSII_SDK.PMDG_747QOTSII.CDU_0_ID, PMDG_747QOTSII_SDK.PMDG_747QOTSII.CDU_0_DEFINITION, DATA_REQUEST_ID.CDU0_REQUEST);
        AssociatePMDGData<PMDG_747QOTSII_SDK.PMDG_747QOTSII_CDU_Screen>(simConnect, PMDG_747QOTSII_SDK.CDU_1_NAME, PMDG_747QOTSII_SDK.PMDG_747QOTSII.CDU_1_ID, PMDG_747QOTSII_SDK.PMDG_747QOTSII.CDU_1_DEFINITION, DATA_REQUEST_ID.CDU1_REQUEST);
        AssociatePMDGData<PMDG_747QOTSII_SDK.PMDG_747QOTSII_CDU_Screen>(simConnect, PMDG_747QOTSII_SDK.CDU_2_NAME, PMDG_747QOTSII_SDK.PMDG_747QOTSII.CDU_2_ID, PMDG_747QOTSII_SDK.PMDG_747QOTSII.CDU_2_DEFINITION, DATA_REQUEST_ID.CDU2_REQUEST);
    }

    public static void RegisterPMDG777DataEvents(dynamic simConnect)
    {
        CreatePMDGEvents<PMDG_777X_SDK.PMDGEvents>(simConnect);

        AssociatePMDGData<PMDG_777X_SDK.PMDG_777X_Data      >(simConnect, PMDG_777X_SDK.DATA_NAME,  PMDG_777X_SDK.PMDG_777X.DATA_ID,  PMDG_777X_SDK.PMDG_777X.DATA_DEFINITION,  DATA_REQUEST_ID.DATA_REQUEST);
        AssociatePMDGData<PMDG_777X_SDK.PMDG_777X_CDU_Screen>(simConnect, PMDG_777X_SDK.CDU_0_NAME, PMDG_777X_SDK.PMDG_777X.CDU_0_ID, PMDG_777X_SDK.PMDG_777X.CDU_0_DEFINITION, DATA_REQUEST_ID.CDU0_REQUEST);
        AssociatePMDGData<PMDG_777X_SDK.PMDG_777X_CDU_Screen>(simConnect, PMDG_777X_SDK.CDU_1_NAME, PMDG_777X_SDK.PMDG_777X.CDU_1_ID, PMDG_777X_SDK.PMDG_777X.CDU_1_DEFINITION, DATA_REQUEST_ID.CDU1_REQUEST);
        AssociatePMDGData<PMDG_777X_SDK.PMDG_777X_CDU_Screen>(simConnect, PMDG_777X_SDK.CDU_2_NAME, PMDG_777X_SDK.PMDG_777X.CDU_2_ID, PMDG_777X_SDK.PMDG_777X.CDU_2_DEFINITION, DATA_REQUEST_ID.CDU2_REQUEST);
    }

    private static void CreatePMDGEvents<T>(dynamic simConnect) where T : Enum
    {
        // Map the PMDG Events to SimConnect
        foreach (T eventid in Enum.GetValues(typeof(T)))
        {
            lock (SimConnectClient.Instance.lockObject)
            {
                simConnect.MapClientEventToSimEvent(eventid, "#" + Convert.ChangeType(eventid, eventid.GetTypeCode()).ToString());
            }
        }
    }

    private static void AssociatePMDGData<T>(dynamic simConnect, string clientDataName, Enum clientDataID, Enum definitionID, Enum requestID) where T : struct
    {
        // Associate an ID with the PMDG data area name
        simConnect.MapClientDataNameToID(clientDataName, clientDataID);
        // Define the data area structure - this is a required step
        simConnect.AddToClientDataDefinition(definitionID, 0, (uint)Marshal.SizeOf<T>(), 0, 0);
        // Register the data area structure
        if (simConnect is MSFS.SimConnect simConnectMSFS)
        {
            simConnectMSFS.RegisterStruct<MSFS.SIMCONNECT_RECV_CLIENT_DATA, T>(definitionID);
            // Sign up for notification of data change
            simConnectMSFS.RequestClientData(
                clientDataID,
                requestID,
                definitionID,
                MSFS.SIMCONNECT_CLIENT_DATA_PERIOD.ON_SET,
                MSFS.SIMCONNECT_CLIENT_DATA_REQUEST_FLAG.CHANGED,
                0,
                0,
                0);
        }
        else if (simConnect is P3D.SimConnect simConnectP3D)
        {
            simConnectP3D.RegisterStruct<P3D.SIMCONNECT_RECV_CLIENT_DATA, T>(definitionID);
            // Sign up for notification of data change
            simConnectP3D.RequestClientData(
                clientDataID,
                requestID,
                definitionID,
                P3D.SIMCONNECT_CLIENT_DATA_PERIOD.ON_SET,
                P3D.SIMCONNECT_CLIENT_DATA_REQUEST_FLAG.CHANGED,
                0,
                0,
                0);
        }
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
}