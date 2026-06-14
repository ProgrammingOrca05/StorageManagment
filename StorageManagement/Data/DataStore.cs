using System.Collections.ObjectModel;
using StorageManagement.Models;

namespace StorageManagement.Data
{
    public static class DataStore
    {
        private static int _nextProductId = 1;

        public static ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>
        {
        };

        static DataStore()
        {
            _nextProductId = 6;
        }

        public static int GetNextProductId() => _nextProductId++;
    }
}