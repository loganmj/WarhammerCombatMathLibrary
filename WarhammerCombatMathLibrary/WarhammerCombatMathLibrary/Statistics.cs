using MathNet.Numerics.Distributions;
using System.Diagnostics;
using System.Numerics;
using WarhammerCombatMathLibrary.Data;

namespace WarhammerCombatMathLibrary
{
    /// <summary>
    /// A static class that provides statstical math functions.
    /// </summary>
    public static class Statistics
    {
        #region Public Methods

        /// <summary>
        /// Calculates the probability of success for a single trial.
        /// </summary>
        /// <param name="numberOfPossibleResults"></param>
        /// <param name="numberOfSuccessfulResults"></param>
        /// <returns></returns>
        public static double ProbabilityOfSuccess(int numberOfPossibleResults, int numberOfSuccessfulResults)
        {
            // Validate parameters
            if (numberOfPossibleResults <= 0)
            {
                Debug.WriteLine($"ProbabilityOfSuccess() | Number of possible results is less than or equal to 0. Returning 0 ...");
                return 0;
            }

            if (numberOfSuccessfulResults < 1)
            {
                Debug.WriteLine($"ProbabilityOfSuccess() | Number of successful results is less than 1. Returning 0 ...");
                return 0;
            }

            if (numberOfSuccessfulResults > numberOfPossibleResults)
            {
                Debug.WriteLine($"ProbabilityOfSuccess() | Number of successful results is greater than the number of possible results. Returning 1 ...");
                return 1;
            }

            // Perform calculation
            return (double)numberOfSuccessfulResults / numberOfPossibleResults;
        }

        /// <summary>
        /// Calculates the binomial coefficient, determining the number of combinations of k elements
        /// in a population of n, independent of order.
        /// </summary>
        /// <param name="population">The population of the group or set.</param>
        /// <param name="combinationSize">The number elements in a unique combination.</param>
        /// <returns>A double value containing the binomial coefficient.</returns>
        public static BigInteger BinomialCoefficient(int population, int combinationSize)
        {
            // Validate parameters
            if (population < 0)
            {
                Debug.WriteLine($"BinomialCoefficient() | Population is less than 0. Returning 0 ...");
                return 0;
            }

            if (combinationSize < 0)
            {
                Debug.WriteLine($"BinomialCoefficient() | Combination size is less than 0. Returning 0 ...");
                return 0;
            }

            if (combinationSize > population)
            {
                Debug.WriteLine("BinomialCoefficient() | Combination size must be less than or equal to the total population. Returning 0 ...");
                return 0;
            }

            // Perform calculation
            var factorialTotal = MathUtilities.Factorial(population);
            var factorialCombination = MathUtilities.Factorial(combinationSize);
            var factorialDifference = MathUtilities.Factorial(population - combinationSize);
            return (factorialTotal / (factorialCombination * factorialDifference));
        }

        /// <summary>
        /// Calculates the probability for the success of a given number of trials
        /// using a specified probability of success for a single trial.
        /// </summary>
        /// <param name="probability">Probability of success for a single trial.</param>
        /// <param name="numberOfSuccesses">Number of trials.</param>
        /// <returns>A double value containing the probability that all trials will be successful.</returns>
        public static double ProbabilityOfMultipleSuccesses(double probability, int numberOfSuccesses)
        {
            // Validate parameters
            if (probability <= 0)
            {
                Debug.WriteLine($"ProbabilityOfMultipleSuccesses() | Probability is less than or equal to 0. Returning 0 ...");
                return 0;
            }

            if (probability >= 1)
            {
                Debug.WriteLine($"ProbabilityOfMultipleSuccesses() | Probability is greater than or equal to 1. Returning 1 ...");
                return 1;
            }

            if (numberOfSuccesses < 0)
            {
                Debug.WriteLine($"ProbabilityOfMultipleSuccesses() | Number of successes is less than 0. Returning 0 ...");
                return 0;
            }

            // Perform calculation
            return Math.Pow(probability, numberOfSuccesses);
        }

        /// <summary>
        /// Calculates the probability mass function for a given number of successes 
        /// when a given number of dice are rolled with a given success threshold.
        /// The function can be broken down as a combination of the following:
        /// - The binomial coefficient, describing the number of unique combinations of results that can contain the desired number of successes.
        /// - The probability of finding the exact specified number of successful results.
        /// - The probability of finding the remaining results to be failures.
        /// </summary>
        /// <param name="numberOfTrials">The total number of dice.</param>
        /// <param name="numberOfSuccesses">The number of successes.</param>
        /// <param name="probability">The probability of success for a single die roll.</param>
        /// <returns></returns>
        public static double ProbabilityMassFunction(int numberOfTrials, int numberOfSuccesses, double probability)
        {
            // Validate parameters
            if (numberOfTrials < 1)
            {
                Debug.WriteLine($"ProbabilityMassFunction() | Number of trials is less than 1. Returning 0 ...");
                return 0;
            }

            if (numberOfSuccesses < 0)
            {
                Debug.WriteLine($"ProbabilityMassFunction() | Number of successes is less than 0. Returning 0 ...");
                return 0;
            }

            if (numberOfSuccesses > numberOfTrials)
            {
                Debug.WriteLine($"ProbabilityMassFunction() | Number of successes is greater than number of trials. Returning 0 ...");
                return 0;
            }

            if (probability <= 0)
            {
                Debug.WriteLine($"ProbabilityMassFunction() | Probability is less than or equal to 0. Returning 0 ...");
                return 0;
            }

            if (probability >= 1)
            {
                Debug.WriteLine($"ProbabilityMassFunction() | Probability is greater than or equal to 1. Returning 1 ...");
                return 1;
            }

            // Perform calculation
            var binomialCoefficient = BinomialCoefficient(numberOfTrials, numberOfSuccesses);
            var successProbability = ProbabilityOfMultipleSuccesses(probability, numberOfSuccesses);
            var failureProbability = ProbabilityOfMultipleSuccesses(1 - probability, numberOfTrials - numberOfSuccesses);
            return (double)binomialCoefficient * successProbability * failureProbability;
        }

        /// <summary>
        /// Calculates the binomial distribution of trial data.
        /// Optionally calculates the binomial distribution of trial data, assuming that a group of a given number of trial successes is considered a single success
        /// in the context of the distribution.
        /// Example: If 'groupSuccessCount' = 2, then the total number of trials is divided by 2, and any combination of 2 successful trials is considered a single success
        /// in the context of the distribution.
        /// </summary>
        /// <param name="numberOfTrials">The number of trials in the process.</param>
        /// <param name="probability">The probability of success for a single trial.</param>
        /// <param name="groupSuccessCount">The number of grouped trial successes that count as a success. Default is 1 (one trial success equals one binomial success)</param>
        /// <returns>A binomial distribution of trial results and their respective probabilities.</returns>
        public static List<BinomialOutcome> BinomialDistribution(int numberOfTrials, double probability, int groupSuccessCount = 1)
        {
            // Validate parameters
            if (numberOfTrials < 1)
            {
                Debug.WriteLine($"BinomialDistribution() | Number of trials is less than 1.");
                return [new(0, 1)];
            }

            if (probability <= 0)
            {
                Debug.WriteLine($"BinomialDistribution() | Probability is less than or equal to 0.");

                // The probability of 0 successes should be 1, all other probabilities should be 0.
                var adjustedDistribution = new List<BinomialOutcome>
                {
                    new(0, 1)
                };

                for (int k = 1; k <= numberOfTrials; k++)
                {
                    adjustedDistribution.Add(new BinomialOutcome(k, 0));
                }

                return adjustedDistribution;
            }

            if (probability >= 1)
            {
                Debug.WriteLine($"BinomialDistribution() | Probability is greater than or equal to 1.");

                // All probabilities should be 0, except the probability of all successes should be 1.
                var adjustedDistribution = new List<BinomialOutcome>();

                for (int k = 0; k <= numberOfTrials - 1; k++)
                {
                    adjustedDistribution.Add(new BinomialOutcome(k, 0));
                }

                adjustedDistribution.Add(new BinomialOutcome(numberOfTrials, 1));

                return adjustedDistribution;
            }

            if (groupSuccessCount < 1)
            {
                Debug.WriteLine($"BinomialDistribution() | Group success count is less than 1.");
                return new List<BinomialOutcome> { new BinomialOutcome(0, 1) };
            }

            if (groupSuccessCount > numberOfTrials)
            {
                Debug.WriteLine($"BinomialDistribution() | Group success count is greater than the total number of trials.");
                return new List<BinomialOutcome> { new BinomialOutcome(0, 1) };
            }

            // Create distribution
            var distribution = new List<BinomialOutcome>();

            for (int k = 0; k <= Math.Floor((double)numberOfTrials / groupSuccessCount); k++)
            {
                distribution.Add(new BinomialOutcome
                {
                    Successes = k,
                    Probability = ProbabilityMassFunction(numberOfTrials, k * groupSuccessCount, probability)
                });
            }

            return distribution;
        }

        /// <summary>
        /// Calculates the lower cumulative probability of trial data.
        /// Lower cumulative probability is the probability of achieving a result less than or equal to X successes such that P(X≤k).
        /// </summary>
        /// <param name="numberOfTrials"></param>
        /// <param name="numberOfSuccesses"></param>
        /// <param name="probability"></param>
        /// <returns>A double containing the cumulative probability value.</returns>
        public static double LowerCumulativeProbability(int numberOfTrials, int numberOfSuccesses, double probability)
        {
            // Validate parameters
            if (numberOfTrials < 1)
            {
                Debug.WriteLine($"LowerCumulativeProbability() | Number of trials is less than 1. Returning 0 ...");
                return 0;
            }

            if (numberOfSuccesses < 0)
            {
                Debug.WriteLine($"LowerCumulativeProbability() | Number of successes is less than 0. Returning 0 ...");
                return 0;
            }

            if (numberOfSuccesses > numberOfTrials)
            {
                Debug.WriteLine($"LowerCumulativeProbability() | Number of successes is greater than number of trials. Returning 0 ...");
                return 0;
            }

            if (probability <= 0)
            {
                Debug.WriteLine($"LowerCumulativeProbability() | Probability is less than or equal to 0. Returning 0 ...");
                return 0;
            }

            // In the case where probability is greater than or equal to 1, all discrete values up to the max possible successes are equal to 0,
            // and the max possible successes has a probability of 1.
            // Therefore, the sum of all values up to the max number of successes is 0.
            if (probability >= 1)
            {
                if (numberOfSuccesses == numberOfTrials)
                {
                    Debug.WriteLine($"LowerCumulativeProbability() | Probability is greater than or equal to 1, and the number of successes equals the number of trials. Returning 1 ...");
                    return 1;
                }

                Debug.WriteLine($"LowerCumulativeProbability() | Probability is greater than or equal to 1, and the number of successes does not equal the number of trials. Returning 1 ...");
                return 0;
            }

            double cumulativeProbability = 0;

            for (int i = 0; i <= numberOfSuccesses; i++)
            {
                cumulativeProbability += ProbabilityMassFunction(numberOfTrials, i, probability);
            }

            return cumulativeProbability;
        }

        /// <summary>
        /// Calculates the lower cumulative distribution P(X≤k) of trial data.
        /// Optionally calculates the lower cumulative distribution P(X≤k) of trial data, assuming that a group of a given number of trial successes is considered a single success
        /// in the context of the distribution.
        /// Example: If 'groupSuccessCount' = 2, then the total number of trials is divided by 2, and any combination of 2 successful trials is considered a single success
        /// in the context of the distribution.
        /// </summary>
        /// <param name="numberOfTrials"></param>
        /// <param name="probability"></param>
        /// <param name="groupSuccessCount">The number of grouped trial successes that count as a success. Default is 1 (one trial success equals one binomial success)</param>
        /// <returns>A cumulative distribution P(X≤k) of trial results and their respective probabilities.</returns>
        public static List<BinomialOutcome> LowerCumulativeDistribution(int numberOfTrials, double probability, int groupSuccessCount = 1)
        {
            // Validate parameters
            if (numberOfTrials < 1)
            {
                Debug.WriteLine($"LowerCumulativeDistribution() | Number of trials is less than 1.");
                return [new(0, 1)];
            }

            if (probability <= 0)
            {
                Debug.WriteLine($"LowerCumulativeDistribution() | Probability is less than or equal to 0.");

                // The probability of 0 successes should be 1, all other probabilities should be 0.
                // So the lower cumulative probability for all entries should be 1.
                var adjustedDistribution = new List<BinomialOutcome>
                {
                    new(0, 1)
                };

                for (int k = 1; k <= numberOfTrials; k++)
                {
                    adjustedDistribution.Add(new BinomialOutcome(k, 1));
                }

                return adjustedDistribution;
            }

            if (probability >= 1)
            {
                Debug.WriteLine($"LowerCumulativeDistribution() | Probability is greater than or equal to 1.");

                // All probabilities should be 0, except the probability of all successes should be 1.
                var adjustedDistribution = new List<BinomialOutcome>();

                for (int k = 0; k <= numberOfTrials - 1; k++)
                {
                    adjustedDistribution.Add(new BinomialOutcome(k, 0));
                }

                adjustedDistribution.Add(new BinomialOutcome(numberOfTrials, 1));

                return adjustedDistribution;
            }

            if (groupSuccessCount < 1)
            {
                Debug.WriteLine($"LowerCumulativeDistribution() | Group success count is less than 1.");
                return new List<BinomialOutcome> { new BinomialOutcome(0, 1) };
            }

            if (groupSuccessCount > numberOfTrials)
            {
                Debug.WriteLine($"LowerCumulativeDistribution() | Group success count is greater than the total number of trials.");
                return new List<BinomialOutcome> { new BinomialOutcome(0, 1) };
            }

            var distribution = new List<BinomialOutcome>();

            for (int k = 0; k <= Math.Floor((double)numberOfTrials / groupSuccessCount); k++)
            {
                distribution.Add(new BinomialOutcome
                {
                    Successes = k,
                    Probability = LowerCumulativeProbability(numberOfTrials, k * groupSuccessCount, probability)
                });
            }

            return distribution;
        }

        /// <summary>
        /// Calculates the upper cumulative probability of trial data.
        /// Upper cumulative probability is the probability of achieving greater than X successes such that P(X>k) or 1-P(X≤k).
        /// </summary>
        /// <param name="numberOfTrials"></param>
        /// <param name="numberOfSuccesses"></param>
        /// <param name="probability"></param>
        /// <returns>A double containing the cumulative probability value.</returns>
        public static double UpperCumulativeProbability(int numberOfTrials, int numberOfSuccesses, double probability)
        {
            // Validate parameters
            if (numberOfTrials < 1)
            {
                Debug.WriteLine($"UpperCumulativeProbability() | Number of trials is less than 1. Returning 0 ...");
                return 0;
            }

            if (numberOfSuccesses < 0)
            {
                Debug.WriteLine($"UpperCumulativeProbability() | Number of successes is less than 0. Returning 0 ...");
                return 0;
            }

            if (numberOfSuccesses > numberOfTrials)
            {
                Debug.WriteLine($"UpperCumulativeProbability() | Number of successes is greater than number of trials. Returning 0 ...");
                return 0;
            }

            if (probability <= 0)
            {
                Debug.WriteLine($"UpperCumulativeProbability() | Probability is less than or equal to 0. Returning 0 ...");
                return 0;
            }

            // In the case where probability is greater than or equal to 1, all discrete values up to the max possible successes are equal to 0,
            // and the max possible successes has a probability of 1.
            // Therefore, the sum of all values that include the max value should equal 1.
            // Additionally P(X > k) should be 0 when k = MaxValue because it is impossible to get more than the max value of successes.
            if (probability >= 1)
            {
                if (numberOfSuccesses == numberOfTrials)
                {
                    Debug.WriteLine($"UpperCumulativeProbability() | Probability is greater than or equal to 1, and the number of successes equals the number of trials. Returning 0 ...");
                    return 0;
                }

                Debug.WriteLine($"UpperCumulativeProbability() | Probability is greater than or equal to 1, and the number of successes does not equal the number of trials. Returning 1 ...");
                return 1;
            }

            return 1 - LowerCumulativeProbability(numberOfTrials, numberOfSuccesses, probability);
        }

        /// <summary>
        /// Calculates the upper cumulative distribution P(X>k) or 1-P(X≤k) of trial data.
        /// Optionally calculates the upper cumulative distribution P(X>k) or 1-P(X≤k) of trial data, assuming that a group of a given number of trial successes is considered a single 
        /// success in the context of the distribution.
        /// Example: If 'groupSuccessCount' = 2, then the total number of trials is divided by 2, and any combination of 2 successful trials is considered a single success
        /// in the context of the distribution.
        /// </summary>
        /// <param name="numberOfTrials"></param>
        /// <param name="probability"></param>
        /// <param name="groupSuccessCount">The number of grouped trial successes that count as a success. Default is 1 (one trial success equals one binomial success)</param>
        /// <returns>A cumulative distribution P(X>k) or 1-P(X≤k) of trial results and their respective probabilities.</returns>
        public static List<BinomialOutcome> UpperCumulativeDistribution(int numberOfTrials, double probability, int groupSuccessCount = 1)
        {
            // Validate parameters
            if (numberOfTrials < 1)
            {
                Debug.WriteLine($"UpperCumulativeDistribution() | Number of trials is less than 1.");
                return [new(0, 1)];
            }

            if (probability <= 0)
            {
                Debug.WriteLine($"UpperCumulativeDistribution() | Probability is less than or equal to 0.");

                // The probability of getting greater than 0 is 0%
                var adjustedDistribution = new List<BinomialOutcome>();

                for (int k = 0; k <= numberOfTrials; k++)
                {
                    adjustedDistribution.Add(new BinomialOutcome(k, 0));
                }

                return adjustedDistribution;
            }

            if (probability >= 1)
            {
                Debug.WriteLine($"UpperCumulativeDistribution() | Probability is greater than or equal to 1.");

                // All probabilities should be 0, except the probability of all successes should be 1.
                // So the upper cumulative distribution should all be ones, except for the last value because you can't get higher than the last value.
                var adjustedDistribution = new List<BinomialOutcome>();

                for (int k = 0; k <= numberOfTrials - 1; k++)
                {
                    adjustedDistribution.Add(new BinomialOutcome(k, 1));
                }

                adjustedDistribution.Add(new BinomialOutcome(numberOfTrials, 0));

                return adjustedDistribution;
            }

            if (groupSuccessCount < 1)
            {
                Debug.WriteLine($"UpperCumulativeDistribution() | Group success count is less than 1.");
                return new List<BinomialOutcome> { new BinomialOutcome(0, 1) };
            }

            if (groupSuccessCount > numberOfTrials)
            {
                Debug.WriteLine($"UpperCumulativeDistribution() | Group success count is greater than the total number of trials.");
                return new List<BinomialOutcome> { new BinomialOutcome(0, 1) };
            }

            var distribution = new List<BinomialOutcome>();

            for (int k = 0; k <= Math.Floor((double)numberOfTrials / groupSuccessCount); k++)
            {
                distribution.Add(new BinomialOutcome
                {
                    Successes = k,
                    Probability = UpperCumulativeProbability(numberOfTrials, k * groupSuccessCount, probability)
                });
            }

            return distribution;
        }

        /// <summary>
        /// Gets the survivor function probability for the specified number of successes k.
        /// That is, the cumulative probability of all probabilities P(X≥k) in the distribution.
        /// </summary>
        /// <param name="numberOfTrials">The total number of trials.</param>
        /// <param name="successes">The minimum number of successes.</param>
        /// <param name="probability">The probability of success in a single trial.</param>
        /// <returns></returns>
        public static double SurvivorFunction(int numberOfTrials, int successes, double probability)
        {
            // Validate inputs
            if (numberOfTrials < 0)
            {
                Debug.WriteLine($"SurvivorFunction() | Binomial is null. Returning 0 ...");
                return 0;
            }

            if (successes < 0)
            {
                Debug.WriteLine($"SurvivorFunction() | Successes is less than 0. Returning 0 ...");
                return 0;
            }

            if (probability < 0)
            {
                return 0;
            }

            if (probability >= 1)
            {
                return 1;
            }

            var binomial = new Binomial(probability, numberOfTrials);
            return 1 - binomial.CumulativeDistribution(successes - 1);
        }

        /// <summary>
        /// Gets the survivor function distribution P(X≥k) of trial data.
        /// Optionally calculates the upper cumulative distribution P(X≥k) of trial data, assuming that a group of a given number of trial successes is considered a single 
        /// success in the context of the distribution.
        /// Example: If 'groupSuccessCount' = 2, then the total number of trials is divided by 2, and any combination of 2 successful trials is considered a single success
        /// in the context of the distribution.
        /// </summary>
        /// <param name="numberOfTrials"></param>
        /// <param name="probability"></param>
        /// <param name="groupSuccessCount">The number of grouped trial successes that count as a success. Default is 1 (one trial success equals one binomial success)</param>
        /// <returns>A survivor function distribution P(X≥k) of trial results and their respective probabilities.</returns>
        public static List<BinomialOutcome> SurvivorDistribution(int numberOfTrials, double probability, int groupSuccessCount = 1)
        {
            // Validate parameters
            if (numberOfTrials < 1)
            {
                Debug.WriteLine($"SurvivorDistribution() | Number of trials is less than 1.");
                return new List<BinomialOutcome> { new BinomialOutcome(0, 1) };
            }

            if (probability <= 0)
            {
                Debug.WriteLine($"SurvivorDistribution() | Probability is less than or equal to 0.");

                // The probability of getting greater than 0 is 0%
                var adjustedDistribution = new List<BinomialOutcome>();

                // If including k=0, the probability should be 1.
                adjustedDistribution.Add(new BinomialOutcome(0, 1));

                // Probabilities for all other values of k should be 0.
                for (int k = 1; k <= numberOfTrials; k++)
                {
                    adjustedDistribution.Add(new BinomialOutcome(k, 0));
                }

                return adjustedDistribution;
            }

            if (probability >= 1)
            {
                Debug.WriteLine($"SurvivorDistribution() | Probability is greater than or equal to 1.");

                // All probabilities should be 0, except the probability of all successes should be 1.
                // So the upper cumulative distribution should all be ones.
                var adjustedDistribution = new List<BinomialOutcome>();

                for (int k = 0; k <= numberOfTrials; k++)
                {
                    adjustedDistribution.Add(new BinomialOutcome(k, 1));
                }

                return adjustedDistribution;
            }

            if (groupSuccessCount < 1)
            {
                Debug.WriteLine($"SurvivorDistribution() | Group success count is less than 1.");
                return new List<BinomialOutcome> { new BinomialOutcome(0, 1) };
            }

            if (groupSuccessCount > numberOfTrials)
            {
                Debug.WriteLine($"SurvivorDistribution() | Group success count is greater than the total number of trials.");
                return new List<BinomialOutcome> { new BinomialOutcome(0, 1) };
            }

            var distribution = new List<BinomialOutcome>();

            for (int k = 0; k <= Math.Floor((double)numberOfTrials / groupSuccessCount); k++)
            {
                distribution.Add(new BinomialOutcome
                {
                    Successes = k,
                    Probability = SurvivorFunction(numberOfTrials, k * groupSuccessCount, probability)
                });
            }

            return distribution;
        }

        /// <summary>
        /// Calculates the mean value of a probability distribution.
        /// </summary>
        /// <param name="numberOfTrials"></param>
        /// <param name="probability"></param>
        /// <returns></returns>
        public static double Mean(int numberOfTrials, double probability)
        {
            if (numberOfTrials < 1)
            {
                Debug.WriteLine($"GetMean() | Number of trials is less than 1. Returning 0 ...");
                return 0;
            }

            if (probability <= 0)
            {
                Debug.WriteLine($"GetMean() | Probability is less or equal to 0. Returning 0 ...");
                return 0;
            }

            return numberOfTrials * probability;
        }

        /// <summary>
        /// Calculates the standard deviation of a probability distribution.
        /// </summary>
        /// <param name="numberOfTrials"></param>
        /// <param name="probability"></param>
        /// <returns></returns>
        public static double StandardDeviation(int numberOfTrials, double probability)
        {
            if (numberOfTrials < 0)
            {
                Debug.WriteLine($"GetStandardDeviation() | Number of trials is less than 0. Returning 0 ...");
                return 0;
            }

            if (probability < 0)
            {
                Debug.WriteLine($"GetStandardDeviation() | Probability is less than 0. Returning 0 ...");
                return 0;
            }

            return Math.Sqrt(numberOfTrials * probability * (1 - probability));
        }

        #endregion
    }
}
