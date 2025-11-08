using System;
using UnityEngine;
using System.Collections.Generic;

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

        /// <summary>
        /// Adds a one-pixel outline around opaque pixels in the sprite.
        /// </summary>
        /// <param name="sprite">Source sprite whose texture rect will be used as the base for the outline.</param>
        /// <param name="outlineColor">Color of the outline to draw (alpha will be used as-is).</param>
        /// <param name="cutCorners">If <c>true</c>, only orthogonal neighbours are considered (4-neighbour outline). If <c>false</c> (default), all 8 neighbours are considered.</param>
        /// <returns>A new <see cref="Sprite"/> with a single-pixel outline applied where transparent pixels neighbour opaque pixels.</returns>
        /// <remarks>
        /// The algorithm is optimized for runtime usage: it copies the sprite's texture rect into a flat
        /// <see cref="Color32"/> buffer, then scans only transparent pixels and checks their neighbours
        /// for any opaque pixel. If any neighbour is opaque the transparent pixel becomes the outline color.
        /// Existing opaque pixels are preserved. The returned sprite is created from a new <see cref="Texture2D"/>.
        /// </remarks>
        public static Sprite AddOutline(this Sprite sprite, Color32 outlineColor, bool cutCorners = false)
        {
            var rect = sprite.textureRect;
            int width = (int)rect.width, height = (int)rect.height;
            var srcPixels = sprite.texture.GetPixels32();
            int texWidth = sprite.texture.width;

            // Copy sprite rect into a compact buffer
            var pixels = new Color32[width * height];
            for (int y = 0; y < height; y++)
            {
                Array.Copy(srcPixels, ((int)rect.y + y) * texWidth + (int)rect.x, pixels, y * width, width);
            }

            // Output buffer starts as a copy of the original pixels; we'll set outline pixels into it
            var outPixels = new Color32[width * height];
            Array.Copy(pixels, outPixels, pixels.Length);

            // helper to check if a pixel is opaque (alpha > 0)
            static bool IsOpaque(in Color32 c) => c.a != 0;

            // Check transparent pixels only; if any neighbour (4 or 8 depending on cutCorners) is opaque set outline color
            for (int y = 0; y < height; y++)
            {
                int row = y * width;
                for (int x = 0; x < width; x++)
                {
                    int idx = row + x;
                    if (IsOpaque(pixels[idx])) continue; // preserve existing opaque

                    bool neighbourOpaque = false;

                    if (cutCorners)
                    {
                        // 4-neighbour check (up, down, left, right)
                        if (x > 0 && IsOpaque(pixels[idx - 1])) neighbourOpaque = true;
                        else if (x < width - 1 && IsOpaque(pixels[idx + 1])) neighbourOpaque = true;
                        else if (y > 0 && IsOpaque(pixels[idx - width])) neighbourOpaque = true;
                        else if (y < height - 1 && IsOpaque(pixels[idx + width])) neighbourOpaque = true;
                    }
                    else
                    {
                        // 8-neighbour check (including diagonals)
                        int y0 = Math.Max(0, y - 1);
                        int y1 = Math.Min(height - 1, y + 1);
                        int x0 = Math.Max(0, x - 1);
                        int x1 = Math.Min(width - 1, x + 1);

                        for (int ny = y0; ny <= y1 && !neighbourOpaque; ny++)
                        {
                            int nRow = ny * width;
                            for (int nx = x0; nx <= x1; nx++)
                            {
                                if (nx == x && ny == y) continue;
                                if (IsOpaque(pixels[nRow + nx]))
                                {
                                    neighbourOpaque = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (neighbourOpaque)
                    {
                        outPixels[idx] = outlineColor;
                    }
                }
            }

            var newTexture = new Texture2D(width, height)
            {
                filterMode = sprite.texture.filterMode,
                wrapMode = sprite.texture.wrapMode
            };
            newTexture.SetPixels32(outPixels);
            newTexture.Apply();

            var pivot = new Vector2(sprite.pivot.x / rect.width, sprite.pivot.y / rect.height);
            return Sprite.Create(newTexture, new Rect(0, 0, width, height), pivot, sprite.pixelsPerUnit);
        }

        /// <summary>
        /// Divides a sprite sheet into a 2D list of sprites (rows x columns).
        /// Only non-empty cells (pixels with alpha &gt; 0.01) are included.
        /// Notes:
        /// - The texture used by the sprite must be Read/Write enabled in import settings for pixel checks to work.
        /// - If the texture is not readable, the function will still create sprites but will not be able to detect emptiness and will include the cell.
        /// </summary>
        /// <param name="sheet">Source sprite (texture) containing the sheet.</param>
        /// <param name="rows">Number of rows to divide into (vertical slices).</param>
        /// <param name="columns">Number of columns to divide into (horizontal slices).</param>
        /// <param name="pixelsPerUnit">Pixels per unit for created sprites (defaults to 100).</param>
        /// <param name="pivot">Optional pivot point for created sprites (defaults to Unity’s (0.5, 0.5)).</param>
        /// <returns>List of rows, each row is a list of Sprites (empty rows are omitted).</returns>
        public static List<List<Sprite>> DivideSpriteSheet(this Sprite sheet, int rows, int columns, int pixelsPerUnit = 100, Vector2? pivot = null)
        {
            var result = new List<List<Sprite>>();
            if (sheet == null || rows <= 0 || columns <= 0)
                return result;

            Texture2D tex = sheet.texture;
            if (tex == null)
                return result;

            int texW = tex.width;
            int texH = tex.height;

            int cellW = texW / columns;
            int cellH = texH / rows;

            if (cellW <= 0 || cellH <= 0)
                return result;

            Vector2 actualPivot = pivot ?? new Vector2(0.5f, 0.5f);

            bool readable = tex.isReadable;
            Color[] allPixels = readable ? tex.GetPixels() : null;

            for (int r = 0; r < rows; r++)
            {
                var rowList = new List<Sprite>();
                for (int c = 0; c < columns; c++)
                {
                    int x = c * cellW;
                    int y = r * cellH;

                    int w = (c == columns - 1) ? texW - x : cellW;
                    int h = (r == rows - 1) ? texH - y : cellH;

                    if (w <= 0 || h <= 0)
                        continue;

                    bool hasVisible = true;
                    if (readable)
                    {
                        hasVisible = false;
                        int yEnd = y + h;
                        int xEnd = x + w;
                        for (int yy = y; yy < yEnd; yy++)
                        {
                            int rowOffset = yy * texW;
                            for (int xx = x; xx < xEnd; xx++)
                            {
                                if (allPixels[rowOffset + xx].a > 0.01f)
                                {
                                    hasVisible = true;
                                    goto FoundVisible; // break both loops fast
                                }
                            }
                        }
                    }
                FoundVisible:

                    if (!hasVisible)
                        continue;

                    Rect rect = new Rect(x, y, w, h);
                    Sprite s = Sprite.Create(tex, rect, actualPivot, pixelsPerUnit);
                    rowList.Add(s);
                }

                if (rowList.Count > 0)
                    result.Add(rowList);
            }

            return result;
        }
    }
}