using DinkToPdf;
using DinkToPdf.Contracts;
using MRE.Presistence.IProvider;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRE.Presistence.Providers
{
    public class PdfProvider : IPdfProvider
    {
        private readonly IConverter _converter;
        private readonly IHostingEnvironment _env;

        public PdfProvider(IConverter converter, IHostingEnvironment env)
        {
            _converter = converter;
            _env = env;
        }

        public async Task<byte[]> Get(string htmlContent, bool landscape = false)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
            };
            if (landscape)
            {
                globalSettings.Orientation = Orientation.Landscape;
            }

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8",
                   UserStyleSheet = Path.Combine(_env.WebRootPath, "assets","css", "table.css"),
                }
            };


            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            return _converter.Convert(pdf);
        }
    }
}
