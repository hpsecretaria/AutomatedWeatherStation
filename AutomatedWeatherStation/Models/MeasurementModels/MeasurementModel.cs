using System.Collections.ObjectModel;
using System.Windows.Input;
using AutomatedWeatherStation.DataAccess;
using AutomatedWeatherStation.DataAccess.EF;
using AutomatedWeatherStation.Helpers;
using AutomatedWeatherStation.Models.StationModels;
using AutomatedWeatherStation.Reports.Measurements;
using GalaSoft.MvvmLight.CommandWpf;

namespace AutomatedWeatherStation.Models.MeasurementModels
{
    public class MeasurementModel : ViewModelBase<Measurement>
    {
        private readonly SingleInstanceWindowViewer<MeasurementReportWindow> _measurementsWindow =
            new SingleInstanceWindowViewer<MeasurementReportWindow>();


        public MeasurementModel(Measurement model, IRepository repository) : base(model, repository)
        {
            //   _repository = repository;
        }

        public ICommand PrintMeasurementCommand => new RelayCommand(PrintMeasurementProc);

        public StationModel AssociatedStation { get; set; }

        //LoadMeasurements
        public ObservableCollection<Measurement> MeasurementsList { get; } =
            new ObservableCollection<Measurement>();

        public ObservableCollection<Station> StationsList { get; } = new ObservableCollection<Station>();

        private void PrintMeasurementProc()
        {
            _measurementsWindow.Show();
        }
    }
}