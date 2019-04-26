using System;

namespace Seed.CodingAssessment
{
    static class TextParser
    {
        public static IWireSerializable
        FromLine(string line)
        {
            int i = 0;
            string[] parts = line.Split('|');

            var timeStamp = ulong.Parse(parts[i++]);

            if (parts[i] == "Q")
            {
                var q = new Quote
                {
                    TimeStamp = timeStamp,
                    Symbol = GetSymbol(parts[++i]),
                    BidPrice = int.Parse(parts[++i]),
                    BidSize = uint.Parse(parts[++i]),
                    AskPrice = int.Parse(parts[++i]),
                    AskSize = uint.Parse(parts[++i])
                };

                return q;
            }
            else if (parts[i] == "T")
            {
                var t = new Trade
                {
                    TimeStamp = timeStamp,
                    Symbol = GetSymbol(parts[++i]),
                    Price = int.Parse(parts[++i]),
                    Size = uint.Parse(parts[++i])
                };

                return t;
            }

            throw new Exception("Problem parsing line: " + line);
        }

        private static string GetSymbol(string s)
        {
            return s
                .Substring(0, Math.Min(8, s.Length))
                .ToUpperInvariant();
        }
    }
}