using Device_Interface_Manager.SimConnectProfiles.PMDG;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static Device_Interface_Manager.SimConnectProfiles.PMDG.PMDG_CDU_SDK;

namespace Device_Interface_Manager.Views
{
    public partial class PMDGCDU : Window
    {
        private bool editormode = false;

        private FontFamily PMDGFontBig { get; set; } = new(new Uri("pack://application:,,,/"), "./Assets/Fonts/#PMDG_NGXu_DU_A");
        private FontFamily PMDGFontNormal { get; set; } = new(new Uri("pack://application:,,,/"), "./Assets/Fonts/#PMDG_NGXu_DU_B");
        private FontFamily PMDGFontsSmall { get; set; } = new(new Uri("pack://application:,,,/"), "./Assets/Fonts/#PMDG_NGXu_DU_C");

        private SolidColorBrush Transparent { get; set; } = new SolidColorBrush(Colors.Transparent);
        private SolidColorBrush Gray { get; set; } = new SolidColorBrush(Colors.Gray);

        private SolidColorBrush White { get; set; } = new(Colors.White);
        private SolidColorBrush Monochrome { get; set; } = new(Colors.Green);
        private SolidColorBrush Cyan { get; set; } = new(Colors.Cyan);
        private SolidColorBrush Green { get; set; } = new(Color.FromRgb(0x10, 0xEF, 0x22));
        private SolidColorBrush Magenta { get; set; } = new(Colors.Magenta);
        private SolidColorBrush Amber { get; set; } = new(Color.FromRgb(0xF4, 0xCD, 0x2A));
        private SolidColorBrush Red { get; set; } = new(Colors.Red);

        private Thickness Zero { get; set; } = new(0);
        private Thickness One { get; set; } = new(1);


        public double TextBlockFontSize { get; set; }
        public double MarginTop { get; set; }
        public double MarginBottom { get; set; }
        public double MarginLeft { get; set; }
        public double MarginRight { get; set; }

        public event EventHandler OnEditormodeOff;

        public PMDGCDU()
        {
            InitializeComponent();
        }

        public void CreatePMDGCDUCells()
        {
            int count = 0;
            for (int column = 0; column < 24; column++)
            {
                for (int row = 0; row < 14; row++)
                {
                    Border border = new()
                    {
                        BorderBrush = new SolidColorBrush(Colors.White),
                        BorderThickness = new Thickness(0),
                    };
                    TextBlock txtBlock = new()
                    {
                        FontSize = TextBlockFontSize,
                        FontWeight = FontWeights.Normal,
                        Margin = new Thickness(MarginLeft, MarginTop, MarginRight, MarginBottom),
                        Name = "Label" + count,
                        Padding = new Thickness(0),
                    };
                    border.Child = txtBlock;
                    Grid.SetColumn(border, column);
                    Grid.SetRow(border, row);
                    CDUGrid.Children.Add(border);
                }
            }
        }

        private int? _brightness;
        public int? Brightness
        {
            get => _brightness;
            set
            {
                if (_brightness != value)
                {
                    _brightness = value;
                    CDUGrid.Opacity = (double)(1d / 4095 * value);
                }
            }
        }

        public void GetPMDGCDUCells(ICDU_Screen cDU_Screen)
        {
            if (cDU_Screen.CDU_Screen.CDU_Columns is not null && CDUGrid.Children.Count > 0)
            {
                int count = 0;
                int columnCount = 0;
                byte brightness = 0;
                foreach (var column in cDU_Screen.CDU_Screen.CDU_Columns)
                {
                    int rowCount = 0;
                    foreach (var rowitem in column.CDU_ROWS)
                    {
                        Border border = (Border)CDUGrid.Children[count];
                        TextBlock txtBlock = border.Child as TextBlock;
                        char symbol = Convert.ToChar(rowitem.Symbol);
                        txtBlock.Text = symbol.ToString();

                        if (symbol == 'ë' && Brightness is null)
                        {
                            CDUGrid.Opacity = 1d / 23 * ++brightness;
                        }

                        if (brightness == 0 && columnCount == 23 && rowCount == 13 && symbol == '-' && rowitem.Flags == CDU_FLAG_UNUSED && Brightness is null)
                        {
                            CDUGrid.Opacity = 0;
                        }

                        if (editormode)
                        {
                            border.BorderThickness = One;
                        }

                        switch (rowitem.Color)
                        {
                            case CDU_COLOR_WHITE:
                                txtBlock.Foreground = White;
                                break;

                            case CDU_COLOR_CYAN:
                                txtBlock.Foreground = Cyan;
                                break;

                            case CDU_COLOR_GREEN:
                                txtBlock.Foreground = Green;
                                break;

                            case CDU_COLOR_MAGENTA:
                                txtBlock.Foreground = Magenta;
                                break;

                            case CDU_COLOR_AMBER:
                                txtBlock.Foreground = Amber;
                                break;

                            case CDU_COLOR_RED:
                                txtBlock.Foreground = Red;
                                break;

                            default:
                                break;
                        }

                        switch (rowitem.Flags)
                        {
                            case 0x00:
                                txtBlock.FontFamily = PMDGFontNormal;
                                txtBlock.FontWeight = FontWeights.Normal;
                                txtBlock.Background = Transparent;
                                txtBlock.Opacity = 1;
                                break;

                            case CDU_FLAG_SMALL_FONT:
                                txtBlock.FontFamily = PMDGFontsSmall;
                                txtBlock.FontWeight = FontWeight.FromOpenTypeWeight(550);
                                txtBlock.Background = Transparent;
                                txtBlock.Opacity = 1;
                                break;

                            case CDU_FLAG_REVERSE:
                                txtBlock.FontFamily = PMDGFontNormal;
                                txtBlock.FontWeight = FontWeights.Normal;
                                txtBlock.Background = Gray;
                                txtBlock.Opacity = 1;
                                break;

                            case CDU_FLAG_SMALL_FONT + CDU_FLAG_REVERSE:
                                txtBlock.FontFamily = PMDGFontsSmall;
                                txtBlock.FontWeight = FontWeight.FromOpenTypeWeight(550);
                                txtBlock.Background = Gray;
                                txtBlock.Opacity = 1;
                                break;

                            case CDU_FLAG_UNUSED:
                                txtBlock.FontFamily = PMDGFontNormal;
                                txtBlock.FontWeight = FontWeights.Normal;
                                txtBlock.Background = Transparent;
                                txtBlock.Opacity = 0.5;
                                txtBlock.Foreground = White;
                                break;

                            case CDU_FLAG_SMALL_FONT + CDU_FLAG_UNUSED:
                                txtBlock.FontFamily = PMDGFontsSmall;
                                txtBlock.FontWeight = FontWeight.FromOpenTypeWeight(550);
                                txtBlock.Background = Transparent;
                                txtBlock.Opacity = 0.5;
                                break;
                        }

                        if (cDU_Screen is PMDG_747QOTSII_SDK.PMDG_747QOTSII_CDU_Screen cDU747 && cDU747.isMonochrome)
                        {
                            txtBlock.Foreground = Green;
                            if (txtBlock.Background != Transparent)
                            {
                                txtBlock.Background = Green;
                            }
                        }

                        if (columnCount == 0 && rowCount == 0)
                        {
                            txtBlock.FontFamily = PMDGFontBig;
                        }

                        count++;
                        rowCount++;
                    }
                    columnCount++;
                }
            }
        }

        public async void ClearPMDGCDUCells()
        {
            await System.Threading.Tasks.Task.Delay(500);
            Dispatcher.Invoke(delegate ()
            {
                foreach (Border br in CDUGrid.Children.OfType<Border>())
                {
                    if (br.Child is TextBlock textBlock)
                    {
                        textBlock.Text = null;
                    }
                }
            });
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

        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            editormode = !editormode;
            foreach (Border cells in CDUGrid.Children)
            {
                if (cells.BorderThickness == Zero)
                {
                    cells.BorderThickness = One;
                }
                else
                {
                    cells.BorderThickness = Zero;
                }
            }
            if (!editormode)
            {
                OnEditormodeOff?.Invoke(this, e);
            }
        }

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
                    case Key.F when Keyboard.IsKeyDown(Key.LeftShift):
                        TextBlockFontSize++;
                        foreach (Border cells in CDUGrid.Children)
                        {
                            (cells.Child as TextBlock).FontSize = TextBlockFontSize;
                        }
                        break;
                    case Key.F when TextBlockFontSize != 1:
                        TextBlockFontSize--;
                        foreach (Border cells in CDUGrid.Children)
                        {
                            (cells.Child as TextBlock).FontSize = TextBlockFontSize;
                        }
                        break;

                    case Key.Space when Keyboard.IsKeyDown(Key.LeftShift):
                        CDUGrid.ClearValue(HeightProperty);
                        CDUGrid.ClearValue(WidthProperty);
                        break;

                    case Key.Enter when Keyboard.IsKeyDown(Key.LeftShift):
                        CDUGrid.ClearValue(HeightProperty);
                        CDUGrid.ClearValue(WidthProperty);
                        foreach (Border cells in CDUGrid.Children)
                        {
                            (cells.Child as TextBlock).FontSize = 60;
                            cells.Margin = new Thickness(0);
                            TextBlockFontSize = 60;
                            MarginLeft = 0;
                            MarginTop = 0;
                            MarginRight = 0;
                            MarginBottom = 0;
                        }
                        break;

                    case Key.PageUp when Keyboard.IsKeyDown(Key.LeftShift):
                        MarginTop--;
                        foreach (Border cells in CDUGrid.Children)
                        {
                            SetMargin(cells);
                        }
                        break;

                    case Key.PageUp:
                        MarginBottom++;
                        foreach (Border cells in CDUGrid.Children)
                        {
                            SetMargin(cells);
                        }
                        break;

                    case Key.PageDown when Keyboard.IsKeyDown(Key.LeftShift):
                        MarginTop++;
                        foreach (Border cells in CDUGrid.Children)
                        {
                            SetMargin(cells);
                        }
                        break;

                    case Key.PageDown:
                        MarginBottom--;
                        foreach (Border cells in CDUGrid.Children)
                        {
                            SetMargin(cells);
                        }
                        break;

                    case Key.Add when Keyboard.IsKeyDown(Key.LeftShift):
                        MarginRight++;
                        foreach (Border cells in CDUGrid.Children)
                        {
                            SetMargin(cells);
                        }
                        break;

                    case Key.Add:
                        MarginLeft--;
                        foreach (Border cells in CDUGrid.Children)
                        {
                            SetMargin(cells);
                        }
                        break;

                    case Key.Subtract when Keyboard.IsKeyDown(Key.LeftShift):
                        MarginRight--;
                        foreach (Border cells in CDUGrid.Children)
                        {
                            SetMargin(cells);
                        }
                        break;

                    case Key.Subtract:
                        MarginLeft++;
                        foreach (Border cells in CDUGrid.Children)
                        {
                            SetMargin(cells);
                        }
                        break;


                    case Key.W:
                        CDUGrid.Height = CDUGrid.ActualHeight;
                        CDUGrid.Height = --CDUGrid.Height;
                        break;

                    case Key.A:
                        CDUGrid.Width = CDUGrid.ActualWidth;
                        CDUGrid.Width = ++CDUGrid.Width;
                        break;

                    case Key.S:
                        CDUGrid.Height = CDUGrid.ActualHeight;
                        CDUGrid.Height = ++CDUGrid.Height;
                        break;

                    case Key.D:
                        CDUGrid.Width = CDUGrid.ActualWidth;
                        CDUGrid.Width = --CDUGrid.Width;
                        break;

                    case Key.Up:
                        MarginTop--;
                        MarginBottom++;
                        foreach (Border cells in CDUGrid.Children)
                        {
                            SetMargin(cells);
                        }
                        break;

                    case Key.Left:
                        MarginLeft--;
                        MarginRight++;
                        foreach (Border cells in CDUGrid.Children)
                        {
                            SetMargin(cells);
                        }
                        break;

                    case Key.Down:
                        MarginTop++;
                        MarginBottom--;
                        foreach (Border cells in CDUGrid.Children)
                        {
                            SetMargin(cells);
                        }
                        break;

                    case Key.Right:
                        MarginLeft++;
                        MarginRight--;
                        foreach (Border cells in CDUGrid.Children)
                        {
                            SetMargin(cells);
                        }
                        break;
                }
            }
        }

        private void SetMargin(Border cells)
        {
            cells.Margin = new Thickness(MarginLeft, MarginTop, MarginRight, MarginBottom);
        }
    }
}