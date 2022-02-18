using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SISCOSMAC.DAL.Models
{
    public class OrdenTrabajo
    {
        [Key]
        public  int OrdenId { get; set; }

        [Required]
        public int NumeroControl { get; set; }

        [Required]
        public string Mantenimiento{ get; set; }

        [Required]
        public string TipoServicio { get; set; }

        [Required]
        public string Asignado { get; set; }

        [Required]
        public DateTime FechaRealizacion { get; set; }

        [Required]
        public string TrabajoRealizado { get; set; }

        [Required]
        public string VerificadoLiberado { get; set; }

        [Required]
        public string AprobadoPor { get; set; }

        [Required]
        public int SolicitudId { get; set; }

        [ForeignKey("SolicitudId")]
        public SolicitudMantenimientoCorrectivo solicitud { get; set; }
    }

}
