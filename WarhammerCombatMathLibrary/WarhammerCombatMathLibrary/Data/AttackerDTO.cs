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
        /// The scalar number of variable attacks that the attacker gets.
        /// </summary>
        public int WeaponScalarOfVariableAttacks { get; set; }

        /// <summary>
        /// The dice type used to determine the variable number of attacks.
        /// </summary>
        public DiceType WeaponVariableAttackType { get; set; }

        /// <summary>
        /// The number of flat attacks the attacker gets.
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
        /// The damage value of the attacker's weapon.
        /// </summary>
        public int WeaponDamage { get; set; }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Attacker: [NumberOfModels: {NumberOfModels}, "
                   + $"WeaponVariableAttacks: {WeaponScalarOfVariableAttacks}, "
                   + $"WeaponVariableAttacksType: {WeaponVariableAttackType}, "
                   + $"WeaponFlatAttacks: {WeaponFlatAttacks},"
                   + $"WeaponSkill: {WeaponSkill},"
                   + $"WeaponStrength: {WeaponStrength},"
                   + $"WeaponArmorPierce: -{WeaponArmorPierce},"
                   + $"WeaponDamage: {WeaponDamage}]";
        }

        #endregion
    }
}
