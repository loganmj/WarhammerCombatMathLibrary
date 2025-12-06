namespace WarhammerCombatMathLibrary.Data
{
    /// <summary>
    /// A data transfer object representing the attacker in a combat scenario.
    /// </summary>
    public class AttackerDTO : IEquatable<AttackerDTO>
    {
        #region Properties

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
        /// The amount of additional hits the weapon gets when Sustained Hits is triggered.
        /// </summary>
        public int WeaponSustainedHitsMultiplier { get; set; }

        /// <summary>
        /// Attacker has DevastatingWounds keyword
        /// </summary>
        public bool WeaponHasDevastatingWounds { get; set; }

        /// <summary>
        /// Attacker may reroll hit rolls
        /// </summary>
        public bool WeaponHasRerollHitRolls { get; set; }

        /// <summary>
        /// Attacker may reroll hit rolls of 1
        /// </summary>
        public bool WeaponHasRerollHitRollsOf1 { get; set; }

        /// <summary>
        /// Attacker may reroll hit rolls
        /// </summary>
        public bool WeaponHasRerollWoundRolls { get; set; }

        /// <summary>
        /// Attacker may reroll hit rolls of 1
        /// </summary>
        public bool WeaponHasRerollWoundRollsOf1 { get; set; }

        /// <summary>
        /// Attacker may reroll hit rolls
        /// </summary>
        public bool WeaponHasRerollDamageRolls { get; set; }

        /// <summary>
        /// Attacker may reroll hit rolls of 1
        /// </summary>
        public bool WeaponHasRerollDamageRollsOf1 { get; set; }

        /// <summary>
        /// The hit roll result threshold that is considered a critical hit
        /// </summary>
        public int CriticalHitThreshold { get; set; }

        /// <summary>
        /// The wound roll result threshold that is considered a critical wound
        /// </summary>
        public int CriticalWoundThreshold { get; set; }

        /// <summary>
        /// Attacker has Anti [keyword] X+ ability against the target
        /// </summary>
        public bool WeaponHasAnti { get; set; }

        /// <summary>
        /// The wound roll result threshold (X+) at which Anti triggers critical wounds
        /// </summary>
        public int WeaponAntiThreshold { get; set; }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Attacker: [ NumberOfModels: {NumberOfModels}, "
                   + $"Weapon Attacks: {(WeaponNumberOfAttackDice > 0 ? $"{WeaponNumberOfAttackDice} {WeaponAttackDiceType} + {WeaponFlatAttacks}" : WeaponFlatAttacks.ToString())}, "
                   + $"WeaponSkill: {WeaponSkill}, "
                   + $"WeaponStrength: {WeaponStrength}, "
                   + $"WeaponArmorPierce: -{WeaponArmorPierce}, "
                   + $"WeaponDamage: {(WeaponNumberOfDamageDice > 0 ? $"{WeaponNumberOfDamageDice} {WeaponDamageDiceType} + {WeaponFlatDamage}" : WeaponFlatDamage)}, "
                   + $"WeaponHasTorrent: {WeaponHasTorrent}, "
                   + $"WeaponHasLethalHits: {WeaponHasLethalHits}, "
                   + $"WeaponHasSustainedHits: {WeaponHasSustainedHits}, "
                   + $"WeaponSustainedHitsMultiplier: {WeaponSustainedHitsMultiplier}, "
                   + $"WeaponHasRerollHitRolls: {WeaponHasRerollHitRolls}, "
                   + $"WeaponHasRerollHitRollsOf1: {WeaponHasRerollHitRollsOf1}, "
                   + $"WeaponHasDevastatingWounds: {WeaponHasDevastatingWounds}, "
                   + $"WeaponHasRerollWoundRolls: {WeaponHasRerollWoundRolls}, "
                   + $"WeaponHasRerollWoundRollsOf1: {WeaponHasRerollWoundRollsOf1}, "
                   + $"WeaponHasRerollDamageRolls: {WeaponHasRerollDamageRolls}, "
                   + $"WeaponHasRerollDamageRollsOf1: {WeaponHasRerollDamageRollsOf1}, "
                   + $"CriticalHitThreshold: {CriticalHitThreshold}, "
                   + $"CriticalWoundThreshold: {CriticalWoundThreshold}, "
                   + $"WeaponHasAnti: {WeaponHasAnti}, "
                   + $"WeaponAntiThreshold: {WeaponAntiThreshold} ]";
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

            return NumberOfModels == other.NumberOfModels
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
                   && WeaponHasSustainedHits == other.WeaponHasSustainedHits
                   && WeaponSustainedHitsMultiplier == other.WeaponSustainedHitsMultiplier
                   && WeaponHasRerollHitRolls == other.WeaponHasRerollHitRolls
                   && WeaponHasRerollHitRollsOf1 == other.WeaponHasRerollHitRollsOf1
                   && WeaponHasDevastatingWounds == other.WeaponHasDevastatingWounds
                   && WeaponHasRerollWoundRolls == other.WeaponHasRerollWoundRolls
                   && WeaponHasRerollWoundRollsOf1 == other.WeaponHasRerollWoundRollsOf1
                   && WeaponHasRerollDamageRolls == other.WeaponHasRerollDamageRolls
                   && WeaponHasRerollDamageRollsOf1 == other.WeaponHasRerollDamageRollsOf1
                   && CriticalHitThreshold == other.CriticalHitThreshold
                   && CriticalWoundThreshold == other.CriticalWoundThreshold
                   && WeaponHasAnti == other.WeaponHasAnti
                   && WeaponAntiThreshold == other.WeaponAntiThreshold;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            // Combine the weapon keyword abilities into a single byte
            var weaponKeywordFlags = ((WeaponHasTorrent ? 1 : 0) << 0)
                                     | ((WeaponHasLethalHits ? 1 : 0) << 1)
                                     | ((WeaponHasSustainedHits ? 1 : 0) << 2)
                                     | ((WeaponHasDevastatingWounds ? 1 : 0) << 3)
                                     | ((WeaponHasAnti ? 1 : 0) << 4);

            // Combine reroll abilities into a single byte
            var rerollFlags = ((WeaponHasRerollHitRolls ? 1 : 0) << 0)
                              | ((WeaponHasRerollHitRollsOf1 ? 1 : 0) << 1)
                              | ((WeaponHasRerollWoundRolls ? 1 : 0) << 2)
                              | ((WeaponHasRerollWoundRollsOf1 ? 1 : 0) << 3)
                              | ((WeaponHasRerollDamageRolls ? 1 : 0) << 4)
                              | ((WeaponHasRerollDamageRollsOf1 ? 1 : 0) << 5);

            return HashCode.Combine(NumberOfModels,
                                    (WeaponNumberOfAttackDice * (int)WeaponAttackDiceType) + WeaponFlatAttacks,
                                    WeaponSkill + WeaponStrength + WeaponArmorPierce,
                                    (WeaponNumberOfDamageDice * (int)WeaponDamageDiceType) + WeaponFlatDamage,
                                    weaponKeywordFlags,
                                    rerollFlags,
                                    WeaponSustainedHitsMultiplier,
                                    (CriticalHitThreshold << 8) | (CriticalWoundThreshold << 4) | WeaponAntiThreshold);
        }

        #endregion
    }
}
