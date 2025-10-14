namespace Arcadian.Generic
{
    /// <summary>
    /// A simple generic struct that wraps a value in a reference-like container. Useful for passing values by reference, or storing mutable data in contexts where normal value types would be copied. Useful for scenarios where <c>ref</c> cannot be used (bit pedantic, but does have it's use cases).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    public class Ref<T>
    {
        /// <summary>
        /// Value to store.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Store the value in the object.
        /// </summary>
        /// <param name="value">Value to store.</param>
        public Ref(T value)
        {
            Value = value;
        }
    }
}