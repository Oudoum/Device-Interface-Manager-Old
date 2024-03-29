﻿using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Device_Interface_Manager.Views;

public partial class OutputCreatorView : MetroWindow
{
    public OutputCreatorView()
    {
        InitializeComponent();
        Loaded += InputCreatorView_Loaded;
    }

    private void InputCreatorView_Loaded(object sender, System.Windows.RoutedEventArgs e)
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