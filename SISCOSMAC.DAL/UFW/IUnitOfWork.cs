using SISCOSMAC.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static SISCOSMAC.DAL.UFW.IGenericRepository;

namespace SISCOSMAC.DAL.UFW
{
    public interface IUnitOfWork
    {
        IGenericRepository<Departamento> DepartamentoRepository { get; }

        IGenericRepository<Usuario> UsuarioRepository { get; }

        IGenericRepository<SolicitudMantenimientoCorrectivo> SolicitudRepository { get; }

        void Dispose();

        Task<int> SaveAsync();
    }
}
