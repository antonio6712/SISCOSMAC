using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SISCOSMAC.DAL.Models
{
    public class Departamento
    {
        [Key]        
        public int DepartamentoId { get; set; }

        [Required]
        public string NombreDepartamento { get; set; }

        public bool OrdenTrabajo { get; set; }

    }
}
