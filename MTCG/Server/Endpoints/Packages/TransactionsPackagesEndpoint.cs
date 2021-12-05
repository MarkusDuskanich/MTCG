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

namespace MTCG.Endpoints.Packages {

    [HttpEndpoint("/transactions/packages")]
    public class TransactionsPackagesEndpoint : Endpoint {

        public TransactionsPackagesEndpoint(HttpRequest request, HttpResponse response) : base(request, response) { }

        [HttpMethod(HttpMethod.POST)]
        public void AcquirePackage() {
            if (!TryAuthorize())
                return;

            using var uow = new UnitOfWork();

            //check coins of user if not enough => forbidden
            var user = uow.UserRepository.GetById(GetIdFromToken());
            if(user.Coins < 5) {
                _response.Send(HttpStatus.Forbidden);
                return;
            }

            //get all packages if count < 5 return => bad request
            var allPackages = uow.PackageRepository.Get(null, q => q.OrderBy(package => package.PackageNum));
            
            if(allPackages.Count < 5) {
                _response.Send(HttpStatus.BadRequest);
                return;
            }

            //make card objects out of package object, put them in db
            var nextPackage = allPackages.Take(5).ToList();
            var cards = new List<Card>();
            foreach (var item in nextPackage) {
                cards.Add(new() {
                    Id = item.Id,
                    UserId = user.Id,
                    Name = item.Name,
                    Damage = item.Damage
                });
            }

            //update user coins
            user.Coins -= 5;
            uow.UserRepository.Update(user);

            //insert the 5 cards into db
            cards.ForEach(card => uow.CardRepository.Insert(card));

            //remove packages
            nextPackage.ForEach(item => uow.PackageRepository.Delete(item));

            uow.Save();
            _response.Send(HttpStatus.OK);
        }

    }

}

