using System.Numerics;
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
            Assert.AreEqual(6, MathUtilities.Factorial(3));
        }

        /// <summary>
        /// Tests that the Factorial method correctly computes the value of 5!.
        /// </summary>
        [TestMethod]
        public void Factorial_TestParameter5()
        {
            Assert.AreEqual(120, MathUtilities.Factorial(5));
        }

        /// <summary>
        /// Tests that the Factorial method correctly computes the value of 9!.
        /// </summary>
        [TestMethod]
        public void Factorial_TestParameter9()
        {
            Assert.AreEqual(362880, MathUtilities.Factorial(9));
        }

        /// <summary>
        /// Tests that the Factorial method correctly computes the value of 9!.
        /// </summary>
        [TestMethod]
        public void Factorial_TestBigParam()
        {
            Assert.AreEqual(BigInteger.Parse("815915283247897734345611269596115894272000000000"), MathUtilities.Factorial(40));
        }

        /// <summary>
        /// Tests that the Factorial method correctly handles a parameter of 0.
        /// </summary>
        [TestMethod]
        public void Factorial_TestParameter0()
        {
            Assert.AreEqual(1, MathUtilities.Factorial(0));
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
