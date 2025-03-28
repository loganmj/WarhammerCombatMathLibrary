using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarhammerCombatMathLibrary;
using WarhammerCombatMathLibrary.Data;

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

        /// <summary>
        /// Tests the case where the attacker has 0 models.
        /// </summary>
        [TestMethod]
        public void GetTotalNumberOfAttacks_ZeroModels()
        {
            var attacker = new AttackerDTO()
            {
                NumberOfModels = 0,
                WeaponAttacks = 1
            };

            Assert.AreEqual(0, CombatMath.GetTotalNumberOfAttacks(attacker));
        }

        /// <summary>
        /// Tests the case where the attacker has a negative number of models.
        /// </summary>
        [TestMethod]
        public void GetTotalNumberOfAttacks_NegativeNumberOfModels()
        {
            var attacker = new AttackerDTO()
            {
                NumberOfModels = -1,
                WeaponAttacks = 1
            };

            Assert.AreEqual(0, CombatMath.GetTotalNumberOfAttacks(attacker));
        }

        /// <summary>
        /// Tests the case where the attacker has 0 weapon attacks.
        /// </summary>
        [TestMethod]
        public void GetTotalNumberOfAttacks_ZeroWeaponAttacks()
        {
            var attacker = new AttackerDTO()
            {
                NumberOfModels = 1,
                WeaponAttacks = 0
            };

            Assert.AreEqual(0, CombatMath.GetTotalNumberOfAttacks(attacker));
        }

        /// <summary>
        /// Tests the case where the attacker has a negative number of weapon attacks.
        /// </summary>
        [TestMethod]
        public void GetTotalNumberOfAttacks_NegativeWeaponAttacks()
        {
            var attacker = new AttackerDTO()
            {
                NumberOfModels = 1,
                WeaponAttacks = -1
            };

            Assert.AreEqual(0, CombatMath.GetTotalNumberOfAttacks(attacker));
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetTotalNumberOfAttacks_1Models_2Weapons() 
        {
            var attacker = new AttackerDTO()
            {
                NumberOfModels = 1,
                WeaponAttacks = 2
            };

            Assert.AreEqual(2, CombatMath.GetTotalNumberOfAttacks(attacker));
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetTotalNumberOfAttacks_2Models_1Weapons()
        {
            var attacker = new AttackerDTO()
            {
                NumberOfModels = 2,
                WeaponAttacks = 1
            };

            Assert.AreEqual(2, CombatMath.GetTotalNumberOfAttacks(attacker));
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetTotalNumberOfAttacks_10Models_20Weapons()
        {
            var attacker = new AttackerDTO()
            {
                NumberOfModels = 10,
                WeaponAttacks = 20
            };

            Assert.AreEqual(200, CombatMath.GetTotalNumberOfAttacks(attacker));
        }
    }
}
