using System;
using System.Globalization;
using System.Windows.Data;

namespace AutomatedWeatherStation.Converter
{
    public class MouseOverToHigherZIndex : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var condition = (bool) value;

            if (condition)
            {
                return 5;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}