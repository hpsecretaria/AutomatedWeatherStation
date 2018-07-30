using System;
using System.Windows;
using System.Windows.Input;
using AutomatedWeatherStation.Modules;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace AutomatedWeatherStation
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private CustomDialog _customDialog;
        // Login1 _loginwindow;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void TabItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;
            _customDialog = new CustomDialog();
            var mySettings = new MetroDialogSettings
            {
                AffirmativeButtonText = "OK",
                AnimateShow = true,
                NegativeButtonText = "Go away!",
                FirstAuxiliaryButtonText = "Cancel"
            };
            //_loginwindow = new Login1();
            //_loginwindow.ButtonCancel.Click += ButtonCancelOnClick;
            //_loginwindow.ButtonLogin.Click += ButtonLoginOnClick;
            //_customDialog.Content = _loginwindow;
            await this.ShowMetroDialogAsync(_customDialog);
        }

        private void ButtonLoginOnClick(object sender, RoutedEventArgs e)
        {
            //if (_loginwindow.TextBoxUserName.Text == "admin" && _loginwindow.PasswordBox1.Password == "admin")
            //{
            //    this.HideMetroDialogAsync(_customDialog);
            //}
            //else
            //{
            //    MessageBox.Show("Invallid Username or Password");
            //}
        }

        private void ButtonCancelOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            try
            {
                // ViewModelLocatorStatic.Locator.StationDependency.Stop();
            }
            catch (Exception)
            {
            }
            try
            {
                ViewModelLocatorStatic.Locator.MiscellaneousModule.CloseWindow();
            }
            catch (Exception)
            {
            }
        }
    }
}