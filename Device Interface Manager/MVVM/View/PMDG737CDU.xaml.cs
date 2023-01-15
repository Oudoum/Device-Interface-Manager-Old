using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static Device_Interface_Manager.MSFSProfiles.PMDG.PMDG_NG3_SDK;

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
            this.ClearPMDGCDUCells();

            int Column = 0;
            if (pMDG_NG3_CDU_Screen.CDU_Columns is not null && pMDG_NG3_CDU_Screen.Powered)
            {
                foreach (var columns in pMDG_NG3_CDU_Screen.CDU_Columns)
                {
                    int Row = 0;
                    foreach (var row in columns.CDU_ROWS)
                    {
                        this.lb = new Label
                        {
                            FontSize = this.fontSize,
                            FontWeight = FontWeights.Normal,
                            Margin = new Thickness(this.marginLeft, this.marginTop, this.marginRight, this.marginBottom),
                            BorderBrush = new SolidColorBrush(Colors.White),
                            Background = new SolidColorBrush(Colors.Transparent),
                            FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#PMDG_NGXu_DU_B"),
                            Name = "Label" + Column.ToString() + Row.ToString(),
                            BorderThickness = new Thickness(0),
                            Padding = new Thickness(0),
                            Content = Convert.ToChar(row.Symbol),
                        };
                        Grid.SetColumn(lb, Column);
                        Grid.SetRow(lb, Row);

                        if (this.editormode)
                        {
                            this.lb.BorderThickness = new Thickness(1);
                        }

                        switch (row.Color)
                        {
                            case PMDG_NG3_CDU_COLOR_WHITE:
                                this.lb.Foreground = new SolidColorBrush(Colors.White);
                                break;

                            case PMDG_NG3_CDU_COLOR_CYAN:
                                this.lb.Foreground = new SolidColorBrush(Colors.Cyan);
                                break;

                            case PMDG_NG3_CDU_COLOR_GREEN:
                                this.lb.Foreground = (Brush)new BrushConverter().ConvertFrom("#10EF22");
                                break;

                            case PMDG_NG3_CDU_COLOR_MAGENTA:
                                this.lb.Foreground = new SolidColorBrush(Colors.Magenta);
                                break;

                            case PMDG_NG3_CDU_COLOR_AMBER:
                                this.lb.Foreground = (Brush)new BrushConverter().ConvertFrom("#F4CD2A");
                                break;

                            case PMDG_NG3_CDU_COLOR_RED:
                                this.lb.Foreground = new SolidColorBrush(Colors.Red);
                                break;

                            default:
                                break;
                        }

                        switch (row.Flags)
                        {
                            case PMDG_NG3_CDU_FLAG_SMALL_FONT:
                                this.lb.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#PMDG_NGXu_DU_C");
                                this.lb.FontWeight = FontWeight.FromOpenTypeWeight(600);

                                break;

                            case PMDG_NG3_CDU_FLAG_REVERSE:
                                this.lb.Background = (Brush)new BrushConverter().ConvertFrom("#B7D2EF");
                                break;

                            case PMDG_NG3_CDU_FLAG_UNUSED:
                                this.lb.Foreground = new SolidColorBrush(Colors.White);
                                this.lb.Opacity = 0.5;
                                break;

                            case PMDG_NG3_CDU_FLAG_SMALL_FONT + PMDG_NG3_CDU_FLAG_UNUSED:
                                this.lb.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#PMDG_NGXu_DU_C");
                                this.lb.FontWeight = FontWeight.FromOpenTypeWeight(600);
                                this.lb.Foreground = new SolidColorBrush(Colors.White);
                                this.lb.Opacity = 0.5;
                                break;

                            default:
                                break;
                        }

                        switch (Column)
                        {
                            case 0:
                                if (Row == 0)
                                {
                                    this.lb.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#PMDG_NGXu_DU_A");
                                }
                                this.CDUGrid.Children.Add(lb);
                                break;

                            case 1:
                                this.CDUGrid.Children.Add(lb);
                                break;

                            case 2:
                                this.CDUGrid.Children.Add(lb);
                                break;

                            case 3:
                                this.CDUGrid.Children.Add(lb);
                                break;

                            case 4:
                                this.CDUGrid.Children.Add(lb);
                                break;

                            case 5:
                                this.CDUGrid.Children.Add(lb);
                                break;

                            case 6:
                                this.CDUGrid.Children.Add(lb);
                                break;

                            case 7:
                                this.CDUGrid.Children.Add(lb);
                                break;

                            case 8:
                                this.CDUGrid.Children.Add(lb);
                                break;

                            case 9:
                                this.CDUGrid.Children.Add(lb);
                                break;

                            case 10:
                                this.CDUGrid.Children.Add(lb);
                                break;

                            case 11:
                                this.CDUGrid.Children.Add(lb);
                                break;

                            case 12:
                                this.CDUGrid.Children.Add(lb);
                                break;

                            case 13:
                                this.CDUGrid.Children.Add(lb);
                                break;

                            case 14:
                                this.CDUGrid.Children.Add(lb);
                                break;

                            case 15:
                                this.CDUGrid.Children.Add(lb);
                                break;

                            case 16:
                                this.CDUGrid.Children.Add(lb);
                                break;

                            case 17:
                                this.CDUGrid.Children.Add(lb);
                                break;

                            case 18:
                                this.CDUGrid.Children.Add(lb);
                                break;

                            case 19:
                                this.CDUGrid.Children.Add(lb);
                                break;

                            case 20:
                                this.CDUGrid.Children.Add(lb);
                                break;

                            case 21:
                                this.CDUGrid.Children.Add(lb);
                                break;

                            case 22:
                                this.CDUGrid.Children.Add(lb);
                                break;

                            case 23:
                                this.CDUGrid.Children.Add(lb);
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
            this.CDUGrid.Children.Clear();
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
            foreach (Label cells in this.CDUGrid.Children)
            {
                if (cells.BorderThickness == new Thickness(0))
                {
                    cells.BorderThickness = new Thickness(1);
                }
                else 
                {
                    cells.BorderThickness = new Thickness(0);
                }
            }
            this.editormode = !this.editormode;
        }

        public double fontSize;
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
            if (this.editormode)
            {
                switch (e.Key)
                {
                    case Key.F when Keyboard.IsKeyDown(Key.LeftShift):
                        ++this.fontSize;
                        foreach (Label cells in this.CDUGrid.Children)
                        {
                            cells.FontSize = this.fontSize;
                        }
                        break;
                    case Key.F when fontSize != 1:
                        --this.fontSize;
                        foreach (Label cells in this.CDUGrid.Children)
                        {
                            cells.FontSize = this.fontSize;
                        }
                        break;

                    case Key.Space when Keyboard.IsKeyDown(Key.LeftShift):
                        this.CDUGrid.ClearValue(HeightProperty);
                        this.CDUGrid.ClearValue(WidthProperty);
                        break;

                    case Key.Enter when Keyboard.IsKeyDown(Key.LeftShift):
                        this.CDUGrid.ClearValue(HeightProperty);
                        this.CDUGrid.ClearValue(WidthProperty);
                        foreach (Label cells in this.CDUGrid.Children)
                        {
                            cells.FontSize = 60;
                            cells.Margin = new Thickness(0);
                            this.marginLeft = 0;
                            this.marginTop = 0;
                            this.marginRight = 0;
                            this.marginBottom = 0;
                        }
                        break;

                    case Key.PageUp when Keyboard.IsKeyDown(Key.LeftShift):
                        --this.marginTop;
                        foreach (Label cells in this.CDUGrid.Children)
                        {
                            cells.Margin = new Thickness(this.marginLeft, this.marginTop, this.marginRight, this.marginBottom);
                        }
                        break;

                    case Key.PageUp:
                        ++this.marginBottom;
                        foreach (Label cells in this.CDUGrid.Children)
                        {
                            cells.Margin = new Thickness(this.marginLeft, this.marginTop, this.marginRight, this.marginBottom);
                        }
                        break;

                    case Key.PageDown when Keyboard.IsKeyDown(Key.LeftShift):
                        ++this.marginTop;
                        foreach (Label cells in this.CDUGrid.Children)
                        {
                            cells.Margin = new Thickness(this.marginLeft, this.marginTop, this.marginRight, this.marginBottom);
                        }
                        break;

                    case Key.PageDown:
                        --this.marginBottom;
                        foreach (Label cells in this.CDUGrid.Children)
                        {
                            cells.Margin = new Thickness(this.marginLeft, this.marginTop, this.marginRight, this.marginBottom);
                        }
                        break;

                    case Key.Add when Keyboard.IsKeyDown(Key.LeftShift):
                        ++this.marginRight;
                        foreach (Label cells in this.CDUGrid.Children)
                        {
                            cells.Margin = new Thickness(this.marginLeft, this.marginTop, this.marginRight, this.marginBottom);
                        }
                        break;

                    case Key.Add:
                        --this.marginLeft;
                        foreach (Label cells in this.CDUGrid.Children)
                        {
                            cells.Margin = new Thickness(this.marginLeft, this.marginTop, this.marginRight, this.marginBottom);
                        }
                        break;

                    case Key.Subtract when Keyboard.IsKeyDown(Key.LeftShift):
                        --this.marginRight;
                        foreach (Label cells in this.CDUGrid.Children)
                        {
                            cells.Margin = new Thickness(this.marginLeft, this.marginTop, this.marginRight, this.marginBottom);
                        }
                        break;

                    case Key.Subtract:
                        ++this.marginLeft;
                        foreach (Label cells in this.CDUGrid.Children)
                        {
                            cells.Margin = new Thickness(this.marginLeft, this.marginTop, this.marginRight, this.marginBottom);
                        }
                        break;


                    case Key.W:
                        this.CDUGrid.Height = this.CDUGrid.ActualHeight;
                        this.CDUGrid.Height = --this.CDUGrid.Height;
                        break;

                    case Key.A:
                        this.CDUGrid.Width = this.CDUGrid.ActualWidth;
                        this.CDUGrid.Width = ++this.CDUGrid.Width;
                        break;

                    case Key.S:
                        this.CDUGrid.Height = this.CDUGrid.ActualHeight;
                        this.CDUGrid.Height = ++this.CDUGrid.Height;
                        break;

                    case Key.D:
                        this.CDUGrid.Width = this.CDUGrid.ActualWidth;
                        this.CDUGrid.Width = --this.CDUGrid.Width;
                        break;

                    case Key.Up:
                        --this.marginTop;
                        ++this.marginBottom;
                        foreach (Label cells in this.CDUGrid.Children)
                        {
                            cells.Margin = new Thickness(this.marginLeft, this.marginTop, this.marginRight, this.marginBottom);
                        }
                        break;

                    case Key.Left:
                        --this.marginLeft;
                        ++this.marginRight;
                        foreach (Label cells in this.CDUGrid.Children)
                        {
                            cells.Margin = new Thickness(this.marginLeft, this.marginTop, this.marginRight, this.marginBottom);
                        }
                        break;

                    case Key.Down:
                        ++this.marginTop;
                        --this.marginBottom;
                        foreach (Label cells in this.CDUGrid.Children)
                        {
                            cells.Margin = new Thickness(this.marginLeft, this.marginTop, this.marginRight, this.marginBottom);
                        }
                        break;

                    case Key.Right:
                        ++this.marginLeft;
                        --this.marginRight;
                        foreach (Label cells in this.CDUGrid.Children)
                        {
                            cells.Margin = new Thickness(this.marginLeft, this.marginTop, this.marginRight, this.marginBottom);
                        }
                        break;

                    default:
                        break;
                }
            }
        }
    }
}