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

namespace MTCG.Endpoints.Users {
    [HttpEndpoint("/users")]
    class UsersEndpoint : Endpoint {

        public UsersEndpoint(HttpRequest request, HttpResponse response) : base(request, response) { }
        
        [HttpMethod(HttpMethod.POST)]
        public void Register() {
            var credentials = JsonConvert.DeserializeObject<Dictionary<string, string>>(_request.Content);

            using UnitOfWork uow = new();


            if(credentials == null) {
                _response.Send(HttpStatus.BadRequest);
                return;
            }

            var duplicateUserName = uow.UserRepository.Get(user => user.UserName == credentials["Username"]);
            if (duplicateUserName.Count > 0) {
                _response.Send(HttpStatus.BadRequest, "Username already exists");
                return;
            }

            User user = new() {
                Id = Guid.NewGuid(),
                UserName = credentials["Username"],
                Password = credentials["Password"],
                TokenExpiration = DateTime.Now
            };

            uow.UserRepository.Insert(user);

            uow.Save();
            _response.Send(HttpStatus.OK);
        }
    
    }
}
 