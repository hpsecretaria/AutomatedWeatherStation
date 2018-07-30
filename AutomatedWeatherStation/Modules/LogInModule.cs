using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AutomatedWeatherStation.DataAccess;
using AutomatedWeatherStation.DataAccess.EF;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace AutomatedWeatherStation.Modules
{
    public class LogInModule : ObservableObject
    {
        private readonly IRepository _repository;
        private bool _allAccess;
        private string _error;
        private bool _isLoggingIn;
        private User _logInUser;
        private string _password;
        private string _userName;

        public LogInModule(IRepository repository)
        {
            _repository = repository;
        }

        public ICommand LogInCommand => new RelayCommand(LogInProc);

        public ICommand LogOutCommad => new RelayCommand(LogOutProc);

        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                RaisePropertyChanged(nameof(Error));
            }
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                RaisePropertyChanged(nameof(UserName));
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged(nameof(Password));
            }
        }

        public bool AllAccess
        {
            get { return _allAccess; }
            set
            {
                _allAccess = value;
                RaisePropertyChanged(nameof(AllAccess));
            }
        }

        public User LogInUser
        {
            get { return _logInUser; }
            set
            {
                _logInUser = value;

                RaisePropertyChanged(nameof(LogInUser));
            }
        }

        public bool IsLoggingIn
        {
            get { return _isLoggingIn; }
            set
            {
                _isLoggingIn = value;
                RaisePropertyChanged(nameof(IsLoggingIn));
            }
        }

        private void LogOutProc()
        {
            ViewModelLocatorStatic.Locator.UnloadModules();
            LogInUser = null;
        }

        private async void LogInProc()
        {
            if (IsLoggingIn)
            {
                return;
            }
            IsLoggingIn = true;
            var user =
                await Task.Run(() => _repository.Users.GetRangeAsync(c => c.Name == UserName && c.Password == Password));

            if (user.FirstOrDefault() == null)
            {
                Error = "Username or Password is not recognized";
            }
            else
            {
                Error = "Log In Successfull";
                await Task.Delay(1000);
                LogInUser = user.FirstOrDefault();
                Error = "";
                ViewModelLocatorStatic.Locator.LoadModules();
                if (LogInUser.Rights == 0)
                {
                    AllAccess = true;
                }
            }
            IsLoggingIn = false;
        }
    }
}