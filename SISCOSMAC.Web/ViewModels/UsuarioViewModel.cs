using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using SISCOSMAC.DAL.Models;
using SISCOSMAC.DAL.UFW;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SISCOSMAC.Web.ViewModels
{
    public class UsuarioViewModel
    {

        private readonly IUnitOfWork UnitOfWork;
        private readonly IMapper mapper;

        public UsuarioViewModel()
        {

        }

        public UsuarioViewModel(IUnitOfWork uofw, IMapper _mapper)
        {
            UnitOfWork = uofw;
            mapper = _mapper;
        }

        [Display(Name = "Indicador")]
        public int UsuarioId { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage ="¡El nombre es requerido!")]
        public string Nombre { get; set; }

        [Display(Name = "Apellido Paterno")]
        [Required(ErrorMessage = "¡El apellido paterno es requerido!")]
        public string APaterno { get; set; }

        [Display(Name = "Apellido Materno")]
        [Required(ErrorMessage = "¡El apellido materno es requerido!")]
        public string AMaterno { get; set; }

        [Display(Name = "Usuario de ingreso")]
        [Required(ErrorMessage = "¡El Usuario es requerido!")]
        public string UsuarioLogin { get; set; }

        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "¡La contraseña es requerido!")]
        public string PasswordClara { get; set; }


        [StringLength(32)]
        public byte[] PasswordHash { get; set; }

       
        [StringLength(32)]
        public byte[] PasswordSal { get; set; }


        [Display(Name = "Rol")]
        [Required(ErrorMessage = "¡Se debe asignar un Rol al usuario!")]
        public string Rol { get; set; }

        [Display(Name = "Departamento")]         
        public int DepartamentoId { get; set; }

        public string NombreDeptoPer { get; set; }



        public async Task<List<UsuarioViewModel>> ObtenerUsuarios()
        {
            List<UsuarioViewModel> model = new List<UsuarioViewModel>();
            var c = (from u in await UnitOfWork.UsuarioRepository.ObtenerTodosAsin(includeProperties: "Rol")
                     select u).ToList();
            //foreach (var i in c)
            //{
            //    var m = mapper.Map<Usuario, UsuarioViewModel>(i);
            //    m.Rol = i.Rol.Nombre;
            //    if (m.Estatus == true)
            //        m.EstatusS = "ACTIVO";
            //    else
            //        m.EstatusS = "INACTIVO";
            //    model.Add(m);
            //}
            return model;
        }

        public async Task<UsuarioViewModel> ObtenerUsuario(string usuarioLogin)
        {
            var c = (from u in await UnitOfWork.UsuarioRepository.ObtenerTodosAsin(includeProperties: "departamento")
                     where u.UsuarioLogin == usuarioLogin
                     select u).FirstOrDefault();
            var model = mapper.Map<Usuario, UsuarioViewModel>(c);
            //model.Rol = c.Rol.Nombre;
            return model;
        }

    }
}
