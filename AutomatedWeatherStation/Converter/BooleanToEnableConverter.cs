using System;
using System.Globalization;
using System.Windows.Data;

namespace AutomatedWeatherStation.Converter
{
    internal class BooleanToEnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isinverse = parameter as string == "inverse";

            var condition = (bool) value;

            if (condition)
            {
                return isinverse;
            }
            return !isinverse;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}