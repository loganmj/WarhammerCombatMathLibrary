namespace CombatCalculator.Data
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
        /// The number of attacks the attacker is making.
        /// </summary>
        public int WeaponAttacks { get; set; }

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
            return $"Attacker:\n"
                   + $"NumberOfModels: {NumberOfModels}\n"
                   + $"WeaponAttacks: {WeaponAttacks}\n"
                   + $"WeaponSkill: {WeaponSkill}+\n"
                   + $"WeaponStrength: {WeaponStrength}\n"
                   + $"WeaponArmorPierce: -{WeaponArmorPierce}\n"
                   + $"WeaponDamage: {WeaponDamage}";
        }

        #endregion
    }
}
