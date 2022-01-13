using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SISCOSMAC.Web.ViewModels
{
    public class DepartamentoViewModel
    {
        [Display(Name ="Indicador")]
        public int DepartamentoId { get; set; }

        [Display(Name = "Nombre del departamento")]
        [Required]
        public string NombreDepto { get; set; }

        [Display(Name = "Ordenes de trabajo")]
        [Required]
        public bool OrdenTrabajo { get; set; }
    }
}
