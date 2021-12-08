using MTCG.Server;
using NUnit.Framework;
 

namespace MTCG.Test.Server {
    [TestFixture]
    public class TestServerStartStop {
        
        [TearDown]
        public void StopServer() {
            HttpServer.Instance.Stop();
        }

        [Test]
        public void CanStartServer() {
            HttpServer.Instance.Start();
            Assert.IsTrue(HttpServer.Instance.IsRunning);
        }

        [Test]
        public void CanStopServerAfterStartServer() {
            HttpServer.Instance.Start();
            HttpServer.Instance.Stop();
            Assert.IsFalse(HttpServer.Instance.IsRunning);
        }

    }
}
