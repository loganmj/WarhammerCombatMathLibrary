using System.Diagnostics;
using WarhammerCombatMathLibrary.Data;

namespace WarhammerCombatMathLibrary
{
    /// <summary>
    /// Represents a binomial distribution of trials and successes.
    /// </summary>
    public static class CombatMath
    {
        #region Constants

        /// <summary>
        /// The number of possible results of rolling a six-sided die.
        /// </summary>
        private const int POSSIBLE_RESULTS_SIX_SIDED_DIE = 6;

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns the success threshold for succeeding on a given die roll, with a given success threshold.
        /// Note that in Warhammer 40k, a roll of 1 always fails, and a roll of 6 always succeeds.
        /// </summary>
        /// <param name="successThreshold"></param>
        /// <returns>An integer value containing the number of possible successful results.</returns>
        private static int GetNumberOfSuccessfulResults(int successThreshold)
        {
            // If the success threshold is greater than the number of possible results, then there are no successful results
            if (successThreshold > POSSIBLE_RESULTS_SIX_SIDED_DIE)
            {
                Debug.WriteLine($"GetNumberOfSuccessfulResults() | Success threshold is greater than {POSSIBLE_RESULTS_SIX_SIDED_DIE}, returning 0 ...");
                return 0;
            }

            // If the success threshold is less than or equal to 0, then there are no successful results
            if (successThreshold <= 0)
            {
                Debug.WriteLine($"GetNumberOfSuccessfulResults() | Success threshold is less than or equal to 0, returning 0 ...");
                return 0;
            }

            // Calculate the number of possible successful results, capping out at the number of possible results minus one (to account for the guaranteed fail result)
            return Math.Min(POSSIBLE_RESULTS_SIX_SIDED_DIE - (successThreshold - 1), POSSIBLE_RESULTS_SIX_SIDED_DIE - 1);
        }

        /// <summary>
        /// Returns the probability of succeeding a roll with a single dice, given the desired success threshold.
        /// </summary>
        /// <param name="weaponSkill">The weapon skill success threshold.</param>
        /// <returns>A double value containing the probability of success for a single trial.</returns>
        private static double GetProbabilityOfHit(int weaponSkill)
        {
            // Validate parameters
            if (weaponSkill <= 0)
            {
                Debug.WriteLine($"GetProbabilityOfHit() | Weapon skill is less than or equal to 0, returning 0 ...");
                return 0;
            }

            // A roll of 1 is always a fail, so treat an input of 1+ as an input of 2+
            var successThreshold = weaponSkill == 1 ? 2 : weaponSkill;
            return Statistics.ProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, GetNumberOfSuccessfulResults(successThreshold));
        }

        /// <summary>
        /// Determines the maximum possible number of models destroyed given a specified amount of applied damage.
        /// </summary>
        /// <param name="attackerWeaponDamage">The damage per attack from the attacker's weapon.</param>
        /// <param name="totalDamage">The total amount of damage done to the defending unit.</param>
        /// <param name="defender">The defending unit data.</param>
        /// <returns></returns>
        private static int GetModelsDestroyed(int attackerWeaponDamage, int totalDamage, DefenderDTO? defender)
        {
            // Validate inputs
            if (attackerWeaponDamage <= 0)
            {
                Debug.WriteLine($"GetModelsDestroyed() | Attacker's weapon damage is less than or equal to 0. Returning 0 ...");
                return 0;
            }

            if (totalDamage <= 0)
            {
                Debug.WriteLine($"GetModelsDestroyed() | Total damage is less than or equal to 0. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetModelsDestroyed() | Defender is null. Returning 0 ...");
                return 0;
            }

            // Determine the divisor based on which value is larger: the defender's wounds per model, or the attacker's weapon damage.
            var damageThreshold = Math.Max(defender.Wounds, attackerWeaponDamage);

            // Adjust the total damage based on the defender Feel No Pain probability
            var adjustedTotalDamage = GetAdjustedDamage(defender, totalDamage);

            // Calculate the maximum possible number of models destroyed
            var modelsDestroyed = (int)Math.Floor(adjustedTotalDamage / damageThreshold);

            // Return either the max possible models destroyed, or the total number of defending models, whichever comes first
            return Math.Min(modelsDestroyed, defender.NumberOfModels);
        }

        /// <summary>
        /// Determines the number of successful, unblocked attacks required to destroy a single model from the defending unit.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        private static int GetAttacksRequiredToDestroyOneModel(int attackerWeaponDamage, DefenderDTO? defender)
        {
            if (attackerWeaponDamage <= 0)
            {
                Debug.WriteLine($"GetAttacksRequiredToDestroyOneModel() | Attacker weapon damage is less than or equal to 0. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetAttacksRequiredToDestroyOneModel() | Defender is null. Returning 0 ...");
                return 0;
            }

            // Factor in feel no pains by calculating the feel no pain probability,
            // and using that to determine average weapon damage.
            var adjustedWeaponDamage = GetAdjustedDamage(defender, attackerWeaponDamage);

            // If the result is a decimal, round up to the next highest int value (as it will require another full attack to destroy the model).
            return (int)Math.Ceiling(defender.Wounds / adjustedWeaponDamage);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the average number of attack rolls the attacking unit is making.
        /// This includes the average of any variable attacks added to the flat number of attacks.
        /// This also takes into account the number of models in the attacking unit.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns>An integer value containing the average number of attacks made by the attacking unit.</returns>
        public static int GetAverageAttacks(AttackerDTO? attacker)
        {
            // If attacker parameter is null, return 0
            if (attacker == null)
            {
                Debug.WriteLine($"GetTotalAttacks() | Attacker is null, returning 0 ...");
                return 0;
            }

            // If either the number of models or the weapon attacks is less than 1, return 0.
            if (attacker.NumberOfModels < 1)
            {
                Debug.WriteLine($"GetTotalAttacks() | Number of models is less than 1, returning 0 ...");
                return 0;
            }

            // If both the variable scalar and the flat value of attacks are less than or equal to 0, then there are no attacks.
            if (attacker.WeaponScalarOfVariableAttacks <= 0 && attacker.WeaponFlatAttacks <= 0)
            {
                Debug.WriteLine($"GetTotalAttacks() | Attacker has no attacks value, returning 0 ...");
                return 0;
            }

            var averageVariableAttacks = attacker.WeaponScalarOfVariableAttacks * Statistics.AverageResult((int)attacker.WeaponVariableAttackType);
            var combinedAttacks = averageVariableAttacks + attacker.WeaponFlatAttacks;
            return combinedAttacks * attacker.NumberOfModels;
        }

        /// <summary>
        /// Gets the minimum number of attacks made by the attacking unit.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns>An integer value containing the minimum number attacks made by the attacking unit.</returns>
        public static int GetMinimumAttacks(AttackerDTO? attacker)
        {
            // If attacker parameter is null, return 0
            if (attacker == null)
            {
                Debug.WriteLine($"GetMinimumAttacks() | Attacker is null, returning 0 ...");
                return 0;
            }

            // If either the number of models or the weapon attacks is less than 1, return 0.
            if (attacker.NumberOfModels < 1)
            {
                Debug.WriteLine($"GetMinimumAttacks() | Number of models is less than 1, returning 0 ...");
                return 0;
            }

            // If both the variable salar and the flat value of attacks are less than or equal to 0, then there are no attacks.
            if (attacker.WeaponScalarOfVariableAttacks <= 0 && attacker.WeaponFlatAttacks <= 0)
            {
                Debug.WriteLine($"GetMinimumAttacks() | Attacker has no attacks value, returning 0 ...");
                return 0;
            }

            var minimumVariableAttacks = attacker.WeaponScalarOfVariableAttacks;
            var flatAttacks = attacker.WeaponFlatAttacks;
            var numberOfModels = attacker.NumberOfModels;

            return (minimumVariableAttacks + flatAttacks) * numberOfModels;
        }

        /// <summary>
        /// Gets the maximum number of attacks made by the attacking unit.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns>An integer value containing the maximum number attacks made by the attacking unit.</returns>
        public static int GetMaximumAttacks(AttackerDTO? attacker)
        {
            // If attacker parameter is null, return 0
            if (attacker == null)
            {
                Debug.WriteLine($"GetMaximumAttacks() | Attacker is null, returning 0 ...");
                return 0;
            }

            // If either the number of models or the weapon attacks is less than 1, return 0.
            if (attacker.NumberOfModels < 1)
            {
                Debug.WriteLine($"GetMaximumAttacks() | Number of models is less than 1, returning 0 ...");
                return 0;
            }

            // If both the variable salar and the flat value of attacks are less than or equal to 0, then there are no attacks.
            if (attacker.WeaponScalarOfVariableAttacks <= 0 && attacker.WeaponFlatAttacks <= 0)
            {
                Debug.WriteLine($"GetMaximumAttacks() | Attacker has no attacks value, returning 0 ...");
                return 0;
            }

            var maximumVariableAttacks = attacker.WeaponScalarOfVariableAttacks * (int)attacker.WeaponVariableAttackType;
            var flatAttacks = attacker.WeaponFlatAttacks;
            var numberOfModels = attacker.NumberOfModels;

            return (maximumVariableAttacks + flatAttacks) * numberOfModels;
        }

        /// <summary>
        /// Returns the mean of the attacker's hit roll distribution.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public static double GetMeanHits(AttackerDTO? attacker)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetMeanHits() | Attacker is null, returning 0 ...");
                return 0;
            }

            var averageNumberOfAttacks = GetAverageAttacks(attacker);
            var probabilityOfHit = GetProbabilityOfHit(attacker.WeaponSkill);
            return Statistics.Mean(averageNumberOfAttacks, probabilityOfHit);
        }

        /// <summary>
        /// Gets the discrete expected number of successful hit rolls, based on the average probability.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public static int GetExpectedHits(AttackerDTO? attacker)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetExpectedHits() | Attacker is null, returning 0 ...");
                return 0;
            }

            return (int)Math.Floor(GetMeanHits(attacker));
        }

        /// <summary>
        /// Returns the standard deviation of the attacker's hit roll distribution.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public static double GetStandardDeviationHits(AttackerDTO? attacker)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetStandardDeviationHits() | Attacker is null. Returning 0 ...");
                return 0;
            }

            var averageAttacks = GetAverageAttacks(attacker);
            var probabilityOfHit = GetProbabilityOfHit(attacker.WeaponSkill);
            return Statistics.StandardDeviation(averageAttacks, probabilityOfHit);
        }

        /// <summary>
        /// Returns a binomial distribution of attack roll results based on the process data.
        /// </summary>
        /// <returns>A BinomialDistribution object containing the hit success data.</returns>
        public static List<BinomialOutcome> GetBinomialDistributionHits(AttackerDTO? attacker)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetBinomialDistributionOfHits() | Attacker is null. Returning empty list ...");
                return [];
            }

            var minimumAttacks = GetMinimumAttacks(attacker);
            var maximumAttacks = GetMaximumAttacks(attacker);
            var probabilityOfHit = GetProbabilityOfHit(attacker.WeaponSkill);
            return Statistics.BinomialDistribution(minimumAttacks, maximumAttacks, probabilityOfHit);
        }

        /// <summary>
        /// Gets a distribution of all discrete survivor function values for a successful hit roll.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public static List<BinomialOutcome> GetSurvivorDistributionHits(AttackerDTO? attacker)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetSurvivorDistributionHits() | Attacker is null. Returning empty list ...");
                return new List<BinomialOutcome>();
            }

            var minimumAttacks = GetMinimumAttacks(attacker);
            var maximumAttacks = GetMaximumAttacks(attacker);
            var probabilityOfHit = GetProbabilityOfHit(attacker.WeaponSkill);
            return Statistics.SurvivorDistribution(minimumAttacks, maximumAttacks, probabilityOfHit);
        }

        /// <summary>
        /// Returns the success threshold for wounding the defender.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static int GetSuccessThresholdOfWound(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetSuccessThresholdOfWound() | Attacker is null. Returning 7+ ...");
                return 7;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetSuccessThresholdOfWound() | Defender is null. Returning 7+ ...");
                return 7;
            }

            var strength = attacker.WeaponStrength;
            var toughness = defender.Toughness;

            // The attacker's weapon Strength is greater than or equal to double the defender's Toughness.
            if (strength >= 2 * toughness)
            {
                return 2;
            }

            // The attacker's weapon Strength is greater than, but less than double, the defender's Toughness.
            else if (strength > toughness)
            {
                return 3;
            }

            // The attacker's weapon Strength is equal to the defender's Toughness.
            else if (strength == toughness)
            {
                return 4;
            }

            // The attacker's weapon Strength is less than, but more than half, the defender's Toughness.
            else if (strength > toughness / 2)
            {
                return 5;
            }

            // The attacker's weapon Strength is less than or equal to half the defender's Toughness.
            else
            {
                return 6;
            }
        }

        /// <summary>
        /// Returns the probability of succeeding in both a hit and a wound roll for any one attack.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static double GetProbabilityWound(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetProbabilityOfWound() | Attacker is null. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetProbabilityOfWound() | Defender is null. Returning 0 ...");
                return 0;
            }

            var woundSuccessThreshold = GetSuccessThresholdOfWound(attacker, defender);
            var numberOfSuccessfulResults = GetNumberOfSuccessfulResults(woundSuccessThreshold);
            return GetProbabilityOfHit(attacker.WeaponSkill) * Statistics.ProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, numberOfSuccessfulResults);
        }

        /// <summary>
        /// Returns the mean of the attacker's wound roll distribution.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static double GetMeanWounds(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetMeanWounds() | Attacker is null. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetMeanWounds() | Defender is null. Returning 0 ...");
                return 0;
            }

            var averageAttacks = GetAverageAttacks(attacker);
            var probabilityOfWound = GetProbabilityWound(attacker, defender);
            return Statistics.Mean(averageAttacks, probabilityOfWound);
        }

        /// <summary>
        /// Gets the discrete expected number of successful wound rolls, based on the average probability.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public static int GetExpectedWounds(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetExpectedWounds() | Attacker is null. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetExpectedWounds() | Defender is null. Returning 0 ...");
                return 0;
            }

            return (int)Math.Floor(GetMeanWounds(attacker, defender));
        }

        /// <summary>
        /// Returns the standard deviation of the attacker's wound roll distribution.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static double GetStandardDeviationWounds(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetStandardDeviationWounds() | Attacker is null. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetStandardDeviationWounds() | Defender is null. Returning 0 ...");
                return 0;
            }

            var averageAttacks = GetAverageAttacks(attacker);
            var probabilityOfWound = GetProbabilityWound(attacker, defender);
            return Statistics.StandardDeviation(averageAttacks, probabilityOfWound);
        }

        /// <summary>
        /// Returns a binomial distribution of wound roll results based on the process data.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static List<BinomialOutcome> GetBinomialDistributionWounds(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetBinomialDistributionWounds() | Attacker is null. Returning empty list ...");
                return new List<BinomialOutcome>();
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetBinomialDistributionWounds() | Defender is null. Returning empty list ...");
                return new List<BinomialOutcome>();
            }

            var minimumAttacks = GetMinimumAttacks(attacker);
            var maximumAttacks = GetMaximumAttacks(attacker);
            var probability = GetProbabilityWound(attacker, defender);
            return Statistics.BinomialDistribution(minimumAttacks, maximumAttacks, probability);
        }

        /// <summary>
        /// Gets a distribution of all discrete survivor function values for a successful hit and wound roll.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public static List<BinomialOutcome> GetSurvivorDistributionWounds(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetSurvivorDistributionWounds() | Attacker is null. Returning empty list ...");
                return new List<BinomialOutcome>();
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetSurvivorDistributionWounds() | Defender is null. Returning empty list ...");
                return new List<BinomialOutcome>();
            }

            var minimumAttacks = GetMinimumAttacks(attacker);
            var maximumAttacks = GetMaximumAttacks(attacker);
            var probability = GetProbabilityWound(attacker, defender);
            return Statistics.SurvivorDistribution(minimumAttacks, maximumAttacks, probability);
        }

        /// <summary>
        /// Returns the adjusted armor save of the defender after applying the attacker's armor pierce.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static int GetAdjustedArmorSave(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetAdjustedArmorSave() | Attacker is null, returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetAdjustedArmorSave() | Defender is null, returning 0 ...");
                return 0;
            }

            // If the defender has an invulnerable save, and the invulnerable save is lower than the regular save after applying armor pierce,
            // then use the invulnerable save.
            return Math.Min(defender.ArmorSave + attacker.WeaponArmorPierce, defender.InvulnerableSave);
        }

        /// <summary>
        /// Returns the probability of the attacker passing their hit and wound roll, and the defender failing their save, for any one attack.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static double GetProbabilityFailedSave(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetProbabilityOfFailedSave() | Attacker is null, returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetProbabilityOfFailedSave() | Defender is null, returning 0 ...");
                return 0;
            }

            var adjustedArmorSave = GetAdjustedArmorSave(attacker, defender);
            var numberOfSuccessfulResults = GetNumberOfSuccessfulResults(adjustedArmorSave);
            var probabilityOfSuccessfulSave = Statistics.ProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, numberOfSuccessfulResults);
            return GetProbabilityWound(attacker, defender) * (1 - probabilityOfSuccessfulSave);
        }

        /// <summary>
        /// Returns the mean of the failed save roll distribution.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static double GetMeanFailedSaves(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetMeanFailedSaves() | Attacker is null, returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetMeanFailedSaves() | Defender is null, returning 0 ...");
                return 0;
            }

            var averageAttacks = GetAverageAttacks(attacker);
            var probabilityOfFailedSave = GetProbabilityFailedSave(attacker, defender);
            return Statistics.Mean(averageAttacks, probabilityOfFailedSave);
        }

        /// <summary>
        /// Gets the discrete expected number of failed save rolls, based on the average probability.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public static int GetExpectedFailedSaves(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetExpectedFailedSaves() | Attacker is null, returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetExpectedFailedSaves() | Defender is null, returning 0 ...");
                return 0;
            }

            return (int)Math.Floor(GetMeanFailedSaves(attacker, defender));
        }

        /// <summary>
        /// Returns the standard deviation of the failed save roll distribution.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static double GetStandardDeviationFailedSaves(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetStandardDeviationFailedSaves() | Attacker is null, returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetStandardDeviationFailedSaves() | Defender is null, returning 0 ...");
                return 0;
            }

            var averageAttacks = GetAverageAttacks(attacker);
            var probabilityOfFailedSave = GetProbabilityFailedSave(attacker, defender);
            return Statistics.StandardDeviation(averageAttacks, probabilityOfFailedSave);
        }

        /// <summary>
        /// Returns a binomial distribution of rolls where the hit and wound have succeeded, and the opponent failed their save.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static List<BinomialOutcome> GetBinomialDistributionFailedSaves(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetBinomialDistributionFailedSaves() | Attacker is null. Returning empty list ...");
                return new List<BinomialOutcome>();
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetBinomialDistributionFailedSaves() | Defender is null. Returning empty list ...");
                return new List<BinomialOutcome>();
            }

            var minimumAttacks = GetMinimumAttacks(attacker);
            var maximumAttacks = GetMaximumAttacks(attacker);
            var probability = GetProbabilityFailedSave(attacker, defender);
            return Statistics.BinomialDistribution(minimumAttacks, maximumAttacks, probability);
        }

        /// <summary>
        /// Gets a distribution of all discrete survivor function values for a successful hit and wound, and a failed save roll.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public static List<BinomialOutcome> GetSurvivorDistributionFailedSaves(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetSurvivorDistributionFailedSaves() | Attacker is null. Returning empty list ...");
                return new List<BinomialOutcome>();
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetSurvivorDistributionFailedSaves() | Defender is null. Returning empty list ...");
                return new List<BinomialOutcome>();
            }

            var minimumAttacks = GetMinimumAttacks(attacker);
            var maximumAttacks = GetMaximumAttacks(attacker);
            var probability = GetProbabilityFailedSave(attacker, defender);
            return Statistics.SurvivorDistribution(minimumAttacks, maximumAttacks, probability);
        }

        /// <summary>
        /// Adjusts the given amount of damage based on any defensive modifiers.
        /// </summary>
        /// <returns></returns>
        public static double GetAdjustedDamage(DefenderDTO? defender, int damage)
        {
            // Validate inputs
            if (defender == null)
            {
                Debug.WriteLine($"GetAdjustedDamage() | Defender is null. Returning 0 ...");
                return 0;
            }

            if (damage <= 0)
            {
                Debug.WriteLine($"GetAdjustedDamage() | Input damage is less than or equal to 0. Returning 0 ...");
                return 0;
            }

            // Account for feel no pains
            // NOTE: Using the average probability for feel no pains is not a perfect way of handling this,
            // but it is SIGNIFICANTLY less resource intensive than performing calculations for every single feel no pain roll.
            var feelNoPainSuccessProbability = Statistics.ProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, GetNumberOfSuccessfulResults(defender.FeelNoPain));
            return damage * (1 - feelNoPainSuccessProbability);
        }

        /// <summary>
        /// Gets the average amount of damage done after all rolls have been completed and after all modifers and feel no pains have been accounted for.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static double GetMeanDamage(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetMeanDamageNet() | Attacker is null. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetMeanDamageNet() | Defender is null. Returning 0 ...");
                return 0;
            }

            return GetMeanFailedSaves(attacker, defender) * attacker.WeaponDamage;
        }

        /// <summary>
        /// Gets the discrete expected total amount of damage, after all modifers and feel no pains have been accounted for.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public static int GetExpectedDamage(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetExpectedDamageNet() | Attacker is null. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetExpectedDamageNet() | Defender is null. Returning 0 ...");
                return 0;
            }

            return (int)Math.Floor(GetMeanFailedSaves(attacker, defender) * attacker.WeaponDamage);
        }

        /// <summary>
        /// Gets the standard deviation of damage done after all rolls have been completed and after all modifers and feel no pains have been accounted for.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static double GetStandardDeviationDamage(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetStandardDeviationDamageNet() | Attacker is null. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetStandardDeviationDamageNet() | Defender is null. Returning 0 ...");
                return 0;
            }

            var adjustedDamage = GetAdjustedDamage(defender, attacker.WeaponDamage);
            return GetStandardDeviationFailedSaves(attacker, defender) * adjustedDamage;
        }

        /// <summary>
        /// Get the expected number of defending models that will be destroyed by the attack.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static int GetExpectedDestroyedModels(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetExpectedDestroyedModels() | Attacker is null. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetExpectedDestroyedModels() | Defender is null. Returning 0 ...");
                return 0;
            }

            var attackerWeaponDamage = attacker.WeaponDamage;
            var totalDamage = GetExpectedDamage(attacker, defender);
            return GetModelsDestroyed(attackerWeaponDamage, totalDamage, defender);
        }

        /// <summary>
        /// Gets the standard deviation of destroyed models.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static int GetStandardDeviationDestroyedModels(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetStandardDeviationDestroyedModels() | Attacker is null. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetStandardDeviationDestroyedModels() | Defender is null. Returning 0 ...");
                return 0;
            }

            var attackerWeaponDamage = attacker.WeaponDamage;
            var standardDeviationDamage = (int)Math.Floor(GetStandardDeviationDamage(attacker, defender));
            return GetModelsDestroyed(attackerWeaponDamage, standardDeviationDamage, defender);
        }

        /// <summary>
        /// Gets the binomial distribution of destroyed models.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static List<BinomialOutcome> GetBinomialDistributionDestroyedModels(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetBinomialDistributionDestroyedModels() | Attacker is null. Returning empty list ...");
                return new List<BinomialOutcome>();
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetBinomialDistributionDestroyedModels() | Defender is null. Returning empty list ...");
                return new List<BinomialOutcome>();
            }

            var minimumAttacks = GetMinimumAttacks(attacker);
            var maximumAttacks = GetMaximumAttacks(attacker);
            var probability = GetProbabilityFailedSave(attacker, defender);
            var groupSuccessCount = GetAttacksRequiredToDestroyOneModel(attacker.WeaponDamage, defender);
            return Statistics.BinomialDistribution(minimumAttacks, maximumAttacks, probability, groupSuccessCount);
        }

        /// <summary>
        /// Gets the survivor distribution of destroyed models.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static List<BinomialOutcome> GetSurvivorDistributionDestroyedModels(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetSurvivorDistributionDestroyedModels() | Attacker is null. Returning empty list ...");
                return new List<BinomialOutcome>();
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetSurvivorDistributionDestroyedModels() | Defender is null. Returning empty list ...");
                return new List<BinomialOutcome>();
            }

            var minimumAttacks = GetMinimumAttacks(attacker);
            var maximumAttacks = GetMaximumAttacks(attacker);
            var probability = GetProbabilityFailedSave(attacker, defender);
            var groupSuccessCount = GetAttacksRequiredToDestroyOneModel(attacker.WeaponDamage, defender);
            return Statistics.SurvivorDistribution(minimumAttacks, maximumAttacks, probability, groupSuccessCount);
        }

        #endregion
    }
}
