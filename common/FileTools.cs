using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Revival.Common
{
    public static class FileTools
    {
        #region Commented Unused Code

        //private static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1);

        //public static uint UnixTime()
        //{
        //    TimeSpan timeSpan = (DateTime.Now - _unixEpoch);
        //    return (uint)timeSpan.TotalSeconds;
        //}

        //public static uint UnixTimeUtc()
        //{
        //    TimeSpan timeSpan = (DateTime.UtcNow - _unixEpoch);
        //    return (uint)timeSpan.TotalSeconds;
        //}

        //public static int MemCmp(byte[] buffer1, int offset1, byte[] buffer2, int offset2, int count)
        //{
        //    Debug.Assert(offset1 + count <= buffer1.Length);
        //    Debug.Assert(offset2 + count <= buffer2.Length);
        //    for (int i = 0; i < count; i++, offset1++, offset2++)
        //    {
        //        if (buffer1[offset1] == buffer2[offset2]) continue;
        //        if (buffer1[offset1] < buffer2[offset2]) return -1;
        //        return 1;
        //    }

        //    return 0;
        //}

        //public static void MemSet(byte[] buffer, int offset, int count, byte value)
        //{
        //    Debug.Assert(offset + count <= buffer.Length);
        //    for (int i = 0; i < count; i++)
        //    {
        //        buffer[offset++] = value;
        //    }
        //}

        //public static void ZeroMemory(byte[] buffer, int offset, int count)
        //{
        //    MemSet(buffer, offset, count, 0x00);
        //}

        //public static byte[] HexFileToByteArray(string hex)
        //{
        //    byte[] test = null;
        //    try
        //    {
        //        string hexTrimmed = hex.Replace("\r\n", " ");
        //        test = hexTrimmed.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToByte(x, 16)).ToArray();
        //        Console.WriteLine("{0}", hexTrimmed);
        //    }
        //    catch (Exception exception)
        //    {
        //        // Error
        //        Console.WriteLine("Exception caught in process: {0}", exception);
        //    }
        //    return test;
        //}

        //public static byte[] ReadFile(string fileName)
        //{
        //    byte[] buffer = null;

        //    try
        //    {
        //        // Open file for reading
        //        FileStream fileStream = new FileStream("packets\\" + fileName, FileMode.Open, FileAccess.Read);

        //        // attach filestream to stream reader
        //        StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8);

        //        // read entire file into string
        //        var hexString = streamReader.ReadToEnd();

        //        // close file reader
        //        fileStream.Close();
        //        fileStream.Dispose();
        //        streamReader.Close();

        //        // convert hex string to byte array, valid delimiters in files SPACE and ENTER
        //        buffer = HexFileToByteArray(hexString);
        //    }
        //    catch (Exception exception)
        //    {
        //        // Error
        //        Console.WriteLine("Exception caught in process: {0}", exception);
        //    }
        //    return buffer;
        //}

        ///// <summary>
        ///// Converts an array of bytes to an Int16 from a given offset.<br />
        ///// <i>offset</i> is incremented by the size of an Int16.
        ///// </summary>
        ///// <param name="byteArray">The byte array containing the Int16.</param>
        ///// <param name="offset">The initial offset within byteArray.</param>
        ///// <returns>The converted Int16 value.</returns>
        //public static Int16 ByteArrayToInt16(byte[] byteArray, ref int offset)
        //{
        //    Int16 value = BitConverter.ToInt16(byteArray, offset);
        //    offset += 2;
        //    return value;
        //}

        ///// <summary>
        ///// Converts an array of bytes to an Int16 from a given offset.<br />
        ///// <i>offset</i> is incremented by the size of an Int16.
        ///// <i>bytesRemaining</i> is decremented by the size of of an Int16
        ///// </summary>
        ///// <param name="byteArray">The byte array containing the Int16.</param>
        ///// <param name="offset">The initial offset within byteArray.</param>
        ///// <param name="bytesRemaining">The externally used bytes remaining counter to be updated.</param>
        ///// <returns>The converted Int16 value.</returns>
        //public static Int16 ByteArrayToInt16(byte[] byteArray, ref int offset, ref int bytesRemaining)
        //{
        //    Int16 value = BitConverter.ToInt16(byteArray, offset);
        //    offset += 2;
        //    bytesRemaining -= 2;
        //    return value;
        //}

        ///// <summary>
        ///// Converts an array of bytes to a Float from a given offset.<br />
        ///// <i>offset</i> is incremented by the size of a Float.
        ///// </summary>
        ///// <param name="byteArray">The byte array containing the Float.</param>
        ///// <param name="offset">The initial offset within byteArray.</param>
        ///// <returns>The converted Float value.</returns>
        //public static float ByteArrayToFloat(byte[] byteArray, ref int offset)
        //{
        //    float value = BitConverter.ToSingle(byteArray, offset);
        //    offset += 4;
        //    return value;
        //}

        ///// <summary>
        ///// Converts an array of bytes to an Int64 from a given offset.
        ///// <i>offset</i> is incremented by the size of an Int64.
        ///// </summary>
        ///// <param name="byteArray">The byte array containing the Int64.</param>
        ///// <param name="offset">The initial offset within byteArray.</param>
        ///// <returns>The converted Int64 value.</returns>
        //public static Int64 ByteArrayToInt64(byte[] byteArray, ref int offset)
        //{
        //    Int64 value = BitConverter.ToInt64(byteArray, offset);
        //    offset += 8;
        //    return value;
        //}

        ///// <summary>
        ///// Converts an array of bytes to an array of Int64 values.<br />
        ///// <i>offset</i> is incremented by the size of the Int64 array.
        ///// </summary>
        ///// <param name="byteArray">The byte array containing the Int32 array.</param>
        ///// <param name="offset">The initial offset within byteArray.</param>
        ///// <param name="count">The number of Int64 array elements.</param>
        ///// <returns>The converted Int64 array.</returns>
        //public static Int64[] ByteArrayToInt64Array(byte[] byteArray, ref int offset, int count)
        //{
        //    Int64[] int64Array = new Int64[count];

        //    GCHandle pinnedArray = GCHandle.Alloc(byteArray, GCHandleType.Pinned);
        //    IntPtr bytePtr = new IntPtr((int)pinnedArray.AddrOfPinnedObject() + offset);
        //    Marshal.Copy(bytePtr, int64Array, 0, count);
        //    pinnedArray.Free();
        //    offset += count * 8;

        //    return int64Array;
        //}


        ///// <summary>
        ///// Converts an array of bytes to a structure.
        ///// </summary>
        ///// <typeparam name="T">The type of structure.</typeparam>
        ///// <param name="byteArray">The byte array containing the structure.</param>
        ///// <param name="offset">The offset within the byte array to the structure.</param>
        ///// <returns>The converted object.</returns>
        //public static T ByteArrayToStructure<T>(byte[] byteArray, int offset)
        //{
        //    return ByteArrayToStructure<T>(byteArray, ref offset);
        //}

        ///// <summary>
        ///// Converts an array of bytes to an ASCII String from offset up to the
        ///// first <i>delimiter</i> character from offset or remaining bytes if <i>delimiter</i> can't be found.
        ///// </summary>
        ///// <param name="byteArray">The byte array containing the ASCII String.</param>
        ///// <param name="offset">The initial offset within byteArray.</param>
        ///// <param name="delimiter">The byte the signifies the end of the string.</param>
        ///// <returns>The converted ASCII String.</returns>
        //public static String ByteArrayToStringAscii(byte[] byteArray, int offset, byte delimiter)
        //{
        //    // may not look as pretty, but much faster/safter than using Marshal string crap
        //    // get first null location etc
        //    int arrayLenth = byteArray.Length;
        //    int strLength = 0;
        //    for (int i = offset; i < arrayLenth; i++)
        //    {
        //        if (byteArray[i] != delimiter) continue;

        //        strLength = i - offset;
        //        break;
        //    }

        //    if (strLength == 0)
        //    {
        //        strLength = byteArray.Length - offset;
        //    }

        //    return Encoding.ASCII.GetString(byteArray, offset, strLength);
        //}

        #endregion

        private static readonly int[] _bitCountTable =
        {
            0,
            1,
            2, 2,
            3, 3, 3, 3,
            4, 4, 4, 4, 4, 4, 4, 4,
            5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
            6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8
        };

        public static int GetMaxBits(int value)
        {
            var bitShift = -1;
            if ((value & 0xFF000000) > 0)
            {
                bitShift = 0x18;
            }
            else if ((value & 0xFF0000) > 0)
            {
                bitShift = 0x10;
            }
            else if ((value & 0xFF00) > 0)
            {
                bitShift = 0x08;
            }
            else if ((value & 0xFF) > 0)
            {
                bitShift = 0x00;
            }

            Debug.Assert(bitShift >= 0);

            var bitCountIndex = value >> bitShift;
            var bitCount = _bitCountTable[bitCountIndex] + bitShift;

            return bitCount;
        }


        /// <summary>
        ///     Reads a Stream and converts it to a byte array.
        /// </summary>
        /// <param name="stream">The Stream the read from.</param>
        /// <returns>The read byte array.</returns>
        public static byte[] StreamToByteArray(Stream stream)
        {
            if (stream == null)
            {
                return null;
            }

            using (var ms = new MemoryStream())
            {
                var buffer = new byte[1024];
                int bytes;
                while ((bytes = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, bytes);
                }

                var output = ms.ToArray();
                return output;
            }
        }


        /// <summary>
        ///     Converts an array of bytes to an Int32 from a given offset.<br />
        ///     <i>offset</i> is incremented by the size of an Int32.
        /// </summary>
        /// <param name="byteArray">The byte array containing the Int32.</param>
        /// <param name="offset">The initial offset within byteArray.</param>
        /// <returns>The converted Int32 value.</returns>
        public static int ByteArrayToInt32(byte[] byteArray, ref int offset)
        {
            var value = BitConverter.ToInt32(byteArray, offset);
            offset += 4;
            return value;
        }

        /// <summary>
        ///     Converts an array of bytes to an Int32 from a given offset.
        /// </summary>
        /// <param name="byteArray">The byte array containing the Int32.</param>
        /// <param name="offset">The initial offset within byteArray.</param>
        /// <returns>The converted Int32 value.</returns>
        public static int ByteArrayToInt32(byte[] byteArray, int offset)
        {
            return BitConverter.ToInt32(byteArray, offset);
        }

        /// <summary>
        ///     Converts an array of bytes to a UInt32 from a given offset.<br />
        ///     <i>offset</i> is incremented by the size of a UInt32.
        /// </summary>
        /// <param name="byteArray">The byte array containing the UInt32.</param>
        /// <param name="offset">The initial offset within byteArray.</param>
        /// <returns>The converted UInt32 value.</returns>
        public static uint ByteArrayToUInt32(byte[] byteArray, ref int offset)
        {
            var value = BitConverter.ToUInt32(byteArray, offset);
            offset += 4;
            return value;
        }

        /// <summary>
        ///     Converts an array of bytes to an UInt32 from a given offset.
        /// </summary>
        /// <param name="byteArray">The byte array containing the UInt32.</param>
        /// <param name="offset">The initial offset within byteArray.</param>
        /// <returns>The converted UInt32 value.</returns>
        public static uint ByteArrayToUInt32(byte[] byteArray, int offset)
        {
            return BitConverter.ToUInt32(byteArray, offset);
        }


        /// <summary>
        ///     Converts an array of bytes to a UInt16 from a given offset.<br />
        ///     <i>offset</i> is incremented by the size of a UInt16.
        /// </summary>
        /// <param name="byteArray">The byte array containing the UInt16.</param>
        /// <param name="offset">The initial offset within byteArray.</param>
        /// <returns>The converted UInt16 value.</returns>
        public static ushort ByteArrayToUShort(byte[] byteArray, ref int offset)
        {
            var value = BitConverter.ToUInt16(byteArray, offset);
            offset += 2;
            return value;
        }


        /// <summary>
        ///     Converts an array of bytes to an array of short values.<br />
        ///     <i>offset</i> is incremented by the size of the short array.
        /// </summary>
        /// <param name="byteArray">The byte array containing the short array.</param>
        /// <param name="offset">The initial offset within byteArray.</param>
        /// <param name="count">The number of short array elements.</param>
        /// <returns>The converted short array.</returns>
        public static short[] ByteArrayToShortArray(byte[] byteArray, ref int offset, int count)
        {
            var shortArray = new short[count];

            var pinnedArray = GCHandle.Alloc(byteArray, GCHandleType.Pinned);
            var bytePtr = new IntPtr((int) pinnedArray.AddrOfPinnedObject() + offset);
            Marshal.Copy(bytePtr, shortArray, 0, count);
            pinnedArray.Free();
            offset += count * 2;

            return shortArray;
        }

        /// <summary>
        ///     Converts an array of bytes to an array of Int32 values.
        /// </summary>
        /// <param name="byteArray">The byte array containing the Int32 array.</param>
        /// <param name="offset">The initial offset within byteArray.</param>
        /// <param name="count">The number of Int32 array elements.</param>
        /// <returns>The converted Int32 array.</returns>
        public static int[] ByteArrayToInt32Array(byte[] byteArray, int offset, int count)
        {
            return ByteArrayToInt32Array(byteArray, ref offset, count);
        }

        /// <summary>
        ///     Converts an array of bytes to an array of Int32 values.<br />
        ///     <i>offset</i> is incremented by the size of the Int32 array.
        /// </summary>
        /// <param name="byteArray">The byte array containing the Int32 array.</param>
        /// <param name="offset">The initial offset within byteArray.</param>
        /// <param name="count">The number of Int32 array elements.</param>
        /// <returns>The converted Int32 array.</returns>
        public static int[] ByteArrayToInt32Array(byte[] byteArray, ref int offset, int count)
        {
            var int32Array = new int[count];

            var pinnedArray = GCHandle.Alloc(byteArray, GCHandleType.Pinned);
            var bytePtr = new IntPtr((int) pinnedArray.AddrOfPinnedObject() + offset);
            Marshal.Copy(bytePtr, int32Array, 0, count);
            pinnedArray.Free();
            offset += count * 4;

            return int32Array;
        }


        /// <summary>
        ///     Converts an array of bytes to a structure.<br />
        ///     <i>offset</i> is incremented by the size of the structure.
        /// </summary>
        /// <param name="byteArray">The byte array containing the structure.</param>
        /// <param name="type">The type of structure.</param>
        /// <param name="offset">The offset within the byte array to the structure.</param>
        /// <returns>The converted object.</returns>
        public static object ByteArrayToStructure(byte[] byteArray, Type type, ref int offset)
        {
            var structSize = Marshal.SizeOf(type);

            var hcHandle = GCHandle.Alloc(byteArray, GCHandleType.Pinned);
            var ptrStruct = Marshal.AllocHGlobal(structSize);
            Marshal.Copy(byteArray, offset, ptrStruct, structSize);
            var structure = Marshal.PtrToStructure(ptrStruct, type);
            Marshal.FreeHGlobal(ptrStruct);
            hcHandle.Free();

            offset += structSize;
            return structure;
        }

        /// <summary>
        ///     Converts an array of bytes to a structure.<br />
        ///     <i>offset</i> is incremented by the size of the structure.
        /// </summary>
        /// <typeparam name="T">The type of structure.</typeparam>
        /// <param name="byteArray">The byte array containing the structure.</param>
        /// <param name="offset">The offset within the byte array to the structure.</param>
        /// <returns>The converted object.</returns>
        public static T ByteArrayToStructure<T>(byte[] byteArray, ref int offset)
        {
            var structType = typeof(T);
            var structSize = Marshal.SizeOf(structType);

            var hcHandle = GCHandle.Alloc(byteArray, GCHandleType.Pinned);
            var ptrStruct = new IntPtr((int) hcHandle.AddrOfPinnedObject() + offset);
            var structure = (T) Marshal.PtrToStructure(ptrStruct, structType);
            hcHandle.Free();

            offset += structSize;
            return structure;
        }


        /// <summary>
        ///     Converts an array of bytes to an array of T values.<br />
        ///     <b>
        ///         This function should not be used for standard types (e.g. Int32).<br />
        ///         (The Marshal.Copy() function can do standard types)
        ///     </b>
        /// </summary>
        /// <typeparam name="T">The type of array elements.</typeparam>
        /// <param name="byteArray">The byte array containing the T array.</param>
        /// <param name="offset">The initial offset within byteArray.</param>
        /// <param name="count">The number of T array elements.</param>
        /// <returns>The converted T array.</returns>
        public static T[] ByteArrayToArray<T>(byte[] byteArray, int offset, int count)
        {
            return ByteArrayToArray<T>(byteArray, ref offset, count);
        }

        /// <summary>
        ///     Converts an array of bytes to an array of T values.<br />
        ///     <i>offset</i> is incremented by the size of the T array.<br />
        ///     <b>
        ///         This function should not be used for standard types (e.g. Int32).<br />
        ///         (The Marshal.Copy() function can do standard types)
        ///     </b>
        /// </summary>
        /// <typeparam name="T">The type of array elements.</typeparam>
        /// <param name="byteArray">The byte array containing the T array.</param>
        /// <param name="offset">The initial offset within byteArray.</param>
        /// <param name="count">The number of T array elements.</param>
        /// <returns>The converted T array.</returns>
        public static unsafe T[] ByteArrayToArray<T>(byte[] byteArray, ref int offset, int count)
        {
            Debug.Assert(offset <= byteArray.Length, "Error: offset > byteArray.Length");

            var sizeOfT = Marshal.SizeOf(typeof(T));
            var sizeOfBuffer = sizeOfT * count;
            Debug.Assert(offset + sizeOfBuffer <= byteArray.Length, "Error: offset + sizeOfBuffer > byteArray.Length");

            var obj = new T[count];
            fixed (byte* pData = byteArray)
            {
                for (var i = 0; i < count; i++)
                {
                    var addr = new IntPtr(pData + offset);
                    obj[i] = (T) Marshal.PtrToStructure(addr, typeof(T));
                    offset += sizeOfT;
                }
            }

            return obj;
        }

        /// <summary>
        ///     Converts an array of bytes to an ASCII String from offset up to the first null character.
        /// </summary>
        /// <param name="byteArray">The byte array containing the ASCII String.</param>
        /// <param name="offset">The initial offset within byteArray.</param>
        /// <returns>The converted ASCII String.</returns>
        public static string ByteArrayToStringAscii(byte[] byteArray, int offset)
        {
            string result;

            unsafe
            {
                fixed (byte* pAscii = &byteArray[offset])
                {
                    result = new string((sbyte*) pAscii);
                }
            }

            return result;
        }


        /// <summary>
        ///     Converts an array of bytes to an ASCII String from offset up to the first null character.<br />
        ///     <i>offset</i> is incremented by the <i>len</i> argument value.
        /// </summary>
        /// <param name="byteArray">The byte array containing the ANSI String.</param>
        /// <param name="offset">The initial offset within byteArray.</param>
        /// <param name="len">The length of the string to convert up to.</param>
        /// <returns>The converted ASCII String.</returns>
        public static string ByteArrayToStringAscii(byte[] byteArray, ref int offset, int len)
        {
            var str = Encoding.ASCII.GetString(byteArray, offset, len);
            offset += len;
            return str;
        }

        /// <summary>
        ///     Converts an array of bytes to a Unicode String from offset up to the first null character.<br />
        /// </summary>
        /// <param name="byteArray">The byte array containing the Unicode String.</param>
        /// <param name="offset">The initial offset within byteArray.</param>
        /// <param name="maxByteCount">The maximum number of bytes to use/check.</param>
        /// <returns>The converted Unicode String.</returns>
        public static string ByteArrayToStringUnicode(byte[] byteArray, int offset, int maxByteCount)
        {
            // may not look as pretty, but much faster/safter than using Marshal string crap
            // get first null location etc
            var arrayLenth = byteArray.Length;
            var strLength = 0;
            var currOffset = offset;
            for (var i = 0; currOffset < arrayLenth && i < maxByteCount; i++, currOffset += 2)
            {
                if (byteArray[currOffset] != 0x00)
                {
                    continue;
                }

                strLength = currOffset - offset;
                break;
            }

            if (strLength == 0 && currOffset != offset /*make sure not empty string*/)
            {
                strLength = arrayLenth - offset;
            }

            var str = Encoding.Unicode.GetString(byteArray, offset, strLength);
            return str;
        }

        /// <summary>
        ///     Converts a String into its Unicode byte array.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>The converted byte array.</returns>
        public static byte[] StringToUnicodeByteArray(string str)
        {
            return string.IsNullOrEmpty(str) ? new byte[0] : Encoding.Unicode.GetBytes(str);
        }

        /// <summary>
        ///     Converts a String into its ASCII byte array.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>The converted byte array.</returns>
        public static byte[] StringToAsciiByteArray(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        /// <summary>
        ///     Converts a string into the given type.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <param name="type">The type to convert the string into.</param>
        /// <returns>The converted object. If the type was unhandled, null will be returned.</returns>
        public static object StringToObject(string value, Type type)
        {
            if (type == typeof(string))
            {
                return value;
            }

            if (type == typeof(int))
            {
                return int.Parse(value);
            }

            if (type == typeof(uint))
            {
                return uint.Parse(value);
            }

            if (type == typeof(float))
            {
                return float.Parse(value);
            }

            if (type == typeof(short))
            {
                return short.Parse(value);
            }

            if (type == typeof(ushort))
            {
                return ushort.Parse(value);
            }

            if (type == typeof(byte))
            {
                return byte.Parse(value);
            }

            if (type == typeof(char))
            {
                return char.Parse(value);
            }

            if (type == typeof(long))
            {
                return long.Parse(value);
            }

            if (type == typeof(ulong))
            {
                return ulong.Parse(value);
            }

            return null;
        }

        public static T[] StringToArray<T>(string array, string delimiter)
        {
            var type = typeof(T);
            var elements = array.Split(new[] {delimiter}, StringSplitOptions.None);
            var returnArray = new T[elements.Length];

            for (var i = 0; i < elements.Length; i++)
            {
                returnArray[i] = (T) StringToObject(elements[i], type);
            }

            return returnArray;
        }

        /// <summary>
        ///     Searches a byte array for a sequence of bytes.<br />
        ///     <i>Uses 0x90 as a wild card.</i>
        /// </summary>
        /// <param name="byteArray">The byte array to search.</param>
        /// <param name="searchFor">The byte sequence to search for.</param>
        /// <returns>The index found or -1.</returns>
        public static int ByteArrayContains(byte[] byteArray, byte[] searchFor)
        {
            for (var i = 0; i < byteArray.Length; i++)
            {
                var found = !searchFor.Where((t, j) => t != 0x90 && byteArray[i + j] != t).Any();

                if (found)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        ///     Converts an Object into a byte array.
        /// </summary>
        /// <param name="obj">The Object to convert.</param>
        /// <returns>The converted byte array.</returns>
        public static byte[] StructureToByteArray(object obj)
        {
            var length = Marshal.SizeOf(obj);
            var byteArray = new byte[length];

            var intPtr = Marshal.AllocHGlobal(length);
            Marshal.StructureToPtr(obj, intPtr, false);
            Marshal.Copy(intPtr, byteArray, 0, length);
            Marshal.FreeHGlobal(intPtr);

            return byteArray;
        }

        /// <summary>
        ///     Converts an Int Array to its respective Byte Array.
        /// </summary>
        /// <param name="source">The Int Array to serialise.</param>
        /// <returns>The Serialised Int Array.</returns>
        public static byte[] IntArrayToByteArray(int[] source)
        {
            var result = new byte[source.Length * sizeof(int)];

            for (var i = 0; i < source.Length; i++)
            {
                Array.Copy(BitConverter.GetBytes(source[i]), 0, result, i * sizeof(int), sizeof(int));
            }

            return result;
        }


        /// <summary>
        ///     Serializes an object and appends it to the supplied buffer, increasing offset by object size.<br />
        ///     If the buffer is too small the bufer size is increaed by the object size + 1024 bytes.
        ///     A string object will be serialized to an ASCII byte array.
        /// </summary>
        /// <param name="buffer">A reference to a byte array (not null).</param>
        /// <param name="offset">A reference to the write offset (offset is increased by the size of object).</param>
        /// <param name="toWrite">A sersializable object to write.</param>
        public static void WriteToBuffer(ref byte[] buffer, int offset, object toWrite)
        {
            WriteToBuffer(ref buffer, ref offset, toWrite);
        }

        ///// <summary>
        ///// Serializes an array of object arrays and appends it to the supplied buffer, increasing offset by object size.<br />
        ///// If the buffer is too small the bufer size is increaed by the object size + 1024 bytes.
        ///// A string object will be serialized to an ASCII byte array.
        ///// </summary>
        ///// <param name="buffer">A reference to a byte array (not null).</param>
        ///// <param name="offset">A reference to the write offset (offset is increased by the size of object array).</param>
        ///// <param name="toWriteArray">A sersializable array of object arrays to write.</param>
        //public static void WriteToBuffer(ref byte[] buffer, ref int offset, Object[][] toWriteArray)
        //{
        //    foreach (Object[] objArray in toWriteArray)
        //    {
        //        WriteToBuffer(ref buffer, ref offset, objArray);
        //    }
        //}


        /// <summary>
        ///     Serializes an object array and appends it to the supplied buffer, increasing offset by object size.<br />
        ///     If the buffer is too small the bufer size is increaed by the object size + 1024 bytes.
        ///     A string object will be serialized to an ASCII byte array.
        /// </summary>
        /// <param name="buffer">A reference to a byte array (not null).</param>
        /// <param name="offset">A reference to the write offset (offset is increased by the size of object array).</param>
        /// <param name="toWriteArray">A sersializable object array to write.</param>
        public static void WriteToBuffer(ref byte[] buffer, ref int offset, object[] toWriteArray)
        {
            foreach (var obj in toWriteArray)
            {
                WriteToBuffer(ref buffer, ref offset, obj);
            }
        }

        /// <summary>
        ///     Serializes an object and appends it to the supplied buffer, increasing offset by object size.<br />
        ///     If the buffer is too small the bufer size is increaed by the object size + 1024 bytes.
        ///     A string object will be serialized to an ASCII byte array.
        /// </summary>
        /// <param name="buffer">A reference to a byte array (not null).</param>
        /// <param name="offset">A reference to the write offset (offset is increased by the size of object).</param>
        /// <param name="toWrite">A sersializable object to write.</param>
        public static void WriteToBuffer(ref byte[] buffer, ref int offset, object toWrite)
        {
            if (toWrite is string str)
            {
                toWrite = str.ToAsciiByteArray();
            }

            var toWriteBytes = toWrite as byte[] ?? StructureToByteArray(toWrite);

            WriteToBuffer(ref buffer, ref offset, toWriteBytes, toWriteBytes.Length, false);
        }

        //public static void WriteToBuffer(ref byte[] buffer, int offset, byte[] toWriteBytes, int lengthToWrite, bool insert)
        //{
        //    WriteToBuffer(ref buffer, ref offset, toWriteBytes, lengthToWrite, insert);
        //}

        public static void WriteToBuffer(ref byte[] buffer, ref int offset, byte[] toWriteBytes, int lengthToWrite,
            bool insert)
        {
            byte[] insertBuffer = null;
            if (insert)
            {
                insertBuffer = new byte[buffer.Length - offset];
                Buffer.BlockCopy(buffer, offset, insertBuffer, 0, insertBuffer.Length);
            }

            if (offset + lengthToWrite > buffer.Length || insert)
            {
                var newBuffer = new byte[buffer.Length + lengthToWrite + offset + 1024];
                Buffer.BlockCopy(buffer, 0, newBuffer, 0, buffer.Length);
                buffer = newBuffer;
            }

            Buffer.BlockCopy(toWriteBytes, 0, buffer, offset, lengthToWrite);

            if (insert)
            {
                Buffer.BlockCopy(insertBuffer, 0, buffer, offset + lengthToWrite, insertBuffer.Length);
            }

            offset += lengthToWrite;
        }

        public static void BinaryToArray<T>(BinaryReader binReader, T[] destination)
        {
            for (var i = 0; i < destination.Length; i++)
            {
                var byteArray = binReader.ReadBytes(Marshal.SizeOf(typeof(T)));
                var offset = 0;
                destination[i] = ByteArrayToStructure<T>(byteArray, ref offset);
            }
        }

        public static string ArrayToStringGeneric<T>(IList<T> array, string delimeter)
        {
            var outputString = "";

            for (var i = 0; i < array.Count; i++)
            {
                if (array[i] is IList<T>)
                {
                    //Recursively convert nested arrays to string
                    outputString += ArrayToStringGeneric((IList<T>) array[i], delimeter);
                }
                else
                {
                    outputString += array[i];
                }

                if (i != array.Count - 1)
                {
                    outputString += delimeter;
                }
            }

            return outputString;
        }

        public static string ObjectToStringGeneric(object obj, string delimiter)
        {
            var outputString = string.Empty;
            const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var fieldInfo = obj.GetType().GetFields(BINDING_FLAGS);
            for (var i = 0; i < fieldInfo.Length; i++)
            {
                outputString += fieldInfo[i].GetValue(obj).ToString();
                if (i != fieldInfo.Length - 1)
                {
                    outputString += delimiter;
                }
            }

            return outputString;
        }

        public static T StringToObject<T>(string str, string delimiter, FieldInfo[] fieldInfos) where T : new()
        {
            var strFields = str.Split(delimiter.ToCharArray());
            if (strFields.Length != fieldInfos.Length)
            {
                return default(T);
            }

            var obj = Activator
                .CreateInstance(
                    typeof(T)); // do not cast this to T here - will fail on struct types when trying to SetValue
            for (var i = 0; i < fieldInfos.Length; i++)
            {
                var value = StringToObject(strFields[i], fieldInfos[i].FieldType);
                fieldInfos[i].SetValue(obj, value);
            }

            return (T) obj;
        }

        //public static string ByteArrayToDelimitedAsciiString(byte[] data, char delimiter, Type castAs)
        //{
        //    using (StringWriter sw = new StringWriter())
        //    {
        //        int noValues = data.Length;
        //        string valueCast = castAs.Name;
        //        int castLen = Marshal.SizeOf(castAs);

        //        for (int i = 0; i < data.Length; i = i + castLen)
        //        {
        //            switch (valueCast)
        //            {
        //                case "Int32":
        //                    sw.Write(BitConverter.ToInt32(data, i).ToString());
        //                    break;
        //                case "Byte":
        //                    sw.Write(data[i].ToString("X2"));
        //                    break;
        //            }

        //            if (i != data.Length - castLen) sw.Write(delimiter);
        //        }

        //        return sw.ToString();
        //    }
        //}

        public static string[][] CsvToStringArray(byte[] source, int columns, byte delimiter,
            bool ignoreFirstRow = false)
        {
            if (source == null)
            {
                return null;
            }

            if (source.Length == 0)
            {
                return null;
            }

            const byte CR = 0x0D;
            const byte LF = 0x0A;
            //byte EN = 0x22;
            var offset = 0;
            var length = source.Length;
            var rowCollection = new List<string[]>();
            var row = 0;
            while (offset < length)
            {
                var newRow = new string[columns];
                for (var i = 0; i < columns; i++)
                {
                    var buffer = offset < length ? GetDelimintedByteArray(source, ref offset, delimiter) : null;
                    var newString = buffer == null || buffer.Length == 0
                        ? string.Empty
                        : ByteArrayToStringAscii(buffer, 0);
                    newRow[i] = newString.Replace("\"", "");
                }

                //if (offset < source.Length && source[offset] == delimiter) offset += sizeof(byte);
                if (offset < source.Length && source[offset] == CR)
                {
                    offset += sizeof(byte);
                }

                if (offset < source.Length && source[offset] == LF)
                {
                    offset += sizeof(byte);
                }

                if (row == 0 && ignoreFirstRow)
                {
                    row++;
                    continue;
                }

                rowCollection.Add(newRow);
            }

            return rowCollection.ToArray();
        }

        public static string[][] UnicodeCsvToStringArray(byte[] source, ushort delimiter, ushort encap)
        {
            if (source == null)
            {
                return null;
            }

            if (source.Length == 0)
            {
                return null;
            }

            const ushort CR = 0x0D;
            const ushort LF = 0x0A;
            const ushort EN = 0x22;

            var noCols = 1;
            var offset = 0;
            ushort characterIn;

            // Get a column count
            while (offset < source.Length)
            {
                characterIn = ByteArrayToUShort(source, ref offset);
                if (characterIn == delimiter)
                {
                    noCols++;
                }

                if (characterIn == CR || characterIn == LF)
                {
                    break;
                }

                if (offset == source.Length)
                {
                    return null;
                }
            }

            offset = 0;
            var col = 0;
            var encapsulationOpen = false;
            var lastCharacterEncap = false;
            var stringLen = 0;
            var arrayBuffer = new string[noCols];
            var stringList = new List<string[]>();

            // Excel puts this token here.
            if (BitConverter.ToUInt16(source, offset) == 0xFEFF)
            {
                offset += sizeof(short);
            }

            while (offset < source.Length)
            {
                characterIn = ByteArrayToUShort(source, ref offset);
                if (characterIn == EN && !lastCharacterEncap)
                {
                    encapsulationOpen = !encapsulationOpen;
                }

                lastCharacterEncap = characterIn == EN;

                if (!encapsulationOpen)
                {
                    if (characterIn == delimiter)
                    {
                        arrayBuffer[col] =
                            Encoding.Unicode.GetString(source, offset - stringLen - sizeof(short), stringLen);
                        arrayBuffer[col] = arrayBuffer[col].Replace("\"", "");
                        col++;
                        stringLen = 0;
                        continue;
                    }

                    if (characterIn == CR || characterIn == LF)
                    {
                        arrayBuffer[col] =
                            Encoding.Unicode.GetString(source, offset - stringLen - sizeof(short), stringLen);
                        arrayBuffer[col] = arrayBuffer[col].Replace("\"", "");
                        stringList.Add(arrayBuffer);
                        stringLen = 0;
                        col = 0;
                        arrayBuffer = new string[noCols];
                        var peek = BitConverter.ToUInt16(source, offset);
                        if (peek == CR || peek == LF)
                        {
                            offset += sizeof(ushort);
                        }

                        continue;
                    }
                }

                stringLen += sizeof(short);
            }

            return stringList.ToArray();
        }

        /// <summary>
        ///     Collects tbe bytes read in the source array until the delimiter byte is encounted.
        /// </summary>
        /// <param name="source">The array containing your data.</param>
        /// <param name="offset">The starting position in the source array to read from.</param>
        /// <param name="delimiter">The byte to interpret as the delimiter symbol.</param>
        /// <returns></returns>
        public static byte[] GetDelimintedByteArray(byte[] source, ref int offset, byte delimiter)
        {
            //List<byte> buffer = new List<byte>();
            var length = source.Length;
            const byte END_ROW = 0x0D;

            // empty string
            if (source[offset] == delimiter)
            {
                offset++; // skip delimiter
                return null;
            }

            // not empty
            var startOffset = offset;
            while (offset < length && source[offset] != delimiter && source[offset] != END_ROW)
            {
                offset++;
                //buffer.Add(source[offset++]);
            }

            var byteCount = offset - startOffset;
            var bytes = new byte[byteCount];
            Buffer.BlockCopy(source, startOffset, bytes, 0, byteCount);

            if (offset < length && source[offset] == delimiter)
            {
                offset++; // skip delimiter
            }
            else
            {
                offset++;
                offset++;
            }

            return bytes;
            //return buffer.ToArray();
        }

        public static byte[] ToByteArray(this float[] floatArray)
        {
            var typeConvert = new ArrayTypeConverter {Floats = floatArray};

            var bytes = new byte[floatArray.Length * 4];
            Buffer.BlockCopy(typeConvert.Bytes, 0, bytes, 0, bytes.Length);

            return bytes;
        }

        public static byte[] ToByteArray(this int[] int32Array)
        {
            var typeConvert = new ArrayTypeConverter {Int32s = int32Array};

            var bytes = new byte[int32Array.Length * 4];
            Buffer.BlockCopy(typeConvert.Bytes, 0, bytes, 0, bytes.Length);

            return bytes;
        }

        public static byte[] ToByteArray(this short[] shortArray)
        {
            var typeConvert = new ArrayTypeConverter {Shorts = shortArray};

            var bytes = new byte[shortArray.Length * 2];
            Buffer.BlockCopy(typeConvert.Bytes, 0, bytes, 0, bytes.Length);

            return bytes;
        }

        //public static byte[] ToByteArray(this Object[][] objectArrayArray)
        //{
        //    int arrayCount = objectArrayArray.Length;
        //    if (arrayCount == 0) return new byte[0];

        //    throw new NotImplementedException();
        //}

        public static byte[] ToByteArray(this object[] objectArray)
        {
            var arrayCount = objectArray.Length;
            if (arrayCount == 0)
            {
                return new byte[0];
            }

            var length = Marshal.SizeOf(objectArray[0]);
            var arrayLength = arrayCount * length;
            var byteArray = new byte[arrayLength];
            for (var i = 0; i < arrayCount; i++)
            {
                var bytes = StructureToByteArray(objectArray[i]);
                Buffer.BlockCopy(bytes, 0, byteArray, i * length, length);
            }

            return byteArray;
        }

        public static byte[] ToAsciiByteArray(this string str)
        {
            return StringToAsciiByteArray(str);
        }

        public static byte[] ToUnicodeByteArray(this string str)
        {
            return StringToUnicodeByteArray(str);
        }

        public static string ToString<T>(this IEnumerable<T> source, string separator)
        {
            var array = source.Where(n => n != null).Select(n => n.ToString()).ToArray();
            return string.Join(separator, array);
        }

        public static string ToString(this IEnumerable source, string separator)
        {
            var array = source.Cast<object>().Where(n => n != null).Select(n => n.ToString()).ToArray();
            return string.Join(separator, array);
        }

        //public static void InitArray<T>(T[] arr, T val)
        //{
        //    for (int i = 0; i < arr.Length; i++)
        //    {
        //        arr[i] = val;
        //    }
        //}

        public static T[] ToArray<T>(this string source, char separator)
        {
            return source.ToArray<T>(new[] {separator});
        }

        public static T[] ToArray<T>(this string source, char[] separator)
        {
            var elements = source.Split(separator, StringSplitOptions.None);
            var elementCount = elements.Length;
            var returnType = typeof(T);

            var retArr = new T[elementCount];
            for (var i = 0; i < elementCount; i++)
            {
                retArr[i] = (T) Convert.ChangeType(elements[i], returnType);
            }

            return retArr;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct ArrayTypeConverter
        {
            [FieldOffset(0)] public readonly byte[] Bytes;
            [FieldOffset(0)] public float[] Floats;
            [FieldOffset(0)] public int[] Int32s;
            [FieldOffset(0)] private readonly int[] Int64s;
            [FieldOffset(0)] public short[] Shorts;
        }
    }
}