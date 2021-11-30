using MTCG.DAL.Exceptions;
using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;

namespace MTCG.DAL.DAO {
    public class UserDAO : IDAO<User> {

        public NpgsqlConnection Connection { get; set; } = null;

        public void Delete(User entityToDelete) {
            if (Connection == null)
                throw new NoDbConnectionException($"connection is null in {GetType().Name}");

            string query = "DELETE FROM users WHERE id = @id";

            using NpgsqlCommand command = new NpgsqlCommand(query, Connection);
            command.Parameters.AddWithValue("@id", entityToDelete.Id);

            int result = command.ExecuteNonQuery();

            if (result < 0)
                throw new StaleObjectStateException($"delete in table {GetType().Name}");
        }

        public List<User> GetAll() {
            if (Connection == null)
                throw new NoDbConnectionException($"connection is null in {GetType().Name}");

            using var command = Connection.CreateCommand();
            command.CommandText = "SELECT * FROM users";
            using var reader = command.ExecuteReader();
            List<User> res = new();

            while (reader.Read()) {
                User user = new();

                user.Id = reader.GetGuid(0);
                user.UserName = reader[1].ToString();
                user.Password = reader[2].ToString();
                user.Bio = reader[3].ToString();
                user.Image = reader[4].ToString();
                user.Coins = reader.GetInt32(5);
                user.Wins = reader.GetInt32(6);
                user.Losses = reader.GetInt32(7);
                user.Token = reader[8].ToString();
                try {
                    user.TokenExpiration = reader.GetDateTime(9);
                } catch (Exception) { }
                try {
                    user.LastLogin = reader.GetDateTime(10);
                } catch (Exception) { }
                user.LoginStreak = reader.GetInt32(11);
                user.Version = reader.GetInt32(12);

                res.Add(user);
            }

            return res;
        }

        public void Insert(User entity) {
            if (Connection == null)
                throw new NoDbConnectionException($"connection is null in {GetType().Name}");

            string query = "INSERT INTO users (id,username,password,bio,image,coins,wins,losses,token,tokenexpiration,lastlogin,loginstreak,version) VALUES" +
                " (@id,@username,@password,@bio,@image,@coins,@wins,@losses,@token,@tokenexpiration,@lastlogin,@loginstreak,@version)";

            using NpgsqlCommand command = new NpgsqlCommand(query, Connection);
            command.Parameters.AddWithValue("@id", entity.Id);
            command.Parameters.AddWithValue("@username", entity.UserName);
            command.Parameters.AddWithValue("@password", entity.Password);
            command.Parameters.AddWithValue("@bio", entity.Bio ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@image", entity.Image ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@coins", entity.Coins);
            command.Parameters.AddWithValue("@wins", entity.Wins);
            command.Parameters.AddWithValue("@losses", entity.Losses);
            command.Parameters.AddWithValue("@token", entity.Token ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@tokenexpiration", entity.TokenExpiration);
            command.Parameters.AddWithValue("@lastlogin", entity.LastLogin);
            command.Parameters.AddWithValue("@loginstreak", entity.LoginStreak);
            command.Parameters.AddWithValue("@version", entity.Version);

            int result = command.ExecuteNonQuery();

            if (result < 0)
                throw new StaleObjectStateException($"insert in table {GetType().Name}");
        }

        public void Update(User entityToUpdate) {
            if (Connection == null)
                throw new NoDbConnectionException($"connection is null in {GetType().Name}");

            string query = "UPDATE users SET (username,bio,password,image,coins,wins,losses,token,tokenexpiration,lastlogin,loginstreak,version)" +
                "= (@username,@password,@bio,@image,@coins,@wins,@losses,@token,@tokenexpiration,@lastlogin,@loginstreak,@newversion)" +
                $"WHERE id = @id AND version = @oldversion";

            using NpgsqlCommand command = new NpgsqlCommand(query, Connection);
            command.Parameters.AddWithValue("@id", entityToUpdate.Id);
            command.Parameters.AddWithValue("@username", entityToUpdate.UserName);
            command.Parameters.AddWithValue("@password", entityToUpdate.Password);
            command.Parameters.AddWithValue("@bio", entityToUpdate.Bio ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@image", entityToUpdate.Image ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@coins", entityToUpdate.Coins);
            command.Parameters.AddWithValue("@wins", entityToUpdate.Wins);
            command.Parameters.AddWithValue("@losses", entityToUpdate.Losses);
            command.Parameters.AddWithValue("@token", entityToUpdate.Token ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@tokenexpiration", entityToUpdate.TokenExpiration);
            command.Parameters.AddWithValue("@lastlogin", entityToUpdate.LastLogin);
            command.Parameters.AddWithValue("@loginstreak", entityToUpdate.LoginStreak);
            command.Parameters.AddWithValue("@newversion", entityToUpdate.Version + 1);
            command.Parameters.AddWithValue("@oldversion", entityToUpdate.Version);

            int result = command.ExecuteNonQuery();
            if (result == 0)
                throw new StaleObjectStateException($"update in table {GetType().Name}");
        }
    }
}
