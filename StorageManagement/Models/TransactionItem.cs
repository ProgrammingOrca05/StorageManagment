using System.ComponentModel;

namespace StorageManagement.Models
{
    public class TransactionItem : INotifyPropertyChanged
    {
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Quantity)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalPrice)));
            }
        }

        public decimal TotalPrice => UnitPrice * Quantity;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}