using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static Device_Interface_Manager.Profiles.PMDG.PMDG_NG3_SDK;

namespace Device_Interface_Manager.MVVM.View
{
    public partial class PMDG737CDU : Window
    {
        Label lb;
        public PMDG737CDU()
        {
            InitializeComponent();
        }

        public void GetPMDGCDUCells(PMDG_NG3_CDU_Screen pMDG_NG3_CDU_Screen)
        {
            CDUGrid.Children.Clear();

            int Column = 0;
            if (pMDG_NG3_CDU_Screen.CDU_Columns != null && pMDG_NG3_CDU_Screen.Powered)
            {
                foreach (var columns in pMDG_NG3_CDU_Screen.CDU_Columns)
                {
                    int Row = 0;
                    foreach (var row in columns.CDU_ROWS)
                    {
                        lb = new Label
                        {
                            FontSize = fontSize,
                            FontWeight = FontWeights.Normal,
                            Margin = new Thickness(marginLeft, marginTop, marginRight, marginBottom),
                            BorderBrush = new SolidColorBrush(Colors.White),
                            Background = new SolidColorBrush(Colors.Transparent),
                            FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#PMDG_NGXu_DU_B"),
                            Name = "Label" + Column.ToString() + Row.ToString(),
                            Content = Convert.ToChar(row.Symbol)
                        };
                        Grid.SetColumn(lb, Column);
                        Grid.SetRow(lb, Row);
                        Grid.SetColumnSpan(lb, 24);
                        Grid.SetRowSpan(lb, 14);

                        switch (row.Color)
                        {
                            case PMDG_NG3_CDU_COLOR_WHITE:
                                lb.Foreground = new SolidColorBrush(Colors.White);
                                break;

                            case PMDG_NG3_CDU_COLOR_CYAN:
                                lb.Foreground = new SolidColorBrush(Colors.Cyan);
                                break;

                            case PMDG_NG3_CDU_COLOR_GREEN:
                                lb.Foreground = (Brush)new BrushConverter().ConvertFrom("#10EF22");
                                break;

                            case PMDG_NG3_CDU_COLOR_MAGENTA:
                                lb.Foreground = new SolidColorBrush(Colors.Magenta);
                                break;

                            case PMDG_NG3_CDU_COLOR_AMBER:
                                lb.Foreground = (Brush)new BrushConverter().ConvertFrom("#F4CD2A");
                                break;

                            case PMDG_NG3_CDU_COLOR_RED:
                                lb.Foreground = new SolidColorBrush(Colors.Red);
                                break;

                            default:
                                break;
                        }

                        switch (row.Flags)
                        {
                            case PMDG_NG3_CDU_FLAG_SMALL_FONT:
                                lb.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#PMDG_NGXu_DU_C");
                                lb.FontWeight = FontWeight.FromOpenTypeWeight(550);
                                break;

                            case PMDG_NG3_CDU_FLAG_REVERSE:
                                break;

                            case PMDG_NG3_CDU_FLAG_UNUSED:
                                lb.Foreground = new SolidColorBrush(Colors.White);
                                lb.Opacity = 0.5;
                                break;

                            default:
                                break;
                        }

                        switch (Column)
                        {
                            case 0:
                                if (Row == 0)
                                {
                                    lb.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#PMDG_NGXu_DU_A");
                                }
                                CDUGrid.Children.Add(lb);
                                break;

                            case 1:
                                CDUGrid.Children.Add(lb);
                                break;

                            case 2:
                                CDUGrid.Children.Add(lb);
                                break;

                            case 3:
                                CDUGrid.Children.Add(lb);
                                break;

                            case 4:
                                CDUGrid.Children.Add(lb);
                                break;

                            case 5:
                                CDUGrid.Children.Add(lb);
                                break;

                            case 6:
                                CDUGrid.Children.Add(lb);
                                break;

                            case 7:
                                CDUGrid.Children.Add(lb);
                                break;

                            case 8:
                                CDUGrid.Children.Add(lb);
                                break;

                            case 9:
                                CDUGrid.Children.Add(lb);
                                break;

                            case 10:
                                CDUGrid.Children.Add(lb);
                                break;

                            case 11:
                                CDUGrid.Children.Add(lb);
                                break;

                            case 12:
                                CDUGrid.Children.Add(lb);
                                break;

                            case 13:
                                CDUGrid.Children.Add(lb);
                                break;

                            case 14:
                                CDUGrid.Children.Add(lb);
                                break;

                            case 15:
                                CDUGrid.Children.Add(lb);
                                break;

                            case 16:
                                CDUGrid.Children.Add(lb);
                                break;

                            case 17:
                                CDUGrid.Children.Add(lb);
                                break;

                            case 18:
                                CDUGrid.Children.Add(lb);
                                break;

                            case 19:
                                CDUGrid.Children.Add(lb);
                                break;

                            case 20:
                                CDUGrid.Children.Add(lb);
                                break;

                            case 21:
                                CDUGrid.Children.Add(lb);
                                break;

                            case 22:
                                CDUGrid.Children.Add(lb);
                                break;

                            case 23:
                                CDUGrid.Children.Add(lb);
                                break;

                            default:
                                break;

                        }
                        ++Row;
                    }
                    ++Column;
                }
            }
        }

        public void ClearPMDGCDUCells()
        {
            CDUGrid.Children.Clear();
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
                }
                else
                {
                    WindowState = WindowState.Normal;
                }
            }
        }

        bool editormode = false;
        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            foreach (Label cells in CDUGrid.Children)
            {
                if (cells.BorderThickness == new Thickness(0))
                {
                    cells.BorderThickness = new Thickness(1);
                    editormode = true;
                }
                else
                {
                    cells.BorderThickness = new Thickness(0);
                    editormode = false;
                }
            }
        }

        public double fontSize;
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
            if (editormode)
            {
                switch (e.Key)
                {
                    case Key.F:
                        if (Keyboard.IsKeyDown(Key.LeftShift))
                        {
                            ++fontSize;
                            foreach (Label cells in CDUGrid.Children)
                            {
                                cells.FontSize = fontSize;
                            }
                        }
                        else
                        {
                            --fontSize;
                            foreach (Label cells in CDUGrid.Children)
                            {
                                cells.FontSize = fontSize;
                            }
                        }
                        break;

                    case Key.Space:
                        if (Keyboard.IsKeyDown(Key.LeftShift))
                        {
                            CDUGrid.ClearValue(HeightProperty);
                            CDUGrid.ClearValue(WidthProperty);
                        }
                        break;

                    case Key.Enter:
                        if (Keyboard.IsKeyDown(Key.LeftShift))
                        {
                            CDUGrid.ClearValue(HeightProperty);
                            CDUGrid.ClearValue(WidthProperty);
                            foreach (Label cells in CDUGrid.Children)
                            {
                                cells.FontSize = 60;
                                cells.Margin = new Thickness(0);
                                marginLeft = 0;
                                marginTop = 0;
                                marginRight = 0;
                                marginBottom = 0;
                            }
                        }
                        break;

                    case Key.PageUp:

                        if (Keyboard.IsKeyDown(Key.LeftShift))
                        {
                            --marginTop;
                            foreach (Label cells in CDUGrid.Children)
                            {
                                cells.Margin = new Thickness(marginLeft, marginTop, marginRight, marginBottom);
                            }
                        }
                        else
                        {
                            ++marginBottom;
                            foreach (Label cells in CDUGrid.Children)
                            {
                                cells.Margin = new Thickness(marginLeft, marginTop, marginRight, marginBottom);
                            }
                        }
                        break;

                    case Key.PageDown:
                        if (Keyboard.IsKeyDown(Key.LeftShift))
                        {
                            ++marginTop;
                            foreach (Label cells in CDUGrid.Children)
                            {
                                cells.Margin = new Thickness(marginLeft, marginTop, marginRight, marginBottom);
                            }
                        }
                        else
                        {
                            --marginBottom;
                            foreach (Label cells in CDUGrid.Children)
                            {
                                cells.Margin = new Thickness(marginLeft, marginTop, marginRight, marginBottom);
                            }
                        }
                        break;

                    case Key.Multiply:
                        if (Keyboard.IsKeyDown(Key.LeftShift))
                        {
                            ++marginRight;
                            foreach (Label cells in CDUGrid.Children)
                            {
                                cells.Margin = new Thickness(marginLeft, marginTop, marginRight, marginBottom);
                            }
                        }
                        else
                        {
                            --marginLeft;
                            foreach (Label cells in CDUGrid.Children)
                            {
                                cells.Margin = new Thickness(marginLeft, marginTop, marginRight, marginBottom);
                            }
                        }
                        break;

                    case Key.Subtract:
                        if (Keyboard.IsKeyDown(Key.LeftShift))
                        {
                            --marginRight;
                            foreach (Label cells in CDUGrid.Children)
                            {
                                cells.Margin = new Thickness(marginLeft, marginTop, marginRight, marginBottom);
                            }
                        }
                        else
                        {
                            ++marginLeft;
                            foreach (Label cells in CDUGrid.Children)
                            {
                                cells.Margin = new Thickness(marginLeft, marginTop, marginRight, marginBottom);
                            }
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
                        --marginTop;
                        ++marginBottom;
                        foreach (Label cells in CDUGrid.Children)
                        {
                            cells.Margin = new Thickness(marginLeft, marginTop, marginRight, marginBottom);
                        }
                        break;

                    case Key.Left:
                        --marginLeft;
                        ++marginRight;
                        foreach (Label cells in CDUGrid.Children)
                        {
                            cells.Margin = new Thickness(marginLeft, marginTop, marginRight, marginBottom);
                        }
                        break;

                    case Key.Down:
                        ++marginTop;
                        --marginBottom;
                        foreach (Label cells in CDUGrid.Children)
                        {
                            cells.Margin = new Thickness(marginLeft, marginTop, marginRight, marginBottom);
                        }
                        break;

                    case Key.Right:
                        ++marginLeft;
                        --marginRight;
                        foreach (Label cells in CDUGrid.Children)
                        {
                            cells.Margin = new Thickness(marginLeft, marginTop, marginRight, marginBottom);
                        }
                        break;

                    default:
                        break;
                }
            }
        }
    }
}