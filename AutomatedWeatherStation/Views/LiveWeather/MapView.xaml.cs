using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using AutomatedWeatherStation.DataAccess;
using AutomatedWeatherStation.DataAccess.EF;
using AutomatedWeatherStation.Models.MeasurementModels;
using AutomatedWeatherStation.Models.StationModels;
using AutomatedWeatherStation.Modules;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using Nito.AsyncEx;
using TableDependency.Enums;
using TableDependency.EventArgs;
using TableDependency.SqlClient;

namespace AutomatedWeatherStation.Views.LiveWeather
{
    /// <summary>
    ///     Interaction logic for MapView.xaml
    /// </summary>
    public partial class MapView : UserControl, IDisposable
    {
        private readonly object _markersLock = new object();
        private readonly AsyncLock _measurementsChangedLock = new AsyncLock();
        private readonly IRepository _repository = new EfRepository();
        private readonly AsyncLock _stationsChangedLock = new AsyncLock();

        public MapView()
        {
            InitializeComponent();
        }

        public Grid Gr { get; private set; }

        public INotifyTaskCompletion UpdateViewLoading { get; private set; }

        public INotifyTaskCompletion LoadInformationLoading { get; private set; }

        public SqlTableDependency<DataAccess.EF.Station> StationDependency { get; private set; }
        public SqlTableDependency<Measurement> MeasurementDependency { get; private set; }

        public RecordChangedEventArgs<DataAccess.EF.Station> Prevstationschange { get; set; }

        public RecordChangedEventArgs<Measurement> Prevmeasurementschange { get; set; }

        public INotifyTaskCompletion LoadTableDependecyLoading { get; set; }

        public void Dispose()
        {
            StationDependency.Stop();
            MeasurementDependency.Stop();
        }

        private async Task LoadInformationAsync()
        {
            await Task.Delay(1000);
            try
            {
                var e = Dns.GetHostEntry("www.google_com.hk");
            }
            catch (Exception)
            {
                MapControl.Manager.Mode = AccessMode.ServerAndCache;
            }
            // config map 

            //use google provider
            MapControl.MapProvider = GMapProviders.GoogleMap;
            //get tiles from server only
            //    MapControl.Manager.Mode = AccessMode.ServerOnly;
            //not use proxy
            GMapProvider.WebProxy = null;
            //center map on moscow
            //MapControl.Position = new PointLatLng(7.199001, 125.454941);
            MapControl.Position = new PointLatLng(7.199001, 125.775604248047);

            //zoom min/max; default both = 2
            MapControl.MinZoom = 1;
            MapControl.MaxZoom = 20;
            //set zoom
            MapControl.Zoom = 10;
            MapControl.MouseMove += MapControlOnMouseMove;
            MapControl.MouseLeftButtonDown += MapControl_MouseLeftButtonDown;

            UpdateView();

            BindingOperations.EnableCollectionSynchronization(MapControl.Markers, _markersLock);

            ViewModelLocatorStatic.Locator.StationMapsModule.IsMapReady = true;
        }

        private async void MeasurementTableDependency_Changed(object sender, RecordChangedEventArgs<Measurement> e)
        {
            using (await _measurementsChangedLock.LockAsync())
            {
                try
                {
                    if (Prevmeasurementschange.ChangeType == e.ChangeType &&
                        e.Entity.StationId == Prevmeasurementschange.Entity.StationId)
                    {
                        return;
                    }
                }
                catch (Exception)
                {
                }
                Prevmeasurementschange = e;
                await MeasurementsUpdate(e);
            }
        }

        private async Task MeasurementsUpdate(RecordChangedEventArgs<Measurement> e)
        {
            try
            {
                var station =
                    await Task.Run(() => _repository.Stations.GetAsync(c => c.StationId == e.Entity.StationId));
                var s =
                    MapControl.Markers.FirstOrDefault(
                        c => c.Position == new PointLatLng(Convert.ToDouble(station.Latitude),
                            Convert.ToDouble(station.Longitude)));
                if (s == null)
                    return;

                MapControl.Markers.Remove(s);
                var newmarker =
                    new GMapMarker(new PointLatLng(Convert.ToDouble(station.Latitude),
                        Convert.ToDouble(station.Longitude)));

                var x = Resources["Grid1"] as Grid;


                var measurement = new MeasurementModel(e.Entity, _repository)
                {
                    AssociatedStation = new StationModel(station, _repository)
                };

                if (x != null)
                {
                    x.DataContext = measurement;
                    newmarker.Shape = x;
                    newmarker.Offset = new Point(-25, -25);
                    newmarker.ZIndex = 0;
                }
                newmarker.Shape.MouseLeftButtonDown += ShapeOnMouseLeftButtonDown;
                MapControl.Markers.Add(newmarker);
            }
            catch (Exception)
            {
            }
        }

        private async void StationTableDependency_Changed(object sender, RecordChangedEventArgs<DataAccess.EF.Station> e)
        {
            using (await _stationsChangedLock.LockAsync())
            {
                try
                {
                    if (Prevstationschange.ChangeType == e.ChangeType &&
                        e.Entity.StationId == Prevstationschange.Entity.StationId)
                    {
                        return;
                    }
                }
                catch (Exception)
                {
                }
                Prevstationschange = e;
                ViewModelLocatorStatic.Locator.StationMapsModule.Update(e);
                ViewModelLocatorStatic.Locator.StationsModule.Update(e);
                await StationsUpdate(e);
            }
        }

        private async Task StationsUpdate(RecordChangedEventArgs<DataAccess.EF.Station> e)
        {
            await Task.Delay(1);
            if (e.ChangeType == ChangeType.None)
                return;

            if (e.ChangeType == ChangeType.Delete)
            {
                var s =
                    MapControl.Markers.FirstOrDefault(
                        c => c.Position == new PointLatLng(Convert.ToDouble(e.Entity.Latitude),
                            Convert.ToDouble(e.Entity.Longitude)));
                if (s == null)
                    return;

                MapControl.Markers.Remove(s);
            }

            if (e.ChangeType == ChangeType.Insert || e.ChangeType == ChangeType.Update)
            {
                try
                {
                    if (e.ChangeType == ChangeType.Update)
                    {
                        var s =
                            MapControl.Markers.FirstOrDefault(
                                c => c.Position == new PointLatLng(Convert.ToDouble(e.Entity.Latitude),
                                    Convert.ToDouble(e.Entity.Longitude)));
                        if (s == null)
                            return;

                        MapControl.Markers.Remove(s);
                    }
                    var newmarker =
                        new GMapMarker(new PointLatLng(Convert.ToDouble(e.Entity.Latitude),
                            Convert.ToDouble(e.Entity.Longitude)));

                    var x = Gr;

                    var meas =
                        await
                            Task.Run(
                                () => _repository.Measurements.GetRangeAsync(c => c.StationId == e.Entity.StationId));

                    var m = meas.OrderByDescending(c => c.Date).FirstOrDefault();

                    var measurement = new MeasurementModel(m, _repository)
                    {
                        AssociatedStation = new StationModel(e.Entity, _repository)
                    };

                    if (x != null)
                    {
                        x.DataContext = measurement;
                        newmarker.Shape = x;
                        newmarker.Offset = new Point(-25, -25);
                        newmarker.ZIndex = 0;
                    }
                    newmarker.Shape.MouseLeftButtonDown += ShapeOnMouseLeftButtonDown;
                    MapControl.Markers.Add(newmarker);
                }
                catch (Exception)
                {
                }
            }
        }

        private void MapControlOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            foreach (var gMapMarker in MapControl.Markers)
            {
                if (gMapMarker.Shape.IsMouseOver)
                    gMapMarker.ZIndex = 5;
                else
                    gMapMarker.ZIndex = 0;
            }
        }


        private void MapControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //var clickPoint = e.GetPosition(MapControl);
            //var point = MapControl.FromLocalToLatLng((int)clickPoint.X, (int)clickPoint.Y);
            ////MapControl.Position = point;
            //MessageBox.Show(point.Lat.ToString() + " "+point.Lng.ToString());
            //////var marker = new GMapMarker(point);
        }

        private void UpdateView()
        {
            UpdateViewLoading = NotifyTaskCompletion.Create(UpdateViewAsync());
        }

        private async Task UpdateViewAsync()
        {
            var newMarkers = new ObservableCollection<GMapMarker>();
            try
            {
                var stations = await Task.Run(() => _repository.Stations.GetRangeAsync());

                foreach (var station in stations)
                {
                    var newmarker =
                        new GMapMarker(new PointLatLng(Convert.ToDouble(station.Latitude),
                            Convert.ToDouble(station.Longitude)));

                    var x = Resources["Grid1"] as Grid;
                    //  var x = Gr;
                    var meas =
                        await
                            Task.Run(() => _repository.Measurements.GetRangeAsync(c => c.StationId == station.StationId));

                    var m = meas.OrderByDescending(c => c.Date).FirstOrDefault();

                    var measurement = new MeasurementModel(m, _repository)
                    {
                        AssociatedStation = new StationModel(station, _repository)
                    };

                    if (x != null)
                    {
                        x.DataContext = measurement;
                        newmarker.Shape = x;
                        newmarker.Offset = new Point(-25, -25);
                        newmarker.ZIndex = 0;
                    }
                    newmarker.Shape.MouseLeftButtonDown += ShapeOnMouseLeftButtonDown;
                    newMarkers.Add(newmarker);
                }
            }
            catch (Exception)
            {
                return;
            }

            MapControl.Markers.Clear();
            var markerss = newMarkers.OrderByDescending(c => c.LocalPositionX).ToList();

            foreach (var marker in markerss)
            {
                MapControl.Markers.Add(marker);
            }
        }

        private void ShapeOnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            foreach (var item in MapControl.Markers)
                if (item.Shape == sender)
                    ViewModelLocatorStatic.Locator.StationMapsModule.MapSelectedStation(
                        Convert.ToDecimal(item.Position.Lat), Convert.ToDecimal(item.Position.Lng));
        }

        private void MapView_OnLoaded(object sender, RoutedEventArgs e)
        {
            Gr = Resources["Grid1"] as Grid;
            LoadInformationLoading = NotifyTaskCompletion.Create(LoadInformationAsync());
            LoadTableDependecyLoading = NotifyTaskCompletion.Create(LoadTableDependencyAsync());
        }

        private async Task LoadTableDependencyAsync()
        {
            var conStr = ConfigurationManager.ConnectionStrings["AWSContext"].ToString();

            await Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        StationDependency = new SqlTableDependency<DataAccess.EF.Station>(conStr);
                        break;
                    }
                    catch (Exception)
                    {
                    }
                }
            });
            await Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        MeasurementDependency = new SqlTableDependency<Measurement>(conStr);
                        break;
                    }
                    catch (Exception)
                    {
                    }
                }
            });


            StationDependency.OnChanged += StationTableDependency_Changed;
            StationDependency.Start();

            MeasurementDependency.OnChanged += MeasurementTableDependency_Changed;
            MeasurementDependency.Start();
        }

        private void BtnCenter_OnClick(object sender, RoutedEventArgs e)
        {
            MapControl.Position = new PointLatLng(7.199001, 125.775604248047);
            MapControl.MinZoom = 1;
            MapControl.MaxZoom = 20;
            MapControl.Zoom = 10;
        }
    }
}