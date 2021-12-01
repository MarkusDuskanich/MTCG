using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL.Exceptions {
    class DBConnectionException : Exception {

        public DBConnectionException() { }

        public DBConnectionException(string message) : base(message) { }

        public DBConnectionException(string message, Exception inner) : base(message, inner) { }
    }
}
