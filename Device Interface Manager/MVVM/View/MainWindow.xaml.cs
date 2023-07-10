﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Forms = System.Windows.Forms;

namespace Device_Interface_Manager.MVVM.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Environment.ProcessPath);
            notifyIcon.Text = "Device Interface Manager";
            notifyIcon.ContextMenuStrip = new Forms.ContextMenuStrip();
            notifyIcon.ContextMenuStrip.Items.Add("Exit", null, OnStatusClicked);
            notifyIcon.Click += NotifyIcon_MouseClick;

            if (Properties.Settings.Default.AutoHide)
            {
                Hide();
                notifyIcon.Visible = true;
                ShowInTaskbar = false;
            }
        }

        //Move Window
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        //Minimize Window
        readonly Forms.NotifyIcon notifyIcon = new();
        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            if (!Properties.Settings.Default.MinimizedHide)
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
            if (Properties.Settings.Default.MinimizedHide)
            {
                Hide();
                notifyIcon.Visible = true;
                ShowInTaskbar = false;
            }
        }

        //Contextmenu NotifyIcon Application Shutdown
        private void OnStatusClicked(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        //Show Window if NotifyIcon is clicked
        private void NotifyIcon_MouseClick(object sender, EventArgs e)
        {
            if (((Forms.MouseEventArgs)e).Button == Forms.MouseButtons.Left)
            {
                Show();
                ShowInTaskbar = true;
                notifyIcon.Visible = false;
            }
        }

        //Maximize Window
        private void ButtonMaximized_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            else
                Application.Current.MainWindow.WindowState = WindowState.Normal;
        }

        //Close Application
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            notifyIcon.Dispose();
            MainWindowPosition();
        }

        // Save MainWindow location
        private void MainWindowPosition()
        {
            if (WindowState == WindowState.Maximized)
            {
                Properties.Settings.Default.MainWindowTop = RestoreBounds.Top;
                Properties.Settings.Default.MainWindowLeft = RestoreBounds.Left;
                Properties.Settings.Default.MainWindowHeight = RestoreBounds.Height;
                Properties.Settings.Default.MainWindowWidth = RestoreBounds.Width;
                Properties.Settings.Default.MainWindowMaximized = true;
            }
            else
            {
                Properties.Settings.Default.MainWindowTop = Top;
                Properties.Settings.Default.MainWindowLeft = Left;
                Properties.Settings.Default.MainWindowHeight = Height;
                Properties.Settings.Default.MainWindowWidth = Width;
                Properties.Settings.Default.MainWindowMaximized = false;
            }
            Properties.Settings.Default.Save();
        }

        // Get MainWindowLocation
        private void Window_Initialized(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.MainWindowTop != 0
             && Properties.Settings.Default.MainWindowLeft != 0
             && Properties.Settings.Default.MainWindowHeight != 0
             && Properties.Settings.Default.MainWindowWidth != 0)
            {
                Top = Properties.Settings.Default.MainWindowTop;
                Left = Properties.Settings.Default.MainWindowLeft;
                Height = Properties.Settings.Default.MainWindowHeight;
                Width = Properties.Settings.Default.MainWindowWidth;
            }
            else WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (Properties.Settings.Default.MainWindowMaximized)
            {
                WindowState = WindowState.Maximized;
            }
        }

        //Item Drag&Drop
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