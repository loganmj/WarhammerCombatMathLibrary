using System.Text;

namespace CombatCalculator.Data
{
    /// <summary>
    /// Represents a discrete probability distribution of Bernoulli trial data, represented as a dictionary data map.
    /// The key is the number of successes.
    /// The value is the probability of that result.
    /// </summary>
    public class ProbabilityDistribution : Dictionary<int, double>
    {
        #region Public Methods

        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder stringBuilder = new();

            foreach (var result in this)
            {
                stringBuilder.AppendLine($"P({result.Key}) = {result.Value * 100:F2}%");
            }

            return stringBuilder.ToString();
        }

        #endregion
    }
}
