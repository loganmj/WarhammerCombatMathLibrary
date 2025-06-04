namespace WarhammerCombatMathLibrary
{
    /// <summary>
    /// A helper class that implements a bounded cache object.
    /// This allows the program to create caches for expensive operations that are bounded using a FIFO process to help avoid using too much memory.
    /// Implements "Least Recently Used" algorithm for determing which items to dump when cache reaches its max capacity.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    internal class BoundedCache<TKey, TValue>(int capacity) where TKey : notnull
    {
        #region Fields

        private readonly int _capacity = capacity;
        private readonly Dictionary<TKey, LinkedListNode<(TKey Key, TValue Value)>> _cacheMap = [];
        private readonly LinkedList<(TKey Key, TValue Value)> _leastRecentlyUsedList = [];
        private readonly object _lock = new();

        #endregion

        #region Public Methods

        /// <summary>
        /// Attempts to retrieve a value from the cache.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>A boolean value, indicating whether the data pair with the given key and value were able to be retrieved.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (_lock)
            {
                if (_cacheMap.TryGetValue(key, out var node))
                {
                    _leastRecentlyUsedList.Remove(node);
                    _leastRecentlyUsedList.AddFirst(node);
                    value = node.Value.Value;
                    return true;
                }

                value = default!;
                return false;
            }

        }


        /// <summary>
        /// Adds a key/value pair to the cache.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, TValue value)
        {
            lock (_lock)
            {
                if (_cacheMap.TryGetValue(key, out var existingNode))
                {
                    _leastRecentlyUsedList.Remove(existingNode);
                }
                else if (_cacheMap.Count >= _capacity)
                {
                    // Remove least recently used item
                    var lru = _leastRecentlyUsedList.Last!;
                    _cacheMap.Remove(lru.Value.Key);
                    _leastRecentlyUsedList.RemoveLast();
                }

                var newNode = new LinkedListNode<(TKey, TValue)>((key, value));
                _leastRecentlyUsedList.AddFirst(newNode);
                _cacheMap[key] = newNode;
            }
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                _cacheMap.Clear();
                _leastRecentlyUsedList.Clear();
            }
        }

        #endregion
    }

}
