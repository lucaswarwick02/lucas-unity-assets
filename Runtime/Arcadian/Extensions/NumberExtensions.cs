using System;
using System.Text;

namespace Arcadian.Extensions
{
    public static class NumberExtensions
    {
        private static readonly (int value, string symbol)[] RomanMap =
        {
            (1000, "M"), (900, "CM"), (500, "D"), (400, "CD"),
            (100, "C"), (90, "XC"), (50, "L"), (40, "XL"),
            (10, "X"), (9, "IX"), (5, "V"), (4, "IV"), (1, "I")
        };

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
