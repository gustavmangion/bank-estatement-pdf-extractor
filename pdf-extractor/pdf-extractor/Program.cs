using UglyToad.PdfPig;

Console.WriteLine("ADCB Bank E-Statement PDF Extractor");

string path = Path.Combine(Directory.GetCurrentDirectory(), "AccountStatement.pdf");
using (PdfDocument document = PdfDocument.Open(path))
{
    int pageCount = document.NumberOfPages;
}
