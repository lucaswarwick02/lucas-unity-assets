using System;

namespace Game.Core.Items
{
    /// <summary>
    /// A generic wrapper for lazy-cached values with custom initialization logic.
    /// </summary>
    public class CachedValue<T> where T : class
    {
        private T _value;
        private readonly Func<T> _factory;

        public CachedValue(Func<T> factory)
        {
            _factory = factory;
        }

        public T Value
        {
            get
            {
                if (_value == null)
                {
                    _value = _factory?.Invoke();
                }
                return _value;
            }
        }

        public void Invalidate() => _value = null;
    }
}