using System;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Forms = System.Windows.Forms;

namespace Device_Interface_Manager.MVVM.View
{
    public partial class MainWindow : MetroWindow
    {
        readonly Forms.NotifyIcon notifyIcon;

        public MainWindow()
        {
            InitializeComponent();

            notifyIcon = new()
            {
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(Environment.ProcessPath),
                Text = "Device Interface Manager",
                ContextMenuStrip = new Forms.ContextMenuStrip
                {
                    Items =
                    {
                        new Forms.ToolStripMenuItem("Exit", null, OnStatusClicked)
                    }
                }
            };
            notifyIcon.Click += NotifyIcon_MouseClick;

            if (Properties.Settings.Default.AutoHide)
            {
                ChangeToNotify();
            }
        }

        private void OnStatusClicked(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void NotifyIcon_MouseClick(object sender, EventArgs e)
        {
            if (((Forms.MouseEventArgs)e).Button == Forms.MouseButtons.Left)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
                ShowInTaskbar = true;
                notifyIcon.Visible = false;
                Show();
            }
        }

        private void ChangeToNotify()
        {
            Hide();
            notifyIcon.Visible = true;
            ShowInTaskbar = false;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.MinimizedHide)
            {
                ChangeToNotify();
            }
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void ButtonMaximized_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
                return;
            }
            Application.Current.MainWindow.WindowState = WindowState.Normal;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            notifyIcon.Dispose();
            Application.Current.Shutdown();
        }

        private void ListViewItem_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && sender is FrameworkElement freamworkElement)
            {
                if (e.Source != null)
                {
                    DragDrop.DoDragDrop(freamworkElement, new DataObject(DataFormats.Serializable, freamworkElement.DataContext), DragDropEffects.Move);
                }
            }
        }
    }
}