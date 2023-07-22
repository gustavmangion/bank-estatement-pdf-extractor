using pdf_extractor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace pdf_extractor.Helpers
{
    internal static class TransactionHelper
    {
        public static void getSecondPart(string p2, Transaction transaction)
        {
            string pattern = "([0-9]{2}/[0-9]{2}/[0-9]{4})|([0-9,]*\\.[0-9]{2})";
            Regex regex = new Regex(pattern);

            List<string> matches = regex.Split(p2).Where(x => !string.IsNullOrEmpty(x)).ToList();

            transaction.Date = DateOnly.Parse(matches[0]);
            transaction.Amount = decimal.Parse(matches[2]);
        }

        public static void getChequeDebit(string p1, Transaction transaction)
        {
            string pattern = "([0-9]{2}/[0-9]{2}/[0-9]{4})([A-Z/ ]*)([0-9]*)";
            Regex regex = new Regex(pattern);

            List<string> matches = regex.Split(p1).Where(x => !string.IsNullOrEmpty(x)).ToList();

            transaction.EnteredBank = getEnteredBank(p1);
            transaction.Description = "Cheque Withdrawl";
            transaction.Reference = matches[2];
            transaction.Category = TranCategory.ChequeWithdrawal;
        }

        public static void getATMWithdrawal(string p1, Transaction transaction)
        {
            transaction.EnteredBank = getEnteredBank(p1);
            transaction.Description = $"{p1.Substring(29, p1.Length - 57)}";
            transaction.Reference = p1.Substring(p1.Length-8, 8);
            transaction.Category = TranCategory.ATMWithdrawal;
        }

        public static void getBankTransferCredit(string p1, Transaction transaction)
        {
            int index = p1.IndexOf("B/O");

            transaction.EnteredBank = getEnteredBank(p1);
            transaction.Description = $"{p1.Substring(index+4, p1.Length - (index+12))}";
            transaction.Reference = p1.Substring(p1.Length - 8, 8);
            transaction.Type = TranType.Credit;
            transaction.Category = TranCategory.BankTransfer;
        }

        public static void getRefund(string p1, Transaction transaction)
        {
            transaction.EnteredBank = getEnteredBank(p1);
            transaction.Description = $"{p1.Substring(20, p1.Length - 31)}";
            transaction.CardNo = p1.Substring(p1.Length - 4, 4);
            transaction.Type = TranType.Credit;
            transaction.Category = TranCategory.Refund;
        }

        public static void getSalary(string p1, Transaction transaction)
        {
            transaction.EnteredBank = getEnteredBank(p1);
            transaction.Description = "Salary";
            transaction.Type = TranType.Credit;
            transaction.Category= TranCategory.Salary;
        }

        public static void getMiscellaneousCharge(string p1, Transaction transaction)
        {
            transaction.EnteredBank = getEnteredBank(p1);
            transaction.Description = $"{p1.Substring(10, p1.Length - 18)}";
            transaction.Reference = p1.Substring(p1.Length - 8, 8);
            transaction.Category = TranCategory.Other;
        }

        private static DateOnly getEnteredBank(string p1)
        {
            return DateOnly.Parse(p1.Substring(0, 10));
        }

        
    }
}
