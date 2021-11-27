using MTCG.Http.Method;
using MTCG.Http.Protocol;
using MTCG.Server;
using System.Net.Sockets;
using System.Threading;

namespace MTCG.Client {
    public class HttpClient {
        private readonly int _port = 10001;

        public HttpClient(int port = 10001) {
            while (!HttpServer.Instance.IsRunning) {
                Thread.Sleep(1);
            }
            _port = port;
        }

        public HttpResponse MakeRequest(HttpMethod method, string path, string content = "", params string[] headers) {
            TcpClient client = new("localhost", _port);
            new HttpRequest(client).Send(method, path, content, headers);
            var res = new HttpResponse(client).Receive();
            client.GetStream().Close();
            client.Close();
            return res;
        }
    }
}
