using System.Windows;
using AutomatedWeatherStation.DataAccess;

namespace AutomatedWeatherStation.Modules
{
    public sealed class ViewModelLocator
    {
        private static readonly IRepository _repository = new EfRepository();

        public ViewModelLocator()
        {
            StationMapsModule = new StationMapsModule(_repository);

            LogInModule = new LogInModule(_repository);
            MiscellaneousModule = new MiscellaneousModule(_repository);
            StationsModule = new StationsModule(_repository);
            MeasurementsModule = new MeasurementsModule(_repository);
        }

        public StationMapsModule StationMapsModule { get; set; }
        public MeasurementsModule MeasurementsModule { get; set; }
        public MiscellaneousModule MiscellaneousModule { get; set; }
        public StationsModule StationsModule { get; set; }
        public LogInModule LogInModule { get; set; }

        public void LoadModules()
        {
            if (LogInModule.LogInUser.Rights == 0)
            {
                MiscellaneousModule.Initialize();
                StationsModule.Initialize();
            }
        }

        public void UnloadModules()
        {
            StationMapsModule = null;
            MeasurementsModule = null;
            MiscellaneousModule = null;
            StationsModule = null;
        }
    }

    public static class ViewModelLocatorStatic
    {
        public static ViewModelLocator Locator = Application.Current.Resources["Locator"] as ViewModelLocator;
    }
}