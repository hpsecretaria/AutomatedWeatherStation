using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GsmComm.GsmCommunication;
using GsmComm.PduConverter;

namespace AutomatedWeatherStation.Models.GsmCommModels
{ 
    public class GsmCommModel : ViewModelBase<GsmCommMain> { 
        private int _signalStrength;
        private string _phoneNumber;
        private SignalQualityModel _signalQuality;

        public GsmCommModel(GsmCommMain model) : base(model)
        {
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                RaisePropertyChanged(nameof(PhoneNumber));
            }
        }


        public SignalQualityModel SignalQuality
        {
            get { return _signalQuality; }
            private set
            {
                _signalQuality = value;
                RaisePropertyChanged(nameof(SignalQuality));
            }
        }

        public int SignalStrength
        {
            get { return _signalStrength; }
            private set
            {
                _signalStrength = value;
                RaisePropertyChanged(nameof(SignalStrength));
            }
        }

        public async Task<bool> IsOpenAsync()
        {
            var tcs = new TaskCompletionSource<bool>();
            await Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        var b = Model.IsOpen();
                        tcs.SetResult(b);
                        break;
                    }
                    catch (CommException)
                    {
                    }
                    catch (Exception)
                    {
                    }
                }
            });

            return tcs.Task.Result;
        }

        public async Task<bool> IsConnectedAsync()
        {
            var tcs = new TaskCompletionSource<bool>();
            await Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        var b = Model.IsConnected();
                        tcs.SetResult(b);
                        break;
                    }
                    catch (CommException)
                    {
                    }
                    catch (Exception)
                    {
                    }
                }
            });

            return tcs.Task.Result;
        }

        public void InitializeGsm()
        {
            try
            {
                SignalQuality = new SignalQualityModel(Model.GetSignalQuality());
                MessageBox.Show(SignalQuality.Model.SignalStrength.ToString());
            }
            catch (CommException)
            {
            }
            catch (Exception)
            {
            }
        }

        public async Task SendMessageASync(string textmessage, string destinationnumber, bool alert, bool unicode,
            int times)
        {
            // Send an SMS message
            SmsSubmitPdu pdu;


            if (!alert && !unicode)
            {
                // The straightforward version
                pdu = new SmsSubmitPdu
                    (textmessage, destinationnumber, "");
            }
            else
            {
                // The extended version with dcs
                byte dcs;
                if (!alert && unicode)
                    dcs = DataCodingScheme.NoClass_16Bit;
                else if (alert && !unicode)
                    dcs = DataCodingScheme.Class0_7Bit;
                else if (alert && unicode)
                    dcs = DataCodingScheme.Class0_16Bit;
                else
                    dcs = DataCodingScheme.NoClass_7Bit;

                pdu = new SmsSubmitPdu
                    (textmessage, destinationnumber, "", dcs);
            }

            await Task.Run(() =>
            {
                while (times > 0)
                {
                    try
                    {
                        Model.SendMessage(pdu);
                        times--;
                    }
                    catch (CommException)
                    {
                    }
                    catch (Exception)
                    {
                    }
                }
            });
        }

        private async Task<SignalQualityInfo> GetSignalQualityAsync()
        {
            var tcs = new TaskCompletionSource<SignalQualityInfo>();
            await Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        var signalquality = Model.GetSignalQuality();
                        tcs.SetResult(signalquality);
                    }
                    catch (CommException)
                    {
                    }
                    catch (Exception)
                    {
                    }
                }
            });
            return tcs.Task.Result;
        }

        public async Task<MemoryStatus> GetMessageMemoryStatusAsync(string storage)
        {
            var tcs = new TaskCompletionSource<MemoryStatus>();
            await Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        var memorystatus = Model.GetMessageMemoryStatus(storage);
                        tcs.SetResult(memorystatus);
                    }
                    catch (CommException)
                    {
                    }
                    catch (Exception)
                    {
                    }
                }
            });
            return tcs.Task.Result;
        }

        public async Task EnableMessageNotificationAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    Model.EnableMessageNotifications();
                }
                catch (CommException)
                {
                }
                catch (Exception)
                {
                }
            });
        }


        public async Task<bool> UsableDeviceTest()
        {
            var tcs = new TaskCompletionSource<bool>();
            await Task.Run(() =>
            {
                try
                {
                    Model.EnableMessageNotifications();
                    tcs.SetResult(true);
                }
                catch (CommException)
                {
                    tcs.SetResult(false);
                }
                catch (Exception)
                {
                    tcs.SetResult(false);
                }
            });

            return tcs.Task.Result;
        }


        public async Task OpenAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    Model.Open();
                }
                catch (CommException)
                {
                }
                catch (Exception)
                {
                }
            });
        }

        public async Task CloseAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    Model.Close();
                }
                catch (CommException)
                {
                }
                catch (Exception)
                {
                }
            });
        }

        public async Task DeleteMessagesAsync(List<MessageViewModel> messages)
        {
            await Task.WhenAll(messages.Select(message => DeleteMessageAsync(message)).ToList());
        }

        public async Task DeleteMessageAsync(MessageViewModel message)
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        Model.DeleteMessage(message.Model.Index, message.Model.Storage);
                        break;
                    }
                    catch (CommException)
                    {
                        Task.Delay(1000);
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }
            });
        }

        public async Task<List<MessageViewModel>> GetMessagesAsync(PhoneMessageStatus status, string storage)
        {
            var tcs = new TaskCompletionSource<DecodedShortMessage[]>();

            await Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        var messages = Model.ReadMessages(status, storage);
                        tcs.SetResult(messages);
                        break;
                    }
                    catch (CommException)
                    {
                        Task.Delay(1000);
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }
            });

            return tcs.Task.Result.Select(c =>
                new MessageViewModel(c, Model.PortName, storage,
                    PhoneMessageStatus.ReceivedRead)).ToList();
        }

        public async Task<List<MessageViewModel>> GetUnsavedMessages()
        {
            var list = new List<MessageViewModel>();

            list.AddRange(await GetMessagesAsync(PhoneMessageStatus.ReceivedUnread, PhoneStorageType.Phone));
            list.AddRange(await GetMessagesAsync(PhoneMessageStatus.ReceivedRead, PhoneStorageType.Phone));
            list.AddRange(await GetMessagesAsync(PhoneMessageStatus.ReceivedUnread, PhoneStorageType.Sim));
            list.AddRange(await GetMessagesAsync(PhoneMessageStatus.ReceivedRead, PhoneStorageType.Sim));

            return list;
        }

        public void EnableMessageReceive()
        {
            //Model.MessageReceived += comm_MessageReceived;
        }
    }

    public class SignalQualityModel : ViewModelBase<SignalQualityInfo>
    {
        private Color _color;
        private int _dBm;

        public SignalQualityModel(SignalQualityInfo model) : base(model)
        {
            DBm = DecodeSignal(Model.SignalStrength);
            Color = DecodeColor(Model.SignalStrength);
        }

        public SignalQualityModel(int value) : base(new SignalQualityInfo(0, 0))
        {
            DBm = value;
            Color = DecodeColor(value);
        }

        public Color Color
        {
            get { return _color; }
            private set
            {
                _color = value;
                RaisePropertyChanged(nameof(Color));
            }
        }

        public int DBm
        {
            get { return _dBm; }
            private set
            {
                _dBm = value;
                RaisePropertyChanged(nameof(DBm));
            }
        }

        private int DecodeSignal(int strength)
        {
            var values = Enumerable.Range(-109, -52).Where(i => i%2 != 0);

            var index = strength - 2;

            var value = values.ElementAt(index);

            return value;
        }

        private Color DecodeColor(int strength)
        {
            if (strength >= 2 && strength <= 9)
            {
                return Colors.Red;
            }
            if (strength >= 10 && strength <= 14)
            {
                return Colors.Yellow;
            }
            if (strength >= 15 && strength <= 19)
            {
                return Colors.YellowGreen;
            }
            if (strength >= 20 && strength <= 30)
            {
                return Colors.Green;
            }
            return Colors.Red;
        }
    }

    public class MessageViewModel : ViewModelBase<DecodedShortMessage>
    {
        private bool _isProcessed;
        private bool _isSaved;

        public MessageViewModel(DecodedShortMessage model, string port) : base(model)
        {
            MessageData = (SmsDeliverPdu) Model.Data;
            PhoneNumber = MessageData.OriginatingAddress;
            Port = port;
        }

        public MessageViewModel(DecodedShortMessage model, string port, string location) : base(model)
        {
            MessageData = (SmsDeliverPdu) Model.Data;
            Location = location;
            PhoneNumber = MessageData.OriginatingAddress;
            Port = port;
        }

        public MessageViewModel(DecodedShortMessage model, string port, string location, PhoneMessageStatus status)
            : base(model)
        {
            MessageData = (SmsDeliverPdu) Model.Data;
            Status = status;
            Port = port;
            PhoneNumber = MessageData.OriginatingAddress;
            Location = location == "SM" ? "SIM" : "MEMORY";
        }

        public PhoneMessageStatus Status { get; set; }

        public string Location { get; private set; }

        public string Port { get; private set; }

        public SmsDeliverPdu MessageData { get; }

        public string PhoneNumber { get; private set; }

        public bool IsSaved
        {
            get { return _isSaved; }
            set
            {
                _isSaved = value;
                RaisePropertyChanged(nameof(IsSaved));
            }
        }

        public bool IsProcessed
        {
            get { return _isProcessed; }
            set
            {
                _isProcessed = value;
                RaisePropertyChanged(nameof(IsProcessed));
            }
        }
    }
}