using MTCG.DAL;
using MTCG.DAL.Exceptions;
using MTCG.Http.Attributes;
using MTCG.Http.Method;
using MTCG.Http.Protocol;
using MTCG.Http.Status;
using MTCG.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MTCG.Endpoints {
    [HttpEndpoint("/clearDB")]
    public class ClearDBEndpoint : Endpoint {
        public ClearDBEndpoint(HttpRequest request, HttpResponse response) : base(request, response) { }

        [HttpMethod(HttpMethod.DELETE)]
        public void ClearDB() {
            var dbAuthorization = "123455432100000";

            if(dbAuthorization != _request.Headers["Authorization"]) {
                _response.Send(HttpStatus.Unauthorized);
                return;
            }

            using UnitOfWork uow = new();

            uow.UserRepository.Get().ForEach(item => uow.UserRepository.Delete(item));
            uow.PackageRepository.Get().ForEach(item => uow.PackageRepository.Delete(item));

            uow.Save();
            _response.Send(HttpStatus.OK);

        }
    }
}
