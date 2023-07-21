using System.Text.RegularExpressions;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

Console.WriteLine("ADCB Bank E-Statement PDF Extractor");

string pdfPassword = getPDFPassword("statement_pwd.txt");

string path = Path.Combine(Directory.GetCurrentDirectory(), "AccountStatement.pdf");
string pageContent = "";

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
Console.WriteLine(pageContent);

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