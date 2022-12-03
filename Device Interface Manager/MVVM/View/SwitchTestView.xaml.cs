using System.Windows.Controls;
using System.Windows.Media;
using static Device_Interface_Manager.MVVM.ViewModel.MainViewModel;

namespace Device_Interface_Manager.MVVM.View
{
    public partial class SwitchTestView : UserControl
    {
        public SwitchTestView()
        {
            InitializeComponent();
            SwitchTestViewModels[GetSeletedController()].SwitchLog.CollectionChanged += SwitchLog_CollectionChanged;
        }

        private void SwitchLog_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                ((ScrollViewer)VisualTreeHelper.GetChild((Border)VisualTreeHelper.GetChild(SwitchLog, 0), 0)).ScrollToBottom();
            }
        }
    }
}