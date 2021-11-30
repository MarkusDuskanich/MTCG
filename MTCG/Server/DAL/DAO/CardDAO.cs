using MTCG.DAL.Exceptions;
using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL.DAO {
    public class CardDAO : IDAO<Card> {
        public NpgsqlConnection Connection { get; set; } = null;

        public void Delete(Card entityToDelete) {
            if (Connection == null)
                throw new NoDbConnectionException($"connection is null in {GetType().Name}");

            string query = "DELETE FROM cards WHERE id = @id";

            using NpgsqlCommand command = new NpgsqlCommand(query, Connection);
            command.Parameters.AddWithValue("@id", entityToDelete.Id);

            int result = command.ExecuteNonQuery();

            if (result < 0)
                throw new StaleObjectStateException($"delete in table {GetType().Name}");
        }

        public List<Card> GetAll() {
            if (Connection == null)
                throw new NoDbConnectionException($"connection is null in {GetType().Name}");

            using var command = Connection.CreateCommand();
            command.CommandText = "SELECT * FROM cards";
            using var reader = command.ExecuteReader();
            List<Card> res = new();

            while (reader.Read()) {
                Card card = new();

                card.Id = reader.GetGuid(0);
                card.UserId = reader.GetGuid(1);
                card.Name = reader[2].ToString();
                card.Damage = reader.GetInt32(3);
                card.InDeck = reader.GetBoolean(4);
                card.IsTradeOffer = reader.GetBoolean(5);
                card.Version = reader.GetInt32(6);

                res.Add(card);
            }

            return res;
        }

        public void Insert(Card entity) {
            if (Connection == null)
                throw new NoDbConnectionException($"connection is null in {GetType().Name}");

            string query = "INSERT INTO cards (id,userid,name,damage,indeck,istradeoffer,version) VALUES" +
                " (@id,@userid,@name,@damage,@indeck,@istradeoffer,@version)";

            using NpgsqlCommand command = new NpgsqlCommand(query, Connection);
            command.Parameters.AddWithValue("@id", entity.Id);
            command.Parameters.AddWithValue("@userid", entity.UserId);
            command.Parameters.AddWithValue("@name", entity.Name ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@damage", entity.Damage);
            command.Parameters.AddWithValue("@indeck", entity.InDeck);
            command.Parameters.AddWithValue("@istradeoffer", entity.IsTradeOffer);
            command.Parameters.AddWithValue("@version", entity.Version);

            int result = command.ExecuteNonQuery();

            if (result < 0)
                throw new StaleObjectStateException($"insert in table {GetType().Name}");
        }

        public void Update(Card entityToUpdate) {
            if (Connection == null)
                throw new NoDbConnectionException($"connection is null in {GetType().Name}");

            string query = "UPDATE cards SET (id,userid,name,damage,indeck,istradeoffer,version)" +
                "= (@id,@userid,@name,@damage,@indeck,@istradeoffer,@newversion)" +
                $"WHERE id = @id AND version = @oldversion";

            using NpgsqlCommand command = new NpgsqlCommand(query, Connection);
            command.Parameters.AddWithValue("@id", entityToUpdate.Id);
            command.Parameters.AddWithValue("@userid", entityToUpdate.UserId);
            command.Parameters.AddWithValue("@name", entityToUpdate.Name ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@damage", entityToUpdate.Damage);
            command.Parameters.AddWithValue("@indeck", entityToUpdate.InDeck);
            command.Parameters.AddWithValue("@istradeoffer", entityToUpdate.IsTradeOffer);
            command.Parameters.AddWithValue("@newversion", entityToUpdate.Version + 1);
            command.Parameters.AddWithValue("@oldversion", entityToUpdate.Version);

            int result = command.ExecuteNonQuery();
            if (result == 0)
                throw new StaleObjectStateException($"update in table {GetType().Name}");
        }
    }
}
