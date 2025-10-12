using UnityEngine;

namespace Arcadian.Extensions
{
    /// <summary>
    /// A set of extension methods for </c>GameObject</c> and <c>Object</c>, providing easy access to child components and deep cloning. Useful for quickly finding nested components or duplicating objects without manually copying fields. 
    /// </summary>
    public static class ObjectExtensions
    {
        public static T GetChildComponent<T>(this GameObject gameObject, string childName) where T : Component
        {
            if (!gameObject || string.IsNullOrEmpty(childName)) return null;

            var child = gameObject.transform.Find(childName);
            return child ? child.GetComponent<T>() : null;
        }

        public static T DeepClone<T>(this T obj)
        {
            if (obj == null) return default;

            var json = JsonUtility.ToJson(obj);
            return JsonUtility.FromJson<T>(json);
        }
    }
}
