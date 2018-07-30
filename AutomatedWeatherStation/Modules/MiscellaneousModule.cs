using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using AutomatedWeatherStation.DataAccess;
using AutomatedWeatherStation.DataAccess.EF;
using AutomatedWeatherStation.Models.GsmCommModels;
using AutomatedWeatherStation.Watcher;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GsmComm.GsmCommunication;
using Nito.AsyncEx;

namespace AutomatedWeatherStation.Modules
{
    public class MiscellaneousModule : ObservableObject
    {
        private readonly AsyncProducerConsumerQueue<Task> _asyncQueue = new AsyncProducerConsumerQueue<Task>();
        private readonly AsyncLock _dequeuingLock = new AsyncLock();
        private readonly object _gsmListLock = new object();
        private readonly object _logListLock = new object();
        private readonly object _messageListLock = new object();

        private readonly AsyncLock _messageReceivedLock = new AsyncLock();

        private readonly IRepository _repository;
        private bool _isLoadingDevices;
        private GsmCommModel _selectedGsm;
        private SerialPortWatcher _watcher;


        public MiscellaneousModule(IRepository repository)
        {
            _repository = repository;
        }

        public INotifyTaskCompletion InitializeLoading { get; private set; }

        public ObservableCollection<GsmCommModel> GsmList { get; } = new ObservableCollection<GsmCommModel>();

        public ObservableCollection<MessageViewModel> MessageList { get; } =
            new ObservableCollection<MessageViewModel>();

        public INotifyTaskCompletion DeviceLoading { get; private set; }

        public ICommand RefreshDeviceList => new RelayCommand(RefreshDeviceProc, RefreshDeviceCondition);

        public ObservableCollection<string> LogList { get; } = new ObservableCollection<string>();

        public ICommand ClearMessageCommand => new RelayCommand(ClearMessageProc);

        public ICommand ClearLogCommand => new RelayCommand(ClearLogProc);

        public GsmCommModel SelectedGsm
        {
            get { return _selectedGsm; }
            set
            {
                _selectedGsm = value;
                RaisePropertyChanged(nameof(SelectedGsm));
            }
        }

        public void Initialize()
        {
            InitializeLoading = NotifyTaskCompletion.Create(InitializeAsync());
        }

        private bool RefreshDeviceCondition()
        {
            return !_isLoadingDevices;
        }

        private async Task InitializeAsync()
        {
            LogList.Add("Initializing...");
            BindingOperations.EnableCollectionSynchronization(MessageList, _messageListLock);
            BindingOperations.EnableCollectionSynchronization(GsmList, _gsmListLock);
            BindingOperations.EnableCollectionSynchronization(LogList, _logListLock);
            _watcher = new SerialPortWatcher();
            _watcher.PortsChanged += WatcherOnPortsChanged;
            if (_watcher.ComPorts.Count == 0)
            {
                LogList.Add("No Available Device");
                return;
            }
            await Task.Run(() => ReadDeviceList(_watcher.ComPorts));
        }

        private async Task RefreshDeviceProcAsync()
        {
            _watcher.Dispose();
            await Task.Run(() => CloseDeviceList(GsmList));
            GsmList.Clear();

            //DeviceLoading =
            //    NotifyTaskCompletion.Create(
            //        Task.WhenAll(_watcher.ComPorts.Select(async item => await AddNewGsmComm(item)).ToList()));
            await Task.Run(() => ReadDeviceList(_watcher.ComPorts));
        }

        private async void RefreshDeviceProc()
        {
            _isLoadingDevices = true;
            LogList.Add("Reloading Devices");
            await RefreshDeviceProcAsync();
            _isLoadingDevices = false;
        }

        private async Task MessageReceiveAsync(object sender, MessageReceivedEventArgs e)
        {
            using (await _messageReceivedLock.LockAsync())
            {
                var gsm = GsmList.FirstOrDefault(c => c.Model == (GsmCommMain)sender);
                var obj = e.IndicationObject;
                var loc = (MemoryLocation)obj;
                if (gsm == null)
                    return;

                var messages =
                    await Task.Run(() => gsm.GetMessagesAsync(PhoneMessageStatus.ReceivedUnread, loc.Storage));

                foreach (var message in messages)
                {
                    if (MessageList.Contains(message))
                    {
                        return;
                    }
                    MessageList.Insert(0, message);
                    EnqueueMessage(message);
                }
            }
        }

        private async void EnqueueMessage(MessageViewModel message)
        {
            await _asyncQueue.EnqueueAsync(ProcessMessage(message));

            using (await _dequeuingLock.LockAsync())
            {
                while (true)
                {
                    var dequeueResult = await _asyncQueue.TryDequeueAsync();
                    if (!dequeueResult.Success)
                        break;
                }
            }
        }

        private async void comm_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            //await MessageReceiveAsync(sender, e).ConfigureAwait(false);
            await MessageReceiveAsync(sender, e);
        }

        private void ManualAddtoDatabase(ObservableCollection<string> list)
        {
            foreach (var item in list)
            {
                var Fields = item.Split(',');
                for (var i = 0; i < Fields.Length; i++)
                {
                    Fields[i] = Fields[i].TrimStart('(', '"');
                    Fields[i] = Fields[i].TrimEnd(')', '"');
                }

                try
                {
                    var dt = DateTime.ParseExact(Fields[1] + " " + Fields[2], "yyyy/MM/dd HH:mm:ss",
                        CultureInfo.InvariantCulture);
                    var m = new Measurement();

                    m.StationId = 1;
                    m.Date = dt;
                    m.Temperature = DecimalChecker(Fields[3]);
                    m.Humidity = DecimalChecker(Fields[4]);
                    m.Pressure = DecimalChecker(Fields[5]);
                    m.Rainfall24H = DecimalChecker(Fields[6]);
                    m.Rainfall15M = DecimalChecker(Fields[7]);
                    m.WindDirection = DecimalChecker(Fields[8]);
                    m.WindSpeed = DecimalChecker(Fields[9]);
                    m.SolarIrradianceM = DecimalChecker(Fields[10]);
                    m.SolarIrradianceH = DecimalChecker(Fields[11]);
                    m.SignalLevel = DecimalChecker(Fields[12]);
                    m.Battery = DecimalChecker(Fields[13]);

                    _repository.Measurements.Add(m);
                }
                catch (Exception)
                {
                }
            }
        }

        private async Task ProcessMessage(MessageViewModel message)
        {
            await Task.Delay(1000);

            var messageType = MessageDecoder(message);

            if (messageType == MessageType.None)
                LogList.Insert(0, "Garbage Message: " + message.MessageData.UserDataText);

            if (messageType == MessageType.WeatherData)
            {
                //(013226007252592,2017/03/06,00:14:59,25.0,89.0,993.4,0.05,0.00,270,0.88,  0.00,  0.00,-52,11.57)
                //(IMEI,DATE,TIME,TEMP,HUM,PRES,RAIN24H,RAIN15M,WDIR,WSPEED15,IRRMIN,IRRHR,SIGNAL,BATTERY)
                var fields = message.MessageData.UserDataText.Split(',');
                for (var i = 0; i < fields.Length; i++)
                    fields[i] = fields[i].Trim('(', ')');

                var list = fields.ToList();
                try
                {
                    var station =
                        await Task.Run(() => _repository.Stations.GetAsync(c => c.Imei == list.FirstOrDefault()));
                    if (station == null)
                    {
                        LogList.Insert(0, "Imei was not recognized for message:" + message.MessageData.UserDataText);
                        return;
                    }
                    var dt = DateTime.ParseExact(list.ElementAt(1) + " " + list.ElementAt(2), "yyyy/MM/dd HH:mm:ss",
                        CultureInfo.InvariantCulture);

                    var records = await
                        Task.Run(
                            () =>
                                _repository.Measurements.GetRangeAsync(
                                    c => c.StationId == station.StationId && c.Date == dt));
                    if (records.Count == 0)
                    {
                        LogList.Insert(0,
                            "Data is already existing in the system for message:" + message.MessageData.UserDataText);
                        return;
                    }
                    await Task.Run(() => _repository.Measurements.AddAsync(new Measurement
                    {
                        StationId = station.StationId,
                        Date = dt,
                        Temperature = DecimalChecker(list.ElementAt(3)),
                        Humidity = DecimalChecker(list.ElementAt(4)),
                        Pressure = DecimalChecker(list.ElementAt(5)),
                        Rainfall24H = DecimalChecker(list.ElementAt(6)),
                        Rainfall15M = DecimalChecker(list.ElementAt(7)),
                        WindDirection = DecimalChecker(list.ElementAt(8)),
                        WindSpeed = DecimalChecker(list.ElementAt(9)),
                        SolarIrradianceM = DecimalChecker(list.ElementAt(10)),
                        SolarIrradianceH = DecimalChecker(list.ElementAt(11)),
                        SignalLevel = DecimalChecker(list.ElementAt(12)),
                        Battery = DecimalChecker(list.ElementAt(13))
                    }));
                    LogList.Insert(0, "Succefully Added Data: " + message.MessageData.UserDataText);
                }
                catch (Exception)
                {
                    LogList.Insert(0,
                        "Unsuccessful data addition. Format is not acceptable for message: " +
                        message.MessageData.UserDataText);
                }
            }
            if (messageType == MessageType.WeatherRequest)
            {
                var keyword = new List<string>(message.MessageData.UserDataText.Split(' ')).LastOrDefault();

                try
                {
                    var station = await Task.Run(() => _repository.Stations.GetAsync(c => c.Keyword == keyword));

                    if (station == null)
                    {
                        LogList.Insert(0, "Keyword does not exist: " + message.MessageData.UserDataText);
                        return;
                    }

                    var measurements =
                        await
                            Task.Run(() => _repository.Measurements.GetRangeAsync(c => c.StationId == station.StationId));
                    if (measurements.Count == 0)
                    {
                        LogList.Insert(0,
                            "There still no data available in this station: " + message.MessageData.UserDataText);
                        return;
                    }
                    var measurement = measurements.OrderByDescending(c => c.Date).FirstOrDefault();
                    var gsm = GsmList.FirstOrDefault(c => c.Model.PortName == message.Port);
                    var sms = "As of " + measurement.Date.ToString("yy-mm-dd") + " Temperature: " +
                              measurement.Temperature + "°C, Humidity: " +
                              measurement.Humidity + "%, Wind: " + measurement.WindDirection + "° " +
                              measurement.WindSpeed + "m/s, Rainfall: " + measurement.Rainfall15M + "mm, Pressure" +
                              measurement.Pressure + " hPa, Solar Irradiance: " + measurement.SolarIrradianceM;
                    if (gsm != null)
                    {
                        await
                            Task.Run(
                                () =>
                                    gsm.SendMessageASync(sms, message.PhoneNumber, false, false,
                                        1));
                        LogList.Insert(0, "Request succesfull: " + message.MessageData);
                    }
                }
                catch (Exception)
                {
                }
            }
            message.IsProcessed = true;
            await DeleteMessage(message);
        }

        private MessageType MessageDecoder(MessageViewModel item)
        {
            var message = item.Model.Data.UserDataText;

            var type = MessageType.None;

            if (message.StartsWith("AWS "))
                return MessageType.WeatherRequest;

            if (message.StartsWith("(") && message.EndsWith(")"))
            {
                try
                {
                    var Fields = message.Split(',');

                    if (Fields.Length == 14)
                        return MessageType.WeatherData;
                }
                catch (Exception)
                {
                }
            }
            return type;
        }

        private async Task DeleteMessage(MessageViewModel message)
        {
            var gsm = GsmList.FirstOrDefault(c => c.Model.PortName == message.Port);
            if (gsm == null)
                return;

            while (true)
            {
                try
                {
                    await Task.Run(() => gsm.DeleteMessageAsync(message));
                    break;
                }
                catch (CommException e)
                {
                    await Task.Delay(1000);
                }
            }
        }

        private async Task ReadDeviceList(List<string> list)
        {
            _isLoadingDevices = true;
            foreach (var comPort in list)
            {
                await Task.Run(() => AddNewGsmComm(comPort));
            }
            _isLoadingDevices = false;
        }

        private async void WatcherOnPortsChanged(object sender, OnPortsChangedEventArgs e)
        {
            if (e.NewItem.Count > 0 && e.NewItem != null)
            {
                if (e.NewItem.Count == 1)
                {
                    LogList.Insert(0, e.NewItem.Count + " New Device Detected...");
                }
                else
                {
                    LogList.Insert(0, e.NewItem.Count + " New Devices Detected...");
                }
                await Task.Run(() => ReadDeviceList(e.NewItem));

                //await Task.WhenAll(_watcher.ComPorts.Select(async item => await AddNewGsmComm(item)).ToList());
            }

            if (e.RemovedItem.Count > 0 && e.RemovedItem != null)
            {
                foreach (var item in e.RemovedItem)
                    try
                    {
                        foreach (var gsm in GsmList.Where(gsm => gsm.Model.PortName == item))
                        {
                            GsmList.Remove(gsm);
                            LogList.Insert(0, "Removing Device " + gsm.Model.PortName);
                        }
                    }
                    catch (Exception)
                    {
                    }
            }
        }

        private void ClearMessageProc()
        {
            MessageList.Clear();
        }

        private void ClearLogProc()
        {
            LogList.Clear();
        }

        public async void CloseWindow()
        {
            try
            {
                _watcher.Dispose();
            }
            catch (Exception)
            {
            }
            await Task.Run(() => CloseDeviceList(GsmList));
        }

        private async Task CloseDeviceList(ObservableCollection<GsmCommModel> list)
        {
            foreach (var item in list)
            {
                await Task.Run(() => item.CloseAsync());
            }
        }

        private async Task AddNewGsmComm(string item)
        {
            for (var i = 0; i < 5; i++)
            {
                var port = new SerialPort(item);
                var gsm = new GsmCommModel(new GsmCommMain(port.PortName, port.BaudRate, 1000));
                await Task.Run(() => gsm.CloseAsync());
                if (!await Task.Run(() => gsm.IsOpenAsync()))
                    await Task.Run(() => gsm.OpenAsync());

                //if (!await Task.Run(() => gsm.IsConnectedAsync()) || !await Task.Run(() => gsm.IsOpenAsync()))
                //    return;

                //if (!await Task.Run(() => gsm.UsableDeviceTest()))
                //    return;

                gsm.Model.MessageReceived += comm_MessageReceived;
                gsm.InitializeGsm();
                //_addNumberWindow.Show();
                GsmList.Add(gsm);
                LogList.Add(gsm.Model.PortName + " Device is added...");
                try
                {
                    var messages = await Task.Run(() => gsm.GetUnsavedMessages());
                    foreach (var message in messages)
                    {
                        MessageList.Insert(0, message);
                        EnqueueMessage(message);
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
                await gsm.EnableMessageNotificationAsync();
                break;
            }
        }

        private decimal? DecimalChecker(string s)
        {
            var numericChars = "-0123456789.".ToCharArray();
            var str = new string(s.Where(c => numericChars.Any(n => n == c)).ToArray());
            decimal number;
            if (decimal.TryParse(str, out number))
            {
                return number;
            }
            return null;
        }

        private enum MessageType
        {
            None,
            WeatherData,
            WeatherRequest
        }
    }
}