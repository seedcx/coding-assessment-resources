using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Seed.CodingAssessment
{
    static class OrderListener
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("order-listener port");
                Console.WriteLine();
                Console.WriteLine("\tport - port to listen for connections on (ex: 65511)");
                Console.WriteLine();
                Console.WriteLine();
                Environment.Exit(1);
            }

            var port = int.Parse(args[0]);

            var tcpListener = new TcpListener(new IPEndPoint(IPAddress.Any, port));
            tcpListener.Start();

            while (true)
            {
                Console.Error.WriteLine($"Accepting order clients on: {tcpListener.LocalEndpoint}");

                TcpClient client = null;
                try
                {
                    client = tcpListener.AcceptTcpClient();
                    Console.Error.WriteLine($"Accepted connection: {client.Client.RemoteEndPoint}");

                    using (var stream = client.GetStream())
                    using (var reader = new BinaryReader(stream, Encoding.ASCII, true))
                    {
                        while (client.Connected)
                        {
                            var timeStamp = reader.ReadUInt64();
                            var symbol = new string(reader.ReadChars(8));
                            var side = reader.ReadChar();
                            var price = reader.ReadInt32();
                            var size = reader.ReadUInt32();

                            Console.WriteLine($"<order timeStamp=\"{timeStamp}\" symbol=\"{symbol}\" side=\"{side}\" price=\"{price}\" size=\"{size}\" />");
                        }
                    }
                }
                catch (EndOfStreamException)
                {
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                }
                finally
                {
                    client?.Dispose();
                }

                Console.Error.WriteLine("client disconnected");
            }
        }
    }
}