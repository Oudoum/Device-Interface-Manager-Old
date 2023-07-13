using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Device_Interface_Manager.MVVM.View;

public partial class ProfileCreatorView : MetroWindow
{ 
    public ProfileCreatorView()
    {
        InitializeComponent();
        DataContext = new ViewModel.ProfileCreatorViewModel(DialogCoordinator.Instance);
        Loaded += ProfileCreatorView_Loaded;
    }

    private void ProfileCreatorView_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        if (DataContext is Core.ICloseWindowsCheck vm)
        {
            vm.Close += Close;
            Closing += (s, e) =>
            {
                if (vm.CanClose() == MessageDialogResult.Negative)
                {
                    e.Cancel = true;
                }
            };
        }
    }
}