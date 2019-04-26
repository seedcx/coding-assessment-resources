using System.IO;
using System.Text;

namespace Seed.CodingAssessment
{
    class Quote : Base, IWireSerializable
    {   
        public int BidPrice { get; set; }
        public uint BidSize { get; set; }
        public int AskPrice { get; set; }
        public uint AskSize { get; set; }

        public void WriteTo(BinaryWriter writer)
        {
            const byte WireSize = 8 + 8 + 4 + 4 + 4 + 4;
            
            writer.Write(WireSize);
            writer.Write((byte)1);
            
            writer.Write(TimeStamp);
            writer.Write(GetSymbolBytes());
            writer.Write(BidPrice);
            writer.Write(BidSize);
            writer.Write(AskPrice);
            writer.Write(AskSize);
        }
        
        public override string ToString()
        {
            return $"{TimeStamp}|Q|{Symbol}|{BidPrice}|{AskSize}|{BidPrice}|{BidSize}";
        }
    }
}