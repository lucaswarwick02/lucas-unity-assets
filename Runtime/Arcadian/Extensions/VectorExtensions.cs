using UnityEngine;
using System.Runtime.CompilerServices;

namespace Arcadian.Extensions
{
    /// <summary>
    /// A set of extension methods for <c>Vector2</c> and <c>Vector3</c> to calculate angles, convert between 2D and 3D, and flip or negate axis. Useful for simplifying common vector math operations in gameplay and UI logic. 
    /// </summary>
    public static class VectorExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float AngleRad(this Vector2 v) => Mathf.Atan2(v.y, v.x);

        public static float AngleDeg(this Vector2 v) => AngleRad(v) * Mathf.Rad2Deg;
        public static Vector3 ToVector3(this Vector2 v, float z = 0) => new(v.x, v.y, z);
        public static Vector2 ToVector2(this Vector3 v) => new(v.x, v.y);

        public static Vector2 FlipY(this Vector2 v) => new(v.x, -v.y);
        public static Vector2 FlipX(this Vector2 v) => new(-v.x, v.y);
        public static Vector2 Negate(this Vector2 v) => new(-v.x, -v.y);

        public static Vector3 FlipY(this Vector3 v) => new(v.x, -v.y, v.z);
        public static Vector3 FlipX(this Vector3 v) => new(-v.x, v.y, v.z);
        public static Vector3 Negate(this Vector3 v) => new(-v.x, -v.y, -v.z);
    }
}
