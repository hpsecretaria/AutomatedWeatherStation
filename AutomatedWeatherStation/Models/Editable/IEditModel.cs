using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using GalaSoft.MvvmLight;

namespace AutomatedWeatherStation.Models.Editable
{
    /// <summary>
    ///     The interface that represents an editable instance of the underlying model.
    /// </summary>
    /// <typeparam name="T">The underlying model</typeparam>
    public interface IEditModel<out T> : INotifyDataErrorInfo
    {
        /// <summary>
        ///     Represents the original model
        /// </summary>
        T ModelOriginal { get; }

        /// <summary>
        ///     Represents the copied model
        /// </summary>
        T ModelCopy { get; }

        /// <summary>
        ///     Identifies if this EditViewModel's properties has data changes
        /// </summary>
        bool HasChanges { get; }

        /// <summary>
        ///     the last error encountered in the model's properties
        /// </summary>
        string TopmostError { get; }
    }

    public abstract class EditModelBase<T> : ObservableObject, IEditModel<T>, IDisposable
    {
        private readonly IDictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
        protected T _ModelCopy;
        protected T _ModelOriginal;


        /// <summary>
        ///     the model passed to this class
        /// </summary>
        /// <param name="model"></param>
        public EditModelBase(T model)
        {
            PropertyChanged += ModelOnPropertyChanged;
            _ModelOriginal = model;
        }

        public void Dispose()
        {
            if (_ModelOriginal == null) return;
            PropertyChanged -= ModelOnPropertyChanged;
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return _errors.Values;

            return _errors.ContainsKey(propertyName) ? _errors[propertyName] : null;
        }

        public bool HasErrors => _errors.Count > 0;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public T ModelOriginal
        {
            get { return _ModelOriginal; }
            protected set
            {
                _ModelOriginal = value;
                RaisePropertyChanged(nameof(ModelOriginal));
            }
        }

        public T ModelCopy
        {
            get { return _ModelCopy; }
            protected set
            {
                _ModelCopy = value;
                RaisePropertyChanged(nameof(ModelCopy));
            }
        }

        public bool HasChanges { get; protected set; }

        public string TopmostError
        {
            get
            {
                var topmost = _errors.LastOrDefault().Value;
                return (topmost == null) || (topmost.Count == 0) ? string.Empty : topmost[0];
            }
        }

        protected void SetErrors(string propertyName, string propertyError)
        {
            if (_errors.ContainsKey(propertyName))
            {
                _errors[propertyName].Clear();
                _errors[propertyName].Add(propertyError);
            }
            else
            {
                var propertyErrors = new List<string> {propertyError};
                _errors.Add(propertyName, propertyErrors);
            }

            RaisePropertyChanged(nameof(TopmostError));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        protected void ClearErrors(string propertyName)
        {
            //Remove the error lists from the property
            _errors.Remove(propertyName);
            RaisePropertyChanged(nameof(TopmostError));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void ModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(HasChanges) || (e.PropertyName == nameof(ModelCopy))) return;
            HasChanges = true;
        }

        protected TField ValidateInputAndAddErrors<TField>(ref TField field, TField value, string propertyName,
            Func<bool> invalidInputFilter, string errorMessage, bool storeInvalidInput = true)
        {
            var isValid = !invalidInputFilter();
            if (isValid)
            {
                ClearErrors(propertyName);
                field = value;
                RaisePropertyChanged(nameof(propertyName));
                return value;
            }
            SetErrors(propertyName, errorMessage);

            if (!storeInvalidInput) return field;
            field = value;

            RaisePropertyChanged(nameof(propertyName));

            return value;
        }
    }
}