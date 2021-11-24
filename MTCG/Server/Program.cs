using System;

namespace MTCG.Server {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("MTCG Server is running...");
            Console.CancelKeyPress += (sender, e) => Environment.Exit(0);

            new MTCGServer(10001).Run();
        }
    }
}
