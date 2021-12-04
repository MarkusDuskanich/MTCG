using MTCG.DAL;
using MTCG.Http.Protocol;
using MTCG.Http.Status;
using System;
using System.Linq;

namespace MTCG.Endpoints {
    public class Endpoint {
        protected readonly HttpRequest _request;
        protected readonly HttpResponse _response;

        public Endpoint(HttpRequest request, HttpResponse response) {
            _request = request;
            _response = response;
        }

        public bool TryAuthorize() {
            using var uow = new UnitOfWork();
            var res = uow.UserRepository.Get(user => user.Token == _request.Headers["Authorization"]);

            if(res.Count != 1 || res[0].TokenExpiration <= DateTime.Now) {
                _response.Send(HttpStatus.Unauthorized);
                return false;
            }

            return true;
        }

        public Guid GetIdFromToken() {
            using var uow = new UnitOfWork();
            var res = uow.UserRepository.Get(user => user.Token == _request.Headers["Authorization"]);
            return res[0].Id;
        }
    }
}
