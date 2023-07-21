using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdf_extractor.Models
{
    internal class Account
    {
        public string Number { get; set; }
        public string IBAN { get; set; }
        public string Currency { get; set; }
    }
}
