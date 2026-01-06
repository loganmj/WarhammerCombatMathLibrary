# Warhammer 40k Combat Rules

This document describes the Warhammer 40k combat rules as implemented by the WarhammerCombatMathLibrary. For the mathematical details and probability calculations behind these rules, see [Math.md](Math.md).

## Overview

Combat in Warhammer 40k is resolved through a sequence of dice rolls that determine whether an attack successfully destroys enemy models. Each attack follows this general sequence:

1. **Hit Roll** - Determine if the attack hits the target
2. **Wound Roll** - Determine if the hit wounds the target
3. **Save Roll** - The defender attempts to negate the wound
4. **Damage** - Apply damage to the target unit
5. **Damage Reduction** (if applicable) - The defender reduces incoming damage
6. **Feel No Pain** (if applicable) - The defender may ignore some remaining damage

## Unit and Weapon Characteristics

### Attacker Characteristics

- **Number of Models**: How many models in the unit are attacking
- **Attacks (A)**: How many attack dice each model rolls (can be fixed or variable)
- **Weapon Skill (WS) / Ballistic Skill (BS)**: The threshold needed to hit (e.g., 3+ means rolling 3 or higher succeeds)
- **Strength (S)**: The strength of the weapon used for wounding
- **Armor Penetration (AP)**: How much the weapon reduces the defender's armor save
- **Damage (D)**: How much damage each successful attack inflicts (can be fixed or variable)
- **Hit Modifier**: Modifier applied to hit rolls, positive values make it easier to hit (capped at +/- 1 after combining with defender modifiers)
- **Wound Modifier**: Modifier applied to wound rolls, positive values make it easier to wound (capped at +/- 1 after combining with defender modifiers)

### Defender Characteristics

- **Number of Models**: How many models are in the defending unit
- **Toughness (T)**: The defender's resistance to being wounded
- **Armor Save (Sv)**: The threshold needed to save against an attack
- **Invulnerable Save (Inv)**: An alternative save that cannot be modified by AP
- **Wounds (W)**: How many wounds each model can sustain before being destroyed
- **Damage Reduction (DR)**: Reduces incoming damage by a fixed amount (minimum 0)
- **Feel No Pain (FNP)**: A special save rolled after damage is allocated and reduced
- **Hit Modifier**: Modifier that affects attacks against this defender, positive values make the defender easier to hit (capped at +/- 1 after combining with attacker modifiers)
- **Wound Modifier**: Modifier that affects wound rolls against this defender, positive values make the defender easier to wound (capped at +/- 1 after combining with attacker modifiers)

## Combat Phases

### 1. Hit Roll

When a model attacks, it rolls dice equal to its Attacks characteristic. Each die must meet or exceed the model's Weapon Skill (for melee) or Ballistic Skill (for ranged) to successfully hit.

**Key Rules:**
- An unmodified roll of 1 always fails
- An unmodified roll of 6 always succeeds
- Critical hits occur when the roll meets or exceeds the critical hit threshold (default is 6, valid range is 2-6)
- **Critical Hit Threshold**: When a unit has an ability that modifies the critical hit threshold (e.g., critical hits on 5+), hit rolls at or above that threshold automatically succeed and count as critical hits
  - These rolls always succeed regardless of the weapon skill requirement
  - They trigger critical hit abilities like Lethal Hits and Sustained Hits
  - The critical hit threshold is based on unmodified die rolls and is not affected by hit modifiers
  - Valid range is 2-6 (1 always fails, 6 is the maximum die result)

**Hit Modifiers:**
Some abilities add to or subtract from the hit success threshold. Both attackers and defenders can have hit modifiers that affect the hit roll:
- Attacker hit modifiers: Positive values make it easier to hit (lower threshold), negative values make it harder (higher threshold)
- Defender hit modifiers: Positive values make the defender easier to hit (debuff), negative values make the defender harder to hit (buff)
- Combined modifiers are capped at +/- 1 maximum after combining attacker and defender modifiers
- **Modified roll thresholds cannot be better than 2+** - rolls of 1 always fail, so the best possible threshold is 2+
- **When a modified threshold is worse than the critical hit threshold, the critical hit threshold takes precedence** - the better (lower) threshold is always used

For example:
- An attacker with +2 to hit and a defender with no modifier results in +1 to hit (capped at maximum)
- An attacker with +1 to hit and a defender with -1 to be hit results in 0 combined modifier
- A hit modifier of +1 changes a 4+ to hit into a 3+ to hit
- A hit modifier of -1 changes a 3+ to hit into a 4+ to hit
- A weapon with 2+ to hit and +1 modifier stays at 2+ (cannot be better than 2+)
- A weapon with 4+ to hit, -1 modifier (making it 5+), and critical hit threshold 3+ uses the 3+ threshold

### 2. Wound Roll

For each successful hit, the attacker rolls to determine if the attack wounds the target. The threshold needed depends on the comparison between the weapon's Strength and the target's Toughness:

- **Strength ≥ 2× Toughness**: Wound on 2+
- **Strength > Toughness**: Wound on 3+
- **Strength = Toughness**: Wound on 4+
- **Strength < Toughness (but > ½ Toughness)**: Wound on 5+
- **Strength ≤ ½ Toughness**: Wound on 6+

**Key Rules:**
- An unmodified roll of 1 always fails
- An unmodified roll of 6 always succeeds
- Critical wounds occur when the roll meets or exceeds the critical wound threshold (default is 6, valid range is 2-6)

**Wound Modifiers:**
Some abilities add to or subtract from the wound success threshold. Both attackers and defenders can have wound modifiers that affect the wound roll:
- Attacker wound modifiers: Positive values make it easier to wound (lower threshold), negative values make it harder (higher threshold)
- Defender wound modifiers: Positive values make the defender easier to wound (debuff), negative values make the defender harder to wound (buff)
- Combined modifiers are capped at +/- 1 maximum after combining attacker and defender modifiers
- **Modified roll thresholds cannot be better than 2+** - rolls of 1 always fail, so the best possible threshold is 2+
- **When a modified threshold is worse than the critical wound threshold, the critical wound threshold takes precedence** - the better (lower) threshold is always used

For example:
- An attacker with +2 to wound and a defender with no modifier results in +1 to wound (capped at maximum)
- An attacker with +1 to wound and a defender with -1 to wound results in 0 combined modifier
- A wound modifier of +1 changes a 4+ to wound into a 3+ to wound
- A wound modifier of -1 changes a 3+ to wound into a 4+ to wound
- A weapon wounding on 2+ with +1 modifier stays at 2+ (cannot be better than 2+)
- A weapon normally wounding on 5+ with -1 modifier (making it 6+) and critical wound threshold 3+ uses the 3+ threshold

### 3. Save Roll

For each successful wound, the defender makes a save roll. The save value used is the defender's Armor Save, modified by the attacker's Armor Penetration.

**Modified Save = Armor Save + Armor Penetration**

For example, if a defender has a 3+ save and the weapon has AP-2, the modified save becomes 5+.

**Invulnerable Saves:**
Some units have an Invulnerable Save that cannot be modified by AP. The defender uses whichever save is better (the lower number needed to succeed):
- Compare the modified Armor Save against the Invulnerable Save
- Use the better (lower) value
- The save cannot be better than 2+

### 4. Damage Allocation

Each attack that successfully wounds and is not saved inflicts damage equal to the weapon's Damage characteristic. When a model accumulates wounds equal to or exceeding its Wounds characteristic, it is destroyed.

### 5. Damage Reduction

Some units have defensive abilities that reduce incoming damage. Damage reduction is applied **before** Feel No Pain rolls:

**How it works:**
- Each successful attack that inflicts damage has its damage reduced by the Damage Reduction value
- Damage cannot be reduced below 0
- This reduction happens before any Feel No Pain rolls are made

For example, if a unit has Damage Reduction 1 and is hit by a weapon that deals 3 damage:
- The damage is reduced to 2 (3 - 1 = 2)
- Then Feel No Pain rolls (if any) are made on those 2 points of damage

If the weapon only deals 1 damage and the unit has Damage Reduction 2:
- The damage is reduced to 0 (max(0, 1 - 2) = 0)
- No damage is dealt, so no Feel No Pain rolls are needed

### 6. Feel No Pain

After damage is allocated and reduced, if the defender has a Feel No Pain ability, they roll once for each point of damage. Each successful roll prevents that point of damage from being applied.

## Variable Stats

### Variable Attacks

Some weapons have a variable number of attacks determined by dice rolls, expressed as **XDy + Z** where:
- X = number of attack dice to roll
- Dy = dice type (D3 or D6)
- Z = flat number of attacks added

Examples:
- **A:5** = 5 attacks (fixed)
- **A:D6** = Roll one D6 for attacks
- **A:2D6+3** = Roll 2D6, add 3

### Variable Damage

Similarly, weapon damage can be variable, expressed as **XDy + Z** where:
- X = number of damage dice to roll
- Dy = dice type (D3 or D6)
- Z = flat damage added

Examples:
- **D:2** = 2 damage (fixed)
- **D:D6** = Roll one D6 for damage
- **D:D3+3** = Roll one D3, add 3

## Weapon Abilities

### Torrent

Weapons with Torrent automatically hit without needing to roll. This means:
- Skip the hit roll phase entirely
- All attacks automatically proceed to the wound roll
- Probability of hitting is 100%

### Lethal Hits

When a critical hit is scored (meeting the critical hit threshold), that attack automatically wounds the target without rolling. This means:
- Critical hits skip the wound roll
- They proceed directly to the save roll
- Non-critical hits still require wound rolls

### Sustained Hits X

When a critical hit is scored, the attack generates X additional hits. These additional hits:
- Still require wound rolls
- Do not themselves generate more hits (no cascading)
- Are resolved along with the original hit

For example, Sustained Hits 1 means each critical hit generates 1 extra hit, for a total of 2 hits.

### Devastating Wounds

When a critical wound is scored (meeting the critical wound threshold), that attack bypasses the defender's save entirely, inflicting mortal wounds. This means:
- Critical wounds skip the save roll
- Damage is applied directly
- Normal wounds still allow save rolls

### Reroll Abilities

Various abilities allow rerolling dice:

**Reroll Hit Rolls:**
- Failed hit rolls can be rerolled once
- Take the result of the second roll

**Reroll Hit Rolls of 1:**
- Only rolls of 1 can be rerolled
- More limited than full rerolls

**Reroll Wound Rolls:**
- Failed wound rolls can be rerolled once

**Reroll Wound Rolls of 1:**
- Only wound rolls of 1 can be rerolled

**Reroll Damage Rolls:**
- The damage dice can be rerolled

**Reroll Damage Rolls of 1:**
- Only damage dice showing 1 can be rerolled

**Important:** Full reroll abilities override the "reroll 1s" version - you cannot use both on the same roll.

### Critical Thresholds

Critical hits and wounds normally occur on unmodified rolls of 6, but some abilities modify these thresholds:
- **Critical Hit Threshold**: The roll value (or higher) needed for a critical hit. **Default is 6. Valid range is 2-6** (1 always fails, 6 is the maximum die result). Critically important: rolls that meet or exceed this threshold **automatically succeed as hits** and count as critical hits, triggering abilities like Lethal Hits and Sustained Hits. For example, a critical hit threshold of 5+ means rolls of 5 and 6 automatically hit and are critical hits.
- **Critical Wound Threshold**: The roll value (or higher) needed for a critical wound. **Default is 6. Valid range is 2-6** (1 always fails, 6 is the maximum die result). For example, a critical wound threshold of 5+ means rolls of 5 and 6 are critical wounds.

**Important Notes:**
- Critical hit/wound thresholds are based on **unmodified die rolls** and are not affected by hit/wound modifiers
- Values outside the range 2-6 are invalid and will be ignored (treated as if no critical threshold was specified)
- When a critical hit threshold is lower than the normal hit threshold (e.g., critical hits on 5+ with WS 4+), the critical hit threshold takes precedence for those rolls, causing them to automatically succeed
- This is similar to how the Anti X+ ability works for wound rolls

**Critical Wound Improvement**: When the Critical Wound Threshold is better (lower) than the normal wound threshold calculated from Strength vs Toughness, wound rolls equal to or higher than the Critical Wound Threshold automatically succeed as critical wounds. This effectively improves the wound success rate while also marking those successes as critical. When combined with Devastating Wounds, this allows more wounds to bypass saves.

**Example**: A weapon with Critical Wound Threshold 4+ against a target where normal wounds require 5+ (S4 vs T5) means that rolls of 4, 5, and 6 all succeed as critical wounds instead of only 5 and 6 succeeding as normal wounds. This increases both the wound success rate and the proportion of critical wounds.

## Model Destruction

Models are destroyed when the total damage allocated to them equals or exceeds their Wounds characteristic. The library calculates expected models destroyed by:

1. Determining total damage dealt after all rolls
2. Accounting for Feel No Pain saves
3. Calculating how damage translates to destroyed models based on wounds per model

When a weapon's damage exceeds a model's wounds (e.g., a 5-damage weapon against 3-wound models), the excess damage does not carry over to other models in most calculations.