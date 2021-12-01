using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL.Exceptions {
    class OrmException : Exception {

        public OrmException() { }

        public OrmException(string message) : base(message) { }

        public OrmException(string message, Exception inner) : base(message, inner) { }
    }
}
