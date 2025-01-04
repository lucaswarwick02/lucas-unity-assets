using UnityEngine;

namespace Arcadian.Extensions
{
    public static class SpriteExtensions
    {
        public const float PixelsPerUnit = 16f;
        public static readonly Vector2 PivotCenter = new(0.5f, 0.5f);
        
        /// <summary>
        /// Centers the pivot of a sprite
        /// </summary>
        /// <param name="sprite"></param>
        /// <returns>Sprite with a pivot of 0.5, 0.5</returns>
        public static Sprite CenterSprite(this Sprite sprite)
        {
            var texture = sprite.texture;
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), PivotCenter, 16);
        }
        
        public static Texture2D GetSubTexture(Texture2D texture, Rect rect)
        {
            var subTexture = new Texture2D((int)rect.width, (int)rect.height)
            {
                filterMode = texture.filterMode
            };
            var pixels = texture.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
            subTexture.SetPixels(pixels);
            subTexture.Apply();
            return subTexture;
        }

        public static Vector2 CalculatePivot(Sprite sprite)
        {
            var bounds = sprite.bounds;
            var pivotX = -bounds.center.x / bounds.extents.x / 2 + 0.5f;
            var pivotY = -bounds.center.y / bounds.extents.y / 2 + 0.5f;

            return new Vector2(pivotX, pivotY);
        }
        
        public static Vector2Int GetPixelPoint(this Sprite sprite, Color32 pixelColor)
        {
            var texture = GetSubTexture(sprite.texture, sprite.textureRect);
            var pixels = texture.GetPixels32();

            for (int x = 0, length = pixels.Length; x < length; x++)
            {
                var pixel = pixels[x];
                if (pixel.r == pixelColor.r && pixel.g == pixelColor.g && pixel.b == pixelColor.b && pixel.a == pixelColor.a)
                {
                    return new Vector2Int(x % texture.width, x / texture.width);
                }
            }
            
            Debug.LogError("Could not find the pixel point!");

            return Vector2Int.zero;
        }
        
        public static Vector2 PixelPointToOffset(this Sprite sprite, Vector2Int pixelPoint)
        {
            var texture = GetSubTexture(sprite.texture, sprite.textureRect);
            var offset = new Vector2(
                (float)pixelPoint.x / texture.width,
                (float)pixelPoint.y / texture.height);

            return offset;
        }

        public static Sprite RemovePixelPoints(this Sprite sprite, params Color32[] pixelColors)
        {
            var originalTexture = GetSubTexture(sprite.texture, sprite.textureRect);
            var pixels = originalTexture.GetPixels32();

            foreach (var pixelColor in pixelColors)
            {
                var pixelPoint = sprite.GetPixelPoint(pixelColor);
                pixels[pixelPoint.y * originalTexture.width + pixelPoint.x] = Color.clear;
            }

            originalTexture.SetPixels32(pixels);
            originalTexture.Apply();

            var pivot = CalculatePivot(sprite);

            return Sprite.Create(originalTexture, new Rect(0, 0, originalTexture.width, originalTexture.height), pivot, 16);
        }
    }
}