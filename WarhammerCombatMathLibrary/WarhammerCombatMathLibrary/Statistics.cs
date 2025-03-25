using System.Diagnostics;
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
            Console.WriteLine($"ProbabilityOfSuccess - numberOfPossibleResults: {numberOfPossibleResults}, numberOfSuccessfulResults: {numberOfSuccessfulResults}");
            ArgumentOutOfRangeException.ThrowIfLessThan(numberOfPossibleResults, 1);
            ArgumentOutOfRangeException.ThrowIfNegative(numberOfSuccessfulResults);

            if (numberOfSuccessfulResults > numberOfPossibleResults)
            {
                throw new ArgumentException("Number of successful results must be less than or equal to the number of possible results.");
            }

            // Perform calculation
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
            // Validate parameters
            Console.WriteLine($"BinomialCoefficient - TotalPopulation: {totalPopulation}, CombinationSize: {combinationSize}");
            ArgumentOutOfRangeException.ThrowIfNegative(totalPopulation);
            ArgumentOutOfRangeException.ThrowIfNegative(combinationSize);

            if (combinationSize > totalPopulation)
            {
                throw new ArgumentException("Combination size must be less than or equal to the total population.");
            }

            // Perform calculation
            var factorialTotal = MathUtilities.Factorial(totalPopulation);
            var factorialCombination = MathUtilities.Factorial(combinationSize);
            var factorialDifference = MathUtilities.Factorial(totalPopulation - combinationSize);

            Debug.WriteLine($"FactorialTotal: {factorialTotal}, FactorialCombination: {factorialCombination}, FactorialDifference: {factorialDifference}");

            return (double)(factorialTotal / (factorialCombination * factorialDifference));
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
            // Validate parameters
            Console.WriteLine($"ProbabilityOfMultipleSuccesses - Probability: {probability}, NumberOfTrials: {numberOfTrials}");
            ArgumentOutOfRangeException.ThrowIfNegative(probability);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(probability, 1);
            ArgumentOutOfRangeException.ThrowIfLessThan(numberOfTrials, 1);

            // Perform calculation
            return Math.Pow(probability, numberOfTrials);
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
            Console.WriteLine($"ProbabilityMassFunction - NumberOfTrials: {numberOfTrials}, NumberOfSuccesses: {numberOfSuccesses}, Probability: {probability}");
            ArgumentOutOfRangeException.ThrowIfLessThan(numberOfTrials, 1);
            ArgumentOutOfRangeException.ThrowIfNegative(numberOfSuccesses);
            ArgumentOutOfRangeException.ThrowIfNegative(probability);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(probability, 1);

            // Perform calculation
            double binomialCoefficient = BinomialCoefficient(numberOfTrials, numberOfSuccesses);
            double successProbability = ProbabilityOfMultipleSuccesses(probability, numberOfSuccesses);
            double failureProbability = ProbabilityOfMultipleSuccesses(1 - probability, numberOfTrials - numberOfSuccesses);

            Console.WriteLine($"BinomialCoefficient: {binomialCoefficient}, SuccessProbability: {successProbability}, FailureProbability: {failureProbability}");

            return binomialCoefficient * successProbability * failureProbability;
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
            Console.WriteLine($"BinomialDistribution - numberOfTrials: {numberOfTrials}, probability: {probability}");
            ArgumentOutOfRangeException.ThrowIfNegative(numberOfTrials);
            ArgumentOutOfRangeException.ThrowIfNegative(probability);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(probability, 1);

            // Create distribution
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
