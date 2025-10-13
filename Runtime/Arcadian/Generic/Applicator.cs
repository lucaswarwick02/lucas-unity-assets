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

        public int Count => _functions.Count;
        public bool HasFunctions => _functions.Count > 0;

        public void Add(Func<T, T> function) => _functions.Add(function);
        public void Remove(Func<T, T> function) => _functions.Remove(function);
        public T Invoke(T arg) => _functions.Aggregate(arg, (current, function) => function(current));
        public void Clear() => _functions.Clear();
    }

    /// <summary>
    /// Stores a list of functions, and can "Invoke" them in a series.
    /// Same functionality as an event. But allows for the same Activator pattern.
    /// </summary>
    public class Applicator
    {
        private List<Action> _functions = new();

        public void Add(Action function) => _functions.Add(function);
        public void Remove(Action function) => _functions.Remove(function);
        public void Invoke() => _functions.ForEach(f => f());
        public void Clear() => _functions.Clear();

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