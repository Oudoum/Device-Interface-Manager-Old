namespace Device_Interface_Manager.MSFSProfiles.PMDG.B737
{
    public class NG_CDU_MAX_L_E : NG_CDU_L_E
    {
        protected override void KeyPressedProcEthernet(int Switch, string Direction)
        {
            switch (Switch)
            {
                case 41:
                    Switch = 49;
                    break;

                case 42:
                    Switch = 41;
                    break;

                case 49:
                    Switch = 42;
                    break;

                case 52:
                    Switch = 53;
                    break;

                case 60:
                    Switch = 52;
                    break;

                default:
                    break;
            }
            base.KeyPressedProcEthernet(Switch, Direction);
        }
    }
}