using UnityEngine;

namespace Arcadian.Extensions
{
    /// <summary>
    /// A set of extension methods for <c>Color</c> to easily adjust brightness, darkness, alpha, saturation, and value. Useful for dynamically modifying colors in UI or visual effects without manually converting between color spaces.
    /// </summary>
    public static class ColorExtensions
    {
        public static Color Brighten(this Color color, float percentage)
        {
            var newColor = color * percentage;
            newColor.a = color.a;
            return newColor;
        }

        public static Color Darken(this Color color, float percentage) => new(color.r * (1 - percentage), color.g * (1 - percentage), color.b * (1 - percentage), color.a);

        public static Color Alpha(this Color color, float alpha) => new(color.r, color.g, color.b, alpha);

        public static Color MultiplySaturation(this Color color, float multiplier)
        {
            Color.RGBToHSV(color, out var h, out var s, out var v);
            s *= multiplier;
            return Color.HSVToRGB(h, s, v);
        }

        public static Color MultiplyValue(this Color color, float multiplier)
        {
            Color.RGBToHSV(color, out var h, out var s, out var v);
            v *= multiplier;
            return Color.HSVToRGB(h, s, v);
        }
    }
}