using System;
using GalaSoft.MvvmLight;

namespace AutomatedWeatherStation.Reports.Measurements
{
    public class MeasurementStation : ObservableObject
    {
        private DateTime _date;
        private decimal? _humidity;
        private int _measurementId;
        private decimal? _pressure;
        private decimal? _rainfall;
        private decimal? _solarIrradianceH;
        private decimal? _solarIrradianceM;
        private string _stationId;
        private decimal? _temperature;
        private decimal? _windDirection;
        private decimal? _windSpeed;

        public int MeasurementId
        {
            get { return _measurementId; }
            set
            {
                _measurementId = value;
                RaisePropertyChanged(nameof(MeasurementId));
            }
        }

        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                RaisePropertyChanged(nameof(Date));
            }
        }

        public string StationLocation
        {
            get { return _stationId; }
            set
            {
                _stationId = value;
                RaisePropertyChanged(nameof(StationLocation));
            }
        }

        public decimal? Temperature
        {
            get { return _temperature; }
            set
            {
                _temperature = value;
                RaisePropertyChanged(nameof(Temperature));
            }
        }

        public decimal? Humidity
        {
            get { return _humidity; }
            set
            {
                _humidity = value;
                RaisePropertyChanged(nameof(Humidity));
            }
        }

        public decimal? Rainfall24H
        {
            get { return _rainfall; }
            set
            {
                _rainfall = value;
                RaisePropertyChanged(nameof(Rainfall24H));
            }
        }

        public decimal? Rainfall15M { get; set; }

        public decimal? Pressure
        {
            get { return _pressure; }
            set
            {
                _pressure = value;
                RaisePropertyChanged(nameof(Pressure));
            }
        }

        public decimal? WindDirection
        {
            get { return _windDirection; }
            set
            {
                _windDirection = value;
                RaisePropertyChanged(WindDirection.ToString());
            }
        }

        public decimal? WindSpeed
        {
            get { return _windSpeed; }
            set
            {
                _windSpeed = value;
                RaisePropertyChanged(nameof(WindSpeed));
            }
        }

        public decimal? SolarIrradianceM
        {
            get { return _solarIrradianceM; }
            set
            {
                _solarIrradianceM = value;
                RaisePropertyChanged(nameof(SolarIrradianceM));
            }
        }

        public decimal? SolarIrradianceH
        {
            get { return _solarIrradianceH; }
            set
            {
                _solarIrradianceH = value;
                RaisePropertyChanged(nameof(SolarIrradianceH));
            }
        }
    }
}