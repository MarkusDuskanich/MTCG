﻿using MTCG.DAL.DAO;
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
        private readonly Dictionary<Type, dynamic> _typeToDao = new();

        private readonly NpgsqlConnection _connection;


        public DBContext(string connectionString) {
            _connection = new NpgsqlConnection(connectionString);
            _connection.Open();
            if (_connection.State == ConnectionState.Closed)
                throw new ArgumentException("No connection do db possible");
        }

        public void LoadTable<TEntity>(string tableName) where TEntity : class, ITEntity{
            var dao = new GenericDAO<TEntity>(_connection, tableName);
            _typeToDao.Add(typeof(TEntity), dao);
            dao.GetAll().ForEach(item => Entities.Add(item, EntityState.Unchanged));
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
                foreach (var entity in Entities) {
                    if (entity.Value == EntityState.Unchanged)
                        continue;

                    var dao = _typeToDao[entity.Key.GetType()];
                    if (entity.Value == EntityState.Added)
                        dao.Insert((ITEntity)entity.Key);
                    else if(entity.Value == EntityState.Modified)
                        dao.Update((ITEntity)entity.Key);
                    else if(entity.Value == EntityState.Deleted) {
                        dao.Delete((ITEntity)entity.Key);
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
