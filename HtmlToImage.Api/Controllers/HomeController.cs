using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using itext.pdfimage.Extensions;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PdfToImage.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HomeController : ControllerBase
    {

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Version()
        {
            return "Api v1";
        }

        [HttpGet]
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
    }
}
