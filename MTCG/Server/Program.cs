using MTCG.Client;
using MTCG.DAL;
using MTCG.Http.Method;
using MTCG.Models;
using MTCG.Server;
using System;
using System.Threading;

namespace MTCG {

    class Program {

        static void Main() {
            Console.CancelKeyPress += (sender, e) => Environment.Exit(0);
            HttpServer.Instance.Start();
        }
    }
}
