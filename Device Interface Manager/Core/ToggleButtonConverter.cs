using System;
using System.Globalization;
using System.Windows.Data;

namespace Device_Interface_Manager.Core;
public class ToggleButtonConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        return Tuple.Create((bool)values[0], System.Convert.ToInt32((string)values[1]));
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        return null;

    }
}