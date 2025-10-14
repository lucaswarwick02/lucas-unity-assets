using System.Text;
using UnityEngine;

namespace Arcadian.Extensions
{
    /// <summary>
    /// A set of extension methods for <c>string</c> to handle pluralisation, formatting, casing, rich text styling, and emoji insertion. Useful for dynamically generating readable, stylised, and interactive text in UI without manual string manipulation. 
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts a word to the English plural using basic semantic rules.
        /// </summary>
        /// <param name="word">Word to pluralise.</param>
        /// <returns>Plural word.</returns>
        public static string ToPlural(this string word)
        {
            if (string.IsNullOrWhiteSpace(word)) return word;

            return word switch
            {
                _ when word.EndsWith("s") || word.EndsWith("ss") || word.EndsWith("sh") ||
                      word.EndsWith("ch") || word.EndsWith("x") || word.EndsWith("o") => word + "es",
                _ when word.EndsWith("f") => word[..^1] + "ves",
                _ when word.EndsWith("fe") => word[..^2] + "ves",
                _ when word.EndsWith('y') && !"aeiou".Contains(char.ToLower(word[^2])) => word[..^1] + "ies",
                _ => word + "s"
            };
        }

        /// <summary>
        /// Convert a string from Pascal case to Title case.
        /// </summary>
        /// <param name="input">Pascal case string.</param>
        /// <returns>Title case string.</returns>
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

        /// <summary>
        /// Use rich text tags to set the size of text.
        /// </summary>
        /// <param name="text">Target text to wrap.</param>
        /// <param name="size">Size to use.</param>
        /// <returns></returns>
        public static string Size(this string text, int size) => $"<size={size}>{text}</size>";

        /// <summary>
        /// Use rich text tags to set the color of text.
        /// </summary>
        /// <param name="text">Target text to wrap.</param>
        /// <param name="color">Color of the text.</param>
        /// <returns></returns>
        public static string Color(this string text, Color color) => $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color>";

        /// <summary>
        /// Use rich text tags to underline text.
        /// </summary>
        /// <param name="text">Target text to wrap.</param>
        /// <returns></returns>
        public static string Underline(this string text) => $"<u>{text}</u>";

        /// <summary>
        /// Use rich text tags to bolden text.
        /// </summary>
        /// <param name="text">Target text to wrap.</param>
        /// <returns></returns>
        public static string Bold(this string text) => $"<b>{text}</b>";

        /// <summary>
        /// Use rich text tags to strikethrough text.
        /// </summary>
        /// <param name="text">Target text to wrap.</param>
        /// <returns></returns>
        public static string Strikethrough(this string text) => $"<s>{text}</s>";

        /// <summary>
        /// Use rich text tags to italicize text.
        /// </summary>
        /// <param name="text">Target text to wrap.</param>
        /// <returns></returns>
        public static string Italic(this string text) => $"<i>{text}</i>";

        /// <summary>
        /// Use rich text tags to insert an emoji in text.
        /// </summary>
        /// <param name="asset">Asset pack name containing emojis.</param>
        /// <param name="emoji">Name of the emoji.</param>
        /// <returns></returns>
        public static string GetEmoji(this string asset, string emoji) => $"<sprite=\"{asset}\" name=\"{emoji}\">";

        /// <summary>
        /// Left align one string, and right align another, on the same line.
        /// </summary>
        /// <param name="left">String to left align.</param>
        /// <param name="right">String to right align.</param>
        /// <returns>Combined, aligned string.</returns>
        public static string DualAlign(string left, string right) => $"<align=left>{left}<line-height=0>\n<align=right>{right}<line-height=1em>";
    }
}