using DiscUtils.Streams;
using System;

namespace DiscUtils.Core.HfsWrapper
{
    internal sealed class ExtDescriptor : IByteArraySerializable
    {
        public ushort FirstAllocationBlock { get; set; }
        public ushort NumberOfAllocationBlocks { get; set; }

        public int Size => 4;

        public int ReadFrom(byte[] buffer, int offset)
        {
            FirstAllocationBlock = EndianUtilities.ToUInt16BigEndian(buffer, offset);
            NumberOfAllocationBlocks = EndianUtilities.ToUInt16BigEndian(buffer, offset + 2);
            return Size;
        }

        public void WriteTo(byte[] buffer, int offset)
        {
            throw new NotImplementedException();
        }
    }
}
