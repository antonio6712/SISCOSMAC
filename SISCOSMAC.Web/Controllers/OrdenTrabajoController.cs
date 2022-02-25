using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SISCOSMAC.DAL.Models;
using SISCOSMAC.DAL.UFW;
using SISCOSMAC.Web.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SISCOSMAC.Web.Controllers
{

    
    public class OrdenTrabajoController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork unitofwork;

        private readonly ILogger<OrdenTrabajoController> _logger;
        private IConverter _converter;

        public OrdenTrabajoController(IMapper mapper, IUnitOfWork _unitofwork, ILogger<OrdenTrabajoController> logger, IConverter converter)
        {
            _mapper = mapper;
            unitofwork = _unitofwork;

            _logger = logger;
            _converter = converter;
        }

        [Authorize(Roles = "MANTENIMIENTO")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var log = HttpContext.User.Identity.IsAuthenticated;
            if (log == true)
            {
                var buscarSolicitudes = (from s in await unitofwork.SolicitudRepository.ObtenerTodosAsin()
                                         where s.DepartamentoDirigido == ConsultarClaim(ClaimTypes.GroupSid) && s.Recibido == false
                                         select new { s }).ToList();
                int NumeroSolicitudes = buscarSolicitudes.Count;
                if (NumeroSolicitudes == 0)
                {
                    return View();
                }
                else
                {
                    TempData["Solicitudes"] = NumeroSolicitudes;

                }
            }

            return View();
        }


        [Authorize(Roles = "MANTENIMIENTO")]
        [HttpGet]
        public async Task<IActionResult> CrearOrden(int id)
        {
            var BuscarOrden = await unitofwork.OrdenRepository.ObtenerAsin(match: x => x.SolicitudId == id);

            if (BuscarOrden == null)
            {
                return View();
            }
            else
            {
                TempData["Mensaje"] = "No Puedes Crear Dos Ordenes de Trabajo de la Misma Solicitud";
                return RedirectToAction("Index");
            }

        }

        [Authorize(Roles = "MANTENIMIENTO")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearOrden(OrdenTrabajoVM ordenTrabajoVM, int id)
        {
            var BuscarSolicitud = await unitofwork.SolicitudRepository.ObtenerPorIdAsin(id);

            var orden = _mapper.Map<OrdenTrabajoVM, OrdenTrabajo>(ordenTrabajoVM);

            if (ModelState.IsValid)
            {
                orden.VerificadoLiberado = ConsultarClaim(ClaimTypes.Name);
                orden.AprobadoPor = BuscarSolicitud.NombreSolicitante;
                orden.SolicitudId = BuscarSolicitud.SolicitudId;

                BuscarSolicitud.Recibido = true;                
                await unitofwork.SolicitudRepository.ActualizarAsin(BuscarSolicitud,id);
                

                await unitofwork.OrdenRepository.AgregarAsin(orden);
                await unitofwork.SaveAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(ordenTrabajoVM);
        }

        [Authorize(Roles = "MANTENIMIENTO")]
        [HttpGet]
        public async Task<IActionResult> ListaOrdenes()
        {
            var log = HttpContext.User.Identity.IsAuthenticated;
            if (log == true)
            {
                var buscarSolicitudes = (from s in await unitofwork.SolicitudRepository.ObtenerTodosAsin()
                                         where s.DepartamentoDirigido == ConsultarClaim(ClaimTypes.GroupSid) && s.Recibido == false
                                         select new { s }).ToList();
                int NumeroSolicitudes = buscarSolicitudes.Count;
                if (NumeroSolicitudes == 0)
                {
                    return View();
                }
                else
                {
                    TempData["Solicitudes"] = NumeroSolicitudes;

                }
            }

            return View();
        }

        [Authorize(Roles = "MANTENIMIENTO")]
        [HttpGet]
        public async Task<IActionResult> EditarOrden(int id)
        {
            var modelo = await unitofwork.OrdenRepository.ObtenerPorIdAsin(id);

            if (modelo == null)
            {
                return NotFound();
            }

            var orden = _mapper.Map<OrdenTrabajo, OrdenTrabajoVM>(modelo);


            return View(orden);
        }

        [Authorize(Roles = "MANTENIMIENTO")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarOrden(OrdenTrabajoVM ordenTrabajoVM, int id)
        {
            try
            {
                var orden = await unitofwork.OrdenRepository.ObtenerAsin(match: x => x.OrdenId == id);

                if (ModelState.IsValid)
                {
                    orden.NumeroControl = ordenTrabajoVM.NumeroControl;
                    orden.Mantenimiento = ordenTrabajoVM.Mantenimiento;
                    orden.TipoServicio = ordenTrabajoVM.TipoServicio;
                    orden.Asignado = ordenTrabajoVM.Asignado;
                    orden.FechaRealizacion = ordenTrabajoVM.FechaRealizacion;
                    orden.TrabajoRealizado = ordenTrabajoVM.TrabajoRealizado;
                    //orden.VerificadoLiberado = ordenTrabajoVM.VerificadoLiberado;
                    //orden.AprobadoPor = ordenTrabajoVM.AprobadoPor;

                    await unitofwork.OrdenRepository.ActualizarAsin(orden, id);
                    await unitofwork.SaveAsync();
                    return RedirectToAction(nameof(ListaOrdenes));
                }

            }
            catch (Exception ex)
            {

            }

            return View(ordenTrabajoVM);
        }

        [Authorize(Roles = "MANTENIMIENTO")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var departamentoDirigido = ConsultarClaim(ClaimTypes.GroupSid);

            return Json(new { data = await unitofwork.SolicitudRepository.ObtenerTodosAsin(match: x => x.DepartamentoDirigido == departamentoDirigido) });
        }
                
        [Authorize(Roles = "MANTENIMIENTO")]
        [HttpGet]
        public async Task<IActionResult> ListaOrdenesGetAll()
        {

            var departamentoDirigido = ConsultarClaim(ClaimTypes.GroupSid);

            return Json(new { data = await unitofwork.OrdenRepository.ObtenerTodosAsin(match: x => x.solicitud.DepartamentoDirigido == departamentoDirigido) });
        }

        [Authorize(Roles = "MANTENIMIENTO")]
        [HttpDelete]
        public async Task<IActionResult> EliminarOrden(int id)
        {
            var objFormDb = await unitofwork.OrdenRepository.ObtenerPorIdAsin(id);
            if (objFormDb == null)
            {
                return Json(new { success = false, message = "Error Borrando la solicitud" });
            }
            unitofwork.OrdenRepository.EliminarAsin(objFormDb);
            await unitofwork.SaveAsync();
            return Json(new { success = true, message = "Orden Borrada Con ¡EXITO!" });


        }

        [HttpGet]
        public async Task<IActionResult> OrdenPDF(int IdOrden)
        {
            var modelo = await unitofwork.OrdenRepository.ObtenerPorIdAsin(IdOrden);

            if (modelo == null)
            {
                return NotFound();
            }

            var orden = _mapper.Map<OrdenTrabajo, OrdenTrabajoVM>(modelo);

            return View(orden);
        }

        // /Home/PrintView?controlador=Home&accion=Privacy&IdSolicitud=12
        //http://localhost:35717/OrdenTrabajo/PrintView?controlador=OrdenTrabajo&accion=OrdenPDF&IdSolicitud=12
        [AllowAnonymous]
        public IActionResult PrintView(string controlador, string accion, int IdOrden)
        {
            //decrpyted values
            var route = string.Format("/{0}/{1}", controlador, accion);
            string absoluteUrl = "";
            absoluteUrl = string.Format("{0}://{1}{2}?IdOrden={3}", Request.Scheme, Request.Host, route, IdOrden);

            var uri = new Uri(absoluteUrl, UriKind.Absolute);
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.Letter,
                Margins = { Top = 1.0, Bottom = 1.0 },
                DocumentTitle = "Orden de Trabajo"
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

        public string ConsultarClaim(string claimType)
        {
            var LogingUser = HttpContext.User;

            var ClaimC = (from d in LogingUser.Claims
                          where d.Type == claimType
                          select d.Value).Single().ToString().Trim();
            return ClaimC;
        }
    }
}
