using MTCG.DAL;
using MTCG.DAL.Exceptions;
using MTCG.Http.Attributes;
using MTCG.Http.Method;
using MTCG.Http.Protocol;
using MTCG.Http.Status;
using MTCG.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MTCG.Endpoints.Battle {
    [HttpEndpoint("/battles")]
    public class BattlesEndpoint : Endpoint {
        public BattlesEndpoint(HttpRequest request, HttpResponse response) : base(request, response) { }

        [HttpMethod(HttpMethod.POST)]
        public void Battle() {
            if (!TryAuthorize())
                return;

            using var uow = new UnitOfWork();

            var user = uow.UserRepository.GetById(GetIdFromToken());

            if (uow.CardRepository.Get(card => card.InDeck && card.UserId == user.Id).Count != 4) {
                _response.Send(HttpStatus.Forbidden);
                return;
            }

            BattleHandler.Instance.RegisterForBattle(user);

            var result = BattleHandler.Instance.GetBattleLog(user);

            if (result != null)
                _response.Send(HttpStatus.OK, result.ToString());
            else
                _response.Send(HttpStatus.ServiceUnavailable, JsonConvert.SerializeObject(new Dictionary<string, string>{ 
                    { "Timeout" , "No other users requested battle" } 
                }));
        }
    }
}
