using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using StorageManagement.Commands;
using StorageManagement.ViewModels;

namespace StorageManagement.ViewModels
{
    public class MainViewModel:ViewModelBase
    {
        private object _currentView;

        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        // Navigation commands
        public ICommand ShowInventoryCommand { get; }
        public ICommand ShowSalesTerminalCommand { get; }
        public ICommand ShowTransactionHistoryCommand { get; }

        public MainViewModel()
        {
            // Initialize child viewmodels
            var inventoryVM = new InventoryViewModel();
            var salesVM = new SalesTerminalViewModel();
            var historyVM = new TransactionHistoryViewModel();

            // Set up navigation commands
            ShowInventoryCommand = new RelayCommand(o => CurrentView = inventoryVM);
            ShowSalesTerminalCommand = new RelayCommand(o => CurrentView = salesVM);
            ShowTransactionHistoryCommand = new RelayCommand(o => CurrentView = historyVM);

            // Default view on login
            CurrentView = inventoryVM;
        }
    }
}
