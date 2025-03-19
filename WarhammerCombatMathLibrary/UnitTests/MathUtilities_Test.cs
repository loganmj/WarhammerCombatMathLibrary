using WarhammerCombatMathLibrary;

namespace UnitTests
{
    /// <summary>
    /// A test class for the MathUtilities class library.
    /// </summary>
    [TestClass]
    public sealed class MathUtilities_Test
    {
        /// <summary>
        /// Tests that the Factorial method correctly computes the value of 3!.
        /// </summary>
        [TestMethod]
        public void Factorial_TestParameter3()
        {
            Assert.AreEqual<int>(6, MathUtilities.Factorial(3));
        }

        /// <summary>
        /// Tests that the Factorial method correctly computes the value of 5!.
        /// </summary>
        [TestMethod]
        public void Factorial_TestParameter5()
        {
            Assert.AreEqual<int>(120, MathUtilities.Factorial(5));
        }

        /// <summary>
        /// Tests that the Factorial method correctly computes the value of 9!.
        /// </summary>
        [TestMethod]
        public void Factorial_TestParameter9()
        {
            Assert.AreEqual<int>(362880, MathUtilities.Factorial(9));
        }

        /// <summary>
        /// Tests that the Factorial method correctly handles a parameter of 0.
        /// </summary>
        [TestMethod]
        public void Factorial_TestParameter0()
        {
            Assert.AreEqual<int>(1, MathUtilities.Factorial(0));
        }

        /// <summary>
        /// Tests that the Factorial method throws an error when a negative parameter is given.
        /// </summary>
        [TestMethod]
        public void Factorial_TestNegativeParameter()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => MathUtilities.Factorial(-1));
        }
    }
}
