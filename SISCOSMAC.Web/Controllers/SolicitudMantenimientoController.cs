using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SISCOSMAC.DAL.UFW;
using SISCOSMAC.Web.ViewModels;
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
        public IActionResult Crear()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Crear(SolicitudMantenimientoCorrectivoVM solicitudMantenimientoCorrectivoVM)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var departamento = _mapper.Map<SolicitudMantenimientoCorrectivoVM, Departamento>(departamentoViewModel);

        //        await unitofwork.DepartamentoRepository.AgregarAsin(departamento);
        //        await unitofwork.SaveAsync();

        //        return RedirectToAction(nameof(Index));
        //    }

        //    return View(departamentoViewModel);

        //}

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await unitofwork.SolicitudRepository.ObtenerTodosAsin() });
        }

    }
}
