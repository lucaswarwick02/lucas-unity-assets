using System.Text;
using UnityEngine;

namespace Arcadian.Extensions
{
    /// <summary>
    /// A set of extension methods for <c>string</c> to handle pluralisation, formatting, casing, rich text styling, and emoji insertion. Useful for dynamically generating readable, stylised, and interactive text in UI without manual string manipulation. 
    /// </summary>
    public static class StringExtensions
    {
        public static string ToPlural(this string word)
        {
            if (string.IsNullOrWhiteSpace(word)) return word;

            return word switch
            {
                _ when word.EndsWith("s") || word.EndsWith("ss") || word.EndsWith("sh") ||
                      word.EndsWith("ch") || word.EndsWith("x") || word.EndsWith("o") => word + "es",
                _ when word.EndsWith("f") => word[..^1] + "ves",
                _ when word.EndsWith("fe") => word[..^2] + "ves",
                _ when word.EndsWith("y") && !"aeiou".Contains(char.ToLower(word[^2])) => word[..^1] + "ies",
                _ => word + "s"
            };
        }

        public static string PascalCaseToTitleCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var sb = new StringBuilder(input.Length * 2);
            sb.Append(input[0]);

            for (var i = 1; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]) && !char.IsUpper(input[i - 1])) sb.Append(' ');
                sb.Append(input[i]);
            }

            return sb.ToString();
        }

        public static string Size(this string text, int size) => $"<size={size}>{text}</size>";
        public static string Color(this string text, Color color) => $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color>";
        public static string Underline(this string text) => $"<u>{text}</u>";
        public static string Bold(this string text) => $"<b>{text}</b>";
        public static string Strikethrough(this string text) => $"<s>{text}</s>";
        public static string Italic(this string text) => $"<i>{text}</i>";
        public static string GetEmoji(this string asset, string emoji) => $"<sprite=\"{asset}\" name=\"{emoji}\">";
        public static string DualAlign(string left, string right) => $"<align=left>{left}<line-height=0>\n<align=right>{right}<line-height=1em>";
    }
}