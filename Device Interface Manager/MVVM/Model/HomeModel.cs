using Device_Interface_Manager.interfaceIT.USB.Controller;
using MobiFlight.SimConnectMSFS;
using System.Threading;

namespace Device_Interface_Manager.MVVM.Model
{
    public class HomeModel
    {
        public static Controller_331A_CDU_MCDU Profile0_MSFS_PMDG_737_CDU { get; set; }

        public static Controller_331A_CDU_MCDU Profile1_MSFS_PMDG_737_CDU { get; set; }

        public static Controller_331A_CDU_MCDU Profile3_MSFS_FBW_A32NX_MCDU { get; set; }

        public static Controller_331A_CDU_MCDU Profile4_MSFS_FBW_A32NX_MCDU { get; set; }

        public static Controller_331A_CDU_MCDU Profile5_MSFS_FENIX_A320_MCDU { get; set; }



        public static System.Windows.Window FBW_A32NX_MCDU_Window { get; set; }

        public static SimConnectCache simConnectCache;

        public static void SimConnectStart()
        {
            if (simConnectCache == null)
            {
                simConnectCache = new SimConnectCache();
                simConnectCache.Start();
                simConnectCache.Connect();
                Thread.Sleep(100);
                simConnectCache.ReceiveSimConnectMessage();
            }
        }

        public static void SimConnectStop()
        {
            simConnectCache?.Disconnect();
            simConnectCache?.Stop();
        }
    }
}