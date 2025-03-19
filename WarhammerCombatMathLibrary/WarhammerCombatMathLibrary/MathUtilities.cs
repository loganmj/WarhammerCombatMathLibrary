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
        public static int Factorial(int number)
        {
            // Throw error if given a negative number
            if (number < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(number), $"Factorial function is not defined for negative numbers.");
            }

            // Return 1 if given 0
            if (number == 0)
            {
                return 1;
            }

            // Multiply together all integers from 1 to the passed in number.
            int result = 1;

            for (int i = 1; i <= Math.Abs(number); i++)
            {
                result *= i;
            }

            return result;
        }

        #endregion
    }
}
