using System.Numerics;

namespace WarhammerCombatMathLibrary
{
    /// <summary>
    /// A static helper class that provides basic math functions.
    /// </summary>
    public static class MathUtilities
    {
        #region Public Methods

        /// <summary>
        /// Calculates the factorial of a positive integer.
        /// Factorials are denoted by the syntax "n!".
        /// </summary>
        /// <param name="number">
        /// The integer value to perform the factorial calculation on. 
        /// The method will use the absolute value of the integer, in case the user passes in a negative value.
        /// </param>
        /// <returns>An int containing the factorial of the passed in value.</returns>
        public static BigInteger Factorial(int number)
        {
            Console.WriteLine($"Factorial - number: {number}");

            // Throw error if given a negative number
            ArgumentOutOfRangeException.ThrowIfNegative(number);

            // Return 1 if given 0
            if (number == 0)
            {
                Console.WriteLine($"Factorial - Passed in 0, returning 1");
                return 1;
            }

            // Multiply together all integers from 1 to the passed in number.
            BigInteger result = 1;

            for (int i = 1; i <= number; i++)
            {
                Console.WriteLine($"Factorial iteration: {i}");
                Console.WriteLine($"Multiplying {result} by {i} ...");
                result *= i;
                Console.WriteLine($"New result: {result}");
            }

            Console.WriteLine($"Factorial - input: {number}, result: {result}");
            return result;
        }

        #endregion
    }
}
