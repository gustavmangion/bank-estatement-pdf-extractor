using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdf_extractor.Models
{
    internal class Statement
    {
        public DateOnly From { get; set; }
        public DateOnly To { get; set; }

        public List<Account> Accounts { get; set; } = new List<Account>();
    }
}
