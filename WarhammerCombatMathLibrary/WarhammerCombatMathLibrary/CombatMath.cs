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
        private static int GetAverageAttacks(AttackerDTO? attacker)
        {
            // If attacker parameter is null, return 0
            if (attacker == null)
            {
                Debug.WriteLine($"GetAverageAttacks() | Attacker is null, returning 0 ...");
                return 0;
            }

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
        private static int GetMinimumAttacks(AttackerDTO? attacker)
        {
            // If attacker parameter is null, return 0
            if (attacker == null)
            {
                Debug.WriteLine($"GetMinimumAttacks() | Attacker is null, returning 0 ...");
                return 0;
            }

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
        private static int GetMaximumAttacks(AttackerDTO? attacker)
        {
            // If attacker parameter is null, return 0
            if (attacker == null)
            {
                Debug.WriteLine($"GetMaximumAttacks() | Attacker is null, returning 0 ...");
                return 0;
            }

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
        /// Returns the adjusted armor save of the defender after applying the attacker's armor pierce.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        private static int GetAdjustedArmorSaveThreshold(AttackerDTO? attacker, DefenderDTO? defender)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetAdjustedArmorSave() | Attacker is null, returning 0 ...");
                return 0;
            }

            if (defender == null)
            {
                Debug.WriteLine($"GetAdjustedArmorSave() | Defender is null, returning 0 ...");
                return 0;
            }

            // If the defender has an invulnerable save, and the invulnerable save is lower than the regular save after applying armor pierce,
            // then use the invulnerable save.
            var piercedArmorSaveThreshold = defender.ArmorSave + attacker.WeaponArmorPierce;
            return Math.Min(piercedArmorSaveThreshold, defender.InvulnerableSave);
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

            Debug.WriteLine($"GetAverageAdjustedDamagePerAttack() | Raw damage per attack: {damagePerAttack}");
            Debug.WriteLine($"GetAverageAdjustedDamagePerAttack() | Damage reductor: {damageReductor}");
            Debug.WriteLine($"GetAverageAdjustedDamagePerAttack() | Damage after reduction: {damageAfterReduction}");
            Debug.WriteLine($"GetAverageAdjustedDamagePerAttack() | Feel no pain success probability: {feelNoPainSuccessProbability}");
            Debug.WriteLine($"GetAverageAdjustedDamagePerAttack() | Average damage after feel no pain: {averageDamageAfterFeelNoPain}");
            Debug.WriteLine($"GetAverageAdjustedDamagePerAttack() | Return value: {returnValue}");

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

            // If the attacker's weapon has torrent, all attacks will automatically hit.
            if (attacker.WeaponHasTorrent)
            {
                Debug.WriteLine($"GetProbabilityOfHit() | Attacker has torrent, returning 1 ...");
                return 1;
            }

            // A roll of 1 always fails, so if the attacker has a weapon skill value of 1+, treat it as 2+
            var hitSuccessThreshold = attacker.WeaponSkill == 1 ? 2 : attacker.WeaponSkill;
            var numberOfSuccessfulHitResults = GetNumberOfSuccessfulResults(hitSuccessThreshold);
            return Statistics.ProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, numberOfSuccessfulHitResults);
        }

        /// <summary>
        /// Returns the mean of the attacker's hit roll distribution.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
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
        /// Returns a binomial distribution of attack roll results based on the process data.
        /// </summary>
        /// <returns>A BinomialDistribution object containing the hit success data.</returns>
        public static List<BinomialOutcome> GetBinomialDistributionHits(AttackerDTO? attacker)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetBinomialDistributionOfHits() | Attacker is null. Returning empty list ...");
                return [];
            }

            var minimumAttacks = GetMinimumAttacks(attacker);
            var maximumAttacks = GetMaximumAttacks(attacker);
            var probabilityOfHit = GetProbabilityOfHit(attacker);
            return Statistics.BinomialDistribution(minimumAttacks, maximumAttacks, probabilityOfHit);
        }

        /// <summary>
        /// Gets a distribution of all discrete survivor function values for a successful hit roll.
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public static List<BinomialOutcome> GetSurvivorDistributionHits(AttackerDTO? attacker)
        {
            if (attacker == null)
            {
                Debug.WriteLine($"GetSurvivorDistributionHits() | Attacker is null. Returning empty list ...");
                return [];
            }

            var minimumAttacks = GetMinimumAttacks(attacker);
            var maximumAttacks = GetMaximumAttacks(attacker);
            var probabilityOfHit = GetProbabilityOfHit(attacker);
            return Statistics.SurvivorDistribution(minimumAttacks, maximumAttacks, probabilityOfHit);
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
                Debug.WriteLine($"GetSuccessThresholdOfWound() | Defender toughness is less than or equal to 0. Returning 6+ ...");
                return 6;
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

            // Calculate the probability of succeeding on a wound roll
            var woundSuccessThreshold = GetSuccessThresholdOfWound(attacker.WeaponStrength, defender.Toughness);
            var numberOfSuccessfulWoundResults = GetNumberOfSuccessfulResults(woundSuccessThreshold);
            var probabilityOfWound = Statistics.ProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, numberOfSuccessfulWoundResults);

            // A weapon with Torrent and Devastating Wounds will automatically succeed all hit rolls, bypassing any critical hit abilities
            if (attacker.WeaponHasTorrent)
            {
                return probabilityOfWound;
            }

            // For all other calculations, include the hit calculations
            var probabilityOfHit = GetProbabilityOfHit(attacker);
            var probabilityOfCriticalHit = Statistics.ProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, 1);
            var probabilityOfNormalHit = probabilityOfHit - probabilityOfCriticalHit;

            // A weapon with Lethal Hits, and Sustained Hits X will:
            // - Bypass the wound roll on a critical hit (Lethal Hits)
            // - Add an additional X attacks to the wound roll (Sustained Hits)
            if (attacker.WeaponHasLethalHits && attacker.WeaponHasSustainedHits)
            {
                // Calculate expected sustained hits
                var expectedSustainedHits = attacker.WeaponSustainedHitsValue * probabilityOfCriticalHit;

                // Calculate probabilities for possible successful outcomes
                var probabilityOfNormalHitAndWound = probabilityOfNormalHit * probabilityOfWound;
                var probabilityOfLethalHit = probabilityOfCriticalHit;
                var probabilityOfSustainedHitsAndWound = expectedSustainedHits * probabilityOfWound;

                // Combine probabilities
                return probabilityOfNormalHitAndWound
                       + probabilityOfLethalHit
                       + probabilityOfSustainedHitsAndWound;
            }

            // A weapon with Lethal Hits will bypass the wound roll on a critical hit
            if (attacker.WeaponHasLethalHits)
            {
                // Calculate probabilities for possible successful outcomes
                var probabilityOfNormalHitAndWound = probabilityOfNormalHit * probabilityOfWound;
                var probabilityOfLethalHit = probabilityOfCriticalHit;

                // Combine probabilities
                return probabilityOfNormalHitAndWound
                       + probabilityOfLethalHit;
            }

            // A weapon with Sustained Hits X will add an additional X attacks to the wound roll
            if (attacker.WeaponHasSustainedHits)
            {
                var expectedSustainedHits = attacker.WeaponSustainedHitsValue * probabilityOfCriticalHit;

                // Calculate probabilities for possible successful outcomes
                var probabilityOfSustainedHitsAndWound = expectedSustainedHits * probabilityOfWound;
                var probabilityOfHitAndWound = probabilityOfHit * probabilityOfWound;

                // Combine probabilities
                return probabilityOfSustainedHitsAndWound
                       + probabilityOfHitAndWound;
            }

            // Probability of unmodified hit and wound
            return probabilityOfHit * probabilityOfWound;
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

            // Determine armor save
            var adjustedArmorSaveThreshold = GetAdjustedArmorSaveThreshold(attacker, defender);
            var numberOfSuccessfulSaveResults = GetNumberOfSuccessfulResults(adjustedArmorSaveThreshold);
            var probabilityOfSuccessfulSave = Statistics.ProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, numberOfSuccessfulSaveResults);
            var probabilityOfFailedSave = (1 - probabilityOfSuccessfulSave);

            // Determine probability of wound
            var woundThreshold = GetSuccessThresholdOfWound(attacker.WeaponStrength, defender.Toughness);
            var numberOfSuccessfulWoundResults = GetNumberOfSuccessfulResults(woundThreshold);
            var probabilityOfWound = Statistics.ProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, numberOfSuccessfulWoundResults);
            var probabilityOfCriticalWound = Statistics.ProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, 1);
            var probabilityOfNormalWound = probabilityOfWound - probabilityOfCriticalWound;

            // A weapon with Torrent and Devastating Wounds will:
            // - Automatically succeed all hit rolls, bypassing any critical hit abilities
            // - Bypass the save roll on a critical wound
            if (attacker.WeaponHasTorrent && attacker.WeaponHasDevastatingWounds)
            {
                // Probability of wound, and failed save is: P(criticalWound) + (P(normalWound) * P(failedSave))
                return probabilityOfCriticalWound + (probabilityOfNormalWound * probabilityOfFailedSave);
            }

            // A weapon with Torrent and will automatically succeed all hit rolls, bypassing any critical hit abilities
            if (attacker.WeaponHasTorrent)
            {
                // Probability of wound, and failed save is: P(wound) * P(failedSave)
                return probabilityOfWound * probabilityOfFailedSave;
            }

            // For all other calculations, include the hit calculations
            var probabilityOfHit = GetProbabilityOfHit(attacker);
            var probabilityOfCriticalHit = Statistics.ProbabilityOfSuccess(POSSIBLE_RESULTS_SIX_SIDED_DIE, 1);
            var probabilityOfNormalHit = probabilityOfHit - probabilityOfCriticalHit;

            // A weapon with Lethal Hits, Sustained Hits X, and Devastating Wounds will:
            // - Bypass the wound roll on a critical hit (Lethal Hits)
            // - Add an additional X attacks to the wound roll (Sustained Hits)
            // - Bypass the save roll on a critical wound (Devastating Wounds)
            // - A Lethal Hit will bypass the wound roll and cannot trigger Devastating Wounds
            if (attacker.WeaponHasLethalHits && attacker.WeaponHasSustainedHits && attacker.WeaponHasDevastatingWounds)
            {
                // Calculate expected sustained hits
                var expectedSustainedHits = attacker.WeaponSustainedHitsValue * probabilityOfCriticalHit;

                // Calculate probabilities for possible successful outcomes
                var probabilityOfNormalHitAndNormalWoundAndFailedSave = probabilityOfNormalHit * probabilityOfNormalWound * probabilityOfFailedSave;
                var probabilityOfNormalHitAndDevastatingWound = probabilityOfNormalHit * probabilityOfCriticalWound;
                var probabilityOfLethalHitAndFailedSave = probabilityOfCriticalHit * probabilityOfFailedSave;
                var probabilityOfSustainedHitsAndNormalWoundAndFailedSave = expectedSustainedHits * probabilityOfNormalWound * probabilityOfFailedSave;
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
                var probabilityOfNormalHitAndNormalWoundAndFailedSave = probabilityOfNormalHit * probabilityOfNormalWound * probabilityOfFailedSave;
                var probabilityOfNormalHitAndDevastatingWound = probabilityOfNormalHit * probabilityOfCriticalWound;
                var probabilityOfLethalHitAndFailedSave = probabilityOfCriticalHit * probabilityOfFailedSave;

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
                var expectedSustainedHits = attacker.WeaponSustainedHitsValue * probabilityOfCriticalHit;

                // Calculate probabilities for possible successful outcomes
                var probabilityOfNormalHitAndNormalWoundAndFailedSave = probabilityOfNormalHit * probabilityOfNormalWound * probabilityOfFailedSave;
                var probabilityOfNormalHitAndDevastatingWound = probabilityOfNormalHit * probabilityOfCriticalWound;
                var probabilityOfSustainedHitsAndNormalWoundAndFailedSave = expectedSustainedHits * probabilityOfNormalWound * probabilityOfFailedSave;
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
                var probabilityOfHitAndNormalWoundAndFailedSave = probabilityOfHit * probabilityOfNormalWound * probabilityOfFailedSave;
                var probabilityOfHitAndDevastatingWound = probabilityOfHit * probabilityOfCriticalWound;

                // Combine probabilities
                return probabilityOfHitAndNormalWoundAndFailedSave
                       + probabilityOfHitAndDevastatingWound;
            }

            // Probability of unmodified hit, wound, and failed save
            return probabilityOfHit * probabilityOfWound * probabilityOfFailedSave;
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

            Debug.WriteLine($"GetMeanDamage() | Average failed saves: {averageFailedSaves}");
            Debug.WriteLine($"GetMeanDamage() | Average damage per attack: {averageDamagePerAttack}");
            Debug.WriteLine($"GetMeanDamage() | Average total damage: {averageTotalDamage}");

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

            var averageSuccessfulAttacks = GetExpectedFailedSaves(attacker, defender);
            var averageDamagePerAttack = GetAverageDamagePerAttack(attacker);
            var adjustedDamagePerAttack = GetAverageAdjustedDamagePerAttack(averageDamagePerAttack, defender);
            var averageModelsDestroyed = GetModelsDestroyed(averageSuccessfulAttacks, adjustedDamagePerAttack, defender);

            Debug.WriteLine($"GetMeanDestroyedModels() | Average successful attacks: {averageSuccessfulAttacks}");
            Debug.WriteLine($"GetMeanDestroyedModels() | Average damage per attack: {averageDamagePerAttack}");
            Debug.WriteLine($"GetMeanDestroyedModels() | Average adjusted damage per attack: {adjustedDamagePerAttack}");
            Debug.WriteLine($"GetMeanDestroyedModels() | Average models destroyed: {averageModelsDestroyed}");

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
