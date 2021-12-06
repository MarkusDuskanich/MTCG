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

namespace MTCG.Endpoints.Stats {
    [HttpEndpoint("score")]
    public class ScoreEndpoint : Endpoint {
        public ScoreEndpoint(HttpRequest request, HttpResponse response) : base(request, response) {
        }


        [HttpMethod(HttpMethod.GET)]
        public void GetScoreBoard() {
            if (!TryAuthorize())
                return;

            using var uow = new UnitOfWork();
            var users = uow.UserRepository.Get(null, q => q.OrderBy(user => -(100 + 3*user.Wins - 5*user.Losses)));

            List<Dictionary<string, string>> board = new();

            foreach (var user in users) {
                board.Add(new() {
                    { "name", user.Name == "" ? user.UserName : user.Name },
                    { "k/d", user.Losses == 0 ? "inf" : (user.Wins / (double)user.Losses).ToString()},
                    { "games", user.GamesPlayed.ToString()},
                    { "elo", (100 + 3 * user.Wins - 5 * user.Losses).ToString() }
                });
            }

            _response.Send(HttpStatus.OK, JsonConvert.SerializeObject(board));
        }
    }
}
