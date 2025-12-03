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
        /// Attacker data profile with:
        /// - A single model
        /// - Devastating Wounds
        /// </summary>
        public static readonly AttackerDTO ATTACKER_DEVASTATING_WOUNDS = new()
        {
            NumberOfModels = 1,
            WeaponFlatAttacks = 5,
            WeaponSkill = 3,
            WeaponStrength = 6,
            WeaponArmorPierce = 2,
            WeaponFlatDamage = 3,
            WeaponHasDevastatingWounds = true,
        };

        /// <summary>
        /// Attacker data profile with:
        /// - A single model
        /// - Lethal Hits
        /// - Devastating Wounds
        /// </summary>
        public static readonly AttackerDTO ATTACKER_LETHAL_HITS_DEVASTATING_WOUNDS = new()
        {
            NumberOfModels = 1,
            WeaponFlatAttacks = 5,
            WeaponSkill = 3,
            WeaponStrength = 6,
            WeaponArmorPierce = 2,
            WeaponFlatDamage = 3,
            WeaponHasLethalHits = true,
            WeaponHasDevastatingWounds = true
        };

        /// <summary>
        /// Attacker data profile with:
        /// - A single model
        /// - Variable D6+1 damage
        /// </summary>
        public static readonly AttackerDTO ATTACKER_SINGLE_MODEL_VARIABLE_D6_DAMAGE = new()
        {
            NumberOfModels = 1,
            WeaponFlatAttacks = 8,
            WeaponSkill = 3,
            WeaponStrength = 14,
            WeaponArmorPierce = 2,
            WeaponNumberOfDamageDice = 1,
            WeaponDamageDiceType = DiceType.D6,
            WeaponFlatDamage = 1
        };

        /// <summary>
        /// Attacker data profile with:
        /// - Multiple models
        /// - Variable D6+2 damage
        /// </summary>
        public static readonly AttackerDTO ATTACKER_MULTI_MODEL_VARIABLE_D6_DAMAGE = new()
        {
            NumberOfModels = 5,
            WeaponFlatAttacks = 3,
            WeaponSkill = 4,
            WeaponStrength = 9,
            WeaponArmorPierce = 3,
            WeaponNumberOfDamageDice = 1,
            WeaponDamageDiceType = DiceType.D6,
            WeaponFlatDamage = 2
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
        /// Defender data profile with:
        /// - Multiple models
        /// - Feel No Pain 5+
        /// </summary>
        public static readonly DefenderDTO DEFENDER_MULTI_MODEL_FEEL_NO_PAIN_5 = new()
        {
            NumberOfModels = 5,
            Toughness = 5,
            ArmorSave = 3,
            InvulnerableSave = 5,
            FeelNoPain = 5,
            Wounds = 3
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
            var expected = 0.7099;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWound(ATTACKER_SINGLE_MODEL_REROLL_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Reroll Wounds of 1
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWound_RerollWoundsOf1()
        {
            var expected = 0.6327;
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
            var expected = 3.5494;
            var actual = Math.Round(CombatMath.GetMeanWounds(ATTACKER_SINGLE_MODEL_REROLL_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Reroll Wounds of 1
        /// </summary>
        [TestMethod]
        public void GetMeanWounds_RerollWoundsOf1()
        {
            var expected = 3.1636;
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
            var expected = 1.0148;
            var actual = Math.Round(CombatMath.GetStandardDeviationWounds(ATTACKER_SINGLE_MODEL_REROLL_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Reroll Wounds of 1
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationWounds_RerollWoundsOf1()
        {
            var expected = 1.0779;
            var actual = Math.Round(CombatMath.GetStandardDeviationWounds(ATTACKER_SINGLE_MODEL_REROLL_WOUNDS_OF_1, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Unit Tests - GetDistributionWounds()

        /// <summary>
        /// Tests the case where the attacker parameter is null.
        /// </summary>
        [TestMethod]
        public void GetDistributionWounds_AttackerIsNull()
        {
            var expected = new List<BinomialOutcome>();
            var actual = CombatMath.GetDistributionWounds(null, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
                new(0, 0.0021),
                new(1, 0.0251),
                new(2, 0.1231),
                new(3, 0.3011),
                new(4, 0.3684),
                new(5, 0.1803)
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
                new(0, 0.0067),
                new(1, 0.0576),
                new(2, 0.1983),
                new(3, 0.3417),
                new(4, 0.2943),
                new(5, 0.1014)
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
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWoundAndFailedSave_DevastatingWounds()
        {
            var expected = 0.2963;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWoundAndFailedSave(ATTACKER_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetProbabilityOfHitAndWoundAndFailedSave_LethalHitsAndDevastatingWounds()
        {
            var expected = 0.2778;
            var actual = Math.Round(CombatMath.GetProbabilityOfHitAndWoundAndFailedSave(ATTACKER_LETHAL_HITS_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

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
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_WeaponHasDevastatingWounds()
        {
            var expected = 1.4815;
            var actual = Math.Round(CombatMath.GetMeanFailedSaves(ATTACKER_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMeanFailedSaves_WeaponHasLethalHitsAndDevastatingWounds()
        {
            var expected = 1.3889;
            var actual = Math.Round(CombatMath.GetMeanFailedSaves(ATTACKER_LETHAL_HITS_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

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
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationFailedSaves_WeaponHasDevastatingWounds()
        {
            var expected = 1.021;
            var actual = Math.Round(CombatMath.GetStandardDeviationFailedSaves(ATTACKER_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationFailedSaves_WeaponHasLethalHitsAndDevastatingWounds()
        {
            var expected = 1.0015;
            var actual = Math.Round(CombatMath.GetStandardDeviationFailedSaves(ATTACKER_LETHAL_HITS_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Unit Tests - GetDistributionFailedSaves()

        /// <summary>
        /// Tests the case where the attacker parameter is null.
        /// </summary>
        [TestMethod]
        public void GetDistributionFailedSaves_AttackerIsNull()
        {
            var expected = new List<BinomialOutcome>();
            var actual = CombatMath.GetDistributionFailedSaves(null, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
        public void GetDistributionFailedSaves_DefenderIsNull()
        {
            var expected = new List<BinomialOutcome>();
            var actual = CombatMath.GetDistributionFailedSaves(ATTACKER_SINGLE_MODEL_NO_ABILITIES, null);

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
        public void GetDistributionFailedSaves_WeaponHasDevastatingWounds()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.1726),
                new(1, 0.3633),
                new(2, 0.3059),
                new(3, 0.1288),
                new(4, 0.0271),
                new(5, 0.0023)
            };

            var actual = CombatMath.GetDistributionFailedSaves(ATTACKER_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
        public void GetDistributionFailedSaves_WeaponHasLethalHitsAndDevastatingWounds()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.1965),
                new(1, 0.3779),
                new(2, 0.2907),
                new(3, 0.1118),
                new(4, 0.0215),
                new(5, 0.0017)
            };

            var actual = CombatMath.GetDistributionFailedSaves(ATTACKER_LETHAL_HITS_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
        /// Tests the case where the attacker object is null
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_NullAttacker()
        {
            var expected = 0;
            var actual = CombatMath.GetMeanDamage(null, DEFENDER_MULTI_MODEL_NO_ABILITIES);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the defender object is null
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_NullDefender()
        {
            var expected = 0;
            var actual = CombatMath.GetMeanDamage(ATTACKER_SINGLE_MODEL_NO_ABILITIES, null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the defender has an invulnerable save.
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_DefenderHasInvulnerableSave()
        {
            var expected = 6.6667;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the defender has Feel No Pains
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_DefenderHasFeelNoPains()
        {
            var expected = 8.8889;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_FEEL_NO_PAIN_5), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has variable attacks
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_VariableAttacks_MultiModelAttacker()
        {
            var expected = 6.6667;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has variable damage
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_MultiModelAttacker_AttackerHasVariableDamage()
        {
            var expected = 31.25;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_MULTI_MODEL_VARIABLE_D6_DAMAGE, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Lethal Hits
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_WeaponHasLethalHits()
        {
            var expected = 5.5556;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_SINGLE_MODEL_LETHAL_HITS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Devastating Wounds
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_DevastatingWounds()
        {
            var expected = 4.4444;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Lethal Hits and Devastating Wounds
        /// </summary>
        [TestMethod]
        public void GetMeanDamage_LethalHitsAndDevastatingWounds()
        {
            var expected = 4.1667;
            var actual = Math.Round(CombatMath.GetMeanDamage(ATTACKER_LETHAL_HITS_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

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
        /// Tests the case where the defender has an invulnerable save.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_DefenderHasInvulnerableSave()
        {
            var expected = 3.8006;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the defender has Feel No Pains
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_DefenderHasFeelNoPains()
        {
            var expected = 4.0976;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_FEEL_NO_PAIN_5), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has variable attacks
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_VariableAttacks_MultiModelAttacker()
        {
            var expected = 2.1837;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has variable damage
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_MultiModelAttacker_AttackerHasVariableDamage()
        {
            var expected = 11.0633;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_MULTI_MODEL_VARIABLE_D6_DAMAGE, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Lethal Hits
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_WeaponHasLethalHits()
        {
            var expected = 3.2394;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_SINGLE_MODEL_LETHAL_HITS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Devastating Wounds
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_DevastatingWounds()
        {
            var expected = 3.0631;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Lethal Hits and Devastating Wounds
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_LethalHitsAndDevastatingWounds()
        {
            var expected = 3.0046;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_LETHAL_HITS_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

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
        /// Tests the case where the defender has an invulnerable save.
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_DefenderHasInvulnerableSave()
        {
            var expected = 2.2222;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the defender has Feel No Pains
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_DefenderHasFeelNoPains()
        {
            var expected = 1.9753;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_FEEL_NO_PAIN_5), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has variable attacks
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_VariableAttacks_MultiModelAttacker()
        {
            var expected = 3.3333;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has variable damage
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_MultiModelAttacker_AttackerHasVariableDamage()
        {
            var expected = 5.2083;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_DAMAGE, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Lethal Hits
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_WeaponHasLethalHits()
        {
            var expected = 1.8519;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_SINGLE_MODEL_LETHAL_HITS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Devastating Wounds
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_DevastatingWounds()
        {
            var expected = 1.4815;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Lethal Hits and Devastating Wounds
        /// </summary>
        [TestMethod]
        public void GetMeanDestroyedModels_LethalHitsAndDevastatingWounds()
        {
            var expected = 1.3889;
            var actual = Math.Round(CombatMath.GetMeanDestroyedModels(ATTACKER_LETHAL_HITS_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

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
        /// Tests the case where the defender has an invulnerable save.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_DefenderHasInvulnerableSave()
        {
            var expected = 1.2669;
            var actual = Math.Round(CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the defender has Feel No Pains
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_DefenderHasFeelNoPains()
        {
            var expected = 0.9106;
            var actual = Math.Round(CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_FEEL_NO_PAIN_5), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has variable attacks
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_VariableAttacks_MultiModelAttacker()
        {
            var expected = 1.0918;
            var actual = Math.Round(CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has variable damage
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_MultiModelAttacker_AttackerHasVariableDamage()
        {
            var expected = 1.8439;
            var actual = Math.Round(CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_DAMAGE, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Lethal Hits
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_WeaponHasLethalHits()
        {
            var expected = 1.0798;
            var actual = Math.Round(CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_SINGLE_MODEL_LETHAL_HITS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Devastating Wounds
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_DevastatingWounds()
        {
            var expected = 1.021;
            var actual = Math.Round(CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has Lethal Hits and Devastating Wounds
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_LethalHitsAndDevastatingWounds()
        {
            var expected = 1.0015;
            var actual = Math.Round(CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_LETHAL_HITS_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES), 4);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Unit Tests - GetDistributionDestroyedModels()

        /// <summary>
        /// Tests the case where the attacker parameter is null.
        /// </summary>
        [TestMethod]
        public void GetDistributionDestroyedModels_AttackerIsNull()
        {
            var expected = new List<BinomialOutcome>();
            var actual = CombatMath.GetDistributionDestroyedModels(null, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
        public void GetDistributionDestroyedModels_DefenderIsNull()
        {
            var expected = new List<BinomialOutcome>();
            var actual = CombatMath.GetDistributionDestroyedModels(ATTACKER_SINGLE_MODEL_NO_ABILITIES, null);

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
        /// Tests the case where the defender has an invulnerable save.
        /// </summary>
        [TestMethod]
        public void GetDistributionDestroyedModels_DefenderHasInvulnerableSave()
        {
            var expected = new List<BinomialOutcome>
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

            var actual = CombatMath.GetDistributionDestroyedModels(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_INVULNERABLE_SAVE);

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
        /// Tests the case where the defender has Feel No Pains
        /// </summary>
        [TestMethod]
        public void GetDistributionDestroyedModels_DefenderHasFeelNoPains()
        {
            var expected = new List<BinomialOutcome>
            {
                new(0, 0.0247),
                new(1, 0.1084),
                new(2, 0.0528),
                new(3, 0.0345),
                new(4, 0.0230),
                new(5, 0.0108),
                new(6, 0.0032),
                new(7, 0.0005),
                new(8, 0)
            };

            var actual = CombatMath.GetDistributionDestroyedModels(ATTACKER_SINGLE_MODEL_NO_ABILITIES, DEFENDER_MULTI_MODEL_FEEL_NO_PAIN_5);

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
        /// Tests the case where the attacker has variable attacks
        /// </summary>
        [TestMethod]
        public void GetDistributionDestroyedModels_VariableAttacks_MultiModelAttacker()
        {
            var expected = new List<BinomialOutcome>
            {
                new(0, 0.0667),
                new(1, 0.1000),
                new(2, 0.0991),
                new(3, 0.0932),
                new(4, 0.0754),
                new(5, 0.0466),
                new(6, 0.0203),
                new(7, 0.0059),
                new(8, 0.0011),
                new(9, 0.0001),
                new(10, 0),
                new(11, 0),
                new(12, 0),
                new(13, 0),
                new(14, 0),
                new(15, 0)
            };

            var actual = CombatMath.GetDistributionDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_ATTACKS_TORRENT, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
        /// Tests the case where the attacker has variable damage
        /// </summary>
        [TestMethod]
        public void GetDistributionDestroyedModels_MultiModelAttacker_AttackerHasVariableDamage()
        {
            var expected = new List<BinomialOutcome>
            {
                new(0, 0.0017),
                new(1, 0.0133),
                new(2, 0.0495),
                new(3, 0.1140),
                new(4, 0.1820),
                new(5, 0.2129),
                new(6, 0.1888),
                new(7, 0.1291),
                new(8, 0.0687),
                new(9, 0.0284),
                new(10, 0.0091),
                new(11, 0.0022),
                new(12, 0.0004),
                new(13, 0),
                new(14, 0),
                new(15, 0)
            };

            var actual = CombatMath.GetDistributionDestroyedModels(ATTACKER_MULTI_MODEL_VARIABLE_D6_DAMAGE, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
        /// Tests the case where the attacker has Lethal Hits
        /// </summary>
        [TestMethod]
        public void GetDistributionDestroyedModels_WeaponHasLethalHits()
        {
            var expected = new List<BinomialOutcome>
            {
                new(0, 0.0990),
                new(1, 0.2910),
                new(2, 0.3424),
                new(3, 0.2014),
                new(4, 0.0592),
                new(5, 0.0070)
            };

            var actual = CombatMath.GetDistributionDestroyedModels(ATTACKER_SINGLE_MODEL_LETHAL_HITS, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
        /// Tests the case where the attacker has Devastating Wounds
        /// </summary>
        [TestMethod]
        public void GetDistributionDestroyedModels_DevastatingWounds()
        {
            var expected = new List<BinomialOutcome>
            {
                new(0, 0.1965),
                new(1, 0.3779),
                new(2, 0.2907),
                new(3, 0.1118),
                new(4, 0.0215),
                new(5, 0.0017)
            };

            var actual = CombatMath.GetDistributionDestroyedModels(ATTACKER_LETHAL_HITS_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
        /// Tests the case where the attacker has Lethal Hits and Devastating Wounds
        /// </summary>
        [TestMethod]
        public void GetDistributionDestroyedModels_LethalHitsAndDevastatingWounds()
        {
            var expected = new List<BinomialOutcome>
            {
                new(0, 0.1965),
                new(1, 0.3779),
                new(2, 0.2907),
                new(3, 0.1118),
                new(4, 0.0215),
                new(5, 0.0017)
            };

            var actual = CombatMath.GetDistributionDestroyedModels(ATTACKER_LETHAL_HITS_DEVASTATING_WOUNDS, DEFENDER_MULTI_MODEL_NO_ABILITIES);

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
