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

namespace MTCG.Endpoints.Stats {
    [HttpEndpoint("/stats")]
    public class StatsEndpoint : Endpoint {
        public StatsEndpoint(HttpRequest request, HttpResponse response) : base(request, response) {
        }

        [HttpMethod(HttpMethod.GET)]
        public void GetStats() {
            if (!TryAuthorize())
                return;

            using var uow = new UnitOfWork();

            var currentUser = uow.UserRepository.GetById(GetIdFromToken());

            var elo = 100 + 3 * currentUser.Wins - 5 * currentUser.Losses;

            var res = JsonConvert.SerializeObject(new Dictionary<string, string> {
                {"wins", currentUser.Wins.ToString() },
                {"losses", currentUser.Losses.ToString() },
                {"elo", elo.ToString() }
            });

            _response.Send(HttpStatus.OK, res);
        }

    }
}
