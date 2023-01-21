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
    }
}