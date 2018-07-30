using System.Windows.Input;

namespace AutomatedWeatherStation.Models.Editable
{
    public interface IEditableModel<T>
    {
        /// <summary>
        ///     gets the status of the edit procedure in an instance
        /// </summary>
        bool isEditing { get; }

        /// <summary>
        ///     The underlying Model used for editing
        ///     Create another class that inherits this type to delegate
        ///     the editing logic
        /// </summary>
        EditModelBase<T> EditModel { get; }

        /// <summary>
        ///     The edit command used to start the edit process.
        /// </summary>
        ICommand EditCommand { get; }

        /// <summary>
        ///     The save command used to end the edit procedure.
        /// </summary>
        ICommand SaveEditCommand { get; }

        /// <summary>
        ///     The cancel command used to cancel the edit procedure.
        /// </summary>
        ICommand CancelEditCommand { get; }
    }
}