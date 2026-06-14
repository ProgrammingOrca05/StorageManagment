using StorageManagement.Data;
using StorageManagement.Models;
using StorageManagement.Commands;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace StorageManagement.ViewModels
{
    public class InventoryViewModel : ViewModelBase
    {
        // ==================== Properties ====================

        public ObservableCollection<Product> Products => DataStore.Products;

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
                LoadSelectedProduct();
            }
        }

        private string _productName;
        public string ProductName
        {
            get => _productName;
            set { _productName = value; OnPropertyChanged(); }
        }

        private string _productCategory;
        public string ProductCategory
        {
            get => _productCategory;
            set { _productCategory = value; OnPropertyChanged(); }
        }

        private string _productPrice;
        public string ProductPrice
        {
            get => _productPrice;
            set { _productPrice = value; OnPropertyChanged(); }
        }

        private string _productQuantity;
        public string ProductQuantity
        {
            get => _productQuantity;
            set { _productQuantity = value; OnPropertyChanged(); }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FilteredProducts));
            }
        }

        public ObservableCollection<Product> FilteredProducts
        {
            get
            {
                if (string.IsNullOrWhiteSpace(SearchText))
                    return DataStore.Products;

                var filtered = new ObservableCollection<Product>();
                foreach (var p in DataStore.Products)
                {
                    if (p.Name.ToLower().Contains(SearchText.ToLower()) ||
                        p.Category.ToLower().Contains(SearchText.ToLower()))
                        filtered.Add(p);
                }
                return filtered;
            }
        }

        // ==================== Commands ====================

        public ICommand AddProductCommand { get; }
        public ICommand UpdateProductCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public ICommand ClearFormCommand { get; }

        // ==================== Constructor ====================

        public InventoryViewModel()
        {
            AddProductCommand = new RelayCommand(_ => AddProduct());
            UpdateProductCommand = new RelayCommand(_ => UpdateProduct(), _ => SelectedProduct != null);
            DeleteProductCommand = new RelayCommand(_ => DeleteProduct(), _ => SelectedProduct != null);
            ClearFormCommand = new RelayCommand(_ => ClearForm());
        }

        // ==================== Methods ====================

        private void LoadSelectedProduct()
        {
            if (SelectedProduct == null) return;
            ProductName = SelectedProduct.Name;
            ProductCategory = SelectedProduct.Category;
            ProductPrice = SelectedProduct.Price.ToString();
            ProductQuantity = SelectedProduct.Quantity.ToString();
        }

        private void AddProduct()
        {
            if (!ValidateForm()) return;

            var product = new Product(
                DataStore.GetNextProductId(),
                ProductName,
                ProductCategory,
                double.Parse(ProductPrice),
                int.Parse(ProductQuantity)
            );

            DataStore.Products.Add(product);
            OnPropertyChanged(nameof(FilteredProducts));
            ClearForm();
            MessageBox.Show("Product added successfully!", "Success",
                            MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void UpdateProduct()
        {
            if (SelectedProduct == null || !ValidateForm()) return;

            int index = DataStore.Products.IndexOf(SelectedProduct);

            SelectedProduct.Name = ProductName;
            SelectedProduct.Category = ProductCategory;
            SelectedProduct.Price = double.Parse(ProductPrice);
            SelectedProduct.Quantity = int.Parse(ProductQuantity);

            DataStore.Products[index] = SelectedProduct;
            OnPropertyChanged(nameof(FilteredProducts));
            ClearForm();
            MessageBox.Show("Product updated successfully!", "Success",
                            MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DeleteProduct()
        {
            if (SelectedProduct == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete '{SelectedProduct.Name}'?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                DataStore.Products.Remove(SelectedProduct);
                OnPropertyChanged(nameof(FilteredProducts));
                ClearForm();
            }
        }

        private void ClearForm()
        {
            SelectedProduct = null;
            ProductName = string.Empty;
            ProductCategory = string.Empty;
            ProductPrice = string.Empty;
            ProductQuantity = string.Empty;
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(ProductName))
            {
                MessageBox.Show("Please enter product name.", "Validation Error",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(ProductCategory))
            {
                MessageBox.Show("Please enter product category.", "Validation Error",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (!double.TryParse(ProductPrice, out _))
            {
                MessageBox.Show("Please enter a valid price.", "Validation Error",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (!int.TryParse(ProductQuantity, out _))
            {
                MessageBox.Show("Please enter a valid quantity.", "Validation Error",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
    }
}