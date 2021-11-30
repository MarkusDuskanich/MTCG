using MTCG.DAL.DAO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL.Context {
    public class DBTable<TEntity> : IQueryable<TEntity> where TEntity : class {

        public Dictionary<TEntity, EntityState> Entities { get; } = new();

        public DBTable(List<TEntity> entities) {
            entities.ForEach(entity => Entities.Add(entity, EntityState.Unchanged));
        }

        private bool CompareEntityId(TEntity entity, Guid id) {
            return (Guid)typeof(TEntity).GetProperty("Id").GetValue(entity) == id;
        }

        public TEntity Find(Guid id) {
            foreach (var entity in Entities.Keys) {
                if (CompareEntityId(entity, id) && Entities[entity] != EntityState.Deleted)
                    return entity;
            }
            return null;
        }

        public EntityState State(TEntity entity) {
            return Entities[entity];
        }

        public void Update(TEntity entityToUpdate) {
            var destination = Find((Guid)typeof(TEntity).GetProperty("Id").GetValue(entityToUpdate));
            var sourceProperties = typeof(TEntity).GetProperties();
            foreach (var sourceProp in sourceProperties) {
                var targetProp = entityToUpdate.GetType().GetProperty(sourceProp.Name);
                targetProp.SetValue(destination, sourceProp.GetValue(entityToUpdate, null), null);
            }
            Entities[destination] = EntityState.Modified;
        }

        public void Delete(TEntity entityToUpdate) {
            Entities[entityToUpdate] = EntityState.Deleted;
        }

        public void Add(TEntity entity) {
            Entities.Add(entity, EntityState.Added);
        }

        public Type ElementType => Entities.Keys.AsQueryable().ElementType;

        public Expression Expression => Entities.Keys.AsQueryable().Expression;

        public IQueryProvider Provider => Entities.Keys.AsQueryable().Provider;

        public IEnumerator<TEntity> GetEnumerator() {
            return Entities.Keys.AsQueryable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Entities.Keys.AsQueryable().GetEnumerator();
        }
    }
}