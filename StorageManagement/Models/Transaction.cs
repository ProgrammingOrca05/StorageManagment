using System;
using System.Collections.Generic;

namespace StorageManagement.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<TransactionItem> Items { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public string PaymentMethod { get; set; }

        public Transaction()
        {
            Items = new List<TransactionItem>();
            Date = DateTime.Now;
        }
    }
}