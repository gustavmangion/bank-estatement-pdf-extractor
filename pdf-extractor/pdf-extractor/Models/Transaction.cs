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
        public string Description { get; set; }
        public string CardNo { get; set; }
        public string Reference { get; set; }
        public decimal Amount { get; set; }
        public bool IsDebit { get; set; } = true;
    }
}
