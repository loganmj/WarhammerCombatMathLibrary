namespace WarhammerCombatMathLibrary.Data
{
    /// <summary>
    /// Data object for binomial data.
    /// </summary>
    public class BinomialData
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
            return $"P({Successes}) = {Probability * 100:F2}%";
        }

        #endregion
    }
}
