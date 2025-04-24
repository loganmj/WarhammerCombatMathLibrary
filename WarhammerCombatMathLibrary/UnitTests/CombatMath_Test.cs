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

        /// <summary>
        /// Defense profile for a 5 man squad of Leagues of Votann Cthonian Beserks
        /// </summary>
        public static readonly DefenderDTO DEFENDER_VOTANN_CTHONIAN_BESERKS = new()
        {
            NumberOfModels = 5,
            Toughness = 5,
            ArmorSave = 6,
            InvulnerableSave = 7,
            FeelNoPain = 5,
            Wounds = 2
        };

        /// <summary>
        /// Defense profile for Mortarian, primarch of the Death Guard
        /// </summary>
        public static readonly DefenderDTO DEFENDER_DEATH_GUARD_MORTARION = new()
        {
            NumberOfModels = 1,
            Toughness = 12,
            ArmorSave = 2,
            InvulnerableSave = 4,
            FeelNoPain = 5,
            Wounds = 16
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
        /// Tests the case where the provided success threshold is 0.
        /// </summary>
        [TestMethod]
        public void GetNumberOfSuccessfulResults_SuccessThreshold0()
        {
            Assert.AreEqual(0, CombatMath.GetNumberOfSuccessfulResults(0));
        }

        /// <summary>
        /// Tests the case where the provided success threshold is lower than 2.
        /// </summary>
        [TestMethod]
        public void GetNumberOfSuccessfulResults_SuccessThresholdLessThan0()
        {
            Assert.AreEqual(0, CombatMath.GetNumberOfSuccessfulResults(-1));
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetNumberOfSuccessfulResults_TestParams1()
        {
            Assert.AreEqual(3, CombatMath.GetNumberOfSuccessfulResults(4));
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetNumberOfSuccessfulResults_TestParams2()
        {
            Assert.AreEqual(2, CombatMath.GetNumberOfSuccessfulResults(5));
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetNumberOfSuccessfulResults_TestParams3()
        {
            Assert.AreEqual(5, CombatMath.GetNumberOfSuccessfulResults(1));
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

            Assert.AreEqual(0, CombatMath.GetProbabilityOfHit(attacker));
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

            Assert.AreEqual(0, CombatMath.GetProbabilityOfHit(attacker));
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
            var expected = new List<BinomialOutcome>
            {
                new(0, 1),
                new(1, 1),
                new(2, 1),
                new(3, 0.9996),
                new(4, 0.9954),
                new(5, 0.9693),
                new(6, 0.8652),
                new(7, 0.6047),
                new(8, 0.2326)
            };

            var actual = CombatMath.GetSurvivorDistributionHits(ATTACKER_KHARN_THE_BETRAYER);

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
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 1),
                new(2, 1),
                new(3, 1),
                new(4, 1),
                new(5, 1),
                new(6, 0.9998),
                new(7, 0.9991),
                new(8, 0.9963),
                new(9, 0.9870),
                new(10, 0.9624),
                new(11, 0.9081),
                new(12, 0.8095),
                new(13, 0.6615),
                new(14, 0.4793),
                new(15, 0.2972),
                new(16, 0.1515),
                new(17, 0.0604),
                new(18, 0.0176),
                new(19, 0.0033),
                new(20, 0.0003)
            };

            var actual = CombatMath.GetSurvivorDistributionHits(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
            var expected = new List<BinomialOutcome>
            {
                new(0, 1),
                new(1, 1),
                new(2, 0.9995),
                new(3, 0.9963),
                new(4, 0.9824),
                new(5, 0.9408),
                new(6, 0.8491),
                new(7, 0.6964),
                new(8, 0.5),
                new(9, 0.3036),
                new(10, 0.1509),
                new(11, 0.0592),
                new(12, 0.0176),
                new(13, 0.0037),
                new(14, 0.0005),
                new(15, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionHits(ATTACKER_SPACE_MARINE_TERMINATOR_SQUAD);

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

            var actual = CombatMath.GetSuccessThresholdOfWound(null, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the defender is null.
        /// </summary>
        [TestMethod]
        public void GetSuccessThresholdOfWound_DefenderIsNull()
        {
            var expected = 7;

            var actual = CombatMath.GetSuccessThresholdOfWound(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, null);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case with given parameters.
        /// </summary>
        [TestMethod]
        public void GetSuccessThresholdOfWound_TestParams1()
        {
            var expected = 3;
            var actual = CombatMath.GetSuccessThresholdOfWound(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case with given parameters.
        /// </summary>
        [TestMethod]
        public void GetSuccessThresholdOfWound_TestParams2()
        {
            var expected = 4;
            var actual = CombatMath.GetSuccessThresholdOfWound(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case with given parameters.
        /// </summary>
        [TestMethod]
        public void GetSuccessThresholdOfWound_TestParams3()
        {
            var expected = 5;
            var actual = CombatMath.GetSuccessThresholdOfWound(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);

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
            var actual = CombatMath.GetProbabilityWound(null, defender);

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
            var actual = CombatMath.GetProbabilityWound(attacker, null);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has a negative number of models.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfWound_TestParams1()
        {
            var expected = 0.5556;
            var actual = Math.Round(CombatMath.GetProbabilityWound(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfWound_TestParams2()
        {
            var expected = 0.3333;
            var actual = Math.Round(CombatMath.GetProbabilityWound(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfWound_TestParams3()
        {
            var expected = 0.2222;
            var actual = Math.Round(CombatMath.GetProbabilityWound(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD), 4);

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

        /// <summary>
        /// Tests the case where the attacker parameter is null.
        /// </summary>
        [TestMethod]
        public void GetExpectedWounds_AttackerIsNull()
        {
            var expected = 0;
            var actual = CombatMath.GetExpectedWounds(null, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the defender parameter is null.
        /// </summary>
        [TestMethod]
        public void GetExpectedWounds_DefenderIsNull()
        {
            var expected = 0;
            var actual = CombatMath.GetExpectedWounds(ATTACKER_KHARN_THE_BETRAYER, null);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedWounds_TestParams1()
        {
            var expected = 4;
            var actual = CombatMath.GetExpectedWounds(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedWounds_TestParams2()
        {
            var expected = 6;
            var actual = CombatMath.GetExpectedWounds(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedWounds_TestParams3()
        {
            var expected = 4;
            var actual = CombatMath.GetExpectedWounds(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker parameter is null.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationWounds_AttackerIsNull()
        {
            var expected = 0;
            var actual = CombatMath.GetStandardDeviationWounds(null, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the defender parameter is null.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationWounds_DefenderIsNull()
        {
            var expected = 0;
            var actual = CombatMath.GetStandardDeviationWounds(ATTACKER_KHARN_THE_BETRAYER, null);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationWounds_TestParams1()
        {
            var expected = 1.4055;
            var actual = Math.Round(CombatMath.GetStandardDeviationWounds(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationWounds_TestParams2()
        {
            var expected = 2.1082;
            var actual = Math.Round(CombatMath.GetStandardDeviationWounds(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationWounds_TestParams3()
        {
            var expected = 1.8592;
            var actual = Math.Round(CombatMath.GetStandardDeviationWounds(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker parameter is null.
        /// </summary>
        [TestMethod]
        public void GetBinomialDistributionWounds_AttackerIsNull()
        {
            var expected = new List<BinomialOutcome>();
            var actual = CombatMath.GetBinomialDistributionWounds(null, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        /// Tests the case where the defender parameter is null.
        /// </summary>
        [TestMethod]
        public void GetBinomialDistributionWounds_DefenderIsNull()
        {
            var expected = new List<BinomialOutcome>();
            var actual = CombatMath.GetBinomialDistributionWounds(ATTACKER_KHARN_THE_BETRAYER, null);

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
        public void GetBinomialDistributionWounds_TestParams1()
        {
            var expected = new List<BinomialOutcome>
            {
                new(0, 0.0015),
                new(1, 0.0152),
                new(2, 0.0666),
                new(3, 0.1665),
                new(4, 0.2602),
                new(5, 0.2602),
                new(6, 0.1626),
                new(7, 0.0581),
                new(8, 0.0091)
            };

            var actual = CombatMath.GetBinomialDistributionWounds(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetBinomialDistributionWounds_TestParams2()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.0003),
                new(1, 0.003),
                new(2, 0.0143),
                new(3, 0.0429),
                new(4, 0.0911),
                new(5, 0.1457),
                new(6, 0.1821),
                new(7, 0.1821),
                new(8, 0.1480),
                new(9, 0.0987),
                new(10, 0.0543),
                new(11, 0.0247),
                new(12, 0.0092),
                new(13, 0.0028),
                new(14, 0.0007),
                new(15, 0.0001),
                new(16, 0),
                new(17, 0),
                new(18, 0),
                new(19, 0),
                new(20, 0)
            };

            var actual = CombatMath.GetBinomialDistributionWounds(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetBinomialDistributionWounds_TestParams3()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.0066),
                new(1, 0.0375),
                new(2, 0.1018),
                new(3, 0.1745),
                new(4, 0.2119),
                new(5, 0.1937),
                new(6, 0.1384),
                new(7, 0.0791),
                new(8, 0.0367),
                new(9, 0.0140),
                new(10, 0.0044),
                new(11, 0.0011),
                new(12, 0.0002),
                new(13, 0),
                new(14, 0),
                new(15, 0),
                new(16, 0),
                new(17, 0),
                new(18, 0),
                new(19, 0),
                new(20, 0)
            };

            var actual = CombatMath.GetBinomialDistributionWounds(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);

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
        public void GetSurvivorDistributionWounds_AttackerIsNull()
        {
            var expected = new List<BinomialOutcome>();
            var actual = CombatMath.GetSurvivorDistributionWounds(null, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        /// Tests the case where the defender parameter is null.
        /// </summary>
        [TestMethod]
        public void GetSurvivorDistributionWounds_DefenderIsNull()
        {
            var expected = new List<BinomialOutcome>();
            var actual = CombatMath.GetSurvivorDistributionWounds(ATTACKER_KHARN_THE_BETRAYER, null);

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
        public void GetSurvivorDistributionWounds_TestParams1()
        {
            var expected = new List<BinomialOutcome>
            {
                new(0, 1),
                new(1, 0.9985),
                new(2, 0.9833),
                new(3, 0.9166),
                new(4, 0.7501),
                new(5, 0.4899),
                new(6, 0.2298),
                new(7, 0.0672),
                new(8, 0.0091)
            };

            var actual = CombatMath.GetSurvivorDistributionWounds(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetSurvivorDistributionWounds_TestParams2()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.9997),
                new(2, 0.9967),
                new(3, 0.9824),
                new(4, 0.9396),
                new(5, 0.8485),
                new(6, 0.7028),
                new(7, 0.5207),
                new(8, 0.3385),
                new(9, 0.1905),
                new(10, 0.0919),
                new(11, 0.0376),
                new(12, 0.0130),
                new(13, 0.0037),
                new(14, 0.0009),
                new(15, 0.0002),
                new(16, 0),
                new(17, 0),
                new(18, 0),
                new(19, 0),
                new(20, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionWounds(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetSurvivorDistributionWounds_TestParams3()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.9934),
                new(2, 0.9559),
                new(3, 0.8541),
                new(4, 0.6796),
                new(5, 0.4677),
                new(6, 0.2740),
                new(7, 0.1356),
                new(8, 0.0565),
                new(9, 0.0198),
                new(10, 0.0058),
                new(11, 0.0014),
                new(12, 0.0003),
                new(13, 0),
                new(14, 0),
                new(15, 0),
                new(16, 0),
                new(17, 0),
                new(18, 0),
                new(19, 0),
                new(20, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionWounds(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);

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
            var expected = 0;
            var actual = CombatMath.GetAdjustedArmorSave(null, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetAdjustedArmorSave_NullDefender()
        {
            var expected = 0;
            var actual = CombatMath.GetAdjustedArmorSave(ATTACKER_KHARN_THE_BETRAYER, null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetAdjustedArmorSave_TestParams1()
        {
            var expected = 5;
            var actual = CombatMath.GetAdjustedArmorSave(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetAdjustedArmorSave_TestParams2()
        {
            var expected = 4;
            var actual = CombatMath.GetAdjustedArmorSave(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetAdjustedArmorSave_TestParams3()
        {
            var expected = 3;
            var actual = CombatMath.GetAdjustedArmorSave(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the attacker object is null
        /// </summary>
        [TestMethod]
        public void GetProbabilityFailedSave_NullAttacker()
        {
            var expected = 0;
            var actual = CombatMath.GetProbabilityFailedSave(null, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetProbabilityFailedSave_NullDefender()
        {
            var expected = 0;
            var actual = CombatMath.GetProbabilityFailedSave(ATTACKER_KHARN_THE_BETRAYER, null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetProbabilityFailedSave_TestParams1()
        {
            var expected = 0.3704;
            var actual = Math.Round(CombatMath.GetProbabilityFailedSave(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetProbabilityFailedSave_TestParams2()
        {
            var expected = 0.1667;
            var actual = Math.Round(CombatMath.GetProbabilityFailedSave(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetProbabilityFailedSave_TestParams3()
        {
            var expected = 0.0741;
            var actual = Math.Round(CombatMath.GetProbabilityFailedSave(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the attacker object is null
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_NullAttacker()
        {
            var expected = 0;
            var actual = CombatMath.GetMeanFailedSaves(null, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_NullDefender()
        {
            var expected = 0;
            var actual = CombatMath.GetMeanFailedSaves(ATTACKER_KHARN_THE_BETRAYER, null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_TestParams1()
        {
            var expected = 2.963;
            var actual = Math.Round(CombatMath.GetMeanFailedSaves(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_TestParams2()
        {
            var expected = 3.3333;
            var actual = Math.Round(CombatMath.GetMeanFailedSaves(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_TestParams3()
        {
            var expected = 1.4815;
            var actual = Math.Round(CombatMath.GetMeanFailedSaves(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the attacker object is null
        /// </summary>
        [TestMethod]
        public void GetExpectedFailedSaves_NullAttacker()
        {
            var expected = 0;
            var actual = CombatMath.GetExpectedFailedSaves(null, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetExpectedFailedSaves_NullDefender()
        {
            var expected = 0;
            var actual = CombatMath.GetExpectedFailedSaves(ATTACKER_KHARN_THE_BETRAYER, null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedFailedSaves_TestParams1()
        {
            var expected = 2;
            var actual = CombatMath.GetExpectedFailedSaves(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedFailedSaves_TestParams2()
        {
            var expected = 3;
            var actual = CombatMath.GetExpectedFailedSaves(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedFailedSaves_TestParams3()
        {
            var expected = 1;
            var actual = CombatMath.GetExpectedFailedSaves(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the attacker object is null
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationFailedSaves_NullAttacker()
        {
            var expected = 0;
            var actual = CombatMath.GetStandardDeviationFailedSaves(null, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationFailedSaves_NullDefender()
        {
            var expected = 0;
            var actual = CombatMath.GetStandardDeviationFailedSaves(ATTACKER_KHARN_THE_BETRAYER, null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationFailedSaves_TestParams1()
        {
            var expected = 1.3659;
            var actual = Math.Round(CombatMath.GetStandardDeviationFailedSaves(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationFailedSaves_TestParams2()
        {
            var expected = 1.6667;
            var actual = Math.Round(CombatMath.GetStandardDeviationFailedSaves(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationFailedSaves_TestParams3()
        {
            var expected = 1.1712;
            var actual = Math.Round(CombatMath.GetStandardDeviationFailedSaves(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker parameter is null.
        /// </summary>
        [TestMethod]
        public void GetBinomialDistributionFailedSaves_AttackerIsNull()
        {
            var expected = new List<BinomialOutcome>();
            var actual = CombatMath.GetBinomialDistributionFailedSaves(null, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        /// Tests the case where the defender parameter is null.
        /// </summary>
        [TestMethod]
        public void GetBinomialDistributionFailedSaves_DefenderIsNull()
        {
            var expected = new List<BinomialOutcome>();
            var actual = CombatMath.GetBinomialDistributionFailedSaves(ATTACKER_KHARN_THE_BETRAYER, null);

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
        public void GetBinomialDistributionFailedSaves_TestParams1()
        {
            var expected = new List<BinomialOutcome>
            {
                new(0, 0.0247),
                new(1, 0.1162),
                new(2, 0.2393),
                new(3, 0.2815),
                new(4, 0.2070),
                new(5, 0.0974),
                new(6, 0.0287),
                new(7, 0.0048),
                new(8, 0.0004)
            };

            var actual = CombatMath.GetBinomialDistributionFailedSaves(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetBinomialDistributionFailedSaves_TestParams2()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.0261),
                new(1, 0.1043),
                new(2, 0.1982),
                new(3, 0.2379),
                new(4, 0.2022),
                new(5, 0.1294),
                new(6, 0.0647),
                new(7, 0.0259),
                new(8, 0.0084),
                new(9, 0.0022),
                new(10, 0.0005),
                new(11, 0.0001),
                new(12, 0),
                new(13, 0),
                new(14, 0),
                new(15, 0),
                new(16, 0),
                new(17, 0),
                new(18, 0),
                new(19, 0),
                new(20, 0)
            };

            var actual = CombatMath.GetBinomialDistributionFailedSaves(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetBinomialDistributionFailedSaves_TestParams3()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.2145),
                new(1, 0.3433),
                new(2, 0.2609),
                new(3, 0.1252),
                new(4, 0.0426),
                new(5, 0.0109),
                new(6, 0.0022),
                new(7, 0.0003),
                new(8, 0),
                new(9, 0),
                new(10, 0),
                new(11, 0),
                new(12, 0),
                new(13, 0),
                new(14, 0),
                new(15, 0),
                new(16, 0),
                new(17, 0),
                new(18, 0),
                new(19, 0),
                new(20, 0)
            };

            var actual = CombatMath.GetBinomialDistributionFailedSaves(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);

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
        public void GetSurvivorDistributionFailedSaves_AttackerIsNull()
        {
            var expected = new List<BinomialOutcome>();
            var actual = CombatMath.GetSurvivorDistributionFailedSaves(null, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        /// Tests the case where the defender parameter is null.
        /// </summary>
        [TestMethod]
        public void GetSurvivorDistributionFailedSaves_DefenderIsNull()
        {
            var expected = new List<BinomialOutcome>();
            var actual = CombatMath.GetSurvivorDistributionFailedSaves(ATTACKER_KHARN_THE_BETRAYER, null);

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
        public void GetSurvivorDistributionFailedSaves_TestParams1()
        {
            var expected = new List<BinomialOutcome>
            {
                new(0, 1),
                new(1, 0.9753),
                new(2, 0.8591),
                new(3, 0.6198),
                new(4, 0.3382),
                new(5, 0.1312),
                new(6, 0.0338),
                new(7, 0.0052),
                new(8, 0.0004)
            };

            var actual = CombatMath.GetSurvivorDistributionFailedSaves(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetSurvivorDistributionFailedSaves_TestParams2()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.9739),
                new(2, 0.8696),
                new(3, 0.6713),
                new(4, 0.4335),
                new(5, 0.2313),
                new(6, 0.1018),
                new(7, 0.0371),
                new(8, 0.0113),
                new(9, 0.0028),
                new(10, 0.0006),
                new(11, 0.0001),
                new(12, 0),
                new(13, 0),
                new(14, 0),
                new(15, 0),
                new(16, 0),
                new(17, 0),
                new(18, 0),
                new(19, 0),
                new(20, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionFailedSaves(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetSurvivorDistributionFailedSaves_TestParams3()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.7855),
                new(2, 0.4422),
                new(3, 0.1813),
                new(4, 0.0561),
                new(5, 0.0135),
                new(6, 0.0026),
                new(7, 0.0004),
                new(8, 0.0001),
                new(9, 0),
                new(10, 0),
                new(11, 0),
                new(12, 0),
                new(13, 0),
                new(14, 0),
                new(15, 0),
                new(16, 0),
                new(17, 0),
                new(18, 0),
                new(19, 0),
                new(20, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionFailedSaves(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);

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
        public void GetMeanDamageGross_NullAttacker()
        {
            var expected = 0;
            var actual = CombatMath.GetMeanDamageGross(null, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetMeanDamageGross_NullDefender()
        {
            var expected = 0;
            var actual = CombatMath.GetMeanDamageGross(ATTACKER_KHARN_THE_BETRAYER, null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDamageGross_TestParams1()
        {
            var expected = 8.8889;
            var actual = Math.Round(CombatMath.GetMeanDamageGross(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDamageGross_TestParams2()
        {
            var expected = 3.3333;
            var actual = Math.Round(CombatMath.GetMeanDamageGross(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDamageGross_TestParams3()
        {
            var expected = 1.4815;
            var actual = Math.Round(CombatMath.GetMeanDamageGross(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the attacker object is null
        /// </summary>
        [TestMethod]
        public void GetExpectedDamageGross_NullAttacker()
        {
            var expected = 0;
            var actual = CombatMath.GetExpectedDamageGross(null, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetExpectedDamageGross_NullDefender()
        {
            var expected = 0;
            var actual = CombatMath.GetExpectedDamageGross(ATTACKER_KHARN_THE_BETRAYER, null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDamageGross_TestParams1()
        {
            var expected = 8;
            var actual = CombatMath.GetExpectedDamageGross(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDamageGross_TestParams2()
        {
            var expected = 3;
            var actual = CombatMath.GetExpectedDamageGross(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDamageGross_TestParams3()
        {
            var expected = 1;
            var actual = CombatMath.GetExpectedDamageGross(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the attacker object is null
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamageGross_NullAttacker()
        {
            var expected = 0;
            var actual = CombatMath.GetStandardDeviationDamageGross(null, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamageGross_NullDefender()
        {
            var expected = 0;
            var actual = CombatMath.GetStandardDeviationDamageGross(ATTACKER_KHARN_THE_BETRAYER, null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamageGross_TestParams1()
        {
            var expected = 4.0976;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamageGross(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamageGross_TestParams2()
        {
            var expected = 1.6667;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamageGross(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamageGross_TestParams3()
        {
            var expected = 1.1712;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamageGross(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the defender is null
        /// </summary>
        [TestMethod]
        public void GetAdjustedDamage_NullDefender() 
        {
            var expected = 0;
            var actual = CombatMath.GetAdjustedDamage(null, 5);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the input damage is less than or equal to 0.
        /// </summary>
        [TestMethod]
        public void GetAdjustedDamage_DamageIsLessThanOrEqualTo0() 
        {
            var expected = 0;
            var actual = CombatMath.GetAdjustedDamage(DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD, 0);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetAdjustedDamage_TestParams1()
        {
            var expected = 20;
            var actual = CombatMath.GetAdjustedDamage(DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD, 20);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetAdjustedDamage_TestParams2() 
        {
            var expected = 13.3333;
            var actual = Math.Round(CombatMath.GetAdjustedDamage(DEFENDER_VOTANN_CTHONIAN_BESERKS, 20), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetAdjustedDamage_TestParams3() 
        {
            var expected = 6.6667;
            var actual = Math.Round(CombatMath.GetAdjustedDamage(DEFENDER_DEATH_GUARD_MORTARION, 10), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the attacker object is null
        /// </summary>
        [TestMethod]
        public void GetModelsDestroyed_NullAttacker()
        {
            var expected = 0;
            var actual = CombatMath.GetModelsDestroyed(null, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD, 1);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetModelsDestroyed_NullDefender()
        {
            var expected = 0;
            var actual = CombatMath.GetModelsDestroyed(ATTACKER_KHARN_THE_BETRAYER, null, 1);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetModelsDestroyed_TestParams1()
        {
            var expected = 3;
            var totalDamage = 9;
            var actual = CombatMath.GetModelsDestroyed(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD, totalDamage);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetModelsDestroyed_TestParams2()
        {
            var expected = 2;
            var totalDamage = 4;
            var actual = CombatMath.GetModelsDestroyed(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD, totalDamage);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetModelsDestroyed_TestParams3()
        {
            var expected = 0;
            var totalDamage = 2;
            var actual = CombatMath.GetModelsDestroyed(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD, totalDamage);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the attacker object is null
        /// </summary>
        [TestMethod]
        public void GetExpectedDestroyedModels_NullAttacker()
        {
            var expected = 0;
            var actual = CombatMath.GetExpectedDestroyedModels(null, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetExpectedDestroyedModels_NullDefender()
        {
            var expected = 0;
            var actual = CombatMath.GetExpectedDestroyedModels(ATTACKER_KHARN_THE_BETRAYER, null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDestroyedModels_TestParams1()
        {
            var expected = 2;
            var actual = CombatMath.GetExpectedDestroyedModels(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDestroyedModels_TestParams2()
        {
            var expected = 1;
            var actual = CombatMath.GetExpectedDestroyedModels(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDestroyedModels_TestParams3()
        {
            var expected = 0;
            var actual = CombatMath.GetExpectedDestroyedModels(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the attacker object is null
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_NullAttacker()
        {
            var expected = 0;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(null, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_NullDefender()
        {
            var expected = 0;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_KHARN_THE_BETRAYER, null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_TestParams1()
        {
            var expected = 1;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_TestParams2()
        {
            var expected = 0;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_TestParams3()
        {
            var expected = 0;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the attacker object is null
        /// </summary>
        [TestMethod]
        public void GetAttacksRequiredToDestroyOneModel_NullAttacker()
        {
            var expected = 0;
            var actual = CombatMath.GetAttacksRequiredToDestroyOneModel(null, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetAttacksRequiredToDestroyOneModel_NullDefender()
        {
            var expected = 0;
            var actual = CombatMath.GetAttacksRequiredToDestroyOneModel(ATTACKER_KHARN_THE_BETRAYER, null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetAttacksRequiredToDestroyOneModel_TestParams1()
        {
            var expected = 1;
            var actual = CombatMath.GetAttacksRequiredToDestroyOneModel(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetAttacksRequiredToDestroyOneModel_TestParams2()
        {
            var expected = 2;
            var actual = CombatMath.GetAttacksRequiredToDestroyOneModel(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetAttacksRequiredToDestroyOneModel_TestParams3()
        {
            var expected = 3;
            var actual = CombatMath.GetAttacksRequiredToDestroyOneModel(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker parameter is null.
        /// </summary>
        [TestMethod]
        public void GetBinomialDistributionDestroyedModels_AttackerIsNull()
        {
            var expected = new List<BinomialOutcome>();
            var actual = CombatMath.GetBinomialDistributionDestroyedModels(null, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        /// Tests the case where the defender parameter is null.
        /// </summary>
        [TestMethod]
        public void GetBinomialDistributionDestroyedModels_DefenderIsNull()
        {
            var expected = new List<BinomialOutcome>();
            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_KHARN_THE_BETRAYER, null);

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
        public void GetBinomialDistributionDestroyedModels_TestParams1()
        {
            var expected = new List<BinomialOutcome>
                {
                    new(0, 0.0247),
                    new(1, 0.1162),
                    new(2, 0.2393),
                    new(3, 0.2815),
                    new(4, 0.2070),
                    new(5, 0.0974),
                    new(6, 0.0287),
                    new(7, 0.0048),
                    new(8, 0.0004)
                };

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetBinomialDistributionDestroyedModels_TestParams2()
        {
            var expected = new List<BinomialOutcome>()
                {
                    new(0, 0.0261),
                    new(1, 0.1982),
                    new(2, 0.2022),
                    new(3, 0.0647),
                    new(4, 0.0084),
                    new(5, 0.0005),
                    new(6, 0),
                    new(7, 0),
                    new(8, 0),
                    new(9, 0),
                    new(10, 0)
                };

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetBinomialDistributionDestroyedModels_TestParams3()
        {
            var expected = new List<BinomialOutcome>()
                {
                    new(0, 0.2145),
                    new(1, 0.1252),
                    new(2, 0.0022),
                    new(3, 0),
                    new(4, 0),
                    new(5, 0),
                    new(6, 0)
                };

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);

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
        public void GetSurvivorDistributionDestroyedModels_AttackerIsNull()
        {
            var expected = new List<BinomialOutcome>();
            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(null, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        /// Tests the case where the defender parameter is null.
        /// </summary>
        [TestMethod]
        public void GetSurvivorDistributionDestroyedModels_DefenderIsNull()
        {
            var expected = new List<BinomialOutcome>();
            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_KHARN_THE_BETRAYER, null);

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
        public void GetSurvivorDistributionDestroyedModels_TestParams1()
        {
            var expected = new List<BinomialOutcome>
                {
                    new(0, 1),
                    new(1, 0.9753),
                    new(2, 0.8591),
                    new(3, 0.6198),
                    new(4, 0.3382),
                    new(5, 0.1312),
                    new(6, 0.0338),
                    new(7, 0.0052),
                    new(8, 0.0004)
                };

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetSurvivorDistributionDestroyedModels_TestParams2()
        {
            var expected = new List<BinomialOutcome>()
                {
                    new(0, 1),
                    new(1, 0.8696),
                    new(2, 0.4335),
                    new(3, 0.1018),
                    new(4, 0.0113),
                    new(5, 0.0006),
                    new(6, 0),
                    new(7, 0),
                    new(8, 0),
                    new(9, 0),
                    new(10, 0)
                };

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetSurvivorDistributionDestroyedModels_TestParams3()
        {
            var expected = new List<BinomialOutcome>()
                {
                    new(0, 1),
                    new(1, 0.1813),
                    new(2, 0.0026),
                    new(3, 0),
                    new(4, 0),
                    new(5, 0),
                    new(6, 0)
                };

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);

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

        #endregion
    }
}
