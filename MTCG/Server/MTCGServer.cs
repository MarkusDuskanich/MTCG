using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MTCG.Server {
    public class MTCGServer {
        protected readonly int _port;
        TcpListener listener;

        public MTCGServer(int port) {
            _port = port;
        }

        public void Run() {
            listener = new TcpListener(IPAddress.Loopback, _port);
            listener.Start(5);
            while (true) {
                new ConnectionHandler(listener.AcceptTcpClient()); 
                Thread.Sleep(1);
            }
        }
    }
}
