namespace Device_Interface_Manager.SimConnectMSFS;

public interface ISimConnectCacheInterface : ICacheInterface, IWriteCacheInterface
{
    public void SetSimVar(string SimVarCode);
}