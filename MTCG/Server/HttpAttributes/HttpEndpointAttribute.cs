using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Server.HttpAttributes {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class HttpEndpointAttribute : Attribute {
        public string Path { get; private set; }

        public HttpEndpointAttribute(string path) {
            Path = path;
        }
    }
}
