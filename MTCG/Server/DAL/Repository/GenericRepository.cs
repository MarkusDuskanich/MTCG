using MTCG.DAL.Context;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL.Repository {
    public class GenericRepository<TEntity> where TEntity : class, ITEntity{
        private readonly MTCGContext _context;
        private readonly DBTable<TEntity> _table;

        public GenericRepository(MTCGContext mtcgContext) {
            _context = mtcgContext;
            _table = mtcgContext.Table<TEntity>();
        }

        public List<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, 
            IOrderedQueryable<TEntity>> orderBy = null) {

            IQueryable<TEntity> query = _table;

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                return orderBy(query).ToList();
            else
                return query.ToList();
        }

        public TEntity GetById(Guid id) {
            return _table.Find(id);
        }

        public void Insert(TEntity entity) {
            _context.Attach(entity);
            _table.Add(entity);
        }

        public void Delete(Guid id) {
            TEntity entityToDelete = _table.Find(id);
            Delete(entityToDelete);
        }

        public void Delete(TEntity entityToDelete) {
            _context.Entities[entityToDelete] = EntityState.Deleted;
            _table.Delete(entityToDelete);
        }

        public void Update(TEntity entityToUpdate) {
            _context.Entities[entityToUpdate] = EntityState.Modified;
            _table.Update(entityToUpdate);
        }
    }
}
