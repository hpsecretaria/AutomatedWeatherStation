using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using AutomatedWeatherStation.DataAccess;
using AutomatedWeatherStation.DataAccess.EF;
using AutomatedWeatherStation.Models.StationModels;
using AutomatedWeatherStation.Views.Station;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Nito.AsyncEx;
using TableDependency.Enums;
using TableDependency.EventArgs;

namespace AutomatedWeatherStation.Modules
{
    public class StationsModule : ObservableObject
    {
        private readonly IRepository _repository;
        private readonly object _stationListLock = new object();

        private AddStationWindow _addStationWindow;
        private NewStationModel _newStation;
        private string _searchStation;
        private StationModel _selectedStation;


        public StationsModule(IRepository repository)
        {
            _repository = repository;
            BindingOperations.EnableCollectionSynchronization(StationList, _stationListLock);
        }

        public string SearchStation
        {
            get { return _searchStation; }
            set
            {
                _searchStation = value;
                var viewCustomersList = CollectionViewSource.GetDefaultView(StationList);
                if (string.IsNullOrWhiteSpace(SearchStation))
                {
                    viewCustomersList.Filter = null;
                }
                else
                {
                    viewCustomersList.Filter =
                        obj => ((StationModel) obj).Model.Location.ToLower().Contains(SearchStation.ToLower());
                }
            }
        }

        public ObservableCollection<StationModel> StationList { get; } = new ObservableCollection<StationModel>();

        public INotifyTaskCompletion StationLoading { get; private set; }

        public StationModel SelectedStation
        {
            get { return _selectedStation; }
            set
            {
                //will return to the before-editing stage
                _selectedStation?.CancelEditCommand.Execute(null);

                _selectedStation = value;
                if (_selectedStation != null)
                {
                    _selectedStation.LoadRelatedInfo();
                }
                RaisePropertyChanged(nameof(SelectedStation));
            }
        }

        public NewStationModel NewStation
        {
            get { return _newStation; }
            set
            {
                _newStation = value;
                RaisePropertyChanged(nameof(NewStation));
            }
        }

        public ICommand SaveCommand => new RelayCommand(SaveProc, SaveCondition);
        public ICommand CancelAddCommand => new RelayCommand(CancelAddProc);
        public ICommand AddCommand => new RelayCommand(AddProc);

        public ICommand RemoveCommand => new RelayCommand(RemoveProc, RemoveCondition);

        public INotifyTaskCompletion SaveLoading { get; private set; }

        public INotifyTaskCompletion RemoveLoading { get; private set; }

        public void Initialize()
        {
            StationLoading = NotifyTaskCompletion.Create(LoadStationAsync());
        }

        private async Task LoadStationAsync()
        {
            //var s = await Task.Run(() => _repository.Stations.AddAsync(new Station
            //{
            //    Location = "a",
            //    DisplayLocationArea = "b",
            //    DisplayLocation = "c",
            //    PhoneNumber = "1",
            //    Imei = "2",
            //    Keyword = "As"
            //}));

            var stations = await Task.Run(() => _repository.Stations.GetRangeAsync());
            foreach (var item in stations)
            {
                StationList.Add(new StationModel(item, _repository));
                //if (item.Name.Contains("y"))
                //{
                //    throw new InvalidOperationException("bawal naay y");
                //}
            }
            //Stations = new AutoRefreshWrapper<Station>(
            //  MyObjectContext.MyObjectSet, RefreshMode.StoreWins);
        }


        private void CancelAddProc()
        {
            NewStation.Dispose();
            _addStationWindow.Close();
        }


        private void AddProc()
        {
            NewStation = new NewStationModel();
            _addStationWindow = new AddStationWindow();
            _addStationWindow.Owner = Application.Current.MainWindow;
            _addStationWindow.Show();
        }

        private bool SaveCondition()
        {
            return NewStation != null && NewStation.HasChanges && !NewStation.HasErrors;
        }

        private async Task SaveProcAsync()
        {
            if (NewStation == null) return;
            if (!NewStation.HasChanges) return;
            try
            {
                //NewStation.ModelCopy.Latitude = new decimal(01.010);
                //NewStation.ModelCopy.Longitude = new decimal(10.010);
                //var s= await Task.Run(() => _repository.Stations.AddAsync(NewStation.ModelCopy));
                // StationList.Add(new StationModel(NewStation.ModelCopy, _repository));
                // _addStationWindow.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("An error occured during save.", "Station");
            }
            //var s = new SaveStationModel(NewStation, _repository);
            await Task.Run(() => _repository.Stations.AddAsync(NewStation.ModelCopy));
            _addStationWindow.Close();
            //StationList.Add(new StationModel(NewStation.ModelCopy, _repository));
        }

        private void SaveProc()
        {
            //SaveProcAsync().ConfigureAwait(false);
            SaveLoading = NotifyTaskCompletion.Create(SaveProcAsync);
        }

        private bool RemoveCondition()
        {
            return SelectedStation != null;
        }


        public async void Update(RecordChangedEventArgs<Station> e)
        {
            //  MessageBox.Show("!");
            await UpdateList(e);
        }

        private async Task UpdateList(RecordChangedEventArgs<Station> e)
        {
            if (e.ChangeType == ChangeType.None)
            {
                return;
            }
            if (e.ChangeType == ChangeType.Delete)
            {
                var s = StationList.FirstOrDefault(c => c.Model.StationId == e.Entity.StationId);
                if (s == null)
                    return;

                StationList.Remove(s);
            }
            else if (e.ChangeType == ChangeType.Insert)
            {
                StationList.Add(new StationModel(e.Entity, _repository));
            }
            else if (e.ChangeType == ChangeType.Update)
            {
                var s = StationList.FirstOrDefault(c => c.Model.StationId == e.Entity.StationId);
                if (s == null)
                    return;
                var i = StationList.IndexOf(s);
                StationList[i] = new StationModel(e.Entity, _repository);
                SelectedStation = StationList[i];
            }
        }

        private async Task RemoveProcAsync()
        {
            try
            {
                await Task.Run(() => _repository.Stations.Remove(SelectedStation.Model));
                //StationList.Remove(SelectedStation);
            }
            catch (Exception)
            {
                MessageBox.Show("An error occured during removal.");
            }
        }

        private void RemoveProc()
        {
            RemoveLoading = NotifyTaskCompletion.Create(RemoveProcAsync);
        }
    }
}