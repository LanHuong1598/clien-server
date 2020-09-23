using System;
using System.Text;
using System.Text.RegularExpressions;

namespace BELibrary.Utils
{
    public class StringHelper
    {
        public static string ConvertToUnSign(string text)
        {
            return
                new Regex(@"[^a-zA-Z_0-9 \s]").Replace(
                    new Regex(@"\s\s+").Replace(text.Normalize(NormalizationForm.FormD), " ")
                                       .Replace('\u0111', 'd')
                                       .Replace('\u0110', 'D'), "");
        }

        public static string ConvertToAlias(string text)
        {
            return ConvertToUnSign(text).Replace(" ", "-");
        }

        public static string ConvertToH(double seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);

            //here backslash is must to tell that colon is
            //not the part of format, it just a character that we want in output
            return time.ToString(@"hh\:mm\:ss") + " (h)";
        }
    }
}