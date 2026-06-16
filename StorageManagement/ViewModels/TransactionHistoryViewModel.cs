using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using StorageManagement.Commands;
using StorageManagement.Data;
using StorageManagement.Models;
using StorageManagement.Views.dialogs;

namespace StorageManagement.ViewModels
{
    public class TransactionHistoryViewModel : ViewModelBase
    {
        private ObservableCollection<Transaction> _transactions;
        public ObservableCollection<Transaction> Transactions
        {
            get => _transactions;
            set
            {
                _transactions = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Transaction> _filteredTransactions;
        public ObservableCollection<Transaction> FilteredTransactions
        {
            get => _filteredTransactions;
            set
            {
                _filteredTransactions = value;
                OnPropertyChanged();
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }

        private Transaction _selectedTransaction;
        public Transaction SelectedTransaction
        {
            get => _selectedTransaction;
            set
            {
                _selectedTransaction = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchCommand { get; }
        public ICommand ViewDetailsCommand { get; }

        public TransactionHistoryViewModel()
        {
            Transactions = DataStore.Transactions;
            FilteredTransactions = new ObservableCollection<Transaction>(Transactions);

            SearchCommand = new RelayCommand(_ => ApplyFilter());
            ViewDetailsCommand = new RelayCommand(
                _ => ViewDetails(),
                _ => CanViewDetails()
            );
        }

        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredTransactions = new ObservableCollection<Transaction>(Transactions);
                return;
            }

            string search = SearchText.Trim().ToLower();

            var result = Transactions.Where(t =>
                t.Id.ToString().ToLower().Contains(search) ||
                t.Date.ToString("g").ToLower().Contains(search) ||
                (!string.IsNullOrEmpty(t.PaymentMethod) &&
                 t.PaymentMethod.ToLower().Contains(search))
            ).ToList();

            FilteredTransactions = new ObservableCollection<Transaction>(result);
        }

        private bool CanViewDetails()
        {
            return SelectedTransaction != null;
        }

        private void ViewDetails()
        {
            if (SelectedTransaction == null)
            {
                MessageBox.Show("Choose the first invoice.");
                return;
            }

            var dialog = new TransactionDetailDialog(SelectedTransaction);
            dialog.ShowDialog();
        }
    }
}