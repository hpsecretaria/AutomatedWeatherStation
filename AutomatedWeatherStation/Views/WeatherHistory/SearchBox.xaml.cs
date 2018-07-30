using System;
using System.Windows.Controls;

namespace AutomatedWeatherStation.Views.WeatherHistory
{
    /// <summary>
    ///     Interaction logic for SearchBox.xaml
    /// </summary>
    public partial class SearchBox : UserControl
    {
        public SearchBox()
        {
            InitializeComponent();
            DatePickerEnd.DisplayDateEnd = DateTime.Now;
        }
    }
}