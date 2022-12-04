using Device_Interface_Manager.interfaceIT.USB;
using Device_Interface_Manager.MVVM.ViewModel;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Device_Interface_Manager.MVVM.Model.HomeModel;

namespace Device_Interface_Manager.MVVM.View
{
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();

            AddDisplaysComboBoxFBWA32NXCDU();
        }

        //
        //Controller Profile Drag&Drop
        //
        //Profile0
        private void LabelSetControllerMSFSPMDG737CaptainCDU_Drop(object sender, DragEventArgs e)
        {
            MainViewModel.HomeVM.MSFS_PMDG_B737_Captain_CDU_SN = ((InterfaceIT_BoardInfo.Device)e.Data.GetData(DataFormats.Serializable)).SerialNumber;
        }

        //Profile1
        private void LabelSetControllerMSFSPMDG737FirstOfficerCDU_Drop(object sender, DragEventArgs e)
        {

            MainViewModel.HomeVM.MSFS_PMDG_B737_Firstofficer_CDU_SN = ((InterfaceIT_BoardInfo.Device)e.Data.GetData(DataFormats.Serializable)).SerialNumber;
        }

        //Profile2
        private void LabelSetControllerMSFSPMDG737MCP_Drop(object sender, DragEventArgs e)
        {
            MainViewModel.HomeVM.MSFS_PMDG_B737_MCP_SN = ((InterfaceIT_BoardInfo.Device)e.Data.GetData(DataFormats.Serializable)).SerialNumber;
        }

        //Profile3
        private void Label_SetController_MSFS_FBW_A32NX_Captain_MCDU_Drop(object sender, DragEventArgs e)
        {
            MainViewModel.HomeVM.MSFS_FBW_A32NX_Captain_MCDU_SN = ((InterfaceIT_BoardInfo.Device)e.Data.GetData(DataFormats.Serializable)).SerialNumber;
        }

        //Profile4
        private void Label_SetController_MSFS_FBW_A32NX_Firstofficer_MCDU_Drop(object sender, DragEventArgs e)
        {
            MainViewModel.HomeVM.MSFS_FBW_A32NX_Firstofficer_MCDU_SN = ((InterfaceIT_BoardInfo.Device)e.Data.GetData(DataFormats.Serializable)).SerialNumber;
        }

        //Profile5
        private void Label_SetController_MSFS_FENIX_A320_Captain_Drop(object sender, DragEventArgs e)
        {
            MainViewModel.HomeVM.MSFS_FENIX_A320_Captain_MCDU_SN = ((InterfaceIT_BoardInfo.Device)e.Data.GetData(DataFormats.Serializable)).SerialNumber;
        }


        //
        //Controller Profile Remove
        //
        //Profile0
        private void LabelSetControllerMSFSPMDG737CaptainCDU_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MainViewModel.HomeVM.MSFS_PMDG_B737_Captain_CDU_SN = null;
        }

        //Profile1
        private void LabelSetControllerMSFSPMDG737FirstOfficerCDU_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MainViewModel.HomeVM.MSFS_PMDG_B737_Firstofficer_CDU_SN = null;
        }

        //Profile2
        private void LabelSetControllerMSFSPMDG737MCP_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MainViewModel.HomeVM.MSFS_PMDG_B737_MCP_SN = null;
        }

        //Profile3
        private void Label_SetController_MSFS_FBW_A32NX_Captain_MCDU_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MainViewModel.HomeVM.MSFS_FBW_A32NX_Captain_MCDU_SN = null;
        }

        //Profile4
        private void Label_SetController_MSFS_FBW_A32NX_Firstofficer_MCDU_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MainViewModel.HomeVM.MSFS_FBW_A32NX_Firstofficer_MCDU_SN = null;
        }

        //Profile5
        private void Label_SetController_MSFS_FENIX_A320_Captain_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MainViewModel.HomeVM.MSFS_FENIX_A320_Captain_MCDU_SN = null;
        }






















        // MOVE THIS AWAY
        //Window Webpage for Profile3 Start
        private void AddDisplaysComboBoxFBWA32NXCDU()
        {
            var obj = new { DeviceName = "- Select Display -" };
            ComboBoxFBWA32NXCDU.Items.Add(obj);
            ComboBoxFBWA32NXCDU.DisplayMemberPath = "DeviceName";
            foreach (var allscreens in System.Windows.Forms.Screen.AllScreens)
            {
                if (allscreens.Primary == false)
                {
                    ComboBoxFBWA32NXCDU.Items.Add(allscreens);
                }
            }
        }

        private void ComboBoxFBWA32NXCDU_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckAllScreens();
            Properties.Settings.Default.MSFS_FBW_A32NX_MCDU_SelectedDisplay = ComboBoxFBWA32NXCDU.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void CreateWebpage()
        {
            if (FBW_A32NX_MCDU_Window == null)
            {
                WebView2 FBW_A32NX_MCDU_WebBrowser = new()
                {
                    Source = new Uri("http://localhost:8380/interfaces/mcdu/?43")
                };
                FBW_A32NX_MCDU_Window = new Window
                {
                    WindowStartupLocation = WindowStartupLocation.Manual,
                    Topmost = true,
                    ResizeMode = ResizeMode.NoResize,
                    WindowStyle = WindowStyle.None,
                    Content = FBW_A32NX_MCDU_WebBrowser
                };
                CheckAllScreens();
            }
        }

        private void CheckAllScreens()
        {
            if (FBW_A32NX_MCDU_Window != null)
            {
                foreach (var allscreens in System.Windows.Forms.Screen.AllScreens)
                {
                    if (ComboBoxFBWA32NXCDU.SelectedItem == allscreens)
                    {
                        FBW_A32NX_MCDU_Window.Top = allscreens.WorkingArea.Top;
                        FBW_A32NX_MCDU_Window.Left = allscreens.WorkingArea.Left;
                    }
                }
            }
        }
        //Windows Webpage Profile3 End
    }
}