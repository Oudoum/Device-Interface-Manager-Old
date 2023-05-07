namespace Device_Interface_Manager.MSFSProfiles.PMDG.B737;

public class NG_CDU_MAX_L_E : NG_CDU_L_E
{
    protected override void KeyPressedAction(int key, uint direction)
    {
        switch (key)
        {
            case 41:
                key = 49;
                break;

            case 42:
                key = 41;
                break;

            case 49:
                key = 42;
                break;

            case 52:
                key = 53;
                break;

            case 60:
                key = 52;
                break;

            default:
                break;
        }
        base.KeyPressedAction(key, direction);
    }
}