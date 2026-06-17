using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StorageManagement.Models
{
    public class Product : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private string _category;
        private double _price;
        private int _quantity;

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string Category
        {
            get => _category;
            set { _category = value; OnPropertyChanged(); }
        }

        public double Price
        {
            get => _price;
            set { _price = value; OnPropertyChanged(); }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsLowStock));
                OnPropertyChanged(nameof(StatusText));
            }
        }

        public int LowStockThreshold { get; set; } = 5;

        public int OutOfStockThreshold { get; set; } = 0;

        public bool IsLowStock => Quantity <= LowStockThreshold && Quantity > OutOfStockThreshold;
        public bool IsOutOfStock => Quantity <= OutOfStockThreshold;
        public string StatusText => IsOutOfStock ? "✖ Out of Stock" : IsLowStock ? "⚠ Low Stock" : "✓ In Stock";

        public Product() { }

        public Product(int id, string name, string category, double price, int quantity)
        {
            _id = id;
            _name = name;
            _category = category;
            _price = price;
            _quantity = quantity;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}