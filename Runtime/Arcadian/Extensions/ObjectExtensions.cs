using UnityEngine;

namespace Arcadian.Extensions
{
    /// <summary>
    /// A set of extension methods for <c>GameObject</c> and <c>Object</c>, providing easy access to child components and deep cloning. Useful for quickly finding nested components or duplicating objects without manually copying fields. 
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Get a child's component based on the it's GameObject's name.
        /// </summary>
        /// <typeparam name="T">Type of component to find.</typeparam>
        /// <param name="gameObject">Parent GameObject</param>
        /// <param name="childName">Name of the child GameObject.</param>
        /// <returns>The found child's component, or null.</returns>
        public static T GetChildComponent<T>(this GameObject gameObject, string childName) where T : Component
        {
            if (!gameObject || string.IsNullOrEmpty(childName)) return null;

            var child = gameObject.transform.Find(childName);
            return child ? child.GetComponent<T>() : null;
        }

        /// <summary>
        /// Use JsonUtility to deep clone an object.
        /// </summary>
        /// <typeparam name="T">Type of object to deep clone.</typeparam>
        /// <param name="obj">Original object to deep clone.</param>
        /// <returns>A deep cloned copy of the original object.</returns>
        public static T DeepClone<T>(this T obj)
        {
            if (obj == null) return default;

            var json = JsonUtility.ToJson(obj);
            return JsonUtility.FromJson<T>(json);
        }
    }
}
