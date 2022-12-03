using System;

namespace MobiFlight.SimConnectMSFS
{
    public interface ISimConnectCacheInterface : Base.ICacheInterface, Base.IWriteCacheInterface
    {
        void SetSimVar(String SimVarCode);
    }
}