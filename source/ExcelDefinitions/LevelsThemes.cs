﻿using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class LevelsThemesRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public Int32[] undefined;
        public Int32 dontDisplayInEditor;//bool
        public Int32 highLander;//bool //is this supposed to be "there can be only one" highlander?

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string environment;
        public Int32 envPriority;
        public Int32 allowedStyles0;
        public Int32 allowedStyles1;
        public Int32 allowedStyles2;
        public Int32 allowedStyles3;
        public Int32 allowedStyles4;
        public Int32 allowedStyles5;
        public Int32 allowedStyles6;
        public Int32 allowedStyles7;
        public Int32 allowedStyles8;
        public Int32 allowedStyles9;
        public Int32 allowedStyles10;
        public Int32 allowedStyles11;
        public Int32 allowedStyles12;
        public Int32 allowedStyles13;
        public Int32 allowedStyles14;
        public Int32 allowedStyles15;
        public Int32 globalThemeRequired;//idx

    }
}