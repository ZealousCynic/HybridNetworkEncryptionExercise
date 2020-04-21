using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkTCPClient
{
    class Program
    {
        static void Main(string[] args)
        {

            NetworkTCPClient client = new NetworkTCPClient();

            while (true)
            {
                Console.WriteLine("Enter message to send. If no message is entered, the client will shut down...");
                string message = Console.ReadLine();

                if (string.IsNullOrEmpty(message))
                {
                    Console.WriteLine("No message entered, shutting down...");
                    Console.ReadKey();
                    Environment.Exit(0);
                }

                client.SendMessage(message);
                Console.WriteLine(client.GetResponse());
                client.Disconnect();
            }
        }
    }
}
