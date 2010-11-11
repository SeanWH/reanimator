﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using Revival.Common;

namespace Hellgate
{
    public partial class StringsFile : DataFile
    {
        private StringsHeader _stringsHeader;
        public List<StringBlock> StringsTable { get; private set; }

        public StringsFile()
        {
            IsStringsFile = true;
        }

        public override bool ParseData(byte[] buffer)
        {
            int offset = 0;
            StringsTable = new List<StringBlock>();

            _stringsHeader = FileTools.ByteArrayToStructure<StringsHeader>(buffer, ref offset);

            for (int i = 0; i < _stringsHeader.Count; i++)
            {
                StringBlock stringBlock = new StringBlock
                {
                    ReferenceId = FileTools.ByteArrayToInt32(buffer, ref offset),
                    Unknown = FileTools.ByteArrayToInt32(buffer, ref offset)
                };

                int count = FileTools.ByteArrayToInt32(buffer, ref offset);
                stringBlock.StringId = FileTools.ByteArrayToStringASCII(buffer, offset);
                offset += count + 1;

                stringBlock.Reserved = FileTools.ByteArrayToInt32(buffer, ref offset);

                count = FileTools.ByteArrayToInt32(buffer, ref offset);
                stringBlock.String = FileTools.ByteArrayToStringUnicode(buffer, offset);
                offset += count;

                stringBlock.AttributeCount = FileTools.ByteArrayToInt32(buffer, ref offset);

                for (int j = 0; j < stringBlock.AttributeCount; j++)
                {
                    count = FileTools.ByteArrayToInt32(buffer, ref offset);

                    switch (j)
                    {
                        case 0:
                            stringBlock.Attribute1 = FileTools.ByteArrayToStringUnicode(buffer, offset);
                            break;
                        case 1:
                            stringBlock.Attribute2 = FileTools.ByteArrayToStringUnicode(buffer, offset);
                            break;
                        case 2:
                            stringBlock.Attribute3 = FileTools.ByteArrayToStringUnicode(buffer, offset);
                            break;
                        case 3:
                            stringBlock.Attribute4 = FileTools.ByteArrayToStringUnicode(buffer, offset);
                            break;
                    }

                    offset += (count + 1) * 2;
                }

                StringsTable.Add(stringBlock);
            }

            if (offset != buffer.Length)
            {
                return false;
            }

            IntegrityCheck = true;
            return true;
        }

        public override bool ParseCSV(byte[] buffer)
        {
            throw new NotImplementedException();
        }

        public override bool ParseDataTable(DataTable table)
        {
            byte[] buffer = new byte[1024];
            int offset = 0;
            int lastOffset = 4;

            // write main header first
            StringsHeader stringsHeader = new StringsHeader
            {
                Header = FileTokens.Header,
                Version = Version,
                Count = table.Rows.Count
            };
            FileTools.WriteToBuffer(ref buffer, ref offset, stringsHeader);

            // write string blocks
            int row = 0;
            foreach (DataRow dr in table.Rows)
            {
                String stringId = dr["StringID"] as String;
                if (String.IsNullOrEmpty(stringId)) continue;


                // ReferenceId
                Int32 referenceId = (Int32)dr[0];
                FileTools.WriteToBuffer(ref buffer, ref offset, referenceId);


                // Unknown
                Int32 unknownValue = (Int32)dr[1];
                FileTools.WriteToBuffer(ref buffer, ref offset, unknownValue);


                // StringId
                byte[] stringIdBytes = FileTools.StringToASCIIByteArray(stringId);
                Int32 byteCount = stringIdBytes.Length;
                FileTools.WriteToBuffer(ref buffer, ref offset, byteCount);
                FileTools.WriteToBuffer(ref buffer, ref offset, stringIdBytes);
                offset++; // \0


                // Reserved
                offset += sizeof(Int32);


                // String
                String str = dr["String"] as String;
                byteCount = 2; // \0
                if (String.IsNullOrEmpty(str))
                {
                    FileTools.WriteToBuffer(ref buffer, ref offset, byteCount);
                }
                else
                {
                    byte[] stringBytes = FileTools.StringToUnicodeByteArray(str);
                    byteCount += stringBytes.Length;
                    FileTools.WriteToBuffer(ref buffer, ref offset, byteCount);
                    FileTools.WriteToBuffer(ref buffer, ref offset, stringBytes);
                    
                }
                offset += 2; // \0


                // Attributes
                List<String> attributeStrings = new List<string>();
                for (int i = 0; i < MaxAttributes; i++)
                {
                    String colName = "Attribute" + (i + 1);
                    if (!table.Columns.Contains(colName)) continue;

                    String attribute = dr[colName] as String;
                    if (String.IsNullOrEmpty(attribute)) continue;

                    attributeStrings.Add(attribute);
                }

                Int32 attributeCount = attributeStrings.Count;
                FileTools.WriteToBuffer(ref buffer, ref offset, attributeCount);

                foreach (String s in attributeStrings)
                {
                    Int32 charCount = s.Length;
                    FileTools.WriteToBuffer(ref buffer, ref offset, charCount);
                    byte[] attributeBytes = FileTools.StringToUnicodeByteArray(s);
                    FileTools.WriteToBuffer(ref buffer, offset, attributeBytes);
                    lastOffset = attributeBytes.Length + 2; // \0
                    offset += lastOffset;
                }

                row++;
            }

            //offset -= lastOffset;
            byte[] returnBuffer = new byte[offset];
            Buffer.BlockCopy(buffer, 0, returnBuffer, 0, offset);

            return true;
        }

        public override byte[] ToByteArray()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts the StringsFile to a CSV
        /// </summary>
        /// <returns>The CSV as a byte array.</returns>
        public override byte[] ExportCSV()
        {
            throw new NotImplementedException();
        }
    }
}