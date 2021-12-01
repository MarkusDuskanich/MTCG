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
    class ClearDBEndpoint : Endpoint {
        public ClearDBEndpoint(HttpRequest request, HttpResponse response) : base(request, response) { }

        [HttpMethod(HttpMethod.DELETE)]
        public void ClearDB() {
            //add authorization so only admin token can delete db

            using UnitOfWork uow = new();

            uow.UserRepository.Get().ForEach(item => uow.UserRepository.Delete(item));
            uow.PackageRepository.Get().ForEach(item => uow.PackageRepository.Delete(item));

            uow.Save();
            _response.Send(HttpStatus.OK);

        }
    }
}
