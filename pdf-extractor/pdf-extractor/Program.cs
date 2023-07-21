using pdf_extractor.Models;
using System.Text.RegularExpressions;
using System.Transactions;
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
var dates = getStatementDates(pageContent);
statement.From = dates.Item1;
statement.To = dates.Item2;

Console.WriteLine(statement.ToString());

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