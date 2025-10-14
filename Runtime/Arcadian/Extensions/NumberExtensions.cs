using System;
using System.Text;

namespace Arcadian.Extensions
{
    /// <summary>
    /// An extension method for converting integers to Roman numerals. Useful for displaying numbers in a classical or stylized format (e.g., UI labels, levels, ranks) without manual mapping logic.
    /// </summary>
    public static class NumberExtensions
    {
        private static readonly (int value, string symbol)[] RomanMap =
        {
            (1000, "M"), (900, "CM"), (500, "D"), (400, "CD"),
            (100, "C"), (90, "XC"), (50, "L"), (40, "XL"),
            (10, "X"), (9, "IX"), (5, "V"), (4, "IV"), (1, "I")
        };

        /// <summary>
        /// Convert an integer (1-3999) to it's Roman numeral string.
        /// </summary>
        /// <param name="num">umber to convert to Roman string.</param>
        /// <returns>Roman numeral string.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the integer is outside of the acceptable range.</exception>
        public static string ToRoman(this int num)
        {
            if (num is < 1 or > 3999)
                throw new ArgumentOutOfRangeException(nameof(num), "Value must be between 1 and 3999.");

            var sb = new StringBuilder();

            foreach (var (value, symbol) in RomanMap)
            {
                while (num >= value)
                {
                    sb.Append(symbol);
                    num -= value;
                }
            }

            return sb.ToString();
        }
    }
}
