using System.Collections.Generic;
using UnityEngine;

namespace LucasWarwick02.UnityAssets
{
    /// <summary>
    /// A <c>Transform</c> extension method to find the closest <c>MonoBehaviour</c> from a list. Useful for targetting the nearest object, enemy, or interactive element efficiently without repeatedly calculating distances manually. 
    /// </summary>
    public static class TransformExtensions
    {
        /// <summary>
        /// Find the closest GameObject based on distance from the target.
        /// </summary>
        /// <typeparam name="T">Type of component to iterate over.</typeparam>
        /// <param name="transform">Main Transform we are comparing against.</param>
        /// <param name="monoBehaviours">List of entities to compare to.</param>
        /// <returns>The closest entity.</returns>
        public static T GetClosest<T>(this Transform transform, List<T> monoBehaviours) where T : MonoBehaviour
        {
            T closest = null;
            float closestDistSqr = Mathf.Infinity;
            Vector3 pos = transform.position;

            for (int i = 0; i < monoBehaviours.Count; i++)
            {
                var mb = monoBehaviours[i];
                float distSqr = (mb.transform.position - pos).sqrMagnitude;

                if (distSqr < closestDistSqr)
                {
                    closest = mb;
                    closestDistSqr = distSqr;
                }
            }

            return closest;
        }

        /// <summary>
        /// Destroy all the children of a transform.
        /// </summary>
        /// <param name="transform">Transform who's children are to be destroyed.</param>
        /// <param name="immediate">Whether to use DestroyImmediate, or Destroy.</param>
        public static void DestroyChildren(this Transform transform, bool immediate = false)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                if (immediate)
                {
                    Object.DestroyImmediate(transform.GetChild(i).gameObject);
                }
                else
                {
                    Object.Destroy(transform.GetChild(i).gameObject);
                }
            }
        }
    }
}