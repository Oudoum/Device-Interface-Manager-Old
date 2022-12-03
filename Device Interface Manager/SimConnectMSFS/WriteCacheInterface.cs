namespace MobiFlight.Base
{
    public interface IWriteCacheInterface
    {
        void SetOffset(int offset, byte value);

        void SetOffset(int offset, short value);

        void SetOffset(int offset, int value, bool writeOnly = false);

        void SetOffset(int offset, float value);

        void SetOffset(int offset, double value);

        void SetOffset(int offset, string value);

        void ExecuteMacro(string macroName, int parameter);

        void SetEventID(int eventID, int param);

        void SetEventID(string eventID);

        void Write();
    }
}