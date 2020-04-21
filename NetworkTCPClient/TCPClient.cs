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
        const Int32 PORTNR = 5000;
        const string SERVERIP = "127.0.0.1";
        TcpClient client;
        NetworkStream stream;

        public void Connect(string message)
        {
            try
            {
                string response = null;

                client = new TcpClient(SERVERIP, PORTNR);

                byte[] data = Encoding.ASCII.GetBytes(message);

                stream = client.GetStream();

                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", message);

                data = new byte[256];

                Int32 receivedBytes = stream.Read(data, 0, data.Length);
                response = Encoding.ASCII.GetString(data);

                Console.WriteLine("Received: {0}", response);
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
                client.Close();
            }
        }
    }
}
