using MTCG.Http.Method;
using System;

namespace MTCG.Http.Attributes {
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpMethodAttribute : Attribute {
        public HttpMethod Method { get; set; }

        public HttpMethodAttribute(HttpMethod method) {
            Method = method;
        }
    }
}
