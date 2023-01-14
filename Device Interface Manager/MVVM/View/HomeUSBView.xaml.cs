using Device_Interface_Manager.interfaceIT.USB;
using System.Windows;
using System.Windows.Controls;

namespace Device_Interface_Manager.MVVM.View
{
    public partial class HomeUSBView : UserControl
    {
        public HomeUSBView()
        {
            InitializeComponent();
        }

        private void DataGridCell_Serial_Drop(object sender, DragEventArgs e)
        {
            ((TextBlock)((DataGridCell)sender).Content).Text = ((InterfaceIT_BoardInfo.Device)e.Data.GetData(DataFormats.Serializable)).SerialNumber;
        }

        // MOVE THIS AWAY
        //Window Webpage for Profile3 Start
        //private void AddDisplaysComboBoxFBWA32NXCDU()
        //{
        //    var obj = new { DeviceName = "- Select Display -" };
        //    ComboBoxFBWA32NXCDU.Items.Add(obj);
        //    ComboBoxFBWA32NXCDU.DisplayMemberPath = "DeviceName";
        //    foreach (var allscreens in System.Windows.Forms.Screen.AllScreens)
        //    {
        //        if (allscreens.Primary == false)
        //        {
        //            ComboBoxFBWA32NXCDU.Items.Add(allscreens);
        //        }
        //    }
        //}

        //private void ComboBoxFBWA32NXCDU_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    CheckAllScreens();
        //    Properties.Settings.Default.MSFS_FBW_A32NX_MCDU_SelectedDisplay = ComboBoxFBWA32NXCDU.SelectedIndex;
        //    Properties.Settings.Default.Save();
        //}

        //private void CreateWebpage()
        //{
        //    if (FBW_A32NX_MCDU_Window == null)
        //    {
        //        WebView2 FBW_A32NX_MCDU_WebBrowser = new()
        //        {
        //            Source = new Uri("http://localhost:8380/interfaces/mcdu/?43")
        //        };
        //        FBW_A32NX_MCDU_Window = new Window
        //        {
        //            WindowStartupLocation = WindowStartupLocation.Manual,
        //            Topmost = true,
        //            ResizeMode = ResizeMode.NoResize,
        //            WindowStyle = WindowStyle.None,
        //            Content = FBW_A32NX_MCDU_WebBrowser
        //        };
        //        CheckAllScreens();
        //    }
        //}

        //private void CheckAllScreens()
        //{
        //    if (FBW_A32NX_MCDU_Window != null)
        //    {
        //        foreach (var allscreens in System.Windows.Forms.Screen.AllScreens)
        //        {
        //            if (ComboBoxFBWA32NXCDU.SelectedItem == allscreens)
        //            {
        //                FBW_A32NX_MCDU_Window.Top = allscreens.WorkingArea.Top;
        //                FBW_A32NX_MCDU_Window.Left = allscreens.WorkingArea.Left;
        //            }
        //        }
        //    }
        //}
        //Windows Webpage Profile3 End
    }
}