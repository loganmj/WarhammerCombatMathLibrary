# Variable Attacks
In Warhammer 40k, some weapons have a variable number of attacks determined by dice rolls. A weapon's Attacks characteristic can be expressed as:

$$\text{Attacks} = (X)(Dy) + Z$$

where:
- $X$ is the number of attack dice to roll
- $Dy$ is the dice type (D3 or D6)
- $Z$ is the flat number of attacks

Most weapons have a flat number of attacks and do not have variable attacks. In this case, $X$ can be considered 0, which simplifies the equation to:

$$(0)(Dy) + Z = Z$$

For example:
- A weapon with "A:5" has 5 attacks: $(0)(D6) + 5 = 5$
- A weapon with "A:D6" has variable attacks: $(1)(D6) + 0$, averaging 3.5 attacks
- A weapon with "A:2D6+3" has $(2)(D6) + 3$, averaging $7 + 3 = 10$ attacks

The average number of attacks can be calculated as:

$$\text{Avg Attacks} = (X \times \text{Avg}(Dy)) + Z$$

where $\text{Avg}(D3) = 2$ and $\text{Avg}(D6) = 3.5$

The minimum number of attacks is:

$$\text{Min Attacks} = X + Z$$

The maximum number of attacks is:

$$\text{Max Attacks} = (X \times Dy) + Z$$