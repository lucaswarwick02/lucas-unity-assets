using UnityEngine;

namespace LucasWarwick02.UnityAssets
{
    /// <summary>
    /// A set of extension methods for <c>Color</c> to easily adjust brightness, darkness, alpha, saturation, and value. Useful for dynamically modifying colors in UI or visual effects without manually converting between color spaces.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Multiplies the red, green, and blue channels of the color by the specified factors.
        /// </summary>
        /// <param name="color">The original color.</param>
        /// <param name="r">Multiplier for the red channel (default 1 = unchanged).</param>
        /// <param name="g">Multiplier for the green channel (default 1 = unchanged).</param>
        /// <param name="b">Multiplier for the blue channel (default 1 = unchanged).</param>
        /// <returns>A new color with adjusted RGB channels and original alpha.</returns>
        public static Color MultiplyRGB(this Color color, float r = 1f, float g = 1f, float b = 1f) => new(color.r * r, color.g * g, color.b * b, color.a);

        /// <summary>
        /// Sets the alpha channel of the color to the specified value.
        /// </summary>
        /// <param name="color">The original color.</param>
        /// <param name="alpha">The new alpha value (0 = fully transparent, 1 = fully opaque).</param>
        /// <returns>A new color with the specified alpha and original RGB channels.</returns>
        public static Color SetAlpha(this Color color, float alpha) => new(color.r, color.g, color.b, alpha);

        /// <summary>
        /// Multiplies the saturation component of the color in HSV space by the specified multiplier.
        /// </summary>
        /// <param name="color">The original color.</param>
        /// <param name="multiplier">Multiplier for saturation (1 = unchanged, &lt;1 = less saturated, &gt;1 = more saturated).</param>
        /// <returns>A new color with adjusted saturation and original hue, value, and alpha.</returns>
        public static Color MultiplySaturation(this Color color, float multiplier)
        {
            Color.RGBToHSV(color, out var h, out var s, out var v);
            s *= multiplier;
            return Color.HSVToRGB(h, s, v, true);
        }

        /// <summary>
        /// Multiplies the value (brightness) component of the color in HSV space by the specified multiplier.
        /// </summary>
        /// <param name="color">The original color.</param>
        /// <param name="multiplier">Multiplier for value (1 = unchanged, &lt;1 = darker, &gt;1 = brighter).</param>
        /// <returns>A new color with adjusted value and original hue, saturation, and alpha.</returns>
        public static Color MultiplyValue(this Color color, float multiplier)
        {
            Color.RGBToHSV(color, out var h, out var s, out var v);
            v *= multiplier;
            return Color.HSVToRGB(h, s, v, true);
        }

        /// <summary>
        /// Multiplies the hue component of the color in HSV space by the specified multiplier.
        /// </summary>
        /// <param name="color">The original color.</param>
        /// <param name="multiplier">Multiplier for hue (1 = unchanged, values wrap around 0–1).</param>
        /// <returns>A new color with adjusted hue and original saturation, value, and alpha.</returns>
        public static Color MultiplyHue(this Color color, float multiplier)
        {
            Color.RGBToHSV(color, out var h, out var s, out var v);
            h *= multiplier;
            return Color.HSVToRGB(h, s, v, true);
        }
    }
}