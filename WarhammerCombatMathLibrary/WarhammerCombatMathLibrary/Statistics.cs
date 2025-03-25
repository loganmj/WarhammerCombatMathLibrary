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
            return (double)numberOfSuccessfulResults / numberOfPossibleResults;
        }

        /// <summary>
        /// Calculates the binomial coefficient, determining the number of combinations of k elements
        /// in a population of n, independent of order.
        /// </summary>
        /// <param name="totalPopulation">The population of the group or set.</param>
        /// <param name="combinationSize">The number elements in a unique combination.</param>
        /// <returns>A double value containing the binomial coefficient.</returns>
        public static double BinomialCoefficient(int totalPopulation, int combinationSize)
        {
            if (combinationSize > totalPopulation || totalPopulation < 0 || combinationSize < 0)
            {
                throw new ArgumentException("Invalid input: combinationSize must be <= totalPopulation, and both must be non-negative.");
            }

            return MathUtilities.Factorial(totalPopulation) / (MathUtilities.Factorial(combinationSize) * MathUtilities.Factorial(totalPopulation - combinationSize));
        }

        /// <summary>
        /// Calculates the probability for the success of a given number of trials
        /// using a specified probability of success for a single trial.
        /// </summary>
        /// <param name="probability">Probability of success for a single trial.</param>
        /// <param name="numberOfTrials">Number of trials.</param>
        /// <returns>A double value containing the probability that all trials will be successful.</returns>
        public static double ProbabilityOfMultipleSuccesses(double probability, int numberOfTrials)
        {
            return Math.Pow(probability, numberOfTrials);
        }

        /// <summary>
        /// Calculates the probability for the success of a given number of trials
        /// using an array of probabilities that represent the probability of each successful trial.
        /// </summary>
        /// <param name="probability">Probability of success for a single trial.</param>
        /// <param name="numberOfTrials">Number of trials.</param>
        /// <returns>A double value containing the probability that all trials will be successful.</returns>
        public static double ProbabilityOfMultipleSuccesses(double[] probabilities)
        {
            double result = 1;

            foreach (var probability in probabilities)
            {
                result *= Math.Pow(probability, probabilities.Length);
            }

            return result;
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
            return BinomialCoefficient(numberOfTrials, numberOfSuccesses)
                   * ProbabilityOfMultipleSuccesses(probability, numberOfSuccesses)
                   * ProbabilityOfMultipleSuccesses(1 - probability, numberOfTrials - numberOfSuccesses);
        }

        /*

        /// <summary>
        /// Calculates the binomial distribution of trial data.
        /// </summary>
        /// <param name="numberOfTrials">The number of trials in the process.</param>
        /// <param name="probability">The probability of success for a single trial.</param>
        /// <returns>A binomial distribution of trial results and their respective probabilities.</returns>
        public static ProbabilityDistribution BinomialDistribution(int numberOfTrials, double probability)
        {
            var distribution = new ProbabilityDistribution();

            for (int k = 0; k <= numberOfTrials; k++)
            {
                distribution.Add(k, ProbabilityMassFunction(numberOfTrials, k, probability));
            }

            return distribution;
        }

        */

        /// <summary>
        /// Calculates the binomial distribution of trial data.
        /// </summary>
        /// <param name="numberOfTrials">The number of trials in the process.</param>
        /// <param name="probability">The probability of success for a single trial.</param>
        /// <returns>A binomial distribution of trial results and their respective probabilities.</returns>
        public static List<BinomialData> BinomialDistribution(int numberOfTrials, double probability)
        {
            var distribution = new List<BinomialData>();

            for (int k = 0; k <= numberOfTrials; k++)
            {
                distribution.Add(new BinomialData { Successes = k, Probability = ProbabilityMassFunction(numberOfTrials, k, probability) });
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

        /*

        /// <summary>
        /// Calculates the lower cumulative distribution of trial data.
        /// </summary>
        /// <param name="numberOfTrials"></param>
        /// <param name="probability"></param>
        /// <returns>A cumulative distribution of trial results and their respective probabilities.</returns>
        public static ProbabilityDistribution LowerCumulativeDistribution(int numberOfTrials, double probability)
        {
            var distribution = new ProbabilityDistribution();

            for (int k = 0; k <= numberOfTrials; k++)
            {
                distribution.Add(k, LowerCumulativeProbability(numberOfTrials, k, probability));
            }

            return distribution;
        }

        */

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

        /*

        /// <summary>
        /// Calculates the upper cumulative distribution of trial data.
        /// </summary>
        /// <param name="numberOfTrials"></param>
        /// <param name="probability"></param>
        /// <returns></returns>
        public static ProbabilityDistribution UpperCumulativeDistribution(int numberOfTrials, double probability)
        {
            var distribution = new ProbabilityDistribution();

            for (int k = 0; k <= numberOfTrials; k++)
            {
                distribution.Add(k, UpperCumulativeProbability(numberOfTrials, k, probability));
            }

            return distribution;
        }

        */

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
            return Math.Sqrt(numberOfTrials * probability * (1 - probability));
        }

        #endregion
    }
}
