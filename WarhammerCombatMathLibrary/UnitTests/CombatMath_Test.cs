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
        /// Attacker data profile with:
        /// - A single model
        /// </summary>
        public static readonly AttackerDTO ATTACKER_SINGLE_MODEL_NO_ABILITIES = new()
        {
            NumberOfModels = 1,
            WeaponFlatAttacks = 8,
            WeaponSkill = 2,
            WeaponStrength = 6,
            WeaponArmorPierce = 2,
            WeaponFlatDamage = 3
        };

        /// <summary>
        /// Attacker data profile with:
        /// - Multiple models
        /// </summary>
        public static readonly AttackerDTO ATTACKER_MULTI_MODEL_NO_ABILITIES = new()
        {
            NumberOfModels = 10,
            WeaponFlatAttacks = 2,
            WeaponSkill = 3,
            WeaponStrength = 4,
            WeaponArmorPierce = 1,
            WeaponFlatDamage = 1
        };

        /// <summary>
        /// Attacker data profile with:
        /// - A single model
        /// - D3 variable attacks
        /// </summary>
        public static readonly AttackerDTO ATTACKER_SINGLE_MODEL_VARIABLE_D3_ATTACKS = new()
        {
            NumberOfModels = 1,
            WeaponNumberOfAttackDice = 3,
            WeaponAttackDiceType = DiceType.D3,
            WeaponSkill = 3,
            WeaponStrength = 10,
            WeaponArmorPierce = 3,
            WeaponFlatDamage = 3
        };

        /// <summary>
        /// Attacker data profile with:
        /// - Multiple models
        /// - D6 variable attacks
        /// - Torrent
        /// </summary>
        public static readonly AttackerDTO ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT = new()
        {
            NumberOfModels = 5,
            WeaponNumberOfAttackDice = 1,
            WeaponAttackDiceType = DiceType.D6,
            WeaponHasTorrent = true,
            WeaponStrength = 5,
            WeaponArmorPierce = 1,
            WeaponFlatDamage = 1
        };

        /// <summary>
        /// Attacker data profile with:
        /// - Multiple models
        /// - Reroll Hits
        /// </summary>
        public static readonly AttackerDTO ATTACKER_MULTI_MODEL_REROLL_HITS = new()
        {
            NumberOfModels = 5,
            WeaponFlatAttacks = 3,
            WeaponSkill = 3,
            WeaponHasRerollHitRolls = true,
            WeaponStrength = 6,
            WeaponArmorPierce = 2,
            WeaponFlatDamage = 3
        };

        /// <summary>
        /// Attacker data profile with:
        /// - Multiple models
        /// - Reroll Hits of 1
        /// </summary>
        public static readonly AttackerDTO ATTACKER_MULTI_MODEL_REROLL_HITS_OF_1 = new()
        {
            NumberOfModels = 5,
            WeaponFlatAttacks = 3,
            WeaponSkill = 3,
            WeaponHasRerollHitRollsOf1 = true,
            WeaponStrength = 6,
            WeaponArmorPierce = 2,
            WeaponFlatDamage = 3
        };

        /// <summary>
        /// Attacker data profile with:
        /// - A single model
        /// - Lethal Hits
        /// </summary>
        public static readonly AttackerDTO ATTACKER_SINGLE_MODEL_LETHAL_HITS = new()
        {
            NumberOfModels = 1,
            WeaponFlatAttacks = 5,
            WeaponSkill = 2,
            WeaponHasLethalHits = true,
            WeaponStrength = 6,
            WeaponArmorPierce = 2,
            WeaponFlatDamage = 3
        };

        /// <summary>
        /// Attacker data profile with:
        /// - A single model
        /// - Sustained Hits 1
        /// </summary>
        public static readonly AttackerDTO ATTACKER_SINGLE_MODEL_SUSTAINED_HITS_1 = new()
        {
            NumberOfModels = 1,
            WeaponFlatAttacks = 5,
            WeaponSkill = 2,
            WeaponHasSustainedHits = true,
            WeaponSustainedHitsMultiplier = 1,
            WeaponStrength = 6,
            WeaponArmorPierce = 2,
            WeaponFlatDamage = 3
        };

        /// <summary>
        /// Attacker data profile with:
        /// - A single model
        /// - Sustained Hits 2
        /// </summary>
        public static readonly AttackerDTO ATTACKER_SINGLE_MODEL_SUSTAINED_HITS_2 = new()
        {
            NumberOfModels = 1,
            WeaponFlatAttacks = 5,
            WeaponSkill = 2,
            WeaponHasSustainedHits = true,
            WeaponSustainedHitsMultiplier = 2,
            WeaponStrength = 6,
            WeaponArmorPierce = 2,
            WeaponFlatDamage = 3
        };

        /// <summary>
        /// Attacker data profile with:
        /// - A single model
        /// - Lethal Hits
        /// - Sustained Hits 1
        /// </summary>
        public static readonly AttackerDTO ATTACKER_SINGLE_MODEL_LETHAL_HITS_SUSTAINED_HITS_1 = new()
        {
            NumberOfModels = 1,
            WeaponFlatAttacks = 5,
            WeaponSkill = 2,
            WeaponHasLethalHits = true,
            WeaponHasSustainedHits = true,
            WeaponSustainedHitsMultiplier = 1,
            WeaponStrength = 6,
            WeaponArmorPierce = 2,
            WeaponFlatDamage = 3
        };

        /// <summary>
        /// Attacker data profile with:
        /// - A single model
        /// - Reroll wounds
        /// </summary>
        public static readonly AttackerDTO ATTACKER_SINGLE_MODEL_REROLL_WOUNDS = new()
        {
            NumberOfModels = 1,
            WeaponFlatAttacks = 5,
            WeaponSkill = 2,
            WeaponStrength = 6,
            WeaponHasRerollWoundRolls = true,
            WeaponArmorPierce = 2,
            WeaponFlatDamage = 3
        };

        /// <summary>
        /// Attacker data profile with:
        /// - A single model
        /// - Reroll wounds of 1
        /// </summary>
        public static readonly AttackerDTO ATTACKER_SINGLE_MODEL_REROLL_WOUNDS_OF_1 = new()
        {
            NumberOfModels = 1,
            WeaponFlatAttacks = 5,
            WeaponSkill = 2,
            WeaponStrength = 6,
            WeaponHasRerollWoundRollsOf1 = true,
            WeaponArmorPierce = 2,
            WeaponFlatDamage = 3
        };

        /// <summary>
        /// Attack profile for a unit with variable damage, rolling a D6 type damage die
        /// </summary>
        public static readonly AttackerDTO ATTACKER_SINGLE_MODEL_VARIABLE_D6_DAMAGE = new()
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
        /// Attack profile for a multi-model unit with variable damage, rolling a D6 type damage die
        /// </summary>
        public static readonly AttackerDTO ATTACKER_MULTI_MODEL_VARIABLE_D6_DAMAGE = new()
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
        /// Attack profile for a unit with Devastating Wounds
        /// </summary>
        public static readonly AttackerDTO ATTACKER_DEVASTATING_WOUNDS = new()
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
        /// Attack profile for a hyper-elite character with Lethal Hits, Sustained Hits 1, and Devastating Wounds
        /// </summary>
        public static readonly AttackerDTO ATTACKER_LETHAL_HITS_DEVASTATING_WOUNDS = new()
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
        /// Attack profile for a hyper-elite character with Lethal Hits, Sustained Hits 1, and Devastating Wounds
        /// </summary>
        public static readonly AttackerDTO ATTACKER_SUSTAINED_HITS_1_DEVASTATING_WOUNDS = new()
        {
            NumberOfModels = 1,
            WeaponFlatAttacks = 8,
            WeaponSkill = 2,
            WeaponStrength = 6,
            WeaponArmorPierce = 2,
            WeaponFlatDamage = 3,
            WeaponHasDevastatingWounds = true,
            WeaponHasSustainedHits = true,
            WeaponSustainedHitsMultiplier = 1
        };

        /// <summary>
        /// Attack profile for a hyper-elite character with Lethal Hits, Sustained Hits 1, and Devastating Wounds
        /// </summary>
        public static readonly AttackerDTO ATTACKER_LETHAL_HITS_SUSTAINED_HITS_1_DEVASTATING_WOUNDS = new()
        {
            NumberOfModels = 1,
            WeaponFlatAttacks = 8,
            WeaponSkill = 2,
            WeaponStrength = 6,
            WeaponArmorPierce = 2,
            WeaponFlatDamage = 3,
            WeaponHasLethalHits = true,
            WeaponHasDevastatingWounds = true,
            WeaponHasSustainedHits = true,
            WeaponSustainedHitsMultiplier = 1
        };

        /// <summary>
        /// Defender data profile with:
        /// - Multiple models
        /// </summary>
        public static readonly DefenderDTO DEFENDER_MULTI_MODEL_NO_ABILITIES = new()
        {
            NumberOfModels = 10,
            Toughness = 4,
            ArmorSave = 3,
            Wounds = 2
        };

        /// <summary>
        /// Defender data profile with:
        /// - Multiple models
        /// - Invulnerable save
        /// </summary>
        public static readonly DefenderDTO DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE = new()
        {
            NumberOfModels = 5,
            Toughness = 5,
            ArmorSave = 2,
            InvulnerableSave = 4,
            Wounds = 3
        };

        /// <summary>
        /// Defense profile for a unit of World Eaters Chaos Spawn
        /// </summary>
        public static readonly DefenderDTO DEFENDER_MULTI_MODEL_FEEL_NO_PAIN_5 = new()
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
        public static readonly DefenderDTO DEFENDER_SINGLE_MODEL_FEEL_NO_PAIN_4 = new()
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
        /// Tests the case where the attacker is null
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHit_NullAttacker()
        {
            Assert.AreEqual(0, CombatMath.GetProbabilityOfHit(null));
        }

        /// <summary>
        /// Tests the case where the attacker has 0 weapon skill.
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
        /// Tests the case where the attacker has a negative weapon skill
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
        /// Tests the case where the attacker has a variable number of attacks.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHit_VariableAttacks()
        {
            var expected = 0.6667;
            var actual = Math.Round(CombatMath.GetProbabilityOfHit(ATTACKER_SINGLE_MODEL_VARIABLE_D3_ATTACKS), 4);
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
            var actual = Math.Round(CombatMath.GetProbabilityOfHit(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Reroll Hits.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHit_RerollHits()
        {
            var expected = 0.8889;
            var actual = Math.Round(CombatMath.GetProbabilityOfHit(ATTACKER_MULTI_MODEL_REROLL_HITS), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Reroll Hits of 1.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHit_RerollHitsOf1()
        {
            var expected = 0.7778;
            var actual = Math.Round(CombatMath.GetProbabilityOfHit(ATTACKER_MULTI_MODEL_REROLL_HITS_OF_1), 4);

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
        /// Tests the case where the attacker is a single model with no abilities
        /// </summary>
        [TestMethod]
        public void GetMeanHits_SingleModelAttacker()
        {
            var expected = 6.6667;
            var actual = Math.Round(CombatMath.GetMeanHits(ATTACKER_SINGLE_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker is a multi model unit with no abilities
        /// </summary>
        [TestMethod]
        public void GetMeanHits_MultiModelAttacker()
        {
            var expected = 13.3333;
            var actual = Math.Round(CombatMath.GetMeanHits(ATTACKER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has a variable number of attacks
        /// </summary>
        [TestMethod]
        public void GetMeanHits_VariableAttacks()
        {
            var expected = 4;
            var actual = Math.Round(CombatMath.GetMeanHits(ATTACKER_SINGLE_MODEL_VARIABLE_D3_ATTACKS), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has variable attacks and torrent.
        /// </summary>
        [TestMethod]
        public void GetMeanHits_Torrent()
        {
            var expected = 20;
            var actual = Math.Round(CombatMath.GetMeanHits(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Reroll Hits.
        /// </summary>
        [TestMethod]
        public void GetMeanHits_RerollHits()
        {
            var expected = 13.3333;
            var actual = Math.Round(CombatMath.GetMeanHits(ATTACKER_MULTI_MODEL_REROLL_HITS), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Reroll Hits of 1.
        /// </summary>
        [TestMethod]
        public void GetMeanHits_RerollHitsOf1()
        {
            var expected = 11.6667;
            var actual = Math.Round(CombatMath.GetMeanHits(ATTACKER_MULTI_MODEL_REROLL_HITS_OF_1), 4);
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
        /// Tests the case where the attacker is a single model with no abilities
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationHits_SingleModelAttacker()
        {
            var expected = 1.0541;
            var actual = Math.Round(CombatMath.GetStandardDeviationHits(ATTACKER_SINGLE_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker is a multi model unit with no abilities
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationHits_MultiModelAttacker()
        {
            var expected = 2.1082;
            var actual = Math.Round(CombatMath.GetStandardDeviationHits(ATTACKER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has a variable number of attacks
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationHits_VariableAttacks()
        {
            var expected = 1.4907;
            var actual = Math.Round(CombatMath.GetStandardDeviationHits(ATTACKER_SINGLE_MODEL_VARIABLE_D3_ATTACKS), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has has variable attacks and torrent.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationHits_Torrent()
        {
            var expected = 1.7078;
            var actual = Math.Round(CombatMath.GetStandardDeviationHits(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Reroll Hits.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationHits_RerollHits()
        {
            var expected = 1.2172;
            var actual = Math.Round(CombatMath.GetStandardDeviationHits(ATTACKER_MULTI_MODEL_REROLL_HITS), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Reroll Hits of 1.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationHits_RerollHitsOf1()
        {
            var expected = 1.6102;
            var actual = Math.Round(CombatMath.GetStandardDeviationHits(ATTACKER_MULTI_MODEL_REROLL_HITS_OF_1), 4);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Unit Tests - GetDistributionHits()

        /// <summary>
        /// Tests the case where the attacker parameter is null.
        /// </summary>
        [TestMethod]
        public void GetDistributionHits_AttackerIsNull()
        {
            var expected = new List<BinomialOutcome>();
            var actual = CombatMath.GetDistributionHits(null);

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
        public void GetDistributionHits_BinomialDistribution()
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

            var actual = CombatMath.GetDistributionHits(ATTACKER_SINGLE_MODEL_NO_ABILITIES);

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
        public void GetDistributionHits_CumulativeDistribution()
        {
            var expected = new List<BinomialOutcome>
            {
                new(0, 0),
                new(1, 0),
                new(2, 0.0004),
                new(3, 0.0046),
                new(4, 0.0307),
                new(5, 0.1348),
                new(6, 0.3953),
                new(7, 0.7674),
                new(8, 1)
            };

            var actual = CombatMath.GetDistributionHits(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DistributionTypes.Cumulative);

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
        public void GetDistributionHits_SurvivorDistribution()
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

            var actual = CombatMath.GetDistributionHits(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DistributionTypes.Survivor);

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
        /// Tests the case where the attacker is a multi model unit with no abilities
        /// </summary>
        [TestMethod]
        public void GetDistributionHits_BinomialDistribution_MultiModelAttacker()
        {
            var expected = new List<BinomialOutcome>
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
                new(12, 0.1480),
                new(13, 0.1821),
                new(14, 0.1821),
                new(15, 0.1457),
                new(16, 0.0911),
                new(17, 0.0429),
                new(18, 0.0143),
                new(19, 0.0030),
                new(20, 0.0003)
            };

            var actual = CombatMath.GetDistributionHits(ATTACKER_MULTI_MODEL_NO_ABILITIES);

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
        /// Tests the case where the attacker has a variable number of attacks
        /// </summary>
        [TestMethod]
        public void GetDistributionHits_BinomialDistribution_VariableAttacks()
        {
            var expected = new List<BinomialOutcome>
            {
                new(0, 0.0556),
                new(1, 0.1666),
                new(2, 0.1661),
                new(3, 0.1634),
                new(4, 0.1539),
                new(5, 0.1311),
                new(6, 0.0932),
                new(7, 0.0499),
                new(8, 0.0173),
                new(9, 0.0029)
            };

            var actual = CombatMath.GetDistributionHits(ATTACKER_SINGLE_MODEL_VARIABLE_D3_ATTACKS);

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
        /// Tests the case where the attacker has has variable attacks and torrent.
        /// </summary>
        [TestMethod]
        public void GetDistributionHits_BinomialDistribution_Torrent()
        {
            var expected = new List<BinomialOutcome>
            {
                new(0, 0),
                new(1, 0.0333),
                new(2, 0.0333),
                new(3, 0.0333),
                new(4, 0.0333),
                new(5, 0.0333),
                new(6, 0.0333),
                new(7, 0.0333),
                new(8, 0.0333),
                new(9, 0.0333),
                new(10, 0.0333),
                new(11, 0.0333),
                new(12, 0.0333),
                new(13, 0.0333),
                new(14, 0.0333),
                new(15, 0.0333),
                new(16, 0.0333),
                new(17, 0.0333),
                new(18, 0.0333),
                new(19, 0.0333),
                new(20, 0.0333),
                new(21, 0.0333),
                new(22, 0.0333),
                new(23, 0.0333),
                new(24, 0.0333),
                new(25, 0.0333),
                new(26, 0.0333),
                new(27, 0.0333),
                new(28, 0.0333),
                new(29, 0.0333),
                new(30, 0.0333)
            };

            var actual = CombatMath.GetDistributionHits(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT);

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
        /// Tests the case where the attacker has Reroll Hits.
        /// </summary>
        [TestMethod]
        public void GetDistributionHits_BinomialDistribution_RerollHits()
        {
            var expected = new List<BinomialOutcome>
            {
                new(0, 0),
                new(1, 0),
                new(2, 0),
                new(3, 0),
                new(4, 0),
                new(5, 0),
                new(6, 0),
                new(7, 0.0001),
                new(8, 0.0005),
                new(9, 0.0033),
                new(10, 0.0157),
                new(11, 0.0569),
                new(12, 0.1519),
                new(13, 0.2804),
                new(14, 0.3204),
                new(15, 0.1709)
            };

            var actual = CombatMath.GetDistributionHits(ATTACKER_MULTI_MODEL_REROLL_HITS);

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
        /// Tests the case where the attacker has Reroll Hits of 1.
        /// </summary>
        [TestMethod]
        public void GetDistributionHits_BinomialDistribution_RerollHitsOf1()
        {
            var expected = new List<BinomialOutcome>
            {
                new(0, 0),
                new(1, 0),
                new(2, 0),
                new(3, 0),
                new(4, 0),
                new(5, 0.0003),
                new(6, 0.0015),
                new(7, 0.0066),
                new(8, 0.0231),
                new(9, 0.0628),
                new(10, 0.1318),
                new(11, 0.2097),
                new(12, 0.2447),
                new(13, 0.1976),
                new(14, 0.0988),
                new(15, 0.0231)
            };

            var actual = CombatMath.GetDistributionHits(ATTACKER_MULTI_MODEL_REROLL_HITS_OF_1);

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
        /// Tests the case where the strength is higher than the toughness.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWound_HigherStrength()
        {
            var expected = 0.5556;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWound(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the strength and toughness are the same.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWound_EqualStrengthAndToughness()
        {
            var expected = 0.3333;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWound(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the toughness is higher than the strength.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWound_HigherToughness()
        {
            var expected = 0.2222;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWound(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Torrent.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWound_VariableAttacks_WeaponHasTorrent()
        {
            var expected = 0.6667;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWound(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Lethal Hits.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWound_LethalHits()
        {
            var expected = 0.5556;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWound(ATTACKER_SINGLE_MODEL_LETHAL_HITS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Sustained Hits 1
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWound_SustainedHits1()
        {
            var expected = 0.6481;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWound(ATTACKER_SINGLE_MODEL_SUSTAINED_HITS_1, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Sustained Hits 2
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWound_SustainedHits2()
        {
            var expected = 0.7407;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWound(ATTACKER_SINGLE_MODEL_SUSTAINED_HITS_2, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Lethal Hits and Sustained Hits 1
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWound_LethalHitsAndSustainedHits1()
        {
            var expected = 0.6296;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWound(ATTACKER_SINGLE_MODEL_LETHAL_HITS_SUSTAINED_HITS_1, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Reroll Wounds
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWound_RerollWounds()
        {
            var expected = 0.7407;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWound(ATTACKER_SINGLE_MODEL_REROLL_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Reroll Wounds of 1
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWound_RerollWoundsOf1()
        {
            var expected = 0.6481;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWound(ATTACKER_SINGLE_MODEL_REROLL_WOUNDS_OF_1, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
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
        /// Tests the case where the strength is higher than the toughness.
        /// </summary>
        [TestMethod]
        public void GetMeanWounds_HigherStrength()
        {
            var expected = 4.4444;
            var actual = Math.Round(CombatMath.GetMeanWounds(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the strength and toughness are the same.
        /// </summary>
        [TestMethod]
        public void GetMeanWounds_EqualStrengthAndToughness()
        {
            var expected = 6.6667;
            var actual = Math.Round(CombatMath.GetMeanWounds(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the toughness is higher than the strength.
        /// </summary>
        [TestMethod]
        public void GetMeanWounds_HigherToughness()
        {
            var expected = 4.4444;
            var actual = Math.Round(CombatMath.GetMeanWounds(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Torrent.
        /// </summary>
        [TestMethod]
        public void GetMeanWounds_VariableAttacks_WeaponHasTorrent()
        {
            var expected = 13.3333;
            var actual = Math.Round(CombatMath.GetMeanWounds(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Lethal Hits.
        /// </summary>
        [TestMethod]
        public void GetMeanWounds_LethalHits()
        {
            var expected = 2.7778;
            var actual = Math.Round(CombatMath.GetMeanWounds(ATTACKER_SINGLE_MODEL_LETHAL_HITS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Sustained Hits 1
        /// </summary>
        [TestMethod]
        public void GetMeanWounds_SustainedHits1()
        {
            var expected = 3.2407;
            var actual = Math.Round(CombatMath.GetMeanWounds(ATTACKER_SINGLE_MODEL_SUSTAINED_HITS_1, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Sustained Hits 2
        /// </summary>
        [TestMethod]
        public void GetMeanWounds_SustainedHits2()
        {
            var expected = 3.7037;
            var actual = Math.Round(CombatMath.GetMeanWounds(ATTACKER_SINGLE_MODEL_SUSTAINED_HITS_2, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Lethal Hits and Sustained Hits 1
        /// </summary>
        [TestMethod]
        public void GetMeanWounds_LethalHitsAndSustainedHits1()
        {
            var expected = 3.1481;
            var actual = Math.Round(CombatMath.GetMeanWounds(ATTACKER_SINGLE_MODEL_LETHAL_HITS_SUSTAINED_HITS_1, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Reroll Wounds
        /// </summary>
        [TestMethod]
        public void GetMeanWounds_RerollWounds()
        {
            var expected = 3.7037;
            var actual = Math.Round(CombatMath.GetMeanWounds(ATTACKER_SINGLE_MODEL_REROLL_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Reroll Wounds of 1
        /// </summary>
        [TestMethod]
        public void GetMeanWounds_RerollWoundsOf1()
        {
            var expected = 3.2407;
            var actual = Math.Round(CombatMath.GetMeanWounds(ATTACKER_SINGLE_MODEL_REROLL_WOUNDS_OF_1, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
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
            var actual = CombatMath.GetStandardDeviationWounds(null, DEFENDER_MULTI_MODEL_NO_ABILITIES);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the defender parameter is null.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationWounds_DefenderIsNull()
        {
            var expected = 0;
            var actual = CombatMath.GetStandardDeviationWounds(ATTACKER_SINGLE_MODEL_NO_ABILITIES, null);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the strength is higher than the toughness.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationWounds_HigherStrength()
        {
            var expected = 1.4055;
            var actual = Math.Round(CombatMath.GetStandardDeviationWounds(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the strength and toughness are the same.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationWounds_EqualStrengthAndToughness()
        {
            var expected = 2.1082;
            var actual = Math.Round(CombatMath.GetStandardDeviationWounds(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the toughness is higher than the strength.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationWounds_HigherToughness()
        {
            var expected = 1.8592;
            var actual = Math.Round(CombatMath.GetStandardDeviationWounds(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Torrent.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationWounds_VariableAttacks_WeaponHasTorrent()
        {
            var expected = 2.396;
            var actual = Math.Round(CombatMath.GetStandardDeviationWounds(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Lethal Hits.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationWounds_LethalHits()
        {
            var expected = 1.1111;
            var actual = Math.Round(CombatMath.GetStandardDeviationWounds(ATTACKER_SINGLE_MODEL_LETHAL_HITS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Sustained Hits 1
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationWounds_SustainedHits1()
        {
            var expected = 1.0678;
            var actual = Math.Round(CombatMath.GetStandardDeviationWounds(ATTACKER_SINGLE_MODEL_SUSTAINED_HITS_1, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Sustained Hits 2
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationWounds_SustainedHits2()
        {
            var expected = 0.9799;
            var actual = Math.Round(CombatMath.GetStandardDeviationWounds(ATTACKER_SINGLE_MODEL_SUSTAINED_HITS_2, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Lethal Hits and Sustained Hits 1
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationWounds_LethalHitsAndSustainedHits1()
        {
            var expected = 1.0798;
            var actual = Math.Round(CombatMath.GetStandardDeviationWounds(ATTACKER_SINGLE_MODEL_LETHAL_HITS_SUSTAINED_HITS_1, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Reroll Wounds
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationWounds_RerollWounds()
        {
            var expected = 0.9799;
            var actual = Math.Round(CombatMath.GetStandardDeviationWounds(ATTACKER_SINGLE_MODEL_REROLL_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Reroll Wounds of 1
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationWounds_RerollWoundsOf1()
        {
            var expected = 1.0678;
            var actual = Math.Round(CombatMath.GetStandardDeviationWounds(ATTACKER_SINGLE_MODEL_REROLL_WOUNDS_OF_1, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Unit Tests - GetDistributionWounds()

        /// <summary>
        /// Tests the case where the defender parameter is null.
        /// </summary>
        [TestMethod]
        public void GetDistributionWounds_DefenderIsNull()
        {
            var expected = new List<BinomialOutcome>();
            var actual = CombatMath.GetDistributionWounds(ATTACKER_SINGLE_MODEL_NO_ABILITIES, null);

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
        /// Tests the case where the strength is higher than the toughness.
        /// </summary>
        [TestMethod]
        public void GetDistributionWounds_HigherStrength()
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

            var actual = CombatMath.GetDistributionWounds(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
        /// Tests the case where the strength and toughness are the same.
        /// </summary>
        [TestMethod]
        public void GetDistributionWounds_EqualStrengthAndToughness()
        {
            var expected = new List<BinomialOutcome>
            {
                new(0, 0.0003),
                new(1, 0.0030),
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

            var actual = CombatMath.GetDistributionWounds(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
        /// Tests the case where the toughness is higher than the strength.
        /// </summary>
        [TestMethod]
        public void GetDistributionWounds_HigherToughness()
        {
            var expected = new List<BinomialOutcome>
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

            var actual = CombatMath.GetDistributionWounds(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE);

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
        /// Tests the case where the attacker has Torrent.
        /// </summary>
        [TestMethod]
        public void GetDistributionWounds_VariableAttacks_WeaponHasTorrent()
        {
            var expected = new List<BinomialOutcome>
            {
                new(0, 0.0167),
                new(1, 0.0500),
                new(2, 0.0500),
                new(3, 0.0500),
                new(4, 0.0500),
                new(5, 0.0500),
                new(6, 0.0500),
                new(7, 0.0500),
                new(8, 0.0500),
                new(9, 0.0500),
                new(10, 0.0500),
                new(11, 0.0500),
                new(12, 0.0499),
                new(13, 0.0498),
                new(14, 0.0494),
                new(15, 0.0486),
                new(16, 0.0471),
                new(17, 0.0442),
                new(18, 0.0399),
                new(19, 0.0339),
                new(20, 0.0267),
                new(21, 0.0192),
                new(22, 0.0123),
                new(23, 0.0070),
                new(24, 0.0034),
                new(25, 0.0014),
                new(26, 0.0005),
                new(27, 0.0001),
                new(28, 0),
                new(29, 0),
                new(30, 0)
            };

            var actual = CombatMath.GetDistributionWounds(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
        /// Tests the case where the attacker has Lethal Hits.
        /// </summary>
        [TestMethod]
        public void GetDistributionWounds_LethalHits()
        {
            var expected = new List<BinomialOutcome>
            {
                new(0, 0.0173),
                new(1, 0.1084),
                new(2, 0.2710),
                new(3, 0.3387),
                new(4, 0.2117),
                new(5, 0.0529)
            };

            var actual = CombatMath.GetDistributionWounds(ATTACKER_SINGLE_MODEL_LETHAL_HITS, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
        /// Tests the case where the attacker has Sustained Hits 1
        /// </summary>
        [TestMethod]
        public void GetDistributionWounds_SustainedHits1()
        {
            var expected = new List<BinomialOutcome>
            {
                new(0, 0.0054),
                new(1, 0.0497),
                new(2, 0.1830),
                new(3, 0.3371),
                new(4, 0.3105),
                new(5, 0.1144)
            };

            var actual = CombatMath.GetDistributionWounds(ATTACKER_SINGLE_MODEL_SUSTAINED_HITS_1, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
        /// Tests the case where the attacker has Sustained Hits 2
        /// </summary>
        [TestMethod]
        public void GetDistributionWounds_SustainedHits2()
        {
            var expected = new List<BinomialOutcome>
            {
                new(0, 0.0012),
                new(1, 0.0167),
                new(2, 0.0956),
                new(3, 0.2732),
                new(4, 0.3903),
                new(5, 0.2230)
            };

            var actual = CombatMath.GetDistributionWounds(ATTACKER_SINGLE_MODEL_SUSTAINED_HITS_2, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
        /// Tests the case where the attacker has Lethal Hits and Sustained Hits 1
        /// </summary>
        [TestMethod]
        public void GetDistributionWounds_LethalHitsAndSustainedHits1()
        {
            var expected = new List<BinomialOutcome>
            {
                new(0, 0.0070),
                new(1, 0.0592),
                new(2, 0.2014),
                new(3, 0.3424),
                new(4, 0.2910),
                new(5, 0.0990)
            };

            var actual = CombatMath.GetDistributionWounds(ATTACKER_SINGLE_MODEL_LETHAL_HITS_SUSTAINED_HITS_1, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
        /// Tests the case where the attacker has Reroll Wounds
        /// </summary>
        [TestMethod]
        public void GetDistributionWounds_RerollWounds()
        {
            var expected = new List<BinomialOutcome>
            {
                new(0, 0.0012),
                new(1, 0.0167),
                new(2, 0.0956),
                new(3, 0.2732),
                new(4, 0.3903),
                new(5, 0.2230)
            };

            var actual = CombatMath.GetDistributionWounds(ATTACKER_SINGLE_MODEL_REROLL_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
        /// Tests the case where the attacker has Reroll Wounds of 1
        /// </summary>
        [TestMethod]
        public void GetDistributionWounds_RerollWoundsOf1()
        {
            var expected = new List<BinomialOutcome>
            {
                new(0, 0.0054),
                new(1, 0.0497),
                new(2, 0.1830),
                new(3, 0.3371),
                new(4, 0.3105),
                new(5, 0.1144)
            };

            var actual = CombatMath.GetDistributionWounds(ATTACKER_SINGLE_MODEL_REROLL_WOUNDS_OF_1, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
            var actual = CombatMath.GetProbabilityOfHitAndWoundAndFailedSave(null, DEFENDER_MULTI_MODEL_NO_ABILITIES);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWoundAndFailedSave_DefenderIsNull()
        {
            var expected = 0;
            var actual = CombatMath.GetProbabilityOfHitAndWoundAndFailedSave(ATTACKER_SINGLE_MODEL_NO_ABILITIES, null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case with given parameters.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWoundAndFailedSave_HigherStrength()
        {
            var expected = 0.3704;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWoundAndFailedSave(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWoundAndFailedSave_EqualStrengthAndToughness()
        {
            var expected = 0.1667;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWoundAndFailedSave(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWoundAndFailedSave_HigherToughness()
        {
            var expected = 0.0741;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWoundAndFailedSave(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWoundAndFailedSave_VariableAttacks_SingleModelAttacker()
        {
            var expected = 0.2778;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWoundAndFailedSave(ATTACKER_SINGLE_MODEL_VARIABLE_D3_ATTACKS, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWoundAndFailedSave_VariableAttacks_MultiModelAttacker()
        {
            var expected = 0.1667;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWoundAndFailedSave(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWoundAndFailedSave_VariableAttacks_WeaponHasTorrent()
        {
            var expected = 0.3333;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWoundAndFailedSave(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWoundAndFailedSave_LethalHits()
        {
            var expected = 0.1667;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWoundAndFailedSave(ATTACKER_SINGLE_MODEL_LETHAL_HITS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWoundAndFailedSave_DevastatingWounds()
        {
            var expected = 0.2593;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWoundAndFailedSave(ATTACKER_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWoundAndFailedSave_LethalHitsAndDevastatingWounds()
        {
            var expected = 0.5278;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWoundAndFailedSave(ATTACKER_LETHAL_HITS_SUSTAINED_HITS_1_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

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
            var actual = CombatMath.GetMeanFailedSaves(null, DEFENDER_MULTI_MODEL_NO_ABILITIES);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_NullDefender()
        {
            var expected = 0;
            var actual = CombatMath.GetMeanFailedSaves(ATTACKER_SINGLE_MODEL_NO_ABILITIES, null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_SingleModelAttacker()
        {
            var expected = 2.963;
            var actual = Math.Round(CombatMath.GetMeanFailedSaves(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_MultiModelAttacker()
        {
            var expected = 3.3333;
            var actual = Math.Round(CombatMath.GetMeanFailedSaves(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_DefenderHasInvulnerableSave()
        {
            var expected = 2.2222;
            var actual = Math.Round(CombatMath.GetMeanFailedSaves(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_VariableAttacks_SingleModelAttacker()
        {
            var expected = 1.6667;
            var actual = Math.Round(CombatMath.GetMeanFailedSaves(ATTACKER_SINGLE_MODEL_VARIABLE_D3_ATTACKS, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_VariableAttacks_MultiModelAttacker()
        {
            var expected = 2;
            var actual = Math.Round(CombatMath.GetMeanFailedSaves(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_VariableAttacks_WeaponHasTorrent()
        {
            var expected = 6.6667;
            var actual = Math.Round(CombatMath.GetMeanFailedSaves(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_WeaponHasLethalHits()
        {
            var expected = 2.5;
            var actual = Math.Round(CombatMath.GetMeanFailedSaves(ATTACKER_SINGLE_MODEL_LETHAL_HITS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_WeaponHasSustainedHits1()
        {
            var expected = 3.3333;
            var actual = Math.Round(CombatMath.GetMeanFailedSaves(ATTACKER_SINGLE_MODEL_SUSTAINED_HITS_1, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_WeaponHasSustainedHits2()
        {
            var expected = 3.3333;
            var actual = Math.Round(CombatMath.GetMeanFailedSaves(ATTACKER_SINGLE_MODEL_SUSTAINED_HITS_2, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_WeaponHasDevastatingWounds()
        {
            var expected = 2.3333;
            var actual = Math.Round(CombatMath.GetMeanFailedSaves(ATTACKER_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_WeaponHasLethalHitsAndSustainedHits1()
        {
            var expected = 2.963;
            var actual = Math.Round(CombatMath.GetMeanFailedSaves(ATTACKER_SINGLE_MODEL_LETHAL_HITS_SUSTAINED_HITS_1, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_WeaponHasLethalHitsAndDevastatingWounds()
        {
            var expected = 3.5556;
            var actual = Math.Round(CombatMath.GetMeanFailedSaves(ATTACKER_LETHAL_HITS_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_WeaponHasSustainedHits1AndDevastatingWounds()
        {
            var expected = 3.3333;
            var actual = Math.Round(CombatMath.GetMeanFailedSaves(ATTACKER_SUSTAINED_HITS_1_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_LethalHitsAndSustainedHits1AndDevastatingWounds()
        {
            var expected = 4.2222;
            var actual = Math.Round(CombatMath.GetMeanFailedSaves(ATTACKER_LETHAL_HITS_SUSTAINED_HITS_1_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

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
            var actual = CombatMath.GetExpectedFailedSaves(null, DEFENDER_MULTI_MODEL_NO_ABILITIES);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetExpectedFailedSaves_NullDefender()
        {
            var expected = 0;
            var actual = CombatMath.GetExpectedFailedSaves(ATTACKER_SINGLE_MODEL_NO_ABILITIES, null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedFailedSaves_SingleModelAttacker()
        {
            var expected = 2;
            var actual = CombatMath.GetExpectedFailedSaves(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedFailedSaves_MultiModelAttacker()
        {
            var expected = 3;
            var actual = CombatMath.GetExpectedFailedSaves(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedFailedSaves_DefenderHasInvulnerableSave()
        {
            var expected = 2;
            var actual = CombatMath.GetExpectedFailedSaves(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedFailedSaves_VariableAttacks_SingleModelAttacker()
        {
            var expected = 1;
            var actual = CombatMath.GetExpectedFailedSaves(ATTACKER_SINGLE_MODEL_VARIABLE_D3_ATTACKS, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedFailedSaves_VariableAttacks_MultiModelAttacker()
        {
            var expected = 2;
            var actual = CombatMath.GetExpectedFailedSaves(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedFailedSaves_VariableAttacks_WeaponHasTorrent()
        {
            var expected = 6;
            var actual = CombatMath.GetExpectedFailedSaves(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedFailedSaves_WeaponHasLethalHits()
        {
            var expected = 2;
            var actual = CombatMath.GetExpectedFailedSaves(ATTACKER_SINGLE_MODEL_LETHAL_HITS, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
            var actual = CombatMath.GetStandardDeviationFailedSaves(null, DEFENDER_MULTI_MODEL_NO_ABILITIES);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationFailedSaves_NullDefender()
        {
            var expected = 0;
            var actual = CombatMath.GetStandardDeviationFailedSaves(ATTACKER_SINGLE_MODEL_NO_ABILITIES, null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationFailedSaves_SingleModelAttacker()
        {
            var expected = 1.3659;
            var actual = Math.Round(CombatMath.GetStandardDeviationFailedSaves(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationFailedSaves_MultiModelAttacker()
        {
            var expected = 1.6667;
            var actual = Math.Round(CombatMath.GetStandardDeviationFailedSaves(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationFailedSaves_DefenderHasInvulnerableSave()
        {
            var expected = 1.2669;
            var actual = Math.Round(CombatMath.GetStandardDeviationFailedSaves(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationFailedSaves_VariableAttacks_SingleModelAttacker()
        {
            var expected = 1.0971;
            var actual = Math.Round(CombatMath.GetStandardDeviationFailedSaves(ATTACKER_SINGLE_MODEL_VARIABLE_D3_ATTACKS, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationFailedSaves_VariableAttacks_MultiModelAttacker()
        {
            var expected = 1.2910;
            var actual = Math.Round(CombatMath.GetStandardDeviationFailedSaves(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationFailedSaves_VariableAttacks_WeaponHasTorrent()
        {
            var expected = 2.1082;
            var actual = Math.Round(CombatMath.GetStandardDeviationFailedSaves(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationFailedSaves_WeaponHasLethalHits()
        {
            var expected = 1.4434;
            var actual = Math.Round(CombatMath.GetStandardDeviationFailedSaves(ATTACKER_SINGLE_MODEL_LETHAL_HITS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationFailedSaves_DevastatingWounds()
        {
            var expected = 1.3147;
            var actual = Math.Round(CombatMath.GetStandardDeviationFailedSaves(ATTACKER_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationFailedSaves_LethalHitsAndDevastatingWounds()
        {
            var expected = 1.412;
            var actual = Math.Round(CombatMath.GetStandardDeviationFailedSaves(ATTACKER_LETHAL_HITS_SUSTAINED_HITS_1_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

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
            var actual = CombatMath.GetBinomialDistributionFailedSaves(null, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
            var actual = CombatMath.GetBinomialDistributionFailedSaves(ATTACKER_SINGLE_MODEL_NO_ABILITIES, null);

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

            var actual = CombatMath.GetBinomialDistributionFailedSaves(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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

            var actual = CombatMath.GetBinomialDistributionFailedSaves(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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

            var actual = CombatMath.GetBinomialDistributionFailedSaves(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE);

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

            var actual = CombatMath.GetBinomialDistributionFailedSaves(ATTACKER_SINGLE_MODEL_VARIABLE_D3_ATTACKS, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE);

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

            var actual = CombatMath.GetBinomialDistributionFailedSaves(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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

            var actual = CombatMath.GetBinomialDistributionFailedSaves(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
                new(0, 0.0649),
                new(1, 0.1947),
                new(2, 0.2726),
                new(3, 0.2363),
                new(4, 0.1418),
                new(5, 0.0624),
                new(6, 0.0208),
                new(7, 0.0053),
                new(8, 0.0011),
                new(9, 0.0002),
                new(10, 0),
                new(11, 0),
                new(12, 0),
                new(13, 0),
                new(14, 0),
                new(15, 0)
            };

            var actual = CombatMath.GetBinomialDistributionFailedSaves(ATTACKER_SINGLE_MODEL_LETHAL_HITS, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
                new(0, 0.0671),
                new(1, 0.2115),
                new(2, 0.2961),
                new(3, 0.2418),
                new(4, 0.1270),
                new(5, 0.0444),
                new(6, 0.0104),
                new(7, 0.0016),
                new(8, 0.0001),
                new(9, 0)
            };

            var actual = CombatMath.GetBinomialDistributionFailedSaves(ATTACKER_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
                new(0, 0.0025),
                new(1, 0.0221),
                new(2, 0.0865),
                new(3, 0.1933),
                new(4, 0.2701),
                new(5, 0.2415),
                new(6, 0.1349),
                new(7, 0.0431),
                new(8, 0.0060)
            };

            var actual = CombatMath.GetBinomialDistributionFailedSaves(ATTACKER_LETHAL_HITS_SUSTAINED_HITS_1_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
            var actual = CombatMath.GetSurvivorDistributionFailedSaves(null, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
            var actual = CombatMath.GetSurvivorDistributionFailedSaves(ATTACKER_SINGLE_MODEL_NO_ABILITIES, null);

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

            var actual = CombatMath.GetSurvivorDistributionFailedSaves(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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

            var actual = CombatMath.GetSurvivorDistributionFailedSaves(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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

            var actual = CombatMath.GetSurvivorDistributionFailedSaves(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE);

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

            var actual = CombatMath.GetSurvivorDistributionFailedSaves(ATTACKER_SINGLE_MODEL_VARIABLE_D3_ATTACKS, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE);

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

            var actual = CombatMath.GetSurvivorDistributionFailedSaves(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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

            var actual = CombatMath.GetSurvivorDistributionFailedSaves(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
                new(1, 0.9351),
                new(2, 0.7404),
                new(3, 0.4678),
                new(4, 0.2315),
                new(5, 0.0898),
                new(6, 0.0274),
                new(7, 0.0066),
                new(8, 0.0013),
                new(9, 0.0002),
                new(10, 0),
                new(11, 0),
                new(12, 0),
                new(13, 0),
                new(14, 0),
                new(15, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionFailedSaves(ATTACKER_SINGLE_MODEL_LETHAL_HITS, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
                new(1, 0.9329),
                new(2, 0.7214),
                new(3, 0.4253),
                new(4, 0.1834),
                new(5, 0.0565),
                new(6, 0.0121),
                new(7, 0.0017),
                new(8, 0.0001),
                new(9, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionFailedSaves(ATTACKER_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
                new(1, 0.9975),
                new(2, 0.9754),
                new(3, 0.8889),
                new(4, 0.6956),
                new(5, 0.4255),
                new(6, 0.1841),
                new(7, 0.0491),
                new(8, 0.0060)
            };

            var actual = CombatMath.GetSurvivorDistributionFailedSaves(ATTACKER_LETHAL_HITS_SUSTAINED_HITS_1_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetSurvivorDistributionFailedSaves_SustainedHits1()
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

            var actual = CombatMath.GetSurvivorDistributionFailedSaves(ATTACKER_SINGLE_MODEL_LETHAL_HITS_SUSTAINED_HITS_1, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetSurvivorDistributionFailedSaves_SustainedHits2()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.9769),
                new(2, 0.8781),
                new(3, 0.6805),
                new(4, 0.4358),
                new(5, 0.2260),
                new(6, 0.0942),
                new(7, 0.0314),
                new(8, 0.0083),
                new(9, 0.0018),
                new(10, 0.0003),
                new(11, 0),
                new(12, 0),
                new(13, 0),
                new(14, 0),
                new(15, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionFailedSaves(ATTACKER_SINGLE_MODEL_SUSTAINED_HITS_2, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetSurvivorDistributionFailedSaves_LethalHitsAndSustainedHits1()
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

            var actual = CombatMath.GetSurvivorDistributionFailedSaves(ATTACKER_SINGLE_MODEL_LETHAL_HITS_SUSTAINED_HITS_1, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetSurvivorDistributionFailedSaves_LethalHitsAndSustainedHits1AndDevastatingWounds()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.9975),
                new(2, 0.9754),
                new(3, 0.8889),
                new(4, 0.6956),
                new(5, 0.4255),
                new(6, 0.1841),
                new(7, 0.0491),
                new(8, 0.0060)
            };

            var actual = CombatMath.GetSurvivorDistributionFailedSaves(ATTACKER_LETHAL_HITS_SUSTAINED_HITS_1_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
            var actual = CombatMath.GetMeanDamage(null, DEFENDER_MULTI_MODEL_NO_ABILITIES);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_NullDefender()
        {
            var expected = 0;
            var actual = CombatMath.GetMeanDamage(ATTACKER_SINGLE_MODEL_NO_ABILITIES, null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_SingleModelAttacker_NoDefenderSpecialRules()
        {
            var expected = 8.8889;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_MultiModelAttacker_NoDefenderSpecialRules()
        {
            var expected = 3.3333;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_SingleModelAttacker_DefenderHasFeelNoPains()
        {
            var expected = 1.6667;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_SINGLE_MODEL_FEEL_NO_PAIN_4), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_MultiModelAttacker_DefenderHasFeelNoPains()
        {
            var expected = 2.963;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_FEEL_NO_PAIN_5), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_DefenderHasInvulnerableSave()
        {
            var expected = 6.6667;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_VariableAttacks_SingleModelAttacker()
        {
            var expected = 5;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_SINGLE_MODEL_VARIABLE_D3_ATTACKS, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_VariableAttacks_MultiModelAttacker()
        {
            var expected = 4;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_SingleModelAttacker_AttackerHasVariableDamage()
        {
            var expected = 14.8148;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_SINGLE_MODEL_VARIABLE_D6_DAMAGE, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_MultiModelAttacker_AttackerHasVariableDamage()
        {
            var expected = 13.3333;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_MULTI_MODEL_VARIABLE_D6_DAMAGE, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_VariableAttacks_WeaponHasTorrent()
        {
            var expected = 6.6667;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_WeaponHasLethalHits()
        {
            var expected = 5;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_SINGLE_MODEL_LETHAL_HITS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_DevastatingWounds()
        {
            var expected = 7;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_LethalHitsAndDevastatingWounds()
        {
            var expected = 12.6667;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_LETHAL_HITS_SUSTAINED_HITS_1_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

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
            var actual = CombatMath.GetExpectedDamage(null, DEFENDER_MULTI_MODEL_NO_ABILITIES);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetExpectedDamage_NullDefender()
        {
            var expected = 0;
            var actual = CombatMath.GetExpectedDamage(ATTACKER_SINGLE_MODEL_NO_ABILITIES, null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDamage_SingleModelAttacker_NoDefenderSpecialRules()
        {
            var expected = 8;
            var actual = CombatMath.GetExpectedDamage(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDamage_MultiModelAttacker_NoDefenderSpecialRules()
        {
            var expected = 3;
            var actual = CombatMath.GetExpectedDamage(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDamage_SingleModelAttacker_DefenderHasFeelNoPains()
        {
            var expected = 1;
            var actual = CombatMath.GetExpectedDamage(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_SINGLE_MODEL_FEEL_NO_PAIN_4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDamage_MultiModelAttacker_DefenderHasFeelNoPains()
        {
            var expected = 2;
            var actual = CombatMath.GetExpectedDamage(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_FEEL_NO_PAIN_5);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDamage_DefenderHasInvulnerableSave()
        {
            var expected = 6;
            var actual = CombatMath.GetExpectedDamage(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDamage_VariableAttacks_SingleModelAttacker()
        {
            var expected = 5;
            var actual = CombatMath.GetExpectedDamage(ATTACKER_SINGLE_MODEL_VARIABLE_D3_ATTACKS, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDamage_VariableAttacks_MultiModelAttacker()
        {
            var expected = 4;
            var actual = CombatMath.GetExpectedDamage(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDamage_SingleModelAttacker_AttackerHasVariableDamage()
        {
            var expected = 14;
            var actual = CombatMath.GetExpectedDamage(ATTACKER_SINGLE_MODEL_VARIABLE_D6_DAMAGE, DEFENDER_MULTI_MODEL_NO_ABILITIES);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDamage_MultiModelAttacker_AttackerHasVariableDamage()
        {
            var expected = 13;
            var actual = CombatMath.GetExpectedDamage(ATTACKER_MULTI_MODEL_VARIABLE_D6_DAMAGE, DEFENDER_MULTI_MODEL_NO_ABILITIES);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDamage_VariableAttacks_WeaponHasTorrent()
        {
            var expected = 6;
            var actual = CombatMath.GetExpectedDamage(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDamage_WeaponHasLethalHits()
        {
            var expected = 5;
            var actual = CombatMath.GetExpectedDamage(ATTACKER_SINGLE_MODEL_LETHAL_HITS, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
            var actual = CombatMath.GetStandardDeviationDamage(null, DEFENDER_MULTI_MODEL_NO_ABILITIES);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_NullDefender()
        {
            var expected = 0;
            var actual = CombatMath.GetStandardDeviationDamage(ATTACKER_SINGLE_MODEL_NO_ABILITIES, null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_SingleModelAttacker_NoDefenderSpecialRules()
        {
            var expected = 4.0976;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_MultiModelAttacker_NoDefenderSpecialRules()
        {
            var expected = 1.6667;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_SingleModelAttacker_DefenderHasFeelNoPains()
        {
            var expected = 2.157;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_SINGLE_MODEL_FEEL_NO_PAIN_4), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_MultiModelAttacker_DefenderHasFeelNoPains()
        {
            var expected = 1.5887;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_FEEL_NO_PAIN_5), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_DefenderHasInvulnerableSave()
        {
            var expected = 3.8006;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_VariableAttacks_SingleModelAttacker()
        {
            var expected = 3.2914;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_SINGLE_MODEL_VARIABLE_D3_ATTACKS, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_VariableAttacks_MultiModelAttacker()
        {
            var expected = 2.582;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_MultiModelAttacker_VariableAttacks_DefenderHasFeelNoPains()
        {
            var expected = 2.8803;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_FEEL_NO_PAIN_5), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_SingleModelAttacker_AttackerHasVariableDamage()
        {
            var expected = 6.8293;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_SINGLE_MODEL_VARIABLE_D6_DAMAGE, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_MultiModelAttacker_AttackerHasVariableDamage()
        {
            var expected = 5.5777;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_MULTI_MODEL_VARIABLE_D6_DAMAGE, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_MultiModelAttacker_VariableDamage_DefenderHasFeelNoPains()
        {
            var expected = 5.3333;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_MULTI_MODEL_VARIABLE_D6_DAMAGE, DEFENDER_MULTI_MODEL_FEEL_NO_PAIN_5), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_VariableAttacks_WeaponHasTorrent()
        {
            var expected = 2.1082;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_WeaponHasLethalHits()
        {
            var expected = 2.8868;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_SINGLE_MODEL_LETHAL_HITS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_DevastatingWounds()
        {
            var expected = 3.9441;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_LethalHitsAndDevastatingWounds()
        {
            var expected = 4.2361;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_LETHAL_HITS_SUSTAINED_HITS_1_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

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
            var actual = CombatMath.GetMeanDestroyedModels(null, DEFENDER_MULTI_MODEL_NO_ABILITIES);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_NullDefender()
        {
            var expected = 0;
            var actual = CombatMath.GetMeanDestroyedModels(ATTACKER_SINGLE_MODEL_NO_ABILITIES, null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_SingleModelAttacker_NoDefenderSpecialRules()
        {
            var expected = 2;
            var actual = CombatMath.GetMeanDestroyedModels(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_MultiModelAttacker_NoDefenderSpecialRules()
        {
            var expected = 1;
            var actual = CombatMath.GetMeanDestroyedModels(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_SingleModelAttacker_DefenderHasFeelNoPains()
        {
            var expected = 0;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_SINGLE_MODEL_FEEL_NO_PAIN_4), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_MultiModelAttacker_DefenderHasFeelNoPains()
        {
            var expected = 0;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_FEEL_NO_PAIN_5), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_DefenderHasInvulnerableSave()
        {
            var expected = 2;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_VariableAttacks_SingleModelAttacker()
        {
            var expected = 1;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_SINGLE_MODEL_VARIABLE_D3_ATTACKS, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_VariableAttacks_MultiModelAttacker()
        {
            var expected = 2;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_MultiModelAttacker_VariableAttacks_DefenderHasFeelNoPains()
        {
            var expected = 0;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_FEEL_NO_PAIN_5), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_SingleModelAttacker_AttackerHasVariableDamage()
        {
            var expected = 2;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_SINGLE_MODEL_VARIABLE_D6_DAMAGE, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_MultiModelAttacker_AttackerHasVariableDamage()
        {
            var expected = 3;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_DAMAGE, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_MultiModelAttacker_VariableDamage_DefenderHasFeelNoPains()
        {
            var expected = 1;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_DAMAGE, DEFENDER_MULTI_MODEL_FEEL_NO_PAIN_5), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_VariableAttacks_WeaponHasTorrent()
        {
            var expected = 3;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_WeaponHasLethalHits()
        {
            var expected = 2;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_SINGLE_MODEL_LETHAL_HITS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_DevastatingWounds()
        {
            var expected = 2;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_LethalHitsAndDevastatingWounds()
        {
            var expected = 4;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_LETHAL_HITS_SUSTAINED_HITS_1_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

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
            var actual = CombatMath.GetExpectedDestroyedModels(null, DEFENDER_MULTI_MODEL_NO_ABILITIES);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetExpectedDestroyedModels_NullDefender()
        {
            var expected = 0;
            var actual = CombatMath.GetExpectedDestroyedModels(ATTACKER_SINGLE_MODEL_NO_ABILITIES, null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDestroyedModels_SingleModelAttacker_NoDefenderSpecialRules()
        {
            var expected = 2;
            var actual = CombatMath.GetExpectedDestroyedModels(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDestroyedModels_MultiModelAttacker_NoDefenderSpecialRules()
        {
            var expected = 1;
            var actual = CombatMath.GetExpectedDestroyedModels(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDestroyedModels_SingleModelAttacker_DefenderHasInvulnerableSave()
        {
            var expected = 1;
            var actual = CombatMath.GetExpectedDestroyedModels(ATTACKER_SINGLE_MODEL_VARIABLE_D3_ATTACKS, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDestroyedModels_SingleModelAttacker_VariableAttacks_DefenderHasFeelNoPain()
        {
            var expected = 1;
            var actual = CombatMath.GetExpectedDestroyedModels(ATTACKER_SINGLE_MODEL_VARIABLE_D3_ATTACKS, DEFENDER_MULTI_MODEL_FEEL_NO_PAIN_5);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDestroyedModels_MultiModelAttacker_VariableAttacks_DefenderHasFeelNoPain()
        {
            var expected = 0;
            var actual = CombatMath.GetExpectedDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_FEEL_NO_PAIN_5);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDestroyedModels_SingleModelAttacker_VariableDamage()
        {
            var expected = 2;
            var actual = CombatMath.GetExpectedDestroyedModels(ATTACKER_SINGLE_MODEL_VARIABLE_D6_DAMAGE, DEFENDER_MULTI_MODEL_NO_ABILITIES);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedDestroyedModels_MultiModelAttacker_VariableDamage()
        {
            var expected = 3;
            var actual = CombatMath.GetExpectedDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_DAMAGE, DEFENDER_MULTI_MODEL_NO_ABILITIES);
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
            var actual = CombatMath.GetStandardDeviationDestroyedModels(null, DEFENDER_MULTI_MODEL_NO_ABILITIES);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_NullDefender()
        {
            var expected = 0;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_SINGLE_MODEL_NO_ABILITIES, null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_SingleModelAttacker_NoDefenderSpecialRules()
        {
            var expected = 1;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_MultiModelAttacker_NoDefenderSpecialRules()
        {
            var expected = 0;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_SingleModelAttacker_DefenderHasFeelNoPains()
        {
            var expected = 0;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_SINGLE_MODEL_FEEL_NO_PAIN_4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_MultiModelAttacker_DefenderHasFeelNoPains()
        {
            var expected = 0;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_FEEL_NO_PAIN_5);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_DefenderHasInvulnerableSave()
        {
            var expected = 1;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_VariableAttacks_SingleModelAttacker()
        {
            var expected = 1;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_SINGLE_MODEL_VARIABLE_D3_ATTACKS, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_VariableAttacks_MultiModelAttacker()
        {
            var expected = 1;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_MultiModelAttacker_VariableAttacks_DefenderHasFeelNoPains()
        {
            var expected = 0;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_FEEL_NO_PAIN_5);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_SingleModelAttacker_AttackerHasVariableDamage()
        {
            var expected = 1;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_SINGLE_MODEL_VARIABLE_D6_DAMAGE, DEFENDER_MULTI_MODEL_NO_ABILITIES);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_MultiModelAttacker_AttackerHasVariableDamage()
        {
            var expected = 1;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_DAMAGE, DEFENDER_MULTI_MODEL_NO_ABILITIES);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_MultiModelAttacker_VariableDamage_DefenderHasFeelNoPains()
        {
            var expected = 0;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_DAMAGE, DEFENDER_MULTI_MODEL_FEEL_NO_PAIN_5);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_VariableAttacks_WeaponHasTorrent()
        {
            var expected = 1;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_WeaponHasLethalHits()
        {
            var expected = 1;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_SINGLE_MODEL_LETHAL_HITS, DEFENDER_MULTI_MODEL_NO_ABILITIES);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_DevastatingWounds()
        {
            var expected = 1;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_LethalHitsAndDevastatingWounds()
        {
            var expected = 1;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_LETHAL_HITS_SUSTAINED_HITS_1_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
            var actual = CombatMath.GetBinomialDistributionDestroyedModels(null, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_SINGLE_MODEL_NO_ABILITIES, null);

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

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_SINGLE_MODEL_FEEL_NO_PAIN_4);

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

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_FEEL_NO_PAIN_5);

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

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE);

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

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_SINGLE_MODEL_VARIABLE_D3_ATTACKS, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE);

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

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_SINGLE_MODEL_VARIABLE_D3_ATTACKS, DEFENDER_MULTI_MODEL_FEEL_NO_PAIN_5);

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

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_FEEL_NO_PAIN_5);

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

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_SINGLE_MODEL_VARIABLE_D6_DAMAGE, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_DAMAGE, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_DAMAGE, DEFENDER_MULTI_MODEL_FEEL_NO_PAIN_5);

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

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
                new(0, 0.0649),
                new(1, 0.1947),
                new(2, 0.2726),
                new(3, 0.2363),
                new(4, 0.1418),
                new(5, 0.0624),
                new(6, 0.0208),
                new(7, 0.0053),
                new(8, 0.0011),
                new(9, 0.0002),
                new(10, 0),
                new(11, 0),
                new(12, 0),
                new(13, 0),
                new(14, 0),
                new(15, 0)
            };

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_SINGLE_MODEL_LETHAL_HITS, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
                new(0, 0.0671),
                new(1, 0.2115),
                new(2, 0.2961),
                new(3, 0.2418),
                new(4, 0.1270),
                new(5, 0.0444),
                new(6, 0.0104),
                new(7, 0.0016),
                new(8, 0.0001),
                new(9, 0)
            };

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
                new(0, 0.0025),
                new(1, 0.0221),
                new(2, 0.0865),
                new(3, 0.1933),
                new(4, 0.2701),
                new(5, 0.2415),
                new(6, 0.1349),
                new(7, 0.0431),
                new(8, 0.0060)
            };

            var actual = CombatMath.GetBinomialDistributionDestroyedModels(ATTACKER_LETHAL_HITS_SUSTAINED_HITS_1_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(null, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_SINGLE_MODEL_NO_ABILITIES, null);

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

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_SINGLE_MODEL_FEEL_NO_PAIN_4);

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

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_MULTI_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_FEEL_NO_PAIN_5);

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

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE);

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

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_SINGLE_MODEL_VARIABLE_D3_ATTACKS, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE);

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

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_SINGLE_MODEL_VARIABLE_D3_ATTACKS, DEFENDER_MULTI_MODEL_FEEL_NO_PAIN_5);

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

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_FEEL_NO_PAIN_5);

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

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_SINGLE_MODEL_VARIABLE_D6_DAMAGE, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_DAMAGE, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_DAMAGE, DEFENDER_MULTI_MODEL_FEEL_NO_PAIN_5);

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

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
                new(1, 0.9351),
                new(2, 0.7404),
                new(3, 0.4678),
                new(4, 0.2315),
                new(5, 0.0898),
                new(6, 0.0274),
                new(7, 0.0066),
                new(8, 0.0013),
                new(9, 0.0002),
                new(10, 0),
                new(11, 0),
                new(12, 0),
                new(13, 0),
                new(14, 0),
                new(15, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_SINGLE_MODEL_LETHAL_HITS, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
                new(1, 0.9329),
                new(2, 0.7214),
                new(3, 0.4253),
                new(4, 0.1834),
                new(5, 0.0565),
                new(6, 0.0121),
                new(7, 0.0017),
                new(8, 0.0001),
                new(9, 0)
            };

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
                new(1, 0.9975),
                new(2, 0.9754),
                new(3, 0.8889),
                new(4, 0.6956),
                new(5, 0.4255),
                new(6, 0.1841),
                new(7, 0.0491),
                new(8, 0.0060)
            };

            var actual = CombatMath.GetSurvivorDistributionDestroyedModels(ATTACKER_LETHAL_HITS_SUSTAINED_HITS_1_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
