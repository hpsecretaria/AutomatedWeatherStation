using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AutomatedWeatherStation.Converter
{
    public class BoolInverterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;
            return (int) value == System.Convert.ToInt32(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return null;
            if ((bool) value)
                return System.Convert.ToInt32(parameter);
            return DependencyProperty.UnsetValue;
        }
    }
}