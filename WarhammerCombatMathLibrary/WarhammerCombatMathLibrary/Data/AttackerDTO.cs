namespace WarhammerCombatMathLibrary.Data
{
    /// <summary>
    /// A data transfer object representing the attacker in a combat scenario.
    /// </summary>
    public class AttackerDTO
    {
        #region Properties

        /// <summary>
        /// The number of models in the attacker's unit.
        /// </summary>
        public int NumberOfModels { get; set; }

        /// <summary>
        /// The scalar number of variable attack rolls that the attacker gets.
        /// </summary>
        public int WeaponScalarOfVariableAttacks { get; set; }

        /// <summary>
        /// The dice type used to determine the variable number of attacks.
        /// </summary>
        public DiceType WeaponVariableAttackType { get; set; }

        /// <summary>
        /// The additional number of flat attacks the attacker gets.
        /// </summary>
        public int WeaponFlatAttacks { get; set; }

        /// <summary>
        /// Denotes whether the weapon has the Torrent keyword.
        /// </summary>
        public bool WeaponHasTorrent { get; set; }

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
        /// The scalar number of variable damage rolls that the attacker gets.
        /// </summary>
        public int WeaponScalarOfVariableDamage { get; set; }

        /// <summary>
        /// The dice type used to determine the variable amount of damage.
        /// </summary>
        public DiceType WeaponVariableDamageType { get; set; }

        /// <summary>
        /// The addition amount of flat damage the weapon has.
        /// </summary>
        public int WeaponFlatDamage { get; set; }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Attacker: [NumberOfModels: {NumberOfModels}, "
                   + $"Weapon Attacks: {(WeaponScalarOfVariableAttacks > 0 ? $"{WeaponScalarOfVariableAttacks} {WeaponVariableAttackType} + {WeaponFlatAttacks}" : WeaponFlatAttacks.ToString())}, "
                   + $"WeaponSkill: {WeaponSkill}, "
                   + $"WeaponStrength: {WeaponStrength}, "
                   + $"WeaponArmorPierce: -{WeaponArmorPierce}, "
                   + $"WeaponDamage: {(WeaponScalarOfVariableDamage > 0 ? $"{WeaponScalarOfVariableDamage} {WeaponVariableDamageType} + {WeaponFlatDamage}" : WeaponFlatDamage)}]";
        }

        #endregion
    }
}
