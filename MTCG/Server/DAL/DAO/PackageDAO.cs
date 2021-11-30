using MTCG.DAL.Exceptions;
using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL.DAO {
    public class PackageDAO : IDAO<Package> {
        public NpgsqlConnection Connection { get; set; } = null;

        public void Delete(Package entityToDelete) {
            if (Connection == null)
                throw new NoDbConnectionException($"connection is null in {GetType().Name}");

            string query = "DELETE FROM packages WHERE id = @id";

            using NpgsqlCommand command = new NpgsqlCommand(query, Connection);
            command.Parameters.AddWithValue("@id", entityToDelete.Id);

            int result = command.ExecuteNonQuery();

            if (result < 0)
                throw new StaleObjectStateException($"delete in table {GetType().Name}");
        }

        public List<Package> GetAll() {
            if (Connection == null)
                throw new NoDbConnectionException($"connection is null in {GetType().Name}");

            using var command = Connection.CreateCommand();
            command.CommandText = "SELECT * FROM packages";
            using var reader = command.ExecuteReader();
            List<Package> res = new();
            
            while (reader.Read()) {
                Package package = new();
                package.Id = reader.GetGuid(0);
                package.Name = reader[1].ToString();
                package.Damage = reader.GetInt32(2);
                package.Version = reader.GetInt32(3);

                res.Add(package);
            }

            return res;
        }

        public void Insert(Package entity) {
            if (Connection == null)
                throw new NoDbConnectionException($"connection is null in {GetType().Name}");

            string query = "INSERT INTO packages (id,name,damage,version) VALUES" +
                " (@id,@name,@damage,@version)";

            using NpgsqlCommand command = new NpgsqlCommand(query, Connection);
            command.Parameters.AddWithValue("@id", entity.Id);
            command.Parameters.AddWithValue("@name", entity.Name);
            command.Parameters.AddWithValue("@damage", entity.Damage);
            command.Parameters.AddWithValue("@version", entity.Version);

            int result = command.ExecuteNonQuery();

            if (result < 0)
                throw new StaleObjectStateException($"insert in table {GetType().Name}");
        }

        public void Update(Package entityToUpdate) {
            if (Connection == null)
                throw new NoDbConnectionException($"connection is null in {GetType().Name}");

            string query = "UPDATE packages SET (id,name,damage,version)" +
                "= (@id,@name,@damage,@newversion)" +
                $"WHERE id = @id AND version = @oldversion";

            using NpgsqlCommand command = new NpgsqlCommand(query, Connection);
            command.Parameters.AddWithValue("@id", entityToUpdate.Id);
            command.Parameters.AddWithValue("@name", entityToUpdate.Name);
            command.Parameters.AddWithValue("@damage", entityToUpdate.Damage);
            command.Parameters.AddWithValue("@newversion", entityToUpdate.Version + 1);
            command.Parameters.AddWithValue("@oldversion", entityToUpdate.Version);

            int result = command.ExecuteNonQuery();
            if (result == 0)
                throw new StaleObjectStateException($"update in table {GetType().Name}");
        }
    }
}
