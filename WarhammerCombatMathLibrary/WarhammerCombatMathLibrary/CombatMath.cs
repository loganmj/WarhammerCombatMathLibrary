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

        #region Public Methods

        /// <summary>
        /// Returns the success threshold for succeeding on a given die roll, with a given success threshold.
        /// Note that in Warhammer 40k, a roll of 1 always fails, and a roll of 6 always succeeds.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public static int GetNumberOfSuccessfulResults(int successThreshold)
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
        /// Returns the total number of attack rolls the attacker is making.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public static int GetTotalNumberOfAttacks(AttackerDTO? attacker)
        {
            // If attacker parameter is null, return 0
            if (attacker == null)
            {
                Debug.WriteLine($"GetTotalNumberOfAttacks() | Attacker is null, returning 0 ...");
                return 0;
            }

            // If either the number of models or the weapon attacks is less than 1, return 0.
            if (attacker.NumberOfModels < 1)
            {
                Debug.WriteLine($"GetTotalNumberOfAttacks() | Number of models is less than 1, returning 0 ...");
                return 0;
            }

            if (attacker.WeaponAttacks < 1)
            {
                Debug.WriteLine($"GetTotalNumberOfAttacks() | Weapon Attacks is less than 1, returning 0 ...");
                return 0;
            }

            // Perform calculation
            return attacker.NumberOfModels * attacker.WeaponAttacks;
        }

        /// <summary>
        /// Returns the probability of succeeding a roll with a single dice, given the desired success threshold.
        /// </summary>
        /// <returns>A double value containing the probability of success for a single trial.</returns>
        public static double GetProbabilityOfHit(AttackerDTO? attacker)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetProbabilityOfHit() | Attacker is null, returning 0 ...");
                return 0;
            }

            return Statistics.ProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, GetNumberOfSuccessfulResults(attacker.WeaponSkill));
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

            return Statistics.Mean(GetTotalNumberOfAttacks(attacker), GetProbabilityOfHit(attacker));
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

            return Statistics.StandardDeviation(GetTotalNumberOfAttacks(attacker), GetProbabilityOfHit(attacker));
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

            var totalAttacks = GetTotalNumberOfAttacks(attacker);
            var probabilityOfHit = GetProbabilityOfHit(attacker);
            return Statistics.BinomialDistribution(totalAttacks, probabilityOfHit);
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

            var numberOfTrials = GetTotalNumberOfAttacks(attacker);
            var probability = GetProbabilityOfHit(attacker);
            return Statistics.SurvivorDistribution(numberOfTrials, probability);
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
            return GetProbabilityOfHit(attacker) * Statistics.ProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, numberOfSuccessfulResults);
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

            return Statistics.Mean(GetTotalNumberOfAttacks(attacker), GetProbabilityWound(attacker, defender));
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

            return Statistics.StandardDeviation(GetTotalNumberOfAttacks(attacker), GetProbabilityWound(attacker, defender));
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

            var numberOfTrials = GetTotalNumberOfAttacks(attacker);
            var probability = GetProbabilityWound(attacker, defender);
            return Statistics.BinomialDistribution(numberOfTrials, probability);
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

            var numberOfTrials = GetTotalNumberOfAttacks(attacker);
            var probability = GetProbabilityWound(attacker, defender);
            return Statistics.SurvivorDistribution(numberOfTrials, probability);
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

            return Statistics.Mean(GetTotalNumberOfAttacks(attacker), GetProbabilityFailedSave(attacker, defender));
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

            return Statistics.StandardDeviation(GetTotalNumberOfAttacks(attacker), GetProbabilityFailedSave(attacker, defender));
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

            var numberOfTrials = GetTotalNumberOfAttacks(attacker);
            var probability = GetProbabilityFailedSave(attacker, defender);
            return Statistics.BinomialDistribution(numberOfTrials, probability);
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

            var numberOfTrials = GetTotalNumberOfAttacks(attacker);
            var probability = GetProbabilityFailedSave(attacker, defender);
            return Statistics.SurvivorDistribution(numberOfTrials, probability);
        }

        /// <summary>
        /// Gets the average amount of damage done after all rolls have been completed.
        /// This is the average amount of gross damage, before any modifers or feel no pains have been accounted for.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static double GetMeanDamageGross(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetMeanDamage() | Attacker is null. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetMeanDamage() | Defender is null. Returning 0 ...");
                return 0;
            }

            return GetMeanFailedSaves(attacker, defender) * attacker.WeaponDamage;
        }

        /// <summary>
        /// Gets the discrete expected total amount of damage, based on the average probability and the amount of damage per attack.
        /// This is the expected amount of gross damage, before any modifers or feel no pains have been accounted for.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public static int GetExpectedDamageGross(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetExpectedDamage() | Attacker is null. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetExpectedDamage() | Defender is null. Returning 0 ...");
                return 0;
            }

            return (int)Math.Floor(GetMeanFailedSaves(attacker, defender) * attacker.WeaponDamage);
        }

        /// <summary>
        /// Gets the standard deviation of damage done after all rolls have been completed.
        /// This is the standard deviation of gross damage, before any modifers or feel no pains have been accounted for.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static double GetStandardDeviationDamageGross(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetStandardDeviationDamage() | Attacker is null. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetStandardDeviationDamage() | Defender is null. Returning 0 ...");
                return 0;
            }

            var adjustedDamage = GetAdjustedDamage(defender, attacker.WeaponDamage);
            return GetStandardDeviationFailedSaves(attacker, defender) * adjustedDamage;
        }

        /// <summary>
        /// Adjusts the given amount of damage based on the expected defender feel no pain rolls.
        /// </summary>
        /// <returns></returns>
        public static double GetAdjustedDamage(DefenderDTO defender, int damage)
        {
            var feelNoPainSuccessProbability = Statistics.ProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, GetNumberOfSuccessfulResults(defender.FeelNoPain));
            return damage * (1 - feelNoPainSuccessProbability);
        }

        /// <summary>
        /// Gets the average amount of damage done after all rolls have been completed and after all modifers and feel no pains have been accounted for.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static double GetMeanDamageNet(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetMeanDamage() | Attacker is null. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetMeanDamage() | Defender is null. Returning 0 ...");
                return 0;
            }

            return GetMeanFailedSaves(attacker, defender) * attacker.WeaponDamage;
        }

        /// <summary>
        /// Gets the discrete expected total amount of damage, after all modifers and feel no pains have been accounted for.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public static int GetExpectedDamageNet(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetExpectedDamage() | Attacker is null. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetExpectedDamage() | Defender is null. Returning 0 ...");
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
        public static double GetStandardDeviationDamageNet(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetStandardDeviationDamage() | Attacker is null. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetStandardDeviationDamage() | Defender is null. Returning 0 ...");
                return 0;
            }

            var adjustedDamage = GetAdjustedDamage(defender, attacker.WeaponDamage);
            return GetStandardDeviationFailedSaves(attacker, defender) * adjustedDamage;
        }

        /// <summary>
        /// Determines the maximum possible number of models destroyed given a specified amount of applied damage.
        /// </summary>
        /// <param name="attacker">The attacking unit data.</param>
        /// <param name="defender">The defending unit data.</param>
        /// <param name="totalDamage">The total amount of damage done to the unit.</param>
        /// <returns></returns>
        public static int GetModelsDestroyed(AttackerDTO? attacker, DefenderDTO? defender, int totalDamage)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetModelsDestroyed() | Attacker is null. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetModelsDestroyed() | Defender is null. Returning 0 ...");
                return 0;
            }

            if (totalDamage <= 0) 
            {
                Debug.WriteLine($"GetModelsDestroyed() | Total damage is less than or equal to 0. Returning 0 ...");
                return 0;
            }

            // Determine the divisor based on which value is larger: the defender's wounds per model, or the attacker's weapon damage.
            var damageThreshold = Math.Max(defender.Wounds, attacker.WeaponDamage);

            // Adjust the total damage based on the defender Feel No Pain probability
            var adjustedTotalDamage = GetAdjustedDamage(defender, totalDamage);

            // Calculate the maximum possible number of models destroyed
            var modelsDestroyed = (int)Math.Floor(adjustedTotalDamage / damageThreshold);

            // Return either the max possible models destroyed, or the total number of defending models, whichever comes first
            return Math.Min(modelsDestroyed, defender.NumberOfModels);
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

            return GetModelsDestroyed(attacker, defender, GetExpectedDamageGross(attacker, defender));
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

            var standardDeviationDamage = (int)Math.Floor(GetStandardDeviationDamageNet(attacker, defender));
            return GetModelsDestroyed(attacker, defender, standardDeviationDamage);
        }

        /// <summary>
        /// Determines the number of successful, unblocked attacks required to destroy a single model from the defending unit.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static int GetAttacksRequiredToDestroyOneModel(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetAttacksRequiredToDestroyOneModel() | Attacker is null. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetAttacksRequiredToDestroyOneModel() | Defender is null. Returning 0 ...");
                return 0;
            }

            // Factor in feel no pains by calculating the feel no pain probability,
            // and using that to determine average weapon damage.
            var adjustedWeaponDamage = GetAdjustedDamage(defender, attacker.WeaponDamage);

            // If the result is a decimal, round up to the next highest int value (as it will require another full attack to destroy the model).
            return (int)Math.Ceiling(defender.Wounds / adjustedWeaponDamage);
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

            // Use the alternate binomial calculation that takes into account requiring groups of successful die rolls to destroy a single model
            var numberOfTrials = GetTotalNumberOfAttacks(attacker);
            var probability = GetProbabilityFailedSave(attacker, defender);
            var groupSuccessCount = GetAttacksRequiredToDestroyOneModel(attacker, defender);
            return Statistics.BinomialDistribution(numberOfTrials, probability, groupSuccessCount);
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

            // Use the alternate binomial calculation that takes into account requiring groups of successful die rolls to destroy a single model
            var numberOfTrials = GetTotalNumberOfAttacks(attacker);
            var probability = GetProbabilityFailedSave(attacker, defender);
            var groupSuccessCount = GetAttacksRequiredToDestroyOneModel(attacker, defender);
            return Statistics.SurvivorDistribution(numberOfTrials, probability, groupSuccessCount);
        }

        #endregion
    }
}
