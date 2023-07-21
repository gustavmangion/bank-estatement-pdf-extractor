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
    }

    enum TranType {
        Debit = 0,
        Credit = 1
    }

}
