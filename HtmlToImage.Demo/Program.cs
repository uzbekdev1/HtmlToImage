using itext.pdfimage.Extensions;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using System;
using System.Drawing.Imaging;
using System.IO;

namespace PdfToImage.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            //save to pdf
            using (var htmlSource = File.Open("input.html", FileMode.Open))
            using (var pdfDest = File.Open("output.pdf", FileMode.OpenOrCreate))
            {
                var converterProperties = new ConverterProperties();

                HtmlConverter.ConvertToPdf(htmlSource, pdfDest, converterProperties);
            }

            //extract to image
            var reader = new PdfReader("output.pdf");
            var pdfDocument = new PdfDocument(reader);
            var page1 = pdfDocument.GetPage(1);
            using var bitmap1 = page1.ConvertPageToBitmap();
            bitmap1.Save("output.png", ImageFormat.Png);

            Console.WriteLine("Done!");
        }
    }
}
