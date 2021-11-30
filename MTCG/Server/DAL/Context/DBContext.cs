﻿using System;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using MTCG.DAL.DAO;

namespace MTCG.DAL.Context {
    public class DBContext : IDisposable {
        private readonly Dictionary<Type, dynamic> _tables = new();
        private readonly Dictionary<Type, dynamic> _entityToDAO = new(); 

        private readonly NpgsqlConnection _connection;


        public DBContext(string connectionString) {
            _connection = new NpgsqlConnection(connectionString);
            _connection.Open();
            if (_connection.State == ConnectionState.Closed)
                throw new ArgumentException("No connection do db possible");
        }

        public void LoadTable<TEntity>(IDAO<TEntity> dao) where TEntity : class{
            dao.Connection = _connection;
            _tables.Add(typeof(TEntity), new DBTable<TEntity>(dao.GetAll()));
            _entityToDAO.Add(typeof(TEntity), dao);
        }

        public DBTable<TEntity> Table<TEntity>() where TEntity : class{
            return _tables[typeof(TEntity)] as DBTable<TEntity>;
        }        
        
        public void SaveChanges() {
            try {
                using NpgsqlTransaction transaction = _connection.BeginTransaction();
                foreach (var item in _tables) {
                    foreach(var entity in item.Value.Entities) {
                        if(entity.Value == EntityState.Added)
                            _entityToDAO[item.Key].Insert(entity.Key);
                        else if (entity.Value == EntityState.Modified)
                            _entityToDAO[item.Key].Update(entity.Key);
                        else if (entity.Value == EntityState.Deleted)
                            _entityToDAO[item.Key].Delete(entity.Key);
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