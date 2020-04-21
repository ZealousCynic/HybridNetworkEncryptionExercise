using Encryption.RSA;
using Encryption.Symmetric;
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
        byte[] symmetricKey = null;
        byte[] iv = null;
        SignalByte signal = 0;
        string data;

        TcpListener server;
        RSAXML rsaxml;

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

                    bytes = new byte[262];
                    data = null;

                    TcpClient client = server.AcceptTcpClient();

                    Console.WriteLine("Client connected: " + client.Client.RemoteEndPoint);

                    NetworkStream stream = client.GetStream();

                    switch (signal)
                    {
                        case SignalByte.KEY:
                            ReadKeyFromStream(stream);
                            break;
                        case SignalByte.IV:
                            ReadIVFromStream(stream);
                            break;
                        case SignalByte.MESSAGE:
                            DecryptWithSymmetricKey(stream);
                            break;
                        case SignalByte.UNSET:
                            SignalUnset(stream);
                            break;
                        default:
                            break;
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

        void SignalUnset(NetworkStream stream)
        {
            Console.WriteLine("Reading first byte received as signal byte...");

            byte[] b = new byte[1];
            stream.Read(b, 0, 1);

            int toParse = b[0];

            if (Enum.IsDefined(typeof(SignalByte), toParse))
                signal = (SignalByte)b[0];

            Console.WriteLine("Signal is: " + signal);
        }

        void ReadKeyFromStream(NetworkStream stream)
        {
            if (stream.DataAvailable)
            {
                int i;
                if (rsaxml is null)
                    rsaxml = new RSAXML();

                Console.WriteLine("MODULUS: \n" + Convert.ToBase64String(rsaxml.Modulus) + "\n");

                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    symmetricKey = rsaxml.Decrypt(bytes);
                    Console.WriteLine("Signal is: " + signal);
                    signal = SignalByte.UNSET;
                }

            }
        }

        void ReadIVFromStream(NetworkStream stream)
        {
            if (stream.DataAvailable)
            {
                if (rsaxml is null)
                {
                    Console.WriteLine("RSA initialized in wrong order.");
                    return;
                }

                int i;

                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    iv = rsaxml.Decrypt(bytes);
                    Console.WriteLine("Signal is: " + signal);
                    signal = SignalByte.MESSAGE;
                }

            }
        }

        void DecryptWithSymmetricKey(NetworkStream stream)
        {
            if (symmetricKey is null)
            {
                Console.WriteLine("Key is NULL!");
                return;
            }

            if (stream.DataAvailable)
            {
                byte[] msgBytes = null;
                int i;
                string message = null;

                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    msgBytes = new byte[i];

                    for (int j = 0; j < msgBytes.Length; j++)
                        msgBytes[j] = bytes[j];

                    AES aes = new AES();

                    aes.Alg.Key = symmetricKey;
                    aes.Alg.IV = iv;

                    msgBytes = aes.Decrypt(msgBytes);

                    message = Encoding.ASCII.GetString(msgBytes);

                    DebugResponseASCII(stream, message);
                }

            }

        }

        void DebugResponseASCII(NetworkStream stream, string message)
        {
            Console.WriteLine("Message received: " + message);

            byte[] msgBytes = Encoding.ASCII.GetBytes(message);

            stream.Write(msgBytes, 0, msgBytes.Length);
        }

        void DebugResponse(NetworkStream stream, string message)
        {
            Console.WriteLine("Message received: " + message);

            byte[] msgBytes = Convert.FromBase64String(message);

            stream.Write(msgBytes, 0, msgBytes.Length);
        }
    }
}
