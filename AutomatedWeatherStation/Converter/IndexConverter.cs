using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace AutomatedWeatherStation.Converter
{
    public class IndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Get the ListViewItem from Value remember we deleted Path, so the value is an object of ListViewItem and not Person
            var lvi = (ListViewItem) value;
            //Get lvi's container (listview)
            var listView = ItemsControl.ItemsControlFromItemContainer(lvi) as ListView;

            //Find out the position for the Person obj in the ListView
            //we can get the Person object from lvi.Content
            // Of course you can do as in the accepted answer instead!
            // I just think this is easier to understand for a beginner.
            var index = listView.Items.IndexOf(lvi.Content);

            //Convert your XML parameter value of 1 to an int.
            var startingIndex = System.Convert.ToInt32(parameter);

            return index + startingIndex;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}