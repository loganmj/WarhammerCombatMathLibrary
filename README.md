# Warhammer 40k Combat Library
This project combines statistics mathematics with the 10th Edition rules for Warhammer 40k to provide the ability to calculate the probability of the outcome of combat at each step of the combat process.

# Arbitrarily Large Numbers
Factorials are able to become very large very quickly. Therefore, as of version 3.1.0, the BigInteger type is used to perform calculations when factorials are involved.
It is not recommended to use versions before 3.1.0, as they will either throw errors or produce incorrect results for calculations with trials above 12.

# Math Document
Please see the Math.md file in the Docs folder of the repository for an in-depth explanation of how the calculations are done.
