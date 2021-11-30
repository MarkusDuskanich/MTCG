using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL.DAO {
    public class PackageDAO : IDAO<Package> {
        public NpgsqlConnection Connection { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Delete(Package entityToDelete) {
            throw new NotImplementedException();
        }

        public List<Package> GetAll() {
            throw new NotImplementedException();
        }

        public void Insert(Package entity) {
            throw new NotImplementedException();
        }

        public void Update(Package entityToUpdate) {
            throw new NotImplementedException();
        }
    }
}
