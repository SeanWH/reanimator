﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;

namespace Reanimator
{
    abstract class XmlCookedBase
    {
        /*
        private class XmlUnknownElement
        {
            private short _token;

            public XmlUnknownElement(short token)
            {
                _token = token;
            }
        }*
         */

        public XmlDocument XmlDoc { get; private set; }
        protected readonly List<Object> DataElements;

        protected byte[] Data;
        protected int Offset;

        protected XmlCookedBase()
        {
            XmlDoc = new XmlDocument();
            DataElements = new List<Object>();
            
            Data = null;
            Offset = 0;
        }

        protected abstract bool ParseDataSegment(XmlElement dataElement);

        public bool ParseData(byte[] data)
        {
            if (data == null) return false;
            Data = data;

            int fileHeadToken = FileTools.ByteArrayTo<Int32>(Data, ref Offset);
            if (fileHeadToken != 0x6B304F43) return false;  // 'CO0k'

            int version = FileTools.ByteArrayTo<Int32>(Data, ref Offset);
            if (version != 0x08) return false;

            XmlElement mainElement = XmlDoc.CreateElement("CO0k");
            XmlDoc.AppendChild(mainElement);

            XmlElement versionElement = XmlDoc.CreateElement("version");
            versionElement.InnerText = "8";
            mainElement.AppendChild(versionElement);


            /* ==Strange Array==
             * 4 bytes		unknown
             * 2 bytes		Type Token
             * *remaining based on token*
             * 
             * =Token=		=Followed By=
             *  00 00       4 bytes (Int32?  Or UInt32 because 00 07 appears to have -1?)
             *  00 01       4 bytes (Float)
             *  00 02       1 byte  (Str Length (NOT inc \0) - if != 0, string WITH \0)
             *  00 06       4 bytes (Float)
             *  00 07       4 bytes (Int32?  Or possibly an array with -1 = end?)
             *  01 0B       8 bytes	(Null Int32?,  32 bit Bitmask)
             *  02 0C       8 bytes	(Int32 index?,  Int32 value?)
             *  03 00       2 bytes	(ShortInt?)
             *  03 09       4 bytes	(Int32?)
             *  05 00       2 bytes (??)
             *  08 03       8 bytes	(Int32??,  Int32??)
             *  09 03       4 bytes (Int32)
             *  0A 03       8 bytes	(Int32??,  Int32??)
             *  C0 00       2 bytes (??)
             *  
             *  // extras found in particle effects
             *  8D 00       2 bytes
             *  00 0A       4 bytes (Int32?)
             *  06 01       8 bytes (Int32?, Int32?)
             *  00 05       4 bytes (Float)
             */

            XmlElement unknownArray = XmlDoc.CreateElement("unknownArray");
            mainElement.AppendChild(unknownArray);

            while (Offset < Data.Length)
            {
                uint unknown = FileTools.ByteArrayTo<UInt32>(Data, ref Offset);

                if (unknown == 0x41544144) break;     // 'DATA'

                ushort token = FileTools.ByteArrayTo<ushort>(Data, ref Offset);

                XmlElement arrayElement = XmlDoc.CreateElement("0x" + unknown.ToString("X8"));
                arrayElement.SetAttribute("token", "0x" + token.ToString("X4"));
                unknownArray.AppendChild(arrayElement);

                XmlElement element;
                switch (token)
                {
                    case 0x0200:    // skills
                        String str = ReadByteString(arrayElement, "str", false);
                        if (str != null)
                        {
                            Offset++; // need to include \0 as it isn't included in the strLen byte for some reason
                        }

                        break;

                    case 0x0003:    // skills
                    //case 0x0005:
                    //case 0x00C0:
                    //case 0x008D: // found in particle effects
                        ReadShort(arrayElement, "short");
                        break;

                    case 0x0000:    // skills
                    case 0x0309:    // skills
                    case 0x0700:    // skills
                    case 0x0903:    // skills
                    //case 0x0A00:   // particle effects
                        ReadInt32(arrayElement, "int");
                        break;

                    case 0x0100:    // skills
                    //case 0x0600:
                    ////case 0x0500: // found in particle effects
                        ReadFloat(arrayElement, "float");
                        break;

                    case 0x0B01:    // skills
                        ReadInt32(arrayElement, "int");
                        ReadBitField(arrayElement, "bitmask");
                        break;

                    case 0x0308:    // skills
                    case 0x030A:    // skills
                    ////case 0x0106: // found in particle effects
                        ReadInt32(arrayElement, "int1");
                        ReadInt32(arrayElement, "int2");
                        break;

                    case 0x0C02:    // skills
                        ReadInt32(arrayElement, "index");
                        ReadInt32(arrayElement, "int");
                        break;

                    default:
                        MessageBox.Show("Unexpected unknownArray token!\n\ntoken = 0x" + token.ToString("X4"), "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Debug.Assert(false);
                        return false;
                }
            }

            XmlElement dataElement = XmlDoc.CreateElement("DATA");
            mainElement.AppendChild(dataElement);

            if (!ParseDataSegment(dataElement)) return false;

            if (Offset != Data.Length)
            {
                MessageBox.Show("Entire file not parsed!\noffset != data.Length\n\noffset = " + Offset +
                                "\ndata.Length = " + Data.Length, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

        protected Int32 ReadInt32(XmlNode parentNode, String elementName)
        {
            Int32 value = FileTools.ByteArrayTo<Int32>(Data, ref Offset);

            XmlElement element = XmlDoc.CreateElement(elementName);
            element.InnerText = value.ToString();
            element.SetAttribute("asHex", "0x" + value.ToString("X8"));
            parentNode.AppendChild(element);

            return value;
        }

        protected float ReadFloat(XmlNode parentNode, String elementName)
        {
            float value = FileTools.ByteArrayTo<float>(Data, ref Offset);

            XmlElement element = XmlDoc.CreateElement(elementName);
            element.InnerText = value.ToString("F5");
            parentNode.AppendChild(element);

            return value;
        }

        private short ReadShort(XmlNode parentNode, String elementName)
        {
            short value = FileTools.ByteArrayTo<short>(Data, ref Offset);

            XmlElement element = XmlDoc.CreateElement(elementName);
            element.InnerText = value.ToString();
            element.SetAttribute("asHex", "0x" + value.ToString("X4"));
            parentNode.AppendChild(element);

            return value;
        }

        protected UInt32 ReadBitField(XmlNode parentNode, String elementName)
        {
            UInt32 value = FileTools.ByteArrayTo<UInt32>(Data, ref Offset);

            XmlElement element = XmlDoc.CreateElement(elementName);
            element.InnerText = Convert.ToString(value, 2).PadLeft(32, '0');
            element.SetAttribute("asHex", "0x" + value.ToString("X8"));
            parentNode.AppendChild(element);

            return value;
        }

        protected String ReadByteString(XmlNode parentNode, String elementName, bool mustExist)
        {
            String value = null;
            byte strLen = FileTools.ByteArrayTo<byte>(Data, ref Offset);
            if (strLen == 0xFF || strLen == 0x00)
            {
                if (mustExist)
                {
                    value = String.Empty;
                }
            }
            else
            {
                value = FileTools.ByteArrayToStringAnsi(Data, ref Offset, strLen);
            }

            if (value == null) return null;

            XmlElement element = XmlDoc.CreateElement(elementName);
            element.InnerText = value;
            parentNode.AppendChild(element);

            return value;
        }

        protected String ReadZeroString(XmlNode parentNode, String elementName)
        {
            Int32 strLen = FileTools.ByteArrayTo<Int32>(Data, ref Offset);
            Debug.Assert(strLen != 0);

            String value = FileTools.ByteArrayToStringAnsi(Data, Offset);
            Offset += strLen;

            XmlElement element = XmlDoc.CreateElement(elementName);
            element.InnerText = value;
            parentNode.AppendChild(element);

            return value;
        }
    }
}