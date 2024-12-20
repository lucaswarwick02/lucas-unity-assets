namespace Arcadian.Wrappers
{
    /// <summary>
    /// Wrapper for a float. Seems redundant but is used to pass
    /// floats by reference, without using the <c>ref</c> keyword.
    /// </summary>
    public class FloatWrapper
    {
        /// <summary>
        /// Stored <c>float</c> value.
        /// </summary>
        public float Value { get; set; }

        public FloatWrapper(float value)
        {
            Value = value;
        }
    }
}