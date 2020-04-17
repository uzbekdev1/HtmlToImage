using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using itext.pdfimage.Extensions;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PdfToImage.UI.Models;

namespace PdfToImage.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult GetPdf()
        {
            //save to pdf
            using (var htmlSource = System.IO.File.Open("input.html", FileMode.Open))
            using (var pdfDest = System.IO.File.Open("output.pdf", FileMode.OpenOrCreate))
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
            var bytes = System.IO.File.ReadAllBytes("output.png");

            return File(bytes, "application/png", "output.png");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
