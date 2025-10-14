using UnityEngine;

namespace Arcadian.Enums
{
    /// <summary>
    /// A simple enum representing 2D directions (<c>Up</c>, <c>Down</c>, ,<c>Left</c>, <c>Right</c>) with an extension to convert a <c>Vector2D</c> into the closest <c>Direction2D</c>. Useful for interpreting movement or input vectors in a grid or direction based system.
    /// </summary>
    public enum Direction2D
    {
        Up,
        Down,
        Left,
        Right
    }

    /// <summary>
    /// Extension class to provide built-in functionality of Direction2D with existing UnityEngine classes.
    /// </summary>
    public static class Direction2DExtensions
    {
        /// <summary>
        /// Convert a Vector2 into a Direction2D, based on the angle.
        /// </summary>
        /// <param name="vector2">2D vector to use for the angle calculation.</param>
        /// <returns>The Direction2D enum based on the angle.</returns>
        public static Direction2D ToDirection2D(this Vector2 vector2)
        {
            if (Mathf.Abs(vector2.x) > Mathf.Abs(vector2.y))
            {
                return vector2.x > 0 ? Direction2D.Right : Direction2D.Left;
            }

            return vector2.y > 0 ? Direction2D.Up : Direction2D.Down;
        }
    }
}