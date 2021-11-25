using System;

namespace MTCG.Server {
    class Program {
        static void Main() {
            Console.CancelKeyPress += (sender, e) => Environment.Exit(0);

            Console.WriteLine("MTCG Server is running...");

            new MTCGServer(10001).Run();
        }

    }
}
