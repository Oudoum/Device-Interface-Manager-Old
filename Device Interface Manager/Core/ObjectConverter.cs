using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Runtime.CompilerServices;

namespace Device_Interface_Manager.Core;

public class ObjectToUIElementConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null)
            return DependencyProperty.UnsetValue;
        if (!uiElements.TryGetValue(value, out var element))
        {
            if (value is UIElement elm)
            {
                element = elm;
            }
            else
            {
                element = new ContentControl()
                { DataContext = value, Content = value };
            }
            uiElements.Add(value, element);
        }
        return element;
    }

    private readonly ConditionalWeakTable<object, UIElement> uiElements = new();

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public static ObjectToUIElementConverter Default { get; } = new ObjectToUIElementConverter();
}