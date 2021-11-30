using MTCG.Http.Method;
using MTCG.Http.Status;
using NUnit.Framework;

namespace MTCG.Test {
    [TestFixture]
    public class TestEndpoints {   

        [Test]
        public void TestEndpointPostReturnsOk() {
            var response = TestSetup.Client.MakeRequest(HttpMethod.GET, "/test");
            Assert.AreEqual(HttpStatus.OK, response.Status);
        }

        [Test]
        public void NonExistingEndpointReturns404() {
            var response = TestSetup.Client.MakeRequest(HttpMethod.GET, "/nonexistingendpoint");
            Assert.AreEqual(HttpStatus.NotFound, response.Status);
        }
    }
}