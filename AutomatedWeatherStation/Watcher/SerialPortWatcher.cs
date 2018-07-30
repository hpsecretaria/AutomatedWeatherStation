using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Threading.Tasks;

namespace AutomatedWeatherStation.Watcher
{
    /// <summary>
    ///     Make sure you create this watcher in the UI thread if you are using the com port list in the UI
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class SerialPortWatcher : IDisposable
    {
        public delegate void PortsChangedEventHandler(object o, OnPortsChangedEventArgs e);

        private readonly TaskScheduler _taskScheduler;
        private readonly ManagementEventWatcher _watcher;

        private bool _isChanging;

        public SerialPortWatcher()
        {
            _taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            ComPorts = new List<string>(SerialPort.GetPortNames().OrderBy(s => s));

            var query = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent");

            _watcher = new ManagementEventWatcher(query);
            _watcher.EventArrived += (sender, eventArgs) => CheckForNewPorts(eventArgs);
            //_watcher.EventArrived += WatcherOnEventArrived;
            _watcher.Start();
        }


        public List<string> ComPorts { get; set; }

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                _watcher.Stop();
            }
            catch (Exception)
            {
            }
        }

        #endregion

        private async void WatcherOnEventArrived(object sender, EventArrivedEventArgs eventArrivedEventArgs)
        {
            await CheckForNewPortsAsync();
        }

        public event PortsChangedEventHandler PortsChanged;

        protected virtual void OnPortsChanged(OnPortsChangedEventArgs e)
        {
            var handler = PortsChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private async void CheckForNewPorts(EventArrivedEventArgs args)
        {
            if (_isChanging)
                return;

            _isChanging = true;
            // do it async so it is performed in the UI thread if this class has been created in the UI thread
            await Task.Run(() => CheckForNewPortsAsync());
        }

        private async Task CheckForNewPortsAsync()
        {
            await Task.Delay(1000);
            var ports = SerialPort.GetPortNames().ToList();
            var newItems = new List<string>(ports);
            var removedItems = new List<string>(ComPorts);

            if (ports.Count == ComPorts.Count)
                return;

            if (ports.Count > ComPorts.Count)
            {
                try
                {
                    foreach (var port in ComPorts)
                        if (ports.Contains(port))
                            newItems.Remove(port);
                }
                catch (Exception)
                {
                }


                ComPorts.AddRange(newItems);
                OnPortsChanged(new OnPortsChangedEventArgs(newItems, new List<string>()));
            }
            else
            {
                try
                {
                    foreach (var port in ports)
                        if (ComPorts.Contains(port))
                            removedItems.Remove(port);

                    foreach (var removedItem in removedItems)
                        ComPorts.Remove(removedItem);

                    OnPortsChanged(new OnPortsChangedEventArgs(new List<string>(), removedItems));
                }
                catch (Exception)
                {
                }
            }
            _isChanging = false;
        }
    }

    public class OnPortsChangedEventArgs : EventArgs
    {
        public List<string> NewItem = new List<string>();
        public List<string> RemovedItem = new List<string>();

        public OnPortsChangedEventArgs(List<string> newItem, List<string> removedItem)
        {
            if (newItem.Count > 0 && newItem != null)
            {
                NewItem = new List<string>(newItem);
            }

            if (removedItem.Count() > 0 && removedItem != null)
            {
                RemovedItem = new List<string>(new List<string>(removedItem));
            }
        }
    }
}