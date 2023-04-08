using System.Windows.Controls;
using System.Windows.Media;
using Device_Interface_Manager.MVVM.Model;

namespace Device_Interface_Manager.MVVM.View
{
    public partial class SwitchTestView : UserControl
    {
        public SwitchTestView()
        {
            InitializeComponent();
            Loaded += SwitchTestView_Loaded;
        }

        private void SwitchTestView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is ISwitchLogChanged slc)
            {
                slc.SwitchLogChanged += () => ((ScrollViewer)VisualTreeHelper.GetChild((Border)VisualTreeHelper.GetChild(SwitchLog, 0), 0)).ScrollToBottom();
            }
        }
    }
}