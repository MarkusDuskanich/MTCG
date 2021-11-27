using MTCG.Http.Method;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace MTCG.Http.Protocol {
    public class HttpRequest {
        private readonly TcpClient _client;

        public string Version { get; set; } = "HTTP/1.1";
        public HttpMethod Method { get; set; }
        public string Path { get; set; } = null;
        public string Content { get; set; } = "";
        public Dictionary<string, string> QueryParameters { get; private set; } = new();
        public Dictionary<string, string> Headers { get; private set; } = new();
        public List<string> PathParameters { get; set; }

        public HttpRequest(TcpClient client) {
            _client = client;
        }

        public void Send(HttpMethod method, string path, string content = "", params string[] headers) {
            Method = method;
            Path = path;
            Content = content;
            foreach (var header in headers) {
                var keyValuePair = header.Split(": ");
                Headers.Add(keyValuePair[0], keyValuePair[1]);
            }
            Send();
        }

        public void Send() {
            StreamWriter writer = new(_client.GetStream());
            writer.WriteLine($"{Enum.GetName(Method)} {Path} {Version}");
            writer.WriteLine("Host: localhost:10001");
            writer.WriteLine("User-Agent: UnitTestClient");
            writer.WriteLine("Accept: */*");
            if (Content.Length > 0)
                writer.WriteLine("Content-Type: application/json; charset=utf-8");
            writer.WriteLine($"Content-Length: {Content.Length}");
            foreach (var item in Headers) {
                writer.WriteLine($"{item.Key}: {item.Value}");
            }
            if (Content.Length > 0) {
                writer.WriteLine();
                writer.Write(Content);
            }
            writer.Flush();
        }

        public HttpRequest Receive() {
            StreamReader reader = new(_client.GetStream());
            while (reader.Peek() > -1) {
                var line = reader.ReadLine();

                if (ContentIsNext(line)) {
                    ParseContent(reader);
                    return this;
                }

                if (FirstLineNotProcessed()) {
                    ParseMethodPathVersion(line);
                    ParseQueryParameters();
                }else
                    ParseHeaders(line);
            }
            return this;
        }

        private void ParseQueryParameters() {
            if (!Path.Contains('?'))
                return;
            var res = Path.Split('?');
            Path = res[0];
            res = res[1].Split('&');
            foreach (var item in res) {
                var keyValuePair = item.Split('=');
                QueryParameters.Add(keyValuePair[0], keyValuePair[1]);
            }
        }

        private void ParseHeaders(string line) {
            var parts = line.Split(": ");
            Headers.Add(parts[0], parts[1]);
        }

        private void ParseMethodPathVersion(string line) {
            var parts = line.Split(' ');
            Method = Enum.Parse<HttpMethod>(parts[0].ToUpper());
            Path = parts[1];
            Version = parts[2];
        }

        private bool FirstLineNotProcessed() {
            return Path == null;
        }

        private static bool ContentIsNext(string line) {
            return line.Length == 0;
        }

        private void ParseContent(StreamReader reader) {
            if (!Headers.ContainsKey("Content-Length"))
                throw new KeyNotFoundException("Headers do not contain Content-Length");

            var buffer = new char[int.Parse(Headers["Content-Length"])];
            if (reader.ReadBlock(buffer, 0, buffer.Length) != buffer.Length)
                throw new Exception("Could not read full content");

            Content = new string(buffer);
        }
    }
}
