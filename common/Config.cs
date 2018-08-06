using System;
using Microsoft.Win32;

namespace Revival.Common
{
    public abstract class Config
    {
        private const string Key = @"SOFTWARE\Reanimator";
        private static readonly RegistryKey _rootKey = Registry.CurrentUser.CreateSubKey(Key);
        private static readonly RegistryKey _configkey = _rootKey.CreateSubKey("config");

        public static string HglDir
        {
            get => _GetValue("HglDir", @"C:\Program Files\Flagship Studios\Hellgate London");
            set => _SetValue("HglDir", value);
        }

        public static string HglDataDir
        {
            get => _GetValue("HglDir", @"C:\Program Files\Flagship Studios\Hellgate London\data");
            set => _SetValue("HglDataDir", value);
        }

        public static string SaveDir
        {
            get => _GetValue("SaveDir",
                $@"C:\Users\{Environment.UserName}\Documents\My Games\Hellgate\Save\Singleplayer");
            set => _SetValue("SaveDir", value);
        }

        public static string BackupDir
        {
            get => _GetValue("BackupDir",
                $@"C:\Users\{Environment.UserName}\Documents\My Games\Hellgate\Save\Singleplayer\Backup");
            set => _SetValue("BackupDir", value);
        }

        public static string ScriptDir
        {
            get => _GetValue("ScriptDir", @"C:\Program Files\Flagship Studios\Hellgate London\Reanimator\Scripts");
            set => _SetValue("ScriptDir", value);
        }

        public static string GameClientPath
        {
            get => _GetValue("GameClientPath",
                @"C:\Program Files\Flagship Studios\Hellgate London\SP_x32\hellgate_sp_dx9_x32.exe");
            set => _SetValue("GameClientPath", value);
        }

        public static int ClientHeight
        {
            get => _GetValue("ClientHeight", 500);
            set => _SetValue("ClientHeight", value);
        }

        public static int ClientWidth
        {
            get => _GetValue("ClientWidth", 700);
            set => _SetValue("ClientWidth", value);
        }

        public static string IntPtrCast
        {
            get => _GetValue("IntPtrCast", "hex");
            set => _SetValue("IntPtrCast", value);
        }

        public static bool GenerateRelations
        {
            get => _GetValue("GenerateRelations", true);
            set => _SetValue("GenerateRelations", value);
        }

        public static bool LoadTCv4DataFiles
        {
            get => _GetValue("LoadTCv4DataFiles", false);
            set => _SetValue("LoadTCv4DataFiles", value);
        }

        public static string LastDirectory
        {
            get => _GetValue("LastDirectory", "");
            set => _SetValue("LastDirectory", value);
        }

        public static string TxtEditor
        {
            get => _GetValue("TxtEditor", "notepad.exe");
            set => _SetValue("TxtEditor", value);
        }

        public static string XmlEditor
        {
            get => _GetValue("XmlEditor", "notepad.exe");
            set => _SetValue("XmlEditor", value);
        }

        public static string CsvEditor
        {
            get => _GetValue("CsvEditor", "notepad.exe");
            set => _SetValue("CsvEditor", value);
        }

        public static string StringsLanguage
        {
            get => _GetValue("StringsLanguage", "english");
            set => _SetValue("StringsLanguage", value);
        }

        public static bool ShowFileExplorer
        {
            get => _GetValue("ShowFileExplorer", false);
            set => _SetValue("ShowFileExplorer", value);
        }

        private static T _GetValue<T>(string name, T defaultValue)
        {
            if (typeof(T) != typeof(bool))
            {
                return (T) _configkey.GetValue(name, defaultValue);
            }

            object ret;
            if ((bool) (object) defaultValue)
            {
                ret = _configkey.GetValue(name, 1);
            }
            else
            {
                ret = _configkey.GetValue(name, 0);
            }

            return (T) (object) ((int) ret != 0);
        }

        private static void _SetValue(string name, object value)
        {
            switch (value)
            {
                case string _:
                    _configkey.SetValue(name, value, RegistryValueKind.String);
                    break;
                case int _:
                case short _:
                    _configkey.SetValue(name, value, RegistryValueKind.DWord);
                    break;
                case long _:
                    _configkey.SetValue(name, value, RegistryValueKind.QWord);
                    break;
                default:
                    if (value.GetType() == typeof(string[]))
                    {
                        _configkey.SetValue(name, value, RegistryValueKind.MultiString);
                    }
                    else if (value is bool b)
                    {
                        _SetValue(name, b ? 1 : 0);
                    }

                    break;
            }

            _configkey.Flush();
        }
    }
}