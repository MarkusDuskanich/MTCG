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

namespace MTCG.Endpoints.Cards {

    [HttpEndpoint("/deck")]
    public class DeckEndpoint : Endpoint {

        public DeckEndpoint(HttpRequest request, HttpResponse response) : base(request, response) { }

        [HttpMethod(HttpMethod.GET)]
        public void GetDeck() {
            if (!TryAuthorize())
                return;

            using var uow = new UnitOfWork();

            var deck = uow.CardRepository.Get(card => GetIdFromToken() == card.UserId && card.InDeck, null);

            //format option
            string res = "";
            if(_request.QueryParameters.ContainsKey("format") && _request.QueryParameters["format"] == "plain")
                res = JsonConvert.SerializeObject(from card in deck select card.Id);
            else
                res = JsonConvert.SerializeObject(deck);
            _response.Send(HttpStatus.OK, res);
        }


        [HttpMethod(HttpMethod.PUT)]
        public void ConfigureDeck() {
            if (!TryAuthorize())
                return;

            using var uow = new UnitOfWork();

            var userCards = uow.CardRepository.Get(card => GetIdFromToken() == card.UserId);

            var requestedDeck = JsonConvert.DeserializeObject<List<string>>(_request.Content);
            
            if(requestedDeck.Count != 4) {
                _response.Send(HttpStatus.BadRequest);
                return;
            }

            //remove old deck
            userCards.ForEach(card => card.InDeck = false);

            //set new deck
            foreach (var id in requestedDeck) {
                if(!userCards.Any(card => card.Id.ToString() == id && card.IsTradeOffer == false)) {
                    _response.Send(HttpStatus.BadRequest);
                    return;
                }
                userCards.Find(card => card.Id.ToString() == id).InDeck = true;
            }

            //update db
            userCards.ForEach(card => uow.CardRepository.Update(card));
            uow.Save();
            _response.Send(HttpStatus.OK);
        }

    }

}

