using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AutomatedWeatherStation.Views.LiveWeather
{
    /// <summary>
    ///     Interaction logic for StationWeatherView.xaml
    /// </summary>
    public partial class StationWeatherView : UserControl
    {
        public StationWeatherView()
        {
            InitializeComponent();
        }

        private void StationWeatherView_OnLoaded(object sender, RoutedEventArgs e)
        {
            var timer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 1)};
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            var dateTime = DateTime.Now;
            txtBlockTime.Text = dateTime.ToString("h:mm:ss tt");
            txtBlockDate.Text = DateTime.Now.ToString("MMMM dd, yyyy");
        }
    }
}