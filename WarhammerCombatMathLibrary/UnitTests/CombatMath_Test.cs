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
        #region Constants

        /// <summary>
        /// Attack profile for Kharn the Betrayer
        /// </summary>
        public static readonly AttackerDTO ATTACKER_KHARN_THE_BETRAYER = new()
        {
            NumberOfModels = 1,
            WeaponAttacks = 8,
            WeaponSkill = 2,
            WeaponStrength = 6,
            WeaponArmorPierce = 2,
            WeaponDamage = 3
        };

        /// <summary>
        /// Attack profile for a 10 man squad of Space Marine Intercessors, all equipped with Bolt Rifles
        /// </summary>
        public static readonly AttackerDTO ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD = new()
        {
            NumberOfModels = 10,
            WeaponAttacks = 2,
            WeaponSkill = 3,
            WeaponStrength = 4,
            WeaponArmorPierce = 1,
            WeaponDamage = 1
        };

        /// <summary>
        /// Attack profile for a five man squad of Space Marine Terminators, all equipped with Chain Fists
        /// </summary>
        public static readonly AttackerDTO ATTACKER_SPACE_MARINE_TERMINATOR_SQUAD = new()
        {
            NumberOfModels = 5,
            WeaponAttacks = 3,
            WeaponSkill = 4,
            WeaponStrength = 8,
            WeaponArmorPierce = 2,
            WeaponDamage = 2
        };

        /// <summary>
        /// Defense profile for a 10 man squad of Space Marine Intercessors
        /// </summary>
        public static readonly DefenderDTO DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD = new()
        {
            NumberOfModels = 10,
            Toughness = 4,
            ArmorSave = 3,
            InvulnerableSave = 7,
            FeelNoPain = 7,
            Wounds = 2
        };

        /// <summary>
        /// Defense profile for a 5 man squad of Space Marine Terminators
        /// </summary>
        public static readonly DefenderDTO DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD = new()
        {
            NumberOfModels = 5,
            Toughness = 5,
            ArmorSave = 2,
            InvulnerableSave = 4,
            FeelNoPain = 7,
            Wounds = 3
        };

        #endregion

        #region Unit Tests 

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
            var expected = 0;
            var actual = CombatMath.GetTotalNumberOfAttacks(null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has 0 models.
        /// </summary>
        [TestMethod]
        public void GetTotalNumberOfAttacks_ZeroModels()
        {
            var expected = 0;

            var attacker = new AttackerDTO()
            {
                NumberOfModels = 0,
                WeaponAttacks = 1
            };

            var actual = CombatMath.GetTotalNumberOfAttacks(attacker);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has a negative number of models.
        /// </summary>
        [TestMethod]
        public void GetTotalNumberOfAttacks_NegativeNumberOfModels()
        {
            var expected = 0;

            var attacker = new AttackerDTO()
            {
                NumberOfModels = -1,
                WeaponAttacks = 1
            };

            var actual = CombatMath.GetTotalNumberOfAttacks(attacker);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has 0 weapon attacks.
        /// </summary>
        [TestMethod]
        public void GetTotalNumberOfAttacks_ZeroWeaponAttacks()
        {
            var expected = 0;

            var attacker = new AttackerDTO()
            {
                NumberOfModels = 1,
                WeaponAttacks = 0
            };

            var actual = CombatMath.GetTotalNumberOfAttacks(attacker);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has a negative number of weapon attacks.
        /// </summary>
        [TestMethod]
        public void GetTotalNumberOfAttacks_NegativeWeaponAttacks()
        {
            var expected = 0;

            var attacker = new AttackerDTO()
            {
                NumberOfModels = 1,
                WeaponAttacks = -1
            };

            var actual = CombatMath.GetTotalNumberOfAttacks(attacker);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetTotalNumberOfAttacks_TestParams1()
        {
            var expected = 8;
            var actual = CombatMath.GetTotalNumberOfAttacks(ATTACKER_KHARN_THE_BETRAYER);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetTotalNumberOfAttacks_TestParams2()
        {
            var expected = 20;
            var actual = CombatMath.GetTotalNumberOfAttacks(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetTotalNumberOfAttacks_TestParams3()
        {
            var expected = 15;
            var actual = CombatMath.GetTotalNumberOfAttacks(ATTACKER_SPACE_MARINE_TERMINATOR_SQUAD);
            Assert.AreEqual(expected, actual);
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
        public void GetProbabilityOfHit_TestParams1()
        {
            var expected = 0.8333;
            var actual = Math.Round(CombatMath.GetProbabilityOfHit(ATTACKER_KHARN_THE_BETRAYER), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHit_TestParams2()
        {
            var expected = 0.6667;
            var actual = Math.Round(CombatMath.GetProbabilityOfHit(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHit_TestParams3()
        {
            var expected = 0.5;
            var actual = Math.Round(CombatMath.GetProbabilityOfHit(ATTACKER_SPACE_MARINE_TERMINATOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
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
            var expected = 6.6667;
            var actual = Math.Round(CombatMath.GetMeanHits(ATTACKER_KHARN_THE_BETRAYER), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanHits_TestParams2()
        {
            var expected = 13.3333;
            var actual = Math.Round(CombatMath.GetMeanHits(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanHits_TestParams3()
        {
            var expected = 7.5;
            var actual = Math.Round(CombatMath.GetMeanHits(ATTACKER_SPACE_MARINE_TERMINATOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
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
            var expected = 6;
            var actual = CombatMath.GetExpectedHits(ATTACKER_KHARN_THE_BETRAYER);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedHits_TestParams2()
        {
            var expected = 13;
            var actual = CombatMath.GetExpectedHits(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedHits_TestParams3()
        {
            var expected = 7;
            var actual = CombatMath.GetExpectedHits(ATTACKER_SPACE_MARINE_TERMINATOR_SQUAD);

            Assert.AreEqual(expected, actual);
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
            var expected = 1.0541;
            var actual = Math.Round(CombatMath.GetStandardDeviationHits(ATTACKER_KHARN_THE_BETRAYER), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationHits_TestParams2()
        {
            var expected = 2.1082;
            var actual = Math.Round(CombatMath.GetStandardDeviationHits(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationHits_TestParams3()
        {
            var expected = 1.9365;
            var actual = Math.Round(CombatMath.GetStandardDeviationHits(ATTACKER_SPACE_MARINE_TERMINATOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker parameter is null.
        /// </summary>
        [TestMethod]
        public void GetBinomialDistributionHits_AttackerIsNull()
        {
            var expected = new List<BinomialOutcome>();
            var actual = CombatMath.GetBinomialDistributionHits(null);

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
        public void GetBinomialDistributionHits_TestParams1()
        {
            var expected = new List<BinomialOutcome>
            {
                new(0, 0),
                new(1, 0),
                new(2, 0.0004),
                new(3, 0.0042),
                new(4, 0.0260),
                new(5, 0.1042),
                new(6, 0.2605),
                new(7, 0.3721),
                new(8, 0.2326)
            };

            var actual = CombatMath.GetBinomialDistributionHits(ATTACKER_KHARN_THE_BETRAYER);

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
        public void GetBinomialDistributionHits_TestParams2()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0),
                new(1, 0),
                new(2, 0),
                new(3, 0),
                new(4, 0),
                new(5, 0.0001),
                new(6, 0.0007),
                new(7, 0.0028),
                new(8, 0.0092),
                new(9, 0.0247),
                new(10, 0.0543),
                new(11, 0.0987),
                new(12, 0.148),
                new(13, 0.1821),
                new(14, 0.1821),
                new(15, 0.1457),
                new(16, 0.0911),
                new(17, 0.0429),
                new(18, 0.0143),
                new(19, 0.003),
                new(20, 0.0003)
            };

            var actual = CombatMath.GetBinomialDistributionHits(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetBinomialDistributionHits_TestParams3()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0),
                new(1, 0.0005),
                new(2, 0.0032),
                new(3, 0.0139),
                new(4, 0.0417),
                new(5, 0.0916),
                new(6, 0.1527),
                new(7, 0.1964),
                new(8, 0.1964),
                new(9, 0.1527),
                new(10, 0.0916),
                new(11, 0.0417),
                new(12, 0.0139),
                new(13, 0.0032),
                new(14, 0.0005),
                new(15, 0)
            };

            var actual = CombatMath.GetBinomialDistributionHits(ATTACKER_SPACE_MARINE_TERMINATOR_SQUAD);

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
        /// Tests the case where the attacker parameter is null.
        /// </summary>
        [TestMethod]
        public void GetSurvivorDistributionHits_AttackerIsNull()
        {
            var expected = new List<BinomialOutcome>();
            var actual = CombatMath.GetSurvivorDistributionHits(null);

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
        public void GetSurvivorDistributionHits_TestParams1()
        {
            var attacker = new AttackerDTO()
            {
                NumberOfModels = 1,
                WeaponAttacks = 1,
                WeaponSkill = 4
            };

            var expected = new List<BinomialOutcome>
            {
                new(0, 1),
                new(1, 0.5)
            };

            var actual = CombatMath.GetSurvivorDistributionHits(attacker);

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
        public void GetSurvivorDistributionHits_TestParams2()
        {
            var attacker = new AttackerDTO()
            {
                NumberOfModels = 1,
                WeaponAttacks = 3,
                WeaponSkill = 4
            };

            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.875),
                new(2, 0.5),
                new(3, 0.125)
            };

            var actual = CombatMath.GetSurvivorDistributionHits(attacker);

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
        public void GetSurvivorDistributionHits_TestParams3()
        {
            var attacker = new AttackerDTO()
            {
                NumberOfModels = 2,
                WeaponAttacks = 2,
                WeaponSkill = 4
            };

            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.9375),
                new(2, 0.6875),
                new(3, 0.3125),
                new(4, 0.0625)
            };

            var actual = CombatMath.GetSurvivorDistributionHits(attacker);

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
        /// Tests the case where the attacker is null.
        /// </summary>
        [TestMethod]
        public void GetSuccessThresholdOfWound_AttackerIsNull()
        {
            var expected = 7;

            var defender = new DefenderDTO()
            {
                Toughness = 4
            };

            var actual = CombatMath.GetSuccessThresholdOfWound(null, defender);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the defender is null.
        /// </summary>
        [TestMethod]
        public void GetSuccessThresholdOfWound_DefenderIsNull()
        {
            var expected = 7;

            var attacker = new AttackerDTO()
            {
                WeaponStrength = 4
            };

            var actual = CombatMath.GetSuccessThresholdOfWound(attacker, null);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case with given parameters.
        /// </summary>
        [TestMethod]
        public void GetSuccessThresholdOfWound_TestParams1()
        {
            var expected = 4;

            var attacker = new AttackerDTO()
            {
                WeaponStrength = 4
            };

            var defender = new DefenderDTO()
            {
                Toughness = 4
            };

            var actual = CombatMath.GetSuccessThresholdOfWound(attacker, defender);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case with given parameters.
        /// </summary>
        [TestMethod]
        public void GetSuccessThresholdOfWound_TestParams2()
        {
            var expected = 5;

            var attacker = new AttackerDTO()
            {
                WeaponStrength = 4
            };

            var defender = new DefenderDTO()
            {
                Toughness = 5
            };

            var actual = CombatMath.GetSuccessThresholdOfWound(attacker, defender);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case with given parameters.
        /// </summary>
        [TestMethod]
        public void GetSuccessThresholdOfWound_TestParams3()
        {
            var expected = 2;

            var attacker = new AttackerDTO()
            {
                WeaponStrength = 4
            };

            var defender = new DefenderDTO()
            {
                Toughness = 2
            };

            var actual = CombatMath.GetSuccessThresholdOfWound(attacker, defender);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker is null.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfWound_AttackerIsNull()
        {
            var expected = 0;

            var defender = new DefenderDTO();
            var actual = CombatMath.GetProbabilityOfWound(null, defender);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the defender is null.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfWound_DefenderIsNull()
        {
            var expected = 0;

            var attacker = new AttackerDTO();
            var actual = CombatMath.GetProbabilityOfWound(attacker, null);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has a negative number of models.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfWound_TestParams1()
        {
            var expected = 0.25;

            var attacker = new AttackerDTO()
            {
                NumberOfModels = 1,
                WeaponSkill = 4,
                WeaponStrength = 4
            };

            var defender = new DefenderDTO()
            {
                Toughness = 4
            };

            var actual = CombatMath.GetProbabilityOfWound(attacker, defender);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfWound_TestParams2()
        {
            var expected = 0.3333;

            var attacker = new AttackerDTO()
            {
                NumberOfModels = 1,
                WeaponSkill = 3,
                WeaponStrength = 4
            };

            var defender = new DefenderDTO()
            {
                Toughness = 4
            };

            var actual = Math.Round(CombatMath.GetProbabilityOfWound(attacker, defender), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfWound_TestParams3()
        {
            var expected = 0.5556;

            var attacker = new AttackerDTO()
            {
                NumberOfModels = 1,
                WeaponSkill = 2,
                WeaponStrength = 5
            };

            var defender = new DefenderDTO()
            {
                Toughness = 4
            };

            var actual = Math.Round(CombatMath.GetProbabilityOfWound(attacker, defender), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker parameter is null.
        /// </summary>
        [TestMethod]
        public void GetMeanWounds_AttackerIsNull()
        {
            var expected = 0;

            var defender = new DefenderDTO();
            var actual = CombatMath.GetMeanWounds(null, defender);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the defender parameter is null.
        /// </summary>
        [TestMethod]
        public void GetMeanWounds_DefenderIsNull()
        {
            var expected = 0;

            var attacker = new AttackerDTO();
            var actual = CombatMath.GetMeanWounds(attacker, null);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanWounds_TestParams1()
        {
            var expected = 4.4444;
            var actual = Math.Round(CombatMath.GetMeanWounds(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanWounds_TestParams2()
        {
            var expected = 6.6667;
            var actual = Math.Round(CombatMath.GetMeanWounds(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanWounds_TestParams3()
        {
            var expected = 4.4444;
            var actual = Math.Round(CombatMath.GetMeanWounds(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /*

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
        public void GetBinomialDistributionHits_AttackerIsNull()
        {
            var expected = new List<BinomialOutcome>();
            var actual = CombatMath.GetBinomialDistributionHits(null);

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
        public void GetBinomialDistributionHits_TestParams1()
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

            var actual = CombatMath.GetBinomialDistributionHits(attacker);

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
        public void GetBinomialDistributionHits_TestParams2()
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

            var actual = CombatMath.GetBinomialDistributionHits(attacker);

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
        public void GetBinomialDistributionHits_TestParams3()
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

            var actual = CombatMath.GetBinomialDistributionHits(attacker);

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
        /// Tests the case where the attacker parameter is null.
        /// </summary>
        [TestMethod]
        public void GetSurvivorDistributionHits_AttackerIsNull()
        {
            var expected = new List<BinomialOutcome>();
            var actual = CombatMath.GetSurvivorDistributionHits(null);

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
        public void GetSurvivorDistributionHits_TestParams1()
        {
            var attacker = new AttackerDTO()
            {
                NumberOfModels = 1,
                WeaponAttacks = 1,
                WeaponSkill = 4
            };

            var expected = new List<BinomialOutcome>
            {
                new(0, 1),
                new(1, 0.5)
            };

            var actual = CombatMath.GetSurvivorDistributionHits(attacker);

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
        public void GetSurvivorDistributionHits_TestParams2()
        {
            var attacker = new AttackerDTO()
            {
                NumberOfModels = 1,
                WeaponAttacks = 3,
                WeaponSkill = 4
            };

            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.875),
                new(2, 0.5),
                new(3, 0.125)
            };

            var actual = CombatMath.GetSurvivorDistributionHits(attacker);

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
        public void GetSurvivorDistributionHits_TestParams3()
        {
            var attacker = new AttackerDTO()
            {
                NumberOfModels = 2,
                WeaponAttacks = 2,
                WeaponSkill = 4
            };

            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.9375),
                new(2, 0.6875),
                new(3, 0.3125),
                new(4, 0.0625)
            };

            var actual = CombatMath.GetSurvivorDistributionHits(attacker);

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

        */

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

    #endregion
}
