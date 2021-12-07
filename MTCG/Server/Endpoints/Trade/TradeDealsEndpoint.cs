using MTCG.DAL;
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
using System.Threading.Tasks;

namespace MTCG.Endpoints.Trade {
    [HttpEndpoint("/tradings/{id}")]
    public class TradeDealsEndpoint : Endpoint {
        public TradeDealsEndpoint(HttpRequest request, HttpResponse response) : base(request, response) { }

        [HttpMethod(HttpMethod.DELETE)]
        public void DeleteDeal() {
            using var uow = new UnitOfWork();
            var tradeOffer = uow.TradeOfferRepository.GetById(Guid.Parse(_request.PathParameters[0]));
            var card = uow.CardRepository.GetById(tradeOffer.CardId);
            card.IsTradeOffer = false;
            uow.CardRepository.Update(card);
            uow.TradeOfferRepository.Delete(tradeOffer);
            uow.Save();
            _response.Send(HttpStatus.OK);
        }

        [HttpMethod(HttpMethod.POST)]
        public void Trade() {
            if (!TryAuthorize()) 
                return;
            
            using var uow = new UnitOfWork();
            //prevent user from trading with himself, bad request
            var tradeOffer = uow.TradeOfferRepository.GetById(Guid.Parse(_request.PathParameters[0]));
            if(tradeOffer.UserId == GetIdFromToken()) {
                _response.Send(HttpStatus.Forbidden);
                return;
            }

            //get cards associated with deal
            var tradeOfferCard = uow.CardRepository.GetById(tradeOffer.CardId);
            var paymentCard = uow.CardRepository.GetById(JsonConvert.DeserializeObject<Guid>(_request.Content));

            //check if user owns cards, this card cant be trading deal nor can it be in deck
            if(paymentCard.UserId != GetIdFromToken() || paymentCard.InDeck || paymentCard.IsTradeOffer) {
                _response.Send(HttpStatus.Forbidden);
                return;
            }

            //check if the offered card meets requirement
            if(tradeOffer.MustBeSpell && paymentCard.GetCardType() != CardType.Spell || tradeOffer.MinDamage > paymentCard.Damage) {
                _response.Send(HttpStatus.Forbidden);
                return;
            }
            //swap the cards, set isTradingOffer prop, remove the trading deal
            paymentCard.UserId = tradeOfferCard.UserId;
            tradeOfferCard.UserId = GetIdFromToken();
            tradeOfferCard.IsTradeOffer = false;
            uow.TradeOfferRepository.Delete(tradeOffer);
            uow.CardRepository.Update(paymentCard);
            uow.CardRepository.Update(tradeOfferCard);
            uow.Save();
            _response.Send(HttpStatus.OK);
        }
    }
}
