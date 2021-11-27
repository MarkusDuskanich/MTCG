using MTCG.Http.Protocol;
using MTCG.Http.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
