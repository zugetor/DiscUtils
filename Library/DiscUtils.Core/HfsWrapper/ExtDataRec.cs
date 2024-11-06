using DiscUtils.Streams;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscUtils.Core.HfsWrapper
{
    internal sealed class ExtDataRec : IByteArraySerializable
    {
        public ExtDescriptor[] ExtDataRecs { get; } = new ExtDescriptor[3];

        public int Size => 12;

        public int ReadFrom(byte[] buffer, int offset)
        {
            for (int i = 0; i < 3; ++i)
            {
                ExtDataRecs[i] = EndianUtilities.ToStruct<ExtDescriptor>(buffer, offset + i * 4);
            }
            return Size;
        }

        public void WriteTo(byte[] buffer, int offset)
        {
            throw new NotImplementedException();
        }
    }
}
