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
            try
            {
                if (e.Data.GetDataPresent(DataFormats.Text))
                {
                    ((TextBlock)sender).Text = (string)e.Data.GetData(DataFormats.Text);
                }
                else if (e.Data.GetDataPresent(DataFormats.UnicodeText))
                {
                    ((TextBlock)sender).Text = (string)e.Data.GetData(DataFormats.UnicodeText);
                }
                else if (e.Data.GetDataPresent(DataFormats.Serializable))
                {
                    ((TextBlock)sender).Text = ((interfaceIT.USB.InterfaceIT_BoardInfo.Device)e.Data.GetData(DataFormats.Serializable)).SerialNumber;
                }
                else
                {
                    ((TextBlock)sender).Text = string.Empty;
                }
            }
            catch (System.Exception)
            {
                ((TextBlock)sender).Text = "Error";
            }
        }
    }
}