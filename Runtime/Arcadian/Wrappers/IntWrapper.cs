namespace Arcadian.Wrappers
{
    /// <summary>
    /// Wrapper for an Integer. Seems redundant but it's used to pass
    /// integers by reference, without using the <c>ref</c> keyword.
    /// </summary>
    public class IntWrapper
    {
        /// <summary>
        /// Stored <c>Integer</c> value.
        /// </summary>
        public int Value { get; set; }

        public IntWrapper(int value)
        {
            Value = value;
        }
    }
}