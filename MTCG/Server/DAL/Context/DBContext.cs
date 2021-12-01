using MTCG.DAL.ORM;
using MTCG.DAL.Exceptions;
using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL.Context {
    public class DBContext : IDisposable {
        public Dictionary<ITEntity, EntityState> Entities { get; private set; } = new();

        private readonly NpgsqlConnection _connection;

        public DBContext(string connectionString) {
            _connection = new NpgsqlConnection(connectionString);
            _connection.Open();
            if (_connection.State != ConnectionState.Open)
                throw new DBConnectionException("connection to DB not open");
        }

        public void LoadTable<TEntity>() where TEntity : class, ITEntity{
            var orm = new ObjectRelationalMapper(_connection);
            orm.GetAll<TEntity>().ForEach(item => Entities.Add(item, EntityState.Unchanged));
        }

        public DBTable<TEntity> Table<TEntity>() where TEntity : class, ITEntity{
            var res = new List<TEntity>();
            foreach (var entity in Entities.Keys) {
                if (entity.GetType() == typeof(TEntity))
                    res.Add(entity as TEntity);
            }
            return new(res);
        }

        public void Attach<TEntity>(TEntity entity) where TEntity : class, ITEntity {
            Entities.Add(entity, EntityState.Added);
        }

        public void SaveChanges() {
            try {
                using NpgsqlTransaction transaction = _connection.BeginTransaction();
                var orm = new ObjectRelationalMapper(_connection);
                foreach (var entity in Entities) {
                    if (entity.Value == EntityState.Unchanged)
                        continue;
  
                    if (entity.Value == EntityState.Added)
                        orm.Insert(entity.Key);
                    else if(entity.Value == EntityState.Modified)
                        orm.Update(entity.Key);
                    else if(entity.Value == EntityState.Deleted) {
                        orm.Delete(entity.Key);
                    }
                }

                transaction.Commit();

            } catch (Exception) {
                throw;
            }
        }

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing) {
            if (!_disposed) {
                if (disposing)
                    _connection.Dispose();
            }
            _disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
