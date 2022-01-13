using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SISCOSMAC.DAL.UFW
{
    public interface IGenericRepository
    {
        public interface IGenericRepository<T> where T : class
        {
            Task<T> ObtenerPorIdAsin(object id);
            Task<T> ObtenerAsin(Expression<Func<T, bool>> match = null, string includeProperties = "");
            Task<IEnumerable<T>> ObtenerTodosAsin(Expression<Func<T, bool>> match = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
            Task<T> AgregarAsin(T entity);
            Task<T> ActualizarAsin(T entity, object key);
            void EliminarAsin(T entity);
        }
    }
}
