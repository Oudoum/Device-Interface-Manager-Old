using System.Windows;
using System.Collections.ObjectModel;
using Device_Interface_Manager.interfaceIT.USB;
using Device_Interface_Manager.MVVM.ViewModel;

namespace Device_Interface_Manager.Core;
interface INavigationService
{
    void NavigateTo<TView>(object parameter) where TView : Window, new();
}

class NavigationService : INavigationService
{
    public void NavigateTo<TView>(object parameter) where TView : Window, new()
    {
        TView view = new();
        if (view.DataContext is TestProfileViewModel viewModel)
        {
            viewModel.DeviceList = parameter as ObservableCollection<InterfaceIT_BoardInfo.Device>;
        }
        view.Show();
    }
}