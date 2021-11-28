using MTCG.Client;
using MTCG.Http.Method;
using MTCG.Server;
using System;
using System.Threading;

namespace MTCG {
    class Program {
        static void Main() {
            Console.CancelKeyPress += (sender, e) => Environment.Exit(0);

            HttpServer.Instance.Start();

            new Thread(() => new HttpClient().MakeRequest(HttpMethod.POST, "/users", @"{""user"": ""1""}")).Start();
            //new Thread(() => new HttpClient().MakeRequest(HttpMethod.POST, "/users", @"{""user"": ""2""}")).Start();
            //new Thread(() => new HttpClient().MakeRequest(HttpMethod.POST, "/users", @"{""user"": ""3""}")).Start();
            //new Thread(() => new HttpClient().MakeRequest(HttpMethod.POST, "/users", @"{""user"": ""4""}")).Start();
        }
    }
}
