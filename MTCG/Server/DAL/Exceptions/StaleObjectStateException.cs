using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL.Exceptions {
    class StaleObjectStateException : Exception {
        public StaleObjectStateException() { }

        public StaleObjectStateException(string message) : base(message) { }

        public StaleObjectStateException(string message, Exception inner) : base(message, inner) { }
    }
}
