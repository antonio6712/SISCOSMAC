using Microsoft.EntityFrameworkCore;
using SISCOSMAC.DAL.DbContextSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static SISCOSMAC.DAL.UFW.IGenericRepository;

namespace SISCOSMAC.DAL.UFW
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        internal ContextSql _db = null;
        internal DbSet<T> entidades = null;

        public GenericRepository(ContextSql db)
        {
            _db = db;
            entidades = _db.Set<T>();
        }

        public virtual async Task<T> ObtenerPorIdAsin(object id)
        {
            return await entidades.FindAsync(id);
        }

        public virtual async Task<T> ObtenerAsin(Expression<Func<T, bool>> match = null, string includeProperties = "")
        {
            IQueryable<T> query = entidades;
            if (match != null)
                query = query.Where(match);

            foreach (var includePropertity in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includePropertity);
            }

            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task<IEnumerable<T>> ObtenerTodosAsin(Expression<Func<T, bool>> match = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = entidades;
            if (match != null)
            {
                query = query.Where(match);
            }

            foreach (var includePropertity in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includePropertity);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public virtual async Task<T> AgregarAsin(T entity)
        {
            await _db.Set<T>().AddAsync(entity);
            return entity;
        }


        public virtual async Task<T> ActualizarAsin(T entity, object key)
        {
            if (entity == null)
                return null;
            T exist = await _db.Set<T>().FindAsync(key);
            if (exist != null)
            {
                _db.Entry(exist).CurrentValues.SetValues(entity);
            }
            return exist;
        }

        public virtual void EliminarAsin(T entity)
        {
            _db.Set<T>().Remove(entity);
        }

        public DbSet<T> Entidades
        {
            get
            {
                if (entidades == null)
                {
                    this.entidades = _db.Set<T>();
                }
                return entidades;
            }
        }



    }
}
