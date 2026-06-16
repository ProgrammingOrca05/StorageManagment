using System.Collections.ObjectModel;
using System.Windows.Input;
using StorageManagement.Models;
using StorageManagement.Commands;
using System.Linq;

namespace StorageManagement.ViewModels
{
    public class SalesTerminalViewModel : ViewModelBase
    {
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Product> FilteredProducts { get; set; }
        public ObservableCollection<TransactionItem> CartItems { get; set; }

        public string SearchText { get; set; }

        public Product SelectedProduct { get; set; }
        public TransactionItem SelectedCartItem { get; set; }

        public ICommand AddToCartCommand { get; set; }
        public ICommand RemoveFromCartCommand { get; set; }
        public ICommand ClearCartCommand { get; set; }
        public ICommand CheckoutCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        public SalesTerminalViewModel()
        {
            Products = new ObservableCollection<Product>();
            FilteredProducts = new ObservableCollection<Product>();
            CartItems = new ObservableCollection<TransactionItem>();

            AddToCartCommand = new RelayCommand(AddToCart);
            RemoveFromCartCommand = new RelayCommand(RemoveFromCart);
            ClearCartCommand = new RelayCommand(ClearCart);
            CheckoutCommand = new RelayCommand(Checkout);
            SearchCommand = new RelayCommand(Search);

            LoadProducts();
        }

        private void LoadProducts()
        {
            foreach (var p in Products)
                FilteredProducts.Add(p);
        }

        private void Search(object obj)
        {
            FilteredProducts.Clear();

            var result = Products
                .Where(p => p.Name.ToLower().Contains(SearchText?.ToLower() ?? ""));

            foreach (var item in result)
                FilteredProducts.Add(item);
        }

        private void AddToCart(object obj)
        {
            var product = obj as Product;
            if (product == null) return;

            var existing = CartItems.FirstOrDefault(x => x.ProductName == product.Name);

            if (existing != null)
                existing.Quantity++;
            else
                CartItems.Add(new TransactionItem
                {
                    ProductName = product.Name,
                    UnitPrice = (decimal)product.Price,
                    Quantity = 1
                });

            OnPropertyChanged(nameof(SubTotal));
            OnPropertyChanged(nameof(Tax));
            OnPropertyChanged(nameof(Total));
        }

        private void RemoveFromCart(object obj)
        {
            if (SelectedCartItem != null)
                CartItems.Remove(SelectedCartItem);

            OnPropertyChanged(nameof(SubTotal));
            OnPropertyChanged(nameof(Tax));
            OnPropertyChanged(nameof(Total));
        }

        private void ClearCart(object obj)
        {
            CartItems.Clear();
            OnPropertyChanged(nameof(SubTotal));
            OnPropertyChanged(nameof(Tax));
            OnPropertyChanged(nameof(Total));
        }

        private void Checkout(object obj)
        {
            CartItems.Clear();
        }

        public decimal SubTotal => CartItems.Sum(x => x.TotalPrice);
        public decimal Tax => SubTotal * 0.14m;
        public decimal Total => SubTotal + Tax;
    }
}