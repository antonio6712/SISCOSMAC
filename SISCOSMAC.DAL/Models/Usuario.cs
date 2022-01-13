using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SISCOSMAC.DAL.Models
{
    public  class Usuario
    {
        [Key]
        public int UsuarioId{ get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string APaterno { get; set; }

        [Required]
        public string AMaterno { get; set; }

        [Required]
        public string UsuarioLogin { get; set; }

        [Required]
        [StringLength(32)]
        public byte[] PasswordHash { get; set; }

        [Required]
        [StringLength(32)]
        public byte[] PasswordSal { get; set; }

        [Required]
        public string Rol { get; set; }

        //Relaciones

        [Required]
        public int DepartamentoId { get; set; }

        [ForeignKey("DepartamentoId")]
        public Departamento departamento{ get; set; }

    }
}
