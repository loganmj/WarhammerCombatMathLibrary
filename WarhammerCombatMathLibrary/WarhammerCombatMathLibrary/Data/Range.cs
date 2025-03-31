namespace WarhammerCombatMathLibrary.Data
{
    /// <summary>
    /// A data object that describes the upper and lower bounds of a range of numberical values
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Range<T> where T : struct, IComparable<T>, IConvertible
    {
        #region Properties

        /// <summary>
        /// The lower bound of the range.
        /// </summary>
        public T LowerBound { get; set; }

        /// <summary>
        /// The upper bound of the range.
        /// </summary>
        public T UpperBound { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lowerBound"></param>
        /// <param name="upperBound"></param>
        /// <exception cref="ArgumentException"></exception>
        public Range(T lowerBound, T upperBound)
        {
            if (lowerBound.CompareTo(upperBound) > 0)
            {
                throw new ArgumentException("LowerBound must be less than or equal to UpperBound.");
            }

            LowerBound = lowerBound;
            UpperBound = upperBound;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Checks if a given value is within the range.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsWithinRange(T value)
        {
            return value.CompareTo(LowerBound) >= 0 && value.CompareTo(UpperBound) <= 0;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Range {{ LowerBound: {LowerBound}, UpperBound: {UpperBound} }}";
        }

        #endregion
    }
}
