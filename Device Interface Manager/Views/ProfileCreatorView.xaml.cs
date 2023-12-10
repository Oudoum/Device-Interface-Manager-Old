using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Device_Interface_Manager.Views;

public partial class ProfileCreatorView : MetroWindow
{ 
    public ProfileCreatorView()
    {
        InitializeComponent();
        Loaded += ProfileCreatorView_Loaded;
    }

    private void ProfileCreatorView_Loaded(object sender, RoutedEventArgs e)
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