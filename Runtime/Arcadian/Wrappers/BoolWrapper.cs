namespace Arcadian.Wrappers
{
    /// <summary>
    /// Wrapper for a boolean. Seems redundant but is used to pass
    /// booleans by reference, without using the <c>ref</c> keyword.
    /// </summary>
    public class BoolWrapper
    {
        /// <summary>
        /// Stored <c>float</c> value.
        /// </summary>
        public bool Value { get; set; }

        public BoolWrapper(bool value)
        {
            Value = value;
        }
    }
}