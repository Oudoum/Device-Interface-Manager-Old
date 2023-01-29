using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Device_Interface_Manager.MVVM.View
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
                this.DragMove();
            }
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (this.WindowState != WindowState.Maximized)
                {
                    this.WindowState = WindowState.Maximized;
                    return;
                }
                this.WindowState = WindowState.Normal;
            }
        }

        bool editormode = false;
        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.editormode = !this.editormode;
            if (this.editormode)
            {
                this.Background = new SolidColorBrush(Colors.Blue);
                return;
            }
            this.EditormodeOff?.Invoke(this, e);
            this.Background = new SolidColorBrush(Colors.Transparent);
        }

        public event EventHandler EditormodeOff;

        public double marginTop;
        public double marginBottom;
        public double marginLeft;
        public double marginRight;
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!this.editormode && e.Key == Key.Escape)
            {
                Hide();
            }
            else if (this.editormode)
            {
                switch (e.Key)
                {
                    case Key.Space when Keyboard.IsKeyDown(Key.LeftShift):
                        this.MCDUGrid.ClearValue(HeightProperty);
                        this.MCDUGrid.ClearValue(WidthProperty);
                        break;

                    case Key.Enter when Keyboard.IsKeyDown(Key.LeftShift):
                        this.MCDUGrid.ClearValue(HeightProperty);
                        this.MCDUGrid.ClearValue(WidthProperty);
                        this.WV.Margin = new Thickness(0);
                        this.marginLeft = 0;
                        this.marginTop = 0;
                        this.marginRight = 0;
                        this.marginBottom = 0;
                        break;

                    case Key.PageUp when Keyboard.IsKeyDown(Key.LeftShift):
                        --this.marginTop;
                        SetMargin();
                        break;

                    case Key.PageUp:
                        ++this.marginBottom;
                        SetMargin();
                        break;

                    case Key.PageDown when Keyboard.IsKeyDown(Key.LeftShift):
                        ++this.marginTop;
                        SetMargin();

                        break;

                    case Key.PageDown:
                        --this.marginBottom;
                        SetMargin();
                        break;

                    case Key.Add when Keyboard.IsKeyDown(Key.LeftShift):
                        ++this.marginRight;
                        SetMargin();
                        break;

                    case Key.Add:
                        --this.marginLeft;
                        SetMargin();
                        break;

                    case Key.Subtract when Keyboard.IsKeyDown(Key.LeftShift):
                        --this.marginRight;
                        SetMargin();
                        break;

                    case Key.Subtract:
                        ++this.marginLeft;
                        SetMargin();
                        break;


                    case Key.W:
                        this.MCDUGrid.Height = this.MCDUGrid.ActualHeight;
                        this.MCDUGrid.Height = --this.MCDUGrid.Height;
                        break;

                    case Key.A:
                        this.MCDUGrid.Width = this.MCDUGrid.ActualWidth;
                        this.MCDUGrid.Width = ++this.MCDUGrid.Width;
                        break;

                    case Key.S:
                        this.MCDUGrid.Height = this.MCDUGrid.ActualHeight;
                        this.MCDUGrid.Height = ++this.MCDUGrid.Height;
                        break;

                    case Key.D:
                        this.MCDUGrid.Width = this.MCDUGrid.ActualWidth;
                        this.MCDUGrid.Width = --this.MCDUGrid.Width;
                        break;

                    case Key.Up:
                        --this.marginTop;
                        ++this.marginBottom;
                        SetMargin();
                        break;

                    case Key.Left:
                        --this.marginLeft;
                        ++this.marginRight;
                        SetMargin();
                        break;

                    case Key.Down:
                        ++this.marginTop;
                        --this.marginBottom;
                        SetMargin();
                        break;

                    case Key.Right:
                        ++this.marginLeft;
                        --this.marginRight;
                        SetMargin();
                        break;

                    default:
                        break;
                }
            }
        }

        private void SetMargin()
        {
            this.WV.Margin = new Thickness(this.marginLeft, this.marginTop, this.marginRight, this.marginBottom);
        }
    }
}