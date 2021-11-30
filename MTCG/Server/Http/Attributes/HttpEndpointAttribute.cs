using System;

namespace MTCG.Http.Attributes {
    [AttributeUsage(AttributeTargets.Class)]
    public class HttpEndpointAttribute : Attribute {
        public string Path { get; private set; }

        public HttpEndpointAttribute(string path) {
            Path = path;
        }
    }
}
