using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL.DAO {
    public class CardDAO : IDAO<Card> {
        public NpgsqlConnection Connection { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Delete(Card entityToDelete) {
            throw new NotImplementedException();
        }

        public List<Card> GetAll() {
            throw new NotImplementedException();
        }

        public void Insert(Card entity) {
            throw new NotImplementedException();
        }

        public void Update(Card entityToUpdate) {
            throw new NotImplementedException();
        }
    }
}
