using System.Windows;
using StorageManagement.Models;

namespace StorageManagement.Views.dialogs
{
    public partial class TransactionDetailDialog : Window
    {
        public TransactionDetailDialog(Transaction transaction)
        {
            InitializeComponent();
            DataContext = transaction;
        }
    }
}