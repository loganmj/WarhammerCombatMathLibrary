# Warhammer 40k Combat Rules

This document describes the Warhammer 40k combat rules as implemented by the WarhammerCombatMathLibrary. For the mathematical details and probability calculations behind these rules, see [Math.md](Math.md).

## Overview

Combat in Warhammer 40k is resolved through a sequence of dice rolls that determine whether an attack successfully destroys enemy models. Each attack follows this general sequence:

1. **Hit Roll** - Determine if the attack hits the target
2. **Wound Roll** - Determine if the hit wounds the target
3. **Save Roll** - The defender attempts to negate the wound
4. **Damage** - Apply damage to the target unit
5. **Feel No Pain** (if applicable) - The defender may ignore some damage

## Unit and Weapon Characteristics

### Attacker Characteristics

- **Number of Models**: How many models in the unit are attacking
- **Attacks (A)**: How many attack dice each model rolls (can be fixed or variable)
- **Weapon Skill (WS) / Ballistic Skill (BS)**: The threshold needed to hit (e.g., 3+ means rolling 3 or higher succeeds)
- **Strength (S)**: The strength of the weapon used for wounding
- **Armor Penetration (AP)**: How much the weapon reduces the defender's armor save
- **Damage (D)**: How much damage each successful attack inflicts (can be fixed or variable)

### Defender Characteristics

- **Number of Models**: How many models are in the defending unit
- **Toughness (T)**: The defender's resistance to being wounded
- **Armor Save (Sv)**: The threshold needed to save against an attack
- **Invulnerable Save (Inv)**: An alternative save that cannot be modified by AP
- **Wounds (W)**: How many wounds each model can sustain before being destroyed
- **Feel No Pain (FNP)**: A special save rolled after damage is allocated

## Combat Phases

### 1. Hit Roll

When a model attacks, it rolls dice equal to its Attacks characteristic. Each die must meet or exceed the model's Weapon Skill (for melee) or Ballistic Skill (for ranged) to successfully hit.

**Key Rules:**
- An unmodified roll of 1 always fails
- An unmodified roll of 6 always succeeds
- Critical hits occur when the roll meets or exceeds the critical hit threshold (default is 6)

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
- Critical wounds occur when the roll meets or exceeds the critical wound threshold (default is 6)

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

### 5. Feel No Pain

After damage is allocated, if the defender has a Feel No Pain ability, they roll once for each point of damage. Each successful roll prevents that point of damage from being applied.

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
- **Critical Hit Threshold**: The roll value (or higher) needed for a critical hit
- **Critical Wound Threshold**: The roll value (or higher) needed for a critical wound

For example, a critical hit threshold of 5+ means rolls of 5 and 6 are critical hits.

## Model Destruction

Models are destroyed when the total damage allocated to them equals or exceeds their Wounds characteristic. The library calculates expected models destroyed by:

1. Determining total damage dealt after all rolls
2. Accounting for Feel No Pain saves
3. Calculating how damage translates to destroyed models based on wounds per model

When a weapon's damage exceeds a model's wounds (e.g., a 5-damage weapon against 3-wound models), the excess damage does not carry over to other models in most calculations.