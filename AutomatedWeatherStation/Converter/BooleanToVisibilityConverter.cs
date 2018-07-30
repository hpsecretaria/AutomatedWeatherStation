using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AutomatedWeatherStation.Converter
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isinverse = parameter as string == "inverse";

            var condition = (bool) value;

            if (condition)
            {
                return isinverse ? Visibility.Visible : Visibility.Collapsed;
            }
            return isinverse ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}