﻿//
// Copyright (c) 2008-2011, Kenneth Bell
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
//

using System;
using System.IO;
using DiscUtils.Core.HfsWrapper;
using DiscUtils.Partitions;
using DiscUtils.Streams;

namespace DiscUtils.ApplePartitionMap
{
    internal sealed class PartitionMapEntry : PartitionInfo, IByteArraySerializable
    {
        private readonly Stream _diskStream;
        public uint BootBlock;
        public uint BootBytes;
        public uint Flags;
        public uint LogicalBlocks;
        public uint LogicalBlockStart;
        public uint MapEntries;
        public string Name;
        public uint PhysicalBlocks;
        public uint PhysicalBlockStart;
        public ushort Signature;
        public string Type;
        public ushort BlockSize;

        public PartitionMapEntry(Stream diskStream, ushort blockSize)
        {
            _diskStream = diskStream;
            BlockSize = blockSize;
        }

        public override byte BiosType
        {
            get { return 0xAF; }
        }

        public override long FirstSector
        {
            get { return PhysicalBlockStart; }
        }

        public override Guid GuidType
        {
            get { return Guid.Empty; }
        }

        public override long LastSector
        {
            get { return PhysicalBlockStart + PhysicalBlocks - 1; }
        }

        public override string TypeAsString
        {
            get { return Type; }
        }

        public override PhysicalVolumeType VolumeType
        {
            get { return PhysicalVolumeType.ApplePartition; }
        }

        public int Size
        {
            get { return BlockSize; }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
        }

        public int ReadFrom(byte[] buffer, int offset)
        {
            Signature = EndianUtilities.ToUInt16BigEndian(buffer, offset + 0);
            MapEntries = EndianUtilities.ToUInt32BigEndian(buffer, offset + 4);
            PhysicalBlockStart = EndianUtilities.ToUInt32BigEndian(buffer, offset + 8);
            PhysicalBlocks = EndianUtilities.ToUInt32BigEndian(buffer, offset + 12);
            Name = EndianUtilities.BytesToString(buffer, offset + 16, 32).TrimEnd('\0');
            Type = EndianUtilities.BytesToString(buffer, offset + 48, 32).TrimEnd('\0');
            LogicalBlockStart = EndianUtilities.ToUInt32BigEndian(buffer, offset + 80);
            LogicalBlocks = EndianUtilities.ToUInt32BigEndian(buffer, offset + 84);
            Flags = EndianUtilities.ToUInt32BigEndian(buffer, offset + 88);
            BootBlock = EndianUtilities.ToUInt32BigEndian(buffer, offset + 92);
            BootBytes = EndianUtilities.ToUInt32BigEndian(buffer, offset + 96);

            return BlockSize;
        }

        public void WriteTo(byte[] buffer, int offset)
        {
            throw new NotImplementedException();
        }

        public override SparseStream Open()
        {
            long startPosition = PhysicalBlockStart * BlockSize;
            long partLength = LogicalBlocks;
            if (LogicalBlocks == 0)
            {
                partLength = ((BootBlock > LogicalBlockStart) ? BootBlock : PhysicalBlocks) - LogicalBlockStart;
            }
            partLength = partLength * BlockSize;

            _diskStream.Position = startPosition + 1024;
            byte[] headerBuf = StreamUtilities.ReadExact(_diskStream, 512);
            VolumeHeader hdr = new VolumeHeader();
            hdr.ReadFrom(headerBuf, 0);
            if (hdr.IsValid)
            {
                ExtDescriptor ext = hdr.DrEmbedExtent;
                startPosition = startPosition + ((hdr.DrAlBlSt * BlockSize) + (ext.FirstAllocationBlock * (long)hdr.DrAlBlkSiz));
                partLength = ext.NumberOfAllocationBlocks * (long)hdr.DrAlBlkSiz;
            }
            return new SubStream(_diskStream, startPosition, partLength);
        }
    }
}