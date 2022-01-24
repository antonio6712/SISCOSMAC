using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SISCOSMAC.DAL.UFW;
using SISCOSMAC.Web.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SISCOSMAC.Web.Controllers
{
    public class AccesoController : Controller
    {

        private readonly IUnitOfWork UnitOfWork;
        private readonly IMapper mapper;

        public AccesoController(IUnitOfWork uofw, IMapper _mapper)
        {
            UnitOfWork = uofw;
            mapper = _mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            await HttpContext.SignOutAsync("SiscosmacCookieAuthenticationSheme");
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            UsuarioViewModel usu = new UsuarioViewModel(UnitOfWork, mapper);
            ViewData["ReturnUrl"] = returnUrl;
            LoginViewModel lvm = new LoginViewModel(UnitOfWork, mapper);

            //ejemplo de campo vacio
            //if(!string.IsNullOrEmpty(model.UsuarioLogin))
            //    model.UsuarioLogin = model.UsuarioLogin.ToUpper();
            //else
            //    ModelState.AddModelError(string.Empty, "el usuario no tiene que estar vacio");

            model.UsuarioLogin = model.UsuarioLogin.ToUpper();
            bool usuarioExiste = await lvm.ValidarUsuario(model.UsuarioLogin, model.Password);

            
            if (usuarioExiste != false)
            {
                var con = await usu.ObtenerUsuario(model.UsuarioLogin);
                //crear consulta para obtener el usuario por el username
                //var us = await UnitOfWork.UsuarioRepository.ObtenerAsin(model);

                string idusu = con.UsuarioId.ToString();

                List<Claim> reclamaciones = new List<Claim>();
                reclamaciones.Add(new Claim(ClaimTypes.GroupSid, con.NombreDeptoPer));
                reclamaciones.Add(new Claim(ClaimTypes.Name, con.Nombre + " " + con.APaterno + " " + con.AMaterno));
                reclamaciones.Add(new Claim(ClaimTypes.Role, con.Rol));
                reclamaciones.Add(new Claim(ClaimTypes.UserData, idusu));

                //reclamaciones.Add(new Claim(ClaimTypes.IsPersistent, model.Recuerdame.ToString()));

                var identidadUsuario = new ClaimsIdentity(reclamaciones, "login");
                ClaimsPrincipal claimPrincipal = new ClaimsPrincipal(identidadUsuario);
                await HttpContext.SignInAsync("SiscosmacCookieAuthenticationSheme", claimPrincipal);
                return RedictToLocal(returnUrl);

            }
            else
            {
                ModelState.AddModelError(string.Empty, "El usuario o contraseña es incorrecto/a");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync("SiscosmacCookieAuthenticationSheme");
            return RedirectToAction(nameof(AccesoController.Login), "Acceso");
        }


        private IActionResult RedictToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");
        }

    }
}
