using System;
using System.Collections.Generic;

namespace StorageManagement.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<TransactionItem> Items { get; set; } = new List<TransactionItem>();
        public double Subtotal { get; set; }
        public double VAT { get; set; }
        public double Total { get; set; }
    }
}