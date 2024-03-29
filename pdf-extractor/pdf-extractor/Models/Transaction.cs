﻿using System;
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
        ChequeDeposit = 1,
        ChequeWithdrawal = 2,
        BankTransfer = 3,
        ATMWithdrawal = 4,
        Salary = 5,
        Refund = 6,
        Other = 99
    }

}
