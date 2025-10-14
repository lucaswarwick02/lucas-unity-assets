namespace Arcadian.StateManagement
{
    /// <summary>
    /// Abstract State with simple abstract functions for entering and exiting it's state.
    /// </summary>
    public abstract class State
    {
        /// <summary>
        /// Ran when the state machine switches to this state.
        /// </summary>
        public virtual void StartState() { }

        /// <summary>
        /// Ran when the state machine exits this state.
        /// </summary>
        public virtual void EndState() { }
    }
}