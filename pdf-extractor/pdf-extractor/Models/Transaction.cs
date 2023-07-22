using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdf_extractor.Models
{
    internal class Transaction
    {
        public DateOnly Date { get; set; }
        public DateOnly EnteredBank { get; set; }
        public string Description { get; set; } = string.Empty;
        public string CardNo { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public TranType Type { get; set; } = 0;
        public TranCategory Category { get; set; }
    }

    enum TranType {
        Debit = 0,
        Credit = 1
    }

    enum TranCategory
    {
        Purchase = 0, 
        Cheque = 1,
        BankTransfer = 2,
        ATM = 3,
        Salary = 4,
        Refund = 5,
        Other = 99
    }

}
