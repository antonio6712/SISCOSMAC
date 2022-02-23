using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SISCOSMAC.DAL.Models;
using SISCOSMAC.DAL.UFW;
using SISCOSMAC.Web.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace SISCOSMAC.Web.Controllers
{
    
    public class SolicitudMantenimientoController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork unitofwork;

        private IConverter _converter;


        public SolicitudMantenimientoController(IMapper mapper, IUnitOfWork _unitofwork, IConverter converter)
        {
            _mapper = mapper;
            unitofwork = _unitofwork;
            _converter = converter;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult CrearSolicitud()
        {

            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearSolicitud(SolicitudMantenimientoCorrectivoVM solicitudMantenimientoCorrectivoVM)
        {
            var solicitud = _mapper.Map<SolicitudMantenimientoCorrectivoVM, SolicitudMantenimientoCorrectivo>(solicitudMantenimientoCorrectivoVM);

            
            if (ModelState.IsValid )
            {
                //Traemos datos del claim
                solicitud.AreaSolicitante = ConsultarClaim(ClaimTypes.GroupSid);

                solicitud.NombreSolicitante = ConsultarClaim(ClaimTypes.Name);
               
                var b = (from f in await unitofwork.SolicitudRepository.ObtenerTodosAsin()
                         where f.DepartamentoDirigido == solicitudMantenimientoCorrectivoVM.DepartamentoDirigido.ToUpper() &&
                         f.AreaSolicitante == solicitud.AreaSolicitante
                         select new { f.Folio.Value }).LastOrDefault();


                var foliovalor = b;

                if (foliovalor == null)
                {
                    solicitud.Folio = 1;
                }
                else
                {
                    solicitud.Folio = foliovalor.Value + 1;
                }

                string idusu = ConsultarClaim(ClaimTypes.UserData);
                var id = Convert.ToInt32(idusu);
                solicitud.UsuarioId = id;
                                
                solicitud.DepartamentoDirigido = solicitudMantenimientoCorrectivoVM.DepartamentoDirigido.ToUpper();

                await unitofwork.SolicitudRepository.AgregarAsin(solicitud);
                await unitofwork.SaveAsync();

                return RedirectToAction(nameof(Index));
            }


            return View(solicitudMantenimientoCorrectivoVM);

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditarSolicitud(int id)
        {          

            var modelo = await unitofwork.SolicitudRepository.ObtenerPorIdAsin(id);

            if (modelo == null)
            {
                return NotFound();
            }

            var solicitud = _mapper.Map<SolicitudMantenimientoCorrectivo, SolicitudMantenimientoCorrectivoVM>(modelo);

            return View(solicitud);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarSolicitud(SolicitudMantenimientoCorrectivoVM svm, int id)
        {
            
            try
            {
                
                var solicitud = await unitofwork.SolicitudRepository.ObtenerAsin(match: x => x.SolicitudId == id);
                if (ModelState.IsValid)
                {
                    solicitud.DepartamentoDirigido = svm.DepartamentoDirigido;
                    solicitud.Folio=svm.Folio;
                    //solicitud.NombreSolicitante = svm.NombreSolicitante;
                    solicitud.FechaElaboracion = svm.FechaElaboracion;
                    solicitud.DescripcionServicios = svm.DescripcionServicios;                   

                    await unitofwork.SolicitudRepository.ActualizarAsin(solicitud, id);
                    await unitofwork.SaveAsync();
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception ex)
            {

            }

            return View(svm);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll(SolicitudMantenimientoCorrectivoVM solicitudMantenimientoCorrectivoVM)
        {
            var solicitud = _mapper.Map<SolicitudMantenimientoCorrectivoVM, SolicitudMantenimientoCorrectivo>(solicitudMantenimientoCorrectivoVM);

            solicitud.AreaSolicitante = ConsultarClaim(ClaimTypes.GroupSid);

            return Json(new { data = await unitofwork.SolicitudRepository.ObtenerTodosAsin(match: x => x.AreaSolicitante == solicitud.AreaSolicitante) });
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> EliminarSolicitud(int id)
        {
            var objFormDb = await unitofwork.SolicitudRepository.ObtenerPorIdAsin(id);
            if (objFormDb == null)
            {
                return Json(new { success = false, message = "Error Borrando la solicitud" });
            }
            unitofwork.SolicitudRepository.EliminarAsin(objFormDb);
            await unitofwork.SaveAsync();
            return Json(new { success = true, message = "Solicitud Borrada Con ¡EXITO!" });


        }

        [HttpGet]
        public async Task<IActionResult> SolicitudPDF(int IdSolicitud)
        {
            var modelo = await unitofwork.SolicitudRepository.ObtenerPorIdAsin(IdSolicitud);

            if (modelo == null)
            {
                return NotFound();
            }

            var orden = _mapper.Map<SolicitudMantenimientoCorrectivo, SolicitudMantenimientoCorrectivoVM>(modelo);

            return View(orden);
        }

        // /Home/PrintView?controlador=Home&accion=Privacy&IdSolicitud=12
        //http://localhost:35717/OrdenTrabajo/PrintView?controlador=OrdenTrabajo&accion=OrdenPDF&IdSolicitud=12
        [AllowAnonymous]
        public IActionResult PrintView(string controlador, string accion, int IdSolicitud)
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
