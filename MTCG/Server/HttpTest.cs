using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Server.HttpAttributes;

namespace MTCG.Server {
    [HttpEndpoint("/users")]
    class HttpTest {

        [HttpMethod("post")]
        public void PostMethod() {
            Console.WriteLine("Post method called");
        }
    }
}
