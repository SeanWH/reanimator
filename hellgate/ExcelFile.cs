﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;
using Hellgate.Excel;
using Revival.Common;
using System.Threading;

namespace Hellgate
{
    public partial class ExcelFile : DataFile
    {
        #region Members
        private byte[] _stringBuffer;
        private byte[] _scriptBuffer;
        private readonly byte[] _myshBuffer;
        private byte[][] _extendedBuffer;
        private StringCollection _secondaryStrings;
        private List<ExcelPropertiesScript> _rowScripts;
        public List<Int32[]> IndexSortArray; // is only available/set when ExcelFile.ExcelDebug = true

        private ExcelHeader _excelFileHeader = new ExcelHeader
        {
            Unknown321 = 0x01,
            Unknown322 = 0x09,
            Unknown161 = 0x0A,
            Unknown162 = 0x0A,
            Unknown163 = 0x00,
            Unknown164 = 0x00,
            Unknown165 = 0x0A,
            Unknown166 = 0x00
        };
        #endregion

        /// <summary>
        /// Creates a new ExcelFile object.
        /// </summary>
        /// <param name="buffer">Byte array of the given Excel file object.</param>
        /// <param name="filePath">Path to file being loaded.</param>
        /// <param name="isTCv4">Set to true if the buffer contains TCv4 data.</param>
        public ExcelFile(byte[] buffer, String filePath, bool isTCv4 = false)
        {
            Thread.CurrentThread.CurrentCulture = Common.EnglishUSCulture;

            IsExcelFile = true;
            FilePath = filePath;
            StringId = _GetStringId(filePath, isTCv4);
            if (StringId == null) throw new Exceptions.DataFileStringIdNotFound(filePath);

            // get the excel type attributes
            Attributes = DataFileMap[StringId];
            if (Attributes.IsEmpty)
            {
                HasIntegrity = true;
                return;
            }

            // parse data
            int peek = FileTools.ByteArrayToInt32(buffer, 0);
            bool isCooked = (peek == Token.cxeh);
            HasIntegrity = isCooked ? ParseData(buffer) : ParseCSV(buffer);

            // we're using hard-coded mysh script table for SKILLS
            if (HasIntegrity && StringId == "SKILLS") _myshBuffer = Skills.Mysh.Data;

            // CSV file
            if (_excelFileHeader.StructureID == 0)
            {
                _excelFileHeader.StructureID = Attributes.StructureId;
            }
        }

        /// <summary>
        /// Converts the Excel file to another Excel type, keeping all but the rows intact.
        /// Used for TCv4 -> SP conversion.
        /// </summary>
        /// <param name="excelFile">An Excel object of type to convert to.</param>
        /// <param name="newRows">The rows to replace the old.</param>
        public void ConvertType(ExcelFile excelFile, IEnumerable<Object> newRows)
        {
            Attributes = excelFile.Attributes;
            _excelFileHeader = excelFile._excelFileHeader;
            Rows = new List<Object>(newRows);
        }

        /// <summary>
        /// Creates a ExcelFile based on the CSV file.
        /// </summary>
        /// <param name="buffer">The CSV file as a byte array.</param>
        /// <returns>True if the buffer parsed okay.</returns>
        public override sealed bool ParseCSV(byte[] buffer)
        {
            // Pre-checks
            if (buffer == null) return false;
            if (buffer.Length < 32) return false;

            // Initialization
            int offset = 0;
            const byte delimiter = (byte)'\t';
            int stringBufferOffset = 0;
            int integerBufferOffset = 1;
            bool isProperties = (StringId == "PROPERTIES" || StringId == "_TCv4_PROPERTIES");

            StringId = FileTools.ByteArrayToStringASCII(FileTools.GetDelimintedByteArray(buffer, ref offset, delimiter), 0);
            StringId = StringId.Replace("\"", "");//in case strings embedded


            // Mutate the buffer into a string array
            int colCount = Attributes.HasExtended ? DataType.GetFields().Count() + 2 : DataType.GetFields().Count() + 1;
            if (isProperties)
            {
                _rowScripts = new List<ExcelPropertiesScript>();
                _scriptBuffer = new byte[1]; // properties is weird - do this just to ensure 100% byte-for-byte accuracy
                colCount++;
            }


            string[][] tableRows = FileTools.CSVToStringArray(buffer, colCount, delimiter);
            if (tableRows == null) return false;

            // Parse the tableRows
            bool failedParsing = false;
            Rows = new List<Object>();
            const BindingFlags bindingFlags = (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            FieldInfo[] fieldInfos = DataType.GetFields(bindingFlags);
            ObjectDelegator objectDelegator = new ObjectDelegator(fieldInfos, "SetValue");
            bool needOutputAttributes = true;
            OutputAttribute[] outputAttributes = new OutputAttribute[fieldInfos.Length];
            for (int row = 0; row < tableRows.Count(); row++)
            {
                int col = 0;
                int fieldCol = -1;
                Object rowInstance = Activator.CreateInstance(DataType);
                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    fieldCol++;

                    if (needOutputAttributes) outputAttributes[fieldCol] = GetExcelOutputAttribute(fieldInfo);
                    OutputAttribute attribute = outputAttributes[fieldCol];

                    // Initialize private fields 
                    if (fieldInfo.IsPrivate)
                    {
                        // create row header object
                        if (fieldInfo.FieldType == typeof(RowHeader))
                        {
                            String headerString = tableRows[row][col++];
                            RowHeader rowHeader = (RowHeader)FileTools.StringToObject(headerString, ",", typeof(RowHeader));
                            objectDelegator[fieldInfo.Name, rowInstance] = rowHeader;
                            continue;
                        }

                        // assign default values
                        MarshalAsAttribute arrayMarshal = null;
                        Array arrayInstance = null;
                        if (fieldInfo.FieldType.BaseType == typeof(Array))
                        {
                            arrayMarshal = (MarshalAsAttribute)fieldInfo.GetCustomAttributes(typeof(MarshalAsAttribute), false).First();
                            arrayInstance = (Array)Activator.CreateInstance(fieldInfo.FieldType, arrayMarshal.SizeConst);
                            objectDelegator[fieldInfo.Name, rowInstance] = arrayInstance;
                        }
                        else if (fieldInfo.FieldType == typeof(String))
                        {
                            objectDelegator[fieldInfo.Name, rowInstance] = String.Empty;
                        }
                        
                        // assign constant non-zero values
                        if (attribute == null || attribute.ConstantValue == null) continue;
                        if (fieldInfo.FieldType.BaseType == typeof(Array))
                        {
                            Debug.Assert(arrayInstance != null, "arrayInstance == null");
                            Debug.Assert(arrayMarshal != null, "arrayMarshal == null");

                            for (int i = 0; i < arrayMarshal.SizeConst; i++)
                            {
                                arrayInstance.SetValue(attribute.ConstantValue, i);
                            }
                        }
                        else
                        {
                            objectDelegator[fieldInfo.Name, rowInstance] = attribute.ConstantValue;
                        }

                        continue;
                    }

                    // Parse public fields
                    // All public fields must be inside the CSV
                    string value = tableRows[row][col++];
                    if (attribute != null)
                    {
                        if (attribute.IsStringOffset)
                        {
                            if (_stringBuffer == null) _stringBuffer = new byte[1024];

                            if (String.IsNullOrEmpty(value))
                            {
                                objectDelegator[fieldInfo.Name, rowInstance] = -1;
                                continue;
                            }

                            objectDelegator[fieldInfo.Name, rowInstance] = stringBufferOffset;
                            FileTools.WriteToBuffer(ref _stringBuffer, ref stringBufferOffset, FileTools.StringToASCIIByteArray(value));
                            stringBufferOffset++; // \0
                            continue;
                        }

                        if (attribute.IsScript)
                        {
                            if (value == "0")
                            {
                                objectDelegator[fieldInfo.Name, rowInstance] = 0;
                                continue;
                            }
                            if (_scriptBuffer == null)
                            {
                                _scriptBuffer = new byte[1024];
                                _scriptBuffer[0] = 0x00;
                            }

                            value = value.Replace("\"", "");
                            string[] splitValue = value.Split(',');
                            int count = splitValue.Length;
                            int[] intValue = new int[count];
                            for (int i = 0; i < count; i++)
                            {
                                intValue[i] = int.Parse(splitValue[i]);
                            }
                            objectDelegator[fieldInfo.Name, rowInstance] = integerBufferOffset;
                            FileTools.WriteToBuffer(ref _scriptBuffer, ref integerBufferOffset, FileTools.IntArrayToByteArray(intValue));
                            continue;
                        }

                        if ((attribute.IsSecondaryString))
                        {
                            if ((_secondaryStrings == null))
                            {
                                _secondaryStrings = new StringCollection();
                            }
                            if ((String.IsNullOrEmpty(value)))
                            {
                                objectDelegator[fieldInfo.Name, rowInstance] = -1;
                                continue;
                            }
                            if (!(_secondaryStrings.Contains(value)))
                            {
                                _secondaryStrings.Add(value);
                            }
                            objectDelegator[fieldInfo.Name, rowInstance] = _secondaryStrings.IndexOf(value);
                            continue;
                        }

                        if ((attribute.IsBitmask))
                        {
                            objectDelegator[fieldInfo.Name, rowInstance] = UInt32.Parse(value);
                            continue;
                        }
                    }

                    try
                    {
                        Object objValue = FileTools.StringToObject(value, fieldInfo.FieldType);
                        objectDelegator[fieldInfo.Name, rowInstance] = objValue;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Critical Parsing Error: " + e);
                        failedParsing = true;
                        break;
                    }

                }
                if (failedParsing) break;
                needOutputAttributes = false;

                // For item types, items, missiles, monsters etc
                // This must be a hex byte delimited array
                if ((Attributes.HasExtended))
                {
                    if ((_extendedBuffer == null))
                    {
                        _extendedBuffer = new byte[tableRows.Count()][];
                    }
                    const char split = ',';
                    string value = tableRows[row][col];
                    string[] stringArray = value.Split(split);
                    byte[] byteArray = new byte[stringArray.Length];
                    for (int i = 0; i < byteArray.Length; i++)
                    {
                        byteArray[i] = Byte.Parse(stringArray[i], NumberStyles.HexNumber);
                    }
                    _extendedBuffer[row] = byteArray;
                }

                // properties has extra Scripts stuffs
                // yea, this is a bit messy, but it's a single table only and mostly done out of curiosity
                if (isProperties)
                {
                    String value = tableRows[row][col];
                    String[] scripts = value.Split('\n');
                    ExcelPropertiesScript excelScript = new ExcelPropertiesScript();
                    if (scripts.Length > 1)
                    {
                        _rowScripts.Add(excelScript);
                    }

                    int i = 0;
                    do
                    {
                        if (scripts.Length == 1) continue;

                        i++;
                        String[] values = scripts[i].Split(',');
                        if (values.Length < 4) continue;


                        // script parameters
                        int typeValuesCount = values.Length - 3;
                        ExcelPropertiesScript.Parameter parameter = new ExcelPropertiesScript.Parameter
                        {
                            Name = values[0],
                            Unknown = UInt32.Parse(values[1]),
                            TypeId = UInt32.Parse(values[2]),
                            TypeValues = new int[typeValuesCount]
                        };

                        for (int j = 0; j < typeValuesCount; j++)
                        {
                            parameter.TypeValues[j] = Int32.Parse(values[3 + j]);
                        }

                        excelScript.Parameters.Add(parameter);

                    } while (i < scripts.Length - 1 - 2); // -2 for: last line is blank, and line before *might* be script values


                    // last line will be script values if it exists
                    if (i < scripts.Length - 2)
                    {
                        String[] values = scripts[++i].Split(',');
                        int[] scriptValues = new int[values.Length];

                        for (int j = 0; j < values.Length; j++)
                        {
                            scriptValues[j] = Int32.Parse(values[j]);
                        }

                        excelScript.ScriptValues = scriptValues.ToByteArray();
                    }
                }

                Rows.Add(rowInstance);
            }

            // resize the integer and string buffers if they were used
            if (_stringBuffer != null) Array.Resize(ref _stringBuffer, stringBufferOffset);
            if (_scriptBuffer != null) Array.Resize(ref _scriptBuffer, integerBufferOffset);

            return true;
        }

        /// <summary>
        /// Creates a ExcelFile based on the serialized data source.
        /// </summary>
        /// <param name="buffer">The serialized excel file as a byte array.</param>
        /// <returns>True if the buffer parsed okay.</returns>
        public override sealed bool ParseData(byte[] buffer)
        {
            if ((buffer == null)) return false;
            if ((buffer.Length == 0)) return false;
            int offset = 0;

            // File Header
            if (!(_CheckToken(buffer, ref offset, Token.cxeh))) return false;
            _excelFileHeader = FileTools.ByteArrayToStructure<ExcelHeader>(buffer, ref offset);
            //ExcelMap = GetTypeMap(StructureId);
            //if ((ExcelMap == null)) return false;

            // Strings Block
            if (!(_CheckToken(buffer, ref offset, Token.cxeh))) return false;
            int stringBufferOffset = FileTools.ByteArrayToInt32(buffer, ref offset);
            if (stringBufferOffset != 0)
            {
                _stringBuffer = new byte[stringBufferOffset];
                Buffer.BlockCopy(buffer, offset, _stringBuffer, 0, stringBufferOffset);
                offset += stringBufferOffset;
            }

            // Dataset Block
            if (!(_CheckToken(buffer, ref offset, Token.cxeh))) return false;
            int rowCount = FileTools.ByteArrayToInt32(buffer, ref offset);
            Rows = new List<Object>(rowCount);
            for (int i = 0; i < rowCount; i++)
            {
                Rows.Add(FileTools.ByteArrayToStructure(buffer, DataType, ref offset));
            }

            // Primary Indice Block
            if (!_CheckToken(buffer, ref offset, Token.cxeh)) return false;
            if (Attributes.HasExtended) // items, objects, missles, players
            {
                _extendedBuffer = new byte[Count][];
                for (int i = 0; i < Count; i++)
                {
                    offset += sizeof(int); // Skip the indice

                    int size = FileTools.ByteArrayToInt32(buffer, ref offset);
                    _extendedBuffer[i] = new byte[size];

                    Buffer.BlockCopy(buffer, offset, _extendedBuffer[i], 0, size);
                    offset += size;
                }
            }
            else
            {
                offset += (Count * sizeof(int)); // do not allocate this array
            }

            // Secondary String Block
            if (!(_CheckToken(buffer, offset, Token.cxeh)))
            {
                int stringCount = FileTools.ByteArrayToInt32(buffer, ref offset);
                if (stringCount != 0) _secondaryStrings = new StringCollection();
                for (int i = 0; i < stringCount; i++)
                {
                    int charCount = FileTools.ByteArrayToInt32(buffer, ref offset);
                    _secondaryStrings.Add(FileTools.ByteArrayToStringASCII(buffer, offset));
                    offset += charCount;
                }
            }

            // Sorted Indices
            for (int i = 0; i < 4; i++)
            {
                if (!(_CheckToken(buffer, ref offset, Token.cxeh))) return false;
                int count = FileTools.ByteArrayToInt32(buffer, ref offset);

                if (EnableDebug)
                {
                    if (IndexSortArray == null) IndexSortArray = new List<Int32[]>();
                    IndexSortArray.Add(FileTools.ByteArrayToInt32Array(buffer, ref offset, count));
                }
                else
                {
                    offset += (count * sizeof(int)); // do not allocate
                }
            }

            // rcsh, tysh, mysh, dneh blocks
            if (!_CheckToken(buffer, ref offset, Token.cxeh)) return false;
            if (!_CheckToken(buffer, offset, 0))
            {
                if (!_CheckToken(buffer, ref offset, Token.rcsh)) return false;
                if (!_CheckToken(buffer, ref offset, Token.RcshValue)) return false;

                if (!_CheckToken(buffer, ref offset, Token.tysh)) return false;
                if (!_CheckToken(buffer, ref offset, Token.TyshValue)) return false;

                if (Attributes.HasScriptTable && !_ParsePropertiesScriptTable(buffer, ref offset)) return false;

                if (!_CheckToken(buffer, ref offset, Token.dneh)) return false;
                if (!_CheckToken(buffer, ref offset, Token.DnehValue)) return false;
            }


            // Integer Block
            if ((_CheckToken(buffer, ref offset, Token.cxeh)))
            {
                int integerBufferOffset = FileTools.ByteArrayToInt32(buffer, ref offset);
                if (integerBufferOffset != 0)
                {
                    _scriptBuffer = new byte[integerBufferOffset];
                    Buffer.BlockCopy(buffer, offset, _scriptBuffer, 0, integerBufferOffset);
                    offset += integerBufferOffset;
                }
            }


            // final data block; why is this not allocated? - no need to save? automatically generated when cooked?
            // -> automatically generated via CreateIndexBitRelations() method
            if (!_CheckToken(buffer, offset, 0))
            {
                if (!_CheckToken(buffer, ref offset, Token.cxeh)) return false;

                int byteCount = FileTools.ByteArrayToInt32(buffer, ref offset);
                int blockCount = FileTools.ByteArrayToInt32(buffer, ref offset);
                if (byteCount != 0)
                {
                    byteCount = byteCount << 2;
                    offset += ((byteCount * blockCount)); // do not allocate
                }
            }

            return offset == buffer.Length;
        }

        /// <summary>
        /// Creates a ExcelFile based on the DataTable data.
        /// </summary>
        /// <param name="dataTable">The DataTable to read the data from.</param>
        /// <returns>True if the DataTable parsed okay.</returns>
        public override bool ParseDataTable(DataTable dataTable)
        {
            if (dataTable == null) throw new ArgumentNullException();

            byte[] newStringBuffer = null;
            int newStringBufferOffset = 0;
            byte[] newIntegerBuffer = null;
            int newIntegerBufferOffset = 1;
            byte[][] newExtendedBuffer = null;
            StringCollection newSecondaryStrings = null;
            List<object> newTable = new List<object>();

            bool failedParsing = false;
            const BindingFlags bindingFlags = (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            FieldInfo[] dataFields = DataType.GetFields(bindingFlags);

            for (int row = 0; row < dataTable.Rows.Count; row++)
            {
                int col = 1; // Skip the indice column (column 0)
                Object rowInstance = Activator.CreateInstance(DataType);
                foreach (FieldInfo fieldInfo in dataFields)
                {
                    // Initialize private fields 
                    if ((fieldInfo.IsPrivate))
                    {
                        if ((fieldInfo.FieldType == typeof(RowHeader)))
                        {
                            string headerString = (string)dataTable.Rows[row][col++];
                            RowHeader tableHeader = (RowHeader)FileTools.StringToObject(headerString, ",", typeof(RowHeader));
                            fieldInfo.SetValue(rowInstance, tableHeader);
                            continue;
                        }
                        if ((fieldInfo.FieldType.BaseType == typeof(Array)))
                        {
                            MarshalAsAttribute marshal = (MarshalAsAttribute)fieldInfo.GetCustomAttributes(typeof(MarshalAsAttribute), false).First();
                            Array arrayInstance = (Array)Activator.CreateInstance(fieldInfo.FieldType, marshal.SizeConst);
                            fieldInfo.SetValue(rowInstance, arrayInstance);
                            continue;
                        }
                        if ((fieldInfo.FieldType == typeof(String)))
                        {
                            fieldInfo.SetValue(rowInstance, String.Empty);
                            continue;
                        }
                        continue;
                    }

                    // Public fields -> these are inside the datatable
                    object value = dataTable.Rows[row][col++];
                    OutputAttribute attribute = GetExcelOutputAttribute(fieldInfo);
                    if (attribute != null)
                    {
                        if (attribute.IsStringOffset)
                        {
                            if (newStringBuffer == null)
                            {
                                newStringBuffer = new byte[1024];
                            }

                            string strValue = value as string;
                            if (strValue == null) return false;

                            if (String.IsNullOrEmpty(strValue))
                            {
                                fieldInfo.SetValue(rowInstance, -1);
                                continue;
                            }

                            fieldInfo.SetValue(rowInstance, newStringBufferOffset);
                            FileTools.WriteToBuffer(ref newStringBuffer, ref newStringBufferOffset, FileTools.StringToASCIIByteArray(strValue));
                            FileTools.WriteToBuffer(ref newStringBuffer, ref newStringBufferOffset, (byte)0x00);
                            continue;
                        }

                        if ((attribute.IsScript))
                        {
                            if ((newIntegerBuffer == null))
                            {
                                newIntegerBuffer = new byte[1024];
                                newIntegerBuffer[0] = 0x00;
                            }

                            string strValue = value as string;
                            if (strValue == null) return false;

                            if (strValue == "0" || String.IsNullOrEmpty(strValue))
                            {
                                fieldInfo.SetValue(rowInstance, 0);
                                continue;
                            }

                            strValue = strValue.Replace("\"", "");
                            string[] splitValue = strValue.Split(',');
                            int count = splitValue.Length;
                            int[] intValue = new int[count];
                            for (int i = 0; i < count; i++)
                            {
                                intValue[i] = int.Parse(splitValue[i]);
                            }
                            fieldInfo.SetValue(rowInstance, newIntegerBufferOffset);
                            FileTools.WriteToBuffer(ref newIntegerBuffer, ref newIntegerBufferOffset, FileTools.IntArrayToByteArray(intValue));
                            continue;
                        }

                        if ((attribute.IsSecondaryString))
                        {
                            if (newSecondaryStrings == null)
                            {
                                newSecondaryStrings = new StringCollection();
                            }

                            string strValue = value as string;
                            if (strValue == null) return false;

                            if (String.IsNullOrEmpty(strValue))
                            {
                                fieldInfo.SetValue(rowInstance, -1);
                                continue;
                            }
                            if (newSecondaryStrings.Contains(strValue) == false)
                            {
                                newSecondaryStrings.Add(strValue);
                            }
                            fieldInfo.SetValue(rowInstance, newSecondaryStrings.IndexOf(strValue));
                            continue;
                        }

                        if (attribute.IsStringIndex || attribute.IsTableIndex)
                        {
                            fieldInfo.SetValue(rowInstance, value);
                            col++; // Skip lookup
                            continue;
                        }
                    }

                    try
                    {
                        fieldInfo.SetValue(rowInstance, value);
                    }
                    catch (Exception e)
                    {
                        ExceptionLogger.LogException(e);
                        Console.WriteLine("Critical Parsing Error: " + e.Message);
                        failedParsing = true;
                        break;
                    }

                }
                if (failedParsing) break;

                // For item types, items, missiles, monsters etc
                // This must be a hex byte delimited array
                if ((Attributes.HasExtended))
                {
                    if (newExtendedBuffer == null)
                    {
                        newExtendedBuffer = new byte[dataTable.Rows.Count][];
                    }
                    const char split = ',';
                    string value = dataTable.Rows[row][col] as string;
                    if (String.IsNullOrEmpty(value))
                    {
                        Console.WriteLine("Error parsing Extended property string.");
                        return false;
                    }
                    string[] stringArray = value.Split(split);
                    byte[] byteArray = new byte[stringArray.Length];
                    for (int i = 0; i < byteArray.Length; i++)
                    {
                        byteArray[i] = Byte.Parse(stringArray[i]);
                    }
                    newExtendedBuffer[row] = byteArray;
                }

                newTable.Add(rowInstance);
            }

            // Parsing Complete, assign new references. These arn't assigned before now incase of a parsing error.
            Rows = newTable;
            _stringBuffer = newStringBuffer;
            _scriptBuffer = newIntegerBuffer;
            _extendedBuffer = newExtendedBuffer;
            _secondaryStrings = newSecondaryStrings;

            return true;
        }

        /// <summary>
        /// Converts the ExcelFile to a byte array.
        /// </summary>
        /// <returns>The serialized ExcelFile.</returns>
        public override byte[] ToByteArray()
        {
            byte[] buffer = new byte[1024];
            int offset = 0;


            // The Excel File header
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
            FileTools.WriteToBuffer(ref buffer, ref offset, _excelFileHeader);


            // strings Block
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
            if (_stringBuffer != null && _stringBuffer.Length > 1)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, _stringBuffer.Length);
                FileTools.WriteToBuffer(ref buffer, ref offset, _stringBuffer);
            }
            else
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, 0);
            }


            // Dataset Block
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
            FileTools.WriteToBuffer(ref buffer, ref offset, Rows.Count);
            foreach (Object row in Rows)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, row);
            }


            // primary index
            FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
            for (int i = 0; i < Rows.Count; i++)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, i);
                if (!Attributes.HasExtended) continue;

                FileTools.WriteToBuffer(ref buffer, ref offset, _extendedBuffer[i].Length);
                FileTools.WriteToBuffer(ref buffer, ref offset, _extendedBuffer[i]);
            }


            // Secondary Strings
            if (_secondaryStrings != null)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, _secondaryStrings.Count);
                foreach (string str in _secondaryStrings)
                {
                    FileTools.WriteToBuffer(ref buffer, ref offset, str.Length + 1); // +1 for \0
                    FileTools.WriteToBuffer(ref buffer, ref offset, FileTools.StringToASCIIByteArray(str));
                    offset++; // \0
                }
            }


            // Generate custom sorts
            IEnumerable<int[]> customSorts = _GenerateSortedIndexArrays();
            foreach (int[] intArray in customSorts)
            {
                if (intArray != null)
                {
                    FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
                    FileTools.WriteToBuffer(ref buffer, ref offset, intArray.Length);
                    FileTools.WriteToBuffer(ref buffer, ref offset, FileTools.IntArrayToByteArray(intArray));
                }
                else
                {
                    FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
                    FileTools.WriteToBuffer(ref buffer, ref offset, 0);
                }
            }

            // rcsh, tysh, mysh, dneh
            // This section exists when there is a string or integer block or a mysh table
            if (_scriptBuffer != null || Attributes.HasScriptTable)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.rcsh);
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.RcshValue);
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.tysh);
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.TyshValue);
                if (Attributes.HasScriptTable)
                {
                    if (_myshBuffer != null)
                    {
                        FileTools.WriteToBuffer(ref buffer, ref offset, Token.mysh);
                        FileTools.WriteToBuffer(ref buffer, ref offset, _myshBuffer);
                    }
                    else if (_rowScripts != null)
                    {
                        _PropertiesScriptToByteArray(ref buffer, ref offset);
                    }
                }
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.dneh);
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.DnehValue);
            }

            // Append the integer array.
            if (_scriptBuffer != null && _scriptBuffer.Length > 0)
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
                FileTools.WriteToBuffer(ref buffer, ref offset, _scriptBuffer.Length);
                FileTools.WriteToBuffer(ref buffer, ref offset, _scriptBuffer);
            }
            else
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
                FileTools.WriteToBuffer(ref buffer, ref offset, 0);
            }


            // row-row index bit relations - generated from isA0, isA1, isA2 etc
            // only applicable on the UNITTYPES and STATES tables
            if (Attributes.HasIndexBitRelations)
            {
                int blockSize = (Count >> 5) + 1; // need 1 bit for every row; 32 bits per int - blockSize = no. of Int's
                UInt32[,] indexBitRelations = _CreateIndexBitRelations();
                byte[] relationsData = new byte[Count * blockSize * sizeof(UInt32)];
                Buffer.BlockCopy(indexBitRelations, 0, relationsData, 0, relationsData.Length);

                FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
                FileTools.WriteToBuffer(ref buffer, ref offset, blockSize);
                FileTools.WriteToBuffer(ref buffer, ref offset, Count);
                FileTools.WriteToBuffer(ref buffer, ref offset, relationsData);
            }
            else
            {
                FileTools.WriteToBuffer(ref buffer, ref offset, Token.cxeh);
                FileTools.WriteToBuffer(ref buffer, ref offset, 0);
                FileTools.WriteToBuffer(ref buffer, ref offset, 0);
            }

            // Resize
            Array.Resize(ref buffer, offset);
            return buffer;
        }

        /// <summary>
        /// Converts the ExcelFile to a CSV
        /// </summary>
        /// <returns>The CSV as a byte array.</returns>
        public override byte[] ExportCSV()
        {
            ObjectDelegator objectDelegator = new ObjectDelegator(Attributes.RowType, "GetValue");

            FieldInfo[] dataTypeFields = DataType.GetFields();
            int noCols = dataTypeFields.Count();
            int noRows = Count + 1; // +1 for column headers
            const byte delimiter = (byte)'\t';

            byte[] csvBuffer = new byte[1024];
            int csvOffset = 0;
            int row = 0;
            int scriptRow = 0;

            // Table Header - put stringID in this field
            FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray(_GetStringId(FilePath)));
            FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, delimiter);
            // Public Field Headers
            foreach (FieldInfo fieldInfo in dataTypeFields)
            {
                FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray(fieldInfo.Name));
                FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, delimiter);
            }
            // Add extra column for extended properties
            if ((Attributes.HasExtended))
            {
                FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray("ExtendedProps"));
                FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, delimiter);
            }
            // if properties we have the scripts to export as well
            bool isProperties = (StringId == "PROPERTIES" || StringId == "_TCv4_PROPERTIES");
            if (isProperties)
            {
                FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray("Script"));
                FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, delimiter);
            }
            // End of line
            FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray(Environment.NewLine));


            // Parse each row, resolve buffers if needed
            bool needOutputAttributes = true;
            OutputAttribute[] outputAttributes = new OutputAttribute[noCols];
            foreach (Object rowObject in Rows)
            {
                // Write Table Header
                FieldInfo headerField = DataType.GetField("header", BindingFlags.Instance | BindingFlags.NonPublic);
                RowHeader tableHeader = (RowHeader)headerField.GetValue(rowObject);
                string tableHeaderString = FileTools.ObjectToStringGeneric(tableHeader, ",");
                FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray(tableHeaderString));

                int col = -1;
                foreach (FieldInfo fieldInfo in dataTypeFields)
                {
                    FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, delimiter);

                    col++;
                    if (needOutputAttributes) outputAttributes[col] = GetExcelOutputAttribute(fieldInfo);
                    OutputAttribute attribute = outputAttributes[col];

                    //if (col == 22 && row == 29)
                    //{
                    //    int bp = 0;
                    //}

                    if (attribute != null)
                    {
                        if ((attribute.IsStringOffset))
                        {
                            int offset = (int)objectDelegator[col](rowObject);
                            if (offset != -1)
                            {
                                byte[] stringBytes = ReadStringTableAsBytes(offset);
                                if (stringBytes == null) continue;

                                FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, stringBytes);
                            }
                            continue;
                        }
                        if ((attribute.IsScript))
                        {
                            int offset = (int)objectDelegator[col](rowObject);
                            if ((offset == 0))
                            {
                                FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray("0"));
                                continue;
                            }
                            int[] buffer = ReadScriptTable(offset);

                            //Debug.Write(String.Format("row: {0}, col: {1}: ", row, col));
                            //String excelScript = ExcelScript.Decompile(_integerBuffer, offset);
                            //Debug.WriteLine(excelScript);

                            FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray(FileTools.ArrayToStringGeneric(buffer, ",")));
                            continue;
                        }
                        if ((attribute.IsSecondaryString))
                        {
                            int index = (int)objectDelegator[col](rowObject);
                            if (index != -1)
                            {
                                FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray(_secondaryStrings[index]));
                            }
                            continue;
                        }
                        if ((attribute.IsBitmask))
                        {
                            uint uintValue = (uint)objectDelegator[col](rowObject);
                            string stringValue = uintValue.ToString();
                            FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray(stringValue));
                            continue;
                        }
                    }
                    FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray(objectDelegator[col](rowObject).ToString()));
                }
                needOutputAttributes = false;

                // Extended Buffer if applies
                if (Attributes.HasExtended)
                {
                    FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, delimiter);
                    FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray(FileTools.ByteArrayToDelimitedASCIIString(_extendedBuffer[row], ',', typeof(byte))));
                }

                // properties scripts
                if (isProperties && scriptRow < _rowScripts.Count)
                {
                    if (tableHeader.Unknown1 != 2 || scriptRow == _rowScripts.Count - 1) // need 1 extra row for some reason
                    {
                        FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, delimiter);
                        ExcelPropertiesScript excelScript = _rowScripts[scriptRow++];
                        String output = String.Empty;
                        foreach (ExcelPropertiesScript.Parameter paramater in excelScript.Parameters)
                        {
                            output += String.Format("\n{0},{1},{2},{3}", paramater.Name, paramater.Unknown,
                                                    paramater.TypeId, paramater.TypeValues.ToString(","));
                        }
                        FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray(output));

                        if (excelScript.ScriptValues != null)
                        {
                            int offset = 0;
                            String scriptValues = "\n" +
                                                  FileTools.ByteArrayToInt32Array(excelScript.ScriptValues, ref offset,
                                                                                  excelScript.ScriptValues.Length / 4).ToString(",") + "\n";
                            FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray(scriptValues));
                        }
                    }
                    else
                    {
                        FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray(Environment.NewLine));
                    }
                }

                row++;
                if (row != noRows - 1)
                {
                    FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, delimiter);
                    FileTools.WriteToBuffer(ref csvBuffer, ref csvOffset, FileTools.StringToASCIIByteArray(Environment.NewLine));
                }
            }

            Array.Resize(ref csvBuffer, csvOffset);
            return csvBuffer;
        }

        /// <summary>
        /// Quick and dirty function to export mysh scripts as xml.
        /// Only applicable to PROPERTIES and SKILLS tables.
        /// </summary>
        /// <returns>Byte array of XML document for writing, or null on error.</returns>
        public byte[] ExportScriptTable()
        {
            if (_rowScripts == null || _rowScripts.Count == 0) return null;

            // this functions is quick and dirty - ignore me
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement mainElement = xmlDocument.CreateElement("ExcelScript");
            xmlDocument.AppendChild(mainElement);

            foreach (ExcelPropertiesScript excelScript in _rowScripts)
            {
                XmlElement scriptElement = xmlDocument.CreateElement("Script");
                mainElement.AppendChild(scriptElement);

                foreach (ExcelPropertiesScript.Parameter paramater in excelScript.Parameters)
                {
                    XmlElement paramElement = xmlDocument.CreateElement("Parameter");
                    scriptElement.AppendChild(paramElement);

                    XmlElement paramName = xmlDocument.CreateElement("Name");
                    paramName.InnerText = paramater.Name;
                    paramElement.AppendChild(paramName);

                    XmlElement paramUnknown = xmlDocument.CreateElement("Unknown");
                    paramUnknown.InnerText = paramater.Unknown.ToString("X8");
                    paramElement.AppendChild(paramUnknown);

                    XmlElement paramTypeId = xmlDocument.CreateElement("TypeId");
                    paramTypeId.InnerText = paramater.TypeId.ToString();
                    paramElement.AppendChild(paramTypeId);

                    XmlElement paramTypeValues = xmlDocument.CreateElement("TypeValues");
                    paramElement.AppendChild(paramTypeValues);

                    // temp
                    String text = String.Empty;
                    for (int i = 0; i < paramater.TypeValues.Length; i++)
                    {
                        text += paramater.TypeValues[i].ToString();
                        if (i < paramater.TypeValues.Length - 1) text += ",";
                    }
                    paramTypeValues.InnerText = text;
                }

                XmlElement scriptValues = xmlDocument.CreateElement("Values");

                if (excelScript.ScriptValues != null)
                {
                    int intCount = excelScript.ScriptValues.Length / 4;
                    String text = String.Empty;
                    int offset = 0;
                    Int32[] intArray = FileTools.ByteArrayToInt32Array(excelScript.ScriptValues, ref offset, intCount);
                    for (int i = 0; i < intArray.Length; i++)
                    {
                        // testing if some of those huge numbers are actually two shorts...
                        //if (Math.Abs(intArray[i]) > 10000)
                        //{
                        //    short s1 = (short)(intArray[i] >> 16);
                        //    short s2 = (short)(intArray[i] & 0xFFFF);
                        //    text += s1 + "," + s2;
                        //    if (i < intArray.Length - 1) text += ",";
                        //}
                        //else
                        //{
                        text += intArray[i].ToString();
                        if (i < intArray.Length - 1) text += ",";
                        //}

                    }
                    scriptValues.InnerText = text;
                }
                scriptElement.AppendChild(scriptValues);
            }

            // being lazy and want as byte array for consistancy
            MemoryStream ms = new MemoryStream();
            xmlDocument.Save(ms);
            byte[] arr = ms.ToArray();
            ms.Close();

            return arr;
        }

        /// <summary>
        /// Converts a TestCenter ExcelFile into a SinglePlayer version.
        /// </summary>
        /// <param name="spDataTable">The source SinglePlayer DataTable.</param>
        /// <param name="tcDataTable">The source TestCenter DataTable.</param>
        /// <returns>The converted DataTable.</returns>
        public static DataTable ConvertToSinglePlayerVersion(DataTable spDataTable, DataTable tcDataTable)
        {
            spDataTable.Rows.Clear();

            foreach (DataRow tcRow in tcDataTable.Rows)
            {
                DataRow convertedRow = spDataTable.NewRow();
                foreach (DataColumn column in spDataTable.Columns)
                {
                    string columnName = column.ColumnName;

                    if (!tcDataTable.Columns.Contains(columnName)) continue;
                    if (column.DataType == tcDataTable.Columns[columnName].DataType)
                    {
                        convertedRow[columnName] = tcRow[columnName];
                        continue;
                    }
                    if (column.DataType.BaseType != typeof(Enum)) continue;

                    Type spBitMask = column.DataType;
                    Type tcBitMask = tcDataTable.Columns[columnName].DataType;
                    uint currentMask = (uint)tcRow[columnName];
                    uint convertedMask = 0;

                    for (int i = 0; i < 32; i++)
                    {
                        uint testBit = (uint)1 << i;
                        if ((currentMask & testBit) == 0)
                        {
                            continue;
                        }
                        string bitString = Enum.GetName(tcBitMask, testBit);
                        if (bitString == null)
                        {
                            continue;
                        }
                        if (Enum.IsDefined(spBitMask, bitString))
                        {
                            convertedMask += (uint)Enum.Parse(spBitMask, bitString);
                        }
                    }
                    convertedRow[columnName] = convertedMask;
                    continue;
                }

                spDataTable.Rows.Add(convertedRow);
            }

            return spDataTable;
        }
    }
}
