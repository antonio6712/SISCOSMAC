using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            var p = ConsultarClaim(ClaimTypes.GroupSid);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearSolicitud(SolicitudMantenimientoCorrectivoVM solicitudMantenimientoCorrectivoVM)
        {
            var solicitud = _mapper.Map<SolicitudMantenimientoCorrectivoVM, SolicitudMantenimientoCorrectivo>(solicitudMantenimientoCorrectivoVM);

            

            if (ModelState.IsValid)
            {
                solicitud.AreaSolicitante = ConsultarClaim(ClaimTypes.GroupSid);
                solicitud.UsuarioId = solicitud.UsuarioId;

                solicitud.Folio = solicitudMantenimientoCorrectivoVM.Folio;
                solicitud.DepartamentoDirigido = solicitudMantenimientoCorrectivoVM.DepartamentoDirigido.ToUpper();
                                
                await unitofwork.SolicitudRepository.AgregarAsin(solicitud);
                await unitofwork.SaveAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(solicitudMantenimientoCorrectivoVM);

        }

        //[HttpGet]
        //public async Task<IActionResult> EditarSolicitud(int id)
        //{
        //    UsuarioViewModel uvm = new UsuarioViewModel();
        //    var departamentoLista = (from u in await unitofwork.DepartamentoRepository.ObtenerTodosAsin()
        //                             select new { u.DepartamentoId, NombreDepartamento = u.NombreDepartamento }).ToList();

        //    var modelo = await unitofwork.UsuarioRepository.ObtenerPorIdAsin(id);

        //    if (modelo == null)
        //    {
        //        return NotFound();
        //    }

        //    ViewBag.DepartamentoId = new SelectList(departamentoLista, "DepartamentoId", "NombreDepartamento", uvm.UsuarioId);

        //    var usuario = _mapper.Map<Usuario, UsuarioViewModel>(modelo);

        //    return View(usuario);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditarSolicitud(UsuarioViewModel uvm, int id)
        //{
        //    var departamentoLista = (from u in await unitofwork.DepartamentoRepository.ObtenerTodosAsin()
        //                             select new { u.DepartamentoId, NombreDepartamento = u.NombreDepartamento }).ToList();

        //    try
        //    {
        //        //var usuario = _mapper.Map<UsuarioViewModel, Usuario>(uvm);
        //        var usuario = await unitofwork.UsuarioRepository.ObtenerAsin(match: x => x.UsuarioId == uvm.UsuarioId);
        //        if (ModelState.IsValid)
        //        {
        //            usuario.Nombre = uvm.Nombre.ToUpper();
        //            usuario.APaterno = uvm.APaterno.ToUpper();
        //            usuario.AMaterno = uvm.AMaterno.ToUpper();
        //            usuario.DepartamentoId = uvm.DepartamentoId;
        //            usuario.Rol = uvm.Rol.ToUpper();
        //            usuario.UsuarioLogin = uvm.UsuarioLogin.ToUpper();


        //            await unitofwork.UsuarioRepository.ActualizarAsin(usuario, id);
        //            await unitofwork.SaveAsync();
        //            return RedirectToAction(nameof(Index));
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    ViewBag.DepartamentoId = new SelectList(departamentoLista, "DepartamentoId", "NombreDepartamento", uvm.UsuarioId);

        //    return View(uvm);
        //}

        [HttpGet]
        public async Task<IActionResult> GetAll(SolicitudMantenimientoCorrectivoVM solicitudMantenimientoCorrectivoVM)
        {
            var solicitud = _mapper.Map<SolicitudMantenimientoCorrectivoVM, SolicitudMantenimientoCorrectivo>(solicitudMantenimientoCorrectivoVM);

            solicitud.AreaSolicitante = ConsultarClaim(ClaimTypes.GroupSid);

            return Json(new { data = await unitofwork.SolicitudRepository.ObtenerTodosAsin(match: x=>x.AreaSolicitante == solicitud.AreaSolicitante ) });
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
