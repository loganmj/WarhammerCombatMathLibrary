using MathNet.Numerics;
using MathNet.Numerics.Distributions;
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
        /// Returns the success threshold for succeeding a hit roll.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public static int GetNumberOfSuccessfulResults(int successThreshold)
        {
            // If the success threshold is greater than 6, there are no successful results
            if (successThreshold > POSSIBLE_RESULTS_SIX_SIDED_DIE)
            {
                Debug.WriteLine($"GetNumberOfSuccessfulResults() | Success threshold is greater than {POSSIBLE_RESULTS_SIX_SIDED_DIE}, returning 0 ...");
                return 0;
            }

            // If the success threshold is less than 2, there are no fail results
            if (successThreshold < 2)
            {
                Debug.WriteLine($"GetNumberOfSuccessfulResults() | Success threshold is less than two, returning {POSSIBLE_RESULTS_SIX_SIDED_DIE} ...");
                return POSSIBLE_RESULTS_SIX_SIDED_DIE;
            }

            return POSSIBLE_RESULTS_SIX_SIDED_DIE - (successThreshold - 1);
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

            return Statistics.GetMean(GetTotalNumberOfAttacks(attacker), GetProbabilityOfHit(attacker));
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

            return Statistics.GetStandardDeviation(GetTotalNumberOfAttacks(attacker), GetProbabilityOfHit(attacker));
        }

        /// <summary>
        /// Returns a binomial distribution of attack roll results based on the process data.
        /// </summary>
        /// <returns>A BinomialDistribution object containing the hit success data.</returns>
        public static List<BinomialOutcome> GetBinomialDistributionOfHits(AttackerDTO? attacker)
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
        /// Gets a binomial distribution of successful hit rolls.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns>A Binomial object for the hit roll.</returns>
        public static Binomial GetHitsBinomial(AttackerDTO? attacker) 
        {
            if (attacker == null) 
            {
                Debug.WriteLine($"GetHitsBinomial() | Attacker is null. Returning binomial for n=0, p=0 ...");
                return new Binomial(0, 0);
            }

            var numberOfTrials = GetTotalNumberOfAttacks(attacker);
            var probability = GetProbabilityOfHit(attacker);
            return new Binomial(probability, numberOfTrials);
        }

        /// <summary>
        /// Gets the survivor function probability for the specified number of successes k.
        /// That is, the cumulative probability of all probabilities P(X≥k) in the distribution.
        /// </summary>
        /// <param name="binomial">The distribution to of probabilities.</param>
        /// <param name="successes">The minimum number of successes.</param>
        /// <returns></returns>
        public static double GetSurvivorProbability(Binomial? binomial, int successes)
        {
            // Validate inputs
            if (binomial == null) 
            {
                Debug.WriteLine($"GetSurvivorProbability() | Binomial is null. Returning 0 ...");
                return 0;
            }

            if (successes < 0) 
            {
                Debug.WriteLine($"GetSurvivorProbability() | Successes is less than 0. Returning 0 ...");
                return 0;
            }

            return 1 - binomial.CumulativeDistribution(successes - 1);
        }

        /// <summary>
        /// Gets a distribution of all discrete survivor function values for a successful hit roll.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public static List<BinomialOutcome> GetHitsSurvivorDistribution(AttackerDTO? attacker) 
        {
            var numberOfTrials = GetTotalNumberOfAttacks(attacker);
            var probability = GetProbabilityOfHit(attacker);
            var hitBinomial = new Binomial(probability, numberOfTrials);
            var survivorDistribution = new List<BinomialOutcome>();

            for (int trial = 0; trial <= numberOfTrials; trial++)
            {
                survivorDistribution.Add(new BinomialOutcome(trial, GetSurvivorProbability(hitBinomial, trial)));
            }

            return survivorDistribution;
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
        /// Returns a binomial distribution of wound roll results based on the process data.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static List<BinomialOutcome> GetBinomialDistributionOfWounds(AttackerDTO attacker, DefenderDTO defender)
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
        /// Returns the upper cumulative distribution P(X≥k) of the attacker's wound roll.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static List<BinomialOutcome> GetUpperCumulativeDistributionOfWounds(AttackerDTO attacker, DefenderDTO defender)
        {
            return Statistics.UpperCumulativeDistribution(GetTotalNumberOfAttacks(attacker), GetProbabilityOfWound(attacker, defender));
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
        public static double GetProbabilityOfFailedSave(AttackerDTO? attacker, DefenderDTO? defender)
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
            return GetProbabilityOfWound(attacker, defender) * (1 - probabilityOfSuccessfulSave);
        }

        /// <summary>
        /// Returns a binomial distribution of rolls where the hit and wound have succeeded, and the opponent failed their save.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static List<BinomialOutcome> GetBinomialDistributionOfFailSaves(AttackerDTO attacker, DefenderDTO defender)
        {
            return Statistics.BinomialDistribution(GetTotalNumberOfAttacks(attacker), GetProbabilityOfFailedSave(attacker, defender));
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
        /// Returns the upper cumulative distribution P(X≥k) of the failed save roll.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static List<BinomialOutcome> GetUpperCumulativeDistributionOfFailedSaves(AttackerDTO attacker, DefenderDTO defender)
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
