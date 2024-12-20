using UnityEngine;

namespace Arcadian.Extensions
{
    public static class VectorExtensions
    {
        private static float AngleRad(this Vector2 vector)
        {
            return Mathf.Atan2(vector.y, vector.x);
        }
        
        /// <summary>
        /// Converts a Vector2 to it's angle in degrees.
        /// </summary>
        /// <param name="vector">Vector2</param>
        /// <returns>Degrees</returns>
        public static float AngleDeg(this Vector2 vector)
        {
            return AngleRad(vector) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Casts the Vector2 to a Vector3, adding z.
        /// </summary>
        /// <param name="vector2">Original vector</param>
        /// <param name="z">Z value</param>
        /// <returns></returns>
        public static Vector3 ToVector3(this Vector2 vector2, float z = 0)
        {
            return new Vector3(vector2.x, vector2.y, z);
        }

        public static Vector2 ToVector2(this Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.y);
        }

        public static Vector2 FlipY(this Vector2 vector)
        {
            return new Vector2(vector.x, -vector.y);
        }
    }
}