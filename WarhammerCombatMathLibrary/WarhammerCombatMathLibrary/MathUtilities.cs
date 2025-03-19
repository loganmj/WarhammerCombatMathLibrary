namespace WarhammerCombatMathLibrary
{
    /// <summary>
    /// A static helper class that provides basic math functions.
    /// </summary>
    public static class MathUtilities
    {
        #region Public Methods

        /// <summary>
        /// Calculates the factorial of a non-negative integer.
        /// Factorials are denoted by the syntax "n!".
        /// </summary>
        /// <param name="number">
        /// The integer value to perform the factorial calculation on. 
        /// The method will use the absolute value of the integer, in case the user passes in a negative value.
        /// </param>
        /// <returns>A double containing the factorial of the passed in value.</returns>
        public static double Factorial(int number)
        {
            double result = 1;

            for (int i = 1; i <= Math.Abs(number); i++)
            {
                result *= i;
            }

            return result;
        }

        #endregion
    }
}
