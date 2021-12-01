using MTCG.DAL.ORM;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL.Context {
    public class MTCGContext : DBContext {

        public MTCGContext() : base("Host=localhost;Username=postgres;Password=postgres;Database=mtcgdb;IncludeErrorDetail=true") {
            LoadTable<User>();
            LoadTable<Card>();
            LoadTable<Package>();
            LoadTable<TradeOffer>();
        }
    }
}
