using System;
using System.IO;

namespace Base
{
    public static class StringExtensions
    {
        public static bool IsSubPathOf(this string path, string baseDirPath)
        {
            string normalizedPath = Path.GetFullPath(path.Replace('/', '\\')
                .WithEnding("\\"));

            string normalizedBaseDirPath = Path.GetFullPath(baseDirPath.Replace('/', '\\')
                .WithEnding("\\"));

            return normalizedPath.StartsWith(normalizedBaseDirPath, StringComparison.OrdinalIgnoreCase);
        }

        public static string WithEnding(this string str, string ending)
        {
            if (str == null)
                return ending;

            string result = str;

            for (int i = 0; i <= ending.Length; i++)
            {
                string tmp = result + ending.Right(i);
                if (tmp.EndsWith(ending))
                    return tmp;
            }

            return result;
        }

        public static string Right(this string value, int length)
        {
            return (length < value.Length) ? value.Substring(value.Length - length) : value;
        }
    }
}
