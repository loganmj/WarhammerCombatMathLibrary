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
            WeaponFlatAttacks = 8,
            WeaponSkill = 2,
            WeaponStrength = 6,
            WeaponArmorPierce = 2,
            WeaponFlatDamage = 3
        };

        /// <summary>
        /// Attack profile for Kharn the Betrayer, blessed with both Lethal Hits and Devastating Wounds
        /// </summary>
        public static readonly AttackerDTO ATTACKER_KHARN_THE_BETRAYER_BLESSED = new()
        {
            NumberOfModels = 1,
            WeaponFlatAttacks = 8,
            WeaponSkill = 2,
            WeaponStrength = 6,
            WeaponArmorPierce = 2,
            WeaponFlatDamage = 3,
            WeaponHasLethalHits = true,
            WeaponHasDevastatingWounds = true
        };

        /// <summary>
        /// Attack profile for a 10 man squad of Space Marine Intercessors, all equipped with Bolt Rifles
        /// </summary>
        public static readonly AttackerDTO ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD = new()
        {
            NumberOfModels = 10,
            WeaponFlatAttacks = 2,
            WeaponSkill = 3,
            WeaponStrength = 4,
            WeaponArmorPierce = 1,
            WeaponFlatDamage = 1
        };

        /// <summary>
        /// Attack profile for a five man squad of Space Marine Terminators, all equipped with Chain Fists
        /// </summary>
        public static readonly AttackerDTO ATTACKER_SPACE_MARINE_TERMINATOR_SQUAD = new()
        {
            NumberOfModels = 5,
            WeaponFlatAttacks = 3,
            WeaponSkill = 4,
            WeaponStrength = 8,
            WeaponArmorPierce = 2,
            WeaponFlatDamage = 2
        };

        /// <summary>
        /// Attack profile for a World Eaters Forgefiend.
        /// </summary>
        public static readonly AttackerDTO ATTACKER_WORLD_EATERS_FORGEFIEND = new()
        {
            NumberOfModels = 1,
            WeaponNumberOfAttackDice = 3,
            WeaponAttackDiceType = DiceType.D3,
            WeaponFlatAttacks = 0,
            WeaponSkill = 3,
            WeaponStrength = 10,
            WeaponArmorPierce = 3,
            WeaponFlatDamage = 3
        };

        /// <summary>
        /// Attack profile for a unit of World Eaters Chaos Spawn.
        /// </summary>
        public static readonly AttackerDTO ATTACKER_WORLD_EATERS_CHAOS_SPAWN = new()
        {
            NumberOfModels = 2,
            WeaponNumberOfAttackDice = 1,
            WeaponAttackDiceType = DiceType.D6,
            WeaponFlatAttacks = 2,
            WeaponSkill = 4,
            WeaponStrength = 6,
            WeaponArmorPierce = 1,
            WeaponFlatDamage = 2
        };

        /// <summary>
        /// Attack profile for a Space Marine Infernus Squad.
        /// </summary>
        public static readonly AttackerDTO ATTACKER_SPACE_MARINE_INFERNUS_SQUAD = new()
        {
            NumberOfModels = 5,
            WeaponNumberOfAttackDice = 1,
            WeaponAttackDiceType = DiceType.D6,
            WeaponFlatAttacks = 0,
            WeaponSkill = 0,
            WeaponStrength = 5,
            WeaponArmorPierce = 1,
            WeaponFlatDamage = 1,
            WeaponHasTorrent = true
        };

        /// <summary>
        /// Attack profile for a World Eaters Jakhal Squad.
        /// </summary>
        public static readonly AttackerDTO ATTACKER_WORLD_EATERS_JAKHALS = new()
        {
            NumberOfModels = 10,
            WeaponNumberOfAttackDice = 0,
            WeaponAttackDiceType = 0,
            WeaponFlatAttacks = 3,
            WeaponSkill = 4,
            WeaponStrength = 3,
            WeaponArmorPierce = 0,
            WeaponFlatDamage = 1
        };

        /// <summary>
        /// Attack profile for a World Eaters Maulerfiend.
        /// </summary>
        public static readonly AttackerDTO ATTACKER_WORLD_EATERS_MAULERFIEND = new()
        {
            NumberOfModels = 1,
            WeaponNumberOfAttackDice = 0,
            WeaponAttackDiceType = 0,
            WeaponFlatAttacks = 8,
            WeaponSkill = 3,
            WeaponStrength = 14,
            WeaponArmorPierce = 2,
            WeaponNumberOfDamageDice = 1,
            WeaponDamageDiceType = DiceType.D6,
            WeaponFlatDamage = 1
        };

        /// <summary>
        /// Attack profile for an Adepta Sororitas Retributor Squad.
        /// </summary>
        public static readonly AttackerDTO ATTACKER_ADEPTA_SORORITAS_RETRIBUTOR_SQUAD = new()
        {
            NumberOfModels = 4,
            WeaponNumberOfAttackDice = 0,
            WeaponAttackDiceType = 0,
            WeaponFlatAttacks = 2,
            WeaponSkill = 4,
            WeaponStrength = 9,
            WeaponArmorPierce = 4,
            WeaponNumberOfDamageDice = 1,
            WeaponDamageDiceType = DiceType.D6,
            WeaponFlatDamage = 0
        };

        /// <summary>
        /// Attack profile for an Adepta Sororitas Celestian Sacresants.
        /// </summary>
        public static readonly AttackerDTO ATTACKER_ADEPTA_SORORITAS_CELESTIAN_SACRESANTS = new()
        {
            NumberOfModels = 5,
            WeaponFlatAttacks = 3,
            WeaponHasLethalHits = true,
            WeaponSkill = 3,
            WeaponStrength = 4,
            WeaponArmorPierce = 1,
            WeaponFlatDamage = 2
        };

        /// <summary>
        /// Attack profile for a Chaos Knight Abominant using its Volkite Combustor weapon.
        /// </summary>
        public static readonly AttackerDTO ATTACKER_CHAOS_KNIGHT_ABOMINANT = new()
        {
            NumberOfModels = 1,
            WeaponFlatAttacks = 9,
            WeaponSkill = 3,
            WeaponStrength = 12,
            WeaponArmorPierce = 0,
            WeaponFlatDamage = 3,
            WeaponHasDevastatingWounds = true,
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
        /// Defense profile for a unit of World Eaters Chaos Spawn
        /// </summary>
        public static readonly DefenderDTO DEFENDER_WORLD_EATERS_CHAOS_SPAWN = new()
        {
            NumberOfModels = 2,
            Toughness = 5,
            ArmorSave = 4,
            InvulnerableSave = 7,
            FeelNoPain = 5,
            Wounds = 4
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

        #region Unit Tests - GetProbabilityOfHit()

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
            var expected = 0;
            var attacker = new AttackerDTO()
            {
                WeaponSkill = -1
            };

            var actual = CombatMath.GetProbabilityOfHit(attacker);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker weapon skill is 1.
        /// Rolls of 1 are always considered a fail, so a weapon skill of 1+ should be treated as a 2+ instead.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHit_WeaponSkillValueOf1()
        {
            var expected = 0.8333;
            var attacker = new AttackerDTO()
            {
                WeaponSkill = 1
            };

            var actual = Math.Round(CombatMath.GetProbabilityOfHit(attacker), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker weapon has torrent.
        /// Torrent weapons automatically hit their target.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHit_WeaponHasTorrent()
        {
            var expected = 1;
            var attacker = new AttackerDTO()
            {
                WeaponHasTorrent = true
            };

            var actual = Math.Round(CombatMath.GetProbabilityOfHit(attacker), 4);

            Assert.AreEqual(expected, actual);
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

        #endregion

        #region Unit Tests - GetMeanHits()

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
        public void GetMeanHits_SingleModelAttacker()
        {
            var expected = 6.6667;
            var actual = Math.Round(CombatMath.GetMeanHits(ATTACKER_KHARN_THE_BETRAYER), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanHits_MultiModelAttacker()
        {
            var expected = 13.3333;
            var actual = Math.Round(CombatMath.GetMeanHits(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanHits_VariableAttacks_SingleModelAttacker()
        {
            var expected = 4;
            var actual = Math.Round(CombatMath.GetMeanHits(ATTACKER_WORLD_EATERS_FORGEFIEND), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanHits_VariableAttacks_MultiModelAttacker()
        {
            var expected = 6;
            var actual = Math.Round(CombatMath.GetMeanHits(ATTACKER_WORLD_EATERS_CHAOS_SPAWN), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker's weapon has torrent.
        /// </summary>
        [TestMethod]
        public void GetMeanHits_VariableAttacks_WeaponHasTorrent()
        {
            var expected = 20;
            var actual = Math.Round(CombatMath.GetMeanHits(ATTACKER_SPACE_MARINE_INFERNUS_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Unit Tests - GetExpectedHits()

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
        public void GetExpectedHits_SingleModelAttacker()
        {
            var expected = 6;
            var actual = CombatMath.GetExpectedHits(ATTACKER_KHARN_THE_BETRAYER);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedHits_MultiModelAttacker()
        {
            var expected = 13;
            var actual = CombatMath.GetExpectedHits(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedHits_VariableAttacks_SingleModelAttacker()
        {
            var expected = 4;
            var actual = CombatMath.GetExpectedHits(ATTACKER_WORLD_EATERS_FORGEFIEND);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedHits_VariableAttacks_MultiModelAttacker()
        {
            var expected = 6;
            var actual = CombatMath.GetExpectedHits(ATTACKER_WORLD_EATERS_CHAOS_SPAWN);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedHits_VariableAttacks_WeaponHasTorrent()
        {
            var expected = 20;
            var actual = CombatMath.GetExpectedHits(ATTACKER_SPACE_MARINE_INFERNUS_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Unit Tests - GetStandardDeviationHits()

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
        public void GetStandardDeviationHits_SingleModelAttacker()
        {
            var expected = 1.0541;
            var actual = Math.Round(CombatMath.GetStandardDeviationHits(ATTACKER_KHARN_THE_BETRAYER), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationHits_MultiModelAttacker()
        {
            var expected = 2.1082;
            var actual = Math.Round(CombatMath.GetStandardDeviationHits(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationHits_VariableAttacks_SingleModelAttacker()
        {
            var expected = 1.1547;
            var actual = Math.Round(CombatMath.GetStandardDeviationHits(ATTACKER_WORLD_EATERS_FORGEFIEND), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationHits_VariableAttacks_MultiModelAttacker()
        {
            var expected = 1.7321;
            var actual = Math.Round(CombatMath.GetStandardDeviationHits(ATTACKER_WORLD_EATERS_CHAOS_SPAWN), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the weapon has torrent.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationHits_VariableAttacks_WeaponHasTorrent()
        {
            var expected = 0;
            var actual = Math.Round(CombatMath.GetStandardDeviationHits(ATTACKER_SPACE_MARINE_INFERNUS_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Unit Tests - GetBinomialDistributionHits()

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
        public void GetBinomialDistributionHits_SingleModelAttacker()
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
        public void GetBinomialDistributionHits_MultiModelAttacker()
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
        public void GetBinomialDistributionHits_VariableAttacks_SingleModelAttacker()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.0079),
                new(1, 0.0555),
                new(2, 0.1501),
                new(3, 0.2101),
                new(4, 0.2309),
                new(5, 0.2361),
                new(6, 0.2097),
                new(7, 0.1496),
                new(8, 0.0780),
                new(9, 0.0260)
            };

            var actual = CombatMath.GetBinomialDistributionHits(ATTACKER_WORLD_EATERS_FORGEFIEND);

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
        public void GetBinomialDistributionHits_VariableAttacks_MultiModelAttacker()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.0028),
                new(1, 0.0199),
                new(2, 0.0623),
                new(3, 0.1182),
                new(4, 0.1575),
                new(5, 0.1659),
                new(6, 0.1516),
                new(7, 0.1371),
                new(8, 0.1111),
                new(9, 0.0786),
                new(10, 0.0475),
                new(11, 0.0239),
                new(12, 0.0098),
                new(13, 0.0032),
                new(14, 0.0008),
                new(15, 0.0001),
                new(16, 0)
            };

            var actual = CombatMath.GetBinomialDistributionHits(ATTACKER_WORLD_EATERS_CHAOS_SPAWN);

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
        public void GetBinomialDistributionHits_VariableAttacks_WeaponHasTorrent()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0),
                new(1, 0),
                new(2, 0),
                new(3, 0),
                new(4, 0),
                new(5, 0),
                new(6, 0),
                new(7, 0),
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
                new(20, 0),
                new(21, 0),
                new(22, 0),
                new(23, 0),
                new(24, 0),
                new(25, 0),
                new(26, 0),
                new(27, 0),
                new(28, 0),
                new(29, 0),
                new(30, 1)
            };

            var actual = CombatMath.GetBinomialDistributionHits(ATTACKER_SPACE_MARINE_INFERNUS_SQUAD);

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

        #region Unit Tests - GetSurvivorDistributionHits()

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
        public void GetSurvivorDistributionHits_SingleModelAttacker()
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
        public void GetSurvivorDistributionHits_MultiModelAttacker()
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
        public void GetSurvivorDistributionHits_VariableAttacks_SingleModelAttacker()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 1),
                new(2, 1),
                new(3, 1),
                new(4, 0.9303),
                new(5, 0.6994),
                new(6, 0.4633),
                new(7, 0.2536),
                new(8, 0.1040),
                new(9, 0.0260)
            };

            var actual = CombatMath.GetSurvivorDistributionHits(ATTACKER_WORLD_EATERS_FORGEFIEND);

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
        public void GetSurvivorDistributionHits_VariableAttacks_MultiModelAttacker()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 1),
                new(2, 1),
                new(3, 1),
                new(4, 0.8872),
                new(5, 0.7297),
                new(6, 0.5638),
                new(7, 0.4121),
                new(8, 0.2751),
                new(9, 0.1639),
                new(10, 0.0853),
                new(11, 0.0378),
                new(12, 0.0139),
                new(13, 0.0041),
                new(14, 0.0009),
                new(15, 0.0002),
                new(16, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionHits(ATTACKER_WORLD_EATERS_CHAOS_SPAWN);

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
        /// Tests the case where the weapon has torrent.
        /// </summary>
        [TestMethod]
        public void GetSurvivorDistributionHits_VariableAttacks_WeaponHasTorrent()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 1),
                new(2, 1),
                new(3, 1),
                new(4, 1),
                new(5, 1),
                new(6, 1),
                new(7, 1),
                new(8, 1),
                new(9, 1),
                new(10, 1),
                new(11, 1),
                new(12, 1),
                new(13, 1),
                new(14, 1),
                new(15, 1),
                new(16, 1),
                new(17, 1),
                new(18, 1),
                new(19, 1),
                new(20, 1),
                new(21, 1),
                new(22, 1),
                new(23, 1),
                new(24, 1),
                new(25, 1),
                new(26, 1),
                new(27, 1),
                new(28, 1),
                new(29, 1),
                new(30, 1)
            };

            var actual = CombatMath.GetSurvivorDistributionHits(ATTACKER_SPACE_MARINE_INFERNUS_SQUAD);

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

        #region Unit Tests - GetProbabilityOfHitAndWound()

        /// <summary>
        /// Tests the case where the attacker is null.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWound_AttackerIsNull()
        {
            var expected = 0;

            var defender = new DefenderDTO();
            var actual = CombatMath.GetProbabilityOfHitAndWound(null, defender);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the defender is null.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWound_DefenderIsNull()
        {
            var expected = 0;

            var attacker = new AttackerDTO();
            var actual = CombatMath.GetProbabilityOfHitAndWound(attacker, null);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case with given parameters.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWound_HigherStrength()
        {
            var expected = 0.5556;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWound(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWound_EqualStrengthAndToughness()
        {
            var expected = 0.3333;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWound(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWound_HigherToughness()
        {
            var expected = 0.2222;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWound(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWound_VariableAttacks_SingleModelAttacker()
        {
            var expected = 0.5556;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWound(ATTACKER_WORLD_EATERS_FORGEFIEND, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWound_VariableAttacks_MultiModelAttacker()
        {
            var expected = 0.3333;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWound(ATTACKER_WORLD_EATERS_CHAOS_SPAWN, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWound_VariableAttacks_WeaponHasTorrent()
        {
            var expected = 0.6667;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWound(ATTACKER_SPACE_MARINE_INFERNUS_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWound_LethalHits()
        {
            var expected = 0.4167;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWound(ATTACKER_ADEPTA_SORORITAS_CELESTIAN_SACRESANTS, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Unit Tests - GetMeanWounds()

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
        public void GetMeanWounds_SingleModelAttacker()
        {
            var expected = 4.4444;
            var actual = Math.Round(CombatMath.GetMeanWounds(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanWounds_MultiModelAttacker()
        {
            var expected = 6.6667;
            var actual = Math.Round(CombatMath.GetMeanWounds(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanWounds_VariableAttacks_SingleModelAttacker()
        {
            var expected = 3.3333;
            var actual = Math.Round(CombatMath.GetMeanWounds(ATTACKER_WORLD_EATERS_FORGEFIEND, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanWounds_VariableAttacks_MultiModelAttacker()
        {
            var expected = 4;
            var actual = Math.Round(CombatMath.GetMeanWounds(ATTACKER_WORLD_EATERS_CHAOS_SPAWN, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanWounds_VariableAttacks_WeaponHasTorrent()
        {
            var expected = 13.3333;
            var actual = Math.Round(CombatMath.GetMeanWounds(ATTACKER_SPACE_MARINE_INFERNUS_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanWounds_WeaponHasLethalHits()
        {
            var expected = 6.25;
            var actual = Math.Round(CombatMath.GetMeanWounds(ATTACKER_ADEPTA_SORORITAS_CELESTIAN_SACRESANTS, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Unit Tests - GetExpectedWounds()

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
        public void GetExpectedWounds_SingleModelAttacker()
        {
            var expected = 4;
            var actual = CombatMath.GetExpectedWounds(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedWounds_MultiModelAttacker()
        {
            var expected = 6;
            var actual = CombatMath.GetExpectedWounds(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedWounds_VariableAttacks_SingleModelAttacker()
        {
            var expected = 3;
            var actual = CombatMath.GetExpectedWounds(ATTACKER_WORLD_EATERS_FORGEFIEND, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedWounds_VariableAttacks_MultiModelAttacker()
        {
            var expected = 4;
            var actual = CombatMath.GetExpectedWounds(ATTACKER_WORLD_EATERS_CHAOS_SPAWN, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedWounds_VariableAttacks_WeaponHasTorrent()
        {
            var expected = 13;
            var actual = CombatMath.GetExpectedWounds(ATTACKER_SPACE_MARINE_INFERNUS_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedWounds_VariableAttacks_WeaponHasLethalHits()
        {
            var expected = 6;
            var actual = CombatMath.GetExpectedWounds(ATTACKER_ADEPTA_SORORITAS_CELESTIAN_SACRESANTS, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Unit Tests - GetStandardDeviationWounds()

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
        public void GetStandardDeviationWounds_SingleModelAttacker()
        {
            var expected = 1.4055;
            var actual = Math.Round(CombatMath.GetStandardDeviationWounds(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationWounds_MultiModelAttacker()
        {
            var expected = 2.1082;
            var actual = Math.Round(CombatMath.GetStandardDeviationWounds(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationWounds_VariableAttacks_SingleModelAttacker()
        {
            var expected = 1.2172;
            var actual = Math.Round(CombatMath.GetStandardDeviationWounds(ATTACKER_WORLD_EATERS_FORGEFIEND, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationWounds_VariableAttacks_MultiModelAttacker()
        {
            var expected = 1.633;
            var actual = Math.Round(CombatMath.GetStandardDeviationWounds(ATTACKER_WORLD_EATERS_CHAOS_SPAWN, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationWounds_VariableAttacks_WeaponHasTorrent()
        {
            var expected = 2.1082;
            var actual = Math.Round(CombatMath.GetStandardDeviationWounds(ATTACKER_SPACE_MARINE_INFERNUS_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationWounds_WeaponHasLethalHits()
        {
            var expected = 1.9094;
            var actual = Math.Round(CombatMath.GetStandardDeviationWounds(ATTACKER_ADEPTA_SORORITAS_CELESTIAN_SACRESANTS, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Unit Tests - GetBinomialDistributionWounds()

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
        public void GetBinomialDistributionWounds_SingleModelAttacker()
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
        public void GetBinomialDistributionWounds_MultiModelAttacker()
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
        public void GetBinomialDistributionWounds_VariableAttacks_SingleModelAttacker()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.0225),
                new(1, 0.1062),
                new(2, 0.2066),
                new(3, 0.2325),
                new(4, 0.2250),
                new(5, 0.1868),
                new(6, 0.1251),
                new(7, 0.0635),
                new(8, 0.0227),
                new(9, 0.0050)
            };

            var actual = CombatMath.GetBinomialDistributionWounds(ATTACKER_WORLD_EATERS_FORGEFIEND, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);

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
        public void GetBinomialDistributionWounds_VariableAttacks_MultiModelAttacker()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.0237),
                new(1, 0.0931),
                new(2, 0.1735),
                new(3, 0.2098),
                new(4, 0.1911),
                new(5, 0.1421),
                new(6, 0.0889),
                new(7, 0.0516),
                new(8, 0.0252),
                new(9, 0.0102),
                new(10, 0.0034),
                new(11, 0.0009),
                new(12, 0.0002),
                new(13, 0),
                new(14, 0),
                new(15, 0),
                new(16, 0)
            };

            var actual = CombatMath.GetBinomialDistributionWounds(ATTACKER_WORLD_EATERS_CHAOS_SPAWN, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetBinomialDistributionWounds_VariableAttacks_WeaponHasTorrent()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.0002),
                new(1, 0.0026),
                new(2, 0.0121),
                new(3, 0.0311),
                new(4, 0.0501),
                new(5, 0.0577),
                new(6, 0.0600),
                new(7, 0.0625),
                new(8, 0.0652),
                new(9, 0.0682),
                new(10, 0.0714),
                new(11, 0.0750),
                new(12, 0.0788),
                new(13, 0.0830),
                new(14, 0.0873),
                new(15, 0.0912),
                new(16, 0.0941),
                new(17, 0.0948),
                new(18, 0.0920),
                new(19, 0.0847),
                new(20, 0.0728),
                new(21, 0.0575),
                new(22, 0.0410),
                new(23, 0.0261),
                new(24, 0.0145),
                new(25, 0.0069),
                new(26, 0.0028),
                new(27, 0.0009),
                new(28, 0.0002),
                new(29, 0),
                new(30, 0),
            };

            var actual = CombatMath.GetBinomialDistributionWounds(ATTACKER_SPACE_MARINE_INFERNUS_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetBinomialDistributionWounds_WeaponHasLethalHits()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.0003),
                new(1, 0.0033),
                new(2, 0.0165),
                new(3, 0.0511),
                new(4, 0.1095),
                new(5, 0.1721),
                new(6, 0.2048),
                new(7, 0.1881),
                new(8, 0.1344),
                new(9, 0.0746),
                new(10, 0.0320),
                new(11, 0.0104),
                new(12, 0.0025),
                new(13, 0.0004),
                new(14, 0),
                new(15, 0)
            };

            var actual = CombatMath.GetBinomialDistributionWounds(ATTACKER_ADEPTA_SORORITAS_CELESTIAN_SACRESANTS, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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

        #region Unit Tests - GetSurvivorDistributionWounds()

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
        public void GetSurvivorDistributionWounds_SingleModelAttacker()
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
        public void GetSurvivorDistributionWounds_MultiModelAttacker()
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
        public void GetSurvivorDistributionWounds_VariableAttacks_SingleModelAttacker()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 1),
                new(2, 1),
                new(3, 0.8607),
                new(4, 0.6282),
                new(5, 0.4031),
                new(6, 0.2163),
                new(7, 0.0912),
                new(8, 0.0277),
                new(9, 0.0050)
            };

            var actual = CombatMath.GetSurvivorDistributionWounds(ATTACKER_WORLD_EATERS_FORGEFIEND, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);

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
        public void GetSurvivorDistributionWounds_VariableAttacks_MultiModelAttacker()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.9902),
                new(2, 0.8971),
                new(3, 0.7235),
                new(4, 0.5137),
                new(5, 0.3226),
                new(6, 0.1805),
                new(7, 0.0916),
                new(8, 0.0400),
                new(9, 0.0148),
                new(10, 0.0046),
                new(11, 0.0012),
                new(12, 0.0002),
                new(13, 0),
                new(14, 0),
                new(15, 0),
                new(16, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionWounds(ATTACKER_WORLD_EATERS_CHAOS_SPAWN, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetSurvivorDistributionWounds_VariableAttacks_WeaponHasTorrent()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 1),
                new(2, 1),
                new(3, 1),
                new(4, 1),
                new(5, 1),
                new(6, 1),
                new(7, 1),
                new(8, 1),
                new(9, 1),
                new(10, 1),
                new(11, 1),
                new(12, 0.9287),
                new(13, 0.8499),
                new(14, 0.7669),
                new(15, 0.6796),
                new(16, 0.5884),
                new(17, 0.4943),
                new(18, 0.3995),
                new(19, 0.3075),
                new(20, 0.2228),
                new(21, 0.1500),
                new(22, 0.0925),
                new(23, 0.0515),
                new(24, 0.0254),
                new(25, 0.0109),
                new(26, 0.0040),
                new(27, 0.0012),
                new(28, 0.0003),
                new(29, 0),
                new(30, 0),
            };

            var actual = CombatMath.GetSurvivorDistributionWounds(ATTACKER_SPACE_MARINE_INFERNUS_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetSurvivorDistributionWounds_WeaponHasLethalHits()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.9997),
                new(2, 0.9964),
                new(3, 0.9799),
                new(4, 0.9288),
                new(5, 0.8193),
                new(6, 0.6472),
                new(7, 0.4424),
                new(8, 0.2543),
                new(9, 0.1199),
                new(10, 0.0453),
                new(11, 0.0133),
                new(12, 0.0029),
                new(13, 0.0005),
                new(14, 0),
                new(15, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionWounds(ATTACKER_ADEPTA_SORORITAS_CELESTIAN_SACRESANTS, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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

        #region Unit Tests - GetProbabilityOfHitAndWoundAndFailedSave()

        /// <summary>
        /// Test the case where the attacker object is null
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWoundAndFailedSave_AttackerIsNull()
        {
            var expected = 0;
            var actual = CombatMath.GetProbabilityOfHitAndWoundAndFailedSave(null, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWoundAndFailedSave_DefenderIsNull()
        {
            var expected = 0;
            var actual = CombatMath.GetProbabilityOfHitAndWoundAndFailedSave(ATTACKER_KHARN_THE_BETRAYER, null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case with given parameters.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWoundAndFailedSave_HigherStrength()
        {
            var expected = 0.3704;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWoundAndFailedSave(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWoundAndFailedSave_EqualStrengthAndToughness()
        {
            var expected = 0.1667;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWoundAndFailedSave(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWoundAndFailedSave_HigherToughness()
        {
            var expected = 0.0741;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWoundAndFailedSave(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWoundAndFailedSave_VariableAttacks_SingleModelAttacker()
        {
            var expected = 0.2778;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWoundAndFailedSave(ATTACKER_WORLD_EATERS_FORGEFIEND, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWoundAndFailedSave_VariableAttacks_MultiModelAttacker()
        {
            var expected = 0.1667;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWoundAndFailedSave(ATTACKER_WORLD_EATERS_CHAOS_SPAWN, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWoundAndFailedSave_VariableAttacks_WeaponHasTorrent()
        {
            var expected = 0.3333;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWoundAndFailedSave(ATTACKER_SPACE_MARINE_INFERNUS_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWoundAndFailedSave_LethalHits()
        {
            var expected = 0.2083;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWoundAndFailedSave(ATTACKER_ADEPTA_SORORITAS_CELESTIAN_SACRESANTS, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWoundAndFailedSave_DevastatingWounds()
        {
            var expected = 0.2963;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWoundAndFailedSave(ATTACKER_CHAOS_KNIGHT_ABOMINANT, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWoundAndFailedSave_LethalHitsAndDevastatingWounds()
        {
            var expected = 0.5;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWoundAndFailedSave(ATTACKER_KHARN_THE_BETRAYER_BLESSED, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Unit Tests - GetMeanFailedSaves()

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
        public void GetMeanFailedSaves_SingleModelAttacker()
        {
            var expected = 2.963;
            var actual = Math.Round(CombatMath.GetMeanFailedSaves(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_MultiModelAttacker()
        {
            var expected = 3.3333;
            var actual = Math.Round(CombatMath.GetMeanFailedSaves(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_DefenderHasInvulnerableSave()
        {
            var expected = 2.2222;
            var actual = Math.Round(CombatMath.GetMeanFailedSaves(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_VariableAttacks_SingleModelAttacker()
        {
            var expected = 1.6667;
            var actual = Math.Round(CombatMath.GetMeanFailedSaves(ATTACKER_WORLD_EATERS_FORGEFIEND, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_VariableAttacks_MultiModelAttacker()
        {
            var expected = 2;
            var actual = Math.Round(CombatMath.GetMeanFailedSaves(ATTACKER_WORLD_EATERS_CHAOS_SPAWN, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_VariableAttacks_WeaponHasTorrent()
        {
            var expected = 6.6667;
            var actual = Math.Round(CombatMath.GetMeanFailedSaves(ATTACKER_SPACE_MARINE_INFERNUS_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_WeaponHasLethalHits()
        {
            var expected = 3.125;
            var actual = Math.Round(CombatMath.GetMeanFailedSaves(ATTACKER_ADEPTA_SORORITAS_CELESTIAN_SACRESANTS, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_DevastatingWounds()
        {
            var expected = 2.6667;
            var actual = Math.Round(CombatMath.GetMeanFailedSaves(ATTACKER_CHAOS_KNIGHT_ABOMINANT, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_LethalHitsAndDevastatingWounds()
        {
            var expected = 4;
            var actual = Math.Round(CombatMath.GetMeanFailedSaves(ATTACKER_KHARN_THE_BETRAYER_BLESSED, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Unit Tests - GetExpectedFailedSaves()

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
        public void GetExpectedFailedSaves_SingleModelAttacker()
        {
            var expected = 2;
            var actual = CombatMath.GetExpectedFailedSaves(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedFailedSaves_MultiModelAttacker()
        {
            var expected = 3;
            var actual = CombatMath.GetExpectedFailedSaves(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedFailedSaves_DefenderHasInvulnerableSave()
        {
            var expected = 2;
            var actual = CombatMath.GetExpectedFailedSaves(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedFailedSaves_VariableAttacks_SingleModelAttacker()
        {
            var expected = 1;
            var actual = CombatMath.GetExpectedFailedSaves(ATTACKER_WORLD_EATERS_FORGEFIEND, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedFailedSaves_VariableAttacks_MultiModelAttacker()
        {
            var expected = 2;
            var actual = CombatMath.GetExpectedFailedSaves(ATTACKER_WORLD_EATERS_CHAOS_SPAWN, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedFailedSaves_VariableAttacks_WeaponHasTorrent()
        {
            var expected = 6;
            var actual = CombatMath.GetExpectedFailedSaves(ATTACKER_SPACE_MARINE_INFERNUS_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedFailedSaves_WeaponHasLethalHits()
        {
            var expected = 3;
            var actual = CombatMath.GetExpectedFailedSaves(ATTACKER_ADEPTA_SORORITAS_CELESTIAN_SACRESANTS, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Unit Tests - GetStandardDeviationFailedSaves()

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
        public void GetStandardDeviationFailedSaves_SingleModelAttacker()
        {
            var expected = 1.3659;
            var actual = Math.Round(CombatMath.GetStandardDeviationFailedSaves(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationFailedSaves_MultiModelAttacker()
        {
            var expected = 1.6667;
            var actual = Math.Round(CombatMath.GetStandardDeviationFailedSaves(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationFailedSaves_DefenderHasInvulnerableSave()
        {
            var expected = 1.2669;
            var actual = Math.Round(CombatMath.GetStandardDeviationFailedSaves(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationFailedSaves_VariableAttacks_SingleModelAttacker()
        {
            var expected = 1.0971;
            var actual = Math.Round(CombatMath.GetStandardDeviationFailedSaves(ATTACKER_WORLD_EATERS_FORGEFIEND, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationFailedSaves_VariableAttacks_MultiModelAttacker()
        {
            var expected = 1.2910;
            var actual = Math.Round(CombatMath.GetStandardDeviationFailedSaves(ATTACKER_WORLD_EATERS_CHAOS_SPAWN, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationFailedSaves_VariableAttacks_WeaponHasTorrent()
        {
            var expected = 2.1082;
            var actual = Math.Round(CombatMath.GetStandardDeviationFailedSaves(ATTACKER_SPACE_MARINE_INFERNUS_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationFailedSaves_WeaponHasLethalHits()
        {
            var expected = 1.5729;
            var actual = Math.Round(CombatMath.GetStandardDeviationFailedSaves(ATTACKER_ADEPTA_SORORITAS_CELESTIAN_SACRESANTS, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationFailedSaves_DevastatingWounds()
        {
            var expected = 1.3699;
            var actual = Math.Round(CombatMath.GetStandardDeviationFailedSaves(ATTACKER_CHAOS_KNIGHT_ABOMINANT, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationFailedSaves_LethalHitsAndDevastatingWounds()
        {
            var expected = 1.4142;
            var actual = Math.Round(CombatMath.GetStandardDeviationFailedSaves(ATTACKER_KHARN_THE_BETRAYER_BLESSED, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Unit Tests - GetBinomialDistributionFailedSaves()

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
        public void GetBinomialDistributionFailedSaves_SingleModelAttacker()
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
        public void GetBinomialDistributionFailedSaves_MultiModelAttacker()
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
        public void GetBinomialDistributionFailedSaves_DefenderHasInvulnerableSave()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.0740),
                new(1, 0.2278),
                new(2, 0.3066),
                new(3, 0.2358),
                new(4, 0.1134),
                new(5, 0.0349),
                new(6, 0.0067),
                new(7, 0.0007),
                new(8, 0)
            };

            var actual = CombatMath.GetBinomialDistributionFailedSaves(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);

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
        public void GetBinomialDistributionFailedSaves_VariableAttacks_SingleModelAttacker()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.1739),
                new(1, 0.3211),
                new(2, 0.2748),
                new(3, 0.1503),
                new(4, 0.0689),
                new(5, 0.0237),
                new(6, 0.0060),
                new(7, 0.0011),
                new(8, 0.0001),
                new(9, 0)
            };

            var actual = CombatMath.GetBinomialDistributionFailedSaves(ATTACKER_WORLD_EATERS_FORGEFIEND, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);

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
        public void GetBinomialDistributionFailedSaves_VariableAttacks_MultiModelAttacker()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.1581),
                new(1, 0.2937),
                new(2, 0.2696),
                new(3, 0.1650),
                new(4, 0.0758),
                new(5, 0.0275),
                new(6, 0.0080),
                new(7, 0.0021),
                new(8, 0.0004),
                new(9, 0.0001),
                new(10, 0),
                new(11, 0),
                new(12, 0),
                new(13, 0),
                new(14, 0),
                new(15, 0),
                new(16, 0)
            };

            var actual = CombatMath.GetBinomialDistributionFailedSaves(ATTACKER_WORLD_EATERS_CHAOS_SPAWN, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetBinomialDistributionFailedSaves_VariableAttacks_WeaponHasTorrent()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.0152),
                new(1, 0.0532),
                new(2, 0.0911),
                new(3, 0.1099),
                new(4, 0.1138),
                new(5, 0.1122),
                new(6, 0.1119),
                new(7, 0.1076),
                new(8, 0.0983),
                new(9, 0.0841),
                new(10, 0.0666),
                new(11, 0.0484),
                new(12, 0.0320),
                new(13, 0.0192),
                new(14, 0.0104),
                new(15, 0.0051),
                new(16, 0.0022),
                new(17, 0.0009),
                new(18, 0.0003),
                new(19, 0.0001),
                new(20, 0),
                new(21, 0),
                new(22, 0),
                new(23, 0),
                new(24, 0),
                new(25, 0),
                new(26, 0),
                new(27, 0),
                new(28, 0),
                new(29, 0),
                new(30, 0)
            };

            var actual = CombatMath.GetBinomialDistributionFailedSaves(ATTACKER_SPACE_MARINE_INFERNUS_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetBinomialDistributionFailedSaves_WeaponHasLethalHits()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.0301),
                new(1, 0.1187),
                new(2, 0.2187),
                new(3, 0.2493),
                new(4, 0.1968),
                new(5, 0.1140),
                new(6, 0.0500),
                new(7, 0.0169),
                new(8, 0.0045),
                new(9, 0.0009),
                new(10, 0.0001),
                new(11, 0),
                new(12, 0),
                new(13, 0),
                new(14, 0),
                new(15, 0)
            };

            var actual = CombatMath.GetBinomialDistributionFailedSaves(ATTACKER_ADEPTA_SORORITAS_CELESTIAN_SACRESANTS, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetBinomialDistributionFailedSaves_WeaponHasDevastatingWounds()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.0423),
                new(1, 0.1604),
                new(2, 0.2701),
                new(3, 0.2653),
                new(4, 0.1676),
                new(5, 0.0706),
                new(6, 0.0198),
                new(7, 0.0036),
                new(8, 0.0004),
                new(9, 0)
            };

            var actual = CombatMath.GetBinomialDistributionFailedSaves(ATTACKER_CHAOS_KNIGHT_ABOMINANT, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetBinomialDistributionFailedSaves_WeaponHasLethalHitsAndDevastatingWounds()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.0039),
                new(1, 0.0312),
                new(2, 0.1094),
                new(3, 0.2188),
                new(4, 0.2734),
                new(5, 0.2188),
                new(6, 0.1094),
                new(7, 0.0312),
                new(8, 0.0039)
            };

            var actual = CombatMath.GetBinomialDistributionFailedSaves(ATTACKER_KHARN_THE_BETRAYER_BLESSED, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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

        #region Unit Tests - GetSurvivorDistributionFailedSaves()

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
        public void GetSurvivorDistributionFailedSaves_SingleModelAttacker()
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
        public void GetSurvivorDistributionFailedSaves_MultiModelAttacker()
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
        public void GetSurvivorDistributionFailedSaves_DefenderHasInvulnerableSave()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.9260),
                new(2, 0.6982),
                new(3, 0.3916),
                new(4, 0.1558),
                new(5, 0.0424),
                new(6, 0.0075),
                new(7, 0.0008),
                new(8, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionFailedSaves(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);

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
        public void GetSurvivorDistributionFailedSaves_VariableAttacks_SingleModelAttacker()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.8460),
                new(2, 0.5250),
                new(3, 0.2501),
                new(4, 0.0998),
                new(5, 0.0309),
                new(6, 0.0072),
                new(7, 0.0012),
                new(8, 0.0001),
                new(9, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionFailedSaves(ATTACKER_WORLD_EATERS_FORGEFIEND, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);

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
        public void GetSurvivorDistributionFailedSaves_VariableAttacks_MultiModelAttacker()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.8422),
                new(2, 0.5485),
                new(3, 0.2789),
                new(4, 0.1139),
                new(5, 0.0381),
                new(6, 0.0106),
                new(7, 0.0026),
                new(8, 0.0005),
                new(9, 0.0001),
                new(10, 0),
                new(11, 0),
                new(12, 0),
                new(13, 0),
                new(14, 0),
                new(15, 0),
                new(16, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionFailedSaves(ATTACKER_WORLD_EATERS_CHAOS_SPAWN, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetSurvivorDistributionFailedSaves_VariableAttacks_WeaponHasTorrent()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 1),
                new(2, 1),
                new(3, 0.9230),
                new(4, 0.8131),
                new(5, 0.6993),
                new(6, 0.5871),
                new(7, 0.4752),
                new(8, 0.3676),
                new(9, 0.2693),
                new(10, 0.1852),
                new(11, 0.1186),
                new(12, 0.0702),
                new(13, 0.0382),
                new(14, 0.0190),
                new(15, 0.0086),
                new(16, 0.0035),
                new(17, 0.0013),
                new(18, 0.0004),
                new(19, 0.0001),
                new(20, 0),
                new(21, 0),
                new(22, 0),
                new(23, 0),
                new(24, 0),
                new(25, 0),
                new(26, 0),
                new(27, 0),
                new(28, 0),
                new(29, 0),
                new(30, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionFailedSaves(ATTACKER_SPACE_MARINE_INFERNUS_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetSurvivorDistributionFailedSaves_WeaponHasLethalHits()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.9699),
                new(2, 0.8512),
                new(3, 0.6326),
                new(4, 0.3832),
                new(5, 0.1864),
                new(6, 0.0724),
                new(7, 0.0224),
                new(8, 0.0055),
                new(9, 0.0011),
                new(10, 0.0002),
                new(11, 0),
                new(12, 0),
                new(13, 0),
                new(14, 0),
                new(15, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionFailedSaves(ATTACKER_ADEPTA_SORORITAS_CELESTIAN_SACRESANTS, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetSurvivorDistributionFailedSaves_WeaponHasDevastatingWounds()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.9577),
                new(2, 0.7973),
                new(3, 0.5273),
                new(4, 0.2619),
                new(5, 0.0943),
                new(6, 0.0238),
                new(7, 0.0040),
                new(8, 0.0004),
                new(9, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionFailedSaves(ATTACKER_CHAOS_KNIGHT_ABOMINANT, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetSurvivorDistributionFailedSaves_WeaponHasLethalHitsAndDevastatingWounds()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.9961),
                new(2, 0.9648),
                new(3, 0.8555),
                new(4, 0.6367),
                new(5, 0.3633),
                new(6, 0.1445),
                new(7, 0.0352),
                new(8, 0.0039)
            };

            var actual = CombatMath.GetSurvivorDistributionFailedSaves(ATTACKER_KHARN_THE_BETRAYER_BLESSED, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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

        #region Unit Tests - GetMeanDamage()

        /// <summary>
        /// Test the case where the attacker object is null
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_NullAttacker()
        {
            var expected = 0;
            var actual = CombatMath.GetMeanDamage(null, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_NullDefender()
        {
            var expected = 0;
            var actual = CombatMath.GetMeanDamage(ATTACKER_KHARN_THE_BETRAYER, null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_SingleModelAttacker_NoDefenderSpecialRules()
        {
            var expected = 8.8889;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_MultiModelAttacker_NoDefenderSpecialRules()
        {
            var expected = 3.3333;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_SingleModelAttacker_DefenderHasFeelNoPains()
        {
            var expected = 1.6667;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_DEATH_GUARD_MORTARION), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_MultiModelAttacker_DefenderHasFeelNoPains()
        {
            var expected = 2.963;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_WORLD_EATERS_CHAOS_SPAWN), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_DefenderHasInvulnerableSave()
        {
            var expected = 6.6667;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_VariableAttacks_SingleModelAttacker()
        {
            var expected = 5;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_WORLD_EATERS_FORGEFIEND, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_VariableAttacks_MultiModelAttacker()
        {
            var expected = 4;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_WORLD_EATERS_CHAOS_SPAWN, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_SingleModelAttacker_AttackerHasVariableDamage()
        {
            var expected = 14.8148;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_WORLD_EATERS_MAULERFIEND, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_MultiModelAttacker_AttackerHasVariableDamage()
        {
            var expected = 13.3333;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_ADEPTA_SORORITAS_RETRIBUTOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_VariableAttacks_WeaponHasTorrent()
        {
            var expected = 6.6667;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_SPACE_MARINE_INFERNUS_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_WeaponHasLethalHits()
        {
            var expected = 6.25;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_ADEPTA_SORORITAS_CELESTIAN_SACRESANTS, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_DevastatingWounds()
        {
            var expected = 8;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_CHAOS_KNIGHT_ABOMINANT, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_LethalHitsAndDevastatingWounds()
        {
            var expected = 12;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_KHARN_THE_BETRAYER_BLESSED, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Unit Tests - GetExpectedDamage()

        /// <summary>
        /// Test the case where the attacker object is null
        /// </summary>
        [TestMethod]
        public void GetExpectedDamage_NullAttacker()
        {
            var expected = 0;
            var actual = CombatMath.GetExpectedDamage(null, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetExpectedDamage_NullDefender()
        {
            var expected = 0;
            var actual = CombatMath.GetExpectedDamage(ATTACKER_KHARN_THE_BETRAYER, null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDamage_SingleModelAttacker_NoDefenderSpecialRules()
        {
            var expected = 8;
            var actual = CombatMath.GetExpectedDamage(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDamage_MultiModelAttacker_NoDefenderSpecialRules()
        {
            var expected = 3;
            var actual = CombatMath.GetExpectedDamage(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDamage_SingleModelAttacker_DefenderHasFeelNoPains()
        {
            var expected = 1;
            var actual = CombatMath.GetExpectedDamage(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_DEATH_GUARD_MORTARION);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDamage_MultiModelAttacker_DefenderHasFeelNoPains()
        {
            var expected = 2;
            var actual = CombatMath.GetExpectedDamage(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_WORLD_EATERS_CHAOS_SPAWN);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDamage_DefenderHasInvulnerableSave()
        {
            var expected = 6;
            var actual = CombatMath.GetExpectedDamage(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDamage_VariableAttacks_SingleModelAttacker()
        {
            var expected = 5;
            var actual = CombatMath.GetExpectedDamage(ATTACKER_WORLD_EATERS_FORGEFIEND, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDamage_VariableAttacks_MultiModelAttacker()
        {
            var expected = 4;
            var actual = CombatMath.GetExpectedDamage(ATTACKER_WORLD_EATERS_CHAOS_SPAWN, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDamage_SingleModelAttacker_AttackerHasVariableDamage()
        {
            var expected = 14;
            var actual = CombatMath.GetExpectedDamage(ATTACKER_WORLD_EATERS_MAULERFIEND, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDamage_MultiModelAttacker_AttackerHasVariableDamage()
        {
            var expected = 13;
            var actual = CombatMath.GetExpectedDamage(ATTACKER_ADEPTA_SORORITAS_RETRIBUTOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDamage_VariableAttacks_WeaponHasTorrent()
        {
            var expected = 6;
            var actual = CombatMath.GetExpectedDamage(ATTACKER_SPACE_MARINE_INFERNUS_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDamage_WeaponHasLethalHits()
        {
            var expected = 6;
            var actual = CombatMath.GetExpectedDamage(ATTACKER_ADEPTA_SORORITAS_CELESTIAN_SACRESANTS, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Unit Tests - GetStandardDeviationDamage()

        /// <summary>
        /// Test the case where the attacker object is null
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_NullAttacker()
        {
            var expected = 0;
            var actual = CombatMath.GetStandardDeviationDamage(null, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_NullDefender()
        {
            var expected = 0;
            var actual = CombatMath.GetStandardDeviationDamage(ATTACKER_KHARN_THE_BETRAYER, null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_SingleModelAttacker_NoDefenderSpecialRules()
        {
            var expected = 4.0976;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_MultiModelAttacker_NoDefenderSpecialRules()
        {
            var expected = 1.6667;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_SingleModelAttacker_DefenderHasFeelNoPains()
        {
            var expected = 2.157;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_DEATH_GUARD_MORTARION), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_MultiModelAttacker_DefenderHasFeelNoPains()
        {
            var expected = 1.5887;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_WORLD_EATERS_CHAOS_SPAWN), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_DefenderHasInvulnerableSave()
        {
            var expected = 3.8006;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_VariableAttacks_SingleModelAttacker()
        {
            var expected = 3.2914;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_WORLD_EATERS_FORGEFIEND, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_VariableAttacks_MultiModelAttacker()
        {
            var expected = 2.582;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_WORLD_EATERS_CHAOS_SPAWN, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_MultiModelAttacker_VariableAttacks_DefenderHasFeelNoPains()
        {
            var expected = 2.8803;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_WORLD_EATERS_CHAOS_SPAWN, DEFENDER_WORLD_EATERS_CHAOS_SPAWN), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_SingleModelAttacker_AttackerHasVariableDamage()
        {
            var expected = 6.8293;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_WORLD_EATERS_MAULERFIEND, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_MultiModelAttacker_AttackerHasVariableDamage()
        {
            var expected = 5.5777;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_ADEPTA_SORORITAS_RETRIBUTOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_MultiModelAttacker_VariableDamage_DefenderHasFeelNoPains()
        {
            var expected = 5.3333;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_ADEPTA_SORORITAS_RETRIBUTOR_SQUAD, DEFENDER_WORLD_EATERS_CHAOS_SPAWN), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_VariableAttacks_WeaponHasTorrent()
        {
            var expected = 2.1082;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_SPACE_MARINE_INFERNUS_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_WeaponHasLethalHits()
        {
            var expected = 3.1458;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_ADEPTA_SORORITAS_CELESTIAN_SACRESANTS, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_DevastatingWounds()
        {
            var expected = 4.1096;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_CHAOS_KNIGHT_ABOMINANT, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_LethalHitsAndDevastatingWounds()
        {
            var expected = 4.2426;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_KHARN_THE_BETRAYER_BLESSED, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Unit Tests - GetMeanDestroyedModels()

        /// <summary>
        /// Test the case where the attacker object is null
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_NullAttacker()
        {
            var expected = 0;
            var actual = CombatMath.GetMeanDestroyedModels(null, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_NullDefender()
        {
            var expected = 0;
            var actual = CombatMath.GetMeanDestroyedModels(ATTACKER_KHARN_THE_BETRAYER, null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_SingleModelAttacker_NoDefenderSpecialRules()
        {
            var expected = 2;
            var actual = CombatMath.GetMeanDestroyedModels(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_MultiModelAttacker_NoDefenderSpecialRules()
        {
            var expected = 1;
            var actual = CombatMath.GetMeanDestroyedModels(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_SingleModelAttacker_DefenderHasFeelNoPains()
        {
            var expected = 0;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_DEATH_GUARD_MORTARION), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_MultiModelAttacker_DefenderHasFeelNoPains()
        {
            var expected = 0;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_WORLD_EATERS_CHAOS_SPAWN), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_DefenderHasInvulnerableSave()
        {
            var expected = 2;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_VariableAttacks_SingleModelAttacker()
        {
            var expected = 1;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_WORLD_EATERS_FORGEFIEND, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_VariableAttacks_MultiModelAttacker()
        {
            var expected = 2;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_WORLD_EATERS_CHAOS_SPAWN, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_MultiModelAttacker_VariableAttacks_DefenderHasFeelNoPains()
        {
            var expected = 0;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_WORLD_EATERS_CHAOS_SPAWN, DEFENDER_WORLD_EATERS_CHAOS_SPAWN), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_SingleModelAttacker_AttackerHasVariableDamage()
        {
            var expected = 2;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_WORLD_EATERS_MAULERFIEND, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_MultiModelAttacker_AttackerHasVariableDamage()
        {
            var expected = 3;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_ADEPTA_SORORITAS_RETRIBUTOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_MultiModelAttacker_VariableDamage_DefenderHasFeelNoPains()
        {
            var expected = 1;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_ADEPTA_SORORITAS_RETRIBUTOR_SQUAD, DEFENDER_WORLD_EATERS_CHAOS_SPAWN), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_VariableAttacks_WeaponHasTorrent()
        {
            var expected = 3;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_SPACE_MARINE_INFERNUS_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_WeaponHasLethalHits()
        {
            var expected = 3;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_ADEPTA_SORORITAS_CELESTIAN_SACRESANTS, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_DevastatingWounds()
        {
            var expected = 2;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_CHAOS_KNIGHT_ABOMINANT, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_LethalHitsAndDevastatingWounds()
        {
            var expected = 4;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_KHARN_THE_BETRAYER_BLESSED, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Unit Tests - GetExpectedDestroyedModels()

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
        public void GetExpectedDestroyedModels_SingleModelAttacker_NoDefenderSpecialRules()
        {
            var expected = 2;
            var actual = CombatMath.GetExpectedDestroyedModels(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDestroyedModels_MultiModelAttacker_NoDefenderSpecialRules()
        {
            var expected = 1;
            var actual = CombatMath.GetExpectedDestroyedModels(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDestroyedModels_SingleModelAttacker_DefenderHasInvulnerableSave()
        {
            var expected = 1;
            var actual = CombatMath.GetExpectedDestroyedModels(ATTACKER_WORLD_EATERS_FORGEFIEND, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDestroyedModels_SingleModelAttacker_VariableAttacks_DefenderHasFeelNoPain()
        {
            var expected = 1;
            var actual = CombatMath.GetExpectedDestroyedModels(ATTACKER_WORLD_EATERS_FORGEFIEND, DEFENDER_WORLD_EATERS_CHAOS_SPAWN);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDestroyedModels_MultiModelAttacker_VariableAttacks_DefenderHasFeelNoPain()
        {
            var expected = 0;
            var actual = CombatMath.GetExpectedDestroyedModels(ATTACKER_WORLD_EATERS_CHAOS_SPAWN, DEFENDER_WORLD_EATERS_CHAOS_SPAWN);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDestroyedModels_SingleModelAttacker_VariableDamage()
        {
            var expected = 2;
            var actual = CombatMath.GetExpectedDestroyedModels(ATTACKER_WORLD_EATERS_MAULERFIEND, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDestroyedModels_MultiModelAttacker_VariableDamage()
        {
            var expected = 3;
            var actual = CombatMath.GetExpectedDestroyedModels(ATTACKER_ADEPTA_SORORITAS_RETRIBUTOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Unit Tests - GetStandardDeviationDestroyedModels()

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
        public void GetStandardDeviationDestroyedModels_SingleModelAttacker_NoDefenderSpecialRules()
        {
            var expected = 1;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_MultiModelAttacker_NoDefenderSpecialRules()
        {
            var expected = 0;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_SingleModelAttacker_DefenderHasFeelNoPains()
        {
            var expected = 0;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_DEATH_GUARD_MORTARION);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_MultiModelAttacker_DefenderHasFeelNoPains()
        {
            var expected = 0;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_WORLD_EATERS_CHAOS_SPAWN);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_DefenderHasInvulnerableSave()
        {
            var expected = 1;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_VariableAttacks_SingleModelAttacker()
        {
            var expected = 1;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_WORLD_EATERS_FORGEFIEND, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_VariableAttacks_MultiModelAttacker()
        {
            var expected = 1;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_WORLD_EATERS_CHAOS_SPAWN, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_MultiModelAttacker_VariableAttacks_DefenderHasFeelNoPains()
        {
            var expected = 0;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_WORLD_EATERS_CHAOS_SPAWN, DEFENDER_WORLD_EATERS_CHAOS_SPAWN);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_SingleModelAttacker_AttackerHasVariableDamage()
        {
            var expected = 1;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_WORLD_EATERS_MAULERFIEND, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_MultiModelAttacker_AttackerHasVariableDamage()
        {
            var expected = 1;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_ADEPTA_SORORITAS_RETRIBUTOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_MultiModelAttacker_VariableDamage_DefenderHasFeelNoPains()
        {
            var expected = 0;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_ADEPTA_SORORITAS_RETRIBUTOR_SQUAD, DEFENDER_WORLD_EATERS_CHAOS_SPAWN);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_VariableAttacks_WeaponHasTorrent()
        {
            var expected = 1;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_SPACE_MARINE_INFERNUS_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_WeaponHasLethalHits()
        {
            var expected = 1;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_ADEPTA_SORORITAS_CELESTIAN_SACRESANTS, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_DevastatingWounds()
        {
            var expected = 1;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_CHAOS_KNIGHT_ABOMINANT, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_LethalHitsAndDevastatingWounds()
        {
            var expected = 1;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_KHARN_THE_BETRAYER_BLESSED, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Unit Tests - GetBinomialDistributionDestroyedModels()

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
        public void GetBinomialDistributionDestroyedModels_SingleModelAttacker_NoDefenderSpecialRules()
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
        public void GetBinomialDistributionDestroyedModels_MultiModelAttacker_NoDefenderSpecialRules()
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
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetBinomialDistributionDestroyedModels_SingleModelAttacker_DefenderHasFeelNoPains()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.5623),
                new(1, 0)
            };

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_DEATH_GUARD_MORTARION);

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
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetBinomialDistributionDestroyedModels_MultiModelAttacker_DefenderHasFeelNoPains()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.0405),
                new(1, 0.0191),
                new(2, 0.0002),
                new(3, 0),
                new(4, 0),
                new(5, 0)
            };

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_WORLD_EATERS_CHAOS_SPAWN);

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
        public void GetBinomialDistributionDestroyedModels_SingleModelAttacker_DefenderHasInvulnerableSave()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.0740),
                new(1, 0.2278),
                new(2, 0.3066),
                new(3, 0.2358),
                new(4, 0.1134),
                new(5, 0.0349),
                new(6, 0.0067),
                new(7, 0.0007),
                new(8, 0)
            };

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);

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
        public void GetBinomialDistributionDestroyedModels_VariableAttacks_SingleModelAttacker()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.1739),
                new(1, 0.3211),
                new(2, 0.2748),
                new(3, 0.1503),
                new(4, 0.0689),
                new(5, 0.0237),
                new(6, 0.0060),
                new(7, 0.0011),
                new(8, 0.0001),
                new(9, 0)
            };

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_WORLD_EATERS_FORGEFIEND, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);

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
        public void GetBinomialDistributionDestroyedModels_VariableAttacks_MultiModelAttacker()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.1581),
                new(1, 0.2937),
                new(2, 0.2696),
                new(3, 0.1650),
                new(4, 0.0758),
                new(5, 0.0275),
                new(6, 0.0080),
                new(7, 0.0021),
                new(8, 0.0004),
                new(9, 0.0001),
                new(10, 0),
                new(11, 0),
                new(12, 0),
                new(13, 0),
                new(14, 0),
                new(15, 0),
                new(16, 0)
            };

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_WORLD_EATERS_CHAOS_SPAWN, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetBinomialDistributionDestroyedModels_SingleModelAttacker_VariableAttacks_DefenderHasFeelNoPain()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.0225),
                new(1, 0.1186),
                new(2, 0.0414),
                new(3, 0.0145),
                new(4, 0.0025)
            };

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_WORLD_EATERS_FORGEFIEND, DEFENDER_WORLD_EATERS_CHAOS_SPAWN);

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
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetBinomialDistributionDestroyedModels_MultiModelAttacker_VariableAttacks_DefenderHasFeelNoPain()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.0849),
                new(1, 0.0440),
                new(2, 0.0099),
                new(3, 0.0016),
                new(4, 0.0002),
                new(5, 0),
                new(6, 0),
                new(7, 0),
                new(8, 0)
            };

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_WORLD_EATERS_CHAOS_SPAWN, DEFENDER_WORLD_EATERS_CHAOS_SPAWN);

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
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetBinomialDistributionDestroyedModels_MultiModelAttacker_VariableAttacks_WeaponHasTorrent()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.0152),
                new(1, 0.0911),
                new(2, 0.1138),
                new(3, 0.1119),
                new(4, 0.0983),
                new(5, 0.0666),
                new(6, 0.0320),
                new(7, 0.0104),
                new(8, 0.0022),
                new(9, 0.0003),
                new(10, 0),
                new(11, 0),
                new(12, 0),
                new(13, 0),
                new(14, 0),
                new(15, 0)
            };

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_SPACE_MARINE_INFERNUS_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetBinomialDistributionDestroyedModels_SingleModelAttacker_VariableDamage()
        {
            var expected = new List<BinomialOutcome>()
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

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_WORLD_EATERS_MAULERFIEND, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetBinomialDistributionDestroyedModels_MultiModelAttacker_VariableDamage()
        {
            var expected = new List<BinomialOutcome>()
                {
                    new(0, 0.0134),
                    new(1, 0.1341),
                    new(2, 0.2179),
                    new(3, 0.1617),
                    new(4, 0.1226),
                    new(5, 0.0698),
                    new(6, 0.0249),
                    new(7, 0.0051),
                    new(8, 0.0005)
                };

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_ADEPTA_SORORITAS_RETRIBUTOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetBinomialDistributionDestroyedModels_MultiModelAttacker_VariableDamage_DefenderHasFeelNoPain()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.0390),
                new(1, 0.1068),
                new(2, 0.0512),
                new(3, 0.0322),
                new(4, 0.0190),
                new(5, 0.0076),
                new(6, 0.0019),
                new(7, 0.0003),
                new(8, 0)
            };

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_ADEPTA_SORORITAS_RETRIBUTOR_SQUAD, DEFENDER_WORLD_EATERS_CHAOS_SPAWN);

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
        public void GetBinomialDistributionDestroyedModels_VariableAttacks_WeaponHasTorrent()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.0152),
                new(1, 0.0911),
                new(2, 0.1138),
                new(3, 0.1119),
                new(4, 0.0983),
                new(5, 0.0666),
                new(6, 0.0320),
                new(7, 0.0104),
                new(8, 0.0022),
                new(9, 0.0003),
                new(10, 0),
                new(11, 0),
                new(12, 0),
                new(13, 0),
                new(14, 0),
                new(15, 0)
            };

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_SPACE_MARINE_INFERNUS_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetBinomialDistributionDestroyedModels_WeaponHasLethalHits()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.0301),
                new(1, 0.1187),
                new(2, 0.2187),
                new(3, 0.2493),
                new(4, 0.1968),
                new(5, 0.1140),
                new(6, 0.0500),
                new(7, 0.0169),
                new(8, 0.0045),
                new(9, 0.0009),
                new(10, 0.0001),
                new(11, 0),
                new(12, 0),
                new(13, 0),
                new(14, 0),
                new(15, 0)
            };

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_ADEPTA_SORORITAS_CELESTIAN_SACRESANTS, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetBinomialDistributionDestroyedModels_WeaponHasDevastatingWounds()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.0423),
                new(1, 0.1604),
                new(2, 0.2701),
                new(3, 0.2653),
                new(4, 0.1676),
                new(5, 0.0706),
                new(6, 0.0198),
                new(7, 0.0036),
                new(8, 0.0004),
                new(9, 0)
            };

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_CHAOS_KNIGHT_ABOMINANT, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetBinomialDistributionDestroyedModels_WeaponHasLethalHitsAndDevastatingWounds()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.0039),
                new(1, 0.0312),
                new(2, 0.1094),
                new(3, 0.2188),
                new(4, 0.2734),
                new(5, 0.2188),
                new(6, 0.1094),
                new(7, 0.0312),
                new(8, 0.0039)
            };

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_KHARN_THE_BETRAYER_BLESSED, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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

        #region Unit Tests - GetSurvivorDistributionDestroyedModels()

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
        public void GetSurvivorDistributionDestroyedModels_SingleModelAttacker_NoDefenderSpecialRules()
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
        public void GetSurvivorDistributionDestroyedModels_MultiModelAttacker_NoDefenderSpecialRules()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.4741),
                new(2, 0.2758),
                new(3, 0.0736),
                new(4, 0.0089),
                new(5, 0.0005),
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
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetSurvivorDistributionDestroyedModels_SingleModelAttacker_DefenderHasFeelNoPains()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_DEATH_GUARD_MORTARION);

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
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetSurvivorDistributionDestroyedModels_MultiModelAttacker_DefenderHasFeelNoPains()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.0193),
                new(2, 0.0002),
                new(3, 0),
                new(4, 0),
                new(5, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_WORLD_EATERS_CHAOS_SPAWN);

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
        public void GetSurvivorDistributionDestroyedModels_SingleModelAttacker_DefenderHasInvulnerableSave()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.9260),
                new(2, 0.6982),
                new(3, 0.3916),
                new(4, 0.1558),
                new(5, 0.0424),
                new(6, 0.0075),
                new(7, 0.0008),
                new(8, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);

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
        public void GetSurvivorDistributionDestroyedModels_VariableAttacks_SingleModelAttacker()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.8460),
                new(2, 0.5250),
                new(3, 0.2501),
                new(4, 0.0998),
                new(5, 0.0309),
                new(6, 0.0072),
                new(7, 0.0012),
                new(8, 0.0001),
                new(9, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_WORLD_EATERS_FORGEFIEND, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);

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
        public void GetSurvivorDistributionDestroyedModels_VariableAttacks_MultiModelAttacker()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.8422),
                new(2, 0.5485),
                new(3, 0.2789),
                new(4, 0.1139),
                new(5, 0.0381),
                new(6, 0.0106),
                new(7, 0.0026),
                new(8, 0.0005),
                new(9, 0.0001),
                new(10, 0),
                new(11, 0),
                new(12, 0),
                new(13, 0),
                new(14, 0),
                new(15, 0),
                new(16, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_WORLD_EATERS_CHAOS_SPAWN, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetSurvivorDistributionDestroyedModels_SingleModelAttacker_VariableAttacks_DefenderHasFeelNoPain()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.1770),
                new(2, 0.0584),
                new(3, 0.0170),
                new(4, 0.0025)
            };

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_WORLD_EATERS_FORGEFIEND, DEFENDER_WORLD_EATERS_CHAOS_SPAWN);

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
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetSurvivorDistributionDestroyedModels_MultiModelAttacker_VariableAttacks_DefenderHasFeelNoPain()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.0558),
                new(2, 0.0117),
                new(3, 0.0018),
                new(4, 0.0002),
                new(5, 0),
                new(6, 0),
                new(7, 0),
                new(8, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_WORLD_EATERS_CHAOS_SPAWN, DEFENDER_WORLD_EATERS_CHAOS_SPAWN);

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
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetSurvivorDistributionDestroyedModels_MultiModelAttacker_VariableAttacks_WeaponHasTorrent()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.5267),
                new(2, 0.4356),
                new(3, 0.3217),
                new(4, 0.2099),
                new(5, 0.1116),
                new(6, 0.0450),
                new(7, 0.0129),
                new(8, 0.0025),
                new(9, 0.0003),
                new(10, 0),
                new(11, 0),
                new(12, 0),
                new(13, 0),
                new(14, 0),
                new(15, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_SPACE_MARINE_INFERNUS_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetSurvivorDistributionDestroyedModels_SingleModelAttacker_VariableDamage()
        {
            var expected = new List<BinomialOutcome>()
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

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_WORLD_EATERS_MAULERFIEND, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetSurvivorDistributionDestroyedModels_MultiModelAttacker_VariableDamage()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.7366),
                new(2, 0.6025),
                new(3, 0.3846),
                new(4, 0.2229),
                new(5, 0.1003),
                new(6, 0.0305),
                new(7, 0.0055),
                new(8, 0.0005)
            };

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_ADEPTA_SORORITAS_RETRIBUTOR_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetSurvivorDistributionDestroyedModels_MultiModelAttacker_VariableDamage_DefenderHasFeelNoPain()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.2190),
                new(2, 0.1122),
                new(3, 0.0610),
                new(4, 0.0288),
                new(5, 0.0098),
                new(6, 0.0022),
                new(7, 0.0003),
                new(8, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_ADEPTA_SORORITAS_RETRIBUTOR_SQUAD, DEFENDER_WORLD_EATERS_CHAOS_SPAWN);

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
        public void GetSurvivorDistributionDestroyedModels_VariableAttacks_WeaponHasTorrent()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.5267),
                new(2, 0.4356),
                new(3, 0.3217),
                new(4, 0.2099),
                new(5, 0.1116),
                new(6, 0.0450),
                new(7, 0.0129),
                new(8, 0.0025),
                new(9, 0.0003),
                new(10, 0),
                new(11, 0),
                new(12, 0),
                new(13, 0),
                new(14, 0),
                new(15, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_SPACE_MARINE_INFERNUS_SQUAD, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetSurvivorDistributionDestroyedModels_WeaponHasLethalHits()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.9699),
                new(2, 0.8512),
                new(3, 0.6326),
                new(4, 0.3832),
                new(5, 0.1864),
                new(6, 0.0724),
                new(7, 0.0224),
                new(8, 0.0055),
                new(9, 0.0011),
                new(10, 0.0002),
                new(11, 0),
                new(12, 0),
                new(13, 0),
                new(14, 0),
                new(15, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_ADEPTA_SORORITAS_CELESTIAN_SACRESANTS, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetSurvivorDistributionDestroyedModels_WeaponHasDevastatingWounds()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.9577),
                new(2, 0.7973),
                new(3, 0.5273),
                new(4, 0.2619),
                new(5, 0.0943),
                new(6, 0.0238),
                new(7, 0.0040),
                new(8, 0.0004),
                new(9, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_CHAOS_KNIGHT_ABOMINANT, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
        public void GetSurvivorDistributionDestroyedModels_WeaponHasLethalHitsAndDevastatingWounds()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.9961),
                new(2, 0.9648),
                new(3, 0.8555),
                new(4, 0.6367),
                new(5, 0.3633),
                new(6, 0.1445),
                new(7, 0.0352),
                new(8, 0.0039)
            };

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_KHARN_THE_BETRAYER_BLESSED, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
