namespace WarhammerCombatMathLibrary.Data
{
    /// <summary>
    /// Data object for binomial data.
    /// </summary>
    public class BinomialOutcome
    {
        #region Properties

        /// <summary>
        /// The number of successful trials.
        /// </summary>
        public int Successes { get; set; }

        /// <summary>
        /// The probability of getting the number of successful trials.
        /// </summary>
        public double Probability { get; set; }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"P({Successes}) = {Probability:F4}";
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is BinomialOutcome other)
            {
                return Successes == other.Successes && Math.Round(Probability, 4) == Math.Round(other.Probability, 4);
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(Successes, Probability);
        }

        #endregion
    }
}
