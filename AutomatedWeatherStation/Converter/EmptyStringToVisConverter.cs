using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AutomatedWeatherStation.Converter
{
    public class EmptyStringToVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isinverse = parameter as string == "inverse";
            var condition = string.IsNullOrEmpty((string) value);

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