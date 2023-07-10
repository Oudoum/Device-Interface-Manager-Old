using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace Device_Interface_Manager.Core
{
    public class StringToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is null)
            {
                return DependencyProperty.UnsetValue;
            }

            if (value is not string)
            {
                throw new ArgumentException("Value must be a string.");
            }

            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is null)
            {
                return DependencyProperty.UnsetValue;
            }

            return (bool)value ? parameter : DependencyProperty.UnsetValue;
        }
    }
}