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
            return Statistics.ProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, GetNumberOfSuccessfulResults(hitSuccessThreshold));
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
            if (attacker.CriticalHitThreshold <= 0 || attacker.CriticalHitThreshold >= 7)
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
        /// Get the probability of a hit.
        /// Weapon has torrent
        /// </summary>
        /// <returns></returns>
        private static double GetProbabilityOfHit_Torrent()
        {
            return 1;
        }

        /// <summary>
        /// Get the probability of failing a hit roll and re-rolling it into a success.
        /// </summary>
        /// <param name="baseProbabilityOfHit"></param>
        /// <returns></returns>
        private static double GetProbabilityOfHit_FailedHitAndRerolledHit(double baseProbabilityOfHit)
        {
            return (1 - baseProbabilityOfHit) * baseProbabilityOfHit;
        }

        /// <summary>
        /// Get the probability of rolling a hit roll of 1 and re-rolling it into a success
        /// </summary>
        /// <param name="baseProbabilityOfHit"></param>
        /// <returns></returns>
        private static double GetProbabilityOfHit_HitRollOf1AndRerolledHit(double baseProbabilityOfHit)
        {
            return (1.0 / POSSIBLE_RESULTS_SIX_SIDED_DIE) * baseProbabilityOfHit;
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
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        private static double GetBaseProbabilityOfWound(AttackerDTO attacker, DefenderDTO defender)
        {
            var woundSuccessThreshold = GetSuccessThresholdOfWound(attacker.WeaponStrength, defender.Toughness);
            return Statistics.ProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, GetNumberOfSuccessfulResults(woundSuccessThreshold));
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
        /// <returns>A double containing the probability of a non-critical wound roll</returns>
        private static double GetProbabilityOfNormalWound(AttackerDTO attacker, DefenderDTO defender)
        {
            return GetBaseProbabilityOfWound(attacker, defender) - GetProbabilityOfCriticalWound(attacker);
        }

        /// <summary>
        /// Gets the probability of wounding by succeeding on a basic hit and wound roll.
        /// </summary>
        /// <param name="baseProbabilityOfHit"></param>
        /// <param name="baseProbabilityOfWound"></param>
        /// <returns></returns>
        private static double GetProbabilityOfWound_BasicHitAndWound(double baseProbabilityOfHit, double baseProbabilityOfWound)
        {
            return baseProbabilityOfHit * baseProbabilityOfWound;
        }

        /// <summary>
        /// Gets the probability of wounding by succeeding on a normal (non-critical) hit and a basic wound roll.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        private static double GetProbabilityOfWound_NormalHitAndWound(double probabilityOfNormalHit, double baseProbabilityOfWound)
        {
            return probabilityOfNormalHit * baseProbabilityOfWound;
        }

        /// <summary>
        /// Gets the probability of wounding by succeeding on a normal (non-critical) hit, successfully re-rolling it into another normal hit, and succeeding on the wound roll.
        /// </summary>
        /// <param name="probabilityOfNormalHit"></param>
        /// <param name="probabilityOfCriticalHit"></param>
        /// <returns></returns>
        private static double GetProbabilityOfWound_NormalHitAndRerolledNormalHitAndWound(double probabilityOfNormalHit, double baseProbabilityOfWound)
        {
            return probabilityOfNormalHit * GetProbabilityOfWound_NormalHitAndWound(probabilityOfNormalHit, baseProbabilityOfWound);
        }

        /// <summary>
        /// Gets the probability of wounding by succeeding on a normal (non-critical) hit and successfully re-rolling it into a Lethal Hit , bypassing the wound roll.
        /// </summary>
        /// <param name="probabilityOfNormalHit"></param>
        /// <param name="probabilityOfCriticalHit"></param>
        /// <returns></returns>
        private static double GetProbabilityOfWound_NormalHitAndRerolledLethalHit(double probabilityOfNormalHit, double probabilityOfCriticalHit)
        {
            return probabilityOfNormalHit * GetProbabilityOfWound_LethalHit(probabilityOfCriticalHit);
        }

        /// <summary>
        /// Gets the probability of wounding by succeeding on a normal (non-critical) hit and successfully re-rolling it into a critical hit, triggering sustained hits, 
        /// with each of those sustained hits succeeding on a wound roll.
        /// </summary>
        /// <param name="probabilityOfNormalHit"></param>
        /// <param name="probabilityOfCriticalHit"></param>
        /// <param name="sustainedHitsValue"></param>
        /// <param name="baseProbabilityOfWound"></param>
        /// <returns></returns>
        private static double GetProbabilityOfWound_NormalHitAndRerolledSustainedHitsAndWound(double probabilityOfNormalHit, double probabilityOfCriticalHit, int sustainedHitsValue, double baseProbabilityOfWound)
        {
            return probabilityOfNormalHit * GetProbabilityOfWound_SustainedHitsAndWound(probabilityOfCriticalHit, sustainedHitsValue, baseProbabilityOfWound);
        }

        /// <summary>
        /// Gets the probability of wounding by failing a hit, re-rolling into a successful hit, and succeeding on the wound roll.
        /// </summary>
        /// <param name="baseProbabilityOfHit"></param>
        /// <param name="baseProbabilityOfWound"></param>
        /// <returns></returns>
        private static double GetProbabilityOfWound_FailedHitAndRerolledBasicHitAndWound(double baseProbabilityOfHit, double baseProbabilityOfWound)
        {
            return (1 - baseProbabilityOfHit) * GetProbabilityOfWound_BasicHitAndWound(baseProbabilityOfHit, baseProbabilityOfWound);
        }

        /// <summary>
        /// Gets the probability of wounding by failing a hit, successfully re-rolling it into a normal hit, and succeeding on the wound roll.
        /// </summary>
        /// <param name="baseProbabilityOfHit"></param>
        /// <param name="probabilityOfNormalHit"></param>
        /// <param name="baseProbabilityOfWound"></param>
        /// <returns></returns>
        private static double GetProbabilityOfWound_FailedHitAndRerolledNormalHitAndWound(double baseProbabilityOfHit, double probabilityOfNormalHit, double baseProbabilityOfWound)
        {
            return (1 - baseProbabilityOfHit) * GetProbabilityOfWound_NormalHitAndWound(probabilityOfNormalHit, baseProbabilityOfWound);
        }

        /// <summary>
        /// Gets the probability of wounding by failing a hit, and successfully re-rolling it into a Lethal Hit, bypassing the wound roll.
        /// </summary>
        /// <param name="baseProbabilityOfHit"></param>
        /// <param name="probabilityOfCriticalHit"></param>
        /// <returns></returns>
        private static double GetProbabilityOfWound_FailedHitAndRerolledLethalHit(double baseProbabilityOfHit, double probabilityOfCriticalHit)
        {
            return (1 - baseProbabilityOfHit) * GetProbabilityOfWound_LethalHit(probabilityOfCriticalHit);
        }

        /// <summary>
        /// Gets the probability of wounding by failing a hit, successfully re-rolling it into a critical hit, triggering sustained hits, and each of the sustained hits succeeding the wound roll.
        /// </summary>
        /// <param name="baseProbabilityOfHit"></param>
        /// <param name="probabilityOfCriticalHit"></param>
        /// <param name="sustainedHitsValue"></param>
        /// <param name="baseProbabilityOfWound"></param>
        /// <returns></returns>
        private static double GetProbabilityOfWound_FailedHitAndRerolledSustainedHitsAndWound(double baseProbabilityOfHit, double probabilityOfCriticalHit, int sustainedHitsValue, double baseProbabilityOfWound)
        {
            return (1 - baseProbabilityOfHit) * GetProbabilityOfWound_SustainedHitsAndWound(probabilityOfCriticalHit, sustainedHitsValue, baseProbabilityOfWound);
        }

        /// <summary>
        /// Gets the probability of wounding by rolling a hit roll of 1, re-rolling into a successful hit, and succeeding on the wound roll.
        /// </summary>
        /// <param name="baseProbabilityOfHit"></param>
        /// <param name="baseProbabilityOfWound"></param>
        /// <returns></returns>
        private static double GetProbabilityOfWound_HitRollOf1AndRerolledBasicHitAndWound(double baseProbabilityOfHit, double baseProbabilityOfWound)
        {
            return 1.0 / POSSIBLE_RESULTS_SIX_SIDED_DIE * GetProbabilityOfWound_BasicHitAndWound(baseProbabilityOfHit, baseProbabilityOfWound);
        }

        /// <summary>
        /// Gets the probability of wounding by rolling a hit roll of 1, successfully re-rolling it into a normal hit, and succeeding on the wound roll.
        /// </summary>
        /// <param name="probabilityOfNormalHit"></param>
        /// <param name="baseProbabilityOfWound"></param>
        /// <returns></returns>
        private static double GetProbabilityOfWound_HitRollOf1AndRerolledNormalHitAndWound(double probabilityOfNormalHit, double baseProbabilityOfWound)
        {
            return 1.0 / POSSIBLE_RESULTS_SIX_SIDED_DIE * GetProbabilityOfWound_NormalHitAndWound(probabilityOfNormalHit, baseProbabilityOfWound);
        }

        /// <summary>
        /// Gets the probability of wounding by rolling a hit roll of 1, and successfully re-rolling it into a Lethal Hit, bypassing the wound roll.
        /// </summary>
        /// <param name="probabilityOfCriticalHit"></param>
        /// <returns></returns>
        private static double GetProbabilityOfWound_HitRollOf1AndRerolledLethalHit(double probabilityOfCriticalHit)
        {
            return 1.0 / POSSIBLE_RESULTS_SIX_SIDED_DIE * GetProbabilityOfWound_LethalHit(probabilityOfCriticalHit);
        }

        /// <summary>
        /// Gets the probability of wounding by rolling a hit roll of 1, successfully re-rolling it into a critical hit, triggering sustained hits, and each of the sustained hits succeeding the wound roll.
        /// </summary>
        /// <param name="probabilityOfCriticalHit"></param>
        /// <param name="sustainedHitsValue"></param>
        /// <param name="baseProbabilityOfWound"></param>
        /// <returns></returns>
        private static double GetProbabilityOfWound_HitRollOf1AndRerolledSustainedHitsAndWound(double probabilityOfCriticalHit, int sustainedHitsValue, double baseProbabilityOfWound)
        {
            return 1.0 / POSSIBLE_RESULTS_SIX_SIDED_DIE * GetProbabilityOfWound_SustainedHitsAndWound(probabilityOfCriticalHit, sustainedHitsValue, baseProbabilityOfWound);
        }

        /// <summary>
        /// Gets the probability of wounding by rolling a Lethal Hit, bypassing the wound roll.
        /// </summary>
        /// <param name="probabilityOfCriticalHit"></param>
        /// <returns></returns>
        private static double GetProbabilityOfWound_LethalHit(double probabilityOfCriticalHit)
        {
            return probabilityOfCriticalHit;
        }

        /// <summary>
        /// Gets the probability of wounding by rolling a critical hit, triggering sustained hits, and each of the sustained hits succeeding on a wound roll
        /// </summary>
        /// <param name="probabilityOfCriticalHit"></param>
        /// <param name="sustainedHitsMultiplier"></param>
        /// <param name="baseProbabilityOfWound"></param>
        /// <returns></returns>
        private static double GetProbabilityOfWound_SustainedHitsAndWound(double probabilityOfCriticalHit, int sustainedHitsMultiplier, double baseProbabilityOfWound)
        {
            // Return the base hit and wound plus any sustained hits and wounds
            var criticalHitAndWound = probabilityOfCriticalHit * baseProbabilityOfWound;
            var sustainedHitsAndWound = sustainedHitsMultiplier * probabilityOfCriticalHit * baseProbabilityOfWound;
            return criticalHitAndWound + sustainedHitsAndWound;
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
        /// Gets the base probability of hitting and wounding and the defender failing the save roll.
        /// </summary>
        /// <param name="baseProbabilityOfHit"></param>
        /// <param name="baseProbabilityOfWound"></param>
        /// <param name="baseProbabilityOfFailedSave"></param>
        /// <returns></returns>
        private static double GetProbabilityOfFailedSave_BasicHitAndWoundAndFailedSave(double baseProbabilityOfHit, double baseProbabilityOfWound, double baseProbabilityOfFailedSave)
        {
            return baseProbabilityOfHit * baseProbabilityOfWound * baseProbabilityOfFailedSave;
        }

        /// <summary>
        /// Gets the probability of hitting and wounding and the defender failing the save roll.
        /// The attacker has torrent.
        /// </summary>
        /// <param name="baseProbabilityOfWound"></param>
        /// <param name="baseProbabilityOfFailedSave"></param>
        /// <returns></returns>
        private static double GetProbabilityOfFailedSave_TorrentAndWoundAndFailedSave(double baseProbabilityOfWound, double baseProbabilityOfFailedSave)
        {
            return baseProbabilityOfWound * baseProbabilityOfFailedSave;
        }

        /// <summary>
        /// Gets the base probability of hitting and wounding with a normal (non-critical) wound and the defender failing the save roll.
        /// The attacker has torrent
        /// </summary>
        /// <param name="probabilityOfNormalWound"></param>
        /// <param name="baseProbabilityOfFailedSave"></param>
        /// <returns></returns>
        private static double GetProbabilityOfFailedSave_TorrentAndNormalWoundAndFailedSave(double probabilityOfNormalWound, double baseProbabilityOfFailedSave)
        {
            return probabilityOfNormalWound * baseProbabilityOfFailedSave;
        }

        /// <summary>
        /// Gets the base probability of hitting and wounding with a critical wound, triggering a devastating wound, and the defender failing the save roll.
        /// The attacker has torrent
        /// </summary>
        /// <param name="probabilityOfCriticalWound"></param>
        /// <returns></returns>
        private static double GetProbabilityOfFailedSave_TorrentAndDevastatingWound(double probabilityOfCriticalWound)
        {
            return probabilityOfCriticalWound;
        }

        /*

        /// <summary>
        /// Gets the probability of wounding by succeeding on a normal (non-critical) hit and a basic wound roll.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        private static double GetProbabilityOfWound_NormalHitAndWound(double probabilityOfNormalHit, double baseProbabilityOfWound)
        {
            return probabilityOfNormalHit * baseProbabilityOfWound;
        }

        /// <summary>
        /// Gets the probability of wounding by succeeding on a normal (non-critical) hit, successfully re-rolling it into another normal hit, and succeeding on the wound roll.
        /// </summary>
        /// <param name="probabilityOfNormalHit"></param>
        /// <param name="probabilityOfCriticalHit"></param>
        /// <returns></returns>
        private static double GetProbabilityOfWound_NormalHitAndRerolledNormalHitAndWound(double probabilityOfNormalHit, double baseProbabilityOfWound)
        {
            return probabilityOfNormalHit * GetProbabilityOfWound_NormalHitAndWound(probabilityOfNormalHit, baseProbabilityOfWound);
        }

        /// <summary>
        /// Gets the probability of wounding by succeeding on a normal (non-critical) hit and successfully re-rolling it into a Lethal Hit , bypassing the wound roll.
        /// </summary>
        /// <param name="probabilityOfNormalHit"></param>
        /// <param name="probabilityOfCriticalHit"></param>
        /// <returns></returns>
        private static double GetProbabilityOfWound_NormalHitAndRerolledLethalHit(double probabilityOfNormalHit, double probabilityOfCriticalHit)
        {
            return probabilityOfNormalHit * GetProbabilityOfWound_LethalHit(probabilityOfCriticalHit);
        }

        /// <summary>
        /// Gets the probability of wounding by succeeding on a normal (non-critical) hit and successfully re-rolling it into a critical hit, triggering sustained hits, 
        /// with each of those sustained hits succeeding on a wound roll.
        /// </summary>
        /// <param name="probabilityOfNormalHit"></param>
        /// <param name="probabilityOfCriticalHit"></param>
        /// <param name="sustainedHitsValue"></param>
        /// <param name="baseProbabilityOfWound"></param>
        /// <returns></returns>
        private static double GetProbabilityOfWound_NormalHitAndRerolledSustainedHitsAndWound(double probabilityOfNormalHit, double probabilityOfCriticalHit, int sustainedHitsValue, double baseProbabilityOfWound)
        {
            return probabilityOfNormalHit * GetProbabilityOfWound_SustainedHitsAndWound(probabilityOfCriticalHit, sustainedHitsValue, baseProbabilityOfWound);
        }

        /// <summary>
        /// Gets the probability of wounding by failing a hit, re-rolling into a successful hit, and succeeding on the wound roll.
        /// </summary>
        /// <param name="baseProbabilityOfHit"></param>
        /// <param name="baseProbabilityOfWound"></param>
        /// <returns></returns>
        private static double GetProbabilityOfWound_FailedHitAndRerolledBasicHitAndWound(double baseProbabilityOfHit, double baseProbabilityOfWound)
        {
            return (1 - baseProbabilityOfHit) * GetProbabilityOfWound_BasicHitAndWound(baseProbabilityOfHit, baseProbabilityOfWound);
        }

        /// <summary>
        /// Gets the probability of wounding by failing a hit, successfully re-rolling it into a normal hit, and succeeding on the wound roll.
        /// </summary>
        /// <param name="baseProbabilityOfHit"></param>
        /// <param name="probabilityOfNormalHit"></param>
        /// <param name="baseProbabilityOfWound"></param>
        /// <returns></returns>
        private static double GetProbabilityOfWound_FailedHitAndRerolledNormalHitAndWound(double baseProbabilityOfHit, double probabilityOfNormalHit, double baseProbabilityOfWound)
        {
            return (1 - baseProbabilityOfHit) * GetProbabilityOfWound_NormalHitAndWound(probabilityOfNormalHit, baseProbabilityOfWound);
        }

        /// <summary>
        /// Gets the probability of wounding by failing a hit, and successfully re-rolling it into a Lethal Hit, bypassing the wound roll.
        /// </summary>
        /// <param name="baseProbabilityOfHit"></param>
        /// <param name="probabilityOfCriticalHit"></param>
        /// <returns></returns>
        private static double GetProbabilityOfWound_FailedHitAndRerolledLethalHit(double baseProbabilityOfHit, double probabilityOfCriticalHit)
        {
            return (1 - baseProbabilityOfHit) * GetProbabilityOfWound_LethalHit(probabilityOfCriticalHit);
        }

        /// <summary>
        /// Gets the probability of wounding by failing a hit, successfully re-rolling it into a critical hit, triggering sustained hits, and each of the sustained hits succeeding the wound roll.
        /// </summary>
        /// <param name="baseProbabilityOfHit"></param>
        /// <param name="probabilityOfCriticalHit"></param>
        /// <param name="sustainedHitsValue"></param>
        /// <param name="baseProbabilityOfWound"></param>
        /// <returns></returns>
        private static double GetProbabilityOfWound_FailedHitAndRerolledSustainedHitsAndWound(double baseProbabilityOfHit, double probabilityOfCriticalHit, int sustainedHitsValue, double baseProbabilityOfWound)
        {
            return (1 - baseProbabilityOfHit) * GetProbabilityOfWound_SustainedHitsAndWound(probabilityOfCriticalHit, sustainedHitsValue, baseProbabilityOfWound);
        }

        /// <summary>
        /// Gets the probability of wounding by rolling a hit roll of 1, re-rolling into a successful hit, and succeeding on the wound roll.
        /// </summary>
        /// <param name="baseProbabilityOfHit"></param>
        /// <param name="baseProbabilityOfWound"></param>
        /// <returns></returns>
        private static double GetProbabilityOfWound_HitRollOf1AndRerolledBasicHitAndWound(double baseProbabilityOfHit, double baseProbabilityOfWound)
        {
            return 1.0 / POSSIBLE_RESULTS_SIX_SIDED_DIE * GetProbabilityOfWound_BasicHitAndWound(baseProbabilityOfHit, baseProbabilityOfWound);
        }

        /// <summary>
        /// Gets the probability of wounding by rolling a hit roll of 1, successfully re-rolling it into a normal hit, and succeeding on the wound roll.
        /// </summary>
        /// <param name="probabilityOfNormalHit"></param>
        /// <param name="baseProbabilityOfWound"></param>
        /// <returns></returns>
        private static double GetProbabilityOfWound_HitRollOf1AndRerolledNormalHitAndWound(double probabilityOfNormalHit, double baseProbabilityOfWound)
        {
            return 1.0 / POSSIBLE_RESULTS_SIX_SIDED_DIE * GetProbabilityOfWound_NormalHitAndWound(probabilityOfNormalHit, baseProbabilityOfWound);
        }

        /// <summary>
        /// Gets the probability of wounding by rolling a hit roll of 1, and successfully re-rolling it into a Lethal Hit, bypassing the wound roll.
        /// </summary>
        /// <param name="probabilityOfCriticalHit"></param>
        /// <returns></returns>
        private static double GetProbabilityOfWound_HitRollOf1AndRerolledLethalHit(double probabilityOfCriticalHit)
        {
            return 1.0 / POSSIBLE_RESULTS_SIX_SIDED_DIE * GetProbabilityOfWound_LethalHit(probabilityOfCriticalHit);
        }

        /// <summary>
        /// Gets the probability of wounding by rolling a hit roll of 1, successfully re-rolling it into a critical hit, triggering sustained hits, and each of the sustained hits succeeding the wound roll.
        /// </summary>
        /// <param name="probabilityOfCriticalHit"></param>
        /// <param name="sustainedHitsValue"></param>
        /// <param name="baseProbabilityOfWound"></param>
        /// <returns></returns>
        private static double GetProbabilityOfWound_HitRollOf1AndRerolledSustainedHitsAndWound(double probabilityOfCriticalHit, int sustainedHitsValue, double baseProbabilityOfWound)
        {
            return 1.0 / POSSIBLE_RESULTS_SIX_SIDED_DIE * GetProbabilityOfWound_SustainedHitsAndWound(probabilityOfCriticalHit, sustainedHitsValue, baseProbabilityOfWound);
        }

        /// <summary>
        /// Gets the probability of wounding by rolling a Lethal Hit, bypassing the wound roll.
        /// </summary>
        /// <param name="probabilityOfCriticalHit"></param>
        /// <returns></returns>
        private static double GetProbabilityOfWound_LethalHit(double probabilityOfCriticalHit)
        {
            return probabilityOfCriticalHit;
        }

        /// <summary>
        /// Gets the probability of wounding by rolling a critical hit, triggering sustained hits, and each of the sustained hits succeeding on a wound roll
        /// </summary>
        /// <param name="probabilityOfCriticalHit"></param>
        /// <param name="sustainedHitsMultiplier"></param>
        /// <param name="baseProbabilityOfWound"></param>
        /// <returns></returns>
        private static double GetProbabilityOfWound_SustainedHitsAndWound(double probabilityOfCriticalHit, int sustainedHitsMultiplier, double baseProbabilityOfWound)
        {
            // Return the base hit and wound plus any sustained hits and wounds
            var criticalHitAndWound = probabilityOfCriticalHit * baseProbabilityOfWound;
            var sustainedHitsAndWound = sustainedHitsMultiplier * probabilityOfCriticalHit * baseProbabilityOfWound;
            return criticalHitAndWound + sustainedHitsAndWound;
        }

        */

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

            // A weapon with Torrent will automatically hit, bypassing the hit roll
            if (attacker.WeaponHasTorrent)
            {
                return GetProbabilityOfHit_Torrent();
            }

            // Calculate base hit probability
            var baseProbabilityOfHit = GetBaseProbabilityOfHit(attacker);

            return baseProbabilityOfHit
                + (attacker.WeaponHasRerollHitRolls ? GetProbabilityOfHit_FailedHitAndRerolledHit(baseProbabilityOfHit) : 0)
                + (attacker.WeaponHasRerollHitRollsOf1 ? GetProbabilityOfHit_HitRollOf1AndRerolledHit(baseProbabilityOfHit) : 0);

            /*

            // Calculate base hit probability
            var baseProbabilityOfHit = GetBaseProbabilityOfHit(attacker);

            // A weapon with reroll hits will:
            // - On a failed hit, attempt to reroll the dice
            if (attacker.WeaponHasRerollHitRolls)
            {
                return baseProbabilityOfHit
                       + GetProbabilityOfHit_FailedHitAndRerolledHit(baseProbabilityOfHit);
            }

            // A weapon with reroll hits of 1 will:
            // - On a hit roll result of 1, attempt to reroll the dice
            if (attacker.WeaponHasRerollHitRollsOf1)
            {
                return baseProbabilityOfHit
                       + GetProbabilityOfHit_HitRollOf1AndRerolledHit(baseProbabilityOfHit);
            }

            return baseProbabilityOfHit;

            */
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
        /// <param name="attacker"></param>
        /// <returns></returns>
        public static int GetExpectedHits(AttackerDTO? attacker)
        {
            return (int)Math.Floor(GetMeanHits(attacker));
        }

        /// <summary>
        /// Returns the standard deviation of the attacker's hit roll distribution.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
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
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
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
        /// <returns></returns>
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
            var baseProbabilityOfWound = GetBaseProbabilityOfWound(attacker, defender);

            // A weapon with Torrent will automatically hit, bypassing the hit roll (and any critical hit abilities)
            if (attacker.WeaponHasTorrent)
            {
                return baseProbabilityOfWound;
            }

            // Calculate hit roll probability
            var baseProbabilityOfHit = GetBaseProbabilityOfHit(attacker);
            var probabilityOfCriticalHit = GetProbabilityOfCriticalHit(attacker);
            var probabilityOfNormalHit = GetProbabilityOfNormalHit(attacker);

            // A weapon with Reroll hits and Lethal Hits and Sustained Hits X will:
            // - On a failed hit, attempt to reroll the dice (Reroll Hits)
            // - Bypass the wound roll on a critical hit (Lethal Hits)
            // - Add an additional X attacks to the wound roll on a critical hit (Sustained Hits)
            if (attacker.WeaponHasRerollHitRolls && attacker.WeaponHasLethalHits && attacker.WeaponHasSustainedHits)
            {
                return GetProbabilityOfWound_NormalHitAndWound(probabilityOfNormalHit, baseProbabilityOfWound)
                       + GetProbabilityOfWound_NormalHitAndRerolledNormalHitAndWound(probabilityOfNormalHit, baseProbabilityOfWound)
                       + GetProbabilityOfWound_NormalHitAndRerolledLethalHit(probabilityOfNormalHit, probabilityOfCriticalHit)
                       + GetProbabilityOfWound_NormalHitAndRerolledSustainedHitsAndWound(probabilityOfNormalHit, probabilityOfCriticalHit, attacker.WeaponSustainedHitsMultiplier, baseProbabilityOfWound)
                       + GetProbabilityOfWound_FailedHitAndRerolledNormalHitAndWound(baseProbabilityOfHit, probabilityOfNormalHit, baseProbabilityOfWound)
                       + GetProbabilityOfWound_FailedHitAndRerolledLethalHit(baseProbabilityOfHit, probabilityOfCriticalHit)
                       + GetProbabilityOfWound_FailedHitAndRerolledSustainedHitsAndWound(baseProbabilityOfHit, probabilityOfCriticalHit, attacker.WeaponSustainedHitsMultiplier, baseProbabilityOfWound)
                       + GetProbabilityOfWound_LethalHit(probabilityOfCriticalHit)
                       + GetProbabilityOfWound_SustainedHitsAndWound(probabilityOfCriticalHit, attacker.WeaponSustainedHitsMultiplier, baseProbabilityOfWound);
            }

            // A weapon with Reroll hits of 1 and Lethal Hits and Sustained Hits X will:
            // - On a hit roll result of 1, attempt to reroll the dice (Reroll Hits of 1)
            // - Bypass the wound roll on a critical hit (Lethal Hits)
            // - Add an additional X attacks to the wound roll on a critical hit (Sustained Hits)
            if (attacker.WeaponHasRerollHitRollsOf1 && attacker.WeaponHasLethalHits && attacker.WeaponHasSustainedHits)
            {
                return GetProbabilityOfWound_NormalHitAndWound(probabilityOfNormalHit, baseProbabilityOfWound)
                       + GetProbabilityOfWound_HitRollOf1AndRerolledNormalHitAndWound(probabilityOfNormalHit, baseProbabilityOfWound)
                       + GetProbabilityOfWound_HitRollOf1AndRerolledLethalHit(probabilityOfCriticalHit)
                       + GetProbabilityOfWound_HitRollOf1AndRerolledSustainedHitsAndWound(probabilityOfCriticalHit, attacker.WeaponSustainedHitsMultiplier, baseProbabilityOfWound)
                       + GetProbabilityOfWound_LethalHit(probabilityOfCriticalHit)
                       + GetProbabilityOfWound_SustainedHitsAndWound(probabilityOfCriticalHit, attacker.WeaponSustainedHitsMultiplier, baseProbabilityOfWound);
            }

            // A weapon with Reroll Hits, and Lethal HIts will:
            // - On a failed hit, attempt to reroll the dice (Reroll Hits)
            // - Bypass the wound roll on a critical hit (Lethal Hits)
            if (attacker.WeaponHasRerollHitRolls && attacker.WeaponHasLethalHits)
            {
                return GetProbabilityOfWound_NormalHitAndWound(probabilityOfNormalHit, baseProbabilityOfWound)
                       + GetProbabilityOfWound_NormalHitAndRerolledNormalHitAndWound(probabilityOfNormalHit, baseProbabilityOfWound)
                       + GetProbabilityOfWound_NormalHitAndRerolledLethalHit(probabilityOfNormalHit, probabilityOfCriticalHit)
                       + GetProbabilityOfWound_FailedHitAndRerolledNormalHitAndWound(baseProbabilityOfHit, probabilityOfNormalHit, baseProbabilityOfWound)
                       + GetProbabilityOfWound_FailedHitAndRerolledLethalHit(baseProbabilityOfHit, probabilityOfCriticalHit)
                       + GetProbabilityOfWound_LethalHit(probabilityOfCriticalHit);
            }

            // A weapon with Reroll Hits, and Sustained Hits X will:
            // - Add an additional X attacks to the wound roll on a critical hit (Sustained Hits)
            if (attacker.WeaponHasRerollHitRolls && attacker.WeaponHasSustainedHits)
            {
                return GetProbabilityOfWound_NormalHitAndWound(probabilityOfNormalHit, baseProbabilityOfWound)
                       + GetProbabilityOfWound_NormalHitAndRerolledNormalHitAndWound(probabilityOfNormalHit, baseProbabilityOfWound)
                       + GetProbabilityOfWound_NormalHitAndRerolledSustainedHitsAndWound(probabilityOfNormalHit, probabilityOfCriticalHit, attacker.WeaponSustainedHitsMultiplier, baseProbabilityOfWound)
                       + GetProbabilityOfWound_FailedHitAndRerolledNormalHitAndWound(baseProbabilityOfHit, probabilityOfNormalHit, baseProbabilityOfWound)
                       + GetProbabilityOfWound_FailedHitAndRerolledSustainedHitsAndWound(baseProbabilityOfHit, probabilityOfCriticalHit, attacker.WeaponSustainedHitsMultiplier, baseProbabilityOfWound)
                       + GetProbabilityOfWound_SustainedHitsAndWound(probabilityOfCriticalHit, attacker.WeaponSustainedHitsMultiplier, baseProbabilityOfWound);
            }

            // A weapon with Reroll hits of 1 and Lethal Hits and Sustained Hits X will:
            // - On a hit roll result of 1, attempt to reroll the dice (Reroll Hits of 1)
            // - Bypass the wound roll on a critical hit (Lethal Hits)
            if (attacker.WeaponHasRerollHitRollsOf1 && attacker.WeaponHasLethalHits)
            {
                return GetProbabilityOfWound_NormalHitAndWound(probabilityOfNormalHit, baseProbabilityOfWound)
                       + GetProbabilityOfWound_HitRollOf1AndRerolledNormalHitAndWound(probabilityOfNormalHit, baseProbabilityOfWound)
                       + GetProbabilityOfWound_HitRollOf1AndRerolledLethalHit(probabilityOfCriticalHit)
                       + GetProbabilityOfWound_LethalHit(probabilityOfCriticalHit);
            }

            // A weapon with Reroll hits of 1 and Lethal Hits and Sustained Hits X will:
            // - On a hit roll result of 1, attempt to reroll the dice (Reroll Hits of 1)
            // - Add an additional X attacks to the wound roll on a critical hit (Sustained Hits)
            if (attacker.WeaponHasRerollHitRollsOf1 && attacker.WeaponHasSustainedHits)
            {
                return GetProbabilityOfWound_NormalHitAndWound(probabilityOfNormalHit, baseProbabilityOfWound)
                       + GetProbabilityOfWound_HitRollOf1AndRerolledNormalHitAndWound(probabilityOfNormalHit, baseProbabilityOfWound)
                       + GetProbabilityOfWound_HitRollOf1AndRerolledSustainedHitsAndWound(probabilityOfCriticalHit, attacker.WeaponSustainedHitsMultiplier, baseProbabilityOfWound)
                       + GetProbabilityOfWound_SustainedHitsAndWound(probabilityOfCriticalHit, attacker.WeaponSustainedHitsMultiplier, baseProbabilityOfWound);
            }

            // A weapon with Lethal Hits, and Sustained Hits X will:
            // - Bypass the wound roll on a critical hit (Lethal Hits)
            // - Add an additional X attacks to the wound roll on a critical hit (Sustained Hits)
            if (attacker.WeaponHasLethalHits && attacker.WeaponHasSustainedHits)
            {
                return GetProbabilityOfWound_NormalHitAndWound(probabilityOfNormalHit, baseProbabilityOfWound)
                       + GetProbabilityOfWound_LethalHit(probabilityOfCriticalHit)
                       + GetProbabilityOfWound_SustainedHitsAndWound(probabilityOfCriticalHit, attacker.WeaponSustainedHitsMultiplier, baseProbabilityOfWound);
            }

            // A weapon with reroll hits will:
            // - On a failed hit, attempt to reroll the dice
            if (attacker.WeaponHasRerollHitRolls)
            {
                return GetProbabilityOfWound_BasicHitAndWound(baseProbabilityOfHit, baseProbabilityOfWound)
                       + GetProbabilityOfWound_FailedHitAndRerolledBasicHitAndWound(baseProbabilityOfHit, baseProbabilityOfWound);
            }

            // A weapon with reroll hits of 1 will:
            // - On a hit roll result of 1, attempt to reroll the dice
            if (attacker.WeaponHasRerollHitRollsOf1)
            {
                return GetProbabilityOfWound_BasicHitAndWound(baseProbabilityOfHit, baseProbabilityOfWound)
                       + GetProbabilityOfWound_HitRollOf1AndRerolledBasicHitAndWound(baseProbabilityOfHit, baseProbabilityOfWound);
            }

            // A weapon with Lethal Hits will bypass the wound roll on a critical hit
            if (attacker.WeaponHasLethalHits)
            {
                return GetProbabilityOfWound_NormalHitAndWound(probabilityOfNormalHit, baseProbabilityOfWound)
                       + GetProbabilityOfWound_LethalHit(probabilityOfCriticalHit);
            }

            // A weapon with Sustained Hits X will add an additional X attacks to the wound roll on a critical hit
            if (attacker.WeaponHasSustainedHits)
            {
                return GetProbabilityOfWound_NormalHitAndWound(probabilityOfNormalHit, baseProbabilityOfWound)
                       + GetProbabilityOfWound_SustainedHitsAndWound(probabilityOfCriticalHit, attacker.WeaponSustainedHitsMultiplier, baseProbabilityOfWound);
            }

            // Probability of unmodified hit and wound
            return baseProbabilityOfHit * baseProbabilityOfWound;
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

            // Determine base probability of failed save
            var baseProbabilityOfFailedSave = GetBaseProbabilityOfFailedSave(attacker, defender);

            // Determine base probability of wound
            var baseProbabilityOfWound = GetBaseProbabilityOfWound(attacker, defender);
            var probabilityOfCriticalWound = GetProbabilityOfCriticalWound(attacker);
            var probabilityOfNormalWound = GetProbabilityOfNormalWound(attacker, defender);

            // A weapon with Torrent and Devastating Wounds will:
            // - Automatically succeed all hit rolls, bypassing any critical hit abilities (Torrent)
            // - Bypass the save roll on a critical wound (Devastating Wounds)
            if (attacker.WeaponHasTorrent && attacker.WeaponHasDevastatingWounds)
            {
                return GetProbabilityOfFailedSave_TorrentAndNormalWoundAndFailedSave(probabilityOfNormalWound, baseProbabilityOfFailedSave)
                       + GetProbabilityOfFailedSave_TorrentAndDevastatingWound(probabilityOfCriticalWound);
            }

            // A weapon with Torrent and will automatically succeed all hit rolls, bypassing any critical hit abilities
            if (attacker.WeaponHasTorrent)
            {
                return GetProbabilityOfFailedSave_TorrentAndWoundAndFailedSave(baseProbabilityOfWound, baseProbabilityOfFailedSave);
            }

            // For all other calculations, include the hit calculations
            var baseProbabilityOfHit = GetBaseProbabilityOfHit(attacker);
            var probabilityOfCriticalHit = GetProbabilityOfCriticalHit(attacker);
            var probabilityOfNormalHit = GetProbabilityOfNormalHit(attacker);

            // All possible ability combinations:
            // - reroll hits + reroll wounds + lethal + sustained + dev wounds
            // - reroll hits + reroll wounds + lethal + sustained + 0
            // - reroll hits + reroll wounds + lethal + 0 + dev wounds
            // - reroll hits + reroll wounds + lethal + 0 + 0
            // - reroll hits + reroll wounds + 0 + sustained + dev wounds
            // - reroll hits + reroll wounds + 0 + sustained + 0
            // - reroll hits + reroll wounds + 0 + 0 + dev wounds
            // - reroll hits + reroll wounds + 0 + 0 + 0
            // - reroll hits + 0 + lethal + sustained + dev wounds
            // - reroll hits + 0 + lethal + sustained + 0
            // - reroll hits + 0 + lethal + 0 + dev wounds
            // - reroll hits + 0 + lethal + 0 + 0
            // - reroll hits + 0 + 0 + sustained + dev wounds
            // - reroll hits + 0 + 0 + sustained + 0
            // - reroll hits + 0 + 0 + 0 + dev wounds
            // - reroll hits + 0 + 0 + 0 + 0
            // - reroll hits + reroll wounds of 1 + lethal + sustained + dev wounds
            // - reroll hits + reroll wounds of 1 + lethal + sustained + 0
            // - reroll hits + reroll wounds of 1 + lethal + 0 + dev wounds
            // - reroll hits + reroll wounds of 1 + lethal + 0 + 0
            // - reroll hits + reroll wounds of 1 + 0 + sustained + dev wounds
            // - reroll hits + reroll wounds of 1 + 0 + sustained + 0
            // - reroll hits + reroll wounds of 1 + 0 + 0 + dev wounds
            // - reroll hits + reroll wounds of 1 + 0 + 0 + 0
            // - reroll hits + 0 + lethal + sustained + dev wounds
            // - reroll hits + 0 + lethal + sustained + 0
            // - reroll hits + 0 + lethal + 0 + dev wounds
            // - reroll hits + 0 + lethal + 0 + 0
            // - reroll hits + 0 + 0 + sustained + dev wounds
            // - reroll hits + 0 + 0 + sustained + 0
            // - reroll hits + 0 + 0 + 0 + dev wounds
            // - reroll hits + 0 + 0 + 0 + 0
            // - reroll hits of 1 + reroll wounds + lethal + sustained + dev wounds
            // - reroll hits of 1 + reroll wounds + lethal + sustained + 0
            // - reroll hits of 1 + reroll wounds + lethal + 0 + dev wounds
            // - reroll hits of 1 + reroll wounds + lethal + 0 + 0
            // - reroll hits of 1 + reroll wounds + 0 + sustained + dev wounds
            // - reroll hits of 1 + reroll wounds + 0 + sustained + 0
            // - reroll hits of 1 + reroll wounds + 0 + 0 + dev wounds
            // - reroll hits of 1 + reroll wounds + 0 + 0 + 0
            // - reroll hits of 1 + 0 + lethal + sustained + dev wounds
            // - reroll hits of 1 + 0 + lethal + sustained + 0
            // - reroll hits of 1 + 0 + lethal + 0 + dev wounds
            // - reroll hits of 1 + 0 + lethal + 0 + 0
            // - reroll hits of 1 + 0 + 0 + sustained + dev wounds
            // - reroll hits of 1 + 0 + 0 + sustained + 0
            // - reroll hits of 1 + 0 + 0 + 0 + dev wounds
            // - reroll hits of 1 + 0 + 0 + 0 + 0
            // - reroll hits of 1 + reroll wounds of 1 + lethal + sustained + dev wounds
            // - reroll hits of 1 + reroll wounds of 1 + lethal + sustained + 0
            // - reroll hits of 1 + reroll wounds of 1 + lethal + 0 + dev wounds
            // - reroll hits of 1 + reroll wounds of 1 + lethal + 0 + 0
            // - reroll hits of 1 + reroll wounds of 1 + 0 + sustained + dev wounds
            // - reroll hits of 1 + reroll wounds of 1 + 0 + sustained + 0
            // - reroll hits of 1 + reroll wounds of 1 + 0 + 0 + dev wounds
            // - reroll hits of 1 + reroll wounds of 1 + 0 + 0 + 0
            // - reroll hits of 1 + 0 + lethal + sustained + dev wounds
            // - reroll hits of 1 + 0 + lethal + sustained + 0
            // - reroll hits of 1 + 0 + lethal + 0 + dev wounds
            // - reroll hits of 1 + 0 + lethal + 0 + 0
            // - reroll hits of 1 + 0 + 0 + sustained + dev wounds
            // - reroll hits of 1 + 0 + 0 + sustained + 0
            // - reroll hits of 1 + 0 + 0 + 0 + dev wounds
            // - reroll hits of 1 + 0 + 0 + 0 + 0


            /*

            // A weapon with Lethal Hits, Sustained Hits X, and Devastating Wounds will:
            // - Bypass the wound roll on a critical hit (Lethal Hits)
            // - Add an additional X attacks to the wound roll (Sustained Hits)
            // - Bypass the save roll on a critical wound (Devastating Wounds)
            // - A Lethal Hit will bypass the wound roll and cannot trigger Devastating Wounds
            if (attacker.WeaponHasLethalHits && attacker.WeaponHasSustainedHits && attacker.WeaponHasDevastatingWounds)
            {
                // Calculate expected sustained hits
                var expectedSustainedHits = attacker.WeaponSustainedHitsMultiplier * probabilityOfCriticalHit;

                // Calculate probabilities for possible successful outcomes
                var probabilityOfNormalHitAndNormalWoundAndFailedSave = probabilityOfNormalHit * probabilityOfNormalWound * baseProbabilityOfFailedSave;
                var probabilityOfNormalHitAndDevastatingWound = probabilityOfNormalHit * probabilityOfCriticalWound;
                var probabilityOfLethalHitAndFailedSave = probabilityOfCriticalHit * baseProbabilityOfFailedSave;
                var probabilityOfSustainedHitsAndNormalWoundAndFailedSave = expectedSustainedHits * probabilityOfNormalWound * baseProbabilityOfFailedSave;
                var probabilityOfSustainedHitsAndDevastatingWound = expectedSustainedHits * probabilityOfCriticalWound;

                // Combine probabilities
                return probabilityOfNormalHitAndNormalWoundAndFailedSave
                       + probabilityOfNormalHitAndDevastatingWound
                       + probabilityOfLethalHitAndFailedSave
                       + probabilityOfSustainedHitsAndNormalWoundAndFailedSave
                       + probabilityOfSustainedHitsAndDevastatingWound;
            }

            // A weapon with Lethal Hits, and Devastating Wounds will:
            // - Bypass the wound roll on a critical hit (Lethal Hits)
            // - Bypass the save roll on a critical wound (Devastating Wounds)
            // - A Lethal Hit will bypass the wound roll and cannot trigger Devastating Wounds
            if (attacker.WeaponHasLethalHits && attacker.WeaponHasDevastatingWounds)
            {
                // Calculate probabilities for possible successful outcomes
                var probabilityOfNormalHitAndNormalWoundAndFailedSave = probabilityOfNormalHit * probabilityOfNormalWound * baseProbabilityOfFailedSave;
                var probabilityOfNormalHitAndDevastatingWound = probabilityOfNormalHit * probabilityOfCriticalWound;
                var probabilityOfLethalHitAndFailedSave = probabilityOfCriticalHit * baseProbabilityOfFailedSave;

                // Combine probabilities
                return probabilityOfNormalHitAndNormalWoundAndFailedSave
                       + probabilityOfNormalHitAndDevastatingWound
                       + probabilityOfLethalHitAndFailedSave;
            }

            // A weapon with Sustained Hits X, and Devastating Wounds will:
            // - Add an additional X attacks to the wound roll (Sustained Hits)
            // - Bypass the save roll on a critical wound (Devastating Wounds)
            if (attacker.WeaponHasSustainedHits && attacker.WeaponHasDevastatingWounds)
            {
                // Calculate expected sustained hits
                var expectedSustainedHits = attacker.WeaponSustainedHitsMultiplier * probabilityOfCriticalHit;

                // Calculate probabilities for possible successful outcomes
                var probabilityOfNormalHitAndNormalWoundAndFailedSave = probabilityOfNormalHit * probabilityOfNormalWound * baseProbabilityOfFailedSave;
                var probabilityOfNormalHitAndDevastatingWound = probabilityOfNormalHit * probabilityOfCriticalWound;
                var probabilityOfSustainedHitsAndNormalWoundAndFailedSave = expectedSustainedHits * probabilityOfNormalWound * baseProbabilityOfFailedSave;
                var probabilityOfSustainedHitsAndDevastatingWound = expectedSustainedHits * probabilityOfCriticalWound;

                // Combine probabilities
                return probabilityOfNormalHitAndNormalWoundAndFailedSave
                       + probabilityOfNormalHitAndDevastatingWound
                       + probabilityOfSustainedHitsAndNormalWoundAndFailedSave
                       + probabilityOfSustainedHitsAndDevastatingWound;
            }

            // Devastating Wounds will bypass the save roll on a critical wound
            if (attacker.WeaponHasDevastatingWounds)
            {
                // Calculate probabilities for possible successful outcomes
                var probabilityOfHitAndNormalWoundAndFailedSave = baseProbabilityOfHit * probabilityOfNormalWound * baseProbabilityOfFailedSave;
                var probabilityOfHitAndDevastatingWound = baseProbabilityOfHit * probabilityOfCriticalWound;

                // Combine probabilities
                return probabilityOfHitAndNormalWoundAndFailedSave
                       + probabilityOfHitAndDevastatingWound;
            }

            */

            // Probability of unmodified hit, wound, and failed save
            return GetProbabilityOfFailedSave_BasicHitAndWoundAndFailedSave(baseProbabilityOfHit, baseProbabilityOfWound, baseProbabilityOfFailedSave);
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
