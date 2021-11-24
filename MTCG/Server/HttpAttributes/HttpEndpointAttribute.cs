using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Server.HttpAttributes {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class HttpEndpointAttribute : Attribute {
        private string _path;

        public HttpEndpointAttribute(string path) {
            _path = path;
        }
    }
}
