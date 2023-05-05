using ComputerInterface;
using ComputerInterface.ViewLib;
using System.Collections.Generic;
using System.Text;

namespace TextureLoader.Core
{
    internal static class Extensions
    {
        internal static StringBuilder AddHeader(this StringBuilder stringBuilder, int WIDTH, string Title, string Author = "", int Offset = 2)
        {
            stringBuilder.Repeat("=", WIDTH).AppendLine();
            stringBuilder.BeginCenter().Append(Title).AppendLine();
            if (Author != "") stringBuilder.AppendClr(Author, "ffffff50").EndAlign().AppendLine();
            stringBuilder.Repeat("=", WIDTH).AppendLines(Offset);
            return stringBuilder;
        }
        internal static StringBuilder AppendFooter(this StringBuilder stringBuilder, UIPageHandler pageHandler, int Offset = 3)
        {
            return stringBuilder
                .AppendLines(Offset)
                .BeginAlign("right")
                .AppendLine($"← {pageHandler.CurrentPage + 1}/{pageHandler.MaxPage + 1} →".ToColor("gray"))
                .EndAlign();
        }
        internal static string[] ValueToArray(this Dictionary<string, string> dictionary)
        {
            string[] array = new string[dictionary.Count];
            int i = 0;
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                array[i] = pair.Value;
                i++;
            }
            return array;
        }
        internal static StringBuilder ToStringBuilder(this string ThisString)
        {
            return new StringBuilder().Append(ThisString);
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

        internal static object Log(this object obj, LogType logType = LogType.Info)
        {
#if DEBUG
            string Message = "**[TEXTURE LOADER]** : " + obj;
            //string path = Path.GetDirectoryName(typeof(Main).Assembly.Location) + "/TextureLoader.log";

            switch (logType)
            {
                case LogType.Info:
                    UnityEngine.Debug.Log(Message);
                    break;
                case LogType.Warning:
                    UnityEngine.Debug.LogWarning(Message);
                    break;
                case LogType.Error:
                    UnityEngine.Debug.LogError(Message);
                    break;
            }
#endif
            return obj;
        }
    }

    internal enum LogType
    {
        Info,
        Warning,
        Error
    }
}
