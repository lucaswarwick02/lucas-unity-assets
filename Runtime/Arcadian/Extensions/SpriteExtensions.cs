using System;
using UnityEngine;

namespace Arcadian.Extensions
{
    /// <summary>
    /// A set of extension methods for <c>Sprite</c> to manipulate pivots, locate pixels by color, covert pixel positions to UV offsets, an remove specific pixel colors. Useful for dynamically adjusting sprite origins, detecting color positions, or creating modified textures at runtime.
    /// </summary>
    public static class SpriteExtensions
    {
        /// <summary>
        /// Vector2 representing the center of a sprite.
        /// </summary>
        public static readonly Vector2 PivotCenter = new(0.5f, 0.5f);

        /// <summary>
        /// Centers the pivot of a sprite to (0.5, 0.5).
        /// </summary>
        /// <param name="sprite">Original sprite.</param>
        /// <returns>Sprite with a pivot of 0.5, 0.5</returns>
        public static Sprite CenterSprite(this Sprite sprite)
        {
            var texture = sprite.texture;
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), PivotCenter, sprite.pixelsPerUnit);
        }

        /// <summary>
        /// Find the (first) pixel coordinate for a given color.
        /// </summary>
        /// <param name="sprite">Sprite to search.</param>
        /// <param name="color">Color to look for.</param>
        /// <returns>Pixel coordinate of the first found color.</returns>
        public static Vector2Int GetPixelPoint(this Sprite sprite, Color32 color)
        {
            var rect = sprite.textureRect;
            var pixels = sprite.texture.GetPixels32();
            int texWidth = sprite.texture.width;

            for (int y = 0; y < (int)rect.height; y++)
            {
                int rowOffset = ((int)rect.y + y) * texWidth + (int)rect.x;
                for (int x = 0; x < (int)rect.width; x++)
                {
                    if (pixels[rowOffset + x].Equals(color)) return new Vector2Int(x, y);
                }
            }

            Debug.LogError("Could not find the pixel point!");
            return Vector2Int.zero;
        }

        /// <summary>
        /// Given a pixel point on a sprite, find the normalized offset.
        /// </summary>
        /// <param name="sprite">Sprite to grab the texture rect from.</param>
        /// <param name="pixelPoint">Pixel point to use as reference.</param>
        /// <returns>Normalized offset.</returns>
        public static Vector2 PixelPointToOffset(this Sprite sprite, Vector2Int pixelPoint)
        {
            var rect = sprite.textureRect;
            return new Vector2(
                (pixelPoint.x + rect.x) / rect.width,
                (pixelPoint.y + rect.y) / rect.height
            );
        }

        /// <summary>
        /// Remove a list of colours from a sprite.
        /// </summary>
        /// <param name="sprite">Sprite to search and filter.</param>
        /// <param name="colors">Colors to find and remove.</param>
        /// <returns>Sprite with removed colours.</returns>
        public static Sprite RemovePixelPoints(this Sprite sprite, params Color32[] colors)
        {
            var rect = sprite.textureRect;
            var texPixels = sprite.texture.GetPixels32();
            int texWidth = sprite.texture.width;
            int width = (int)rect.width, height = (int)rect.height;

            var pixels = new Color32[width * height];
            for (int y = 0; y < height; y++)
            {
                Array.Copy(texPixels, ((int)rect.y + y) * texWidth + (int)rect.x, pixels, y * width, width);
            }

            for (int i = 0; i < pixels.Length; i++)
            {
                foreach (var c in colors)
                {
                    if (pixels[i].Equals(c))
                    {
                        pixels[i] = Color.clear;
                        break;
                    }
                }
            }

            var newTexture = new Texture2D(width, height)
            {
                filterMode = sprite.texture.filterMode
            };
            newTexture.SetPixels32(pixels);
            newTexture.Apply();

            var pivot = new Vector2(sprite.pivot.x / rect.width, sprite.pivot.y / rect.height);
            return Sprite.Create(newTexture, new Rect(0, 0, width, height), pivot, sprite.pixelsPerUnit);
        }
    }
}