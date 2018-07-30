using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Threading;
using AutomatedWeatherStation.DataAccess;
using AutomatedWeatherStation.DataAccess.EF;
using AutomatedWeatherStation.Models.StationModels;
using GalaSoft.MvvmLight;
using Nito.AsyncEx;
using TableDependency.Enums;
using TableDependency.EventArgs;

namespace AutomatedWeatherStation.Modules
{
    public class StationMapsModule : ObservableObject
    {
        private readonly IRepository _repository;
        private readonly object _stationslock = new object();
        private readonly DispatcherTimer timer = new DispatcherTimer();

        private DateTime _dateTimeNow;
        private bool _isMapReady;
        private StationModel _selectedStationMap;

        public StationMapsModule(IRepository repository)
        {
            _repository = repository;
            BindingOperations.EnableCollectionSynchronization(Stations, _stationslock);
            CheckStationsLoading = NotifyTaskCompletion.Create(LoadInformationAsync);
        }

        public INotifyTaskCompletion CheckStationsLoading { get; set; }

        public ObservableCollection<StationModel> Stations { get; } = new ObservableCollection<StationModel>();

        public bool IsMapReady
        {
            get { return _isMapReady; }
            set
            {
                _isMapReady = value;
                RaisePropertyChanged(nameof(IsMapReady));
            }
        }

        public DateTime DateTimeNow
        {
            get { return _dateTimeNow; }
            set
            {
                _dateTimeNow = value;
                RaisePropertyChanged(nameof(DateTimeNow));
            }
        }

        public StationModel SelectedStationMap
        {
            get { return _selectedStationMap; }
            set
            {
                _selectedStationMap = value;
                _selectedStationMap?.LoadRelatedInfo();
                RaisePropertyChanged(nameof(SelectedStationMap));
            }
        }

        private async Task LoadInformationAsync()
        {
            var stations = await Task.Run(() => _repository.Stations.GetRangeAsync());
            foreach (var station in stations)
            {
                Stations.Add(new StationModel(station, _repository));
            }

            SelectedStationMap = Stations.FirstOrDefault();


            timer.Interval = new TimeSpan(0, 0, 10);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            var index = Stations.IndexOf(SelectedStationMap);
            if (index == Stations.Count - 1)
            {
                //var station = Stations.FirstOrDefault();
                //station?.LoadRelatedInfo();
                ;
                SelectedStationMap = Stations.FirstOrDefault();
            }
            else
            {
                //var station = Stations.ElementAt(index + 1);
                //station?.LoadRelatedInfo();
                SelectedStationMap = Stations.ElementAt(index + 1);
            }
        }

        private async Task UpdateList(RecordChangedEventArgs<Station> e)
        {
            await Task.Delay(1);
            if (e.ChangeType == ChangeType.None)
                return;

            if (e.ChangeType == ChangeType.Delete)
            {
                var s = Stations.FirstOrDefault(c => c.Model.StationId == e.Entity.StationId);
                if (s == null)
                    return;

                Stations.Remove(s);
            }
            if (e.ChangeType == ChangeType.Insert)
            {
                if (Stations.Contains(new StationModel(e.Entity, _repository)))
                {
                    return;
                }
                Stations.Add(new StationModel(e.Entity, _repository));
            }
            if (e.ChangeType == ChangeType.Update)
            {
                var s = Stations.FirstOrDefault(c => c.Model.StationId == e.Entity.StationId);
                if (s == null)
                    return;
                var i = Stations.IndexOf(s);
                Stations.Remove(s);
                Stations.Insert(i, new StationModel(e.Entity, _repository));
            }
        }

        public void MapSelectedStation(decimal lat, decimal lng)
        {
            LoadSelectedStation(lat, lng);
        }

        public async void Update(RecordChangedEventArgs<Station> e)
        {
            await UpdateList(e);
        }

        private void LoadSelectedStation(decimal lat, decimal lng)
        {
            foreach (var station in Stations)
                if (station.Model.Latitude == Convert.ToDecimal(lat) &&
                    station.Model.Longitude == Convert.ToDecimal(lng))
                    SelectedStationMap = station;
            timer.Stop();
            timer.Start();
        }
    }
}