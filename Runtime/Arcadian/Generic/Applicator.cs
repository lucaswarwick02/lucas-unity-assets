using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/// A generic utility class that stores a list of functions or actions and invokes them in a sequence. Useful for building modular, chainable operations or event-like systems where you want to process a value through multiple transformations, or none at all. Good example would be equipment in an RPG that provide bonuses when equipped. 
namespace Arcadian.Generic
{
    /// <summary>
    /// Stores a list of functions, and can "Invoke" them in a series.
    /// Similar to an event, but returns the invoked object.
    /// </summary>
    public class Applicator<T>
    {
        private readonly List<Func<T, T>> _functions = new();

        /// <summary>
        /// Current number of added functions.
        /// </summary>
        public int Count => _functions.Count;

        /// <summary>
        /// True if the applicator has any added functions.
        /// </summary>
        public bool HasFunctions => _functions.Count > 0;

        /// <summary>
        /// Add a function to the applicator at the end of the queue.
        /// </summary>
        /// <param name="function">Function to add</param>
        public void Add(Func<T, T> function) => _functions.Add(function);

        /// <summary>
        /// Remove a function from the queue.
        /// </summary>
        /// <param name="function">Function to remove</param>
        public void Remove(Func<T, T> function) => _functions.Remove(function);

        /// <summary>
        /// Invoke the applicator and pass through a value.
        /// </summary>
        /// <param name="arg">Value to pass through.</param>
        /// <returns>Value after being applied.</returns>
        public T Invoke(T arg) => _functions.Aggregate(arg, (current, function) => function(current));

        /// <summary>
        /// Remove all added functions.
        /// </summary>
        public void Clear() => _functions.Clear();
    }

    /// <summary>
    /// Stores a list of functions, and can "Invoke" them in a series.
    /// Same functionality as an event. But allows for the same Activator pattern.
    /// </summary>
    public class Applicator
    {
        private readonly List<Action> _functions = new();

        /// <summary>
        /// Current number of added functions.
        /// </summary>
        public int Count => _functions.Count;

        /// <summary>
        /// True if the applicator has any added functions.
        /// </summary>
        public bool HasFunctions => _functions.Count > 0;

        /// <summary>
        /// Add a function to the applicator at the end of the queue.
        /// </summary>
        /// <param name="function">Function to add</param>
        public void Add(Action function) => _functions.Add(function);

        /// <summary>
        /// Remove a function from the queue.
        /// </summary>
        /// <param name="function">Function to remove</param>
        public void Remove(Action function) => _functions.Remove(function);

        /// <summary>
        /// Invoke all the functions.
        /// </summary>
        public void Invoke() => _functions.ForEach(f => f());

        /// <summary>
        /// Remove all added functions.
        /// </summary>
        public void Clear() => _functions.Clear();

        /// <summary>
        /// Clear all applicator objects.
        /// </summary>
        /// <param name="classType">Type to search for.</param>
        public static void ClearFields(Type classType)
        {
            var fields = classType.GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in fields)
            {
                var fieldType = field.FieldType;

                if (fieldType != typeof(Applicator) &&
                    (!fieldType.IsGenericType || fieldType.GetGenericTypeDefinition() != typeof(Applicator<>)))
                    continue;

                var applicator = field.GetValue(null);
                if (applicator == null) continue;

                var clearMethod = fieldType.GetMethod("Clear");
                clearMethod?.Invoke(applicator, null);
            }
        }
    }
}