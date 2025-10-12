using TypeReferences;
using System;

namespace Arcadian.Extensions
{
    /// <summary>
    /// A simple extension for <c>TypeReference</c> to create an instance of the referenced type and cast it to a specified generic type. Useful for dynamically instantiating types without manually calling <c>Activator.CreateInstance</c>.
    /// </summary>
    public static class TypeReferenceExtensions
    {
        public static T Cast<T>(this TypeReference typeReference)
        {
            return (T)Activator.CreateInstance(typeReference);
        }
    }
}