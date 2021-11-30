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
using System.Linq;
using System.Text;

namespace MTCG.Endpoints {
    [HttpEndpoint("/users")]
    class UsersEndpoint : Endpoint {

        public UsersEndpoint(HttpRequest request, HttpResponse response) : base(request, response) { }
        
        [HttpMethod(HttpMethod.POST)]
        public void Register() {
            var credentials = JsonConvert.DeserializeObject<Dictionary<string, string>>(_request.Content);

            if(credentials == null) {
                _response.Send(HttpStatus.BadRequest);
                return;
            }

            User user = new() {
                Id = Guid.NewGuid(),
                UserName = credentials["Username"],
                Password = credentials["Password"]
            };

            using UnitOfWork uow = new();
            uow.UserRepository.Insert(user);

            if (uow.TrySave())
                _response.Send(HttpStatus.OK);
            else
                _response.Send(HttpStatus.InternalServerError);
        }
    
    }
}
 