
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
    [HttpEndpoint("/sessions")]
    public class SessionsEndpoint : Endpoint {
        public SessionsEndpoint(HttpRequest request, HttpResponse response) : base(request, response) {
        }

        [HttpMethod(HttpMethod.POST)]
        public void Login() {
            //unique feature: check last login, loginstreak etc.

            //user sends password and name
            var credentials = JsonConvert.DeserializeObject<Dictionary<string, string>>(_request.Content);

            using var uow = new UnitOfWork();
            var res = uow.UserRepository.Get(user => credentials["Password"] == user.Password && credentials["Username"] == user.UserName);

            //if no entry or no match return 403
            if(res.Count != 1) {
                _response.Send(HttpStatus.Unauthorized);
                return;
            }

            //if entry match generate new token, update the user with token timeout
            var token = "Basic " + credentials["Username"] + "-mtcgToken";
            _response.Content = JsonConvert.SerializeObject(new Dictionary<string, string> { { "Token", token } });

            res[0].Token = token;
            res[0].TokenExpiration = DateTime.Now.AddHours(1);

            uow.UserRepository.Update(res[0]);
            uow.Save();
            _response.Send(HttpStatus.OK);
        }
    }
}
