using System;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace App
{
    class App
    {
        private static string IP = "0.0.0.0";
        private static int PORT = 8080;

        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(IPAddress.Parse(IP), PORT);
            server.Start();

            Console.WriteLine("Server started on http://{0}:{1}", IP, PORT);

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();

                string html = "Hello from C#";

                string[] headers = {
                    "HTTP/1.1 200 OK",
                    "Content-Type: text/html",
                    "Content-Length: " + html.Length,
                    "",
                    html
                };

                string response = string.Join("\r\n", headers);
                byte[] data = Encoding.ASCII.GetBytes(response.ToCharArray(), 0, response.Length);

                client.GetStream().Write(data);
            }
        }
    }
}