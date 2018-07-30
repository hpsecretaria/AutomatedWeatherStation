using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using AutomatedWeatherStation.DataAccess;
using AutomatedWeatherStation.DataAccess.EF;
using AutomatedWeatherStation.Helpers;
using AutomatedWeatherStation.Models.MeasurementModels;
using AutomatedWeatherStation.Models.StationModels;
using AutomatedWeatherStation.Reports.Measurements;
using AutomatedWeatherStation.Views.WeatherHistory;
using AutomatedWeatherStation.Views.WeatherHistory.DailyCharts;
using AutomatedWeatherStation.Views.WeatherHistory.MonthlyCharts;
using AutomatedWeatherStation.Views.WeatherHistory.YearlyCharts;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using LiveCharts;
using LiveCharts.Wpf;
using Nito.AsyncEx;

namespace AutomatedWeatherStation.Modules
{
    public class MeasurementsModule : ObservableObject
    {
        private readonly SingleInstanceWindowViewer<MeasurementReportWindow> _measurementsWindow =
            new SingleInstanceWindowViewer<MeasurementReportWindow>();

        private readonly IRepository _repository;
        private readonly List<decimal> dailyHumValues = new List<decimal>();
        private readonly List<decimal> dailyPressureValues = new List<decimal>();
        private readonly List<decimal> dailyRain15Values = new List<decimal>();
        private readonly List<decimal> dailyRain24Values = new List<decimal>();
        private readonly List<decimal> dailysolarHourValues = new List<decimal>();
        private readonly List<decimal> dailysolarMinuteValues = new List<decimal>();
        private readonly List<decimal> dailyTempValues = new List<decimal>();
        private readonly List<decimal> dailywindspeedValues = new List<decimal>();


        //<-----monthly charts------>
        private readonly List<string> dayValues = new List<string>();
        private readonly List<decimal> monthlyHumValues = new List<decimal>();
        private readonly List<decimal> monthlyPressureValues = new List<decimal>();
        private readonly List<decimal> monthlyRain15Values = new List<decimal>();
        private readonly List<decimal> monthlyRain24Values = new List<decimal>();
        private readonly List<decimal> monthlysolarHourValues = new List<decimal>();
        private readonly List<decimal> monthlysolarMinuteValues = new List<decimal>();
        private readonly List<decimal> monthlyTempValues = new List<decimal>();
        private readonly List<decimal> monthlywindspeedValues = new List<decimal>();


        //<-----yearly charts-------->
        private readonly List<string> monthValues = new List<string>();

        //<-----daily charts----->
        private readonly List<string> timeValues = new List<string>();
        private readonly List<decimal> yearlyHumValues = new List<decimal>();
        private readonly List<decimal> yearlyPressureValues = new List<decimal>();
        private readonly List<decimal> yearlyRain15Values = new List<decimal>();
        private readonly List<decimal> yearlyRain24Values = new List<decimal>();
        private readonly List<decimal> yearlysolarHourValues = new List<decimal>();
        private readonly List<decimal> yearlysolarMinuteValues = new List<decimal>();
        private readonly List<decimal> yearlyTempValues = new List<decimal>();
        private readonly List<decimal> yearlywindspeedValues = new List<decimal>();
        private bool _allRecordsGenerated;
        private decimal _averageHumidity;

        private decimal _averageMonthlyHumidity;
        private decimal _averageMonthlyPressure;
        private decimal _averageMonthlyRain15;
        private decimal _averageMonthlyRain24;
        private decimal _averageMonthlyRainfall;
        private decimal _averageMonthlySolarHour;
        private decimal _averageMonthlySolarMinute;

        private decimal _averageMonthlyTemperature
            ;

        private decimal _averageMonthlyWindSpeed;
        private decimal _averagePressure;


        private decimal _averageRain15;
        private decimal _averageRain24;
        private decimal _averageRainfall;
        private decimal _averageSolarHour;
        private decimal _averageSolarMinute;

        private decimal _averageTemperature;
        private decimal _averageWindSpeed;
        private bool _isDailyGenerated;
        private bool _isMonthlyGenerated;
        private bool _isYearlyGenerated;

        private DateTime _selectedDateTime;
        private DateTime _selectedEndDateTime;
        private string _selectedMonth;
        private string _selectedMonth1;
        private DateTime _selectedStartDateTime;


        private StationModel _selectedStation;
        private int _selectedTabIndex;

        private string _selectedYear;
        private string _selectedYear1;


        //<------Radio button selection------->
        private int _sortDateOption = 1;
        private int _sortStationOption = 1;
        private HumidityChartWindow _viewHumidityChartWindow;
        private MonthlyHumidityWindow _viewMonthlyHumWindow;
        private MonthlyPressureWindow _viewMonthlyPressureWindow;
        private MonthlyRain15Window _viewMonthlyRain15Window;
        private MonthlyRain24Window _viewMonthlyRain24Window;
        private MonthlySolarHourWindow _viewMonthlySolarHourWindow;
        private MonthlySolarMinutewWindow _viewMonthlySolarMinutewWindow;
        private MonthlyTempWindow _viewMonthlyTempWindow;
        private MonthlyWindSpeedWindow _viewMonthlyWindSpeedWindow;
        private PressureChartWindow _viewPressureChartWindow;
        private Rain15ChartWindow _viewRain15ChartWindow;
        private Rain24HChartWindow _viewRain24ChartWindow;
        private SolarHourChartWindow _viewSolarHourChartWindow;
        private SolarMinuteChartWindow _viewSolarMinuteChartWindow;
        private TemperatureChartWindow _viewTemperatureChart;
        private WindSpeedChartWindow _viewWindChartWindow;
        private YearlyHumidityWindow _viewYearlyHumWindow;
        private YearlyPressureWindow _viewYearlyPressureWindow;
        private YearlyRain15Window _viewYearlyRain15Window;
        private YearlyRain24Window _viewYearlyRain24Window;
        private YearlySolarHourWindow _viewYearlySolarHourWindow;
        private YearlySolarMinuteWindow _viewYearlySolarMinutewWindow;
        private YearlyTempWindow _viewYearlyTempWindow;
        private YearlyWindSpeedWindow _viewYearlyWindSpeedWindow;

        public string[] stationStrings = new string[100];

        public MeasurementsModule(IRepository repository)
        {
            _repository = repository;
            StationLoading = NotifyTaskCompletion.Create(LoadStationsAsync());
            InitializeLoading = NotifyTaskCompletion.Create(InitializeAsync());
        }

        public INotifyTaskCompletion InitializeLoading { get; set; }

        public INotifyTaskCompletion StationLoading { get; set; }

        public List<string> AllMonths { get; private set; }

        //<-----------LISTS------------>

        public ObservableCollection<MeasurementModel> MeasurementsList { get; } =
            new ObservableCollection<MeasurementModel>();

        public ObservableCollection<StationModel> StationsList { get; } = new ObservableCollection<StationModel>();

        public ObservableCollection<Measurement> PrintMeasurementsList { get; } =
            new ObservableCollection<Measurement>();

        public ObservableCollection<MeasurementModel> ResultssList { get; } =
            new ObservableCollection<MeasurementModel>();

        public ObservableCollection<MeasurementModel> PerMonthList { get; } =
            new ObservableCollection<MeasurementModel>(); //list of measurements per month


        //<---------ASYNC LOADING------------------->
        public INotifyTaskCompletion MeasurementLoading { get; private set; }

        //<-------ALL RECORDS, DATE RANGE SELECTION-------->

        public DateTime SelectedStartDateTime
        {
            get { return _selectedStartDateTime; }
            set
            {
                _selectedStartDateTime = value;
                LoadMeasurements();
                RaisePropertyChanged(nameof(SelectedStartDateTime));
            }
        }

        public DateTime SelectedEndDateTime
        {
            get { return _selectedEndDateTime; }
            set
            {
                _selectedEndDateTime = value;
                LoadMeasurements();
                RaisePropertyChanged(nameof(SelectedEndDateTime));
            }
        }

        public ICommand SortMeasurementCommand => new RelayCommand(SortMeasurementProc);

        //For booleanvisibility
        public bool AllRecordsGenerated
        {
            get { return _allRecordsGenerated; }
            set
            {
                _allRecordsGenerated = value;
                RaisePropertyChanged(nameof(AllRecordsGenerated));
            }
        }

        public bool IsDailyGenerated
        {
            get { return _isDailyGenerated; }
            set
            {
                _isDailyGenerated = value;
                RaisePropertyChanged(nameof(IsDailyGenerated));
            }
        }

        public bool IsMonthlyGenerated
        {
            get { return _isMonthlyGenerated; }
            set
            {
                _isMonthlyGenerated = value;
                RaisePropertyChanged(nameof(IsMonthlyGenerated));
            }
        }

        public bool IsYearlyGenerated
        {
            get { return _isYearlyGenerated; }
            set
            {
                _isYearlyGenerated = value;
                RaisePropertyChanged(nameof(IsYearlyGenerated));
            }
        }

        public int SortDateOption
        {
            get { return _sortDateOption; }
            set
            {
                if (_sortDateOption == value) return;
                _sortDateOption = value;
                LoadMeasurements();
                RaisePropertyChanged(nameof(SortDateOption));
            }
        }

        public int SortStationOption
        {
            get { return _sortStationOption; }
            set
            {
                if (_sortStationOption == value) return;
                _sortStationOption = value;
                LoadMeasurements();

                RaisePropertyChanged(nameof(SortStationOption));
            }
        }

        /// <summary>
        ///     MONTHLY AVERAGE
        /// </summary>
        public decimal AverageTemperature
        {
            get { return _averageTemperature; }
            set
            {
                _averageTemperature = value;
                RaisePropertyChanged(nameof(AverageTemperature));
            }
        }

        public decimal AverageHumidity
        {
            get { return _averageHumidity; }
            set
            {
                _averageHumidity = value;
                RaisePropertyChanged(nameof(AverageHumidity));
            }
        }

        public decimal AverageRainfall
        {
            get { return _averageRainfall; }
            set
            {
                _averageRainfall = value;
                RaisePropertyChanged(nameof(AverageRainfall));
            }
        }

        public decimal AverageRain15
        {
            get { return _averageRain15; }
            set
            {
                _averageRain15 = value;
                RaisePropertyChanged(nameof(AverageRain15));
            }
        }

        public decimal AverageRain24
        {
            get { return _averageRain24; }
            set
            {
                _averageRain24 = value;
                RaisePropertyChanged(nameof(AverageRain24));
            }
        }

        public decimal AverageWindSpeed
        {
            get { return _averageWindSpeed; }
            set
            {
                _averageWindSpeed = value;
                RaisePropertyChanged(nameof(AverageWindSpeed));
            }
        }

        public decimal AverageSolarMinute
        {
            get { return _averageSolarMinute; }
            set
            {
                _averageSolarMinute = value;
                RaisePropertyChanged(nameof(AverageSolarMinute));
            }
        }

        public decimal AverageSolarHour
        {
            get { return _averageSolarHour; }
            set
            {
                _averageSolarHour = value;
                RaisePropertyChanged(nameof(AverageSolarHour));
            }
        }

        public decimal AveragePressure
        {
            get { return _averagePressure; }
            set
            {
                _averagePressure = value;
                RaisePropertyChanged(nameof(AveragePressure));
            }
        }


        /// <summary>
        ///     Yearly Average
        /// </summary>
        public decimal AveragMonthlyTemperature
        {
            get { return _averageMonthlyTemperature; }
            set
            {
                _averageMonthlyTemperature = value;
                RaisePropertyChanged(nameof(AverageTemperature));
            }
        }

        public decimal AverageMonthlyHumidity
        {
            get { return _averageMonthlyHumidity; }
            set
            {
                _averageMonthlyHumidity = value;
                RaisePropertyChanged(nameof(AverageMonthlyHumidity));
            }
        }

        public decimal AverageMonthlyRain15
        {
            get { return _averageMonthlyRain15; }
            set
            {
                _averageMonthlyRain15 = value;
                RaisePropertyChanged(nameof(AverageMonthlyRain15));
            }
        }

        public decimal AverageMonthlyRain24
        {
            get { return _averageMonthlyRain24; }
            set
            {
                _averageMonthlyRain24 = value;
                RaisePropertyChanged(nameof(AverageMonthlyRain24));
            }
        }

        public decimal AverageMonthlyWindSpeed
        {
            get { return _averageMonthlyWindSpeed; }
            set
            {
                _averageMonthlyWindSpeed = value;
                RaisePropertyChanged(nameof(AverageMonthlyWindSpeed));
            }
        }

        public decimal AverageMonthlySolarMinute
        {
            get { return _averageMonthlySolarMinute; }
            set
            {
                _averageMonthlySolarMinute = value;
                RaisePropertyChanged(nameof(AverageMonthlySolarMinute));
            }
        }

        public decimal AverageMonthlySolarHour
        {
            get { return _averageMonthlySolarHour; }
            set
            {
                _averageMonthlySolarHour = value;
                RaisePropertyChanged(nameof(AverageMonthlySolarHour));
            }
        }

        public decimal AverageMonthlyPressure
        {
            get { return _averageMonthlyPressure; }
            set
            {
                _averageMonthlyPressure = value;
                RaisePropertyChanged(nameof(AverageMonthlyPressure));
            }
        }

        //----Sort Daily records------>

        public ICommand SortMeasurementDailyCommand => new RelayCommand(SortMeasurementDailyProc);

        public DateTime SelectedDateTime
        {
            get { return _selectedDateTime; }
            set
            {
                _selectedDateTime = value;
                UpdateIndex();
                RaisePropertyChanged(nameof(SelectedDateTime));
            }
        }


        //<--------sort monthly records------>
        public ICommand SortMeasurementMonthlyCommand => new RelayCommand(SortMeasurementMonthlyProc);

        public string SelectedYear
        {
            get { return _selectedYear1; }
            set
            {
                _selectedYear1 = value;
                UpdateIndex();
                RaisePropertyChanged(nameof(SelectedYear));
            }
        }

        public string SelectedMonth
        {
            get { return Convert.ToString(_selectedMonth1); }
            set
            {
                _selectedMonth1 = value;
                UpdateIndex();
                RaisePropertyChanged(nameof(SelectedMonth));
            }
        }

        //<--------sort yearly records------>
        public ICommand SortMeasurementYearlyCommand => new RelayCommand(SortMeasurementYearlyProc);


        //<-----Printing all records----->
        public ICommand PrintMeasurementCommand => new RelayCommand(PrintMeasurementProc);

        public StationModel SelectedStation
        {
            get { return _selectedStation; }
            set
            {
                _selectedStation = value;
                UpdateIndex();

                RaisePropertyChanged(nameof(SelectedStation));
            }
        }

        //<------Daily temperature chart-------->
        public ICommand ViewTemperatureChart => new RelayCommand(ViewTemperatureChartProc);


        public ICommand ViewDailyHumidityChart => new RelayCommand(ViewDailyHumidChartProc);


        public ICommand ViewDailyPressureChart => new RelayCommand(ViewDailyPressureChartProc);
        public ICommand ViewDailyRain15Chart => new RelayCommand(ViewDailyRain15ChartProc);

        public ICommand ViewDailyRain24Chart => new RelayCommand(ViewDailyRain24ChartProc);

        public ICommand ViewDailyWindChart => new RelayCommand(ViewDailyWindChartProc);

        public ICommand ViewDailySolarHourChart => new RelayCommand(ViewDailySolarHourChartProc);

        public ICommand ViewDailySolarMinuteChart => new RelayCommand(ViewDailySolarMinuteChartProc);


        //<---------Monthly Charts---------->
        public ICommand ViewMonthlyTemperatureChart => new RelayCommand(ViewMonthlyTempChart);


        public ICommand ViewMonthlyHumidityChart => new RelayCommand(ViewMonthlyHumidChartProc);


        public ICommand ViewMonthlyPressureChart => new RelayCommand(ViewMonthlyPressureChartProc);
        public ICommand ViewMonthlyRain15Chart => new RelayCommand(ViewMonthlyRain15ChartProc);

        public ICommand ViewMonthlyRain24Chart => new RelayCommand(ViewMonthlyRain24ChartProc);

        public ICommand ViewMonthlyWindChart => new RelayCommand(ViewMonthlyWindChartProc);

        public ICommand ViewMonthlySolarHourChart => new RelayCommand(ViewMonthlySolarHourChartProc);

        public ICommand ViewMonthlySolarMinuteChart => new RelayCommand(ViewMonthlySolarMinuteChartProc);

        //<----------------View Yearly Charts------------------------->
        public ICommand ViewYearlyTemperatureChart => new RelayCommand(ViewYearlyTempChart);


        public ICommand ViewYearlyHumidityChart => new RelayCommand(ViewYearlyHumidChartProc);


        public ICommand ViewYearlyPressureChart => new RelayCommand(ViewYearlyPressureChartProc);
        public ICommand ViewYearlyRain15Chart => new RelayCommand(ViewYearlyRain15ChartProc);

        public ICommand ViewYearlyRain24Chart => new RelayCommand(ViewYearlyRain24ChartProc);

        public ICommand ViewYearlyWindChart => new RelayCommand(ViewYearlyWindChartProc);

        public ICommand ViewYearlySolarHourChart => new RelayCommand(ViewYearlySolarHourChartProc);

        public ICommand ViewYearlySolarMinuteChart => new RelayCommand(ViewYearlySolarMinuteChartProc);


        public string[] Labels { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                _selectedTabIndex = value;
                UpdateIndex();
                RaisePropertyChanged(nameof(SelectedTabIndex));
            }
        }

        private async Task InitializeAsync()
        {
            AllRecordsGenerated = false;
            IsDailyGenerated = false;
            IsMonthlyGenerated = false;
            IsYearlyGenerated = false;
            try
            {
                AllMonths = new List<string>
                {
                    "January",
                    "February",
                    "March",
                    "April",
                    "May",
                    "June",
                    "July",
                    "August",
                    "Septmeber",
                    "October",
                    "November",
                    "December"
                };
                SelectedMonth = AllMonths.FirstOrDefault();
                SelectedYear = DateTime.Now.Year.ToString();


                //For initialization

                SelectedStartDateTime = DateTime.Today.AddMonths(-1);
                SelectedEndDateTime = DateTime.Now;
                SelectedDateTime = DateTime.Now;
                //_tempIsSelected = true;
                //_humIsSelected = true;
                //_pressureIsSelected = true;
                //_rainfallIsSelected = true;
            }
            catch (NullReferenceException)
            {
            }
        }

        private void LoadMeasurement()
        {
            MeasurementLoading = NotifyTaskCompletion.Create(LoadMeasurementsAsync);
        }

        private async Task LoadMeasurementsAsync()
        {
            MeasurementsList.Clear();
            PrintMeasurementsList.Clear();

            var index = 0;
            var measurements = await _repository.Measurements.GetRangeAsync();
            AllRecordsGenerated = true;
            foreach (var measurement in measurements)
            {
                var item = measurements[index];
                var stations = _repository.Stations.Get(c => c.StationId == measurement.StationId);
                measurement.Station = stations;
                MeasurementsList.Add(new MeasurementModel(measurement, _repository));
                await Task.Delay(10);

                PrintMeasurementsList.Add(item);

                index++;
            }


            var view = CollectionViewSource.GetDefaultView(MeasurementsList) as ListCollectionView;
            view?.SortDescriptions.Add(new SortDescription("Model.Date", ListSortDirection.Descending));
        }

        private async Task LoadMeasurements()
        {
            PrintMeasurementsList.Clear();
            MeasurementsList.Clear();
            //decimal tempValue = 0;
            //decimal humValue = 0;
            //decimal rainValue = 0;
            //decimal pressValue = 0;

            if (SortStationOption == 1)
            {
                if (SortDateOption == 1)
                {
                    MeasurementLoading = NotifyTaskCompletion.Create(LoadMeasurementsAsync);
                    LoadAllStations();
                }
                else
                {
                    var tmp = 0; //for stationString array

                    //var query = await _repository.Measurements.GetRangeAsync(c => c.Date >= SelectedStartDateTime && c.Date <= SelectedEndDateTime);
                    var q = await Task.Run(() => _repository.Measurements.GetRangeAsync());
                    var query = q.Where(c => c.Date >= SelectedStartDateTime && c.Date <= SelectedEndDateTime);

                    var measurements = query as IList<Measurement> ?? query.ToList();

                    AllRecordsGenerated = true;
                    foreach (var measurement in measurements)
                    {
                        var item = measurements[tmp];
                        var stations =
                            await
                                Task.Run(() => _repository.Stations.GetAsync(c => c.StationId == measurement.StationId));
                        measurement.Station = stations;
                        MeasurementsList.Add(new MeasurementModel(measurement, _repository));
                        tmp++;

                        PrintMeasurementsList.Add(item);
                    }
                }
            }
            else
            {
                if (SortDateOption == 1)
                {
                    var tmp = 0; //for stationString array
                    //var query = await _repository.Measurements.GetRangeAsync(c => c.StationId == SelectedStation.Model.StationId);
                    var q = await Task.Run(() => _repository.Measurements.GetRangeAsync());
                    var query = q.Where(c => c.StationId == SelectedStation.Model.StationId);

                    var measurements = query as IList<Measurement> ?? query.ToList();

                    AllRecordsGenerated = true;

                    foreach (var measurement in measurements)
                    {
                        var stations =
                            await
                                Task.Run(() => _repository.Stations.GetAsync(c => c.StationId == measurement.StationId));
                        measurement.Station = stations;
                        var item = measurements[tmp];

                        MeasurementsList.Add(new MeasurementModel(measurement, _repository));

                        stationStrings[tmp] = measurement.Station.ToString();
                        tmp++;
                        PrintMeasurementsList.Add(item);
                    }
                }
                else
                {
                    // var query = await _repository.Measurements.GetRangeAsync(c => c.StationId == SelectedStation.Model.StationId && c.Date >= SelectedStartDateTime && c.Date <= SelectedEndDateTime);
                    var tmp = 0;
                    var q = await Task.Run(() => _repository.Measurements.GetRangeAsync());
                    var query = q.Where(
                        c =>
                            c.StationId == SelectedStation.Model.StationId && c.Date >= SelectedStartDateTime &&
                            c.Date <= SelectedEndDateTime);

                    var measurements = query as IList<Measurement> ?? query.ToList();
                    AllRecordsGenerated = true;

                    foreach (var measurement in measurements)
                    {
                        var stations =
                            await
                                Task.Run(() => _repository.Stations.GetAsync(c => c.StationId == measurement.StationId));
                        measurement.Station = stations;
                        MeasurementsList.Add(new MeasurementModel(measurement, _repository));
                        var item = measurements[tmp];

                        tmp++;
                        PrintMeasurementsList.Add(item);
                    }
                }
            }
        }

        private async void SortMeasurementProc()
        {
            await LoadMeasurements();
        }

        private async void SortMeasurementDailyProc()
        {
            try
            {
                PrintMeasurementsList.Clear();
                MeasurementsList.Clear();
                timeValues.Clear();
                dailyTempValues.Clear();
                dailyHumValues.Clear();
                dailyPressureValues.Clear();
                dailyRain15Values.Clear();
                dailyRain24Values.Clear();
                dailywindspeedValues.Clear();
                dailysolarHourValues.Clear();
                dailysolarMinuteValues.Clear();

                var q = await Task.Run(() => _repository.Measurements.GetRangeAsync());
                var query = q.Where(
                    c =>
                        c.StationId == SelectedStation.Model.StationId && c.Date.Day == SelectedDateTime.Day &&
                        c.Date.Month == SelectedDateTime.Month && c.Date.Year == SelectedDateTime.Year);

                var measurements = query as IList<Measurement> ?? query.ToList();

                IsDailyGenerated = true;
                var tmp = 0;
                foreach (var measurement in measurements)
                {
                    var item = measurements[tmp];
                    var stations =
                        await Task.Run(() => _repository.Stations.Get(c => c.StationId == measurement.StationId));
                    measurement.Station = stations;
                    MeasurementsList.Add(new MeasurementModel(measurement, _repository));
                    PrintMeasurementsList.Add(item);

                    timeValues.Add(measurement.Date.ToShortTimeString());

                    if (measurement.Temperature != null) dailyTempValues.Add((decimal) measurement.Temperature);
                    if (measurement.Humidity != null) dailyHumValues.Add((decimal) measurement.Humidity);
                    if (measurement.Pressure != null)
                    {
                        dailyPressureValues.Add((decimal) measurement.Pressure);
                    }

                    if (measurement.Rainfall15M != null) dailyRain15Values.Add((decimal) measurement.Rainfall15M);
                    if (measurement.Rainfall24H != null) dailyRain24Values.Add((decimal) measurement.Rainfall24H);
                    if (measurement.WindSpeed != null) dailywindspeedValues.Add((decimal) measurement.WindSpeed);
                    if (measurement.SolarIrradianceH != null)
                        dailysolarHourValues.Add((decimal) measurement.SolarIrradianceH);
                    if (measurement.SolarIrradianceM != null)
                        dailysolarMinuteValues.Add((decimal) measurement.SolarIrradianceM);


                    tmp++;
                }
            }
            catch (NullReferenceException)
            {
            }
        }

        private async void SortMeasurementMonthlyProc()
        {
            PrintMeasurementsList.Clear();
            MeasurementsList.Clear();
            ResultssList.Clear();
            dayValues.Clear();
            monthlyTempValues.Clear();
            monthlyHumValues.Clear();
            monthlyPressureValues.Clear();
            monthlyRain15Values.Clear();
            monthlyRain24Values.Clear();
            monthlywindspeedValues.Clear();
            monthlysolarHourValues.Clear();
            monthlysolarMinuteValues.Clear();

            decimal tempValue = 0;
            decimal humValue = 0;
            decimal pressValue = 0;
            decimal rain15Value = 0;
            decimal rain24Value = 0;
            decimal windspeedValue = 0;
            decimal solarhourValue = 0;
            decimal solarminuteValue = 0;


            var monthSelected = AllMonths.IndexOf(SelectedMonth) + 1;

            var q = await Task.Run(() => _repository.Measurements.GetRangeAsync());
            var query = q.Where(
                c =>
                    c.StationId == SelectedStation.Model.StationId && c.Date.Month == monthSelected &&
                    c.Date.Year == int.Parse(SelectedYear));

            var measurements = query as IList<Measurement> ?? query.ToList();


            var tmp = 0;

            IsMonthlyGenerated = true;
            foreach (var measurement in measurements)
            {
                var item = measurements[tmp];
                var stations = await Task.Run(() => _repository.Stations.Get(c => c.StationId == measurement.StationId));
                measurement.Station = stations;
                MeasurementsList.Add(new MeasurementModel(measurement, _repository));
                PrintMeasurementsList.Add(item);


                tmp++;
            }

            try
            {
                var daymonth = 1;
                foreach (var day in AllDatesInMonth(Convert.ToInt32(SelectedYear), monthSelected))
                {
                    var r = await Task.Run(() => _repository.Measurements.GetRangeAsync());
                    var results = r.Where(
                        c =>
                            c.StationId == SelectedStation.Model.StationId && c.Date.Day == daymonth &&
                            c.Date.Month == monthSelected && c.Date.Year == int.Parse(SelectedYear));

                    foreach (var result in results)
                    {
                        var temp = result.Temperature;
                        if (temp != null) tempValue += temp.Value;

                        var hum = result.Humidity;
                        if (hum != null) humValue += hum.Value;

                        var press = result.Pressure;
                        if (press != null) pressValue += press.Value;

                        var rain15 = result.Rainfall15M;
                        if (rain15 != null) rain15Value += rain15.Value;

                        var rain24 = result.Rainfall24H;
                        if (rain24 != null) rain24Value += rain24.Value;

                        var windspeed = result.WindSpeed;
                        if (windspeed != null) windspeedValue += windspeed.Value;

                        var solarhour = result.SolarIrradianceH;
                        if (solarhour != null) solarhourValue += solarhour.Value;

                        var solarminute = result.SolarIrradianceM;
                        if (solarminute != null) solarminuteValue += solarminute.Value;

                        ResultssList.Add(new MeasurementModel(result, _repository));
                    }

                    dayValues.Add("Day " + daymonth);
                    AverageTemperature = tempValue/ResultssList.Count;
                    AverageHumidity = humValue/ResultssList.Count;
                    AveragePressure = pressValue/ResultssList.Count;
                    AverageRain15 = rain15Value/ResultssList.Count;
                    AverageRain24 = rain24Value/ResultssList.Count;
                    AverageWindSpeed = windspeedValue/ResultssList.Count;
                    AverageSolarHour = solarhourValue/ResultssList.Count;
                    AverageSolarMinute = solarminuteValue/ResultssList.Count;


                    monthlyTempValues.Add(AverageTemperature);
                    monthlyHumValues.Add(AverageHumidity);
                    monthlyPressureValues.Add(AveragePressure);
                    monthlyRain15Values.Add(AverageRain15);
                    monthlyRain24Values.Add(AverageRain24);
                    monthlywindspeedValues.Add(AverageWindSpeed);
                    monthlysolarHourValues.Add(AverageSolarHour);
                    monthlysolarMinuteValues.Add(AverageSolarMinute);
                    daymonth++;
                }
            }
            catch (DivideByZeroException)
            {
            }
        }

        public static IEnumerable<int> AllDatesInMonth(int year, int month)
        {
            var days = DateTime.DaysInMonth(year, month);
            for (var day = 1; day <= days; day++)
            {
                // yield return new DateTime(year, month, day);
                yield return new int();
            }
        }

        private async void UpdateIndex()
        {
            if (SelectedTabIndex == 0)
            {
                await LoadMeasurements();
            }
            if (SelectedTabIndex == 1)
            {
                SortMeasurementDailyProc();
            }
            if (SelectedTabIndex == 2)
            {
                SortMeasurementMonthlyProc();
            }
            if (SelectedTabIndex == 3)
            {
                SortMeasurementYearlyProc();
            }
        }

        private async void SortMeasurementYearlyProc()
        {
            PrintMeasurementsList.Clear();
            MeasurementsList.Clear();

            monthValues.Clear();
            yearlyTempValues.Clear();
            yearlyHumValues.Clear();
            yearlyPressureValues.Clear();
            yearlyRain15Values.Clear();
            yearlyRain24Values.Clear();
            yearlywindspeedValues.Clear();
            yearlysolarHourValues.Clear();
            yearlysolarMinuteValues.Clear();

            decimal tempValue = 0;
            decimal humValue = 0;
            decimal pressValue = 0;
            decimal rain15Value = 0;
            decimal rain24Value = 0;
            decimal windspeedValue = 0;
            decimal solarhourValue = 0;
            decimal solarminuteValue = 0;

            var q = await Task.Run(() => _repository.Measurements.GetRangeAsync());

            var query =
                q
                    .Where(c => c.StationId == SelectedStation.Model.StationId && c.Date.Year == int.Parse(SelectedYear));
            var measurements = query as IList<Measurement> ?? query.ToList();

            var tmp = 0;

            // int monthSelected = AllMonths.IndexOf(tmp) + 1;

            IsYearlyGenerated = true;

            foreach (var measurement in measurements)
            {
                var item = measurements[tmp];
                var stations =
                    await Task.Run(() => _repository.Stations.GetAsync(c => c.StationId == measurement.StationId));
                measurement.Station = stations;
                MeasurementsList.Add(new MeasurementModel(measurement, _repository));
                PrintMeasurementsList.Add(item);
                //  monthValues.Add(measurement.Date.Day.ToString());
                tmp++;
            }
            try
            {
                var daymonth = 1;
                foreach (var month in AllMonths)
                {
                    ResultssList.Clear();
                    var monthofyear = AllMonths.IndexOf(month) + 1;
                    var r = await Task.Run(() => _repository.Measurements.GetRangeAsync());
                    var results =
                        r
                            .Where(
                                c =>
                                    c.StationId == SelectedStation.Model.StationId && c.Date.Month == monthofyear &&
                                    c.Date.Year == int.Parse(SelectedYear));
                    foreach (var result in results)
                    {
                        var temp = result.Temperature;
                        if (temp != null) tempValue += temp.Value;

                        var hum = result.Humidity;
                        if (hum != null) humValue += hum.Value;

                        var press = result.Pressure;
                        if (press != null) pressValue += press.Value;

                        var rain15 = result.Rainfall15M;
                        if (rain15 != null) rain15Value += rain15.Value;

                        var rain24 = result.Rainfall24H;
                        if (rain24 != null) rain24Value += rain24.Value;

                        var windspeed = result.WindSpeed;
                        if (windspeed != null) windspeedValue += windspeed.Value;

                        var solarhour = result.SolarIrradianceH;
                        if (solarhour != null) solarhourValue += solarhour.Value;

                        var solarminute = result.SolarIrradianceM;
                        if (solarminute != null) solarminuteValue += solarminute.Value;

                        ResultssList.Add(new MeasurementModel(result, _repository));
                    }

                    //monthValues.Add(daymonth.ToString());
                    if (ResultssList.Count > 0)
                    {
                        AverageTemperature = tempValue/ResultssList.Count;
                        AverageHumidity = humValue/ResultssList.Count;
                        AveragePressure = pressValue/ResultssList.Count;
                        AverageRain15 = rain15Value/ResultssList.Count;
                        AverageRain24 = rain24Value/ResultssList.Count;
                        AverageWindSpeed = windspeedValue/ResultssList.Count;
                        AverageSolarHour = solarhourValue/ResultssList.Count;
                        AverageSolarMinute = solarminuteValue/ResultssList.Count;


                        yearlyTempValues.Add(AverageTemperature);
                        yearlyHumValues.Add(AverageHumidity);
                        yearlyPressureValues.Add(AveragePressure);
                        yearlyRain15Values.Add(AverageRain15);
                        yearlyRain24Values.Add(AverageRain24);
                        yearlywindspeedValues.Add(AverageWindSpeed);
                        yearlysolarHourValues.Add(AverageSolarHour);
                        yearlysolarMinuteValues.Add(AverageSolarMinute);
                    }
                    else
                    {
                        AverageTemperature = 0;
                        AverageHumidity = 0;
                        AveragePressure = 0;
                        AverageRain15 = 0;
                        AverageRain24 = 0;
                        AverageWindSpeed = 0;
                        AverageSolarHour = 0;
                        AverageSolarMinute = 0;


                        yearlyTempValues.Add(AverageTemperature);
                        yearlyHumValues.Add(AverageHumidity);
                        yearlyPressureValues.Add(AveragePressure);
                        yearlyRain15Values.Add(AverageRain15);
                        yearlyRain24Values.Add(AverageRain24);
                        yearlywindspeedValues.Add(AverageWindSpeed);
                        yearlysolarHourValues.Add(AverageSolarHour);
                        yearlysolarMinuteValues.Add(AverageSolarMinute);
                    }

                    daymonth++;
                    monthValues.Add(month);
                }
            }
            catch (DivideByZeroException)
            {
            }
        }

        private void PrintMeasurementProc()
        {
            _measurementsWindow.Show();
        }

        //private string _months;
        //private bool _tempIsSelected;
        //private bool _humIsSelected;
        //private bool _rainfallIsSelected;
        //private bool _pressureIsSelected;


        private void ViewTemperatureChartProc()
        {
            Labels = timeValues.ToArray();
            YFormatter = value => value.ToString();


            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = SelectedStation.Model.Location,
                    Values = new ChartValues<decimal>(dailyTempValues)
                }
            };

            _viewTemperatureChart = new TemperatureChartWindow();
            _viewTemperatureChart.Owner = Application.Current.MainWindow;
            _viewTemperatureChart.Show();
        }

        private void ViewDailyHumidChartProc()
        {
            Labels = timeValues.ToArray();
            YFormatter = value => value.ToString();


            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = SelectedStation.Model.Location,
                    Values = new ChartValues<decimal>(dailyHumValues)
                }
            };

            _viewHumidityChartWindow = new HumidityChartWindow();
            _viewHumidityChartWindow.Owner = Application.Current.MainWindow;
            _viewHumidityChartWindow.Show();
        }

        private void ViewDailyPressureChartProc()
        {
            Labels = timeValues.ToArray();
            YFormatter = value => value.ToString();


            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = SelectedStation.Model.Location,
                    Values = new ChartValues<decimal>(dailyPressureValues)
                }
            };

            _viewPressureChartWindow = new PressureChartWindow();
            _viewPressureChartWindow.Owner = Application.Current.MainWindow;
            _viewPressureChartWindow.Show();
        }

        private void ViewDailyRain15ChartProc()
        {
            Labels = timeValues.ToArray();
            YFormatter = value => value.ToString();


            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = SelectedStation.Model.Location,
                    Values = new ChartValues<decimal>(dailyRain15Values)
                }
            };

            _viewRain15ChartWindow = new Rain15ChartWindow();
            _viewRain15ChartWindow.Owner = Application.Current.MainWindow;
            _viewRain15ChartWindow.Show();
        }

        private void ViewDailyRain24ChartProc()
        {
            Labels = timeValues.ToArray();
            YFormatter = value => value.ToString();

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = SelectedStation.Model.Location,
                    Values = new ChartValues<decimal>(dailyRain24Values)
                }
            };

            _viewRain24ChartWindow = new Rain24HChartWindow();
            _viewRain24ChartWindow.Owner = Application.Current.MainWindow;
            _viewRain24ChartWindow.Show();
        }

        private void ViewDailyWindChartProc()
        {
            Labels = timeValues.ToArray();
            YFormatter = value => value.ToString();


            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = SelectedStation.Model.Location,
                    Values = new ChartValues<decimal>(dailywindspeedValues)
                }
            };

            _viewWindChartWindow = new WindSpeedChartWindow();
            _viewWindChartWindow.Owner = Application.Current.MainWindow;
            _viewWindChartWindow.Show();
        }

        private void ViewDailySolarHourChartProc()
        {
            Labels = timeValues.ToArray();
            YFormatter = value => value.ToString();


            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = SelectedStation.Model.Location,
                    Values = new ChartValues<decimal>(dailysolarHourValues)
                }
            };

            _viewSolarHourChartWindow = new SolarHourChartWindow();
            _viewSolarHourChartWindow.Owner = Application.Current.MainWindow;
            _viewSolarHourChartWindow.Show();
        }

        private void ViewDailySolarMinuteChartProc()
        {
            Labels = timeValues.ToArray();
            YFormatter = value => value.ToString();


            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = SelectedStation.Model.Location,
                    Values = new ChartValues<decimal>(dailysolarMinuteValues)
                }
            };

            _viewSolarMinuteChartWindow = new SolarMinuteChartWindow();
            _viewSolarMinuteChartWindow.Owner = Application.Current.MainWindow;
            _viewSolarMinuteChartWindow.Show();
        }


        private void ViewMonthlyTempChart()
        {
            Labels = dayValues.ToArray();
            YFormatter = value => value.ToString();


            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = SelectedStation.Model.Location,
                    Values = new ChartValues<decimal>(monthlyTempValues)
                }
            };

            _viewMonthlyTempWindow = new MonthlyTempWindow();
            _viewMonthlyTempWindow.Owner = Application.Current.MainWindow;
            _viewMonthlyTempWindow.Show();
        }

        private void ViewMonthlyHumidChartProc()
        {
            Labels = dayValues.ToArray();
            YFormatter = value => value.ToString();


            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = SelectedStation.Model.Location,
                    Values = new ChartValues<decimal>(monthlyHumValues)
                }
            };

            _viewMonthlyHumWindow = new MonthlyHumidityWindow();
            _viewMonthlyHumWindow.Owner = Application.Current.MainWindow;
            _viewMonthlyHumWindow.Show();
        }

        private void ViewMonthlyPressureChartProc()
        {
            Labels = dayValues.ToArray();
            YFormatter = value => value.ToString();


            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = SelectedStation.Model.Location,
                    Values = new ChartValues<decimal>(monthlyPressureValues)
                }
            };

            _viewMonthlyPressureWindow = new MonthlyPressureWindow();
            _viewMonthlyPressureWindow.Owner = Application.Current.MainWindow;
            _viewMonthlyPressureWindow.Show();
        }

        private void ViewMonthlyRain15ChartProc()
        {
            Labels = dayValues.ToArray();
            YFormatter = value => value.ToString();


            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = SelectedStation.Model.Location,
                    Values = new ChartValues<decimal>(monthlyRain15Values)
                }
            };

            _viewMonthlyRain15Window = new MonthlyRain15Window();
            _viewMonthlyRain15Window.Owner = Application.Current.MainWindow;
            _viewMonthlyRain15Window.Show();
        }

        private void ViewMonthlyRain24ChartProc()
        {
            Labels = dayValues.ToArray();
            YFormatter = value => value.ToString();


            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = SelectedStation.Model.Location,
                    Values = new ChartValues<decimal>(monthlyRain24Values)
                }
            };

            _viewMonthlyRain24Window = new MonthlyRain24Window();
            _viewMonthlyRain24Window.Owner = Application.Current.MainWindow;
            _viewMonthlyRain24Window.Show();
        }

        private void ViewMonthlyWindChartProc()
        {
            Labels = dayValues.ToArray();
            YFormatter = value => value.ToString();


            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = SelectedStation.Model.Location,
                    Values = new ChartValues<decimal>(monthlywindspeedValues)
                }
            };

            _viewMonthlyWindSpeedWindow = new MonthlyWindSpeedWindow();
            _viewMonthlyWindSpeedWindow.Owner = Application.Current.MainWindow;
            _viewMonthlyWindSpeedWindow.Show();
        }

        private void ViewMonthlySolarHourChartProc()
        {
            Labels = dayValues.ToArray();
            YFormatter = value => value.ToString();


            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = SelectedStation.Model.Location,
                    Values = new ChartValues<decimal>(monthlysolarHourValues)
                }
            };

            _viewMonthlySolarHourWindow = new MonthlySolarHourWindow();
            _viewMonthlySolarHourWindow.Owner = Application.Current.MainWindow;
            _viewMonthlySolarHourWindow.Show();
        }

        private void ViewMonthlySolarMinuteChartProc()
        {
            Labels = dayValues.ToArray();
            YFormatter = value => value.ToString();


            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = SelectedStation.Model.Location,
                    Values = new ChartValues<decimal>(monthlysolarMinuteValues)
                }
            };

            _viewMonthlySolarMinutewWindow = new MonthlySolarMinutewWindow();
            _viewMonthlySolarMinutewWindow.Owner = Application.Current.MainWindow;
            _viewMonthlySolarMinutewWindow.Show();
        }


        private void ViewYearlyTempChart()
        {
            Labels = monthValues.ToArray();
            YFormatter = value => value.ToString();


            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = SelectedStation.Model.Location,
                    Values = new ChartValues<decimal>(yearlyTempValues)
                }
            };

            _viewYearlyTempWindow = new YearlyTempWindow();
            _viewYearlyTempWindow.Owner = Application.Current.MainWindow;
            _viewYearlyTempWindow.Show();
        }

        private void ViewYearlyHumidChartProc()
        {
            Labels = monthValues.ToArray();
            YFormatter = value => value.ToString();


            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = SelectedStation.Model.Location,
                    Values = new ChartValues<decimal>(yearlyHumValues)
                }
            };

            _viewYearlyHumWindow = new YearlyHumidityWindow();
            _viewYearlyHumWindow.Owner = Application.Current.MainWindow;
            _viewYearlyHumWindow.Show();
        }

        private void ViewYearlyPressureChartProc()
        {
            Labels = monthValues.ToArray();
            YFormatter = value => value.ToString();


            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = SelectedStation.Model.Location,
                    Values = new ChartValues<decimal>(yearlyPressureValues)
                }
            };

            _viewYearlyPressureWindow = new YearlyPressureWindow();
            _viewYearlyPressureWindow.Owner = Application.Current.MainWindow;
            _viewYearlyPressureWindow.Show();
        }

        private void ViewYearlyRain15ChartProc()
        {
            Labels = monthValues.ToArray();
            YFormatter = value => value.ToString();


            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = SelectedStation.Model.Location,
                    Values = new ChartValues<decimal>(yearlyRain15Values)
                }
            };

            _viewYearlyRain15Window = new YearlyRain15Window();
            _viewYearlyRain15Window.Owner = Application.Current.MainWindow;
            _viewYearlyRain15Window.Show();
        }

        private void ViewYearlyRain24ChartProc()
        {
            Labels = monthValues.ToArray();
            YFormatter = value => value.ToString();


            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = SelectedStation.Model.Location,
                    Values = new ChartValues<decimal>(yearlyRain24Values)
                }
            };

            _viewYearlyRain24Window = new YearlyRain24Window();
            _viewYearlyRain24Window.Owner = Application.Current.MainWindow;
            _viewYearlyRain24Window.Show();
        }

        private void ViewYearlyWindChartProc()
        {
            Labels = monthValues.ToArray();
            YFormatter = value => value.ToString();


            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = SelectedStation.Model.Location,
                    Values = new ChartValues<decimal>(yearlywindspeedValues)
                }
            };

            _viewYearlyWindSpeedWindow = new YearlyWindSpeedWindow();
            _viewYearlyWindSpeedWindow.Owner = Application.Current.MainWindow;
            _viewYearlyWindSpeedWindow.Show();
        }

        private void ViewYearlySolarHourChartProc()
        {
            Labels = monthValues.ToArray();
            YFormatter = value => value.ToString();


            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = SelectedStation.Model.Location,
                    Values = new ChartValues<decimal>(yearlysolarHourValues)
                }
            };

            _viewYearlySolarHourWindow = new YearlySolarHourWindow();
            _viewYearlySolarHourWindow.Owner = Application.Current.MainWindow;
            _viewYearlySolarHourWindow.Show();
        }

        private void ViewYearlySolarMinuteChartProc()
        {
            Labels = monthValues.ToArray();
            YFormatter = value => value.ToString();


            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = SelectedStation.Model.Location,
                    Values = new ChartValues<decimal>(yearlysolarMinuteValues)
                }
            };

            _viewYearlySolarMinutewWindow = new YearlySolarMinuteWindow();
            _viewYearlySolarMinutewWindow.Owner = Application.Current.MainWindow;
            _viewYearlySolarMinutewWindow.Show();
        }


        //public string SelectedMonth
        //{
        //    get { return Convert.ToString(_selectedMonth); }
        //    set
        //    {
        //        _selectedMonth = value;
        //        var monthSelected = AllMonths.IndexOf(_selectedMonth.ToString());
        //        var viewDeliveriesList = CollectionViewSource.GetDefaultView(MeasurementsList);

        //        if (SelectedMonth != null && SelectedYear != null)
        //        {
        //            viewDeliveriesList.Filter = obj => ((MeasurementViewModel)obj).Model.Date.Month.ToString().Contains(monthSelected.ToString()) && ((MeasurementViewModel)obj).Model.Date.Year.ToString().Contains(SelectedYear.ToString());
        //        }
        //        if (SelectedMonth != null && SelectedYear == null)
        //        {
        //            viewDeliveriesList.Filter = obj => ((MeasurementViewModel)obj).Model.Date.Month.ToString().Equals(monthSelected.ToString());
        //        }
        //        RaisePropertyChanged(nameof(SelectedMonth));
        //    }
        //}

        //public string SelectedYear
        //{
        //    get { return _selectedYear; }
        //    set
        //    {
        //        _selectedYear = value;
        //        var monthSelected = AllMonths.IndexOf(_selectedMonth);
        //        var viewDeliveriesList = CollectionViewSource.GetDefaultView(MeasurementsList);

        //        if (SelectedMonth != null && SelectedYear != null)
        //        {
        //            viewDeliveriesList.Filter = obj => ((MeasurementViewModel)obj).Model.Date.Month.ToString().Contains(monthSelected.ToString()) && ((MeasurementViewModel)obj).Model.Date.Year.ToString().Contains(SelectedYear.ToString());
        //        }

        //        if (_selectedYear != null && SelectedMonth == null)
        //        {
        //            viewDeliveriesList.Filter = obj => ((MeasurementViewModel)obj).Model.Date.Year.ToString().Contains(_selectedYear.ToString());
        //        }

        //        RaisePropertyChanged(nameof(SelectedYear));
        //    }
        //}


        private async Task LoadStationsAsync()
        {
            var tmp = 0;
            var stations = await Task.Run(() => _repository.Stations.GetRangeAsync());
            foreach (var station in stations)
            {
                StationsList.Add(new StationModel(station, _repository));
                stationStrings[tmp] = station.Location;
                tmp++;
            }
        }

        private async void LoadAllStations()
        {
            var tmp = 0;
            var stations = await Task.Run(() => _repository.Stations.GetRangeAsync());
            foreach (var station in stations)
            {
                stationStrings[tmp] = station.Location;
                tmp++;
            }
        }

        private void LoadSelectedStation()
        {
            Array.Clear(stationStrings, 0, 20);
            var tmp = 0;
            stationStrings[tmp] = SelectedStation.Model.Location;
        }

        public void Update()
        {
            UpdateIndex();
        }
    }
}