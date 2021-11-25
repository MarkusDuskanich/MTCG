using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Server {

    class HttpRequest {
        private readonly StreamReader _reader;

        public string Method { get; private set; } = null;
        public string Path { get; private set; }
        public string Version { get; private set; }
        public string Content { get; private set; }

        public Dictionary<string, string> Headers { get; } = new();

        public HttpRequest(TcpClient client) {
            _reader = new(client.GetStream());
            //Error handling is missing if some of the values are not set after parse
            Parse();
        }

        private void Parse() {
            while (!_reader.EndOfStream) {
                var line = _reader.ReadLine();

                if (ContentIsNext(line)) {
                    ParseContent();
                    return;
                }

                if (FirstLineNotProcessed())
                    ParseMethodPathVersion(line);
                else
                    ParseHeaders(line);
            }
        }

        private void ParseHeaders(string line) {
            var parts = line.Split(": ");
            Headers.Add(parts[0], parts[1]);
        }

        private void ParseMethodPathVersion(string line) {
            var parts = line.Split(' ');
            Method = parts[0];
            Path = parts[1];
            Version = parts[2];
        }

        private bool FirstLineNotProcessed() {
            return Method == null;
        }

        private static bool ContentIsNext(string line) {
            return line.Length == 0;
        }

        private void ParseContent() {
            if (!Headers.ContainsKey("Content-Length"))
                throw new KeyNotFoundException("Headers do not contain Content-Length");

            var buffer = new char[int.Parse(Headers["Content-Length"])];
            if (_reader.ReadBlock(buffer, 0, buffer.Length) != buffer.Length)
                throw new Exception("Could not read full content");
            Content = buffer.ToString();
        }
    }
}
