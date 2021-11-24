using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Server {
    class ConnectionHandler {
        private readonly TcpClient _socket;

        public ConnectionHandler(TcpClient socket) {
            _socket = socket;
            HandleConnection();
        }

        private void HandleConnection() {
            Task.Run(() => {
                HttpRequestProcessor request = new(_socket);
                //call method of proper class
                //hand method the request and response processor
            });
        }
    }
}
