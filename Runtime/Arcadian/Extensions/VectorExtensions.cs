using UnityEngine;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace Arcadian.Extensions
{
    /// <summary>
    /// A set of extension methods for <c>Vector2</c> and <c>Vector3</c> to calculate angles, convert between 2D and 3D, and flip or negate axis. Useful for simplifying common vector math operations in gameplay and UI logic. 
    /// </summary>
    public static class VectorExtensions
    {
        /// <summary>
        /// Calculate the angle of a vector, in radians.
        /// </summary>
        /// <param name="v">Target vector.</param>
        /// <returns>Angle (in radians) of the vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float AngleRad(this Vector2 v) => Mathf.Atan2(v.y, v.x);

        /// <summary>
        /// Calculate the angle of a vector, in degrees.
        /// </summary>
        /// <param name="v">Target vector.</param>
        /// <returns>Angle (in degrees) of the vector.</returns>
        public static float AngleDeg(this Vector2 v) => AngleRad(v) * Mathf.Rad2Deg;

        /// <summary>
        /// Convert a 2D vector to a 3D vector.
        /// </summary>
        /// <param name="v">Target vector.</param>
        /// <param name="z">Value to set the z axis.</param>
        /// <returns>3D vector.</returns>
        public static Vector3 ToVector3(this Vector2 v, float z = 0) => new(v.x, v.y, z);

        /// <summary>
        /// Convert a 3D vector to a 2D vector.
        /// </summary>
        /// <param name="v">Target vector.</param>
        /// <returns>2D vector.</returns>
        public static Vector2 ToVector2(this Vector3 v) => new(v.x, v.y);

        /// <summary>
        /// Flip the y-axis of a vector.
        /// </summary>
        /// <param name="v">Target vector.</param>
        /// <returns>Vector with the y-axis flipped</returns>
        public static Vector2 FlipY(this Vector2 v) => new(v.x, -v.y);

        /// <summary>
        /// Flip the x-axis of a vector.
        /// </summary>
        /// <param name="v">Target vector.</param>
        /// <returns>Vector with the x-axis flipped.</returns>
        public static Vector2 FlipX(this Vector2 v) => new(-v.x, v.y);

        /// <summary>
        /// Set the x and y-axis of a vector to negative.
        /// </summary>
        /// <param name="v">Target vector.</param>
        /// <returns>Negated vector.</returns>
        public static Vector2 Negate(this Vector2 v) => new(-v.x, -v.y);

        /// <summary>
        /// Flip the y-axis of a vector.
        /// </summary>
        /// <param name="v">Target vector.</param>
        /// <returns>Vector with the y-axis flipped</returns>
        public static Vector3 FlipY(this Vector3 v) => new(v.x, -v.y, v.z);

        /// <summary>
        /// Flip the x-axis of a vector.
        /// </summary>
        /// <param name="v">Target vector.</param>
        /// <returns>Vector with the x-axis flipped.</returns>
        public static Vector3 FlipX(this Vector3 v) => new(-v.x, v.y, v.z);

        /// <summary>
        /// Set the x, y, and z-axis of a vector to negative.
        /// </summary>
        /// <param name="v">Target vector.</param>
        /// <returns>Negated vector.</returns>
        public static Vector3 Negate(this Vector3 v) => new(-v.x, -v.y, -v.z);
    }
}
