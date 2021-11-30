using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL.Exceptions {
    class NoDbConnectionException : Exception {

        public NoDbConnectionException() { }

        public NoDbConnectionException(string message): base(message) { }

        public NoDbConnectionException(string message, Exception inner) : base(message, inner) { }
    }
}
