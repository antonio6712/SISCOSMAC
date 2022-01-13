using AutoMapper;
using SISCOSMAC.AlgoritmoSeguridad;
using SISCOSMAC.DAL.Models;
using SISCOSMAC.DAL.UFW;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISCOSMAC.Web.ViewModels
{
    public class LoginViewModel
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public LoginViewModel()
        {

        }

        public LoginViewModel(IUnitOfWork uofw, IMapper _mapper)
        {
            unitOfWork = uofw;
            mapper = _mapper;
        }

        [Required(ErrorMessage = "Se Requiere Usuario")]
        [Display(Name = "Usuario")]
        public string UsuarioLogin { get; set; }

        [Required(ErrorMessage = "Se Requiere Contraseña")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }


        public async Task<bool> ValidarUsuario(string usuario, string password)
        {
            byte[] passw = Encoding.UTF8.GetBytes(password);
            var resultado = false;
            var existeUsuario = (from u in await unitOfWork.UsuarioRepository.ObtenerTodosAsin()
                                 where u.UsuarioLogin == usuario
                                 select new { u.UsuarioLogin, u.PasswordHash, u.PasswordSal }).FirstOrDefault();
            if (existeUsuario != null)
            {
                if (!String.IsNullOrEmpty(existeUsuario.UsuarioLogin) && existeUsuario.PasswordHash != null && existeUsuario.PasswordSal != null)
                {
                    byte[] combinarPassSal = Hash.PasswordABytes(passw, existeUsuario.PasswordSal);
                    if (existeUsuario.UsuarioLogin == usuario && existeUsuario.PasswordHash.SequenceEqual(combinarPassSal))
                        resultado = true;
                    else
                        resultado = false;
                }
                else
                    resultado = false;
            }
            else
                resultado = false;
            return resultado;
        }

        public async Task<Usuario> ObtenerUsuario(string usuarioLogin, string password)
        {
            var con = (from u in await unitOfWork.UsuarioRepository.ObtenerTodosAsin()
                       where u.UsuarioLogin == usuarioLogin /*&& u.Estatus == true*/ && u.PasswordHash == Encoding.ASCII.GetBytes(Hash.ObtenerMD5(password))
                       select u).FirstOrDefault();
            return con;
        }
    }
}
