using MahApps.Metro.Controls;

namespace Device_Interface_Manager.MVVM.View;

public partial class OutputCreatorView : MetroWindow
{
    public OutputCreatorView()
    {
        InitializeComponent();
        Loaded += InputCreatorView_Loaded;
    }

    private void InputCreatorView_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        if (DataContext is Core.ICloseWindows vm)
        {
            vm.Close += Close;
        }
    }
}