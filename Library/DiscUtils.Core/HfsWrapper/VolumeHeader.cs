using DiscUtils.Streams;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscUtils.Core.HfsWrapper
{
    internal sealed class VolumeHeader : IByteArraySerializable
    {
        public const ushort HfsSignature = 0x4244;
        public const ushort HfsWrapSignature = 0x482B;

        public ushort DrSigWord;
        public uint DrCrDate;
        public uint DrLsMod;
        public ushort DrAtrb;
        public ushort DrNmFls;
        public ushort DrVBMSt;
        public ushort DrAllocPtr;
        public ushort DrNmAlBlks;
        public uint DrAlBlkSiz;
        public uint DrClpSiz;
        public ushort DrAlBlSt;
        public uint DrNxtCNID;
        public ushort DrFreeBks;
        public byte DrVNLength;
        public char[] DrVN;
        public uint DrVolBkUp;
        public ushort DrVSeqNum;
        public uint DrWrCnt;
        public uint DrXTClpSiz;
        public uint DrCTClpSiz;
        public ushort DrNmRtDirs;
        public uint DrFilCnt;
        public uint DrDirCnt;
        public uint[] DrFndrInfo;
        public ushort DrEmbedSigWord;
        public ExtDescriptor DrEmbedExtent;
        public uint DrXTFlSize;
        public ExtDataRec DrXTExtRec;
        public uint DrCTFlSize;
        public ExtDataRec DrCTExtRec;

        public int Size => 170;

        public bool IsValid => DrSigWord == HfsSignature && DrEmbedSigWord == HfsWrapSignature;

        public int ReadFrom(byte[] buffer, int offset)
        {
            DrSigWord = EndianUtilities.ToUInt16BigEndian(buffer, offset + 0);
            DrCrDate = EndianUtilities.ToUInt32BigEndian(buffer, offset + 2);
            DrLsMod = EndianUtilities.ToUInt32BigEndian(buffer, offset + 6);
            DrAtrb = EndianUtilities.ToUInt16BigEndian(buffer, offset + 10);
            DrNmFls = EndianUtilities.ToUInt16BigEndian(buffer, offset + 12);
            DrVBMSt = EndianUtilities.ToUInt16BigEndian(buffer, offset + 14);
            DrAllocPtr = EndianUtilities.ToUInt16BigEndian(buffer, offset + 16);
            DrNmAlBlks = EndianUtilities.ToUInt16BigEndian(buffer, offset + 18);
            DrAlBlkSiz = EndianUtilities.ToUInt32BigEndian(buffer, offset + 20);
            DrClpSiz = EndianUtilities.ToUInt32BigEndian(buffer, offset + 24);
            DrAlBlSt = EndianUtilities.ToUInt16BigEndian(buffer, offset + 28);
            DrNxtCNID = EndianUtilities.ToUInt32BigEndian(buffer, offset + 30);
            DrFreeBks = EndianUtilities.ToUInt16BigEndian(buffer, offset + 34);
            DrVNLength = buffer[offset + 36];
            DrVN = new char[27];
            for (int i = 0; i < 27; i++)
            {
                DrVN[i] = (char)buffer[offset + 37 + i];
            }
            DrVolBkUp = EndianUtilities.ToUInt32BigEndian(buffer, offset + 64);
            DrVSeqNum = EndianUtilities.ToUInt16BigEndian(buffer, offset + 68);
            DrWrCnt = EndianUtilities.ToUInt32BigEndian(buffer, offset + 70);
            DrXTClpSiz = EndianUtilities.ToUInt32BigEndian(buffer, offset + 74);
            DrCTClpSiz = EndianUtilities.ToUInt32BigEndian(buffer, offset + 78);
            DrNmRtDirs = EndianUtilities.ToUInt16BigEndian(buffer, offset + 82);
            DrFilCnt = EndianUtilities.ToUInt32BigEndian(buffer, offset + 84);
            DrDirCnt = EndianUtilities.ToUInt32BigEndian(buffer, offset + 88);
            DrFndrInfo = new uint[8];
            for (int i = 0; i < 8; i++)
            {
                DrFndrInfo[i] = EndianUtilities.ToUInt32BigEndian(buffer, offset + 92 + i * 4);
            }
            DrEmbedSigWord = EndianUtilities.ToUInt16BigEndian(buffer, offset + 124);
            if (!IsValid) return Size;

            DrEmbedExtent = EndianUtilities.ToStruct<ExtDescriptor>(buffer, offset + 126);
            DrXTFlSize = EndianUtilities.ToUInt32BigEndian(buffer, offset + 130);
            DrXTExtRec = EndianUtilities.ToStruct<ExtDataRec>(buffer, offset + 134);
            DrCTFlSize = EndianUtilities.ToUInt32BigEndian(buffer, offset + 150);
            DrCTExtRec = EndianUtilities.ToStruct<ExtDataRec>(buffer, offset + 154);

            return Size;
        }

        public void WriteTo(byte[] buffer, int offset)
        {
            throw new NotImplementedException();
        }
    }
}
