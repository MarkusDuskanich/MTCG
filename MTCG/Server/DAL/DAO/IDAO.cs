using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL.DAO {
    public interface IDAO<TEntity> where TEntity : class{
        public NpgsqlConnection Connection { get; set; }
        List<TEntity> GetAll();
        void Update(TEntity entityToUpdate);
        void Insert(TEntity entity);
        void Delete(TEntity entityToDelete);
    }
}
