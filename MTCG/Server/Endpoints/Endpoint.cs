using MTCG.Http.Protocol;

namespace MTCG.Endpoints {
    public class Endpoint {
        protected readonly HttpRequest _request;
        protected readonly HttpResponse _response;

        public Endpoint(HttpRequest request, HttpResponse response) {
            _request = request;
            _response = response;
        }
    }
}
