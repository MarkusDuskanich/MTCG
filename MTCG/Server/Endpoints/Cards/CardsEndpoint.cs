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

    [HttpEndpoint("/cards")]
    public class CardsEndpoint : Endpoint {

        public CardsEndpoint(HttpRequest request, HttpResponse response) : base(request, response) { }

        [HttpMethod(HttpMethod.GET)]
        public void AcquirePackage() {
            if (!TryAuthorize())
                return;

            using var uow = new UnitOfWork();

            var cards = uow.CardRepository.Get(card => GetIdFromToken() == card.UserId, null);
            var res = JsonConvert.SerializeObject(cards);
            _response.Send(HttpStatus.OK, res);
        }

    }

}

