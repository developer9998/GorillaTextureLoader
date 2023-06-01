using BepInEx.Logging;
using ComputerInterface;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TextureLoader
{
    internal static class Utilities
    {
        internal static string AddonsDirectoryPath { get { return AssemblyDirectoryPath + "/addons"; } }
        internal static string AssemblyDirectoryPath { get { return Path.GetFullPath(Directory.GetParent(typeof(Main).Assembly.Location).FullName); } }
        internal static string[] GetAllFileNamesInDirectory(string DirectoryPath, string SearchPattern = "")
        {
            if (!Directory.Exists(DirectoryPath)) return new string[0];
            List<string> files = Directory.GetFiles(DirectoryPath, SearchPattern).ToList();
            for (int i = 0; i < files.Count; i++)
                files[i] = Path.GetFileNameWithoutExtension(files[i]);
            return files.ToArray();
        }
        internal static string ToColor(this string ThisString, string color = "green")
        {
            string HEX = Colors[color];
            return $"<color=#{HEX}>{ThisString}</color>";
        }

        private static Dictionary<string, string> Colors = new Dictionary<string, string>()
        {
            { "green", "09ff00" },
            { "red", "ff0800" },
            { "gray", "ffffff50" }
        };
        internal static StringBuilder AppendHeader(this StringBuilder stringBuilder, string TitleField, string AuthorField = "", int Offset = 2)
        {
            stringBuilder
                .BeginCenter()
                .MakeBar('=', ComputerInterface.ViewLib.ComputerView.SCREEN_WIDTH, 0)
                .AppendLine(TitleField);
            if (AuthorField != "") stringBuilder.AppendLine("<color=#ffffff50>" + AuthorField + "</color>");
            stringBuilder
                .MakeBar('=', ComputerInterface.ViewLib.ComputerView.SCREEN_WIDTH, 0)
                .AppendLines(Offset)
                .EndAlign();
            return stringBuilder;
        }
        /*/// <returns>Bool from playerprefs, idk what it does if the key doesn't exist so...</returns>
        internal static bool GetBool(string Key, bool? NewValue = null)
        {
            if (NewValue.HasValue) PlayerPrefs.SetInt(Key, NewValue.Value ? 1 : 0);
            return PlayerPrefs.GetInt(Key) == 1;
        }*/
        internal static void Log(this object obj, LogLevel logLevel = LogLevel.Info)
        {
#if DEBUG
            Main.manualLogSource.Log(logLevel, obj);
#endif
        }
    }
}
