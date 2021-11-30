using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL.DAO {
    class TradeOfferDAO : IDAO<TradeOffer> {
        public NpgsqlConnection Connection { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Delete(TradeOffer entityToDelete) {
            throw new NotImplementedException();
        }

        public List<TradeOffer> GetAll() {
            throw new NotImplementedException();
        }

        public void Insert(TradeOffer entity) {
            throw new NotImplementedException();
        }

        public void Update(TradeOffer entityToUpdate) {
            throw new NotImplementedException();
        }
    }
}
