using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace MTCG.Server {
    internal class HttpProcessor {
        private readonly TcpClient _socket;
        private readonly MTCGServer _httpServer;

        public string Method { get; private set; } = null;
        public string Path { get; private set; }
        public string Version { get; private set; }

        public Dictionary<string, string> Headers { get; } = new();

        public HttpProcessor(TcpClient s, MTCGServer httpServer) {
            _socket = s;
            _httpServer = httpServer;
        }

        public void Process() {
            var writer = new StreamWriter(_socket.GetStream()) { AutoFlush = true };
            var reader = new StreamReader(_socket.GetStream());
            Console.WriteLine();
     
            Console.WriteLine(reader.ToString());

            // read (and handle) the full HTTP-request
            string line = null;
            bool isContent = false;
            while (!reader.EndOfStream) {
 

                if (isContent) {

                    //handle the content here
                    if (Headers.ContainsKey("Content-Length")) {
                        int contentlength = int.Parse(Headers["Content-Length"]);
                        char[] buffer = new char[contentlength];
                        reader.ReadBlock(buffer, 0, contentlength);

                        Console.WriteLine("content start:");
                        Console.WriteLine(buffer);
                        Console.WriteLine("content end");

                        //while (contentlength-- >= 3) {
                        //    Console.Write((char)reader.Read());
                        //}
                        //break;
                    }
                    break;
                }
                line = reader.ReadLine();
                Console.WriteLine(line);

                if (line.Length == 0) {
                    isContent = true;
                    continue;  // empty line means next comes the content
                }

                // handle first line of HTTP
                if (Method == null) {
                    var parts = line.Split(' ');
                    Method = parts[0];
                    Path = parts[1];
                    Version = parts[2];
                }
                // handle HTTP headers
                else {
                    var parts = line.Split(": ");
                    Headers.Add(parts[0], parts[1]);
                }
            }

            // write the full HTTP-response
            string content = $"<html><body><h1>test server</h1>" +
                $"Current Time: {DateTime.Now}" +
                $"<form method=\"GET\" action=\"/form\">" +
                $"<input type=\"text\" name=\"foo\" value=\"foovalue\">" +
                $"<input type=\"submit\" name=\"bar\" value=\"barvalue\">" +
                $"</form></html>";

            Console.WriteLine();
            WriteLine(writer, "HTTP/1.1 200 OK");
            WriteLine(writer, "Server: My simple HttpServer");
            WriteLine(writer, $"Current Time: {DateTime.Now}");
            WriteLine(writer, $"Content-Length: {content.Length}");
            WriteLine(writer, "Content-Type: text/html; charset=utf-8");
            WriteLine(writer, "");
            WriteLine(writer, content);

            writer.WriteLine();
            writer.Flush();
            writer.Close();
        }

        private void WriteLine(StreamWriter writer, string s) {
            Console.WriteLine(s);
            writer.WriteLine(s);
        }
    }
}