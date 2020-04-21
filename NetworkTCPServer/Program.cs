using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BaseDirectory;

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
