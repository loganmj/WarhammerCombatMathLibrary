namespace WarhammerCombatMathLibrary.Data
{
    /// <summary>
    /// Gives the supported distribution types.
    /// </summary>
    public enum DistributionTypes
    {
        /// <summary>
        /// A binomial distribution is a simple, discrete distribution that gives a probability value P(X = k) for each value of k.
        /// </summary>
        Binomial,

        /// <summary>
        /// A lower cumulative distribution gives a probabiilty value P(X <= k) for each value of k.
        /// </summary>
        LowerCumulative,

        /// <summary>
        /// An upper cumulative distribution gives a probability value P(X > k) for each value of k.
        /// </summary>
        UpperCumulative,

        /// <summary>
        /// A survivor distribution gives a probability value P(X >= k) for each value of k, essentially calculating the probability of 
        /// getting at least k successes for each value of k.
        /// </summary>
        Suvivor
    }
}
