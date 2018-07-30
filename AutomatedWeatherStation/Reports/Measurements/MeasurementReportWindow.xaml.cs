using System.Collections.Generic;
using System.Collections.ObjectModel;
using AutomatedWeatherStation.Helpers.ReportViewer;
using AutomatedWeatherStation.Modules;
using MahApps.Metro.Controls;
using Microsoft.Reporting.WinForms;

namespace AutomatedWeatherStation.Reports.Measurements
{
    /// <summary>
    ///     Interaction logic for MeasurementReportWindow.xaml
    /// </summary>
    public partial class MeasurementReportWindow : MetroWindow
    {
        private readonly ReportViewBuilder _reportView;

        private readonly ReportParameter[] parameters = new ReportParameter[9];
        private bool _isHumidityChecked;
        private bool _isPressureChecked;
        private bool _isRain15Checked;
        private bool _isRain24Checked;
        private bool _isSolarHourChecked;
        private bool _isSolarMinuteChecked;
        private bool _isTemperatureChecked;
        private bool _isWindDirectionChecked;
        private bool _isWindSpeedChecked;


        public MeasurementReportWindow()
        {
            InitializeComponent();

            //IsTemperatureChecked = true;
            //IsPressureChecked = true;
            //IsHumidityChecked = true;
            //IsRain15Checked = true;
            //IsRain24Checked = true;
            //IsSolarMinuteChecked = true;
            //IsSolarHourChecked = true;
            //IsWindSpeedChecked = true;
            //IsWindDirectionChecked = true;

            _reportView = new ReportViewBuilder("AutomatedWeatherStation.Reports.Measurements.MeasurementReport.rdlc",
                UpdateDatasetSource());
            _reportView.RefreshDataSOurceCallback = UpdateDatasetSource;
            ReportContainer.Content = _reportView.ReportContent;

            DataContext = this;

            SetParameters();
        }

        public bool IsTemperatureChecked
        {
            get { return _isTemperatureChecked; }
            set
            {
                _isTemperatureChecked = value;

                parameters[0] = new ReportParameter("TemperatureVisibility", _isTemperatureChecked.ToString());
                _reportView.ReportContent.Viewer.LocalReport.SetParameters(parameters);
                _reportView.ReportContent.Viewer.RefreshReport();
            }
        }

        public bool IsHumidityChecked
        {
            get { return _isHumidityChecked; }
            set
            {
                _isHumidityChecked = value;
                parameters[1] = new ReportParameter("HumidityVisibility", _isHumidityChecked.ToString());
                _reportView.ReportContent.Viewer.LocalReport.SetParameters(parameters);
                _reportView.ReportContent.Viewer.RefreshReport();
            }
        }

        public bool IsPressureChecked
        {
            get { return _isPressureChecked; }
            set
            {
                _isPressureChecked = value;
                parameters[2] = new ReportParameter("PressureVisibility", _isPressureChecked.ToString());
                _reportView.ReportContent.Viewer.LocalReport.SetParameters(parameters);
                _reportView.ReportContent.Viewer.RefreshReport();
            }
        }

        public bool IsRain15Checked
        {
            get { return _isRain15Checked; }
            set
            {
                _isRain15Checked = value;
                parameters[3] = new ReportParameter("Rain15Visibility", _isRain15Checked.ToString());
                _reportView.ReportContent.Viewer.LocalReport.SetParameters(parameters);
                _reportView.ReportContent.Viewer.RefreshReport();
            }
        }

        public bool IsRain24Checked
        {
            get { return _isRain24Checked; }
            set
            {
                _isRain24Checked = value;
                parameters[4] = new ReportParameter("Rain24Visibility", _isRain24Checked.ToString());
                _reportView.ReportContent.Viewer.LocalReport.SetParameters(parameters);
                _reportView.ReportContent.Viewer.RefreshReport();
            }
        }


        public bool IsSolarMinuteChecked
        {
            get { return _isSolarMinuteChecked; }
            set
            {
                _isSolarMinuteChecked = value;
                parameters[5] = new ReportParameter("SolarMinuteVisibility", _isSolarMinuteChecked.ToString());
                _reportView.ReportContent.Viewer.LocalReport.SetParameters(parameters);
                _reportView.ReportContent.Viewer.RefreshReport();
            }
        }

        public bool IsSolarHourChecked
        {
            get { return _isSolarHourChecked; }
            set
            {
                _isSolarHourChecked = value;
                parameters[6] = new ReportParameter("SolarHourVisibility", _isSolarHourChecked.ToString());
                _reportView.ReportContent.Viewer.LocalReport.SetParameters(parameters);
                _reportView.ReportContent.Viewer.RefreshReport();
            }
        }

        public bool IsWindSpeedChecked
        {
            get { return _isWindSpeedChecked; }
            set
            {
                _isWindSpeedChecked = value;
                parameters[7] = new ReportParameter("WindSpeedVisibility", _isWindSpeedChecked.ToString());
                _reportView.ReportContent.Viewer.LocalReport.SetParameters(parameters);
                _reportView.ReportContent.Viewer.RefreshReport();
            }
        }

        public bool IsWindDirectionChecked
        {
            get { return _isWindDirectionChecked; }
            set
            {
                _isWindDirectionChecked = value;
                parameters[8] = new ReportParameter("WindDirectionVisibility", _isWindDirectionChecked.ToString());
                _reportView.ReportContent.Viewer.LocalReport.SetParameters(parameters);
                _reportView.ReportContent.Viewer.RefreshReport();
            }
        }

        private void SetParameters()
        {
            parameters[0] = new ReportParameter("TemperatureVisibility", _isTemperatureChecked.ToString());
            parameters[1] = new ReportParameter("HumidityVisibility", _isHumidityChecked.ToString());
            parameters[2] = new ReportParameter("PressureVisibility", _isPressureChecked.ToString());
            parameters[3] = new ReportParameter("Rain15Visibility", _isRain15Checked.ToString());
            parameters[4] = new ReportParameter("Rain24Visibility", _isRain24Checked.ToString());
            parameters[5] = new ReportParameter("SolarHourVisibility", _isSolarHourChecked.ToString());
            parameters[6] = new ReportParameter("SolarMinuteVisibility", _isSolarMinuteChecked.ToString());
            parameters[7] = new ReportParameter("WindSpeedVisibility", _isWindSpeedChecked.ToString());
            parameters[8] = new ReportParameter("WindDirectionVisibility", _isWindDirectionChecked.ToString());
        }


        private IReadOnlyCollection<DataSetValuePair> UpdateDatasetSource()
        {
            var sources = new List<DataSetValuePair>();

            var allMeasurements = ViewModelLocatorStatic.Locator.MeasurementsModule.PrintMeasurementsList;
            var measurementdataset = new ObservableCollection<MeasurementStation>();
            measurementdataset.Clear();

            foreach (var allMeasurement in allMeasurements)
            {
                measurementdataset.Add(new MeasurementStation
                {
                    MeasurementId = allMeasurement.MeasurementId,
                    Date = allMeasurement.Date,
                    StationLocation = allMeasurement.Station?.Location,
                    Temperature = allMeasurement.Temperature,
                    Humidity = allMeasurement.Humidity,
                    Pressure = allMeasurement.Pressure,
                    Rainfall24H = allMeasurement.Rainfall24H,
                    Rainfall15M = allMeasurement.Rainfall15M,
                    WindDirection = allMeasurement.WindDirection,
                    WindSpeed = allMeasurement.WindSpeed,
                    SolarIrradianceM = allMeasurement.SolarIrradianceM,
                    SolarIrradianceH = allMeasurement.SolarIrradianceH
                });
            }
            //magmatch dapat ang "PublisherDataset" sa publisherReport
            sources.Add(new DataSetValuePair("MeasurementDataset", measurementdataset));

            return sources;
        }
    }
}