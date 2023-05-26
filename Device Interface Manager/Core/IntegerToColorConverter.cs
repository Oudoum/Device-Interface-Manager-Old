using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Device_Interface_Manager.Core
{
    public class IntegerToColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[1] == DependencyProperty.UnsetValue)
            {
                return Brushes.Transparent;
            }

            try
            {
                if (((bool[])values[1])[int.Parse((string)values[0]) - 1])
                {
                    return Brushes.Green;
                }
                return Brushes.Transparent;
            }
            catch (Exception)
            {
                return Brushes.Transparent;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}