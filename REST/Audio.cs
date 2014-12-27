using System;
using System.Net;

namespace VKAUDIO
{
    public class Audio
    {
        public bool ischecked { get; set; }
        public string artist { get; set; }
        public string title { get; set; }
        public string duration { get; set; }
        public string url { get; set; }

        public override string ToString()
        {
            string str = (artist.Clear().Trim() + " - " + title.Clear()).ToShort()+".mp3";
            return str;
        }
    }

    public static class StringsUtility
    {
        public static string Clear(this string str)
        {
            str =
                str.Replace("/", "_")
                    .Replace("\\", "_")
                    .Replace("\n", "_")
                    .Replace(" ", "_")
                    .Replace(".", "_")
                    .Replace(",", "_");
            return str;
        }

        public static string ToShort(this string str)
        {
            str = str.Substring(0, Math.Min(str.Length, 100));
            return str;
        }
    }
}