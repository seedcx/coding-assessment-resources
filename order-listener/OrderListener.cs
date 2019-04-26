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
            if (args.Length != 2)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("order-listener port byte_order");
                Console.WriteLine();
                Console.WriteLine("\tport - port to listen for connections on (ex: 65511)");
                Console.WriteLine("\tbyte_order - whether data is encoded using 'big' or 'little' endian order");
                Console.WriteLine("\t             valid values: 'big' or 'little'");
                Console.WriteLine();
                Console.WriteLine("Example:   65512 big");
                Console.WriteLine();
                Console.WriteLine();
                Environment.Exit(1);
            }

            var port = int.Parse(args[0]);
            var isDataBigEndian = !args[1].ToLowerInvariant().StartsWith('l');

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
                    using (var reader = GetReader(stream, isDataBigEndian))
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

        static BinaryReader GetReader(Stream stream, bool bigEndian)
        {
            if (bigEndian)
            {
                return new Be.IO.BeBinaryReader(stream, Encoding.ASCII, true);
            }

            return new BinaryReader(stream, Encoding.ASCII, true);
        }
    }
}