namespace SimpleWebServer
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    public class StartUp
    {
        public static void Main()
        {
            var tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8080);
            tcpListener.Start();

            Console.WriteLine("Server listening...");
            Console.WriteLine($"Listening to TCP clients at 127.0.0.1:8080");

            var task = Task.Run(() => ConnectWithTcpClient(tcpListener));
            task.Wait();
        }

        private static async Task ConnectWithTcpClient(TcpListener tcpListener)
        {
            while (true)
            {
                Console.WriteLine("Waiting for client...");
                var client = await tcpListener.AcceptTcpClientAsync();

                Console.WriteLine("Client connected.");

                var buffer = new byte[1024];
                client.GetStream().Read(buffer, 0, buffer.Length);

                var message = Encoding.ASCII.GetString(buffer);
                Console.WriteLine(message);

                var data = Encoding.ASCII.GetBytes("Hello from server!");
                client.GetStream().Write(data, 0, data.Length);

                Console.WriteLine("Closing connection!");
                client.GetStream().Dispose();
            }
        }
    }
}