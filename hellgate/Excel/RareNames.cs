﻿using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class RareNames
    {
        RowHeader header;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(IsBool = true, SortColumnOrder = 1, SecondarySortColumn = "code")]
        public Int32 suffix;//bool
        public Int32 level;
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)] // changed to int.. problem reading last character
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        public Int32 types1;
        public Int32 types2;
        public Int32 types3;
        public Int32 types4;
        public Int32 types5;
        public Int32 types6;
        [ExcelOutput(IsScript = true)]
        public Int32 cond;//intptr
    }
}