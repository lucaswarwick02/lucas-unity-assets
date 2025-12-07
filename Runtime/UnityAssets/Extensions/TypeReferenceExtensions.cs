using TypeReferences;
using System;

namespace LucasWarwick02.UnityAssets
{
    /// <summary>
    /// A simple extension for <c>TypeReference</c> to create an instance of the referenced type and cast it to a specified generic type. Useful for dynamically instantiating types without manually calling <c>Activator.CreateInstance</c>.
    /// </summary>
    public static class TypeReferenceExtensions
    {
        /// <summary>
        /// Create an instance of a TypeReference, assuming no constructor arguments.
        /// </summary>
        /// <typeparam name="T">Type of object to create</typeparam>
        /// <param name="typeReference">Type to cast to</param>
        /// <returns>Created object.</returns>
        public static T Cast<T>(this TypeReference typeReference)
        {
            return (T)Activator.CreateInstance(typeReference);
        }
    }
}