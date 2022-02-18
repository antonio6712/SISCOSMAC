using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SISCOSMAC.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SISCOSMAC.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IConverter _converter;

        public HomeController(ILogger<HomeController> logger,IConverter converter)
        {
            _logger = logger;
            _converter = converter;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy(int IdSolicitud)
        {
            int a = 1;
            a = IdSolicitud;

            return View();
        }

        public IActionResult AccesoNegado()
        {
            return View();
        }

        // /Home/PrintView?controlador=Home&accion=Privacy&IdSolicitud=12
        //http://localhost:35717/Home/PrintView?controlador=Home&accion=Privacy&IdSolicitud=12
        [AllowAnonymous]
        public IActionResult PrintView(string controlador, string accion,int IdSolicitud)
        {
            //decrpyted values
            var route = string.Format("/{0}/{1}", controlador, accion);
            string absoluteUrl = "";
            absoluteUrl = string.Format("{0}://{1}{2}?IdSolicitud={3}", Request.Scheme, Request.Host, route, IdSolicitud);

            var uri = new Uri(absoluteUrl, UriKind.Absolute);
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.Letter,
                Margins = { Top = 1.0, Bottom = 1.0 },
                DocumentTitle = "solicitud"
            },
                Objects = {
                new ObjectSettings() {
                PagesCount = true,
                Page = uri.AbsoluteUri.ToString(),
                WebSettings = { DefaultEncoding = "utf-8" },
                }
            }
            };
            byte[] pdf = _converter.Convert(doc);
            return new FileContentResult(pdf, "application/pdf");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
