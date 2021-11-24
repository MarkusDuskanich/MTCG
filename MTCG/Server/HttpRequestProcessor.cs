using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Server {
    class HttpRequestProcessor {
        private readonly StreamReader _reader;

        public string Method { get; private set; } = null;
        public string Path { get; private set; }
        public string Version { get; private set; }
        public string Content { get; private set; }

        public Dictionary<string, string> Headers { get; } = new();

        public HttpRequestProcessor(TcpClient s) {
            _reader = new(s.GetStream());
        }

        public void Process() {
            while (!_reader.EndOfStream) {
                string line = _reader.ReadLine();

                if (ContentIsNext(line)) {
                    ProcessContent();
                    return;
                }

                if (MethodNotProcessed())
                    ProcessMethod(line);
                else
                    ProcessHeaders(line);
            }
        }

        private void ProcessHeaders(string line) {
            var parts = line.Split(": ");
            Headers.Add(parts[0], parts[1]);
        }

        private void ProcessMethod(string line) {
            var parts = line.Split(' ');
            Method = parts[0];
            Path = parts[1];
            Version = parts[2];
        }

        private bool MethodNotProcessed() {
            return Method == null;
        }

        private static bool ContentIsNext(string line) {
            return line.Length == 0;
        }

        private void ProcessContent() {
            if (Headers.ContainsKey("Content-Length")) {
                int contentlength = int.Parse(Headers["Content-Length"]);
                char[] buffer = new char[contentlength];
                _reader.ReadBlock(buffer, 0, contentlength);
                Content = buffer.ToString();
            }
            //error handling if no content??
        }
    }
}
