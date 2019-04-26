using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Seed.CodingAssessment
{
    static class ReplayServer
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("replay-server file.dat port byte_order");
                Console.WriteLine();
                Console.WriteLine("\tfile.dat - path to file containing market data to replay");
                Console.WriteLine("\tport - port to listen for connections on (ex: 65511)");
                Console.WriteLine("\tbyte_order - whether data is encoded using 'big' or 'little' endian order");
                Console.WriteLine("\t             valid values: 'big' or 'little'");
                Console.WriteLine();
                Console.WriteLine("Example:   65511 big");
                Console.WriteLine();
                Console.WriteLine();
                Environment.Exit(1);
            }

            var datFile = new FileInfo(args[0]);
            if (!datFile.Exists)
            {
                throw new FileNotFoundException($"File not found: {datFile.FullName}");
            }
            
            
            var port = int.Parse(args[1]);
            var isDataBigEndian = !args[2].ToLowerInvariant().StartsWith('l');

            var tcpListener = new TcpListener(new IPEndPoint(IPAddress.Any, port));
            tcpListener.Start();

            while (true)
            {
                Console.WriteLine($"Accepting replay clients on: {tcpListener.LocalEndpoint}");

                TcpClient client = null;
                try
                {
                    client = tcpListener.AcceptTcpClient();
                    Console.WriteLine($"Accepted connection: {client.Client.RemoteEndPoint}");
                    
                    using (var stream = client.GetStream())
                    using (var writer = GetWriter(stream, isDataBigEndian))
                    {
                        foreach (var line in File.ReadLines(datFile.FullName))
                        {
                            var writable = TextParser.FromLine(line);
                            writable.WriteTo(writer);
                            Console.WriteLine(writable);
                        }
                    }
                    
                    client.Close();
                    Console.WriteLine("Finished replay");
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                }
                finally
                {
                    client?.Dispose();
                }

                Console.WriteLine("client disconnected");
            }
        }
        
        static BinaryWriter GetWriter(Stream stream, bool bigEndian)
        {
            if (bigEndian)
            {
                return new Be.IO.BeBinaryWriter(stream, Encoding.ASCII, true);
            }

            return new BinaryWriter(stream, Encoding.ASCII, true);
        }
    }
}