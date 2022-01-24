using AutoMapper;
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
    [Authorize]
    public class SolicitudMantenimientoController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork unitofwork;

        public SolicitudMantenimientoController(IMapper mapper, IUnitOfWork _unitofwork)
        {
            _mapper = mapper;
            unitofwork = _unitofwork;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CrearSolicitud()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearSolicitud(SolicitudMantenimientoCorrectivoVM solicitudMantenimientoCorrectivoVM)
        {
            var solicitud = _mapper.Map<SolicitudMantenimientoCorrectivoVM, SolicitudMantenimientoCorrectivo>(solicitudMantenimientoCorrectivoVM);


            if (ModelState.IsValid)
            {
                //Traemos datos del claim
                solicitud.AreaSolicitante = ConsultarClaim(ClaimTypes.GroupSid);

                string idusu = ConsultarClaim(ClaimTypes.UserData);
                var id = Convert.ToInt32(idusu);
                solicitud.UsuarioId = id;

                solicitud.Folio = solicitudMantenimientoCorrectivoVM.Folio;

                solicitud.DepartamentoDirigido = solicitudMantenimientoCorrectivoVM.DepartamentoDirigido.ToUpper();

                await unitofwork.SolicitudRepository.AgregarAsin(solicitud);
                await unitofwork.SaveAsync();

                return RedirectToAction(nameof(Index));
            }


            return View(solicitudMantenimientoCorrectivoVM);

        }

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
                    solicitud.NombreSolicitante = svm.NombreSolicitante;
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

        [HttpGet]
        public async Task<IActionResult> GetAll(SolicitudMantenimientoCorrectivoVM solicitudMantenimientoCorrectivoVM)
        {
            var solicitud = _mapper.Map<SolicitudMantenimientoCorrectivoVM, SolicitudMantenimientoCorrectivo>(solicitudMantenimientoCorrectivoVM);

            solicitud.AreaSolicitante = ConsultarClaim(ClaimTypes.GroupSid);

            return Json(new { data = await unitofwork.SolicitudRepository.ObtenerTodosAsin(match: x => x.AreaSolicitante == solicitud.AreaSolicitante) });
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
