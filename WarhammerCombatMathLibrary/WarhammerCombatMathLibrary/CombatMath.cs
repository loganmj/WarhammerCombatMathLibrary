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

            var averageAttackDieResult = Statistics.AverageResult((int)attacker.WeaponAttackDiceType);
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
        /// This takes into account the attacker's weapon skill.
        /// </summary>
        /// <param name="attacker">The attacker data object</param>
        /// <returns>A double containing the base probability of a successful hit roll</returns>
        private static double GetBaseProbabilityOfHit(AttackerDTO attacker)
        {
            // Validate inputs
            if (attacker.WeaponSkill <= 0)
            {
                Debug.WriteLine($"GetBaseProbabilityOfHit() | Attacker weapon skill is less than or equal to 0, returning 0 ...");
                return 0;
            }

            // Account for the fact that the smallest possible result on the die is considered an automatic failure,
            // and should not count as part of the success threshold
            var hitSuccessThreshold = attacker.WeaponSkill == AUTOMATIC_FAIL_RESULT ? AUTOMATIC_FAIL_RESULT + 1 : attacker.WeaponSkill;
            var result = Statistics.ProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, GetNumberOfSuccessfulResults(hitSuccessThreshold));

            Debug.WriteLine($"GetBaseProbabilityOfHit() | Hit success threshold: {hitSuccessThreshold}+");
            Debug.WriteLine($"GetBaseProbabilityOfHit() | Hit probability: {result}");

            return result;
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
                return Statistics.ProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, 1);
            }

            // Account for the fact that the smallest possible result on the die is considered an automatic failure,
            // and should not count as part of the success threshold
            var adjustedCriticalHitThreshold = attacker.CriticalHitThreshold == AUTOMATIC_FAIL_RESULT ? AUTOMATIC_FAIL_RESULT + 1 : attacker.CriticalHitThreshold;
            return Statistics.ProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, GetNumberOfSuccessfulResults(adjustedCriticalHitThreshold));
        }

        /// <summary>
        /// Gets the probability of a normal (non-critical) hit roll success.
        /// </summary>
        /// <param name="attacker">The attacker data object</param>
        /// <returns>A double containing the probability of a non-critical hit roll</returns>
        private static double GetProbabilityOfNormalHit(AttackerDTO attacker)
        {
            return GetBaseProbabilityOfHit(attacker) - GetProbabilityOfCriticalHit(attacker);
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
        /// Returns the adjusted armor save of the defender after applying the attacker's armor pierce.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        private static int GetAdjustedArmorSaveThreshold(AttackerDTO attacker, DefenderDTO defender)
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
            var piercedArmorSaveThreshold = defender.ArmorSave + attacker.WeaponArmorPierce;
            var adjustedArmorSave = Math.Min(piercedArmorSaveThreshold, defender.InvulnerableSave);
            return Math.Max(minimumArmorSave, adjustedArmorSave);
        }

        /// <summary>
        /// Gets the base probability of succeeding on a wound roll, based on the attacker and defender stats.
        /// </summary>
        /// <param name="attacker">The attacker data object</param>
        /// <param name="defender">The defender data object</param>
        /// <returns></returns>
        private static double GetBaseProbabilityOfWound(AttackerDTO attacker, DefenderDTO defender)
        {
            var woundSuccessThreshold = GetSuccessThresholdOfWound(attacker.WeaponStrength, defender.Toughness);
            var result = Statistics.ProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, GetNumberOfSuccessfulResults(woundSuccessThreshold));

            Debug.WriteLine($"GetBaseProbabilityOfWound() | Wound success threshold: {woundSuccessThreshold}+");
            Debug.WriteLine($"GetBaseProbabilityOfWound() | Wound probability: {result}");

            return result;
        }

        /// <summary>
        /// Gets the probability of a critical wound.
        /// This is based on the critical wound threshold of the attacker.
        /// </summary>
        /// <param name="attacker">The attacker data object</param>
        /// <returns>A double containing the probability of a critical wound roll</returns>
        private static double GetProbabilityOfCriticalWound(AttackerDTO attacker)
        {
            // If critical hit threshold is out of bounds, return base result
            if (attacker.CriticalWoundThreshold <= 0 || attacker.CriticalWoundThreshold >= 7)
            {
                return Statistics.ProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, 1);
            }

            // Account for the fact that the smallest possible result on the die is considered an automatic failure,
            // and should not count as part of the success threshold
            var adjustedCriticalWoundThreshold = attacker.CriticalWoundThreshold == AUTOMATIC_FAIL_RESULT ? AUTOMATIC_FAIL_RESULT + 1 : attacker.CriticalWoundThreshold;
            return Statistics.ProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, GetNumberOfSuccessfulResults(adjustedCriticalWoundThreshold));
        }

        /// <summary>
        /// Gets the probability of a normal (non-critical) wound roll success.
        /// </summary>
        /// <param name="attacker">The attacker data object</param>
        /// <param name="defender">The defender data object</param>
        /// <returns>A double containing the probability of a non-critical wound roll</returns>
        private static double GetProbabilityOfNormalWound(AttackerDTO attacker, DefenderDTO defender)
        {
            return GetBaseProbabilityOfWound(attacker, defender) - GetProbabilityOfCriticalWound(attacker);
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
        /// <param name="probabilityOfWound"></param>
        /// <returns>A double value containing the probability modifier</returns>
        private static double GetWoundModifier_RerollWounds(double probabilityOfWound)
        {
            return (1 - probabilityOfWound) * probabilityOfWound;
        }

        /// <summary>
        /// Get the wound probability modifier of the Reroll Wounds of 1 ability.
        /// </summary>
        /// <param name="probabilityOfWound"></param>
        /// <returns>A double value containing the probability modifier</returns>
        private static double GetWoundModifier_RerollWoundsOf1(double probabilityOfWound)
        {
            return (1.0 / POSSIBLE_RESULTS_SIX_SIDED_DIE) * probabilityOfWound;
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
            var probabilityOfSuccessfulSave = Statistics.ProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, GetNumberOfSuccessfulResults(adjustedArmorSaveThreshold));
            return 1 - probabilityOfSuccessfulSave;
        }

        /// <summary>
        /// Gets the probability modifier value for hitting and wounding when the attacker has Devastating Wounds.
        /// Devastating Wounds bypass the save roll.
        /// </summary>
        /// <param name="probabilityOfHit"></param>
        /// <param name="probabilityOfWound"></param>
        /// <param name="probabilityOfFailedSave"></param>
        /// <returns>A double value containing the probability modifier</returns>
        private static double GetHitAndWoundAndSaveModifier_RerollHits(double probabilityOfHit, double probabilityOfWound, double probabilityOfFailedSave)
        {
            return (1 - probabilityOfHit) * probabilityOfHit * probabilityOfWound * probabilityOfFailedSave;
        }

        /// <summary>
        /// Gets the probability modifier value for hitting and wounding when the attacker has Devastating Wounds.
        /// Devastating Wounds bypass the save roll.
        /// </summary>
        /// <param name="probabilityOfHit"></param>
        /// <param name="probabilityOfWound"></param>
        /// <param name="probabilityOfFailedSave"></param>
        /// <returns>A double value containing the probability modifier</returns>
        private static double GetHitAndWoundAndSaveModifier_RerollHitsOf1(double probabilityOfHit, double probabilityOfWound, double probabilityOfFailedSave)
        {
            return (1.0 / POSSIBLE_RESULTS_SIX_SIDED_DIE) * probabilityOfWound * probabilityOfWound * probabilityOfFailedSave;
        }

        /// <summary>
        /// Gets the probability modifier value for hitting and wounding when the attacker has Devastating Wounds.
        /// Devastating Wounds bypass the save roll.
        /// </summary>
        /// <param name="probabilityOfNormalHit"></param>
        /// <param name="probabilityOfCriticalHit"></param>
        /// <param name="probabilityOfCriticalWound"></param>
        /// <param name="probabilityOfFailedSave"></param>
        /// <param name="hasLethalHits"></param>
        /// <param name="hasSustainedHits"></param>
        /// <param name="sustainedHitsMultiplier"></param>
        /// <returns>A double value containing the probability modifier</returns>
        private static double GetHitAndWoundAndSaveModifier_DevastatingWounds(double probabilityOfNormalHit, double probabilityOfCriticalHit, double probabilityOfCriticalWound, double probabilityOfFailedSave, bool hasLethalHits, bool hasSustainedHits, int sustainedHitsMultiplier)
        {
            double devastatingWoundsModifier = 0;

            // If attacker has Lethal Hits, dev wounds can only trigger on normal hits
            if (hasLethalHits)
            {
                devastatingWoundsModifier += probabilityOfNormalHit * probabilityOfCriticalWound;
            }
            else
            {
                devastatingWoundsModifier += (probabilityOfNormalHit + probabilityOfCriticalHit) * probabilityOfCriticalWound;
            }

            if (hasSustainedHits)
            {
                devastatingWoundsModifier += sustainedHitsMultiplier * probabilityOfCriticalHit * probabilityOfCriticalWound;
            }

            return devastatingWoundsModifier;
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
            var averageDamagePerDieRoll = Statistics.AverageResult((int)attacker.WeaponDamageDiceType);
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
        /// <returns>An integer value containing the average amount of damage per attack after applying defensive modifiers</returns>
        private static int GetAverageAdjustedDamagePerAttack(int damagePerAttack, DefenderDTO? defender)
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

            // TODO: Account for damage reduction
            var damageReductor = 0;
            var damageAfterReduction = damagePerAttack - damageReductor;

            // Account for feel no pains
            var feelNoPainSuccessProbability = Statistics.ProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, GetNumberOfSuccessfulResults(defender.FeelNoPain));
            var averageDamageAfterFeelNoPain = damageAfterReduction * (1 - feelNoPainSuccessProbability);
            var returnValue = (int)Math.Floor(averageDamageAfterFeelNoPain);

            return returnValue;
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

            // If the defender has feel no pains, then the minimum amount of damage is 0, since it is possible for the defender to succeed in all of their feel no pain rolls.
            if (defender.FeelNoPain >= 2 && defender.FeelNoPain <= 6)
            {
                Debug.WriteLine($"GetMinimumAdjustedDamagePerAttack() | Defender has feel no pains. Returning 0 ...");
                return 0;
            }

            // TODO: Account for damage reduction
            var damageReductor = 0;
            var damageAfterReduction = damagePerAttack - damageReductor;

            Debug.WriteLine($"GetMinimumAdjustedDamagePerAttack() | Raw damage per attack: {damagePerAttack}");
            Debug.WriteLine($"GetMinimumAdjustedDamagePerAttack() | Damage reductor: {damageReductor}");
            Debug.WriteLine($"GetMinimumAdjustedDamagePerAttack() | Damage after reduction: {damageAfterReduction}");
            Debug.WriteLine($"GetMinimumAdjustedDamagePerAttack() | Return value: {damageAfterReduction}");

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

            // TODO: Account for damage reduction
            var damageReductor = 0;
            var damageAfterReduction = damagePerAttack - damageReductor;

            Debug.WriteLine($"GetMaximumAdjustedDamagePerAttack() | Raw damage per attack: {damagePerAttack}");
            Debug.WriteLine($"GetMaximumAdjustedDamagePerAttack() | Damage reductor: {damageReductor}");
            Debug.WriteLine($"GetMaximumAdjustedDamagePerAttack() | Damage after reduction: {damageAfterReduction}");
            Debug.WriteLine($"GetMaximumAdjustedDamagePerAttack() | Feel no pain rolls are assumed to fail");
            Debug.WriteLine($"GetMaximumAdjustedDamagePerAttack() | Return value: {damageAfterReduction}");

            // Feel no pains are ignored, as the maximum amount of damage is done when defender fails all of their feel no pain rolls
            return damageAfterReduction;
        }

        /// <summary>
        /// Determines the maximum possible number of models destroyed given a specified amount of applied damage.
        /// </summary>
        /// <param name="numberOfAttacks">The number of successful attacks</param>
        /// <param name="damagePerAttack">The damage per attack from the attacker's weapon.</param>
        /// <param name="defender">The defending unit data.</param>
        /// <returns>An integer value representing the number of defending models that will be destroyed by the attack.</returns>
        private static int GetModelsDestroyed(int numberOfAttacks, int damagePerAttack, DefenderDTO? defender)
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
            var modelsDestroyed = (int)Math.Floor(totalDamage / damageThreshold);

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
        /// <returns>A double value containing the probability of succeeding on any single hit roll.</returns>
        public static double GetProbabilityOfHit(AttackerDTO? attacker)
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
                Debug.WriteLine($"GetProbabilityOfHit() | Attacker has Torrent. Returning 1");
                return 1;
            }

            // Calculate base hit probability
            var baseHitProbability = GetBaseProbabilityOfHit(attacker);

            // Calculate modifiers
            double totalHitModifiers = 0;

            // Full rerolls overrides rerolls of 1
            if (attacker.WeaponHasRerollHitRolls)
            {
                var rerollModifier = GetHitModifier_RerollHits(baseHitProbability);
                Debug.WriteLine($"GetProbabilityOfHit() | Reroll hits modifier: {rerollModifier}");
                totalHitModifiers += rerollModifier;
            }
            else if (attacker.WeaponHasRerollHitRollsOf1)
            {
                var rerollModifier = GetHitModifier_RerollHitsOf1(baseHitProbability);
                Debug.WriteLine($"GetProbabilityOfHit() | Reroll hits of 1 modifier: {rerollModifier}");
                totalHitModifiers += rerollModifier;
            }

            var totalHitProbability = baseHitProbability + totalHitModifiers;

            Debug.WriteLine($"GetProbabilityOfHitAndWound() | Base hit probability: {baseHitProbability}");
            Debug.WriteLine($"GetProbabilityOfHitAndWound() | Total hit modifiers: {totalHitModifiers}");
            Debug.WriteLine($"GetProbabilityOfHitAndWound() | Total hit probability: {totalHitProbability}");

            return totalHitProbability;
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
            return Statistics.Mean(averageNumberOfAttacks, probabilityOfHit);
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
            var probabilityOfHit = GetProbabilityOfHit(attacker);
            return Statistics.StandardDeviation(averageAttacks, probabilityOfHit);
        }

        /// <summary>
        /// Returns a calculated distribution of hit roll results.
        /// </summary>
        /// <param name="attacker">The attacker data object</param>
        /// <param name="distributionType">The type of distribution to create. Defaults to a Binomial distribution.</param>
        /// <returns>A List of BinomomialOutcome data objects, representing a distribution of hit roll outcomes.</returns>
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
                DistributionTypes.Binomial => Statistics.BinomialDistribution(minimumAttacks, maximumAttacks, probabilityOfHit),
                DistributionTypes.Cumulative => Statistics.CumulativeDistribution(minimumAttacks, maximumAttacks, probabilityOfHit),
                DistributionTypes.Survivor => Statistics.SurvivorDistribution(minimumAttacks, maximumAttacks, probabilityOfHit),
                _ => Statistics.BinomialDistribution(minimumAttacks, maximumAttacks, probabilityOfHit),
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
            Debug.WriteLine($"GetSuccessThresholdOfWound() | Attacker strength: {attackerWeaponStrength}");
            Debug.WriteLine($"GetSuccessThresholdOfWound() | Defender toughness: {defenderToughness}");

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

            // Calculate wound roll probability
            var baseWoundProbability = GetBaseProbabilityOfWound(attacker, defender);

            Debug.WriteLine($"GetProbabilityOfHitAndWound() | Base wound probability: {baseWoundProbability}");

            // A weapon with Torrent will automatically hit, bypassing the hit roll (and any critical hit abilities)
            if (attacker.WeaponHasTorrent)
            {
                Debug.WriteLine($"GetProbabilityOfHitAndWound() | Weapon has torrent. Hits auto-succeed. Total hit and wound probability: {baseWoundProbability}");
                return baseWoundProbability;
            }

            var baseHitProbability = GetProbabilityOfHit(attacker);
            var criticalHitProbability = GetProbabilityOfCriticalHit(attacker);
            var normalHitProbability = baseHitProbability - criticalHitProbability;

            // Lethal Hits will bypass the wound roll.
            // If the attacker has Lethal Hits, we have to use the normal hit probability
            // to avoid double-counting the critical hit probability
            var hitProbability = attacker.WeaponHasLethalHits ? normalHitProbability : baseHitProbability;

            // DEBUG
            if (attacker.WeaponHasLethalHits)
            {
                Debug.WriteLine($"GetProbabilityOfHitAndWound() | Attacker has Lethal Hits.");
                Debug.WriteLine($"GetProbabilityOfHitAndWound() | Separating base hit probability {baseHitProbability} into normal hit {normalHitProbability} and critical hit {criticalHitProbability}");
            }

            // Calculate modifiers
            double totalWoundModifiers = 0;

            if (attacker.WeaponHasLethalHits)
            {
                var lethalHitsModifier = GetWoundModifier_LethalHits(criticalHitProbability);
                Debug.WriteLine($"GetProbabilityOfHitAndWound() | Lethal Hits modifier: {lethalHitsModifier}");
                totalWoundModifiers += lethalHitsModifier;
            }

            if (attacker.WeaponHasSustainedHits)
            {
                var sustainedHitsModifier = GetWoundModifier_SustainedHits(criticalHitProbability, attacker.WeaponSustainedHitsMultiplier, baseWoundProbability);
                Debug.WriteLine($"GetProbabilityOfHitAndWound() | Sustained Hits {attacker.WeaponSustainedHitsMultiplier} modifier: {sustainedHitsModifier}");
                totalWoundModifiers += sustainedHitsModifier;
            }

            // Reroll wounds overrides reroll wounds of 1
            if (attacker.WeaponHasRerollWoundRolls)
            {
                var rerollModifier = GetWoundModifier_RerollWounds(baseWoundProbability);
                Debug.WriteLine($"GetProbabilityOfHitAndWound() | Reroll wounds modifier: {rerollModifier}");
                totalWoundModifiers += rerollModifier;
            }
            else if (attacker.WeaponHasRerollWoundRollsOf1)
            {
                var rerollModifier = GetWoundModifier_RerollWoundsOf1(baseWoundProbability);
                Debug.WriteLine($"GetProbabilityOfHitAndWound() | Reroll wounds modifier: {rerollModifier}");
                totalWoundModifiers += rerollModifier;
            }

            // Calculate total wound probability
            var totalWoundProbability = baseWoundProbability + totalWoundModifiers;

            // Calculate combined hit and wound probability
            var totalHitAndWoundProbability = hitProbability * totalWoundProbability;

            Debug.WriteLine($"GetProbabilityOfHitAndWound() | Total wound modifiers: {totalWoundModifiers}");
            Debug.WriteLine($"GetProbabilityOfHitAndWound() | Total wound probability: {totalWoundProbability}");
            Debug.WriteLine($"GetProbabilityOfHitAndWound() | Total hit and wound probability: {hitProbability} * {totalWoundProbability} = {totalHitAndWoundProbability}");

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
            return Statistics.Mean(averageAttacks, probabilityOfHitAndWound);
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
            var probabilityOfWound = GetProbabilityOfHitAndWound(attacker, defender);
            return Statistics.StandardDeviation(averageAttacks, probabilityOfWound);
        }

        /// <summary>
        /// Returns a binomial distribution of wound roll results based on the process data.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static List<BinomialOutcome> GetBinomialDistributionWounds(AttackerDTO? attacker, DefenderDTO? defender)
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
            return Statistics.BinomialDistribution(minimumAttacks, maximumAttacks, probability);
        }

        /// <summary>
        /// Gets a distribution of all discrete survivor function values for a successful hit and wound roll.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public static List<BinomialOutcome> GetSurvivorDistributionWounds(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetSurvivorDistributionWounds() | Attacker is null. Returning empty list ...");
                return [];
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetSurvivorDistributionWounds() | Defender is null. Returning empty list ...");
                return [];
            }

            var minimumAttacks = GetMinimumAttacks(attacker);
            var maximumAttacks = GetMaximumAttacks(attacker);
            var probability = GetProbabilityOfHitAndWound(attacker, defender);
            return Statistics.SurvivorDistribution(minimumAttacks, maximumAttacks, probability);
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

            // Probability of unmodified hit, wound, and failed save
            return 0;
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
            return Statistics.Mean(averageAttacks, probabilityOfFailedSave);
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
            var probabilityOfFailedSave = GetProbabilityOfHitAndWoundAndFailedSave(attacker, defender);
            return Statistics.StandardDeviation(averageAttacks, probabilityOfFailedSave);
        }

        /// <summary>
        /// Returns a binomial distribution of rolls where the hit and wound have succeeded, and the opponent failed their save.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static List<BinomialOutcome> GetBinomialDistributionFailedSaves(AttackerDTO? attacker, DefenderDTO? defender)
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
            return Statistics.BinomialDistribution(minimumAttacks, maximumAttacks, probability);
        }

        /// <summary>
        /// Gets a distribution of all discrete survivor function values for a successful hit and wound, and a failed save roll.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public static List<BinomialOutcome> GetSurvivorDistributionFailedSaves(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetSurvivorDistributionFailedSaves() | Attacker is null. Returning empty list ...");
                return [];
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetSurvivorDistributionFailedSaves() | Defender is null. Returning empty list ...");
                return [];
            }

            var minimumAttacks = GetMinimumAttacks(attacker);
            var maximumAttacks = GetMaximumAttacks(attacker);
            var probability = GetProbabilityOfHitAndWoundAndFailedSave(attacker, defender);
            return Statistics.SurvivorDistribution(minimumAttacks, maximumAttacks, probability);
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

            var averageFailedSaves = GetMeanFailedSaves(attacker, defender);
            var averageDamagePerAttack = GetAverageDamagePerAttack(attacker);
            var averageTotalDamage = averageFailedSaves * averageDamagePerAttack;

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

            var averageDamagePerAttack = GetAverageDamagePerAttack(attacker);
            var standardDeviationFailedSaves = GetStandardDeviationFailedSaves(attacker, defender);
            return standardDeviationFailedSaves * averageDamagePerAttack;
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

            var expectedSuccessfulAttacks = GetExpectedFailedSaves(attacker, defender);
            var averageDamagePerAttack = GetAverageDamagePerAttack(attacker);
            var adjustedDamagePerAttack = GetAverageAdjustedDamagePerAttack(averageDamagePerAttack, defender);
            var averageModelsDestroyed = GetModelsDestroyed(expectedSuccessfulAttacks, adjustedDamagePerAttack, defender);

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
        public static int GetStandardDeviationDestroyedModels(AttackerDTO? attacker, DefenderDTO? defender)
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

            var standardDeviationSuccessfulAttacks = Math.Floor(GetStandardDeviationFailedSaves(attacker, defender));
            var averageDamagePerAttack = GetAverageDamagePerAttack(attacker);
            var adjustedDamagePerAttack = GetAverageAdjustedDamagePerAttack(averageDamagePerAttack, defender);
            var standardDeviationDestroyedModels = GetModelsDestroyed((int)standardDeviationSuccessfulAttacks, adjustedDamagePerAttack, defender);

            Debug.WriteLine($"GetStandardDeviationDestroyedModels() | Standard deviation successful attacks: {standardDeviationSuccessfulAttacks}");
            Debug.WriteLine($"GetStandardDeviationDestroyedModels() | Average damage per attack: {averageDamagePerAttack}");
            Debug.WriteLine($"GetStandardDeviationDestroyedModels() | Average adjusted damage per attack: {adjustedDamagePerAttack}");
            Debug.WriteLine($"GetStandardDeviationDestroyedModels() | Standard deviation models destroyed: {standardDeviationDestroyedModels}");

            return standardDeviationDestroyedModels;
        }

        /// <summary>
        /// Gets the binomial distribution of destroyed models.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static List<BinomialOutcome> GetBinomialDistributionDestroyedModels(AttackerDTO? attacker, DefenderDTO? defender)
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
            return Statistics.BinomialDistribution(minimumAttacks, maximumAttacks, probability, minGroupSuccessCount, maxGroupSuccessCount);
        }

        /// <summary>
        /// Gets the survivor distribution of destroyed models.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static List<BinomialOutcome> GetSurvivorDistributionDestroyedModels(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetSurvivorDistributionDestroyedModels() | Attacker is null. Returning empty list ...");
                return [];
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetSurvivorDistributionDestroyedModels() | Defender is null. Returning empty list ...");
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
            return Statistics.SurvivorDistribution(minimumAttacks, maximumAttacks, probability, minGroupSuccessCount, maxGroupSuccessCount);
        }

        #endregion
    }
}
