using AutomatedWeatherStation.DataAccess;
using GalaSoft.MvvmLight;

namespace AutomatedWeatherStation.Models
{
    public abstract class ViewModelBase<T> : ObservableObject
    {
        private T _model;
        protected IRepository _Repository;

        public ViewModelBase(T model)
        {
            Model = model;
        }

        public ViewModelBase(T model, IRepository repository)
        {
            Model = model;
            _Repository = repository;
        }

        public T Model
        {
            get { return _model; }
            protected set
            {
                _model = value;
                RaisePropertyChanged(nameof(Model));
            }
        }
    }
}