using System;
using System.Net.Sockets;

namespace NetworkTCPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                NetworkTCPServer server = new NetworkTCPServer();

                server.Start();

            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }
    }
}
