namespace Device_Interface_Manager.SimConnectMSFS;

public interface IWriteCacheInterface
{
    public void SetOffset(int offset, byte value);

    public void SetOffset(int offset, short value);

    public void SetOffset(int offset, int value, bool writeOnly = false);

    public void SetOffset(int offset, float value);

    public void SetOffset(int offset, double value);

    public void SetOffset(int offset, string value);

    public void ExecuteMacro(string macroName, int parameter);

    public void SetEventID(int eventID, int param);

    public void SetEventID(string eventID);

    public void Write();
}