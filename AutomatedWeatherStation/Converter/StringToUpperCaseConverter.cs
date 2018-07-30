using System;
using System.Globalization;
using System.Windows.Data;

namespace AutomatedWeatherStation.Converter
{
    public class DateToUpperCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = (DateTime) value;

            var param = (string) parameter;

            if (param == "dayoftheweek")
            {
                var dow = s.DayOfWeek.ToString();
                return dow.ToUpper();
            }
            if (param == "monthandday")
            {
                var dow = s.ToString("MMMM d");
                return dow.ToUpper();
            }
            if (param == "time")
            {
                var dow = s.ToString("h:mm tt");
                return dow.ToUpper();
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}