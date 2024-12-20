using TypeReferences;
using System;

namespace Arcadian.Extensions
{
    public static class TypeReferenceExtensions
    {
        public static T Cast<T>(this TypeReference typeReference)
        {
            return (T) Activator.CreateInstance(typeReference);
        }
    }
}