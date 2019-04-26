using System.Collections.Generic;
using System.IO;

namespace Seed.CodingAssessment
{
    interface IWireSerializable
    {
        void WriteTo(BinaryWriter writer);
    }
}