using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SISCOSMAC.DAL.Models;
using SISCOSMAC.DAL.UFW;
using SISCOSMAC.Web.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using SISCOSMAC.AlgoritmoSeguridad;
using System;
using Microsoft.AspNetCore.Authorization;

namespace SISCOSMAC.Web.Controllers
{
    [Authorize(Roles = "ADMINISTRADOR")]
    public class UsuariosController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork unitofwork;

        public UsuariosController(IMapper mapper, IUnitOfWork _unitofwork)
        {
            _mapper = mapper;
            unitofwork = _unitofwork;

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            DepartamentoViewModel dvm = new DepartamentoViewModel();

            var departamento = (from u in await unitofwork.DepartamentoRepository.ObtenerTodosAsin()
                                select new { u.DepartamentoId, NombreDepartamento = u.NombreDepartamento }).ToList();
            ViewBag.DepartamentoId = new SelectList(departamento, "DepartamentoId", "NombreDepartamento");


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(UsuarioViewModel usuarioViewModel)
        {
            var departamentoLista = (from u in await unitofwork.DepartamentoRepository.ObtenerTodosAsin()
                                     select new { u.DepartamentoId, NombreDepartamento = u.NombreDepartamento }).ToList();

            try
            {
                if (ModelState.IsValid)
                {
                    var us = _mapper.Map<UsuarioViewModel, Usuario>(usuarioViewModel);
                    //var departamento = _mapper.Map<UsuarioViewModel, Usuario>(usuarioViewModel);

                    var usuarioExiste = (from u in await unitofwork.UsuarioRepository.ObtenerTodosAsin()
                                         where u.UsuarioLogin == usuarioViewModel.UsuarioLogin
                                         select u).FirstOrDefault();

                    if (usuarioExiste == null)
                    {
                        us.Nombre = us.Nombre.ToUpper();
                        us.APaterno = us.APaterno.ToUpper();
                        us.AMaterno = us.AMaterno.ToUpper();
                        us.Rol = us.Rol.ToUpper();
                        us.UsuarioLogin = us.UsuarioLogin.ToUpper();


                        us.PasswordSal = Hash.GenerarSal();
                        us.PasswordHash = Hash.HashPasswordConSal(Encoding.ASCII.GetBytes(usuarioViewModel.PasswordClara), us.PasswordSal);

                        await unitofwork.UsuarioRepository.AgregarAsin(us);

                        await unitofwork.SaveAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, "Este nombre de Usuario de ingreso ya ¡EXISTE!: " + us.UsuarioLogin);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            ViewBag.DepartamentoId = new SelectList(departamentoLista, "DepartamentoId", "NombreDepartamento", usuarioViewModel.DepartamentoId);
            return View(usuarioViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            UsuarioViewModel uvm = new UsuarioViewModel();
            var departamentoLista = (from u in await unitofwork.DepartamentoRepository.ObtenerTodosAsin()
                                     select new { u.DepartamentoId, NombreDepartamento = u.NombreDepartamento }).ToList();

            var modelo = await unitofwork.UsuarioRepository.ObtenerPorIdAsin(id);

            if (modelo == null)
            {
                return NotFound();
            }

            ViewBag.DepartamentoId = new SelectList(departamentoLista, "DepartamentoId", "NombreDepartamento", uvm.UsuarioId);

            var usuario = _mapper.Map<Usuario, UsuarioViewModel>(modelo);

            return View(usuario);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(UsuarioViewModel uvm, int id)
        {
            var departamentoLista = (from u in await unitofwork.DepartamentoRepository.ObtenerTodosAsin()
                                     select new { u.DepartamentoId, NombreDepartamento = u.NombreDepartamento }).ToList();

            try
            {
                //var usuario = _mapper.Map<UsuarioViewModel, Usuario>(uvm);
                var usuario =  await unitofwork.UsuarioRepository.ObtenerAsin(match: x => x.UsuarioId == uvm.UsuarioId);
                if (ModelState.IsValid)
                {
                    usuario.Nombre = uvm.Nombre.ToUpper();
                    usuario.APaterno = uvm.APaterno.ToUpper();
                    usuario.AMaterno = uvm.AMaterno.ToUpper();
                    usuario.DepartamentoId = uvm.DepartamentoId;
                    usuario.Rol = uvm.Rol.ToUpper();
                    usuario.UsuarioLogin = uvm.UsuarioLogin.ToUpper();
                                      
                                        
                    await unitofwork.UsuarioRepository.ActualizarAsin(usuario, id);
                    await unitofwork.SaveAsync();
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception ex)
            {

            }

            ViewBag.DepartamentoId = new SelectList(departamentoLista, "DepartamentoId", "NombreDepartamento", uvm.UsuarioId);

            return View(uvm);
        }

        //Editar La Contraseña de Un Usuario
        [HttpGet]
        public async Task<IActionResult> EditarCont(int? id)
        {
            var modelo = await unitofwork.UsuarioRepository.ObtenerAsin(match: x => x.UsuarioId == id);
            if (modelo == null)
            {
                return NotFound();
            }
            return View(_mapper.Map<Usuario, UsuarioViewModel>(modelo));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarCont(UsuarioViewModel uvm)
        {
            try
            {
                //var usuario = _mapper.Map<UsuarioViewModel, Usuario>(uvm);
                var usuario = await unitofwork.UsuarioRepository.ObtenerAsin(match: x => x.UsuarioId == uvm.UsuarioId);

                if (ModelState.IsValid)
                {
                    usuario.PasswordSal = Hash.GenerarSal();
                    usuario.PasswordHash = Hash.HashPasswordConSal(Encoding.ASCII.GetBytes(uvm.PasswordClara), usuario.PasswordSal);

                    await unitofwork.UsuarioRepository.ActualizarAsin(usuario, usuario.UsuarioId);


                }
                await unitofwork.SaveAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
            }
            return View(uvm);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //este es un jemplo de realizar una consulta y filtrarlo 
            //return Json(new { data = await unitofwork.UsuarioRepository.ObtenerTodosAsin(match:x=>x.APaterno=="flores" && x.AMaterno=="nuñez", orderBy:y=>y.OrderBy(z=>z.DepartamentoId)  , includeProperties:"departamento") });
            var resultado = await unitofwork.UsuarioRepository.ObtenerTodosAsin(includeProperties: "departamento");

            var records = _mapper.Map<IEnumerable<Usuario>, IEnumerable<UsuarioViewModel>>(resultado);

            return Json(new { data = records });

        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            var objFormDb = await unitofwork.UsuarioRepository.ObtenerPorIdAsin(id);
            if (objFormDb == null)
            {
                return Json(new { success = false, message = "Error Borrando el Usuario" });
            }
            unitofwork.UsuarioRepository.EliminarAsin(objFormDb);
            await unitofwork.SaveAsync();
            return Json(new { success = true, message = "Usuario Borrado Con ¡EXITO!" });


        }



    }
}
