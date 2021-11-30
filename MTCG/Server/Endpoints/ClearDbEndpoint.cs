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
    [HttpEndpoint("/clearDb")]
    class ClearDbEndpoint : Endpoint {
        public ClearDbEndpoint(HttpRequest request, HttpResponse response) : base(request, response) { }

        [HttpMethod(HttpMethod.DELETE)]
        public void ClearDB() {
            using UnitOfWork uow = new();
            var res = uow.UserRepository.Get();
            res.ForEach(item => uow.UserRepository.Delete(item));

            try {
                uow.Save();
                _response.Send(HttpStatus.OK);
            } catch (Exception) {
                _response.Send(HttpStatus.InternalServerError);
            }
        }
    }
}
