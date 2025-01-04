using System.Text;
using UnityEngine;

namespace Arcadian.Extensions
{
    public static class StringExtensions
    {
        public static string ToPlural(this string word)
        {
            if (string.IsNullOrWhiteSpace(word))
                return word;

            // Rules for regular nouns
            if (word.EndsWith("s") || word.EndsWith("ss") || word.EndsWith("sh") || word.EndsWith("ch") || word.EndsWith("x") || word.EndsWith("o"))
                return word + "es";
            
            if (word.EndsWith("f"))
                return word[..^1] + "ves";
            
            if (word.EndsWith("fe"))
                return word[..^2] + "ves";
            
            if (word.EndsWith("y") && !IsVowel(word[^2]))
                return word[..^1] + "ies";
            
            return word + "s";
        }

        private static bool IsVowel(char c)
        {
            return "aeiou".IndexOf(char.ToLower(c)) != -1;
        }
        
        public static string PascalCaseToTitleCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var result = new StringBuilder(input.Length * 2);
            result.Append(input[0]);

            for (var i = 1; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]) && i > 0 && !char.IsUpper(input[i - 1]))
                    result.Append(' ');

                result.Append(input[i]);
            }

            return result.ToString();
        }
        
        public static string Size(this string text, int size)
        {
            return $"<size={size}>{text}</size>";
        }

        public static string Color(this string text, Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color>";
        }

        public static string DualAlign(string left, string right)
        {
            return $"<align=left>{left}<line-height=0>\n<align=right>{right}<line-height=1em>";
        }

        public static string Underline(this string text) => $"<u>{text}</u>";
        public static string Bold(this string text) => $"<b>{text}</b>";
        public static string Strikethrough(this string text) => $"<s>{text}</s>";
        public static string Italic(this string text) => $"<i>{text}</i>";

        public static string GetEmoji(this string asset, string emoji)
        {
            return $"<sprite=\"{asset}\" name=\"{emoji}\">";
        }
    }
}