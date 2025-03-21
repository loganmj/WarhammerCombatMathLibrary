﻿using WarhammerCombatMathLibrary.Data;

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
        /// Returns the success threshold for succeeding a hit roll.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        private static int GetNumberOfSuccessfulResults(int successThreshold)
        {
            return 6 - (successThreshold - 1);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the total number of attack rolls the attacker is making.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public static int GetTotalNumberOfAttacks(AttackerDTO attacker)
        {
            return attacker.NumberOfModels * attacker.WeaponAttacks;
        }

        /// <summary>
        /// Returns the probability of succeeding a roll with a single dice, given the desired success threshold.
        /// </summary>
        /// <returns>A double value containing the probability of success for a single trial.</returns>
        public static double GetProbabilityOfHit(AttackerDTO attacker)
        {
            return Statistics.ProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, GetNumberOfSuccessfulResults(attacker.WeaponSkill));
        }

        /// <summary>
        /// Returns the success threshold for wounding the defender.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static int GetSuccessThresholdOfWound(AttackerDTO attacker, DefenderDTO defender)
        {
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
        public static double GetProbabilityOfWound(AttackerDTO attacker, DefenderDTO defender)
        {
            var woundSuccessThreshold = GetSuccessThresholdOfWound(attacker, defender);
            var numberOfSuccessfulResults = GetNumberOfSuccessfulResults(woundSuccessThreshold);
            return GetProbabilityOfHit(attacker) * Statistics.ProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, numberOfSuccessfulResults);
        }

        /// <summary>
        /// Returns a binomial distribution of attack roll results based on the process data.
        /// </summary>
        /// <returns>A BinomialDistribution object containing the hit success data.</returns>
        public static ProbabilityDistribution GetBinomialDistributionOfHits(AttackerDTO attacker)
        {
            return Statistics.BinomialDistribution(GetTotalNumberOfAttacks(attacker), GetProbabilityOfHit(attacker));
        }

        /// <summary>
        /// Returns the mean of the attacker's hit roll distribution.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public static double GetMeanHits(AttackerDTO attacker)
        {
            return Statistics.GetMean(GetTotalNumberOfAttacks(attacker), GetProbabilityOfHit(attacker));
        }

        /// <summary>
        /// Gets the discrete expected number of successful hit rolls, based on the average probability.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public static int GetExpectedHits(AttackerDTO attacker)
        {
            return (int)Math.Floor(GetMeanHits(attacker));
        }

        /// <summary>
        /// Returns the standard deviation of the attacker's hit roll distribution.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public static double GetStandardDeviationHits(AttackerDTO attacker)
        {
            return Statistics.GetStandardDeviation(GetTotalNumberOfAttacks(attacker), GetProbabilityOfHit(attacker));
        }

        /// <summary>
        /// Returns the lower and upper range for expected successful hits.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns>A Tuple containing the lower and upper range values. Item1 is the lower bound, Item2 is the upper bound.</returns>
        public static Tuple<int, int> GetExpectedRangeHits(AttackerDTO attacker)
        {
            var lowerBound = GetExpectedHits(attacker) - (int)Math.Floor(GetStandardDeviationHits(attacker));
            var upperBound = GetExpectedHits(attacker) + (int)Math.Floor(GetStandardDeviationHits(attacker));
            return Tuple.Create(lowerBound, upperBound);
        }

        /// <summary>
        /// Returns the upper cumulative distribution of the attacker's hit roll.
        /// </summary>
        /// <returns></returns>
        public static ProbabilityDistribution GetUpperCumulativeDistributionOfHits(AttackerDTO attacker)
        {
            return Statistics.UpperCumulativeDistribution(GetTotalNumberOfAttacks(attacker), GetProbabilityOfHit(attacker));
        }

        /// <summary>
        /// Returns a binomial distribution of wound roll results based on the process data.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static ProbabilityDistribution GetBinomialDistributionOfWounds(AttackerDTO attacker, DefenderDTO defender)
        {
            return Statistics.BinomialDistribution(GetTotalNumberOfAttacks(attacker), GetProbabilityOfWound(attacker, defender));
        }

        /// <summary>
        /// Returns the mean of the attacker's wound roll distribution.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static double GetMeanWounds(AttackerDTO attacker, DefenderDTO defender)
        {
            return Statistics.GetMean(GetTotalNumberOfAttacks(attacker), GetProbabilityOfWound(attacker, defender));
        }

        /// <summary>
        /// Gets the discrete expected number of successful wound rolls, based on the average probability.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public static int GetExpectedWounds(AttackerDTO attacker, DefenderDTO defender)
        {
            return (int)Math.Floor(GetMeanWounds(attacker, defender));
        }

        /// <summary>
        /// Returns the standard deviation of the attacker's wound roll distribution.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static double GetStandardDeviationWounds(AttackerDTO attacker, DefenderDTO defender)
        {
            return Statistics.GetStandardDeviation(GetTotalNumberOfAttacks(attacker), GetProbabilityOfWound(attacker, defender));
        }

        /// <summary>
        /// Returns the lower and upper range for expected successful wounds.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns>A Tuple containing the lower and upper range values. Item1 is the lower bound, Item2 is the upper bound.</returns>
        public static Tuple<int, int> GetExpectedRangeWounds(AttackerDTO attacker, DefenderDTO defender)
        {
            var expectedDeviation = (int)Math.Floor(GetStandardDeviationWounds(attacker, defender));
            var lowerBound = GetExpectedWounds(attacker, defender) - expectedDeviation;
            var upperBound = GetExpectedWounds(attacker, defender) + expectedDeviation;
            return Tuple.Create(lowerBound, upperBound);
        }

        /// <summary>
        /// Returns the upper cumulative distribution of the attacker's wound roll.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static ProbabilityDistribution GetUpperCumulativeDistributionOfWounds(AttackerDTO attacker, DefenderDTO defender)
        {
            return Statistics.UpperCumulativeDistribution(GetTotalNumberOfAttacks(attacker), GetProbabilityOfWound(attacker, defender));
        }

        /// <summary>
        /// Returns the adjusted armor save of the defender after applying the attacker's armor pierce.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static int GetAdjustedArmorSave(AttackerDTO attacker, DefenderDTO defender)
        {
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
        public static double GetProbabilityOfFailedSave(AttackerDTO attacker, DefenderDTO defender)
        {
            var adjustedArmorSave = GetAdjustedArmorSave(attacker, defender);
            var numberOfSuccessfulResults = GetNumberOfSuccessfulResults(adjustedArmorSave);
            var probabilityOfSuccessfulSave = Statistics.ProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, numberOfSuccessfulResults);
            return GetProbabilityOfWound(attacker, defender) * (1 - probabilityOfSuccessfulSave);
        }

        /// <summary>
        /// Returns a binomial distribution of rolls where the hit and wound have succeeded, and the opponent failed their save.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static ProbabilityDistribution GetBinomialDistributionOfFailSaves(AttackerDTO attacker, DefenderDTO defender)
        {
            return Statistics.BinomialDistribution(GetTotalNumberOfAttacks(attacker), GetProbabilityOfFailedSave(attacker, defender));
        }

        /// <summary>
        /// Returns the mean of the failed save roll distribution.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static double GetMeanFailedSaves(AttackerDTO attacker, DefenderDTO defender)
        {
            return Statistics.GetMean(GetTotalNumberOfAttacks(attacker), GetProbabilityOfFailedSave(attacker, defender));
        }

        /// <summary>
        /// Gets the discrete expected number of failed save rolls, based on the average probability.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public static int GetExpectedFailedSaves(AttackerDTO attacker, DefenderDTO defender)
        {
            return (int)Math.Floor(GetMeanFailedSaves(attacker, defender));
        }

        /// <summary>
        /// Returns the standard deviation of the failed save roll distribution.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static double GetStandardDeviationFailedSaves(AttackerDTO attacker, DefenderDTO defender)
        {
            return Statistics.GetStandardDeviation(GetTotalNumberOfAttacks(attacker), GetProbabilityOfFailedSave(attacker, defender));
        }

        /// <summary>
        /// Returns the lower and upper range for expected failed saves.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns>A Tuple containing the lower and upper range values. Item1 is the lower bound, Item2 is the upper bound.</returns>
        public static Tuple<int, int> GetExpectedRangeFailedSaves(AttackerDTO attacker, DefenderDTO defender)
        {
            var expectedDeviation = (int)Math.Floor(GetStandardDeviationFailedSaves(attacker, defender));
            var lowerBound = GetExpectedFailedSaves(attacker, defender) - expectedDeviation;
            var upperBound = GetExpectedFailedSaves(attacker, defender) + expectedDeviation;
            return Tuple.Create(lowerBound, upperBound);
        }

        /// <summary>
        /// Returns the upper cumulative distribution of the failed save roll.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static ProbabilityDistribution GetUpperCumulativeDistributionOfFailedSaves(AttackerDTO attacker, DefenderDTO defender)
        {
            return Statistics.UpperCumulativeDistribution(GetTotalNumberOfAttacks(attacker), GetProbabilityOfFailedSave(attacker, defender));
        }

        /// <summary>
        /// Gets the average amount of damage done after all rolls have been completed.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static double GetMeanDamage(AttackerDTO attacker, DefenderDTO defender)
        {
            return GetMeanFailedSaves(attacker, defender) * attacker.WeaponDamage;
        }

        /// <summary>
        /// Gets the discrete expected total amount of damage, based on the average probability and the amount of damage per attack.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public static int GetExpectedDamage(AttackerDTO attacker, DefenderDTO defender)
        {
            return (int)Math.Floor(GetMeanDamage(attacker, defender));
        }

        /// <summary>
        /// Gets the standard deviation of damage done after all rolls have been completed.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static double GetStandardDeviationDamage(AttackerDTO attacker, DefenderDTO defender)
        {
            return GetStandardDeviationFailedSaves(attacker, defender) * attacker.WeaponDamage;
        }

        /// <summary>
        /// Returns the lower and upper range for expected total damage.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns>A Tuple containing the lower and upper range values. Item1 is the lower bound, Item2 is the upper bound.</returns>
        public static Tuple<int, int> GetExpectedRangeDamage(AttackerDTO attacker, DefenderDTO defender)
        {
            var expectedDeviation = (int)Math.Floor(GetStandardDeviationDamage(attacker, defender));
            var lowerBound = GetExpectedDamage(attacker, defender) - expectedDeviation;
            var upperBound = GetExpectedDamage(attacker, defender) + expectedDeviation;
            return Tuple.Create(lowerBound, upperBound);
        }

        #endregion
    }
}
