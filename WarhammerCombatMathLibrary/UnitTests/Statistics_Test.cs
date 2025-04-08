using System.Diagnostics;
using System.Numerics;
using WarhammerCombatMathLibrary;
using WarhammerCombatMathLibrary.Data;

namespace UnitTests
{
    /// <summary>
    /// Tests the Statistics class.
    /// </summary>
    [TestClass]
    public sealed class Statistics_Test
    {
        /// <summary>
        /// Tests the case where the numberOfPossibleResults argument is out of range.
        /// </summary>
        [TestMethod]
        public void ProbabilityOfSuccess_NumberOfPossibleResultsLessThanOrEqualTo0()
        {
            Assert.AreEqual(0, Statistics.ProbabilityOfSuccess(-1, 0));
        }

        /// <summary>
        /// Tests the case where the numberOfSuccessfulResults argument is out of range.
        /// </summary>
        [TestMethod]
        public void ProbabilityOfSuccess_NumberOfSuccessfulResultsLessThanOrEqualTo0()
        {
            Assert.AreEqual(0, Statistics.ProbabilityOfSuccess(1, -1));
        }

        /// <summary>
        /// Tests the case where the numberOfSuccessfulResults argument is greater than the numberOfPossibleResults argument.
        /// </summary>
        [TestMethod]
        public void ProbabilityOfSuccess_SuccessfulResultsGreaterThanPossibleResults()
        {
            Assert.AreEqual(1, Statistics.ProbabilityOfSuccess(1, 2));
        }

        /// <summary>
        /// Tests the ProbabilityOfSuccess() method with given parameters.
        /// </summary>
        [TestMethod]
        public void ProbabilityOfSuccess_TestParams1()
        {
            Assert.AreEqual(0.5, Statistics.ProbabilityOfSuccess(2, 1));
        }

        /// <summary>
        /// Tests the ProbabilityOfSuccess() method with given parameters.
        /// </summary>
        [TestMethod]
        public void ProbabilityOfSuccess_TestParams2()
        {
            Assert.AreEqual(0.25, Statistics.ProbabilityOfSuccess(4, 1));
        }

        /// <summary>
        /// Tests the ProbabilityOfSuccess() method with given parameters.
        /// </summary>
        [TestMethod]
        public void ProbabilityOfSuccess_TestParams3()
        {
            Assert.AreEqual(0.7, Statistics.ProbabilityOfSuccess(10, 7));
        }

        /// <summary>
        /// Tests the case where the number of trials is less than 1.
        /// </summary>
        [TestMethod]
        public void GetMean_NumberOfTrialsLessThan1()
        {
            Assert.AreEqual(0, Statistics.GetMean(0, 1));
        }

        /// <summary>
        /// Tests the case where the probability is less than 0.
        /// </summary>
        [TestMethod]
        public void GetMean_ProbabilityLessThan0()
        {
            Assert.AreEqual(0, Statistics.GetMean(1, -1));
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMean_TestParams1()
        {
            Assert.AreEqual(0.5, Statistics.GetMean(1, 0.5));
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMean_TestParams2()
        {
            Assert.AreEqual(5, Statistics.GetMean(10, 0.5));
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetMean_TestParams3()
        {
            Assert.AreEqual(25, Statistics.GetMean(100, 0.25));
        }

        /// <summary>
        /// Tests the case where the number of trials is less than 1.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviation_NumberOfTrialsLessThan1()
        {
            Assert.AreEqual(0, Statistics.GetStandardDeviation(0, 1));
        }

        /// <summary>
        /// Tests the case where the probability is less than 0.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviation_ProbabilityLessThan0()
        {
            Assert.AreEqual(0, Statistics.GetStandardDeviation(1, -1));
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviation_TestParams1()
        {
            Assert.AreEqual(0.5, Statistics.GetMean(1, 0.5));
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviation_TestParams2()
        {
            Assert.AreEqual(1.58, Math.Round(Statistics.GetStandardDeviation(10, 0.5), 2));
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void GetStandardDeviation_TestParams3()
        {
            Assert.AreEqual(4.33, Math.Round(Statistics.GetStandardDeviation(100, 0.25), 2));
        }

        /// <summary>
        /// Tests the case where the totalPopulation argument is out of range.
        /// </summary>
        [TestMethod]
        public void BinomialCoefficient_PopulationLessThan0()
        {
            Assert.AreEqual(0, Statistics.BinomialCoefficient(-1, 1));
        }

        /// <summary>
        /// Tests the case where the combinationSize argument is out of range.
        /// </summary>
        [TestMethod]
        public void BinomialCoefficient_CombinationSizeLessThan0()
        {
            Assert.AreEqual(0, Statistics.BinomialCoefficient(1, -1));
        }

        /// <summary>
        /// Tests the case where combinationSize is bigger than totalPopulation.
        /// </summary>
        [TestMethod]
        public void BinomialCoefficient_CombinationSizeBiggerThanTotalPopulation()
        {
            Assert.AreEqual(0, Statistics.BinomialCoefficient(1, 2));
        }

        /// <summary>
        /// Tests the probability mass function with given inputs.
        /// </summary>
        [TestMethod]
        public void BinomialCoefficient_TestParams1()
        {
            Assert.AreEqual(1, Statistics.BinomialCoefficient(1, 1));
        }

        /// <summary>
        /// Tests the probability mass function with given inputs.
        /// </summary>
        [TestMethod]
        public void BinomialCoefficient_TestParams2()
        {
            Assert.AreEqual(252, Statistics.BinomialCoefficient(10, 5));
        }

        /// <summary>
        /// Tests the probability mass function with given large inputs.
        /// </summary>
        [TestMethod]
        public void BinomialCoefficient_TestBigParams()
        {
            Assert.AreEqual(BigInteger.Parse("18053528883775"), Statistics.BinomialCoefficient(50, 32));
        }

        /// <summary>
        /// Tests the case where the probability argument is negative.
        /// </summary>
        [TestMethod]
        public void ProbabilityOfMultipleSuccesses_ProbabilityLessThan0()
        {
            Assert.AreEqual(0, Statistics.ProbabilityOfMultipleSuccesses(-1, 1));
        }

        /// <summary>
        /// Tests the case where the probability argument is greater than 1.
        /// </summary>
        [TestMethod]
        public void ProbabilityOfMultipleSuccesses_ProbabilityGreaterThan1()
        {
            Assert.AreEqual(1, Statistics.ProbabilityOfMultipleSuccesses(2, 1));
        }

        /// <summary>
        /// Tests the case where the numberOfTrials argument is out of range.
        /// </summary>
        [TestMethod]
        public void ProbabilityOfMultipleSuccesses_NumberOfSuccessesLessThan0()
        {
            Assert.AreEqual(0, Statistics.ProbabilityOfMultipleSuccesses(0.5, -1));
        }

        /// <summary>
        /// Tests the probability mass function with given large inputs.
        /// </summary>
        [TestMethod]
        public void ProbabilityOfMultipleSuccesses_TestParams1()
        {
            Assert.AreEqual(0.1, Statistics.ProbabilityOfMultipleSuccesses(0.1, 1));
        }

        /// <summary>
        /// Tests the probability mass function with given inputs.
        /// </summary>
        [TestMethod]
        public void ProbabilityOfMultipleSuccesses_TestParams2()
        {
            Assert.AreEqual(0.25, Statistics.ProbabilityOfMultipleSuccesses(0.5, 2));
        }

        /// <summary>
        /// Tests the probability mass function with given inputs.
        /// </summary>
        [TestMethod]
        public void ProbabilityOfMultipleSuccesses_TestParams3()
        {
            Assert.AreEqual(0.59, Math.Round(Statistics.ProbabilityOfMultipleSuccesses(0.9, 5), 2));
        }

        /// <summary>
        /// Tests the case where the numberOfTrials argument is out of range.
        /// </summary>
        [TestMethod]
        public void ProbabilityMassFunction_NumberOfTrialsLessThan1()
        {
            Assert.AreEqual(0, Statistics.ProbabilityMassFunction(0, 1, 0.5));
        }

        /// <summary>
        /// Tests the case where the numberOfSuccesses argument is out of range.
        /// </summary>
        [TestMethod]
        public void ProbabilityMassFunction_NumberOfSuccessesLessThan0()
        {
            Assert.AreEqual(0, Statistics.ProbabilityMassFunction(1, -1, 0.5));
        }

        /// <summary>
        /// Tests the case where the number of successes is greater than the number of trials.
        /// </summary>
        [TestMethod]
        public void ProbabilityMassFunction_NumberOfSuccessesGreaterThanNumberOfTrials() 
        {
            Assert.AreEqual(0, Statistics.ProbabilityMassFunction(1, 2, 0.5));
        }

        /// <summary>
        /// Tests the case where the probability argument is negative.
        /// </summary>
        [TestMethod]
        public void ProbabilityMassFunction_ProbabilityLessThan0()
        {
            Assert.AreEqual(0, Statistics.ProbabilityMassFunction(1, 1, -1));
        }

        /// <summary>
        /// Tests the case where the probability argument is greater than 1.
        /// </summary>
        [TestMethod]
        public void ProbabilityMassFunction_ProbabilityGreaterThan1()
        {
            Assert.AreEqual(1, Statistics.ProbabilityMassFunction(1, 1, 2));
        }

        /// <summary>
        /// Tests the probability mass function with given inputs.
        /// </summary>
        [TestMethod]
        public void ProbabilityMassFunction_TestParams1()
        {
            Assert.AreEqual(0.5, Statistics.ProbabilityMassFunction(1, 1, 0.5));
        }

        /// <summary>
        /// Tests the probability mass function with given inputs.
        /// </summary>
        [TestMethod]
        public void ProbabilityMassFunction_TestParams2()
        {
            Assert.AreEqual(0.0584, Math.Round(Statistics.ProbabilityMassFunction(10, 5, 0.25), 4));
        }

        /// <summary>
        /// Tests the probability mass function with given large inputs.
        /// </summary>
        [TestMethod]
        public void ProbabilityMassFunction_BigParams()
        {
            Assert.AreEqual(0.0160, Math.Round(Statistics.ProbabilityMassFunction(50, 32, 0.5), 4));
        }

        /// <summary>
        /// Tests the case where the numberOfTrials argument is out of range.
        /// </summary>
        [TestMethod]
        public void BinomialDistribution_NumberOfTrialsLessThan1()
        {
            var expected = new List<BinomialOutcome>() { new(0, 1) };
            var actual = Statistics.BinomialDistribution(0, 0.5);

            // Print expected
            Debug.WriteLine($"Expected: ");
            foreach (var value in expected)
            {
                Debug.WriteLine(value);
            }

            // Print actual
            Debug.WriteLine($"Actual: ");
            foreach (var value in actual)
            {
                Debug.WriteLine(value);
            }

            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the probability argument is negative.
        /// </summary>
        [TestMethod]
        public void BinomialDistribution_ProbabilityLessThanOrEqualTo0()
        {
            var expected = new List<BinomialOutcome>() 
            {
                new(0, 1),
                new(1,0),
                new(2,0),
                new(3,0)
            };
            var actual = Statistics.BinomialDistribution(3, -1);

            // Print expected
            Debug.WriteLine($"Expected: ");
            foreach (var value in expected)
            {
                Debug.WriteLine(value);
            }

            // Print actual
            Debug.WriteLine($"Actual: ");
            foreach (var value in actual)
            {
                Debug.WriteLine(value);
            }

            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the probability argument is greater than 1.
        /// </summary>
        [TestMethod]
        public void BinomialDistribution_ProbabilityGreaterThanOrEqualTo1()
        {
            var expected = new List<BinomialOutcome>() 
            {
                new(0, 0),
                new(1, 0),
                new(2, 0),
                new(3, 1) 
            };

            var actual = Statistics.BinomialDistribution(3, 1);

            // Print expected
            Debug.WriteLine($"Expected: ");
            foreach (var value in expected)
            {
                Debug.WriteLine(value);
            }

            // Print actual
            Debug.WriteLine($"Actual: ");
            foreach (var value in actual)
            {
                Debug.WriteLine(value);
            }

            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the probability mass function with given inputs.
        /// </summary>
        [TestMethod]
        public void BinomialDistribution_TestParams1()
        {
            var numberOfTrials = 1;
            var probability = 0.5;
            var expected = new List<BinomialOutcome>() 
            {
                new(0, 0.5),
                new(1, 0.5) 
            };

            var actual = Statistics.BinomialDistribution(numberOfTrials, probability);

            // Print expected
            Debug.WriteLine($"Expected: ");
            foreach (var value in expected)
            {
                Debug.WriteLine(value);
            }

            // Print actual
            Debug.WriteLine($"Actual: ");
            foreach (var value in actual)
            {
                Debug.WriteLine(value);
            }

            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the probability mass function with given inputs.
        /// </summary>
        [TestMethod]
        public void BinomialDistribution_TestParams2()
        {
            var numberOfTrials = 1;
            var probability = 0.1;
            var expected = new List<BinomialOutcome>() 
            { 
                new(0, 0.9), 
                new(1, 0.1)
            };

            var actual = Statistics.BinomialDistribution(numberOfTrials, probability);

            // Print expected
            Debug.WriteLine($"Expected: ");
            foreach (var value in expected)
            {
                Debug.WriteLine(value);
            }

            // Print actual
            Debug.WriteLine($"Actual: ");
            foreach (var value in actual)
            {
                Debug.WriteLine(value);
            }

            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the probability mass function with given large inputs.
        /// </summary>
        [TestMethod]
        public void BinomialDistribution_TestParams3()
        {
            var numberOfTrials = 2;
            var probability = 0.5;
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.25),
                new(1, 0.5),
                new(2, 0.25)
            };

            var actual = Statistics.BinomialDistribution(numberOfTrials, probability);

            // Print expected
            Debug.WriteLine($"Expected: ");
            foreach (var value in expected)
            {
                Debug.WriteLine(value);
            }

            // Print actual
            Debug.WriteLine($"Actual: ");
            foreach (var value in actual)
            {
                Debug.WriteLine(value);
            }

            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the number of trials is less than 1.
        /// </summary>
        [TestMethod]
        public void LowerCumulativeProbability_NumberOfTrialsLessThan1() 
        {
            Assert.AreEqual(0, Statistics.LowerCumulativeProbability(0, 1, 0.5));
        }

        /// <summary>
        /// Tests the case where the number of successes is less than 0.
        /// </summary>
        [TestMethod]
        public void LowerCumulativeProbability_NumberOfSuccessesLessThan0() 
        {
            Assert.AreEqual(0, Statistics.LowerCumulativeProbability(1, -1, 0.5));
        }

        /// <summary>
        /// Tests the case where the number of successes is greater than the number of trials.
        /// </summary>
        [TestMethod]
        public void LowerCumulativeProbability_NumberOfSuccessesGreaterThanNumberOfTrials()
        {
            Assert.AreEqual(0, Statistics.LowerCumulativeProbability(1, 2, 0.5));
        }

        /// <summary>
        /// Tests the case where the probability is less than or equal to 0.
        /// </summary>
        [TestMethod]
        public void LowerCumulativeProbability_ProbabilityLessThanOrEqualTo0() 
        {
            Assert.AreEqual(0, Statistics.LowerCumulativeProbability(1, 1, -1));
        }

        /// <summary>
        /// Tests the case where probability is greater than or equal to 1, and the number of successes is less than the number of trials.
        /// </summary>
        [TestMethod]
        public void LowerCumulativeProbability_ProbabilityGreaterThanOrEqualTo1_SuccessesLessThanTrials() 
        {
            Assert.AreEqual(0, Statistics.LowerCumulativeProbability(2, 1, 1));
        }

        /// <summary>
        /// Tests the case where probability is greater than or equal to 1, and the number of successes is equal to the number of trials.
        /// </summary>
        [TestMethod]
        public void LowerCumulativeProbability_ProbabilityGreaterThanOrEqualTo1_SuccessesEqualsTrials()
        {
            Assert.AreEqual(1, Statistics.LowerCumulativeProbability(2, 2, 1));
        }

        /// <summary>
        /// Tests the probability mass function with given inputs.
        /// </summary>
        [TestMethod]
        public void LowerCumulativeProbability_TestParams1()
        {
            Assert.AreEqual(1, Statistics.LowerCumulativeProbability(1, 1, 0.5));
        }

        /// <summary>
        /// Tests the probability mass function with given inputs.
        /// </summary>
        [TestMethod]
        public void LowerCumulativeProbability_TestParams2()
        {
            Assert.AreEqual(0.9803, Math.Round(Statistics.LowerCumulativeProbability(10, 5, 0.25), 4));
        }

        /// <summary>
        /// Tests the probability mass function with given large inputs.
        /// </summary>
        [TestMethod]
        public void LowerCumulativeProbability_BigParams()
        {
            Assert.AreEqual(0.9836, Math.Round(Statistics.LowerCumulativeProbability(50, 32, 0.5), 4));
        }

        /// <summary>
        /// Tests the case where the numberOfTrials argument is out of range.
        /// </summary>
        [TestMethod]
        public void LowerCumulativeDistribution_NumberOfTrialsLessThan1()
        {
            var expected = new List<BinomialOutcome>() { new(0, 1) };
            var actual = Statistics.LowerCumulativeDistribution(0, 0.5);

            // Print expected
            Debug.WriteLine($"Expected: ");
            foreach (var value in expected)
            {
                Debug.WriteLine(value);
            }

            // Print actual
            Debug.WriteLine($"Actual: ");
            foreach (var value in actual)
            {
                Debug.WriteLine(value);
            }

            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the probability argument is negative.
        /// </summary>
        [TestMethod]
        public void LowerCumulativeDistribution_ProbabilityLessThanOrEqualTo0()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1,1),
                new(2,1),
                new(3,1)
            };
            var actual = Statistics.LowerCumulativeDistribution(3, -1);

            // Print expected
            Debug.WriteLine($"Expected: ");
            foreach (var value in expected)
            {
                Debug.WriteLine(value);
            }

            // Print actual
            Debug.WriteLine($"Actual: ");
            foreach (var value in actual)
            {
                Debug.WriteLine(value);
            }

            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the probability argument is greater than 1.
        /// </summary>
        [TestMethod]
        public void LowerCumulativeDistribution_ProbabilityGreaterThanOrEqualTo1()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0),
                new(1, 0),
                new(2, 0),
                new(3, 1)
            };

            var actual = Statistics.LowerCumulativeDistribution(3, 1);

            // Print expected
            Debug.WriteLine($"Expected: ");
            foreach (var value in expected)
            {
                Debug.WriteLine(value);
            }

            // Print actual
            Debug.WriteLine($"Actual: ");
            foreach (var value in actual)
            {
                Debug.WriteLine(value);
            }

            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the probability mass function with given inputs.
        /// </summary>
        [TestMethod]
        public void LowerCumulativeDistribution_TestParams1()
        {
            var numberOfTrials = 1;
            var probability = 0.5;
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.5),
                new(1, 1)
            };

            var actual = Statistics.LowerCumulativeDistribution(numberOfTrials, probability);

            // Print expected
            Debug.WriteLine($"Expected: ");
            foreach (var value in expected)
            {
                Debug.WriteLine(value);
            }

            // Print actual
            Debug.WriteLine($"Actual: ");
            foreach (var value in actual)
            {
                Debug.WriteLine(value);
            }

            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the probability mass function with given inputs.
        /// </summary>
        [TestMethod]
        public void LowerCumulativeDistribution_TestParams2()
        {
            var numberOfTrials = 1;
            var probability = 0.1;
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.9),
                new(1, 1.0)
            };

            var actual = Statistics.LowerCumulativeDistribution(numberOfTrials, probability);

            // Print expected
            Debug.WriteLine($"Expected: ");
            foreach (var value in expected)
            {
                Debug.WriteLine(value);
            }

            // Print actual
            Debug.WriteLine($"Actual: ");
            foreach (var value in actual)
            {
                Debug.WriteLine(value);
            }

            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the probability mass function with given large inputs.
        /// </summary>
        [TestMethod]
        public void LowerCumulativeDistribution_TestParams3()
        {
            var numberOfTrials = 2;
            var probability = 0.5;
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.25),
                new(1, 0.75),
                new(2, 1.0)
            };

            var actual = Statistics.LowerCumulativeDistribution(numberOfTrials, probability);

            // Print expected
            Debug.WriteLine($"Expected: ");
            foreach (var value in expected)
            {
                Debug.WriteLine(value);
            }

            // Print actual
            Debug.WriteLine($"Actual: ");
            foreach (var value in actual)
            {
                Debug.WriteLine(value);
            }

            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the number of trials is less than 1.
        /// </summary>
        [TestMethod]
        public void UpperCumulativeProbability_NumberOfTrialsLessThan1()
        {
            Assert.AreEqual(0, Statistics.UpperCumulativeProbability(0, 1, 0.5));
        }

        /// <summary>
        /// Tests the case where the number of successes is less than 0.
        /// </summary>
        [TestMethod]
        public void UpperCumulativeProbability_NumberOfSuccessesLessThan0()
        {
            Assert.AreEqual(0, Statistics.UpperCumulativeProbability(1, -1, 0.5));
        }

        /// <summary>
        /// Tests the case where the number of successes is greater than the number of trials.
        /// </summary>
        [TestMethod]
        public void UpperCumulativeProbability_NumberOfSuccessesGreaterThanNumberOfTrials()
        {
            Assert.AreEqual(0, Statistics.UpperCumulativeProbability(1, 2, 0.5));
        }

        /// <summary>
        /// Tests the case where the probability is less than or equal to 0.
        /// </summary>
        [TestMethod]
        public void UpperCumulativeProbability_ProbabilityLessThanOrEqualTo0_SuccessesGreaterThan0()
        {
            Assert.AreEqual(0, Statistics.UpperCumulativeProbability(1, 1, -1));
        }

        /// <summary>
        /// Tests the case where the probability is less than or equal to 0.
        /// </summary>
        [TestMethod]
        public void UpperCumulativeProbability_ProbabilityLessThanOrEqualTo0_SuccessesEquals0()
        {
            Assert.AreEqual(0, Statistics.UpperCumulativeProbability(1, 0, -1));
        }

        /// <summary>
        /// Tests the case where probability is greater than or equal to 1, and the number of successes is less than the number of trials.
        /// </summary>
        [TestMethod]
        public void UpperCumulativeProbability_ProbabilityGreaterThanOrEqualTo1()
        {
            Assert.AreEqual(1, Statistics.UpperCumulativeProbability(2, 1, 1));
        }

        /// <summary>
        /// Tests the probability mass function with given inputs.
        /// </summary>
        [TestMethod]
        public void UpperCumulativeProbability_TestParams1()
        {
            Assert.AreEqual(0.25, Statistics.UpperCumulativeProbability(2, 1, 0.5));
        }

        /// <summary>
        /// Tests the probability mass function with given inputs.
        /// </summary>
        [TestMethod]
        public void UpperCumulativeProbability_TestParams2()
        {
            Assert.AreEqual(0.0197, Math.Round(Statistics.UpperCumulativeProbability(10, 5, 0.25), 4));
        }

        /// <summary>
        /// Tests the probability mass function with given large inputs.
        /// </summary>
        [TestMethod]
        public void UpperCumulativeProbability_BigParams()
        {
            Assert.AreEqual(0.0164, Math.Round(Statistics.UpperCumulativeProbability(50, 32, 0.5), 4));
        }

        /// <summary>
        /// Tests the case where the numberOfTrials argument is out of range.
        /// </summary>
        [TestMethod]
        public void UpperCumulativeDistribution_NumberOfTrialsLessThan1()
        {
            var expected = new List<BinomialOutcome>() { new(0, 1) };
            var actual = Statistics.UpperCumulativeDistribution(0, 0.5);

            // Print expected
            Debug.WriteLine($"Expected: ");
            foreach (var value in expected)
            {
                Debug.WriteLine(value);
            }

            // Print actual
            Debug.WriteLine($"Actual: ");
            foreach (var value in actual)
            {
                Debug.WriteLine(value);
            }

            CollectionAssert.AreEqual(expected, actual);
        }

        /*

        /// <summary>
        /// Tests the case where the probability argument is negative.
        /// </summary>
        [TestMethod]
        public void LowerCumulativeDistribution_ProbabilityLessThanOrEqualTo0()
        {
            var expected = new List<BinomialData>()
            {
                new(0, 1),
                new(1,1),
                new(2,1),
                new(3,1)
            };
            var actual = Statistics.LowerCumulativeDistribution(3, -1);

            // Print expected
            Debug.WriteLine($"Expected: ");
            foreach (var value in expected)
            {
                Debug.WriteLine(value);
            }

            // Print actual
            Debug.WriteLine($"Actual: ");
            foreach (var value in actual)
            {
                Debug.WriteLine(value);
            }

            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the case where the probability argument is greater than 1.
        /// </summary>
        [TestMethod]
        public void LowerCumulativeDistribution_ProbabilityGreaterThanOrEqualTo1()
        {
            var expected = new List<BinomialData>()
            {
                new(0, 0),
                new(1, 0),
                new(2, 0),
                new(3, 1)
            };

            var actual = Statistics.LowerCumulativeDistribution(3, 1);

            // Print expected
            Debug.WriteLine($"Expected: ");
            foreach (var value in expected)
            {
                Debug.WriteLine(value);
            }

            // Print actual
            Debug.WriteLine($"Actual: ");
            foreach (var value in actual)
            {
                Debug.WriteLine(value);
            }

            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the probability mass function with given inputs.
        /// </summary>
        [TestMethod]
        public void LowerCumulativeDistribution_TestParams1()
        {
            var numberOfTrials = 1;
            var probability = 0.5;
            var expected = new List<BinomialData>()
            {
                new(0, 0.5),
                new(1, 1)
            };

            var actual = Statistics.LowerCumulativeDistribution(numberOfTrials, probability);

            // Print expected
            Debug.WriteLine($"Expected: ");
            foreach (var value in expected)
            {
                Debug.WriteLine(value);
            }

            // Print actual
            Debug.WriteLine($"Actual: ");
            foreach (var value in actual)
            {
                Debug.WriteLine(value);
            }

            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the probability mass function with given inputs.
        /// </summary>
        [TestMethod]
        public void LowerCumulativeDistribution_TestParams2()
        {
            var numberOfTrials = 1;
            var probability = 0.1;
            var expected = new List<BinomialData>()
            {
                new(0, 0.9),
                new(1, 1.0)
            };

            var actual = Statistics.LowerCumulativeDistribution(numberOfTrials, probability);

            // Print expected
            Debug.WriteLine($"Expected: ");
            foreach (var value in expected)
            {
                Debug.WriteLine(value);
            }

            // Print actual
            Debug.WriteLine($"Actual: ");
            foreach (var value in actual)
            {
                Debug.WriteLine(value);
            }

            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the probability mass function with given large inputs.
        /// </summary>
        [TestMethod]
        public void LowerCumulativeDistribution_TestParams3()
        {
            var numberOfTrials = 2;
            var probability = 0.5;
            var expected = new List<BinomialData>()
            {
                new(0, 0.25),
                new(1, 0.75),
                new(2, 1.0)
            };

            var actual = Statistics.LowerCumulativeDistribution(numberOfTrials, probability);

            // Print expected
            Debug.WriteLine($"Expected: ");
            foreach (var value in expected)
            {
                Debug.WriteLine(value);
            }

            // Print actual
            Debug.WriteLine($"Actual: ");
            foreach (var value in actual)
            {
                Debug.WriteLine(value);
            }

            CollectionAssert.AreEqual(expected, actual);
        }

        */
    }
}
