using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorageManagement.Commands;
using System.Windows.Input;
using StorageManagement.ViewModels;
using StorageManagement.Views.windows;


namespace StorageManagement.ViewModels
{
    public class LoginViewModel:ViewModelBase
    {
        // Hardcoded credentials
        private const string ValidUsername = "cashier";
        private const string ValidPassword = "cashier_2014";

        private string _username;
        private string _password;
        private string _errorMessage;

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        // Login button is only enabled when both fields are filled
        private bool CanExecuteLogin(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(Password);
        }

        private void ExecuteLogin(object parameter)
        {
            // Validation checks
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter username and password.";
                return;
            }

            if (Username.Length < 3)
            {
                ErrorMessage = "Username must be at least 3 characters.";
                return;
            }

            if (Password.Length < 8)
            {
                ErrorMessage = "Password must be at least 8 characters.";
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(Password, @"\d"))
            {
                ErrorMessage = "Password must contain at least one number.";
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(Password, @"[^a-zA-Z0-9]"))
            {
                ErrorMessage = "Password must contain at least one special character.";
                return;
            }

            // Actual credential check
            if (Username == ValidUsername && Password == ValidPassword)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                App.Current.Windows[0].Close();
            }
            else
            {
                ErrorMessage = "Invalid username or password.";
            }
        }
    }
}
