using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageManagement.Models
{
    public  class Transaction
    {
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
        public List<TransactionItem> Items { get; set; }
    }
}
