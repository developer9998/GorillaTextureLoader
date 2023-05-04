using ComputerInterface;
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
        internal static StringBuilder ToStringBuilder(this string ThisString)
        {
            return new StringBuilder().Append(ThisString);
        }

        internal static object Log(this object obj, LogType logType = LogType.Info)
        {
#if DEBUG
            string Message = "**[TEXTURE LOADER]** : " + obj;
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
