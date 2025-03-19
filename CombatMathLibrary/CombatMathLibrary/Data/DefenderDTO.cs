namespace CombatCalculator.Data
{
    /// <summary>
    /// A data transfer object representing the defender in a combat scenario.
    /// </summary>
    public class DefenderDTO
    {
        #region Properties

        /// <summary>
        /// The toughness stat of the defender.
        /// </summary>
        public int Toughness { get; set; }

        /// <summary>
        /// The armor save stat of the defender.
        /// </summary>
        public int ArmorSave { get; set; }

        /// <summary>
        /// The invulnerable save stat of the defender.
        /// </summary>
        public int InvulnerableSave { get; set; }

        /// <summary>
        /// The feel no pain stat of the defender.
        /// </summary>
        public int FeelNoPain { get; set; }

        /// <summary>
        /// The number of wounds the defender has.
        /// </summary>
        public int Wounds { get; set; }

        /// <summary>
        /// The number of models in the defender's unit.
        /// </summary>
        public int NumberOfModels { get; set; }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Defender:\n"
                   + $"NumberOfModels: {NumberOfModels}\n"
                   + $"Toughness: {Toughness}\n"
                   + $"ArmorSave: {ArmorSave}+\n"
                   + $"InvulnerableSave: {InvulnerableSave}+\n"
                   + $"FeelNoPain: {FeelNoPain}+\n"
                   + $"Wounds: {Wounds}";
        }

        #endregion
    }
}
