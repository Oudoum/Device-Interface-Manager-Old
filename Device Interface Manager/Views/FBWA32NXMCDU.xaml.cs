using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Device_Interface_Manager.Views
{
    public partial class FBWA32NXMCDU : Window
    {
        public FBWA32NXMCDU()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (WindowState != WindowState.Maximized)
                {
                    WindowState = WindowState.Maximized;
                    return;
                }
                WindowState = WindowState.Normal;
            }
        }

        bool editormode = false;
        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            editormode = !editormode;
            if (editormode)
            {
                Background = new SolidColorBrush(Colors.Blue);
                return;
            }
            EditormodeOff?.Invoke(this, e);
            Background = new SolidColorBrush(Colors.Transparent);
        }

        public event EventHandler EditormodeOff;

        public double marginTop;
        public double marginBottom;
        public double marginLeft;
        public double marginRight;
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!editormode && e.Key == Key.Escape)
            {
                Hide();
            }
            else if (editormode)
            {
                switch (e.Key)
                {
                    case Key.Space when Keyboard.IsKeyDown(Key.LeftShift):
                        MCDUGrid.ClearValue(HeightProperty);
                        MCDUGrid.ClearValue(WidthProperty);
                        break;

                    case Key.Enter when Keyboard.IsKeyDown(Key.LeftShift):
                        MCDUGrid.ClearValue(HeightProperty);
                        MCDUGrid.ClearValue(WidthProperty);
                        WV.Margin = new Thickness(0);
                        marginLeft = 0;
                        marginTop = 0;
                        marginRight = 0;
                        marginBottom = 0;
                        break;

                    case Key.PageUp when Keyboard.IsKeyDown(Key.LeftShift):
                        --marginTop;
                        SetMargin();
                        break;

                    case Key.PageUp:
                        ++marginBottom;
                        SetMargin();
                        break;

                    case Key.PageDown when Keyboard.IsKeyDown(Key.LeftShift):
                        ++marginTop;
                        SetMargin();

                        break;

                    case Key.PageDown:
                        --marginBottom;
                        SetMargin();
                        break;

                    case Key.Add when Keyboard.IsKeyDown(Key.LeftShift):
                        ++marginRight;
                        SetMargin();
                        break;

                    case Key.Add:
                        --marginLeft;
                        SetMargin();
                        break;

                    case Key.Subtract when Keyboard.IsKeyDown(Key.LeftShift):
                        --marginRight;
                        SetMargin();
                        break;

                    case Key.Subtract:
                        ++marginLeft;
                        SetMargin();
                        break;


                    case Key.W:
                        MCDUGrid.Height = MCDUGrid.ActualHeight;
                        MCDUGrid.Height = --MCDUGrid.Height;
                        break;

                    case Key.A:
                        MCDUGrid.Width = MCDUGrid.ActualWidth;
                        MCDUGrid.Width = ++MCDUGrid.Width;
                        break;

                    case Key.S:
                        MCDUGrid.Height = MCDUGrid.ActualHeight;
                        MCDUGrid.Height = ++MCDUGrid.Height;
                        break;

                    case Key.D:
                        MCDUGrid.Width = MCDUGrid.ActualWidth;
                        MCDUGrid.Width = --MCDUGrid.Width;
                        break;

                    case Key.Up:
                        --marginTop;
                        ++marginBottom;
                        SetMargin();
                        break;

                    case Key.Left:
                        --marginLeft;
                        ++marginRight;
                        SetMargin();
                        break;

                    case Key.Down:
                        ++marginTop;
                        --marginBottom;
                        SetMargin();
                        break;

                    case Key.Right:
                        ++marginLeft;
                        --marginRight;
                        SetMargin();
                        break;

                    default:
                        break;
                }
            }
        }

        private void SetMargin()
        {
            WV.Margin = new Thickness(marginLeft, marginTop, marginRight, marginBottom);
        }
    }
}