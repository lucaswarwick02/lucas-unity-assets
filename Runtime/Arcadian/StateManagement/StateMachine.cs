using System;
using System.Linq;

namespace Arcadian.StateManagement
{
    /// <summary>
    /// A generic state machine for managing <c>State</c> objects. Automatically calls <c>EndState</c> on the previous state and <c>StartState</c> on the new state. Useful for basic AI, gameplay logic, or UI state management.
    /// </summary>
    /// <typeparam name="T">Type of State to use.</typeparam>
    public class StateMachine<T> where T : State
    {
        /// <summary>
        /// Current state of the system.
        /// </summary>
        public T CurrentState { get; private set; }
        
        private T[] States { get; }

        /// <summary>
        /// Event triggered when the state changes.
        /// </summary>
        public event Action OnStateChange;

        /// <summary>
        /// Setup the system with the preconfigured states.
        /// </summary>
        /// <param name="states">List of states to pick from.</param>
        public StateMachine(T[] states)
        {
            States = states;
        }

        /// <summary>
        /// Switch to a state given it's type.
        /// </summary>
        /// <param name="type"></param>
        public void SetState(Type type)
        {
            // No changes in state
            if (CurrentState != null && type == CurrentState.GetType()) return;

            CurrentState?.EndState();

            CurrentState = States.FirstOrDefault(state => state.GetType() == type);

            CurrentState?.StartState();
            OnStateChange?.Invoke();
        }
    }
}