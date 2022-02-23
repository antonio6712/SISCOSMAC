

using SISCOSMAC.DAL.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SISCOSMAC.Web.ViewModels
{
    public class SolicitudMantenimientoCorrectivoVM
    {
        [Key]
        [Required]
        public int SolicitudId { get; set; }

        
        public int? Folio { get; set; }

        [Required(ErrorMessage ="Este campo es requerido")]
        [Display(Name = "Departamento Dirigido")]
        public string DepartamentoDirigido { get; set; }

        [Required]
        [Display(Name = "Area Solicitante")]
        public string AreaSolicitante { get; set; }

        //[Required(ErrorMessage = "Este campo es requerido")]
        //[Display(Name = "Nombre y Firma del Solicitante")]
        public string NombreSolicitante { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [Display(Name = "Fecha de Elaboración")]
        public DateTime FechaElaboracion { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [Display(Name = "Descripción del Servicio ")]
        public String DescripcionServicios { get; set; }
        
        [Required]
        public int UsuarioId { get; set; }
                
        public string   NombreUsuSoli { get; set; }
        
        
    }
}
