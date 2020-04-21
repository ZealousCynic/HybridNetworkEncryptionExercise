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
            finally
            {
                stream.Close();
            }
        }

        public void SendMessage(byte[] message)
        {
            try
            {
                client = new TcpClient(SERVERIP, PORTNR);

                stream = client.GetStream();

                stream.Write(message, 0, message.Length);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                stream.Close();
            }
        }

        public string SendMessageGetResponse(byte[] message)
        {
            string responseData = null;

            try
            {
                client = new TcpClient(SERVERIP, PORTNR);

                stream = client.GetStream();

                stream.Write(message, 0, message.Length);

                byte[] data = new byte[256];

                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                stream.Close();
            }
            return responseData;
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
