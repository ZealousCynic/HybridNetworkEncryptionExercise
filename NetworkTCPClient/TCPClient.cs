using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkTCPClient
{
    class NetworkTCPClient
    {
        Int32 PORTNR = 5000;
        string SERVERIP = "127.0.0.1";
        TcpClient client;
        NetworkStream stream;
        byte[] toSend;
        byte[] received;

        public NetworkTCPClient() { }

        public NetworkTCPClient(int portNr, string serverIP) { PORTNR = portNr; SERVERIP = serverIP; }

        public void SendMessage(string message)
        {
            try
            {
                client = new TcpClient(SERVERIP, PORTNR);

                toSend = Encoding.ASCII.GetBytes(message);

                stream = client.GetStream();

                stream.Write(toSend, 0, toSend.Length);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }

        public string GetResponse()
        {
            string response = null;

            try
            {
                if (client is null || client.Connected != true || stream is null || stream.DataAvailable != true)
                    return "Client is not connected!";

                received = new byte[256];

                Int32 receivedBytes = stream.Read(received, 0, received.Length);
                response = Encoding.ASCII.GetString(received);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            return response;
        }

        public void Disconnect()
        {
            if (client != null || client.Connected == true)
            {
                if (!(stream is null))
                    stream.Close();
                client.Close();
            }
        }
    }
}
