using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SISCOSMAC.DAL.Models
{
    public class SolicitudMantenimientoCorrectivo
    {
        [Key]
        [Required]
        public int SolicitudId { get; set; }

        
        public int? Folio { get; set; }

        [Required]
        public string DepartamentoDirigido { get; set; }

        [Required]
        public string AreaSolicitante { get; set; }

        [Required]
        public string NombreSolicitante { get; set; }

        [Required]
        public  DateTime FechaElaboracion { get; set; }

        [Required]
        public  String DescripcionServicios { get; set; }

        public bool Enviado { get; set; }

        public bool Recibido { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario usuario { get; set; }


    }
}
