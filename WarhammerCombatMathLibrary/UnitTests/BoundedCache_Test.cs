using WarhammerCombatMathLibrary;

namespace UnitTests
{
    /// <summary>
    /// Tests the BoundedCache class.
    /// </summary>
    [TestClass]
    public sealed class BoundedCache_Test
    {
        #region Unit Tests

        /// <summary>
        /// Tests the ability to add and retrieve a value to/from the cache
        /// </summary>
        [TestMethod]
        public void AddAndRetrieveItem_ShouldReturnCorrectValue()
        {
            var cache = new BoundedCache<string, int>(2);
            cache.Add("a", 1);

            var result = cache.TryGetValue("a", out var value);

            Assert.IsTrue(result);
            Assert.AreEqual(1, value);
        }

        /// <summary>
        /// Tests the case where the user tries to remove an item that does not exist in the cache
        /// </summary>
        [TestMethod]
        public void RetrieveNonExistentItem_ShouldReturnFalse()
        {
            var cache = new BoundedCache<string, int>(2);

            var result = cache.TryGetValue("missing", out var value);

            Assert.IsFalse(result);
        }

        /// <summary>
        /// Tests the case where a user adds an item beyond the capacity of the cache.
        /// </summary>
        [TestMethod]
        public void AddItemsBeyondCapacity_ShouldEvictLeastRecentlyUsed()
        {
            var cache = new BoundedCache<string, int>(2);
            cache.Add("a", 1);
            cache.Add("b", 2);
            cache.Add("c", 3); // Should evict "a"

            var result = cache.TryGetValue("a", out _);

            Assert.IsFalse(result);
        }

        /// <summary>
        /// Tests the "least recently used" algorithm by manipulating which itme gets evicted when the user adds a new item.
        /// </summary>
        [TestMethod]
        public void AccessItem_ShouldUpdateUsageOrder()
        {
            var cache = new BoundedCache<string, int>(2);
            cache.Add("a", 1);
            cache.Add("b", 2);
            cache.TryGetValue("a", out _); // "a" becomes most recently used
            cache.Add("c", 3); // Should evict "b"

            Assert.IsTrue(cache.TryGetValue("a", out _));
            Assert.IsFalse(cache.TryGetValue("b", out _));
        }

        /// <summary>
        /// Tests the ability to update a value in the cache
        /// </summary>
        [TestMethod]
        public void AddExistingKey_ShouldUpdateValueAndUsage()
        {
            var cache = new BoundedCache<string, int>(2);
            cache.Add("a", 1);
            cache.Add("a", 99); // Update value

            var result = cache.TryGetValue("a", out var value);

            Assert.IsTrue(result);
            Assert.AreEqual(99, value);
        }

        /// <summary>
        /// Tests the Clear() method
        /// </summary>
        [TestMethod]
        public void Clear_ShouldRemoveAllItems()
        {
            var cache = new BoundedCache<string, int>(2);
            cache.Add("a", 1);
            cache.Add("b", 2);

            cache.Clear();

            Assert.IsFalse(cache.TryGetValue("a", out _));
            Assert.IsFalse(cache.TryGetValue("b", out _));
        }


        #endregion
    }
}
