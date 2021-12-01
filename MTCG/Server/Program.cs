using MTCG.Client;
using MTCG.DAL;
using MTCG.DAL.ORM;
using MTCG.Http.Method;
using MTCG.Models;
using MTCG.Server;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;

namespace MTCG {

    class Program {

        static void Main() {
            Console.CancelKeyPress += (sender, e) => Environment.Exit(0);
            HttpServer.Instance.Start();

        }
    }
}
