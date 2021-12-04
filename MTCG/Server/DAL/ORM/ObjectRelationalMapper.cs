using MTCG.DAL.Exceptions;
using MTCG.Models;
using MTCG.Models.Attributes;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MTCG.DAL.ORM {
    public class ObjectRelationalMapper{

        public NpgsqlConnection Connection { get; }

        public ObjectRelationalMapper(NpgsqlConnection connection) {
            Connection = connection;
        }

        public void Delete<TEntity>(TEntity entityToDelete) where TEntity : class, ITEntity {
            string query = $"DELETE FROM {GetTableName(entityToDelete.GetType())} WHERE id = @id";

            using NpgsqlCommand command = new NpgsqlCommand(query, Connection);
            command.Parameters.AddWithValue("@id", entityToDelete.Id);

            int result = command.ExecuteNonQuery();

            if (result < 0)
                throw new StaleObjectStateException($"delete in table {entityToDelete.GetType()}");
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

            string query = $"UPDATE {GetTableName(entityToUpdate.GetType())} SET ({columns})" +
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
                throw new StaleObjectStateException($"update in table {entityToUpdate.GetType()}");
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

            string query = $"INSERT INTO {GetTableName(entityToInsert.GetType())} ({columns}) VALUES ({param})";
            using NpgsqlCommand command = new NpgsqlCommand(query, Connection);

            foreach (var item in propInfo) {
                command.Parameters.AddWithValue($"@{item.Name.ToLower()}", item.GetValue(entityToInsert));
            }

            int result = command.ExecuteNonQuery();

            if (result < 0)
                throw new StaleObjectStateException($"insert in table {entityToInsert.GetType()}");
        }

        public List<TEntity> GetAll<TEntity>() where TEntity : class, ITEntity {
            using var command = Connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {GetTableName(typeof(TEntity))}";
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

                    try {
                        MethodInfo parse = targetProp.PropertyType.GetMethod("Parse", new Type[] { typeof(string) });
                        targetProp.SetValue(entity, parse.Invoke(null, new object[] { reader[i].ToString() }));
                    } catch (Exception) {
                        throw new OrmException($"Could not map prop<{targetProp.Name}> to col name<{columnNames[i]}>");
                    }
                }
                result.Add(entity);
            }

            return result;
        }
        private static string GetTableName(Type entityType){
            var res = entityType.GetCustomAttribute<DataSourceAttribute>(false)?.TableName;
            if (res == null)
                throw new OrmException($"Could not get table name of {entityType.Name}");
            return res;
        }

        private static PropertyInfo GetPropertyFromString<TEntity>(TEntity entity, string s) where TEntity : class, ITEntity {
            return entity.GetType().GetProperties().ToList().Find(propInfo => propInfo.Name.ToLower() == s.ToLower());
        }

        private static string[] GetColumnNames(NpgsqlDataReader reader) {
            var columnNames = from item in reader.GetColumnSchema() select item.ColumnName.ToLower();
            return columnNames.ToArray();
        }
    }
}
