using MTCG.Client;
using MTCG.Http.Method;
using MTCG.Http.Status;
using MTCG.Server;
using NUnit.Framework;

namespace MTCG.Test {
    [TestFixture]
    public class TestEndpoints {   

        [Test]
        public void TestUsersEndpointPost() {
            var response = TestSetup.Client.MakeRequest(HttpMethod.POST, "/users");
            Assert.AreEqual(HttpStatus.OK, response.Status);
        }
    }
}