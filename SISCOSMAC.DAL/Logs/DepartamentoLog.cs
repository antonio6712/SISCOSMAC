using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SISCOSMAC.DAL.Logs
{
    public class DepartamentoLog
    {
        [Key]
        public int DepartamentoLogId { get; set; }

        public String DepartamentoAfectado { get; set; }

        public string Movimiento{ get; set; }

        public DateTime Fecha { get; set; }

        public string Usuario { get; set; }
    }
}
