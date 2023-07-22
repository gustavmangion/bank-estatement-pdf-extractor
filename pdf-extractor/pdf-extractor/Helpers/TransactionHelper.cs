using pdf_extractor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace pdf_extractor.Helpers
{
    internal static class TransactionHelper
    {
        public static void getChequeDebit(string p1, Transaction transaction)
        {
            string pattern = "([0-9]{2}/[0-9]{2}/[0-9]{4})([A-Z/ ]*)([0-9]*)";
            Regex regex = new Regex(pattern);

            List<string> matches = regex.Split(p1).Where(x => !string.IsNullOrEmpty(x)).ToList();

            transaction.EnteredBank = DateOnly.Parse(matches[0]);
            transaction.Description = "Cheque Withdrawl";
            transaction.Reference = matches[2];
        }

        public static void getSecondPart(string p2, Transaction transaction)
        {
            string pattern = "([0-9]{2}/[0-9]{2}/[0-9]{4})|([0-9,]*\\.[0-9]{2})";
            Regex regex = new Regex(pattern);

            List<string> matches = regex.Split(p2).Where(x => !string.IsNullOrEmpty(x)).ToList();

            transaction.Date = DateOnly.Parse(matches[0]);
            transaction.Amount = decimal.Parse(matches[2]);
        }

        public static void getATMWithdrawal(string p1, Transaction transaction)
        {
            transaction.EnteredBank = DateOnly.Parse(p1.Substring(0, 10));
            transaction.Description = $"ATM Withdrawal - {p1.Substring(29, p1.Length - 57)}";
            transaction.Reference = p1.Substring(p1.Length-8, 8);
        }
    }
}
