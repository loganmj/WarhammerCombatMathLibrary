namespace WarhammerCombatMathLibrary.Data
{
    /// <summary>
    /// A data transfer object representing the defender in a combat scenario.
    /// </summary>
    public class DefenderDTO : IEquatable<DefenderDTO>
    {
        #region Properties

        /// <summary>
        /// The name of the defending unit.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The number of models in the defender's unit.
        /// </summary>
        public int NumberOfModels { get; set; }

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
        /// The damage reduction value of the defender. This reduces the damage of each attack by the specified amount (minimum 0).
        /// </summary>
        public int DamageReduction { get; set; }

        /// <summary>
        /// The number of wounds the defender has.
        /// </summary>
        public int Wounds { get; set; }

        /// <summary>
        /// Hit modifier applied to hit rolls against this defender. Positive values make it easier for attackers to hit, negative values make it harder.
        /// Combined with attacker's hit modifier, the total is capped at +/- 1.
        /// </summary>
        public int HitModifier { get; set; }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Defender: [NumberOfModels: {NumberOfModels}, " +
            $"Toughness: {Toughness}, " +
            $"ArmorSave: {ArmorSave}, " +
            $"InvulnerableSave: {InvulnerableSave}, " +
            $"FeelNoPain: {FeelNoPain}, " +
            $"DamageReduction: {DamageReduction}, " +
            $"Wounds: {Wounds}, " +
            $"HitModifier: {HitModifier}]";
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return Equals(obj as DefenderDTO);
        }

        /// <summary>
        /// Checks if the given DefenderDTO object is equal to this one.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>A boolean value. True if the objects are the same, False otherwise.</returns>
        public bool Equals(DefenderDTO? other)
        {
            return other != null
                   && string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase)
                   && NumberOfModels == other.NumberOfModels
                   && Toughness == other.Toughness
                   && ArmorSave == other.ArmorSave
                   && InvulnerableSave == other.InvulnerableSave
                   && FeelNoPain == other.FeelNoPain
                   && DamageReduction == other.DamageReduction
                   && Wounds == other.Wounds
                   && HitModifier == other.HitModifier;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(Name,
                                    NumberOfModels,
                                    Toughness,
                                    ArmorSave,
                                    InvulnerableSave,
                                    FeelNoPain,
                                    DamageReduction,
                                    HashCode.Combine(Wounds, HitModifier));
        }


        #endregion
    }
}
