using System;
using System.Linq;
using System.Threading;
using MTCG.Http.Attributes;
using MTCG.Http.Method;
using MTCG.Http.Protocol;
using MTCG.Http.Status;

namespace MTCG.Endpoints {
    [HttpEndpoint("/test")]
    public class TestEndpoint : Endpoint {

        public TestEndpoint(HttpRequest request, HttpResponse response) : base(request, response) { }

        [HttpMethod(HttpMethod.GET)]
        public void TestMethod() {
            _response.Send(HttpStatus.OK, _request.Content);
        }
    }
}
