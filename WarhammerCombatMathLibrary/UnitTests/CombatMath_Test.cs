using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarhammerCombatMathLibrary;

namespace UnitTests
{
    /// <summary>
    /// Tests the CombatMath class.
    /// </summary>
    [TestClass]
    public sealed class CombatMath_Test
    {
        /// <summary>
        /// Tests the case where the provided success threshold is higher than 7.
        /// </summary>
        [TestMethod]
        public void GetNumberOfSuccessfulResults_SuccessThresholdGreaterThan6()
        {
            Assert.AreEqual(0, CombatMath.GetNumberOfSuccessfulResults(7));
        }

        /// <summary>
        /// Tests the case where the provided success threshold is lower than 2.
        /// </summary>
        [TestMethod]
        public void GetNumberOfSuccessfulResults_SuccessThresholdLessThan2()
        {
            Assert.AreEqual(6, CombatMath.GetNumberOfSuccessfulResults(-1));
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetNumberOfSuccessfulResults_SuccessThreshold4()
        {
            Assert.AreEqual(3, CombatMath.GetNumberOfSuccessfulResults(4));
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetNumberOfSuccessfulResults_SuccessThreshold2()
        {
            Assert.AreEqual(5, CombatMath.GetNumberOfSuccessfulResults(2));
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetNumberOfSuccessfulResults_SuccessThreshold5()
        {
            Assert.AreEqual(2, CombatMath.GetNumberOfSuccessfulResults(5));
        }
    }
}
