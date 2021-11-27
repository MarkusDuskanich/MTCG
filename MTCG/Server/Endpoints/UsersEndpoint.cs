using System;
using System.Linq;
using System.Threading;
using MTCG.Http.Attributes;
using MTCG.Http.Method;
using MTCG.Http.Protocol;
using MTCG.Http.Status;

namespace MTCG.Endpoints {
    [HttpEndpoint("/users")]
    public class UsersEndpoint : Endpoint {

        public UsersEndpoint(HttpRequest request, HttpResponse response) : base(request, response) { }

        [HttpMethod(HttpMethod.POST)]
        public void PostMethod() {
            Console.WriteLine("Post method in /users endpoint called");
            Console.WriteLine(_request.Content);
            _response.Send(HttpStatus.OK, @"{""hello"":""there""}");
        }
    }
}
