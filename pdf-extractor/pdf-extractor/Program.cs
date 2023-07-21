using UglyToad.PdfPig;

Console.WriteLine("ADCB Bank E-Statement PDF Extractor");

string pdfPassword = getPDFPassword("statement_pwd.txt");

string path = Path.Combine(Directory.GetCurrentDirectory(), "AccountStatement.pdf");
using (PdfDocument document = PdfDocument.Open(path, new ParsingOptions { Password = pdfPassword }))
{
    int pageCount = document.NumberOfPages;
}


string getPDFPassword(string filename)
{
    return File.ReadAllText(filename);
}