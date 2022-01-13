using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SISCOSMAC.DAL.Models;
using SISCOSMAC.DAL.UFW;
using SISCOSMAC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISCOSMAC.Web.Controllers
{
    [Authorize]
    public class DepartamentosController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork unitofwork;

        public DepartamentosController(IMapper mapper, IUnitOfWork _unitofwork)
        {
            _mapper = mapper;
            unitofwork = _unitofwork;

        }

        [HttpGet]
        public  IActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(DepartamentoViewModel departamentoViewModel)
        {
            if (ModelState.IsValid)
            {
                var departamento = _mapper.Map<DepartamentoViewModel, Departamento>(departamentoViewModel);

                await unitofwork.DepartamentoRepository.AgregarAsin(departamento);
                await unitofwork.SaveAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(departamentoViewModel);

        }

        [HttpGet]
        public async Task<IActionResult> Editar( int id)
        {                      

            var modelo = await  unitofwork.DepartamentoRepository.ObtenerPorIdAsin(id);
            

            if (modelo == null)
            {
                return NotFound();
            }
            var departamento = _mapper.Map<Departamento, DepartamentoViewModel>(modelo);
            return View(departamento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(DepartamentoViewModel departamentoViewModel, int id)
        {
            var departamento = _mapper.Map<DepartamentoViewModel, Departamento>(departamentoViewModel);

            if (ModelState.IsValid)
            {

                await unitofwork.DepartamentoRepository.ActualizarAsin(departamento, id);
                await unitofwork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }

            

            return View(departamentoViewModel);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {            
            return Json(new { data = await unitofwork.DepartamentoRepository.ObtenerTodosAsin() });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var objFormDb = await unitofwork.DepartamentoRepository.ObtenerPorIdAsin(id);
            if (objFormDb == null)
            {
                return Json(new { success = false, message = "Error Borrando Departamento" });
            }
            unitofwork.DepartamentoRepository.EliminarAsin(objFormDb);
            await unitofwork.SaveAsync();
            return Json(new { success = true, message = "Departamento Borrado Con ¡EXITO!" });


        }

    }
}
