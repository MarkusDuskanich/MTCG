using MTCG.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models {
    [DataSource("users")]
    public class User : ITEntity {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Bio { get; set; } = "";
        public string Image { get; set; } = "";
        public int Coins { get; set; } = 20;
        public int Wins { get; set; } = 0;
        public int Losses { get; set; } = 0;
        public string Token { get; set; } = "";
        public DateTime TokenExpiration { get; set; } = DateTime.Now;
        public DateTime LastLogin { get; set; } = DateTime.Now;
        public int LoginStreak { get; set; } = 0;
        public int Version { get; set; } = 1;

        public User() { }

        public User(User origin) {
            Id = origin.Id;
            UserName = origin.UserName;
            Password = origin.Password;
            Bio = origin.Bio;
            Image = origin.Image;
            Coins = origin.Coins;
            Wins = origin.Wins;
            Losses = origin.Losses;
            Token = origin.Token;
            TokenExpiration = origin.TokenExpiration;
            LastLogin = origin.LastLogin;
            LoginStreak = origin.LoginStreak;
            Version = origin.Version;
        }
    }
}
