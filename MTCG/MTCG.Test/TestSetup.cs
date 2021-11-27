using MTCG.Client;
using MTCG.Http.Method;
using MTCG.Http.Status;
using MTCG.Server;
using NUnit.Framework;

namespace MTCG.Test {
    [SetUpFixture]
    class TestSetup {
        public static HttpClient Client { get; private set; }

        [OneTimeSetUp]
        public void StartServer() {
            HttpServer.Instance.Start();
            Client = new();
        }

        [OneTimeTearDown]
        public void StopServer() {
            HttpServer.Instance.Stop();
        }
    }
}
