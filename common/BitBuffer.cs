using System;

namespace Revival.Common
{
    public class BitBuffer
    {
        private byte[] _buffer;
        private bool _internalBuffer;
        private int _maxBytes;
        private int _startOffset;
        public int BitOffset;

        public int Offset;
        public int BytesUsed => (BitOffset >> 3) + (BitOffset % 8 == 0 ? 0 : 1);
        public int Length => _buffer.Length;

        private int BitByteOffset => BitOffset >> 3;

        public void SetBuffer(byte[] buffer, int offset, int maxBytes)
        {
            _buffer = buffer;
            Offset = _startOffset = offset;
            _maxBytes = maxBytes;
            BitOffset = 0;
            _internalBuffer = false;
        }

        public void CreateBuffer()
        {
            _buffer = new byte[1024];
            Offset = _startOffset = 0;
            BitOffset = 0;
            _internalBuffer = true;
            _maxBytes = 0;
        }

        public void FreeBuffer()
        {
            _buffer = null;
            Offset = _startOffset = 0;
            BitOffset = 0;
            _internalBuffer = false;
            _maxBytes = 0;
        }

        public bool ReadBool()
        {
            return ReadBits(1) != 0;
        }

        // Commented out instead of deleted because it might be needed
        //public char ReadChar()
        //{
        //    return (char)ReadBits(8);
        //}

        public byte ReadByte()
        {
            return (byte) ReadBits(8);
        }

        public short ReadInt16()
        {
            return (short) ReadBits(16);
        }

        public ushort ReadUInt16()
        {
            return (ushort) ReadBits(16);
        }

        public unsafe float ReadFloat()
        {
            var val = ReadBits(32);
            return *(float*) &val;
        }

        public int ReadInt32()
        {
            return ReadBits(32);
        }

        public uint ReadUInt32()
        {
            return (uint) ReadBits(32);
        }

        public int ReadBitsShift(int bitCount)
        {
            var val = ReadBits(bitCount);
            var shift = 1 << (bitCount - 1);
            return val - shift;
        }

        //public Int64 ReadInt64()
        //{
        //    Int64 low = ReadUInt32();
        //    Int64 high = ReadUInt32();

        //    high <<= 32;
        //    high |= low;

        //    return high;
        //}

        public ulong ReadUInt64()
        {
            ulong low = ReadUInt32();
            ulong high = ReadUInt32();

            high <<= 32;
            high |= low;

            return high;
        }

        public long
            ReadNonStandardFunc() // todo: this is how this function works in ASM - this might be reading in a double value, so until we've tested all usages, leaving it as "ReadNonStandardFunc"
        {
            var ret = new byte[8];

            for (var i = 0; i < 8; i++)
            {
                ret[i] = (byte) ReadBits(8);
            }

            return BitConverter.ToInt64(ret, 0);
        }

        public int ReadBits(int bitCount)
        {
            var bitsToRead = bitCount;
            int b = _buffer[Offset + BitByteOffset];

            var offsetBitsInThisByte = BitOffset & 0x07;
            var bitsToUseFromByte = 0x08 - offsetBitsInThisByte;

            var bitOffset = bitCount;
            if (bitsToUseFromByte < bitCount)
            {
                bitOffset = bitsToUseFromByte;
            }

            b >>= offsetBitsInThisByte;
            bitsToRead -= bitOffset;

            // clean any excess bits we don't want
            b &= (0x01 << bitOffset) - 1;

            var bytesStillToRead = bitsToRead + 0x07;
            bytesStillToRead >>= 3;

            var ret = b;
            for (var i = bytesStillToRead; i > 0; i--)
            {
                var bitLevel = (i - 1) * 8;

                b = _buffer[Offset + BitByteOffset + i];
                var bitsRead = 0x08;

                if (i == bytesStillToRead)
                {
                    var cleanBits = bitsToRead - bitLevel;
                    bitsRead = cleanBits;
                    cleanBits = 0x01 << cleanBits;
                    cleanBits--;
                    b &= (byte) cleanBits;
                }

                b <<= bitOffset + bitLevel;
                ret |= b;
                bitsToRead -= bitsRead;
            }

            BitOffset += bitCount;

            return ret;
        }

        public void WriteBool(bool value)
        {
            WriteBits(value ? 1 : 0, 1);
        }

        public void WriteByte(int value)
        {
            WriteBits(value, 8);
        }

        public void WriteByte(byte value)
        {
            WriteBits(value, 8);
        }

        public void WriteInt16(short value)
        {
            WriteBits(value, 16);
        }

        public void WriteUInt16(ushort value)
        {
            WriteBits(value, 16);
        }

        public void WriteInt16(int value)
        {
            WriteBits(value, 16);
        }

        public void WriteUInt16(uint value)
        {
            WriteBits((int) value, 16);
        }

        public unsafe void WriteFloat(float value)
        {
            var intVal = *(int*) &value;
            WriteBits(intVal, 32);
        }

        public void WriteInt32(int value)
        {
            WriteBits(value, 32);
        }

        public void WriteUInt32(uint value)
        {
            WriteBits((int) value, 32);
        }

        public void WriteUInt64(ulong value)
        {
            WriteBits((int) value, 32); // low
            WriteBits((int) (value >> 32), 32); // high
        }

        public void WriteNonStandardFunc(long val)
        {
            var byteArray = BitConverter.GetBytes(val);
            for (var i = 0; i < 8; i++)
            {
                WriteBits(byteArray[i], 8);
            }
        }

        public void WriteBitsShift(int value, int bitCount)
        {
            var shift = 1 << (bitCount - 1);
            value += shift;
            WriteBits(value, bitCount);
        }

        public void WriteBits(int value, int bitCount)
        {
            WriteBits(value, bitCount, BitOffset, true);
        }

        public void WriteBits(int value, int bitCount, int bitOffset, bool incrementBitOffset = false)
        {
            var currByteOffset = bitOffset >> 3;
            if (_internalBuffer && currByteOffset > _buffer.Length - 10)
            {
                var newData = new byte[_buffer.Length + 1024];
                Buffer.BlockCopy(_buffer, 0, newData, 0, _buffer.Length);
                _buffer = newData;
            }

            var bitsToWrite = bitCount;
            var offsetBitsInFirstByte = bitOffset & 0x07;
            var bitByteOffset = 0x08 - offsetBitsInFirstByte;

            var bitsInFirstByte = bitCount;
            if (bitByteOffset < bitCount)
            {
                bitsInFirstByte = bitByteOffset;
            }

            var bytesToWriteTo = (bitsToWrite + 0x07 + offsetBitsInFirstByte) >> 3;
            if (_maxBytes > 0 && incrementBitOffset &&
                Offset + currByteOffset + bytesToWriteTo > _startOffset + _maxBytes)
            {
                throw new IndexOutOfRangeException("Written byte count has exceeded the maximum byte count of " +
                                                   _maxBytes);
            }

            for (var i = 0; i < bytesToWriteTo; i++, currByteOffset++)
            {
                var bitLevel = 0;
                if (offsetBitsInFirstByte > 0 && i > 0)
                {
                    bitLevel = 8 - offsetBitsInFirstByte;
                }

                if (offsetBitsInFirstByte > 0 && i >= 2)
                {
                    bitLevel += (i - 1) * 8;
                }
                else if (offsetBitsInFirstByte == 0 && i >= 1)
                {
                    bitLevel += i * 8;
                }

                var toWrite = value >> bitLevel;
                if (i == 0)
                {
                    toWrite &= (1 << bitsInFirstByte) - 1;
                    toWrite <<= offsetBitsInFirstByte;
                    bitsToWrite -= bitsInFirstByte;
                }
                else if (i == bytesToWriteTo - 1 && offsetBitsInFirstByte > 0)
                {
                    toWrite &= (1 << bitsToWrite) - 1;
                }
                else
                {
                    bitsToWrite -= 8;
                }

                _buffer[Offset + currByteOffset] |= (byte) toWrite;
            }

            if (incrementBitOffset)
            {
                BitOffset += bitCount;
            }
        }

        public byte[] GetBuffer()
        {
            if (!_internalBuffer)
            {
                return _buffer;
            }

            Array.Resize(ref _buffer, BytesUsed);
            return _buffer;
        }
    }
}