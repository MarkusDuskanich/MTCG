using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using MTCG.Http.Status;
using MTCG.Server;

namespace MTCG.Http.Protocol {
    public class HttpResponse {
        private readonly TcpClient _client;
        public string Version { get; set; } = "HTTP/1.1";
        public HttpStatus? Status { get; set; } = null;
        public string Content { get; set; } = "";
        public Dictionary<string, string> Headers { get; private set; } = new();

        public HttpResponse(TcpClient client) {
            _client = client;
        }


        public void Send(HttpStatus status, string content = "", params string[] headers) {
            Status = status;
            Content = content;
            foreach (var header in headers) {
                var keyValuePair = header.Split(": ");
                Headers.Add(keyValuePair[0], keyValuePair[1]);
            }
            Send();
        }

        public void Send() {
            StreamWriter writer = new(_client.GetStream());
            writer.WriteLine($"{Version} {HttpStatusConverter.From((HttpStatus)Status)}");
            writer.WriteLine($"Server: {HttpServer.Name}");
            writer.WriteLine($"Date: {DateTime.Now}");

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


        public HttpResponse Receive() {
            StreamReader reader = new(_client.GetStream());
            while (reader.Peek() > -1) {
                var line = reader.ReadLine();

                if (ContentIsNext(line)) {
                    ParseContent(reader);
                    return this;
                }

                if (FirstLineNotProcessed())
                    ParseVersionAndStatus(line);
                else
                    ParseHeaders(line);
            }
            return this;
        }

        private void ParseHeaders(string line) {
            var parts = line.Split(": ");
            Headers.Add(parts[0], parts[1]);
        }

        private void ParseVersionAndStatus(string line) {
            var parts = line.Split(' ');
            Version = parts[0];
            var statusDescription = parts[1];
            for (int i = 2; i < parts.Length; i++) {
                statusDescription += $" {parts[i]}";
            }
            Status = HttpStatusConverter.From(statusDescription);
        }

        private bool FirstLineNotProcessed() {
            return Status == null;
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
