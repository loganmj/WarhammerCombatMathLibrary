# Probability Statistics and Warhammer
This project aims to determine the probability of success in combat using the rules of combat in Warhammer 40k. This project will make use of concepts from the probability theory of statistics to perform these evaluations. Combat 
in Warhammer 40k has many influencing parameters that must be input by the user and included in the combat calculation. At it's core, combat is determined using six sided dice to determine outcomes. Each die roll has a discrete 
success/failure case, and is evaluated independently. All calculations in this project will be based on these discrete die rolls, which in the context of statistics are called a *Bernoulli trial* or *Bernoulli experiment*, and 
a sequence of such outcomes is called a *Bernoulli process*.

This document outlines the concepts and steps necessary to perform combat calculations for Warhammer 40k, and discloses how probability values are determined.

## Probability of Success in a Single Trial
Statistics is based on probability, expressed as a decimal value, with the highest probability being 1. A probability of 1 represents an event that is guaranteed to occur. In statistics, probability of success in a 
trial is denoted by $p$. Probability can also be expressed as a percent value, with 1 being 100% probability, and all other $p$ values being some probability below 100.

In the context of a single trial, the probability of success can be calculated as:

$$p = \frac{k}{n}$$

where
- $n$ is the total number of possible results
- $k$ is the number of results that are considered a success

In the context of dice, there are 6 possible results for rolling a six-sided dice. Because each outcome is equally likely, the probability of any particular result is:

$$\frac{1}{6} = 0.1666$$

or 16.66%.

What if the success case includes more than one result value? In the context of Warhammer, a success may be defined as rolling a 4+ on a dice. This means that a result of 4, 5, or 6 would be considered a success. In this case,
the probabilities of each possible successful result can be added together to determine the probability of the success case as a whole. The probability of rolling a 4, a 5, or a 6 are all 1/6. So the probability 
of the entire success case can be calculated as:

$$p(\text{4, 5, or 6}) = \frac{1}{6} + \frac{1}{6} + \frac{1}{6} = \frac{3}{6} = \frac{1}{2} = 0.5$$

Or 50%. This makes sense, as half of the results on the dice are considered a successful roll.

Given a max number of possible results, and a success threshold value, the probability of getting a result within the success threshold can be calculated as:

$$p(X \geq k) = \frac{n - (k - 1)}{n}$$

where 
- $n$ is the total number of possible results
- $k$ is the success threshold value

## Probability of Failure in a Single Trial
Probability of failure is defined as the probability that success will *not* occur, and is denoted by $q$.

$$q = 1 - p$$

In the context of dice, if the probability of any one result is $\frac{1}{6}$, then the probability of *failing* to roll any one result is: 

$$q = 1 - p = \left(1 - \frac{1}{6}\right) = \frac{5}{6} = 0.8333$$

or 83.33%.

If the success case includes multiple values, such as rolling a 3+, then the probability of failure can be calculated as:

$$q = 1-p = \left(1- \left(\frac{1}{6} + \frac{1}{6} + \frac{1}{6} + \frac{1}{6}\right)\right) = \left(1- \left(\frac{4}{6}\right)\right) = \frac{2}{6} = 0.3333$$

or 33.33%

Given a max number of possible results, and a success threshold value, the probability of getting a result below the success threshold can be calculated as:

$$q(X \geq k) = 1 - p(X \geq k) = \left(1 - \left(\frac{n - (k - 1)}{n}\right)\right)$$

where 
- $n$ is the total number of possible results
- $k$ is the success threshold value

## Probability of Success for Common Cases
In Warhammer 40k, success cases are commonly set as 2+, 3+, 4+, 5+, or 6+ (Rolling a 1 is almost always considered a failure). Each of these success cases on a single die have the following probabilities:

| Roll | $p$ |
|----------|----------|
| 2+ | 83.33% |
| 3+ | 66.66% |
| 4+ | 50% |
| 5+ | 33.33% |
| 6+ | 16.66% |

## Probability of Success in Multiple Trials
In Warhammer, combat is rarely determined by the roll of a single die. Trials must often be repeated and passed multiple times to be considered a success. In stastics this can be expressed by multiplying together the 
probabilities of each roll.

$$p_0 * p_1 * p_2 ... * p_n$$

where
- $p_n$ is the probability of success for the $n^th$ trial.

For example, say the die must be rolled three times. The success case for the first trial is a roll of 3+, the success case for the second trial is 4+, and the success case for the third roll is 5+. That all three trials will 
result in success is calculated as:

$$\frac{4}{6} * \frac{3}{6} * frac{2}{6} = \frac{24}{216} = \frac{1}{9}$$ = 0.1111

or 11.11%

In some cases, the success case will be the same for all trials in the process, meaning that the probability of success is the same for each trial. In such a case, the equation to calculate the probability of success in all 
trials can be simplified as:

$$p^k$$

where
- $p$ is the probability of success in any one trial
- $k$ is the total number of trials
- all values of $p$ are equal

If the success case is a result of 4+, and the die is rolled five times, the probability that the die will succeed all five times is calculated as:

$$\left(\frac{3}{6}\right)^5 = \left(\frac{1}{2}\right)^5 = \frac{1}{32} = 0.0313$$

or 3.13%

## Probability of Failure for Multiple Trials
The probability of failure for multiple trials is calculated the same way as the probability of success, using the same conversion between success and failure:

$$q_0 * q_1 * q_2 ... * q_n = (p_0 - 1) * (p_1 - 1) * (p_2 - 1) ... * (p_n - 1)$$

And if the probability for success/failure is the same in all trials, the probability of failure is calculated as:

$$q^k = (1-p)^k$$

where
- $p$ is the probability of success in any one trial
- $k$ is the total number of trials
- all values of $p$ are equal

If the success case is a result of 3+ (and therefore the fail case is rolling a 1 or a 2), and the die is rolled five times, the probability that the die will *fail* all five times is calculated as:

$$\left(\frac{2}{6}\right)^5 = \left(\frac{1}{3}\right)^5 = \frac{1}{243} = 0.0041$$

or 0.41%

## The Binomial Coefficient
In Warhammer, it is common that combat is resolved by rolling multiple dice at a time. In such a case, it is important to be able to calculate the probability of any number of independent successes within the context of the 
group of dice. Such a calculation is called the *biomial distribution*. The math required to calculate a binomial distribution will be covered later, after a few more concepts have been explained.

Before a binomial distribution can be can be calculated, it is important to first calculate the *binomial coefficient*. The binomial coefficient is a scalar value used to represent the number of possible *combinations* of 
elements in a set, ignoring the order of selection. This coefficient is expressed in shorthand as $\binom{n}{k}$, and is 
calculated using the equation:

$$\binom{n}{k} = \frac{n!}{k!(n-k)!}$$

where 
- $n$ is the total number of objects in the set
- $k$ is the number of objects to select

For example, consider a group of five objects, labeled 1 through 5. The task is to select any two objects from this group of five. How many combinations of two objects exist in the group? Assuming that order does not matter, 
a fixed number of *combinations* of objects can be selected from the group, as shown below:
 
| Selection 1 | Selection 2 |
|----------|----------|
| Object 1 | Object 2 |
| Object 1 | Object 3 |
| Object 1 | Object 4 |
| Object 1 | Object 5 |
| Object 2 | Object 3 |
| Object 2 | Object 4 |
| Object 2 | Object 5 |
| Object 3 | Object 4 |
| Object 3 | Object 5 |
| Object 4 | Object 5 |

The table shows that there are 10 possible combinations of two objects that can be made from a group of five. This number of combinations can be determined mathematically by using the binomial coefficient equation:

$$\frac{n!}{k!(n-k)!} = \frac{5!}{2!(5-2)!} = 10$$

## Probability Mass Function
The *probability mass function* is an equation that can determine the probability for finding a number of successful results in a population of independent trials. In other words, for a group of trials where the results of each 
trial do not affect the results of the other trials (such as rolling a group of dice), the probability mass function is used to determine the probability that a specified number of those trials were successful. In the context
of Warhammer, this is the equation used to determine the probability that, given a number of dice to roll and a success case, a specified number of those dice would succeed the roll. The probability mass function is expressed
as:

$$f(k,n,p) = P(X = k) = \binom{n}{k} p^k (1-p)^{n-k}$$

where
- $k$ represents the number of successful trials
- $n$ represents the total number of trials
- $p$ represents the probability of success for a single trial

The probability mass function can be broken down into three parts for better understanding:

- The binomial coefficient $\binom{n}{k}$, which accounts for all of the different possible combinations of trials that contain the desired number of successes
- The probability $p^k$ of obtaining a sequence of $n$ independent trials in which $k$ trials are successes
- The probability that the remaining $(n − k)$ trials result in failure, expressed as $(1-p)^{n-k}$

For example, an attack roll requires the player to roll 10 dice that will succeed on a result of 3+. What is the probability that exactly six of the dice will result in a successful roll? This can be calculated using the 
probability mass function as follows:

$$P(X = 6) = \binom{10}{6} \left(\frac{4}{6}\right)^6 \left(1-\left(\frac{4}{6}\right)\right)^{10-6} = 210 * (0.6666)^6 * (0.3334)^4 = 0.2276$$

or 22.76%

## Binomial Distribution
The binomial distribution is a discrete probability distribution that displays the probabilities of all of the possible results of $n$ trials. In essence, a binomial distribution is a set of probability mass functions that
give the probability for every possible number of successes in a set of trials. In the context of Warhammer, a binomial distribution of a combat roll will show the computed probability for each number of successfull results
when all of the dice have been rolled.

A binomial distribution function is expressed using the following notation:

$$B(n,p)$$

where
- $n$ is the number of independent trials.
- $p$ is the probability of success for any given trial.

In the previous section, it was shown how the probability mass function could be used to calculate the probability of getting 6 successes when rolling 10 dice that succeed on a roll of 3+. Further probability mass calculations
could be used to determine the probability of each possible number of successes (from 0 to 10). The result would be the binomial distribution of the attack roll. The table below shows the results of calculating the binomial
distribution of this attack roll.

Binomial distribution of an attack roll of 10 dice, hitting on 3+:

| Successes | $p$ |
|----------|----------|
| 0 | 0.00% |
| 1 | 0.03% |
| 2 | 0.30% |
| 3 | 1.63% |
| 4 | 5.69% |
| 5 | 13.66% |
| 6 | 22.76% |
| 7 | 26.01% |
| 8 | 19.51% |
| 9 | 8.67% |
| 10 | 1.73% |

## Mean and Standard Deviation
Calculating the mean of the binomial data will determine the expected average number of successes to expect from an attack roll. The mean value can be calculated as:

$$Mean[B(n,p)] = np$$

where
- $n$ is the number of trials
- $p$ is the probability of success for a single trial

Calculating the standard deviation of the binomial data will determine the expected number of successes above or below the average value due to normal variance. Standard deviation can be calculated as:

$$SD[B(n,p)] = \sqrt{np(1-p)}$$

where
- $n$ is the number of trials
- $p$ is the probability of success for a single trial

## Binomial Sums
Probability mass functions can be added together to form binomial sums. A binomial sum shows the combined probability of getting different amounts of successes in the trials. For example, using the table from the previous 
section, it can be determined that the probability of getting either 5 or 6 successes is the sum of their individual probabilities:

$$P((X = 5) + (X = 6)) = P(X = 5) + P(X = 6) = 0.1366 + 0.2276 = 0.3642$$

or 36.42% chance of rolling either 5 or 6 successes.

This is especially useful when calculating *cumulative* probabilities. A cumulative probabilty is a sum of all probabilities up to and including a certain number of successes. That is, a cumulative probability will be a 
calculation of either $P(X \leq k)$ (lower cumulative probability) or $P(X \geq k)$ (upper cumulative probability).

Cumulative probabilities can be expressed using the following equations:

Lower Cumulative Probability

$$F(n,k,p) = P(X \leq k) = \displaystyle\sum_{i=0}^k \binom{n}{i} p^i (1-p)^{n-i}$$

Upper Cumulative Probability

$$F(n,k,p) = P(X \geq k) = \displaystyle\sum_{i=k}^n \binom{n}{i} p^i (1-p)^{n-i}$$

where
- $n$ is the total number of trials
- $k$ is the number of successes
- $p$ is the probability of success for a single trial

Notice that these equations are just the probability mass function put through a summation from either 0 to $k$ (lower cumulative probability) or from $k$ to $n$ (upper cumulative probability).

Continuing the example of rolling 10 dice, and needing a roll of 3+ to succeed, it may be useful to calculate the probability of rolling *at least* 6 successes. In this case, an upper cumulative probability function can be used 
like so:

$$P(X \geq 6) = \displaystyle\sum_{i=6}^10 \binom{10}{i} \left(\frac{4}{6}\right)^i \left(1-\left(\frac{4}{6}\right)\right)^{10-i} = 0.7869$$

or 78.69% chance of rolling at least 6 successes.

## Cumulative Distribution
The cumulative distribution is similar to the binomial distribution, but it displays the set of all cumulative probabilities (either upper or lower bound) for a set of trials. The upper cumulative distribution for rolling 10 dice
with a success being a roll of 3+ is shown below:

| Successes | $p$ |
|----------|----------|
| 0 | 100.00% |
| 1 | 100.00% |
| 2 | 99.96% |
| 3 | 99.66% |
| 4 | 98.03% |
| 5 | 92.34% |
| 6 | 78.69% |
| 7 | 55.93% |
| 8 | 29.91% |
| 9 | 10.40% |
| 10 | 1.73% |

Notice that the data shows an average roll should always produce at least 1 success. Cumulative data can be useful for predicting whether a roll is likely to produce at least some number of successes, and can be used to help 
make strategic decisions.

## Predicting Results of A Hit Roll
In Warhammer 40k, the attacker will make an attack using $x$ models, using a weapon with an Attacks stat of $y$. This requires them to makes a hit roll using $x \ast y$ dice. Success is determined by the Ballistic Skill 
(for ranged weapons) or Weapon Skill (for melee weapons) stat of the attacker's weapon.

For example, a Space Marine Intercessor model may make an attack with 10 models, each wielding a Bolt Rifle. The Bolt Rifle has an Attacks stat of 2, and a Ballistic Skill of 3+. This means that a hit roll will consist of 
rolling 20 dice, and succeeding on a roll of 3, 4, 5, or 6.

The expected mean number of successes and the standard deviation can be calculated as:

$$Mean[B(20, 0.6666)] = (20)(0.6666) = 13.33$$

$$SD[B(20, 0.6666)] = \sqrt{(20)(0.6666)(1 - 0.6666)} = 2.11$$

The cumulative distribution can be calculated to determine the likelihood of rolling $k$ successes for all values of $k$ from $0$ to $n$:

| Successes | $P(X \geq k)$ |
|----------|----------|
| 0 | 100.00% |
| 1 | 100.00% |
| 2 | 100.00% |
| 3 | 100.00% |
| 4 | 100.00% |
| 5 | 100.00% |
| 6 | 99.98% |
| 7 | 99.91% |
| 8 | 99.63% |
| 9 | 98.70% |
| 10 | 96.24% |
| 11 | 90.81% |
| 12 | 80.95% |
| 13 | 66.15% |
| 14 | 47.93% |
| 15 | 29.72% |
| 16 | 15.15% |
| 17 | 6.04% |
| 18 | 1.76% |
| 19 | 0.33% |
| 20 | 0.03% |

## Predicting Results of a Wound Roll
In Warhammer 40k after a hit roll is made, the attacker must then make a wound roll, to determine if the hit is able to wound the defender. The success threshold of the wound roll depends on the ratio of Strength of the 
attacker's weapon and the Toughness stat of the defender. The table below describes this relationship:

| Ratio | Wound Success | Description |
|----------|----------|----------|
| $r \geq 2.0$ | 2+ | The attacker's weapon Strength is greater than or equal to double the defender's Toughness. |
| $1.0<r<2.0$ | 3+ | The attacker's weapon Strength is greater than, but less than double, the defender's Toughness. |
| $r = 1.0$ | 4+ | The attacker's weapon Strength is equal to the defender's Toughness. |
| $0.5 < r < 1.0$ | 5+ | The attacker's weapon Strength is less than, but more than half, the defender's Toughness. |
| $r \leq 0.5$ | 6+ | The attacker's weapon Strength is less than or equal to half the defender's Toughness. |

The probability of an attack succeeding both the hit and the wound roll would be calculated by multiplying together the probability of the hit success and the probability of the wound roll, like so:

$$P(\text{hit and wound}) = (p_\text{hit})(p_\text{wound})$$

For example, if a unit of 10 Space Marine Intercessors fire their Bolt Rifles into another unit of 10 Space Marine Intercessors, the probability of succeeding a hit and wound roll for any one die would be calculated as follows:

Parameters:
- Bolt Rifle Weapon Skill: 3+
- Bolt Rifle Strength: 4
- Intercessor Toughness: 4

$$P(\text{hit and wound}) = (p_\text{hit})(p_\text{wound}) = (0.6666)(0.5) = 0.3333$$

or 33.33%

The cumulative probability distribution would be then calculated by plugging $P(\text{hit and wound})$ in for $p$ in the equation:

| Successes | $P(X \geq k)$ |
|----------|----------|
| 0 | 100.00% |
| 1 | 99.97% |
| 2 | 99.67% |
| 3 | 98.24% |
| 4 | 93.96% |
| 5 | 84.85% |
| 6 | 70.28% |
| 7 | 52.07% |
| 8 | 33.85% |
| 9 | 19.05% |
| 10 | 9.19% |
| 11 | 3.76% |
| 12 | 1.30% |
| 13 | 0.37% |
| 14 | 0.09% |
| 15 | 0.02% |
| 16 | 0.00% |
| 17 | 0.00% |
| 18 | 0.00% |
| 19 | 0.00% |
| 20 | 0.00% |

## Predicting the Results of the Save roll
In Warhammer 40k, after the hit and wound rolls are finished, the opponent must make a save roll, to determine if they are able to perform armor saves against the successful wound rolls. To include the defender's save 
roll in the projection, it is necessary to calculate the probability that the defender *fails* their save roll, making the compound calculation:

$$P(\text{attack}) = (p_\text{hit})(p_\text{wound})(q_\text{save}) = (p_\text{hit})(p_\text{wound})(1 - p_\text{save})$$

The defender's Armor Save stat is expressed as a die roll threshold, like the attacker's Ballistic/Weapon Skill stat. This threshold can be used to determine the save probability and plug it into the probability calculation.
Continuing the example from the previous section, the Save stat of the Space Marine Intercessor unit is 3+. This means that the probability of any one attack succeeding the hit and wound rolls, and the opponent failing their
save roll is calculated as follows:

$$P(\text{attack}) = (p_\text{hit})(p_\text{wound})(q_\text{save}) = (p_\text{hit})(p_\text{wound})(1 - p_\text{save}) = (0.6666)(0.5)(1 - 0.6666) = 0.1111$$

or 11.11%.

The updated cumulative probability distribution would be then calculated by plugging $P(\text{hit and wound, and failed save})$ in for $p$:

| Successes | $P(X \geq k)$ |
|----------|----------|
| 0 | 100.00% |
| 1 | 99.97% |
| 2 | 99.67% |
| 3 | 98.24% |
| 4 | 93.96% |
| 5 | 84.85% |
| 6 | 70.28% |
| 7 | 52.07% |
| 8 | 33.85% |
| 9 | 19.05% |
| 10 | 9.19% |
| 11 | 3.76% |
| 12 | 1.30% |
| 13 | 0.37% |
| 14 | 0.09% |
| 15 | 0.02% |
| 16 | 0.00% |
| 17 | 0.00% |
| 18 | 0.00% |
| 19 | 0.00% |
| 20 | 0.00% |

## Factoring in Armor Pierce
In Warhammer 40k, weapons have an Armor Pierce stat. The Armor Pierce stat of a weapon is applied as a direct debuff to the defender's save stat. For example, if the defender has an armor save of 3+ and the attacker's weapon 
has an Armor Pierce of 1, then the defender's save is treated as a 4+ instead.

The probability of a successful save roll, then must factor in the attacker's Armor Pierce stat as follows:

$$p(X \geq k) = \frac{n - (k - 1) + j}{n}$$

where 
- $n$ is the total number of possible results
- $k$ is the success threshold value
- $j$ is the attacker's weapon Armor Pierce stat

## Factoring in Invulnerable saves
In Warhammer 40k, units can have an Invulnerable Save stat. An invulnerable save is an alternative save stat that puts an upper limit on the effect of armor pierce. For example, Space Marine Terminators have a Save stat of 2+ 
and an Invulnerable Save stat of 4+. This means that the effects of armor pierce (or any similar effect that negatively impacts the save value) can at worst reduce the effective save stat to a 4+. If a unit of Space Marine
Terminators were attacked with a weapon that had an Armor Pierce of 3, then instead of their Save stat being reduced to 5+ for the calculation, the Invulnerable Save value of 4+ is used instead.

When calculating the effect of armor pierce on the save roll, the invulnerable save can be factored in as follows:

$$I = \min(n - (k - 1) + j, h)$$

where
- $I$ represents the effective save value
- $n$ is the total number of possible results
- $k$ is the success threshold value
- $j$ is the attacker's weapon Armor Pierce stat
- $h$ is the defender's Invulnerable Save stat

## Critical Hits and Critical Wounds
In Warhammer 40k, certain hit and wound rolls are considered "critical" based on specific thresholds. By default, an unmodified roll of 6 is a critical hit or critical wound. However, some abilities can change these thresholds.

A critical hit threshold determines which hit roll results are considered critical hits. For example, if a weapon has a critical hit threshold of 5+, then rolls of 5 or 6 are critical hits.

Similarly, a critical wound threshold determines which wound roll results are considered critical wounds.

The probability of rolling a critical hit can be calculated as:

$$p_{\text{crit}} = \frac{n - (k_{\text{crit}} - 1)}{n}$$

where
- $p_{\text{crit}}$ is the probability of a critical hit or wound
- $n$ is the total number of possible results (6 for a standard die)
- $k_{\text{crit}}$ is the critical threshold value

Critical hits and wounds are important because they trigger various weapon abilities, which are discussed in the following sections.

## Weapon Abilities

### Torrent
The Torrent ability allows a weapon to automatically hit without rolling. This means that the probability of a successful hit roll is 1 (or 100%).

When a weapon has Torrent:

$$p_{\text{hit}} = 1$$

This simplifies all subsequent calculations, as every attack automatically succeeds the hit roll.

### Lethal Hits
The Lethal Hits ability causes critical hits to automatically wound the target without requiring a wound roll. When a critical hit occurs, it bypasses the wound roll entirely and proceeds directly to the save roll.

To calculate the probability of hitting and wounding with Lethal Hits, we must separate critical hits from normal hits:

$$p_{\text{normal hit}} = p_{\text{hit}} - p_{\text{crit hit}}$$

The total probability of hitting and wounding becomes:

$$p_{\text{hit and wound}} = (p_{\text{normal hit}} \times p_{\text{wound}}) + p_{\text{crit hit}}$$

where critical hits automatically succeed the wound roll, contributing their full probability to the hit and wound result.

### Sustained Hits
The Sustained Hits ability generates additional hit rolls when a critical hit is rolled. The number of additional hits is determined by a multiplier value (typically 1 for Sustained Hits 1, 2 for Sustained Hits 2, etc.).

The additional hits from Sustained Hits must still roll to wound normally. The probability modifier from Sustained Hits can be calculated as:

$$m_{\text{sustained}} = x \times p_{\text{crit hit}} \times p_{\text{wound}}$$

where
- $m_{\text{sustained}}$ is the probability modifier from Sustained Hits
- $x$ is the Sustained Hits multiplier value
- $p_{\text{crit hit}}$ is the probability of a critical hit
- $p_{\text{wound}}$ is the probability of wounding

This modifier is added to the base hit and wound probability.

### Devastating Wounds
The Devastating Wounds ability causes critical wound rolls to bypass the save roll entirely, inflicting mortal wounds directly. Similar to Lethal Hits, this requires separating critical wounds from normal wounds.

The probability modifier from Devastating Wounds can be calculated as:

$$m_{\text{dev wounds}} = p_{\text{hit}} \times p_{\text{crit wound}}$$

where
- $m_{\text{dev wounds}}$ is the probability modifier from Devastating Wounds
- $p_{\text{hit}}$ is the probability of hitting
- $p_{\text{crit wound}}$ is the probability of a critical wound

This represents attacks that hit, roll a critical wound, and bypass the save entirely.

### Reroll Abilities
Several abilities allow rerolling dice under specific conditions. These abilities affect the probability calculations by giving a second chance at success.

#### Reroll All Failed Rolls
When an ability allows rerolling all failed rolls (for hits, wounds, or damage), the probability modifier is:

$$m_{\text{reroll}} = (1 - p) \times p$$

where
- $m_{\text{reroll}}$ is the probability modifier from rerolling
- $p$ is the base probability of success

This represents the chance that a failed roll (probability $1 - p$) succeeds on the reroll (probability $p$).

#### Reroll Rolls of 1
When an ability allows rerolling only rolls of 1, the probability modifier is:

$$m_{\text{reroll 1}} = \frac{1}{6} \times p$$

where
- $m_{\text{reroll 1}}$ is the probability modifier from rerolling 1s
- $p$ is the base probability of success

This represents the chance that a roll of 1 (probability $\frac{1}{6}$) succeeds on the reroll (probability $p$).

These reroll modifiers can be applied to hit rolls, wound rolls, and damage rolls independently.

## Variable Damage
Similar to variable attacks, weapons can have variable damage values determined by dice rolls. A weapon's damage characteristic can be expressed as:

$$\text{Damage} = (X)(Dy) + Z$$

where
- $X$ is the number of damage dice to roll
- $Dy$ is the dice type (D3 or D6)
- $Z$ is the flat damage value

The average damage per attack can be calculated as:

$$\text{Avg Damage} = (X \times \text{Avg}(Dy)) + Z$$

For example, a weapon with damage D6+2 would have an average damage of $(1 \times 3.5) + 2 = 5.5$ per attack.

The minimum damage per attack is:

$$\text{Min Damage} = X + Z$$

The maximum damage per attack is:

$$\text{Max Damage} = (X \times Dy) + Z$$

where
- $X$ is the number of damage dice to roll
- $Dy$ is the **maximum value** of the die type (e.g., 3 for D3, 6 for D6)
- $Z$ is the flat damage value

> **Note:** In this formula, $Dy$ represents the maximum value of the die type, not just the die type itself.
## Feel No Pain
Feel No Pain is a defensive ability that gives the defender a chance to ignore damage. After a model would lose a wound, the Feel No Pain roll is made for each point of damage. Success on the Feel No Pain roll prevents that damage.

The probability of a successful Feel No Pain roll is calculated the same way as other threshold-based rolls:

$$p_{\text{FNP}} = \frac{n - (k - 1)}{n}$$

where
- $p_{\text{FNP}}$ is the probability of a successful Feel No Pain roll
- $n$ is 6 (for a standard die)
- $k$ is the Feel No Pain threshold

To calculate the expected damage after Feel No Pain, we multiply the base damage by the failure rate:

$$\text{Adjusted Damage} = \text{Base Damage} \times (1 - p_{\text{FNP}})$$

For example, if a weapon does 10 damage and the defender has a 5+ Feel No Pain (probability $\frac{2}{6} = 0.3333$), the expected damage after Feel No Pain is:

$$10 \times (1 - 0.3333) = 6.67$$

## Distribution Types
In addition to the standard binomial distribution, the library supports two other distribution types that are useful for analyzing combat scenarios.

### Cumulative Distribution
The cumulative distribution function (CDF) shows the probability of getting *at most* $k$ successes, expressed as $P(X \leq k)$. This is calculated by summing the probability mass function from 0 to $k$:

$$F(n,k,p) = P(X \leq k) = \displaystyle\sum_{i=0}^k \binom{n}{i} p^i (1-p)^{n-i}$$

The cumulative distribution is useful for determining the likelihood of not exceeding a certain number of successes.

### Survivor Distribution
The survivor distribution (also called the complementary cumulative distribution) shows the probability of getting *at least* $k$ successes, expressed as $P(X \geq k)$. This is calculated by summing the probability mass function from $k$ to $n$:

$$S(n,k,p) = P(X \geq k) = \displaystyle\sum_{i=k}^n \binom{n}{i} p^i (1-p)^{n-i}$$

Equivalently, it can be calculated as:

$$S(n,k,p) = 1 - P(X < k) = 1 - F(n,k-1,p)$$

The survivor distribution is particularly useful in combat scenarios for determining the likelihood of achieving at least a certain number of hits, wounds, or destroyed models.

## Calculating Destroyed Models
The ultimate goal of combat calculations is often to determine how many enemy models will be destroyed. This calculation combines all previous factors: hit rolls, wound rolls, save rolls, damage per attack, and Feel No Pain.

The library calculates destroyed models by first determining the total damage dealt, then dividing by a damage threshold. The damage threshold is determined as:

$$\text{Damage Threshold} = \max(\text{Wounds per Model}, \text{Damage per Attack})$$

Then the expected number of destroyed models is:

$$\text{Models Destroyed} = \frac{\text{Total Damage}}{\text{Damage Threshold}}$$

where
- Total Damage is the total damage dealt (number of successful attacks × damage per attack) after Feel No Pain is applied
- Wounds per Model is the defender's Wounds characteristic
- Damage per Attack is the attacker's damage per successful attack

This approach accounts for scenarios where:
- High damage weapons (damage per attack > wounds per model): Uses the higher damage value as threshold
- Low damage weapons (wounds per model > damage per attack): Uses the higher wounds value as threshold

For example, if an attack sequence has 4 successful attacks, each dealing 2 damage, against a defender with 3 Wounds per model:

$$\text{Total Damage} = 4 \times 2 = 8$$
$$\text{Damage Threshold} = \max(3, 2) = 3$$
$$\text{Models Destroyed} = \frac{8}{3} = 2.67$$

This means approximately 2-3 models would be destroyed, with the fractional part representing partial damage to a model.