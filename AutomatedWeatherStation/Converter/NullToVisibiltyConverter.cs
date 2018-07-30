using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AutomatedWeatherStation.Converter
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isInverse = parameter as string == "inverse";

            if (value == null)
            {
                return isInverse ? Visibility.Visible : Visibility.Collapsed;
            }

            return isInverse ? Visibility.Collapsed : Visibility.Visible;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}