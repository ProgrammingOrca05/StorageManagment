using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using StorageManagement.Commands;
using StorageManagement.Data;
using StorageManagement.Models;
using StorageManagement.Views.dialogs;


namespace StorageManagement.ViewModels
{
    public class SalesTerminalViewModel : ViewModelBase
    {
        private string _searchQuery;
        private Product _selectedProduct;
        private TransactionItem _selectedCartItem;

        public const double VAT_RATE = 0.14;

        // Search query bound to the search box
        public string SearchQuery
        {
            get => _searchQuery;
            set { _searchQuery = value; OnPropertyChanged(); UpdateSearchResults(); }
        }

        // Product selected from search results
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set { _selectedProduct = value; OnPropertyChanged(); }
        }

        // Item selected in the cart
        public TransactionItem SelectedCartItem
        {
            get => _selectedCartItem;
            set { _selectedCartItem = value; OnPropertyChanged(); }
        }

        // Filtered search results
        public ObservableCollection<Product> SearchResults { get; set; } = new ObservableCollection<Product>();

        // Current cart
        public ObservableCollection<TransactionItem> Cart { get; set; } = new ObservableCollection<TransactionItem>();

        // Calculated fields
        public double Subtotal => Cart.Sum(item => item.TotalPrice);
        public double VAT => Subtotal * VAT_RATE;
        public double Total => Subtotal + VAT;

        public ICommand AddToCartCommand { get; }
        public ICommand RemoveFromCartCommand { get; }
        public ICommand CheckoutCommand { get; }

        public ICommand OpenCameraCommand { get; }

        public SalesTerminalViewModel()
        {
            foreach (var product in DataStore.Products)
                SearchResults.Add(product);
            AddToCartCommand = new RelayCommand(ExecuteAddToCart, o => SelectedProduct != null && SelectedProduct.Quantity > 0);
            RemoveFromCartCommand = new RelayCommand(ExecuteRemoveFromCart, o => SelectedCartItem != null);
            CheckoutCommand = new RelayCommand(ExecuteCheckout, o => Cart.Count > 0);
            OpenCameraCommand = new RelayCommand(_ =>
            {
                Process.Start(new ProcessStartInfo { 
                    FileName="microsoft.windows.camera:",
                    UseShellExecute=true});
            } );
        }

        // Filter products by name or SKU (using Id as SKU since Product has Id)
        private void UpdateSearchResults()
        {
            SearchResults.Clear();
            if (string.IsNullOrWhiteSpace(SearchQuery)) return;

            var results = DataStore.Products.Where(p =>
                p.Name.ToLower().Contains(SearchQuery.ToLower()) ||
                p.Id.ToString().Contains(SearchQuery));

            foreach (var product in results)
                SearchResults.Add(product);
        }

        private void ExecuteAddToCart(object parameter)
        {
            if (SelectedProduct == null || SelectedProduct.Quantity <= 0) return;

            // If product already in cart just increment quantity
            var existing = Cart.FirstOrDefault(i => i.Product.Id == SelectedProduct.Id);
            if (existing != null)
            {
                if (existing.Quantity < SelectedProduct.Quantity)
                    existing.Quantity++;
            }
            else
            {
                Cart.Add(new TransactionItem { Product = SelectedProduct, Quantity = 1 });
            }

            RefreshTotals();
        }

        private void ExecuteRemoveFromCart(object parameter)
        {
            if (SelectedCartItem == null) return;
            Cart.Remove(SelectedCartItem);
            RefreshTotals();
        }

        private void ExecuteCheckout(object parameter)
        {
            // Build the transaction
            var transaction = new Transaction
            {
                Id = DataStore.Transactions.Count + 1,
                Date = DateTime.Now,
                Items = Cart.ToList(),
                Subtotal = Subtotal,
                VAT = VAT,
                Total = Total
            };

            // Open checkout dialog
            var dialog = new CheckoutDialog(transaction);
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                // Deduct stock
                foreach (var item in Cart)
                    item.Product.Quantity -= item.Quantity;

                // Save transaction
                DataStore.Transactions.Add(transaction);

                // Clear cart
                Cart.Clear();
                RefreshTotals();
            }
        }

        // Notify UI to refresh totals
        private void RefreshTotals()
        {
            OnPropertyChanged(nameof(Subtotal));
            OnPropertyChanged(nameof(VAT));
            OnPropertyChanged(nameof(Total));
        }
    }
}
