using Encryption.RSA;
using Encryption.Symmetric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkTCPClient
{
    class Program
    {
        static AES aes = new AES();
        static RSAXML rsaxml = new RSAXML();

        static void Main(string[] args)
        {
            NetworkTCPClient client = new NetworkTCPClient();

            SendKey(client);
            SendIV(client);

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


                Console.WriteLine("Press enter to send the message...");
                Console.ReadKey();

                Console.WriteLine("Response received: " + SendMessage(client, message));

                client.Disconnect();
            }
        }

        static void SendKey(NetworkTCPClient client)
        {
            Console.WriteLine("MODULUS: \n" + Convert.ToBase64String(rsaxml.Modulus) + "\n");

            Console.WriteLine("AES initialized with default key: " + Convert.ToBase64String(aes.Alg.Key) + "\nEncrypting key....");

            byte[] encrypted = rsaxml.Encrypt(aes.Alg.Key);
            byte[] decrypted = rsaxml.Decrypt(encrypted);

            Console.WriteLine(decrypted);

            Console.WriteLine("Sending encrypted key to server: " + Convert.ToBase64String(encrypted));
            client.SendMessage(new byte[] { 42 });
            client.SendMessage(encrypted);
        }
        static void SendIV(NetworkTCPClient client)
        {
            Console.WriteLine("AES initialized with default IV: " + Convert.ToBase64String(aes.Alg.Key) + "\nEncrypting IV....");

            byte[] encrypted = rsaxml.Encrypt(aes.Alg.IV);
            byte[] decrypted = rsaxml.Decrypt(encrypted);


            Console.WriteLine("Sending encrypted IV to server: " + Convert.ToBase64String(encrypted));
            client.SendMessage(new byte[] { 12 });
            client.SendMessage(encrypted);
        }

        static string SendMessage(NetworkTCPClient client, string message)
        {
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            byte[] encrypted = aes.Encrypt(messageBytes);

            return client.SendMessageGetResponse(encrypted);
        }
    }
}
