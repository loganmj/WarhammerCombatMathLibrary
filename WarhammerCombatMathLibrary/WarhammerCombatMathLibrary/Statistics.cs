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

            if (numberOfSuccessfulResults <= 0)
            {
                Debug.WriteLine($"ProbabilityOfSuccess() | Number of successful results is less than or equal to 0. Returning 0 ...");
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
        /// </summary>
        /// <param name="numberOfTrials">The number of trials in the process.</param>
        /// <param name="probability">The probability of success for a single trial.</param>
        /// <returns>A binomial distribution of trial results and their respective probabilities.</returns>
        public static List<BinomialData> BinomialDistribution(int numberOfTrials, double probability)
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
                var adjustedDistribution = new List<BinomialData>
                {
                    new(0, 1)
                };

                for (int k = 1; k <= numberOfTrials; k++)
                {
                    adjustedDistribution.Add(new BinomialData(k, 0));
                }

                return adjustedDistribution;
            }

            if (probability >= 1)
            {
                Debug.WriteLine($"BinomialDistribution() | Probability is greater than or equal to 1.");

                // All probabilities should be 0, except the probability of all successes should be 1.
                var adjustedDistribution = new List<BinomialData>();

                for (int k = 0; k <= numberOfTrials-1; k++)
                {
                    adjustedDistribution.Add(new BinomialData(k, 0));
                }

                adjustedDistribution.Add(new BinomialData(numberOfTrials, 1));

                return adjustedDistribution;
            }

            // Create distribution
            var distribution = new List<BinomialData>();

            for (int k = 0; k <= numberOfTrials; k++)
            {
                distribution.Add(new BinomialData(k, ProbabilityMassFunction(numberOfTrials, k, probability)));
            }

            return distribution;
        }

        /// <summary>
        /// Calculates the lower cumulative probability of trial data.
        /// Lower cumulative probability is the probability of achieving a result less than or equal to the given number of successes.
        /// </summary>
        /// <param name="numberOfTrials"></param>
        /// <param name="numberOfSuccesses"></param>
        /// <param name="probability"></param>
        /// <returns>A double containing the cumulative probability value.</returns>
        public static double LowerCumulativeProbability(int numberOfTrials, int numberOfSuccesses, double probability)
        {
            double cumulativeProbability = 0;

            for (int i = 0; i <= numberOfSuccesses; i++)
            {
                cumulativeProbability += ProbabilityMassFunction(numberOfTrials, i, probability);
            }

            return cumulativeProbability;
        }

        /// <summary>
        /// Calculates the lower cumulative distribution of trial data.
        /// </summary>
        /// <param name="numberOfTrials"></param>
        /// <param name="probability"></param>
        /// <returns>A cumulative distribution of trial results and their respective probabilities.</returns>
        public static List<BinomialData> LowerCumulativeDistribution(int numberOfTrials, double probability)
        {
            var distribution = new List<BinomialData>();

            for (int k = 0; k <= numberOfTrials; k++)
            {
                distribution.Add(new BinomialData { Successes = k, Probability = LowerCumulativeProbability(numberOfTrials, k, probability) });
            }

            return distribution;
        }

        /// <summary>
        /// Calculates the upper cumulative probability of trial data.
        /// Upper cumulative probability is the probability of achieving a result greater than or equal to the given number of successes.
        /// </summary>
        /// <param name="numberOfTrials"></param>
        /// <param name="numberOfSuccesses"></param>
        /// <param name="probability"></param>
        /// <returns>A double containing the cumulative probability value.</returns>
        public static double UpperCumulativeProbability(int numberOfTrials, int numberOfSuccesses, double probability)
        {
            double cumulativeProbability = 0;

            for (int i = numberOfSuccesses; i <= numberOfTrials; i++)
            {
                cumulativeProbability += ProbabilityMassFunction(numberOfTrials, i, probability);
            }

            return cumulativeProbability;
        }

        /// <summary>
        /// Calculates the lower cumulative distribution of trial data.
        /// </summary>
        /// <param name="numberOfTrials"></param>
        /// <param name="probability"></param>
        /// <returns>A cumulative distribution of trial results and their respective probabilities.</returns>
        public static List<BinomialData> UpperCumulativeDistribution(int numberOfTrials, double probability)
        {
            var distribution = new List<BinomialData>();

            for (int k = 0; k <= numberOfTrials; k++)
            {
                distribution.Add(new BinomialData { Successes = k, Probability = UpperCumulativeProbability(numberOfTrials, k, probability) });
            }

            return distribution;
        }

        /// <summary>
        /// Calculates the mean value of a probability distribution.
        /// </summary>
        /// <param name="numberOfTrials"></param>
        /// <param name="probability"></param>
        /// <returns></returns>
        public static double GetMean(int numberOfTrials, double probability)
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
        /// Calculates the mode value of a probability distribution.
        /// </summary>
        /// <param name="numberOfTrials"></param>
        /// <param name="probability"></param>
        /// <returns></returns>
        public static int GetMode(int numberOfTrials, double probability)
        {
            return (int)Math.Round((numberOfTrials + 1) * probability);
        }

        /// <summary>
        /// Calculates the standard deviation of a probability distribution.
        /// </summary>
        /// <param name="numberOfTrials"></param>
        /// <param name="probability"></param>
        /// <returns></returns>
        public static double GetStandardDeviation(int numberOfTrials, double probability)
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
