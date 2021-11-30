using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MTCG.Server {
    public class HttpServer {
        private readonly TcpListener _listener;
        public const string Name = "MTCG Server";

        private volatile bool _isRunning = false;
        public bool IsRunning => _isRunning;

        private Thread t_server;

        public static HttpServer Instance { get; } = new();

        private HttpServer(int port = 10001) {
            _listener = new TcpListener(IPAddress.Loopback, port);
        }

        public void Start() {
            if (IsRunning)
                return;

            t_server = new Thread(Work);
            t_server.Start();
        }


        public void Stop() {
            while (!IsRunning && t_server?.ThreadState == ThreadState.Running) {
                Thread.Sleep(10);
            }

            if (!IsRunning)
                return;

            _isRunning = false;
            _listener.Stop();
            t_server.Join();
        }

        private void Work() {
            _listener.Start(5);
            Console.WriteLine($"{Name} is running...");
            _isRunning = true;
            while (IsRunning) {
                try {
                    new ConnectionHandler(_listener.AcceptTcpClient());
                } catch (SocketException) {
                    Console.WriteLine($"{Name} stopped");
                }
            }
        }
    }
}

