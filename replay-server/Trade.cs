using System;
using System.IO;
using System.Text;

namespace Seed.CodingAssessment
{
    class Base
    {
        public ulong TimeStamp { get; set; }
        public string Symbol { get; set; }

        protected byte[] GetSymbolBytes()
        {
            byte[] bytes = new byte[8];
            byte[] ascii = Encoding.ASCII.GetBytes(Symbol);
            Array.Copy(
                ascii,
                bytes,
                Math.Min(bytes.Length, ascii.Length));
            return bytes;
        }
    }
    
    class Trade : Base, IWireSerializable
    {
        public int Price { get; set; }
        public uint Size { get; set; }

        public void WriteTo(BinaryWriter writer)
        {
            const byte WireSize = 8 + 8 + 4 + 4;
            
            writer.Write(WireSize);
            writer.Write((byte)2);
            
            writer.Write(TimeStamp);
            writer.Write(GetSymbolBytes());
            writer.Write(Price);
            writer.Write(Size);
        }

        public override string ToString()
        {
            return $"{TimeStamp}|T|{Symbol}|{Price}|{Size}";
        }
    }
}