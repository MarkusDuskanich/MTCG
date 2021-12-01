using MTCG.DAL.DAO;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL.Context {
    public class MTCGContext : DBContext {

        public MTCGContext() : base("Host=localhost;Username=postgres;Password=postgres;Database=mtcgdb;IncludeErrorDetail=true") {
            LoadTable<User>("users");
            LoadTable<Card>("cards");
            LoadTable<Package>("packages");
            LoadTable<TradeOffer>("tradeoffers");
        }
    }
}
