using SISCOSMAC.DAL.DbContextSql;
using SISCOSMAC.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static SISCOSMAC.DAL.UFW.IGenericRepository;

namespace SISCOSMAC.DAL.UFW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ContextSql _db;


        public UnitOfWork(ContextSql db)
        {
            _db = db;
        }

        private IGenericRepository<Departamento> _departamentoRepository;

        public IGenericRepository<Departamento> DepartamentoRepository
        {
            get
            {
                return _departamentoRepository ??= new GenericRepository<Departamento>(_db);
            }
        }


        private IGenericRepository<Usuario> _usuarioRepository;

        public IGenericRepository<Usuario> UsuarioRepository
        {
            get
            {
                return _usuarioRepository ??= new GenericRepository<Usuario>(_db);
            }
        }

        private IGenericRepository<SolicitudMantenimientoCorrectivo> _solicitudRepository;

        public IGenericRepository<SolicitudMantenimientoCorrectivo> SolicitudRepository
        {
            get
            {
                return _solicitudRepository ??= new GenericRepository<SolicitudMantenimientoCorrectivo>(_db);
            }
        }

        private IGenericRepository<OrdenTrabajo> _ordenRepository;
        public IGenericRepository<OrdenTrabajo> OrdenRepository
        {
            get
            {
                return _ordenRepository ??= new GenericRepository<OrdenTrabajo>(_db);
            }
        }

        public async Task<int> SaveAsync()
        {
            return await _db.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    _db.Dispose();
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
