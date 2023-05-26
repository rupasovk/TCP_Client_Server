using System;

namespace TCP_Client_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            int port = 8888;
            string hostname = "192.168.0.14";

            Client tcp_client = new Client(hostname, port);

            Console.WriteLine("Hello World!");
        }
    }
}
