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


    [HttpEndpoint("/packages")]
    public class PackagesEndpoint : Endpoint {

        public PackagesEndpoint(HttpRequest request, HttpResponse response) : base(request, response) { }

        [HttpMethod(HttpMethod.POST)]
        public void CreatePackage() {
            if (!TryAuthorize())
                return;

            if (_request.Headers["Authorization"] != "Basic admin-mtcgToken") {
                _response.Send(HttpStatus.Unauthorized);
                return;
            }

            _request.Content = _request.Content.Replace(".0", "");

            var newPackage = JsonConvert.DeserializeObject<Package[]>(_request.Content);

            using var uow = new UnitOfWork();
            var packages = uow.PackageRepository.Get(null, q => q.OrderBy(package => package.PackageNum));
            var nextPackNum = packages.Count > 0 ? packages.Last().PackageNum + 1 : 0; 
            foreach (var item in newPackage) {
                item.PackageNum = nextPackNum;
                uow.PackageRepository.Insert(item);
            }
            uow.Save();
            _response.Send(HttpStatus.OK);
        }

    }

}
