using Microsoft.EntityFrameworkCore;
using SISCOSMAC.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISCOSMAC.DAL.DbContextSql
{
    public class ContextSql : DbContext
    {
        public ContextSql(DbContextOptions<ContextSql> options) : base(options) { }

        public DbSet<Departamento> Departamento { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<SolicitudMantenimientoCorrectivo> SolicitudMantenimientoCorrectivo { get; set; }
        public DbSet<OrdenTrabajo> OrdenTrabajo { get; set; }

    }

}
