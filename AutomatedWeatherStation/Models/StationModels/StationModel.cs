using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AutomatedWeatherStation.DataAccess;
using AutomatedWeatherStation.DataAccess.EF;
using AutomatedWeatherStation.Helpers;
using AutomatedWeatherStation.Models.Editable;
using AutomatedWeatherStation.Models.MeasurementModels;
using AutomatedWeatherStation.Reports.Measurements;
using GalaSoft.MvvmLight.CommandWpf;
using Nito.AsyncEx;

namespace AutomatedWeatherStation.Models.StationModels
{
    public class StationModel : ViewModelBase<Station>, IEditableModel<Station>
    {
        private readonly SingleInstanceWindowViewer<MeasurementReportWindow> _measurementsWindow =
            new SingleInstanceWindowViewer<MeasurementReportWindow>();

        private EditModelBase<Station> _editModel;
        private bool _isEditing;
        private bool _isSelected;

        private MeasurementModel _latestMeasurement;
        private int _priority;

        public StationModel(Station model, IRepository repository) : base(model, repository)
        {
        }


        public ICommand PrintCommand => new RelayCommand(PrintProc);

        public EditModelBase<Station> EditModel
        {
            get { return _editModel; }
            protected set
            {
                _editModel = value;
                RaisePropertyChanged(nameof(EditModel));
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged(nameof(IsSelected));
            }
        }

        public MeasurementModel LatestMeasurement
        {
            get { return _latestMeasurement; }
            set
            {
                _latestMeasurement = value;
                RaisePropertyChanged(nameof(LatestMeasurement));
            }
        }

        public INotifyTaskCompletion SaveLoading { get; private set; }

        public object LoadingInfoLoading { get; private set; }

        public bool isEditing
        {
            get { return _isEditing; }
            private set
            {
                _isEditing = value;
                RaisePropertyChanged(nameof(isEditing));
            }
        }

        public ICommand EditCommand => new RelayCommand(EditProc);

        public ICommand SaveEditCommand => new RelayCommand(SaveEditProc, SaveEditCondition);

        public ICommand CancelEditCommand => new RelayCommand(CancelEditProc);

        EditModelBase<Station> IEditableModel<Station>.EditModel
        {
            get { throw new NotImplementedException(); }
        }


        private void PrintProc()
        {
            _measurementsWindow.Show();
            //    //            _booksWindow.Instance.Show(); DONT USE THIS COZ ALREADY MADE
            //    _booksWindow.Show();
        }

        private void EditProc()
        {
            isEditing = true;

            EditModel?.Dispose();

            EditModel = new StationEditModel(Model);
        }

        private bool SaveEditCondition()
        {
            return (EditModel != null) && EditModel.HasChanges && !EditModel.HasErrors;
        }

        private void SaveEditProc()
        {
            SaveLoading = NotifyTaskCompletion.Create(SaveEditProcAsync);
        }

        private async Task SaveEditProcAsync()
        {
            if (EditModel == null) return;
            if (!EditModel.HasChanges) return;

            try
            {
                await Task.Run(() => _Repository.Stations.UpdateAsync(EditModel.ModelCopy));

                //replace the model with the edited copy
                Model = EditModel.ModelCopy;

                isEditing = false;
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to save.");
            }
        }

        private void CancelEditProc()
        {
            isEditing = false;
            EditModel?.Dispose();
        }

        public void LoadRelatedInfo()
        {
            LoadingInfoLoading = NotifyTaskCompletion.Create(LoadRelatedInfoAsync());
        }

        private async Task LoadRelatedInfoAsync()
        {
            var task1 = LoadLatestMeasurement();
            await Task.WhenAll(new List<Task> {task1});
        }

        private async Task LoadLatestMeasurement()
        {
            var lm = await Task.Run(() => _Repository.Measurements.GetRangeAsync(c => c.StationId == Model.StationId));
            var m = lm.OrderByDescending(c => c.Date).FirstOrDefault();
            LatestMeasurement = new MeasurementModel(m, _Repository);
        }

        //}
        //    }
        //        Measurements.Add(measurement);
        //    {
        //    foreach (var measurement in measurements)
        //    var measurements = await Task.Run(()=>_Repository.Measurements.GetRangeAsync(c => c.StationId == Model.StationId));
        //    Measurements.Clear();
        //{

        //public async Task LoadMeasurementsAsync()
    }
}