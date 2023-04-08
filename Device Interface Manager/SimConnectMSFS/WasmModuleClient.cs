using Microsoft.FlightSimulator.SimConnect;

namespace Device_Interface_Manager.SimConnectMSFS;

public static class WasmModuleClient
{
    public static void Ping(SimConnect simConnect)
    {
        if (simConnect is null) return;

        SendWasmCmd(simConnect, "MF.Ping");
        DummyCommand(simConnect);
    }

    public static void Stop(SimConnect simConnect)
    {
        if (simConnect is null) return;

        SendWasmCmd(simConnect, "MF.SimVars.Clear");
    }

    public static void GetLVarList(SimConnect simConnect)
    {
        if (simConnect is null) return;

        SendWasmCmd(simConnect, "MF.LVars.List");
        DummyCommand(simConnect);
    }

    public static void DummyCommand(SimConnect simConnect)
    {
        if (simConnect is null) return;

        SendWasmCmd(simConnect, "MF.DummyCmd");
    }

    public static void SendWasmCmd(SimConnect simConnect, string command)
    {
        if (simConnect is null) return;

        simConnect.SetClientData(
            SIMCONNECT_CLIENT_DATA_ID.MOBIFLIGHT_CMD,
           (SIMCONNECT_CLIENT_DATA_ID)0,
           SIMCONNECT_CLIENT_DATA_SET_FLAG.DEFAULT, 0,
           new ClientDataString(command)
        );
    }

    public static void SetConfig(SimConnect simConnect, string ConfigName, string ConfigValue)
    {
        if (simConnect is null) return;

        SendWasmCmd(simConnect, $"MF.Config.{ConfigName}.Set.{ConfigValue}");
        DummyCommand(simConnect);
    }
}