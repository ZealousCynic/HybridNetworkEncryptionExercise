using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkTCPServer
{
    class NetworkTCPServer
    {
        const Int32 PORTNR = 5000;
        const string SERVERIP = "127.0.0.1";

        byte[] bytes;
        string data;

        TcpListener server;

        public NetworkTCPServer()
        {
            IPAddress address = IPAddress.Parse(SERVERIP);

            server = new TcpListener(address, PORTNR);
        }

        public void Start()
        {
            try
            {
                Console.WriteLine("TCP server starting on port: {0}", PORTNR);

                server.Start();

                while (true)
                {

                    bytes = new byte[256];
                    data = null;
                    int i;

                    TcpClient client = server.AcceptTcpClient();

                    Console.WriteLine("Client connected: " + client.Client.RemoteEndPoint);

                    NetworkStream stream = client.GetStream();

                    while((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = Encoding.ASCII.GetString(bytes, 0, i);

                        DebugResponse(stream, data);
                    }

                    client.Close();
                }
            }
            catch (SocketException e)
            {
                throw e;
            }
            finally
            {
                server.Stop();
            }
        }

        void DebugResponse(NetworkStream stream, string message)
        {
            message.ToUpper();

            byte[] msgBytes = Encoding.ASCII.GetBytes(message);

            stream.Write(msgBytes, 0, msgBytes.Length);
        }
    }
}
