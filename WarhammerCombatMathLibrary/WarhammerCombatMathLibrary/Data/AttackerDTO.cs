namespace WarhammerCombatMathLibrary.Data
{
    /// <summary>
    /// A data transfer object representing the attacker in a combat scenario.
    /// </summary>
    public class AttackerDTO : IEquatable<AttackerDTO>
    {
        #region Properties

        /// <summary>
        /// The name of the attacking model.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The number of models in the attacker's unit.
        /// </summary>
        public int NumberOfModels { get; set; }

        /// <summary>
        /// The number of attacks dice that the weapon gets as part of its attacks stat.
        /// </summary>
        public int WeaponNumberOfAttackDice { get; set; }

        /// <summary>
        /// The attack dice type used to determine the variable number of attacks.
        /// </summary>
        public DiceType WeaponAttackDiceType { get; set; }

        /// <summary>
        /// The number of flat attacks that the attacker gets.
        /// </summary>
        public int WeaponFlatAttacks { get; set; }

        /// <summary>
        /// The ballistic/weapon skill threshold value of the attacker.
        /// </summary>
        public int WeaponSkill { get; set; }

        /// <summary>
        /// The strength of the attacker's weapon.
        /// </summary>
        public int WeaponStrength { get; set; }

        /// <summary>
        /// The armor pierce value of the attacker's weapon.
        /// </summary>
        public int WeaponArmorPierce { get; set; }

        /// <summary>
        /// The number of damage dice that the weapon gets as part of its damage stat.
        /// </summary>
        public int WeaponNumberOfDamageDice { get; set; }

        /// <summary>
        /// The damage dice type used to determine the variable amount of damage.
        /// </summary>
        public DiceType WeaponDamageDiceType { get; set; }

        /// <summary>
        /// The amount of flat damage the weapon deals.
        /// </summary>
        public int WeaponFlatDamage { get; set; }

        /// <summary>
        /// Attacker has Torrent keyword
        /// </summary>
        public bool WeaponHasTorrent { get; set; }

        /// <summary>
        /// Attacker has Lethal Hits keyword
        /// </summary>
        public bool WeaponHasLethalHits { get; set; }

        /// <summary>
        /// Attacker has Sustained Hits keyword
        /// </summary>
        public bool WeaponHasSustainedHits { get; set; }

        /// <summary>
        /// The amount of additional hits the weapon gets when Sustained Hits is triggers.
        /// </summary>
        public int WeaponSustainedHitsValue { get; set; }

        // TODO: Weapon has full reroll hits
        // TODO: Weapon has reroll hits of 1

        /// <summary>
        /// Attacker has DevastatingWounds keyword
        /// </summary>
        public bool WeaponHasDevastatingWounds { get; set; }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Attacker: [ Name: '{Name}', "
                   + $"NumberOfModels: {NumberOfModels}, "
                   + $"Weapon Attacks: {(WeaponNumberOfAttackDice > 0 ? $"{WeaponNumberOfAttackDice} {WeaponAttackDiceType} + {WeaponFlatAttacks}" : WeaponFlatAttacks.ToString())}, "
                   + $"WeaponSkill: {WeaponSkill}, "
                   + $"WeaponStrength: {WeaponStrength}, "
                   + $"WeaponArmorPierce: -{WeaponArmorPierce}, "
                   + $"WeaponDamage: {(WeaponNumberOfDamageDice > 0 ? $"{WeaponNumberOfDamageDice} {WeaponDamageDiceType} + {WeaponFlatDamage}" : WeaponFlatDamage)}, "
                   + $"WeaponHasTorrent: {WeaponHasTorrent}, "
                   + $"WeaponHasLethalHits: {WeaponHasLethalHits}, "
                   + $"WeaponHasDevastatingWounds: {WeaponHasDevastatingWounds} ]";
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return Equals(obj as AttackerDTO);
        }

        /// <summary>
        /// Checks if the given AttackerDTO object is equal to this one.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>A boolean value. True if the objects are the same, False otherwise.</returns>
        public bool Equals(AttackerDTO? other)
        {
            if (other == null) return false;

            return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase)
                   && NumberOfModels == other.NumberOfModels
                   && WeaponNumberOfAttackDice == other.WeaponNumberOfAttackDice
                   && WeaponAttackDiceType == other.WeaponAttackDiceType
                   && WeaponFlatAttacks == other.WeaponFlatAttacks
                   && WeaponSkill == other.WeaponSkill
                   && WeaponStrength == other.WeaponStrength
                   && WeaponArmorPierce == other.WeaponArmorPierce
                   && WeaponNumberOfDamageDice == other.WeaponNumberOfDamageDice
                   && WeaponDamageDiceType == other.WeaponDamageDiceType
                   && WeaponFlatDamage == other.WeaponFlatDamage
                   && WeaponHasTorrent == other.WeaponHasTorrent
                   && WeaponHasLethalHits == other.WeaponHasLethalHits
                   && WeaponHasDevastatingWounds == other.WeaponHasDevastatingWounds;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            // Combine the weapon keyword abilities into a single byte
            var weaponKeywordFlags = ((WeaponHasTorrent ? 1 : 0) << 0)
                                     | ((WeaponHasLethalHits ? 1 : 0) << 1)
                                     | ((WeaponHasDevastatingWounds ? 1 : 0) << 2);

            return HashCode.Combine(Name?.ToLowerInvariant(),
                                    NumberOfModels,
                                    (WeaponNumberOfAttackDice * (int)WeaponAttackDiceType) + WeaponFlatAttacks,
                                    WeaponSkill,
                                    WeaponStrength,
                                    WeaponArmorPierce,
                                    (WeaponNumberOfDamageDice * (int)WeaponDamageDiceType) + WeaponFlatDamage,
                                    weaponKeywordFlags);
        }

        #endregion
    }
}
