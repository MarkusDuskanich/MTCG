using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Server.HttpAttributes {
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpMethodAttribute : Attribute {
        public string Method { get; private set; }

        public HttpMethodAttribute(string method) {
            Method = method;
        }
    }
}
