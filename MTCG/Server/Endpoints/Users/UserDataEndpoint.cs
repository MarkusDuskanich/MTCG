using MTCG.DAL;
using MTCG.Http.Attributes;
using MTCG.Http.Method;
using MTCG.Http.Protocol;
using MTCG.Http.Status;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Endpoints.Users {
    [HttpEndpoint("/users/{username}")]
    public class UserDataEndpoint : Endpoint {
        public UserDataEndpoint(HttpRequest request, HttpResponse response) : base(request, response) {}
        
        [HttpMethod(HttpMethod.GET)]
        public void GetData() {
            if (!TryAuthorize())
                return;

            using var uow = new UnitOfWork();
            var currentUser = uow.UserRepository.GetById(GetIdFromToken());
            if(currentUser.UserName != _request.PathParameters[0]) {
                _response.Send(HttpStatus.Unauthorized);
                return;
            }

            _response.Send(HttpStatus.OK, JsonConvert.SerializeObject(currentUser));
        }


        [HttpMethod(HttpMethod.PUT)]
        public void UpdateData() {
            if (!TryAuthorize())
                return;

            using var uow = new UnitOfWork();
            var currentUser = uow.UserRepository.GetById(GetIdFromToken());
            if (currentUser.UserName != _request.PathParameters[0]) {
                _response.Send(HttpStatus.Unauthorized);
                return;
            }

            var newUserData = JsonConvert.DeserializeObject<Dictionary<string, string>>(_request.Content);

            currentUser.Name = newUserData["Name"];
            currentUser.Bio = newUserData["Bio"];
            currentUser.Image = newUserData["Image"];

            uow.UserRepository.Update(currentUser);
            uow.TrySave();
 
            _response.Send(HttpStatus.OK);
        }

    }
}
