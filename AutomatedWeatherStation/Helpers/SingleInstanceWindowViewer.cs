using System;
using System.Windows;

namespace AutomatedWeatherStation.Helpers
{
    /// <summary>
    ///     Acts as a wrapper to a Window to create a single instance of that Window
    ///     If the Window is not yet creates, anew instance will be created and shown/activated
    ///     If the Window is already created, it still becomes the active window
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingleInstanceWindowViewer<T> where T : Window, new()
    {
        private bool _isWindowActive;
        private T _window;

        /// <summary>
        ///     Gets the associated Window of this instance
        ///     Make sure not to call the associated WIndow's Show method
        /// </summary>
        public T Instance { get; private set; }

        public void Close()
        {
            _window?.Close();
        }

        public void Show()
        {
            if (!_isWindowActive)
            {
                _window = new T
                {
                    Topmost = false,
                    ShowActivated = true
                };
                Instance = _window;
                _window.Closed += OnClosed;
            }

            _window?.Show();
            _window?.Activate();
            _isWindowActive = true;
        }

        private void OnClosed(object sender, EventArgs e)
        {
            _isWindowActive = false;
            _window.Closed -= OnClosed;
        }
    }
}