using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Server {
    class HttpResponse {
        private readonly StreamWriter _writer;

        public HttpResponse(TcpClient client) {
            _writer = new(client.GetStream());
        }

        public void Send() {

        }
    }
}
