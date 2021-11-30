using MTCG.DAL.Exceptions;
using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL.DAO {
    class TradeOfferDAO : IDAO<TradeOffer> {
        public NpgsqlConnection Connection { get; set; } = null;

        public void Delete(TradeOffer entityToDelete) {
            if (Connection == null)
                throw new NoDbConnectionException($"connection is null in {GetType().Name}");

            string query = "DELETE FROM tradeoffers WHERE id = @id";

            using NpgsqlCommand command = new NpgsqlCommand(query, Connection);
            command.Parameters.AddWithValue("@id", entityToDelete.Id);

            int result = command.ExecuteNonQuery();

            if (result < 0)
                throw new StaleObjectStateException($"delete in table {GetType().Name}");
        }

        public List<TradeOffer> GetAll() {
            if (Connection == null)
                throw new NoDbConnectionException($"connection is null in {GetType().Name}");

            using var command = Connection.CreateCommand();
            command.CommandText = "SELECT * FROM tradeoffers";
            using var reader = command.ExecuteReader();
            List<TradeOffer> res = new();

            while (reader.Read()) {
                TradeOffer tradeOffer = new();

                tradeOffer.Id = reader.GetGuid(0);
                tradeOffer.CardId = reader.GetGuid(1);
                tradeOffer.UserId = reader.GetGuid(2);
                tradeOffer.MustBeSpell = reader.GetBoolean(3);
                tradeOffer.Element = reader[4].ToString();
                tradeOffer.MinDamage = reader.GetInt32(5);
                tradeOffer.Version = reader.GetInt32(6);

                res.Add(tradeOffer);
            }

            return res;
        }

        public void Insert(TradeOffer entity) {
            if (Connection == null)
                throw new NoDbConnectionException($"connection is null in {GetType().Name}");

            string query = "INSERT INTO tradeoffers (id,cardid,userid,mustbespell,element,mindamage,version) VALUES" +
                " (@id,@cardid,@userid,@mustbespell,@element,@mindamage,@version)";

            using NpgsqlCommand command = new NpgsqlCommand(query, Connection);
            command.Parameters.AddWithValue("@id", entity.Id);
            command.Parameters.AddWithValue("@cardid", entity.CardId);
            command.Parameters.AddWithValue("@userid", entity.UserId);
            command.Parameters.AddWithValue("@mustbespell", entity.MustBeSpell);
            command.Parameters.AddWithValue("@element", entity.Element ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@mindamage", entity.MinDamage);
            command.Parameters.AddWithValue("@version", entity.Version);

            int result = command.ExecuteNonQuery();

            if (result < 0)
                throw new StaleObjectStateException($"insert in table {GetType().Name}");
        }

        public void Update(TradeOffer entityToUpdate) {
            if (Connection == null)
                throw new NoDbConnectionException($"connection is null in {GetType().Name}");

            string query = "UPDATE tradeoffers SET (id,cardid,userid,mustbespell,element,mindamage,version)" +
                "= (@id,@cardid,@userid,@mustbespell,@element,@mindamage,@version)" +
                $"WHERE id = @id AND version = @oldversion";

            using NpgsqlCommand command = new NpgsqlCommand(query, Connection);
            command.Parameters.AddWithValue("@id", entityToUpdate.Id);
            command.Parameters.AddWithValue("@cardid", entityToUpdate.CardId);
            command.Parameters.AddWithValue("@userid", entityToUpdate.UserId);
            command.Parameters.AddWithValue("@mustbespell", entityToUpdate.MustBeSpell);
            command.Parameters.AddWithValue("@element", entityToUpdate.Element ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@mindamage", entityToUpdate.MinDamage);
            command.Parameters.AddWithValue("@newversion", entityToUpdate.Version + 1);
            command.Parameters.AddWithValue("@oldversion", entityToUpdate.Version);

            int result = command.ExecuteNonQuery();
            if (result == 0)
                throw new StaleObjectStateException($"update in table {GetType().Name}");
        }
    }
}
