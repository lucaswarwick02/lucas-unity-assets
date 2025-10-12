using System;
using System.Collections.Generic;
using System.Linq;

namespace Arcadian.Extensions
{
    /// <summary>
    /// Contains extensions for Enumerables.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Iterates through an Enumerable.
        /// </summary>
        /// <param name="items">Enumerable</param>
        /// <param name="action">Function to run for each item</param>
        /// <typeparam name="T">Type of item to iterate across.</typeparam>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }

        public static T Random<T>(this IReadOnlyList<T> choices)
        {
            return Random(choices, _ => 1);
        }

        public static T Random<T>(this IReadOnlyList<T> items, Func<T, float> weight)
        {
            if (items == null || items.Count == 0)
                throw new ArgumentException("Items collection is empty.", nameof(items));

            float total = 0;
            foreach (var item in items) total += weight(item);

            float pick = UnityEngine.Random.Range(0f, total);
            foreach (var item in items)
            {
                float w = weight(item);
                if (pick < w)
                    return item;
                pick -= w;
            }
            return items[items.Count - 1];
        }
    }
}