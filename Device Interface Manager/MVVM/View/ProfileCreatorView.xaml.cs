using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Device_Interface_Manager.MVVM.View;

public partial class ProfileCreatorView : MetroWindow
{ 
    public ProfileCreatorView()
    {
        InitializeComponent();
        DataContext = new ViewModel.ProfileCreatorViewModel(DialogCoordinator.Instance);
    }
}