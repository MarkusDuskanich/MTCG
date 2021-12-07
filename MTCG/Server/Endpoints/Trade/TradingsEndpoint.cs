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
    [HttpEndpoint("/tradings")]
    public class TradingsEndpoint : Endpoint {
        public TradingsEndpoint(HttpRequest request, HttpResponse response) : base(request, response) { }

        [HttpMethod(HttpMethod.GET)]
        public void CheckTradingDeals() {
            if (!TryAuthorize())
                return;

            using var uow = new UnitOfWork();

            var trades = uow.TradeOfferRepository.Get(trade => trade.UserId != GetIdFromToken(), null);

            _response.Send(HttpStatus.OK, JsonConvert.SerializeObject(trades));
        }

        [HttpMethod(HttpMethod.POST)]
        public void CreateTrading() {
            if (!TryAuthorize())
                return;

            using var uow = new UnitOfWork();

            var newOffer = JsonConvert.DeserializeObject<Dictionary<string, string>>(_request.Content);

            TradeOffer tradeOffer = new() {
                Id = Guid.Parse(newOffer["Id"]),
                CardId = Guid.Parse(newOffer["CardToTrade"]),
                UserId = GetIdFromToken(),
                MustBeSpell = newOffer["Type"] != "monster",
                MinDamage = int.Parse(newOffer["MinimumDamage"])
            };

            var res = uow.CardRepository.GetById(Guid.Parse(newOffer["CardToTrade"]));
            res.IsTradeOffer = true;
            uow.CardRepository.Update(res);

            uow.TradeOfferRepository.Insert(tradeOffer);
            uow.Save();
            _response.Send(HttpStatus.OK);
        }
    }
}
