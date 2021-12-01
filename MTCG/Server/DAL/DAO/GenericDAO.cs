using MTCG.DAL.Exceptions;
using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace MTCG.DAL.DAO {
    public class GenericDAO{

        public NpgsqlConnection Connection { get; }
        public string TableName { get; }

        public GenericDAO(NpgsqlConnection connection, string tableName) {
            Connection = connection;
            TableName = tableName;
        }

        public void Delete<TEntity>(TEntity entityToDelete) where TEntity : class, ITEntity {

            string query = $"DELETE FROM {TableName} WHERE id = @id";

            using NpgsqlCommand command = new NpgsqlCommand(query, Connection);
            command.Parameters.AddWithValue("@id", entityToDelete.Id);

            int result = command.ExecuteNonQuery();

            if (result < 0)
                throw new StaleObjectStateException($"delete in table {TableName}");
        }

        public void Update<TEntity>(TEntity entityToUpdate) where TEntity : class, ITEntity {
            var propInfo = entityToUpdate.GetType().GetProperties();
            var propNames = from info in propInfo select info.Name.ToLower();

            string columns = "";
            string param = "";

            propNames.ToList().ForEach(item => {
                columns += item + ",";
                param += "@" + item + ",";
            });


            columns = columns.Remove(columns.Length - 1);
            param = param.Remove(param.Length - 1);

            string query = $"UPDATE {TableName} SET ({columns})" +
                $"= ({param})" +
                $"WHERE id = @id AND version = @oldversion";

            using NpgsqlCommand command = new NpgsqlCommand(query, Connection);

            entityToUpdate.Version++;

            foreach (var item in propInfo) {
                command.Parameters.AddWithValue($"@{item.Name.ToLower()}", item.GetValue(entityToUpdate));
            }
            command.Parameters.AddWithValue("@oldversion", entityToUpdate.Version - 1);

            int result = command.ExecuteNonQuery();

            if (result == 0)
                throw new StaleObjectStateException($"update in table {TableName}");
        }

        public void Insert<TEntity>(TEntity entityToInsert) where TEntity : class, ITEntity {
            var propInfo = entityToInsert.GetType().GetProperties();
            var propNames = from info in propInfo select info.Name.ToLower();

            string columns = "";
            string param = "";

            propNames.ToList().ForEach(item => {
                columns += item + ",";
                param += "@" + item + ",";
            });

            columns = columns.Remove(columns.Length - 1);
            param = param.Remove(param.Length - 1);

            string query = $"INSERT INTO {TableName} ({columns}) VALUES ({param})";
            using NpgsqlCommand command = new NpgsqlCommand(query, Connection);

            foreach (var item in propInfo) {
                command.Parameters.AddWithValue($"@{item.Name.ToLower()}", item.GetValue(entityToInsert));
            }

            int result = command.ExecuteNonQuery();

            if (result < 0)
                throw new StaleObjectStateException($"insert in table {TableName}");
        }

        public List<TEntity> GetAll<TEntity>() where TEntity : class, ITEntity {
            using var command = Connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {TableName}";
            using var reader = command.ExecuteReader();

            var columnNames = GetColumnNames(reader);

            List<TEntity> result = new();

            while (reader.Read()) {
                var entity = Activator.CreateInstance(typeof(TEntity)) as TEntity;
                for (int i = 0; i < reader.FieldCount; i++) {
                    PropertyInfo targetProp = GetPropertyFromString(entity, columnNames[i]);

                    if (targetProp.PropertyType == typeof(string)) {
                        targetProp.SetValue(entity, reader[i].ToString());
                        continue;
                    }

                    MethodInfo parse = targetProp.PropertyType.GetMethod("Parse", new Type[] { typeof(string) });
                    if (parse != null)
                        targetProp.SetValue(entity, parse.Invoke(null, new object[] { reader[i].ToString() }));
                    else
                        throw new OrmException($"Could not map prop<{targetProp.Name}> to col name<{columnNames[i]}>");
                }
                result.Add(entity);
            }

            return result;
        }

        private static PropertyInfo GetPropertyFromString<TEntity>(TEntity entity, string s) where TEntity : class, ITEntity {
            return entity.GetType().GetProperties().ToList().Find(propInfo => propInfo.Name.ToLower() == s);
        }

        private static string[] GetColumnNames(NpgsqlDataReader reader) {
            var columnNames = from item in reader.GetColumnSchema() select item.ColumnName.ToLower();
            return columnNames.ToArray();
        }
    }
}
