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
            WeaponDamage = 3
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
            WeaponDamage = 1
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
            WeaponDamage = 2
        };

        /// <summary>
        /// Attack profile for a World Eaters Forgefiend.
        /// </summary>
        public static readonly AttackerDTO ATTACKER_WORLD_EATERS_FORGEFIEND = new()
        {
            NumberOfModels = 1,
            WeaponScalarOfVariableAttacks = 3,
            WeaponVariableAttackType = DiceType.D3,
            WeaponFlatAttacks = 0,
            WeaponSkill = 3,
            WeaponStrength = 10,
            WeaponArmorPierce = 3,
            WeaponDamage = 3
        };

        /// <summary>
        /// Attack profile for a unit of World Eaters Chaos Spawn.
        /// </summary>
        public static readonly AttackerDTO ATTACKER_WORLD_EATERS_CHAOS_SPAWN = new()
        {
            NumberOfModels = 2,
            WeaponScalarOfVariableAttacks = 1,
            WeaponVariableAttackType = DiceType.D6,
            WeaponFlatAttacks = 2,
            WeaponSkill = 4,
            WeaponStrength = 6,
            WeaponArmorPierce = 1,
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

        #region Unit Tests - GetNumberOfSuccessfulResults()

        /// <summary>
        /// Tests the case where the provided success threshold is higher than 7.
        /// </summary>
        [TestMethod]
        public void GetNumberOfSuccessfulResults_SuccessThresholdGreaterThan6()
        {
            var expected = 0;
            var actual = CombatMath.GetNumberOfSuccessfulResults(7);
            Assert.AreEqual(expected, actual);
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

        #endregion

        #region Unit Tests - GetAverageAttacks()

        /// <summary>
        /// Tests the case where the attacker has 0 models.
        /// </summary>
        [TestMethod]
        public void GetAverageAttacks_NullAttacker()
        {
            var expected = 0;
            var actual = CombatMath.GetAverageAttacks(null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has 0 models.
        /// </summary>
        [TestMethod]
        public void GetAverageAttacks_ZeroModels()
        {
            var expected = 0;

            var attacker = new AttackerDTO()
            {
                NumberOfModels = 0,
                WeaponFlatAttacks = 1
            };

            var actual = CombatMath.GetAverageAttacks(attacker);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has a negative number of models.
        /// </summary>
        [TestMethod]
        public void GetAverageAttacks_NegativeNumberOfModels()
        {
            var expected = 0;

            var attacker = new AttackerDTO()
            {
                NumberOfModels = -1,
                WeaponFlatAttacks = 1
            };

            var actual = CombatMath.GetAverageAttacks(attacker);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has 0 weapon attacks.
        /// </summary>
        [TestMethod]
        public void GetAverageAttacks_ZeroWeaponAttacks()
        {
            var expected = 0;

            var attacker = new AttackerDTO()
            {
                NumberOfModels = 1,
                WeaponScalarOfVariableAttacks = 0,
                WeaponFlatAttacks = 0
            };

            var actual = CombatMath.GetAverageAttacks(attacker);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has 0 scalar weapon attacks, but at least one flat attack.
        /// </summary>
        [TestMethod]
        public void GetAverageAttacks_ZeroScalar()
        {
            var expected = 1;

            var attacker = new AttackerDTO()
            {
                NumberOfModels = 1,
                WeaponScalarOfVariableAttacks = 0,
                WeaponFlatAttacks = 1
            };

            var actual = CombatMath.GetAverageAttacks(attacker);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has 0 flat weapon attacks, but at least one scalar attack.
        /// </summary>
        [TestMethod]
        public void GetAverageAttacks_ZeroFlat()
        {
            var expected = 4;

            var attacker = new AttackerDTO()
            {
                NumberOfModels = 1,
                WeaponScalarOfVariableAttacks = 1,
                WeaponVariableAttackType = DiceType.D6,
                WeaponFlatAttacks = 0
            };

            var actual = CombatMath.GetAverageAttacks(attacker);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has a negative number of weapon attacks.
        /// </summary>
        [TestMethod]
        public void GetAverageAttacks_NegativeWeaponAttacks()
        {
            var expected = 0;

            var attacker = new AttackerDTO()
            {
                NumberOfModels = 1,
                WeaponFlatAttacks = -1
            };

            var actual = CombatMath.GetAverageAttacks(attacker);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetAverageAttacks_TestParams1()
        {
            var expected = 8;
            var actual = CombatMath.GetAverageAttacks(ATTACKER_KHARN_THE_BETRAYER);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetAverageAttacks_TestParams2()
        {
            var expected = 20;
            var actual = CombatMath.GetAverageAttacks(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetAverageAttacks_TestParams3()
        {
            var expected = 6;
            var actual = CombatMath.GetAverageAttacks(ATTACKER_WORLD_EATERS_FORGEFIEND);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetAverageAttacks_TestParams4()
        {
            var expected = 12;
            var actual = CombatMath.GetAverageAttacks(ATTACKER_WORLD_EATERS_CHAOS_SPAWN);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Unit Tests - GetMinimumAttacks()

        /// <summary>
        /// Tests the case where the attacker has 0 models.
        /// </summary>
        [TestMethod]
        public void GetMinimumAttacks_NullAttacker()
        {
            var expected = 0;
            var actual = CombatMath.GetMinimumAttacks(null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has 0 models.
        /// </summary>
        [TestMethod]
        public void GetMinimumAttacks_ZeroModels()
        {
            var expected = 0;

            var attacker = new AttackerDTO()
            {
                NumberOfModels = 0,
                WeaponFlatAttacks = 1
            };

            var actual = CombatMath.GetMinimumAttacks(attacker);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has a negative number of models.
        /// </summary>
        [TestMethod]
        public void GetMinimumAttacks_NegativeNumberOfModels()
        {
            var expected = 0;

            var attacker = new AttackerDTO()
            {
                NumberOfModels = -1,
                WeaponFlatAttacks = 1
            };

            var actual = CombatMath.GetMinimumAttacks(attacker);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has 0 weapon attacks.
        /// </summary>
        [TestMethod]
        public void GetMinimumAttacks_ZeroWeaponAttacks()
        {
            var expected = 0;

            var attacker = new AttackerDTO()
            {
                NumberOfModels = 1,
                WeaponScalarOfVariableAttacks = 0,
                WeaponFlatAttacks = 0
            };

            var actual = CombatMath.GetMinimumAttacks(attacker);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has 0 scalar weapon attacks, but at least one flat attack.
        /// </summary>
        [TestMethod]
        public void GetMinimumAttacks_ZeroScalar()
        {
            var expected = 1;

            var attacker = new AttackerDTO()
            {
                NumberOfModels = 1,
                WeaponScalarOfVariableAttacks = 0,
                WeaponFlatAttacks = 1
            };

            var actual = CombatMath.GetMinimumAttacks(attacker);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has 0 flat weapon attacks, but at least one scalar attack.
        /// </summary>
        [TestMethod]
        public void GetMinimumAttacks_ZeroFlat()
        {
            var expected = 1;

            var attacker = new AttackerDTO()
            {
                NumberOfModels = 1,
                WeaponScalarOfVariableAttacks = 1,
                WeaponVariableAttackType = DiceType.D6,
                WeaponFlatAttacks = 0
            };

            var actual = CombatMath.GetMinimumAttacks(attacker);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has a negative number of weapon attacks.
        /// </summary>
        [TestMethod]
        public void GetMinimumAttacks_NegativeWeaponAttacks()
        {
            var expected = 0;

            var attacker = new AttackerDTO()
            {
                NumberOfModels = 1,
                WeaponFlatAttacks = -1
            };

            var actual = CombatMath.GetMinimumAttacks(attacker);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetMinimumAttacks_TestParams1()
        {
            var expected = 8;
            var actual = CombatMath.GetMinimumAttacks(ATTACKER_KHARN_THE_BETRAYER);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetMinimumAttacks_TestParams2()
        {
            var expected = 20;
            var actual = CombatMath.GetMinimumAttacks(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetMinimumAttacks_TestParams3()
        {
            var expected = 3;
            var actual = CombatMath.GetMinimumAttacks(ATTACKER_WORLD_EATERS_FORGEFIEND);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetMinimumAttacks_TestParams4()
        {
            var expected = 6;
            var actual = CombatMath.GetMinimumAttacks(ATTACKER_WORLD_EATERS_CHAOS_SPAWN);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Unit Tests - GetMaximumAttacks()

        /// <summary>
        /// Tests the case where the attacker has 0 models.
        /// </summary>
        [TestMethod]
        public void GetMaximumAttacks_NullAttacker()
        {
            var expected = 0;
            var actual = CombatMath.GetMaximumAttacks(null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has 0 models.
        /// </summary>
        [TestMethod]
        public void GetMaximumAttacks_ZeroModels()
        {
            var expected = 0;

            var attacker = new AttackerDTO()
            {
                NumberOfModels = 0,
                WeaponFlatAttacks = 1
            };

            var actual = CombatMath.GetMaximumAttacks(attacker);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has a negative number of models.
        /// </summary>
        [TestMethod]
        public void GetMaximumAttacks_NegativeNumberOfModels()
        {
            var expected = 0;

            var attacker = new AttackerDTO()
            {
                NumberOfModels = -1,
                WeaponFlatAttacks = 1
            };

            var actual = CombatMath.GetMaximumAttacks(attacker);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has 0 weapon attacks.
        /// </summary>
        [TestMethod]
        public void GetMaximumAttacks_ZeroWeaponAttacks()
        {
            var expected = 0;

            var attacker = new AttackerDTO()
            {
                NumberOfModels = 1,
                WeaponScalarOfVariableAttacks = 0,
                WeaponFlatAttacks = 0
            };

            var actual = CombatMath.GetMaximumAttacks(attacker);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has 0 scalar weapon attacks, but at least one flat attack.
        /// </summary>
        [TestMethod]
        public void GetMaximumAttacks_ZeroScalar()
        {
            var expected = 1;

            var attacker = new AttackerDTO()
            {
                NumberOfModels = 1,
                WeaponScalarOfVariableAttacks = 0,
                WeaponFlatAttacks = 1
            };

            var actual = CombatMath.GetMaximumAttacks(attacker);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has 0 flat weapon attacks, but at least one scalar attack.
        /// </summary>
        [TestMethod]
        public void GetMaximumAttacks_ZeroFlat()
        {
            var expected = 6;

            var attacker = new AttackerDTO()
            {
                NumberOfModels = 1,
                WeaponScalarOfVariableAttacks = 1,
                WeaponVariableAttackType = DiceType.D6,
                WeaponFlatAttacks = 0
            };

            var actual = CombatMath.GetMaximumAttacks(attacker);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the attacker has a negative number of weapon attacks.
        /// </summary>
        [TestMethod]
        public void GetMaximumAttacks_NegativeWeaponAttacks()
        {
            var expected = 0;

            var attacker = new AttackerDTO()
            {
                NumberOfModels = 1,
                WeaponFlatAttacks = -1
            };

            var actual = CombatMath.GetMaximumAttacks(attacker);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetMaximumAttacks_TestParams1()
        {
            var expected = 8;
            var actual = CombatMath.GetMaximumAttacks(ATTACKER_KHARN_THE_BETRAYER);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetMaximumAttacks_TestParams2()
        {
            var expected = 20;
            var actual = CombatMath.GetMaximumAttacks(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetMaximumAttacks_TestParams3()
        {
            var expected = 9;
            var actual = CombatMath.GetMaximumAttacks(ATTACKER_WORLD_EATERS_FORGEFIEND);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with a given parameter.
        /// </summary>
        [TestMethod]
        public void GetMaximumAttacks_TestParams4()
        {
            var expected = 16;
            var actual = CombatMath.GetMaximumAttacks(ATTACKER_WORLD_EATERS_CHAOS_SPAWN);
            Assert.AreEqual(expected, actual);
        }

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
            var expected = 4;
            var actual = CombatMath.GetExpectedHits(ATTACKER_WORLD_EATERS_FORGEFIEND);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedHits_TestParams4()
        {
            var expected = 6;
            var actual = CombatMath.GetExpectedHits(ATTACKER_WORLD_EATERS_CHAOS_SPAWN);

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
            var expected = 1.1547;
            var actual = Math.Round(CombatMath.GetStandardDeviationHits(ATTACKER_WORLD_EATERS_FORGEFIEND), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationHits_TestParams4()
        {
            var expected = 1.7321;
            var actual = Math.Round(CombatMath.GetStandardDeviationHits(ATTACKER_WORLD_EATERS_CHAOS_SPAWN), 4);

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
                new(1, 0.9921),
                new(2, 0.9366),
                new(3, 0.7865),
                new(4, 0.6725),
                new(5, 0.5300),
                new(6, 0.3674),
                new(7, 0.2103),
                new(8, 0.0910),
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
                new(1, 0.9972),
                new(2, 0.9773),
                new(3, 0.9150),
                new(4, 0.7969),
                new(5, 0.6394),
                new(6, 0.4734),
                new(7, 0.3540),
                new(8, 0.2410),
                new(9, 0.1462),
                new(10, 0.0772),
                new(11, 0.0346),
                new(12, 0.0129),
                new(13, 0.0038),
                new(14, 0.0009),
                new(15, 0.0001),
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

        #endregion

        #region Unit Tests - GetSuccessThresholdOfWound()

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

        #endregion

        #region Unit Tests - GetProbabilityOfWound()

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
            var expected = 3;
            var actual = CombatMath.GetExpectedWounds(ATTACKER_WORLD_EATERS_FORGEFIEND, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetExpectedWounds_TestParams4()
        {
            var expected = 4;
            var actual = CombatMath.GetExpectedWounds(ATTACKER_WORLD_EATERS_CHAOS_SPAWN, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD);

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
            var expected = 1.2172;
            var actual = Math.Round(CombatMath.GetStandardDeviationWounds(ATTACKER_WORLD_EATERS_FORGEFIEND, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD), 4);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationWounds_TestParams4()
        {
            var expected = 1.633;
            var actual = Math.Round(CombatMath.GetStandardDeviationWounds(ATTACKER_WORLD_EATERS_CHAOS_SPAWN, DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD), 4);

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
        public void GetSurvivorDistributionWounds_VariableAttakcs_SingleModelAttacker()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.9775),
                new(2, 0.8713),
                new(3, 0.6647),
                new(4, 0.5042),
                new(5, 0.3350),
                new(6, 0.1853),
                new(7, 0.0803),
                new(8, 0.0252),
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
                new(1, 0.9763),
                new(2, 0.8832),
                new(3, 0.7097),
                new(4, 0.4998),
                new(5, 0.3087),
                new(6, 0.1666),
                new(7, 0.0855),
                new(8, 0.0377),
                new(9, 0.0141),
                new(10, 0.0044),
                new(11, 0.0011),
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

        #endregion

        #region Unit Tests - GetAdjustedArmorSave()

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

        #endregion

        #region Unit Tests - GetProbabilityFailedSave()

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
                new(1, 0.8261),
                new(2, 0.5051),
                new(3, 0.2302),
                new(4, 0.0932),
                new(5, 0.0292),
                new(6, 0.0069),
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
                new(1, 0.8419),
                new(2, 0.5482),
                new(3, 0.2787),
                new(4, 0.1136),
                new(5, 0.0378),
                new(6, 0.0103),
                new(7, 0.0025),
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

        #endregion

        #region Unit Tests - GetAdjustedDamage()

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
        public void GetAdjustedDamage_NoDefensiveSpecialRules()
        {
            var expected = 20;
            var actual = CombatMath.GetAdjustedDamage(DEFENDER_SPACE_MARINE_INTERCESSOR_SQUAD, 20);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetAdjustedDamage_SingleModel_DefenderHasFeelNoPains()
        {
            var expected = 6.6667;
            var actual = Math.Round(CombatMath.GetAdjustedDamage(DEFENDER_DEATH_GUARD_MORTARION, 10), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetAdjustedDamage_MultipleModels_DefenderHasFeelNoPains()
        {
            var expected = 13.3333;
            var actual = Math.Round(CombatMath.GetAdjustedDamage(DEFENDER_WORLD_EATERS_CHAOS_SPAWN, 20), 4);
            Assert.AreEqual(expected, actual);
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
        public void GetStandardDeviationDamage_SingleModelAttacker_DefenderHasFeelNoPains()
        {
            var expected = 1.438;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_KHARN_THE_BETRAYER, DEFENDER_DEATH_GUARD_MORTARION), 4);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDamage_MultiModelAttacker_DefenderHasFeelNoPains()
        {
            var expected = 1.0591;
            var actual = Math.Round(CombatMath.GetStandardDeviationDamage(ATTACKER_SPACE_MARINE_INTERCESSOR_SQUAD, DEFENDER_WORLD_EATERS_CHAOS_SPAWN), 4);
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
        public void GetStandardDeviationDestroyedModels_SingleModelAttacker_DefenderHasInvulnerableSave()
        {
            var expected = 1;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_WORLD_EATERS_FORGEFIEND, DEFENDER_SPACE_MARINE_TERMINATOR_SQUAD);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_SingleModelAttacker_VariableAttacks_DefenderHasFeelNoPain()
        {
            var expected = 0;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_WORLD_EATERS_FORGEFIEND, DEFENDER_WORLD_EATERS_CHAOS_SPAWN);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviationDestroyedModels_MultiModelAttacker_VariableAttacks_DefenderHasFeelNoPain()
        {
            var expected = 0;
            var actual = CombatMath.GetStandardDeviationDestroyedModels(ATTACKER_WORLD_EATERS_CHAOS_SPAWN, DEFENDER_WORLD_EATERS_CHAOS_SPAWN);
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
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetBinomialDistributionDestroyedModels_SingleModelAttacker_VariableAttacks_DefenderHasFeelNoPain()
        {
            var expected = new List<BinomialOutcome>()
                {
                    new(0, 0.0225),
                    new(1, 0.2066),
                    new(2, 0.2250),
                    new(3, 0.1251),
                    new(4, 0.0227)
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
                    new(1, 0.2117),
                    new(2, 0.0257),
                    new(3, 0.0007),
                    new(4, 0),
                    new(5, 0)
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
        /// Test the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetSurvivorDistributionDestroyedModels_SingleModelAttacker_VariableAttacks_DefenderHasFeelNoPain()
        {
            var expected = new List<BinomialOutcome>()
                {
                    new(0, 1),
                    new(1, 0.8713),
                    new(2, 0.5042),
                    new(3, 0.1853),
                    new(4, 0.0252)
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
                    new(1, 0.4428),
                    new(2, 0.0369),
                    new(3, 0.0008),
                    new(4, 0),
                    new(5, 0)
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

        #endregion
    }
}
