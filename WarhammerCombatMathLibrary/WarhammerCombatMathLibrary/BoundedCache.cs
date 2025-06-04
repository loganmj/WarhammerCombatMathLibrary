using System.Diagnostics.CodeAnalysis;

namespace WarhammerCombatMathLibrary
{
    /// <summary>
    /// A helper class that implements a bounded cache object.
    /// This allows the program to create caches for expensive operations that are bounded using a FIFO process to help avoid using too much memory.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    internal class BoundedCache<TKey, TValue>(int maxSize) where TKey : notnull
    {
        #region Fields

        private readonly int _maxSize = maxSize;
        private readonly Dictionary<TKey, TValue> _cache = [];
        private readonly Queue<TKey> _order = new();

        #endregion

        #region Public Methods

        /// <summary>
        /// Attempts to retrieve a value from the cache.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>A boolean value, indicating whether the data pair with the given key and value were able to be retrieved.</returns>
        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            return _cache.TryGetValue(key, out value);
        }

        /// <summary>
        /// Adds a key/value pair to the cache.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, TValue value)
        {
            if (_cache.ContainsKey(key)) return;

            if (_cache.Count >= _maxSize)
            {
                var oldestKey = _order.Dequeue();
                _cache.Remove(oldestKey);
            }

            _cache[key] = value;
            _order.Enqueue(key);
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public void Clear()
        {
            _cache.Clear();
            _order.Clear();
        }

        #endregion
    }

}
