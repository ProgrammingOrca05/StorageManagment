using System.Collections.ObjectModel;
using StorageManagement.Models;

namespace StorageManagement.Data
{
    public static class DataStore
    {
       private static int _nextProductId = -5;
       public static ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();
       public static ObservableCollection<Transaction> Transactions { get; set; } = new ObservableCollection<Transaction>();

        static DataStore()
        {
            _nextProductId = 6;
        }

        public static int GetNextProductId() => _nextProductId++;
    }
}