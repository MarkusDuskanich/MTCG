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

            //user sends password and name
            var credentials = JsonConvert.DeserializeObject<Dictionary<string, string>>(_request.Content);

            using var uow = new UnitOfWork();
            var res = uow.UserRepository.Get(user => credentials["Password"] == user.Password && credentials["Username"] == user.UserName);

            //if no entry or no match return 403
            if(res.Count != 1) {
                _response.Send(HttpStatus.Unauthorized);
                return;
            }

            var user = res[0];

            //if entry match generate new token, update the user with token timeout
            var token = "Basic " + credentials["Username"] + "-mtcgToken";
            _response.Content = JsonConvert.SerializeObject(new Dictionary<string, string> { { "Token", token } });
            user.Token = token;
            user.TokenExpiration = DateTime.Now.AddHours(4);

            //check if last login was yesterday
            //if it was update the login-streak and give user reward
            //if it was longer ago than that, reset login-streak
            //query params to easier test feature
            var getRewardIsSet = _request.QueryParameters.ContainsKey("getReward");
            var breakStreakIsSet = _request.QueryParameters.ContainsKey("breakStreak");

            if (user.LastLogin.Date == DateTime.Now.AddDays(-1).Date || getRewardIsSet) {
                user.LoginStreak++;
                //rewards
                if (user.LoginStreak == 1) user.Coins++;
                else if (user.LoginStreak == 2 || user.LoginStreak == 3) user.Coins += 2;
                else user.Coins += 3;

            }else if (user.LastLogin.Date < DateTime.Now.AddDays(-1).Date || breakStreakIsSet) {
                user.LoginStreak = 0;
            }

            //update last login to now
            user.LastLogin = DateTime.Now;  

            uow.UserRepository.Update(user);
            uow.Save();
            _response.Send(HttpStatus.OK);
        }
    }
}
