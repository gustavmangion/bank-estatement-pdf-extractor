using pdf_extractor.Helpers;
using pdf_extractor.Models;
using System.Text.RegularExpressions;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

Console.WriteLine("ADCB Bank E-Statement PDF Extractor");

string pdfPassword = getPDFPassword("statement_pwd.txt");

string path = Path.Combine(Directory.GetCurrentDirectory(), "AccountStatement.pdf");
string pageContent = "";

Statement statement = new Statement();

using (PdfDocument document = PdfDocument.Open(path, new ParsingOptions { Password = pdfPassword }))
{
    int pageCount = document.NumberOfPages;

    for(int i = 1; i < pageCount; i++)
    {
        //Page numbering is 1 indexed
        Page page = document.GetPage(i+1);

        pageContent += page.Text;
    }
}
pageContent= cleanNextPageEntities(pageContent);
(statement.From, statement.To) = getStatementDates(pageContent);

string[] accounts = pageContent.Split("Account Details: ");
for (int i=1;  i< accounts.Length; i++)
    statement.Accounts.Add(getAccount(accounts[i]));

Account getAccount(string content)
{
    Account account = new Account();
    account.Number = content.Substring(0, 14);
    account.IBAN = content.Substring(content.IndexOf("IBAN: ")+6, 23);
    account.Currency = content.Substring(content.IndexOf("Currency: ")+10, 3);

    List<string> transactionsSplit = getTransactionsSplit(content);
    
    for(int i=2; i<transactionsSplit.Count-1; i+=2)
    {
        account.Transactions.Add(getTransaction(transactionsSplit[i], transactionsSplit[i+1]));
    }

    return account;
}

Transaction getTransaction(string p1, string p2)
{
    Transaction transaction = new Transaction();
    string pattern;
    if (p1.Contains("I/W CLEARING CHEQUE"))
        TransactionHelper.getChequeDebit(p1, transaction);
    else if (p1.Contains("ATM WDL"))
        TransactionHelper.getATMWithdrawal(p1, transaction);
    else if (p1.Contains("B/O"))
        TransactionHelper.getBankTransferCredit(p1, transaction);

    TransactionHelper.getSecondPart(p2, transaction);
    return transaction;
}

List<string> getTransactionsSplit(string content)
{
    Regex regex = new Regex("([0-9]{2}\\/[0-9]{2}\\/[0-9]{4}.*?)(?=[0-9]{2}\\/[0-9]{2}\\/[0-9]{4})|([0-9]{2}\\/[0-9]{2}\\/[0-9]{4}.*)Total");
    return regex.Split(content).Where(x => !string.IsNullOrEmpty(x)).ToList();
}

Tuple<DateOnly, DateOnly> getStatementDates(string pageContent)
{
    string pattern = "Transactions Details for the period from ([0-9]{2}/[0-9]{2}/[0-9]{4}) to ([0-9]{2}/[0-9]{2}/[0-9]{4})";
    Regex regex = new Regex(pattern);
    MatchCollection matches = regex.Matches(pageContent);

    if (matches.Count == 0 || matches[0].Groups.Count != 3)
        throw new Exception("Unable to find statement date range");

    return new Tuple<DateOnly, DateOnly>(DateOnly.Parse(matches[0].Groups[1].Value), DateOnly.Parse(matches[0].Groups[2].Value));
}

string cleanNextPageEntities(string text)
{
    string pattern = "(Page [0-9] of [0-9]Transactions Details for the period from " +
        "[0-9]{2}/[0-9]{2}/[0-9]{4} to [0-9]{2}/[0-9]{2}/[0-9]{4}DateDescriptionChq" +
        "/Ref No.Value DateDebitCreditBalance)";

    return Regex.Replace(text, pattern, "");
}

string getPDFPassword(string filename)
{
    return File.ReadAllText(filename);
}