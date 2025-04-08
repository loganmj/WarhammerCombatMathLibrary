using System.Diagnostics;
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
        public void GetTotalNumberOfAttacks_NullAttacker()
        {
            Assert.AreEqual(0, CombatMath.GetTotalNumberOfAttacks(null));
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
        public void GetTotalNumberOfAttacks_TestParams1()
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
        public void GetTotalNumberOfAttacks_TestParams2()
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
        public void GetTotalNumberOfAttacks_TestParams3()
        {
            var attacker = new AttackerDTO()
            {
                NumberOfModels = 10,
                WeaponAttacks = 20
            };

            Assert.AreEqual(200, CombatMath.GetTotalNumberOfAttacks(attacker));
        }

        /// <summary>
        /// Tests the case where the attacker has 0 models.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHit_NullAttacker()
        {
            Assert.AreEqual(0, CombatMath.GetProbabilityOfHit(null));
        }

        /// <summary>
        /// Tests the case where the attacker has 0 models.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHit_ZeroWeaponSkill()
        {
            var attacker = new AttackerDTO()
            {
                WeaponSkill = 0
            };

            Assert.AreEqual(1, CombatMath.GetProbabilityOfHit(attacker));
        }

        /// <summary>
        /// Tests the case where the attacker has a negative number of models.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHit_NegativeWeaponSkill()
        {
            var attacker = new AttackerDTO()
            {
                WeaponSkill = -1
            };

            Assert.AreEqual(1, CombatMath.GetProbabilityOfHit(attacker));
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHit_WeaponSKill2()
        {
            var attacker = new AttackerDTO()
            {
                WeaponSkill = 2
            };

            Console.WriteLine($"TEST - Attacker: {attacker}");

            Assert.AreEqual(0.83, Math.Round(CombatMath.GetProbabilityOfHit(attacker)), 2);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHit_WeaponSKill3()
        {
            var attacker = new AttackerDTO()
            {
                WeaponSkill = 3
            };

            Assert.AreEqual(0.67, Math.Round(CombatMath.GetProbabilityOfHit(attacker)), 2);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHit_WeaponSKill5()
        {
            var attacker = new AttackerDTO()
            {
                WeaponSkill = 5
            };

            Assert.AreEqual(0.33, Math.Round(CombatMath.GetProbabilityOfHit(attacker)), 2);
        }

        /// <summary>
        /// Tests the case where the attacker parameter is null.
        /// </summary>
        [TestMethod]
        public void GetMeanHits_AttackerIsNull()
        {
            Assert.AreEqual(0, CombatMath.GetMeanHits(null));
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanHits_TestParams1()
        {
            var attacker = new AttackerDTO()
            {
                NumberOfModels = 1,
                WeaponAttacks = 1,
                WeaponSkill = 4
            };

            Assert.AreEqual(0.5, CombatMath.GetMeanHits(attacker));
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanHits_TestParams2()
        {
            var attacker = new AttackerDTO()
            {
                NumberOfModels = 10,
                WeaponAttacks = 10,
                WeaponSkill = 4
            };

            Assert.AreEqual(50, CombatMath.GetMeanHits(attacker));
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanHits_TestParams3()
        {
            var attacker = new AttackerDTO()
            {
                NumberOfModels = 10,
                WeaponAttacks = 4,
                WeaponSkill = 3
            };

            Assert.AreEqual(26.67, Math.Round(CombatMath.GetMeanHits(attacker), 2));
        }

        /// <summary>
        /// Tests the case where the attacker parameter is null.
        /// </summary>
        [TestMethod]
        public void GetExpectedHits_AttackerIsNull()
        {
            Assert.AreEqual(0, CombatMath.GetExpectedHits(null));
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedHits_TestParams1()
        {
            var attacker = new AttackerDTO()
            {
                NumberOfModels = 1,
                WeaponAttacks = 1,
                WeaponSkill = 4
            };

            Assert.AreEqual(0, CombatMath.GetExpectedHits(attacker));
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedHits_TestParams2()
        {
            var attacker = new AttackerDTO()
            {
                NumberOfModels = 10,
                WeaponAttacks = 10,
                WeaponSkill = 4
            };

            Assert.AreEqual(50, CombatMath.GetExpectedHits(attacker));
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedHits_TestParams3()
        {
            var attacker = new AttackerDTO()
            {
                NumberOfModels = 10,
                WeaponAttacks = 4,
                WeaponSkill = 3
            };

            Assert.AreEqual(26, CombatMath.GetExpectedHits(attacker));
        }

        /// <summary>
        /// Tests the case where the attacker parameter is null.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationHits_AttackerIsNull()
        {
            Assert.AreEqual(0, CombatMath.GetStandardDeviationHits(null));
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationHits_TestParams1()
        {
            var attacker = new AttackerDTO()
            {
                NumberOfModels = 1,
                WeaponAttacks = 1,
                WeaponSkill = 4
            };

            Assert.AreEqual(0.5, CombatMath.GetStandardDeviationHits(attacker));
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationHits_TestParams2()
        {
            var attacker = new AttackerDTO()
            {
                NumberOfModels = 10,
                WeaponAttacks = 10,
                WeaponSkill = 4
            };

            Assert.AreEqual(5, CombatMath.GetStandardDeviationHits(attacker));
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationHits_TestParams3()
        {
            var attacker = new AttackerDTO()
            {
                NumberOfModels = 10,
                WeaponAttacks = 4,
                WeaponSkill = 3
            };

            Assert.AreEqual(2.98, Math.Round(CombatMath.GetStandardDeviationHits(attacker), 2));
        }

        /// <summary>
        /// Tests the case where the attacker parameter is null.
        /// </summary>
        [TestMethod]
        public void GetBinomialDistributionOfHits_AttackerIsNull()
        {
            var expected = new List<BinomialOutcome>();
            var actual = CombatMath.GetBinomialDistributionOfHits(null);

            // Print expected
            Debug.WriteLine($"Expected: ");
            foreach (var value in expected)
            {
                Debug.WriteLine(value);
            }

            // Print actual
            Debug.WriteLine($"Actual: ");
            foreach (var value in actual)
            {
                Debug.WriteLine(value);
            }

            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetBinomialDistributionOfHits_TestParams1()
        {
            var attacker = new AttackerDTO()
            {
                NumberOfModels = 1,
                WeaponAttacks = 1,
                WeaponSkill = 4
            };

            var expected = new List<BinomialOutcome>
            {
                new(0, 0.5),
                new(1, 0.5)
            };
            
            var actual = CombatMath.GetBinomialDistributionOfHits(attacker);

            // Print expected
            Debug.WriteLine($"Expected: ");
            foreach (var value in expected)
            {
                Debug.WriteLine(value);
            }

            // Print actual
            Debug.WriteLine($"Actual: ");
            foreach (var value in actual)
            {
                Debug.WriteLine(value);
            }

            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetBinomialDistributionOfHits_TestParams2()
        {
            var attacker = new AttackerDTO()
            {
                NumberOfModels = 1,
                WeaponAttacks = 3,
                WeaponSkill = 4
            };

            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.125),
                new(1, 0.375),
                new(2, 0.375),
                new(3, 0.125)
            };

            var actual = CombatMath.GetBinomialDistributionOfHits(attacker);

            // Print expected
            Debug.WriteLine($"Expected: ");
            foreach (var value in expected)
            {
                Debug.WriteLine(value);
            }

            // Print actual
            Debug.WriteLine($"Actual: ");
            foreach (var value in actual)
            {
                Debug.WriteLine(value);
            }

            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetBinomialDistributionOfHits_TestParams3()
        {
            var attacker = new AttackerDTO()
            {
                NumberOfModels = 2,
                WeaponAttacks = 2,
                WeaponSkill = 4
            };

            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.0625),
                new(1, 0.25),
                new(2, 0.375),
                new(3, 0.25),
                new(4, 0.0625)
            };

            var actual = CombatMath.GetBinomialDistributionOfHits(attacker);

            // Print expected
            Debug.WriteLine($"Expected: ");
            foreach (var value in expected)
            {
                Debug.WriteLine(value);
            }

            // Print actual
            Debug.WriteLine($"Actual: ");
            foreach (var value in actual)
            {
                Debug.WriteLine(value);
            }

            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the attacker object is null
        /// </summary>
        [TestMethod]
        public void GetAdjustedArmorSave_NullAttacker()
        {
            var defender = new DefenderDTO();
            Assert.AreEqual(0, CombatMath.GetAdjustedArmorSave(null, defender));
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetAdjustedArmorSave_NullDefender()
        {
            var attacker = new AttackerDTO();
            Assert.AreEqual(0, CombatMath.GetAdjustedArmorSave(attacker, null));
        }

        /// <summary>
        /// Test the case where the attacker object is null
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfFailedSave_NullAttacker()
        {
            var defender = new DefenderDTO();
            Assert.AreEqual(0, CombatMath.GetProbabilityOfFailedSave(null, defender));
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfFailedSave_NullDefender()
        {
            var attacker = new AttackerDTO();
            Assert.AreEqual(0, CombatMath.GetProbabilityOfFailedSave(attacker, null));
        }

        /// <summary>
        /// Test the case where the attacker object is null
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_NullAttacker()
        {
            var defender = new DefenderDTO();
            Assert.AreEqual(0, CombatMath.GetMeanFailedSaves(null, defender));
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_NullDefender()
        {
            var attacker = new AttackerDTO();
            Assert.AreEqual(0, CombatMath.GetMeanFailedSaves(attacker, null));
        }
    }
}
