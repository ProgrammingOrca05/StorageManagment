namespace StorageManagement.Models
{
    public class TransactionItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal LineTotal => Quantity * Price;
    }
}