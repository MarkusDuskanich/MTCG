using System;

namespace MTCG.Http.Attributes {
    [AttributeUsage(AttributeTargets.Class)]
    public class HttpEndpointAttribute : Attribute {
        public string Path { get; private set; }
        public const string PathParameter = "{pathParam}";

        public HttpEndpointAttribute(string path) {
            Path = path;
        }
    }
}
