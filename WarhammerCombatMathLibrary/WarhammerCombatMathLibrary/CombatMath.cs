using System.Diagnostics;
using WarhammerCombatMathLibrary.Data;

namespace WarhammerCombatMathLibrary
{
    /// <summary>
    /// Represents a binomial distribution of trials and successes.
    /// </summary>
    public static class CombatMath
    {
        #region Constants

        /// <summary>
        /// The number of possible results of rolling a six-sided die.
        /// </summary>
        private const int POSSIBLE_RESULTS_SIX_SIDED_DIE = 6;

        /// <summary>
        /// A result of 1 on a die is always considered a failure.
        /// </summary>
        private const int AUTOMATIC_FAIL_RESULT = 1;

        /// <summary>
        /// The maximum result of a die is always considered a success.
        /// </summary>
        private const int AUTOMATIC_SUCCESS_RESULT = 6;

        #endregion

        #region Private Methods

        /// <summary>
        /// Calculates the combined hit modifier from the attacker and defender, capped at +/- 1.
        /// </summary>
        /// <param name="attacker">The attacker data object</param>
        /// <param name="defender">The defender data object</param>
        /// <returns>An integer value containing the combined hit modifier, capped at +/- 1</returns>
        private static int GetCombinedHitModifier(AttackerDTO? attacker, DefenderDTO? defender)
        {
            int attackerModifier = attacker?.HitModifier ?? 0;
            int defenderModifier = defender?.HitModifier ?? 0;
            int combinedModifier = attackerModifier + defenderModifier;

            // Cap the combined modifier at +/- 1
            return Math.Clamp(combinedModifier, -1, 1);
        }

        /// <summary>
        /// Calculates the combined wound modifier from the attacker and defender, capped at +/- 1.
        /// </summary>
        /// <param name="attacker">The attacker data object</param>
        /// <param name="defender">The defender data object</param>
        /// <returns>An integer value containing the combined wound modifier, capped at +/- 1</returns>
        private static int GetCombinedWoundModifier(AttackerDTO? attacker, DefenderDTO? defender)
        {
            int attackerModifier = attacker?.WoundModifier ?? 0;
            int defenderModifier = defender?.WoundModifier ?? 0;
            int combinedModifier = attackerModifier + defenderModifier;

            // Cap the combined modifier at +/- 1
            return Math.Clamp(combinedModifier, -1, 1);
        }

        /// <summary>
        /// Returns the success threshold for succeeding on a given die roll, with a given success threshold.
        /// Note that in Warhammer 40k, a roll of 1 always fails, and a roll of 6 always succeeds.
        /// </summary>
        /// <param name="successThreshold">The success threshold value.</param>
        /// <returns>An integer value containing the number of possible successful results.</returns>
        private static int GetNumberOfSuccessfulResults(int successThreshold)
        {
            // If the success threshold is greater than the number of possible results, then there are no successful results
            if (successThreshold > POSSIBLE_RESULTS_SIX_SIDED_DIE)
            {
                Debug.WriteLine($"GetNumberOfSuccessfulResults() | Success threshold is greater than {POSSIBLE_RESULTS_SIX_SIDED_DIE}, returning 0 ...");
                return 0;
            }

            // If the success threshold is less than or equal to 0, then there are no successful results
            if (successThreshold <= 0)
            {
                Debug.WriteLine($"GetNumberOfSuccessfulResults() | Success threshold is less than or equal to 0, returning 0 ...");
                return 0;
            }

            // Calculate the number of possible successful results, capping out at the number of possible results minus one (to account for the guaranteed fail result)
            return Math.Min(POSSIBLE_RESULTS_SIX_SIDED_DIE - (successThreshold - 1), POSSIBLE_RESULTS_SIX_SIDED_DIE - 1);
        }

        /// <summary>
        /// Returns the average number of attack rolls the attacking unit is making.
        /// This takes into account:
        /// - The average of any variable attacks added to the flat number of attacks.
        /// - The number of models in the attacking unit.
        /// </summary>
        /// <param name="attacker">The attacker data object</param>
        /// <returns>An integer value containing the average number of attacks made by the attacking unit.</returns>
        private static int GetAverageAttacks(AttackerDTO attacker)
        {
            // If either the number of models or the weapon attacks is less than 1, return 0.
            if (attacker.NumberOfModels < 1)
            {
                Debug.WriteLine($"GetAverageAttacks() | Number of models is less than 1, returning 0 ...");
                return 0;
            }

            // If both the variable scalar and the flat value of attacks are less than or equal to 0, then there are no attacks.
            if (attacker.WeaponNumberOfAttackDice <= 0 && attacker.WeaponFlatAttacks <= 0)
            {
                Debug.WriteLine($"GetAverageAttacks() | Attacker has no attacks value, returning 0 ...");
                return 0;
            }

            var averageAttackDieResult = Statistics.GetMeanResult((int)attacker.WeaponAttackDiceType);
            var averageVariableAttacksPerModel = attacker.WeaponNumberOfAttackDice * averageAttackDieResult;
            var totalAverageAttacksPerModel = averageVariableAttacksPerModel + attacker.WeaponFlatAttacks;
            var totalAverageAttacks = totalAverageAttacksPerModel * attacker.NumberOfModels;
            return totalAverageAttacks;
        }

        /// <summary>
        /// Gets the minimum number of attacks made by the attacking unit.
        /// </summary>
        /// <param name="attacker">The attacker data object</param>
        /// <returns>An integer value containing the minimum number attacks made by the attacking unit.</returns>
        private static int GetMinimumAttacks(AttackerDTO attacker)
        {
            // If either the number of models or the weapon attacks is less than 1, return 0.
            if (attacker.NumberOfModels < 1)
            {
                Debug.WriteLine($"GetMinimumAttacks() | Number of models is less than 1, returning 0 ...");
                return 0;
            }

            // If both the variable salar and the flat value of attacks are less than or equal to 0, then there are no attacks.
            if (attacker.WeaponNumberOfAttackDice <= 0 && attacker.WeaponFlatAttacks <= 0)
            {
                Debug.WriteLine($"GetMinimumAttacks() | Attacker has no attacks value, returning 0 ...");
                return 0;
            }

            var minimumAttacksPerModel = attacker.WeaponNumberOfAttackDice + attacker.WeaponFlatAttacks;
            var totalMinimumAttacks = minimumAttacksPerModel * attacker.NumberOfModels;
            return totalMinimumAttacks;
        }

        /// <summary>
        /// Gets the maximum number of attacks made by the attacking unit.
        /// </summary>
        /// <param name="attacker">The attacker data object</param>
        /// <returns>An integer value containing the maximum number attacks made by the attacking unit.</returns>
        private static int GetMaximumAttacks(AttackerDTO attacker)
        {
            // If either the number of models or the weapon attacks is less than 1, return 0.
            if (attacker.NumberOfModels < 1)
            {
                Debug.WriteLine($"GetMaximumAttacks() | Number of models is less than 1, returning 0 ...");
                return 0;
            }

            // If both the variable salar and the flat value of attacks are less than or equal to 0, then there are no attacks.
            if (attacker.WeaponNumberOfAttackDice <= 0 && attacker.WeaponFlatAttacks <= 0)
            {
                Debug.WriteLine($"GetMaximumAttacks() | Attacker has no attacks value, returning 0 ...");
                return 0;
            }

            var maximumAttackRollValue = (int)attacker.WeaponAttackDiceType;
            var maximumVariableAttacksPerModel = (attacker.WeaponNumberOfAttackDice * maximumAttackRollValue);
            var totalMaximumAttacksPerModel = maximumVariableAttacksPerModel + attacker.WeaponFlatAttacks;
            var totalMaximumAttacks = totalMaximumAttacksPerModel * attacker.NumberOfModels;
            return totalMaximumAttacks;
        }

        /// <summary>
        /// Gets the base probability of hit roll success of the attacker.
        /// This takes into account the attacker's weapon skill and any hit modifiers.
        /// </summary>
        /// <param name="attacker">The attacker data object</param>
        /// <param name="hitModifier">The combined hit modifier to apply to the hit threshold</param>
        /// <returns>A double containing the base probability of a successful hit roll</returns>
        private static double GetBaseProbabilityOfHit(AttackerDTO attacker, int hitModifier)
        {
            // Validate inputs
            if (attacker.WeaponSkill <= 0)
            {
                Debug.WriteLine($"GetBaseProbabilityOfHit() | Attacker weapon skill is less than or equal to 0, returning 0 ...");
                return 0;
            }

            // Apply hit modifier to the weapon skill threshold
            // Positive modifiers make it easier to hit (lower threshold), negative modifiers make it harder (higher threshold)
            var adjustedWeaponSkill = attacker.WeaponSkill - hitModifier;

            // Account for the fact that the smallest possible result on the die is considered an automatic failure,
            // and should not count as part of the success threshold
            var hitSuccessThreshold = adjustedWeaponSkill == AUTOMATIC_FAIL_RESULT ? AUTOMATIC_FAIL_RESULT + 1 : adjustedWeaponSkill;
            return Statistics.GetProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, GetNumberOfSuccessfulResults(hitSuccessThreshold));
        }

        /// <summary>
        /// Gets the probability of a critical hit.
        /// This is based on the critical hit threshold of the attacker.
        /// </summary>
        /// <param name="attacker">The attacker data object</param>
        /// <returns>A double containing the probability of a critical hit roll</returns>
        private static double GetProbabilityOfCriticalHit(AttackerDTO attacker)
        {
            // If critical hit threshold is out of bounds, return base result
            if (attacker.CriticalHitThreshold <= 1 || attacker.CriticalHitThreshold >= 7)
            {
                return Statistics.GetProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, 1);
            }

            // Account for the fact that the smallest possible result on the die is considered an automatic failure,
            // and should not count as part of the success threshold
            var adjustedCriticalHitThreshold = attacker.CriticalHitThreshold == AUTOMATIC_FAIL_RESULT ? AUTOMATIC_FAIL_RESULT + 1 : attacker.CriticalHitThreshold;
            return Statistics.GetProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, GetNumberOfSuccessfulResults(adjustedCriticalHitThreshold));
        }

        /// <summary>
        /// Get the hit probability modifier of the Reroll Hits ability.
        /// </summary>
        /// <param name="probabilityOfHit"></param>
        /// <returns>A double value containing the probability modifier</returns>
        private static double GetHitModifier_RerollHits(double probabilityOfHit)
        {
            return (1 - probabilityOfHit) * probabilityOfHit;
        }

        /// <summary>
        /// Get the hit probability modifier of the Reroll Hits of 1 ability.
        /// </summary>
        /// <param name="probabilityOfHit"></param>
        /// <returns>A double value containing the probability modifier</returns>
        private static double GetHitModifier_RerollHitsOf1(double probabilityOfHit)
        {
            return (1.0 / POSSIBLE_RESULTS_SIX_SIDED_DIE) * probabilityOfHit;
        }

        /// <summary>
        /// Gets the base probability of succeeding on a wound roll, based on the attacker and defender stats.
        /// If the attacker has Anti X+, wound rolls of X+ automatically succeed as critical wounds.
        /// Wound modifiers are applied to the normal wound threshold, but not to the Anti threshold (which is based on unmodified rolls).
        /// The final threshold used is whichever gives the higher probability of success (lower threshold number).
        /// Also applies wound modifiers from both attacker and defender, capped at +/- 1.
        /// </summary>
        /// <param name="attacker">The attacker data object</param>
        /// <param name="defender">The defender data object</param>
        /// <returns></returns>
        private static double GetBaseProbabilityOfWound(AttackerDTO attacker, DefenderDTO defender)
        {
            // Get the combined wound modifier
            var combinedWoundModifier = GetCombinedWoundModifier(attacker, defender);
            
            // Get base wound threshold from strength vs toughness
            var normalWoundThreshold = GetSuccessThresholdOfWound(attacker.WeaponStrength, defender.Toughness);
            
            // Apply wound modifier to the normal wound threshold
            // Positive modifiers make it easier to wound (lower threshold), negative modifiers make it harder (higher threshold)
            var adjustedNormalThreshold = normalWoundThreshold - combinedWoundModifier;
            
            // Determine the final wound threshold to use
            int finalWoundThreshold;
            
            // If the attacker has Anti X+ and it's valid, compare it with the adjusted normal threshold
            // and use whichever gives the better (lower) threshold
            if (attacker.WeaponHasAnti && IsValidThreshold(attacker.WeaponAntiThreshold))
            {
                // Use the better (lower) threshold between adjusted normal and Anti
                // Note: Anti threshold is NOT modified by wound modifiers (it's based on unmodified die rolls)
                finalWoundThreshold = Math.Min(adjustedNormalThreshold, attacker.WeaponAntiThreshold);
            }
            else
            {
                // No Anti, just use the adjusted normal threshold
                finalWoundThreshold = adjustedNormalThreshold;
            }
            
            // Account for the fact that the smallest possible result on the die is considered an automatic failure,
            // and should not count as part of the success threshold
            var validatedThreshold = finalWoundThreshold == AUTOMATIC_FAIL_RESULT ? AUTOMATIC_FAIL_RESULT + 1 : finalWoundThreshold;
            
            return Statistics.GetProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, GetNumberOfSuccessfulResults(validatedThreshold));
        }

        /// <summary>
        /// Checks if a threshold value is valid (between 2 and 6 inclusive).
        /// </summary>
        /// <param name="threshold">The threshold value to validate</param>
        /// <returns>True if the threshold is valid, false otherwise</returns>
        private static bool IsValidThreshold(int threshold)
        {
            return threshold >= 2 && threshold <= 6;
        }

        /// <summary>
        /// Gets the probability of a critical wound.
        /// This is based on the critical wound threshold of the attacker.
        /// If the attacker has Anti X+, wound rolls of X+ also count as critical wounds.
        /// </summary>
        /// <param name="attacker">The attacker data object</param>
        /// <returns>A double containing the probability of a critical wound roll</returns>
        private static double GetProbabilityOfCriticalWound(AttackerDTO attacker)
        {
            // Determine the effective critical wound threshold
            bool hasValidAnti = attacker.WeaponHasAnti && IsValidThreshold(attacker.WeaponAntiThreshold);
            bool hasValidCriticalWound = IsValidThreshold(attacker.CriticalWoundThreshold);
            
            int effectiveCriticalWoundThreshold;
            if (hasValidAnti && hasValidCriticalWound)
            {
                // Both are valid: use the lower threshold (which means higher probability)
                effectiveCriticalWoundThreshold = Math.Min(attacker.CriticalWoundThreshold, attacker.WeaponAntiThreshold);
            }
            else if (hasValidAnti)
            {
                effectiveCriticalWoundThreshold = attacker.WeaponAntiThreshold;
            }
            else if (hasValidCriticalWound)
            {
                effectiveCriticalWoundThreshold = attacker.CriticalWoundThreshold;
            }
            else
            {
                // Default to 6+ if nothing is configured
                effectiveCriticalWoundThreshold = 6;
            }

            // Account for the fact that the smallest possible result on the die is considered an automatic failure,
            // and should not count as part of the success threshold
            var adjustedCriticalWoundThreshold = effectiveCriticalWoundThreshold == AUTOMATIC_FAIL_RESULT ? AUTOMATIC_FAIL_RESULT + 1 : effectiveCriticalWoundThreshold;
            return Statistics.GetProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, GetNumberOfSuccessfulResults(adjustedCriticalWoundThreshold));
        }

        /// <summary>
        /// Get the wound probability modifier of the Lethal Hits ability.
        /// </summary>
        /// <param name="probabilityOfCriticalHit"></param>
        /// <returns>A double value containing the probability modifier</returns>
        private static double GetWoundModifier_LethalHits(double probabilityOfCriticalHit)
        {
            // Lethal hits trigger on a critical hit roll, and auto-succeed the wound roll
            return probabilityOfCriticalHit;
        }

        /// <summary>
        /// Get the wound probability modifier of the Sustained Hits ability.
        /// </summary>
        /// <param name="probabilityOfCriticalHit"></param>
        /// <returns>A double value containing the probability modifier</returns>
        private static double GetWoundModifier_SustainedHits(double probabilityOfCriticalHit, int sustainedHitsMultiplier, double probabilityOfWound)
        {
            return sustainedHitsMultiplier * probabilityOfCriticalHit * probabilityOfWound;
        }

        /// <summary>
        /// Get the wound probability modifier of the Reroll Wounds ability.
        /// </summary>
        /// <param name="probabilityOfHit"></param>
        /// <param name="probabilityOfWound"></param>
        /// <returns>A double value containing the probability modifier</returns>
        private static double GetWoundModifier_RerollWounds(double probabilityOfHit, double probabilityOfWound)
        {
            return probabilityOfHit * (1 - probabilityOfWound) * probabilityOfWound;
        }

        /// <summary>
        /// Get the wound probability modifier of the Reroll Wounds of 1 ability.
        /// </summary>
        /// <param name="probabilityOfHit"></param>
        /// <param name="probabilityOfWound"></param>
        /// <returns>A double value containing the probability modifier</returns>
        private static double GetWoundModifier_RerollWoundsOf1(double probabilityOfHit, double probabilityOfWound)
        {
            return probabilityOfHit * (1.0 / POSSIBLE_RESULTS_SIX_SIDED_DIE) * probabilityOfWound;
        }

        /// <summary>
        /// Gets the base probability of the defender failing a save roll.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        private static double GetBaseProbabilityOfFailedSave(AttackerDTO attacker, DefenderDTO defender)
        {
            var adjustedArmorSaveThreshold = GetAdjustedArmorSaveThreshold(attacker, defender);
            var probabilityOfSuccessfulSave = Statistics.GetProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, GetNumberOfSuccessfulResults(adjustedArmorSaveThreshold));
            return (double)(1 - probabilityOfSuccessfulSave);
        }

        /// <summary>
        /// Get the failed save probability modifier of the Devastating Wounds ability.
        /// </summary>
        /// <param name="probabilityOfHit"></param>
        /// <param name="probabilityOfCriticalWound"></param>
        /// <returns>A double value containing the probability modifier</returns>
        private static double GetFailedSaveModifier_DevastatingWounds(double probabilityOfHit, double probabilityOfCriticalWound)
        {
            return probabilityOfHit * probabilityOfCriticalWound;
        }

        /// <summary>
        /// Returns the average amount of damage that the attacker's weapon is able to deal.
        /// This takes into account: 
        /// - The average of any variable attacks added to the flat number of attacks.
        /// </summary>
        /// <param name="attacker">The attacker data object</param>
        /// <returns>An integer value containing the average amount of damage that the attacker is able to do.</returns>
        private static int GetAverageDamagePerAttack(AttackerDTO? attacker)
        {
            // Validate inputs
            if (attacker == null)
            {
                Debug.WriteLine($"GetAverageDamagePerAttack() | Attacker is null, returning 0 ...");
                return 0;
            }

            // If both the variable scalar and the flat number of attacks are less than or equal to 0, then there are no attacks.
            if (attacker.WeaponNumberOfAttackDice <= 0 && attacker.WeaponFlatAttacks <= 0)
            {
                Debug.WriteLine($"GetAverageDamagePerAttack() | Attacker has no attacks value, returning 0 ...");
                return 0;
            }

            // If both the variable scalar and the flat value of damage are less than or equal to 0, then there is no damage.
            if (attacker.WeaponNumberOfDamageDice <= 0 && attacker.WeaponFlatDamage <= 0)
            {
                Debug.WriteLine($"GetAverageDamagePerAttack() | Attacker has no damage value, returning 0 ...");
                return 0;
            }

            var numberOfDamageDieRolls = attacker.WeaponNumberOfDamageDice;
            var averageDamagePerDieRoll = Statistics.GetMeanResult((int)attacker.WeaponDamageDiceType);
            var flatDamage = attacker.WeaponFlatDamage;
            var averageDamagePerAttack = (numberOfDamageDieRolls * averageDamagePerDieRoll) + flatDamage;
            return averageDamagePerAttack;
        }

        /// <summary>
        /// Gets the minimum amount of damage the attacking unit is able to do for each attack.
        /// </summary>
        /// <param name="attacker">The attacker data object</param>
        /// <returns>An integer value containing the minimum amount of damage that the attacker is able to do for each attack.</returns>
        private static int GetMinimumDamagePerAttack(AttackerDTO? attacker)
        {
            // Validate inputs
            if (attacker == null)
            {
                Debug.WriteLine($"GetMinimumDamagePerAttack() | Attacker is null, returning 0 ...");
                return 0;
            }

            // If both the variable scalar and the flat number of attacks are less than or equal to 0, then there are no attacks.
            if (attacker.WeaponNumberOfAttackDice <= 0 && attacker.WeaponFlatAttacks <= 0)
            {
                Debug.WriteLine($"GetMinimumDamagePerAttack() | Attacker has no attacks value, returning 0 ...");
                return 0;
            }

            // If both the variable scalar and the flat value of damage are less than or equal to 0, then there is no damage.
            if (attacker.WeaponNumberOfDamageDice <= 0 && attacker.WeaponFlatDamage <= 0)
            {
                Debug.WriteLine($"GetMinimumDamagePerAttack() | Attacker has no damage value, returning 0 ...");
                return 0;
            }

            var numberOfDamageDieRolls = attacker.WeaponNumberOfDamageDice;
            var minimumDamagePerDieRoll = 1;
            var flatDamage = attacker.WeaponFlatDamage;
            var minimumDamagePerAttack = (numberOfDamageDieRolls * minimumDamagePerDieRoll) + flatDamage;
            return minimumDamagePerAttack;
        }

        /// <summary>
        /// Gets the maximum amount of damage the attacking unit is able to do for each attack
        /// </summary>
        /// <param name="attacker">The attacker data object</param>
        /// <returns>An integer value containing the maximum amount of damage that the attacker is able to do for each attack</returns>
        private static int GetMaximumDamagePerAttack(AttackerDTO? attacker)
        {
            // Validate inputs
            if (attacker == null)
            {
                Debug.WriteLine($"GetMaximumDamagePerAttack() | Attacker is null, returning 0 ...");
                return 0;
            }

            // If both the variable scalar and the flat number of attacks are less than or equal to 0, then there are no attacks.
            if (attacker.WeaponNumberOfAttackDice <= 0 && attacker.WeaponFlatAttacks <= 0)
            {
                Debug.WriteLine($"GetMaximumDamagePerAttack() | Attacker has no attacks value, returning 0 ...");
                return 0;
            }

            // If both the variable scalar and the flat value of damage are less than or equal to 0, then there is no damage.
            if (attacker.WeaponNumberOfDamageDice <= 0 && attacker.WeaponFlatDamage <= 0)
            {
                Debug.WriteLine($"GetMaximumDamagePerAttack() | Attacker has no damage value, returning 0 ...");
                return 0;
            }

            var numberOfDamageDieRolls = attacker.WeaponNumberOfDamageDice;
            var maximumDamagePerDieRoll = (int)attacker.WeaponDamageDiceType;
            var flatDamage = attacker.WeaponFlatDamage;
            var maximumDamagePerAttack = (numberOfDamageDieRolls * maximumDamagePerDieRoll) + flatDamage;
            return maximumDamagePerAttack;
        }

        /// <summary>
        /// Determines the average amount of damage per attack that the attacker can do, after applying defensive modifiers
        /// </summary>
        /// <param name="damagePerAttack">The amount of damage done per attack.</param>
        /// <param name="defender">The defender object containing defensive attributes such as Feel No Pain.</param>
        /// <returns>A double value containing the average amount of damage per attack after applying defensive modifiers</returns>
        private static double GetAverageAdjustedDamagePerAttack(int damagePerAttack, DefenderDTO? defender)
        {
            // Validate inputs
            if (damagePerAttack <= 0)
            {
                Debug.WriteLine($"GetAverageAdjustedDamagePerAttack() | Input damage is less than or equal to 0. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetAverageAdjustedDamagePerAttack() | Defender is null. Returning 0 ...");
                return 0;
            }

            // Apply damage reduction (minimum 0 damage)
            var damageAfterReduction = Math.Max(0, damagePerAttack - defender.DamageReduction);

            // Account for feel no pains
            var feelNoPainSuccessProbability = Statistics.GetProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, GetNumberOfSuccessfulResults(defender.FeelNoPain));
            var averageDamageAfterFeelNoPain = damageAfterReduction * (1 - feelNoPainSuccessProbability);

            return averageDamageAfterFeelNoPain;
        }

        /// <summary>
        /// Determines the minimum amount of damage per attack that the attacker can do, after applying defensive modifiers
        /// </summary>
        /// <param name="damagePerAttack">The amount of damage done per attack.</param>
        /// <param name="defender">The defender object containing defensive attributes such as Feel No Pain.</param>
        /// <returns>An integer value containing the minimum amount of damage per attack after applying defensive modifiers</returns>
        private static int GetMinimumAdjustedDamagePerAttack(int damagePerAttack, DefenderDTO? defender)
        {
            // Validate inputs
            if (damagePerAttack <= 0)
            {
                Debug.WriteLine($"GetMinimumAdjustedDamagePerAttack() | Input damage is less than or equal to 0. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetMinimumAdjustedDamagePerAttack() | Defender is null. Returning 0 ...");
                return 0;
            }

            // Apply damage reduction (minimum 0 damage)
            var damageAfterReduction = Math.Max(0, damagePerAttack - defender.DamageReduction);

            // If the defender has feel no pains, then the minimum amount of damage is 0, since it is possible for the defender to succeed in all of their feel no pain rolls.
            if (defender.FeelNoPain >= 2 && defender.FeelNoPain <= 6)
            {
                Debug.WriteLine($"GetMinimumAdjustedDamagePerAttack() | Defender has feel no pains. Returning 0 ...");
                return 0;
            }

            return damageAfterReduction;
        }

        /// <summary>
        /// Determines the maximum amount of damage per attack that the attacker can do, after applying defensive modifiers
        /// </summary>
        /// <param name="damagePerAttack">The amount of damage done per attack.</param>
        /// <param name="defender">The defender object containing defensive attributes such as Feel No Pain.</param>
        /// <returns>An integer value containing the maximum amount of damage per attack after applying defensive modifiers</returns>
        private static int GetMaximumAdjustedDamagePerAttack(int damagePerAttack, DefenderDTO? defender)
        {
            // Validate inputs
            if (damagePerAttack <= 0)
            {
                Debug.WriteLine($"GetMaximumAdjustedDamagePerAttack() | Input damage is less than or equal to 0. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetMaximumAdjustedDamagePerAttack() | Defender is null. Returning 0 ...");
                return 0;
            }

            // Apply damage reduction (minimum 0 damage)
            // Feel no pains are ignored, as the maximum amount of damage is done when defender fails all of their feel no pain rolls
            return Math.Max(0, damagePerAttack - defender.DamageReduction);
        }

        /// <summary>
        /// Determines the maximum possible number of models destroyed given a specified amount of applied damage.
        /// </summary>
        /// <param name="numberOfAttacks">The number of successful attacks</param>
        /// <param name="damagePerAttack">The damage per attack from the attacker's weapon.</param>
        /// <param name="defender">The defending unit data.</param>
        /// <returns>A double value representing the number of defending models that will be destroyed by the attack.</returns>
        private static double GetModelsDestroyed(double numberOfAttacks, double damagePerAttack, DefenderDTO? defender)
        {
            // Validate inputs
            if (numberOfAttacks <= 0)
            {
                Debug.WriteLine($"GetModelsDestroyed() | Number of attacks is less than or equal to 0. Returning 0 ...");
                return 0;
            }

            if (damagePerAttack <= 0)
            {
                Debug.WriteLine($"GetModelsDestroyed() | Attacker's weapon damage is less than or equal to 0. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetModelsDestroyed() | Defender is null. Returning 0 ...");
                return 0;
            }

            // Determine the total possible amount of damage
            double totalDamage = damagePerAttack * numberOfAttacks;

            // Determine the divisor based on which value is larger: the defender's wounds per model, or the attacker's weapon damage.
            var damageThreshold = Math.Max(defender.Wounds, damagePerAttack);

            // Calculate the maximum possible number of models destroyed
            var modelsDestroyed = totalDamage / damageThreshold;

            // Return either the max possible models destroyed, or the total number of defending models, whichever comes first
            return Math.Min(modelsDestroyed, defender.NumberOfModels);
        }

        /// <summary>
        /// Determines the number of successful, unblocked attacks required to destroy a single model from the defending unit.
        /// </summary>
        /// <param name="damagePerAttack">The amount of damage being dealt per attack</param>
        /// <param name="defender">The defender data object.</param>
        /// <returns>An integer value representing the minimum number of successful attacks required to destroy a single model in the defending unit.</returns>
        private static int GetAttacksRequiredToDestroyOneModel(int damagePerAttack, DefenderDTO? defender)
        {
            if (defender == null)
            {
                Debug.WriteLine($"GetAttacksRequiredToDestroyOneModel() | Defender is null. Returning 0 ...");
                return 0;
            }

            if (damagePerAttack <= 0)
            {
                Debug.WriteLine($"GetAttacksRequiredToDestroyOneModel() | Attacker weapon damage is less than or equal to 0. Returning 0 ...");
                return 0;
            }

            // If the result is a decimal, round up to the next highest int value (as it will require another full attack to destroy the model).
            return (int)Math.Ceiling((double)defender.Wounds / damagePerAttack);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the attacker's probability of succeeding any single hit roll.
        /// Includes modifiers for any abilities that affect hit rolls.
        /// </summary>
        /// <param name="attacker">The attacker data object</param>
        /// <param name="defender">The defender data object (optional, used for hit modifiers)</param>
        /// <returns>A double value containing the probability of succeeding on any single hit roll.</returns>
        public static double GetProbabilityOfHit(AttackerDTO? attacker, DefenderDTO? defender = null)
        {
            // Validate inputs
            if (attacker == null)
            {
                Debug.WriteLine($"GetProbabilityOfHit() | Attacker is null, returning 0 ...");
                return 0;
            }

            // If weapon has torrent, no other modifiers are added
            if (attacker.WeaponHasTorrent)
            {
                return 1;
            }

            // Calculate combined hit modifier
            var combinedHitModifier = GetCombinedHitModifier(attacker, defender);

            // Calculate base hit probability with hit modifier
            var baseHitProbability = GetBaseProbabilityOfHit(attacker, combinedHitModifier);

            // Calculate modifiers
            double totalHitModifiers = 0;

            // Full rerolls overrides rerolls of 1
            if (attacker.WeaponHasRerollHitRolls)
            {
                totalHitModifiers += GetHitModifier_RerollHits(baseHitProbability);
            }
            else if (attacker.WeaponHasRerollHitRollsOf1)
            {
                totalHitModifiers += GetHitModifier_RerollHitsOf1(baseHitProbability);
            }

            return (double)(baseHitProbability + totalHitModifiers);
        }

        /// <summary>
        /// Returns the mean of the attacker's hit roll distribution.
        /// </summary>
        /// <param name="attacker">The attacker data object</param>
        /// <returns>A double value containing the average number of successful hit rolls</returns>
        public static double GetMeanHits(AttackerDTO? attacker)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetMeanHits() | Attacker is null, returning 0 ...");
                return 0;
            }

            var averageNumberOfAttacks = GetAverageAttacks(attacker);
            var probabilityOfHit = GetProbabilityOfHit(attacker);
            return Statistics.GetMeanOfDistribution(averageNumberOfAttacks, probabilityOfHit);
        }

        /// <summary>
        /// Gets the discrete expected number of successful hit rolls, based on the average probability.
        /// </summary>
        /// <param name="attacker">The attacker data object</param>
        /// <returns></returns>
        public static int GetExpectedHits(AttackerDTO? attacker)
        {
            return (int)Math.Floor(GetMeanHits(attacker));
        }

        /// <summary>
        /// Returns the standard deviation of the attacker's hit roll distribution.
        /// TODO: We may want to add a special case for Torrent attacks. Not sure at this point.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns>A double value containing the standard deviation of successful hits</returns>
        public static double GetStandardDeviationHits(AttackerDTO? attacker)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetStandardDeviationHits() | Attacker is null. Returning 0 ...");
                return 0;
            }

            var averageAttacks = GetAverageAttacks(attacker);
            var varianceAttacks = Statistics.GetVarianceOfResults(attacker.WeaponNumberOfAttackDice, (int)attacker.WeaponAttackDiceType);
            var probabilityOfHit = GetProbabilityOfHit(attacker);

            // If the attacker has Torrent, all attacks will hit.
            // Any variance comes from the number of attacks.
            if (attacker.WeaponHasTorrent)
            {
                return Math.Sqrt(varianceAttacks);
            }

            return Statistics.GetCombinedStandardDeviationOfDistribution(averageAttacks, varianceAttacks, probabilityOfHit);
        }

        /// <summary>
        /// Returns a distribution of hit roll results.
        /// </summary>
        /// <param name="attacker">The attacker data object</param>
        /// <param name="distributionType">The type of distribution to create. Defaults to a Binomial distribution.</param>
        /// <returns>A List of BinomomialOutcome data objects, representing a distribution of successful outcomes.</returns>
        public static List<BinomialOutcome> GetDistributionHits(AttackerDTO? attacker, DistributionTypes distributionType = DistributionTypes.Binomial)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetBinomialDistributionOfHits() | Attacker is null. Returning empty list ...");
                return [];
            }

            var minimumAttacks = GetMinimumAttacks(attacker);
            var maximumAttacks = GetMaximumAttacks(attacker);
            var probabilityOfHit = GetProbabilityOfHit(attacker);

            return distributionType switch
            {
                DistributionTypes.Binomial => Statistics.GetBinomialDistribution(minimumAttacks, maximumAttacks, probabilityOfHit),
                DistributionTypes.Cumulative => Statistics.GetCumulativeDistribution(minimumAttacks, maximumAttacks, probabilityOfHit),
                DistributionTypes.Survivor => Statistics.GetSurvivorDistribution(minimumAttacks, maximumAttacks, probabilityOfHit),
                _ => Statistics.GetBinomialDistribution(minimumAttacks, maximumAttacks, probabilityOfHit),
            };
        }

        /// <summary>
        /// Returns the success threshold for wounding the defender.
        /// </summary>
        /// <param name="attackerWeaponStrength"></param>
        /// <param name="defenderToughness"></param>
        /// <returns>An integer value containing the success threshold of the wound roll</returns>
        public static int GetSuccessThresholdOfWound(int attackerWeaponStrength, int defenderToughness)
        {
            if (attackerWeaponStrength <= 0)
            {
                Debug.WriteLine($"GetSuccessThresholdOfWound() | Attacker strength is less than or equal to 0. Returning 6+ ...");
                return 6;
            }

            if (defenderToughness <= 0)
            {
                Debug.WriteLine($"GetSuccessThresholdOfWound() | Defender toughness is less than or equal to 0. Returning 2+ ...");
                return 2;
            }

            // The attacker's weapon Strength is greater than or equal to double the defender's Toughness.
            if (attackerWeaponStrength >= 2 * defenderToughness)
            {
                return 2;
            }

            // The attacker's weapon Strength is greater than, but less than double, the defender's Toughness.
            else if (attackerWeaponStrength > defenderToughness)
            {
                return 3;
            }

            // The attacker's weapon Strength is equal to the defender's Toughness.
            else if (attackerWeaponStrength == defenderToughness)
            {
                return 4;
            }

            // The attacker's weapon Strength is less than, but more than half, the defender's Toughness.
            else if (attackerWeaponStrength > defenderToughness / 2)
            {
                return 5;
            }

            // The attacker's weapon Strength is less than or equal to half the defender's Toughness.
            else
            {
                return 6;
            }
        }

        /// <summary>
        /// Returns the probability of succeeding in both a hit and a wound roll for any one attack.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns>A double value containing the probability of hitting and wounding with a single attack</returns>
        public static double GetProbabilityOfHitAndWound(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetProbabilityOfHitAndWound() | Attacker is null. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetProbabilityOfHitAndWound() | Defender is null. Returning 0 ...");
                return 0;
            }

            // Calculate hit and wound roll probabilities
            var baseHitProbability = GetProbabilityOfHit(attacker);
            var criticalHitProbability = GetProbabilityOfCriticalHit(attacker);
            var normalHitProbability = baseHitProbability - criticalHitProbability;
            var baseWoundProbability = GetBaseProbabilityOfWound(attacker, defender);

            // Lethal Hits will bypass the wound roll.
            // If the attacker has Lethal Hits, we have to use the normal hit probability
            // to avoid double-counting the critical hit probability
            var hitProbability = attacker.WeaponHasLethalHits && !attacker.WeaponHasTorrent ? normalHitProbability : baseHitProbability;

            // Calculate modifiers
            double totalWoundModifiers = 0;

            if (attacker.WeaponHasLethalHits && !attacker.WeaponHasTorrent)
            {
                totalWoundModifiers += GetWoundModifier_LethalHits(criticalHitProbability);
            }

            if (attacker.WeaponHasSustainedHits && !attacker.WeaponHasTorrent)
            {
                totalWoundModifiers += GetWoundModifier_SustainedHits(criticalHitProbability, attacker.WeaponSustainedHitsMultiplier, baseWoundProbability);
            }

            // Reroll wounds overrides reroll wounds of 1
            if (attacker.WeaponHasRerollWoundRolls)
            {
                totalWoundModifiers += GetWoundModifier_RerollWounds(hitProbability, baseWoundProbability);
            }
            else if (attacker.WeaponHasRerollWoundRollsOf1)
            {
                totalWoundModifiers += GetWoundModifier_RerollWoundsOf1(hitProbability, baseWoundProbability);
            }

            // Calculate total wound probability
            var totalWoundProbability = baseWoundProbability + totalWoundModifiers;

            // Calculate combined hit and wound probability
            var totalHitAndWoundProbability = hitProbability * totalWoundProbability;

            return totalHitAndWoundProbability;
        }

        /// <summary>
        /// Returns the mean of the attacker's wound roll distribution.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static double GetMeanWounds(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetMeanWounds() | Attacker is null. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetMeanWounds() | Defender is null. Returning 0 ...");
                return 0;
            }

            var averageAttacks = GetAverageAttacks(attacker);
            var probabilityOfHitAndWound = GetProbabilityOfHitAndWound(attacker, defender);
            return Statistics.GetMeanOfDistribution(averageAttacks, probabilityOfHitAndWound);
        }

        /// <summary>
        /// Gets the discrete expected number of successful wound rolls, based on the average probability.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public static int GetExpectedWounds(AttackerDTO? attacker, DefenderDTO? defender)
        {
            return (int)Math.Floor(GetMeanWounds(attacker, defender));
        }

        /// <summary>
        /// Returns the standard deviation of the attacker's wound roll distribution.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static double GetStandardDeviationWounds(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetStandardDeviationWounds() | Attacker is null. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetStandardDeviationWounds() | Defender is null. Returning 0 ...");
                return 0;
            }

            var averageAttacks = GetAverageAttacks(attacker);
            var varianceAttacks = Statistics.GetVarianceOfResults(attacker.WeaponNumberOfAttackDice, (int)attacker.WeaponAttackDiceType);
            var probabilityOfHitAndWound = GetProbabilityOfHitAndWound(attacker, defender);
            return Statistics.GetCombinedStandardDeviationOfDistribution(averageAttacks, varianceAttacks, probabilityOfHitAndWound);
        }

        /// <summary>
        /// Returns a distribution of hit and wound roll results.
        /// </summary>
        /// <param name="attacker">The attacker data object</param>
        /// <param name="defender">The defender data object</param>
        /// <param name="distributionType">The type of distribution to create. Defaults to a Binomial distribution.</param>
        /// <returns>A List of BinomomialOutcome data objects, representing a distribution of successful outcomes.</returns>
        public static List<BinomialOutcome> GetDistributionWounds(AttackerDTO? attacker, DefenderDTO? defender, DistributionTypes distributionType = DistributionTypes.Binomial)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetBinomialDistributionWounds() | Attacker is null. Returning empty list ...");
                return [];
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetBinomialDistributionWounds() | Defender is null. Returning empty list ...");
                return [];
            }

            var minimumAttacks = GetMinimumAttacks(attacker);
            var maximumAttacks = GetMaximumAttacks(attacker);
            var probability = GetProbabilityOfHitAndWound(attacker, defender);

            return distributionType switch
            {
                DistributionTypes.Binomial => Statistics.GetBinomialDistribution(minimumAttacks, maximumAttacks, probability),
                DistributionTypes.Cumulative => Statistics.GetCumulativeDistribution(minimumAttacks, maximumAttacks, probability),
                DistributionTypes.Survivor => Statistics.GetSurvivorDistribution(minimumAttacks, maximumAttacks, probability),
                _ => Statistics.GetBinomialDistribution(minimumAttacks, maximumAttacks, probability),
            };
        }

        /// <summary>
        /// Returns the adjusted armor save of the defender after applying the attacker's armor pierce.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static int GetAdjustedArmorSaveThreshold(AttackerDTO attacker, DefenderDTO defender)
        {
            if ((defender.ArmorSave <= 0 && defender.InvulnerableSave <= 0)
                || (defender.ArmorSave <= 0 && defender.InvulnerableSave >= 7)
                || (defender.ArmorSave >= 7 && defender.InvulnerableSave <= 0)
                || (defender.ArmorSave >= 7 && defender.InvulnerableSave >= 7))
            {
                Debug.WriteLine($"GetAdjustedArmorSaveThreshold() | Defender has invalid armor save and invulnverable save values. Returning adjusted save value of 7+ ...");
                return 6;
            }

            // If the defender has an invulnerable save, and the invulnerable save is lower than the regular save after applying armor pierce,
            // then use the invulnerable save.
            // Compare against minimum armor save to guard against negative armor pierce values.
            var minimumArmorSave = 2;
            var effectiveInvulnerableSave = defender.InvulnerableSave <= 0 ? 7 : defender.InvulnerableSave;
            var piercedArmorSaveThreshold = defender.ArmorSave + attacker.WeaponArmorPierce;
            var adjustedArmorSave = Math.Min(piercedArmorSaveThreshold, effectiveInvulnerableSave);
            return Math.Max(minimumArmorSave, adjustedArmorSave);
        }

        /// <summary>
        /// Returns the probability of the attacker passing their hit and wound roll, and the defender failing their save, for any one attack.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static double GetProbabilityOfHitAndWoundAndFailedSave(AttackerDTO? attacker, DefenderDTO? defender)
        {
            // Validate inputs
            if (attacker == null)
            {
                Debug.WriteLine($"GetProbabilityOfFailedSave() | Attacker is null, returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetProbabilityOfFailedSave() | Defender is null, returning 0 ...");
                return 0;
            }

            // Base probabilities
            var baseHitProbability = GetProbabilityOfHit(attacker);
            var criticalHitProbability = GetProbabilityOfCriticalHit(attacker);
            var normalHitProbability = baseHitProbability - criticalHitProbability;
            var baseFailedSaveProbability = GetBaseProbabilityOfFailedSave(attacker, defender);
            var hitAndWoundProbability = GetProbabilityOfHitAndWound(attacker, defender);

            // Calculate Devastating Wounds (only from normal hits that roll critical wounds)
            double totalFailedSaveModifiers = 0;

            if (attacker.WeaponHasDevastatingWounds)
            {
                // Adjust hit probability if Lethal Hits are active
                var hitProbability = attacker.WeaponHasLethalHits && !attacker.WeaponHasTorrent ? normalHitProbability : baseHitProbability;
                totalFailedSaveModifiers += GetFailedSaveModifier_DevastatingWounds(hitProbability, GetProbabilityOfCriticalWound(attacker));
            }

            // Calculate total failed save probability
            var totalFailedSaveProbability = baseFailedSaveProbability + totalFailedSaveModifiers;
            return (double)(hitAndWoundProbability * baseFailedSaveProbability);
        }

        /// <summary>
        /// Returns the mean of the failed save roll distribution.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static double GetMeanFailedSaves(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetMeanFailedSaves() | Attacker is null, returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetMeanFailedSaves() | Defender is null, returning 0 ...");
                return 0;
            }

            var averageAttacks = GetAverageAttacks(attacker);
            var probabilityOfFailedSave = GetProbabilityOfHitAndWoundAndFailedSave(attacker, defender);
            return Statistics.GetMeanOfDistribution(averageAttacks, probabilityOfFailedSave);
        }

        /// <summary>
        /// Gets the discrete expected number of failed save rolls, based on the average probability.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public static int GetExpectedFailedSaves(AttackerDTO? attacker, DefenderDTO? defender)
        {
            return (int)Math.Floor(GetMeanFailedSaves(attacker, defender));
        }

        /// <summary>
        /// Returns the standard deviation of the failed save roll distribution.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static double GetStandardDeviationFailedSaves(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetStandardDeviationFailedSaves() | Attacker is null, returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetStandardDeviationFailedSaves() | Defender is null, returning 0 ...");
                return 0;
            }

            var averageAttacks = GetAverageAttacks(attacker);
            var varianceAttacks = Statistics.GetVarianceOfResults(attacker.WeaponNumberOfAttackDice, (int)attacker.WeaponAttackDiceType);
            var probabilityOfHitAndWoundAndFailedSave = GetProbabilityOfHitAndWoundAndFailedSave(attacker, defender);
            return Statistics.GetCombinedStandardDeviationOfDistribution(averageAttacks, varianceAttacks, probabilityOfHitAndWoundAndFailedSave);
        }

        /// <summary>
        /// Returns a distribution of hit and wound and failed save roll results.
        /// </summary>
        /// <param name="attacker">The attacker data object</param>
        /// <param name="defender">The defender data object</param>
        /// <param name="distributionType">The type of distribution to create. Defaults to a Binomial distribution.</param>
        /// <returns>A List of BinomomialOutcome data objects, representing a distribution of successful outcomes.</returns>
        public static List<BinomialOutcome> GetDistributionFailedSaves(AttackerDTO? attacker, DefenderDTO? defender, DistributionTypes distributionType = DistributionTypes.Binomial)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetBinomialDistributionFailedSaves() | Attacker is null. Returning empty list ...");
                return [];
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetBinomialDistributionFailedSaves() | Defender is null. Returning empty list ...");
                return [];
            }

            var minimumAttacks = GetMinimumAttacks(attacker);
            var maximumAttacks = GetMaximumAttacks(attacker);
            var probability = GetProbabilityOfHitAndWoundAndFailedSave(attacker, defender);

            return distributionType switch
            {
                DistributionTypes.Binomial => Statistics.GetBinomialDistribution(minimumAttacks, maximumAttacks, probability),
                DistributionTypes.Cumulative => Statistics.GetCumulativeDistribution(minimumAttacks, maximumAttacks, probability),
                DistributionTypes.Survivor => Statistics.GetSurvivorDistribution(minimumAttacks, maximumAttacks, probability),
                _ => Statistics.GetBinomialDistribution(minimumAttacks, maximumAttacks, probability),
            };
        }

        /// <summary>
        /// Gets the average amount of damage done by the attacker after all hit, wound, and save rolls have been completed.
        /// This does not take into account feel no pain rolls or damage reduction abilities.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static double GetMeanDamage(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetMeanDamageNet() | Attacker is null. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetMeanDamageNet() | Defender is null. Returning 0 ...");
                return 0;
            }

            var meanFailedSaves = GetMeanFailedSaves(attacker, defender);
            var averageDamagePerAttack = GetAverageDamagePerAttack(attacker);
            var averageTotalDamage = meanFailedSaves * averageDamagePerAttack;

            return averageTotalDamage;
        }

        /// <summary>
        /// Gets the discrete expected total amount of damage after all hit, wound, and save rolls have been completed.
        /// This does not take into account feel no pain rolls or damage reduction abilities.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public static int GetExpectedDamage(AttackerDTO? attacker, DefenderDTO? defender)
        {
            return (int)Math.Floor(GetMeanDamage(attacker, defender));
        }

        /// <summary>
        /// Gets the standard deviation of damage done after all hit, wound, and save rolls have been completed.
        /// This does not take into account feel no pain rolls or damage reduction abilities.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static double GetStandardDeviationDamage(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetStandardDeviationDamageNet() | Attacker is null. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetStandardDeviationDamageNet() | Defender is null. Returning 0 ...");
                return 0;
            }

            var averageAttacks = GetAverageAttacks(attacker);
            var varianceAttacks = Statistics.GetVarianceOfResults(attacker.WeaponNumberOfAttackDice, (int)attacker.WeaponAttackDiceType);
            var probabilityOfHitAndWoundAndFailedSave = GetProbabilityOfHitAndWoundAndFailedSave(attacker, defender);
            var averageDamagePerAttack = GetAverageDamagePerAttack(attacker);
            return Statistics.GetCombinedStandardDeviationOfDistribution(averageAttacks, varianceAttacks, probabilityOfHitAndWoundAndFailedSave) * averageDamagePerAttack;
        }

        /// <summary>
        /// Get the mean number of defending models that will be destroyed by the attack.
        /// This takes into account any feel no pain rolls and damage reduction abilities.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns>A double value containing the mean number of models destroyed by the attack.</returns>
        public static double GetMeanDestroyedModels(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetMeanDestroyedModels() | Attacker is null. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetMeanDestroyedModels() | Defender is null. Returning 0 ...");
                return 0;
            }

            var meanFailedSaves = GetMeanFailedSaves(attacker, defender);
            var averageDamagePerAttack = GetAverageDamagePerAttack(attacker);
            var adjustedDamagePerAttack = GetAverageAdjustedDamagePerAttack(averageDamagePerAttack, defender);
            var averageModelsDestroyed = GetModelsDestroyed(meanFailedSaves, adjustedDamagePerAttack, defender);

            return averageModelsDestroyed;
        }

        /// <summary>
        /// Get the expected number of defending models that will be destroyed by the attack.
        /// This takes into account any feel no pain rolls and damage reduction abilities.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns>An integer value containing the expected number of destroyed models</returns>
        public static int GetExpectedDestroyedModels(AttackerDTO? attacker, DefenderDTO? defender)
        {
            return (int)Math.Floor(GetMeanDestroyedModels(attacker, defender));
        }

        /// <summary>
        /// Gets the standard deviation of destroyed models.
        /// This takes into account any feel no pain rolls and damage reduction abilities.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns>An integer value containing the standard deviation of destroyed models.</returns>
        public static double GetStandardDeviationDestroyedModels(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetStandardDeviationDestroyedModels() | Attacker is null. Returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetStandardDeviationDestroyedModels() | Defender is null. Returning 0 ...");
                return 0;
            }

            // Calculate the standard deviation of failed saves
            var averageAttacks = GetAverageAttacks(attacker);
            var varianceAttacks = Statistics.GetVarianceOfResults(attacker.WeaponNumberOfAttackDice, (int)attacker.WeaponAttackDiceType);
            var probabilityOfHitAndWoundAndFailedSave = GetProbabilityOfHitAndWoundAndFailedSave(attacker, defender);
            var standardDeviationSuccessfulAttacks = Statistics.GetCombinedStandardDeviationOfDistribution(averageAttacks, varianceAttacks, probabilityOfHitAndWoundAndFailedSave);

            // Calculate the average amount of adjusted damage done per failed save
            var averageDamagePerAttack = GetAverageDamagePerAttack(attacker);
            var adjustedDamagePerAttack = GetAverageAdjustedDamagePerAttack(averageDamagePerAttack, defender);

            // Calculate how many models would be destroyed
            return GetModelsDestroyed(standardDeviationSuccessfulAttacks, adjustedDamagePerAttack, defender);
        }

        /// <summary>
        /// Returns a distribution of destroyed models from an attack.
        /// </summary>
        /// <param name="attacker">The attacker data object</param>
        /// <param name="defender">The defender data object</param>
        /// <param name="distributionType">The type of distribution to create. Defaults to a Binomial distribution.</param>
        /// <returns>A List of BinomomialOutcome data objects, representing a distribution of destroyed models.</returns>
        public static List<BinomialOutcome> GetDistributionDestroyedModels(AttackerDTO? attacker, DefenderDTO? defender, DistributionTypes distributionType = DistributionTypes.Binomial)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetBinomialDistributionDestroyedModels() | Attacker is null. Returning empty list ...");
                return [];
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetBinomialDistributionDestroyedModels() | Defender is null. Returning empty list ...");
                return [];
            }

            // Get probability of a successful attack
            var probability = GetProbabilityOfHitAndWoundAndFailedSave(attacker, defender);

            // Get upper and lower bounds for the number of trials
            var minimumAttacks = GetMinimumAttacks(attacker);
            var maximumAttacks = GetMaximumAttacks(attacker);

            // Get lower bound of group success count
            var maximumDamagePerAttack = GetMaximumDamagePerAttack(attacker);
            var maximumAdjustedDamagePerAttack = GetMaximumAdjustedDamagePerAttack(maximumDamagePerAttack, defender);
            var minimumAttacksRequiredToDestroyOneModel = GetAttacksRequiredToDestroyOneModel(maximumAdjustedDamagePerAttack, defender);
            var minGroupSuccessCount = minimumAttacksRequiredToDestroyOneModel;

            // Get upper bound of group success count
            var minimumDamagePerAttack = GetMinimumDamagePerAttack(attacker);
            var minimumAdjustedDamagePerAttack = GetMinimumAdjustedDamagePerAttack(minimumDamagePerAttack, defender);
            var maxAttacksRequiredToDestroyOneModel = GetAttacksRequiredToDestroyOneModel(minimumAdjustedDamagePerAttack, defender);
            var maxGroupSuccessCount = maxAttacksRequiredToDestroyOneModel == 0 ? maximumAttacks + 1 : maxAttacksRequiredToDestroyOneModel;

            // Get distribution
            return distributionType switch
            {
                DistributionTypes.Binomial => Statistics.GetBinomialDistribution(minimumAttacks, maximumAttacks, probability, minGroupSuccessCount, maxGroupSuccessCount),
                DistributionTypes.Cumulative => Statistics.GetCumulativeDistribution(minimumAttacks, maximumAttacks, probability, minGroupSuccessCount, maxGroupSuccessCount),
                DistributionTypes.Survivor => Statistics.GetSurvivorDistribution(minimumAttacks, maximumAttacks, probability, minGroupSuccessCount, maxGroupSuccessCount),
                _ => Statistics.GetBinomialDistribution(minimumAttacks, maximumAttacks, probability, minGroupSuccessCount, maxGroupSuccessCount),
            };
        }

        #endregion
    }
}
