using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace SISCOSMAC.Web.ViewModels
{
    public class OrdenTrabajoVM
    {

        public int OrdenId { get; set; }
            
        [Required (ErrorMessage = "Este Campo es Requierido")]
        [Display(Name = "Numero de Control")]
        public int NumeroControl { get; set; }

        [Required(ErrorMessage = "Este Campo es Requerido")]
        [Display(Name = "Mantenimiento")]
        public string Mantenimiento { get; set; }

        [Required(ErrorMessage = "Este Campo es Requerido")]
        [Display(Name = "Tipo de Servicio")]
        public string TipoServicio { get; set; }

        [Required(ErrorMessage = "Este Campo es Requerido")]
        [Display(Name = "Asignado a")]
        public string Asignado { get; set; }

        [Required(ErrorMessage = "Este Campo es Requerido")]
        [Display(Name = "Fecha de Realización")]
        public DateTime FechaRealizacion { get; set; }

        [Required(ErrorMessage = "Este Campo es Requerido")]
        [Display(Name = "Trabajo Realizado")]
        public string TrabajoRealizado { get; set; }

        //[Required(ErrorMessage = "Este Campo es Requerido")]
        //[Display(Name = "Verificado y Liberado por")]
        public string VerificadoLiberado { get; set; }

        //[Required(ErrorMessage = "Este Campo es Requerido")]
        //[Display(Name = "Aprobado por")]
        public string AprobadoPor { get; set; }

        
        public int SolicitudId { get; set; }

                
    }

}
