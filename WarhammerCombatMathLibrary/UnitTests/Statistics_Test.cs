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
        #region Unit Tests - ProbabilityOfSuccess()

        /// <summary>
        /// Tests the case where the numberOfPossibleResults argument is out of range.
        /// </summary>
        [TestMethod]
        public void ProbabilityOfSuccess_NumberOfPossibleResultsLessThanOrEqualTo0()
        {
            Assert.AreEqual(0, Statistics.GetProbabilityOfSuccess(-1, 0));
        }

        /// <summary>
        /// Tests the case where the numberOfSuccessfulResults argument is out of range.
        /// </summary>
        [TestMethod]
        public void ProbabilityOfSuccess_NumberOfSuccessfulResultsLessThanOrEqualTo0()
        {
            Assert.AreEqual(0, Statistics.GetProbabilityOfSuccess(1, -1));
        }

        /// <summary>
        /// Tests the case where the numberOfSuccessfulResults argument is greater than the numberOfPossibleResults argument.
        /// </summary>
        [TestMethod]
        public void ProbabilityOfSuccess_SuccessfulResultsGreaterThanPossibleResults()
        {
            Assert.AreEqual(1, Statistics.GetProbabilityOfSuccess(1, 2));
        }

        /// <summary>
        /// Tests the ProbabilityOfSuccess() method with given parameters.
        /// </summary>
        [TestMethod]
        public void ProbabilityOfSuccess_TestParams1()
        {
            Assert.AreEqual(0.5, Statistics.GetProbabilityOfSuccess(2, 1));
        }

        /// <summary>
        /// Tests the ProbabilityOfSuccess() method with given parameters.
        /// </summary>
        [TestMethod]
        public void ProbabilityOfSuccess_TestParams2()
        {
            Assert.AreEqual(0.25, Statistics.GetProbabilityOfSuccess(4, 1));
        }

        /// <summary>
        /// Tests the ProbabilityOfSuccess() method with given parameters.
        /// </summary>
        [TestMethod]
        public void ProbabilityOfSuccess_TestParams3()
        {
            Assert.AreEqual(0.7, Statistics.GetProbabilityOfSuccess(10, 7));
        }

        #endregion

        #region Unit Tests - AverageResult()

        /// <summary>
        /// Tests the AverageResult() method when the input parameter is less than or equal to 0.
        /// </summary>
        [TestMethod]
        public void AverageResult_PossibleResultsLessThanOrEqualTo0()
        {
            var expected = 0;
            var actual = Statistics.GetMeanResult(0);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the AverageResult() method with given params.
        /// </summary>
        [TestMethod]
        public void AverageResult_TestParams1()
        {
            var expected = 2;
            var numberOfResults = 3;
            var actual = Statistics.GetMeanResult(numberOfResults);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the AverageResult() method with given params.
        /// </summary>
        [TestMethod]
        public void AverageResult_TestParams2()
        {
            var expected = 4;
            var numberOfResults = 6;
            var actual = Statistics.GetMeanResult(numberOfResults);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the AverageResult() method with given params.
        /// </summary>
        [TestMethod]
        public void AverageResult_TestParams3()
        {
            var expected = 6;
            var numberOfResults = 10;
            var actual = Statistics.GetMeanResult(numberOfResults);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Unit Tests - Mean()

        /// <summary>
        /// Tests the case where the number of trials is less than 1.
        /// </summary>
        [TestMethod]
        public void Mean_NumberOfTrialsLessThan1()
        {
            Assert.AreEqual(0, Statistics.GetMeanOfDistribution(0, 1));
        }

        /// <summary>
        /// Tests the case where the probability is less than 0.
        /// </summary>
        [TestMethod]
        public void Mean_ProbabilityLessThan0()
        {
            Assert.AreEqual(0, Statistics.GetMeanOfDistribution(1, -1));
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void Mean_TestParams1()
        {
            Assert.AreEqual(0.5, Statistics.GetMeanOfDistribution(1, 0.5));
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void Mean_TestParams2()
        {
            Assert.AreEqual(5, Statistics.GetMeanOfDistribution(10, 0.5));
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void Mean_TestParams3()
        {
            Assert.AreEqual(25, Statistics.GetMeanOfDistribution(100, 0.25));
        }

        #endregion

        #region Unit Tests - StandardDeviation()

        /// <summary>
        /// Tests the case where the number of trials is less than 1.
        /// </summary>
        [TestMethod]
        public void StandardDeviation_NumberOfTrialsLessThan1()
        {
            Assert.AreEqual(0, Statistics.GetStandardDeviationOfDistribution(0, 1));
        }

        /// <summary>
        /// Tests the case where the probability is less than 0.
        /// </summary>
        [TestMethod]
        public void StandardDeviation_ProbabilityLessThan0()
        {
            Assert.AreEqual(0, Statistics.GetStandardDeviationOfDistribution(1, -1));
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void StandardDeviation_TestParams1()
        {
            Assert.AreEqual(0.5, Statistics.GetMeanOfDistribution(1, 0.5));
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void StandardDeviation_TestParams2()
        {
            Assert.AreEqual(1.58, Math.Round(Statistics.GetStandardDeviationOfDistribution(10, 0.5), 2));
        }

        /// <summary>
        /// Tests the method with given parameters.
        /// </summary>
        [TestMethod]
        public void StandardDeviation_TestParams3()
        {
            Assert.AreEqual(4.33, Math.Round(Statistics.GetStandardDeviationOfDistribution(100, 0.25), 2));
        }

        #endregion

        #region Unit Tests - BinomialCoefficient()

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

        #endregion

        #region Unit Tests - ProbabilityOfMultipleSuccesses()

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

        #endregion

        #region Unit Tests - ProbabilityMassFunction()

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

        #endregion

        #region Unit Tests - BinomialDistribution()

        /// <summary>
        /// Tests the case where the numberOfTrials argument is out of range.
        /// </summary>
        [TestMethod]
        public void BinomialDistribution_NumberOfTrialsLessThan1()
        {
            var expected = new List<BinomialOutcome>() { new(0, 1) };
            var actual = Statistics.GetBinomialDistribution(0, 0.5);

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
        /// Tests the case for the variable trials override where the minNumberOfTrials argument is out of range.
        /// </summary>
        [TestMethod]
        public void BinomialDistribution_VariableTrials_MinNumberOfTrialsLessThan1()
        {
            var expected = new List<BinomialOutcome>() { new(0, 1) };
            var actual = Statistics.GetBinomialDistribution(0, 1, 0.5);

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
        /// Tests the case for the variable trials override where the maxNumberOfTrials argument is out of range.
        /// </summary>
        [TestMethod]
        public void BinomialDistribution_VariableTrials_MaxNumberOfTrialsLessThan1()
        {
            var expected = new List<BinomialOutcome>() { new(0, 1) };
            var actual = Statistics.GetBinomialDistribution(1, 0, 0.5);

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
            var actual = Statistics.GetBinomialDistribution(3, -1);

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

            var actual = Statistics.GetBinomialDistribution(3, 1);

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
        public void BinomialDistribution_GroupSuccessCountLessThan1()
        {
            var expected = new List<BinomialOutcome> { new BinomialOutcome(0, 1) };
            var actual = Statistics.GetBinomialDistribution(3, 0.5, 0);

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
        public void BinomialDistribution_GroupSuccessCountGreaterThanNumberOfTrials()
        {
            var expected = new List<BinomialOutcome> { new BinomialOutcome(0, 1) };
            var actual = Statistics.GetBinomialDistribution(3, 0.5, 5);

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
        /// Tests the method with given large inputs.
        /// </summary>
        [TestMethod]
        public void BinomialDistribution_VariableTrials()
        {
            var minNumberOfTrials = 1;
            var maxNumberOfTrials = 3;
            var probability = 0.5;
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.2917),
                new(1, 0.4583),
                new(2, 0.2083),
                new(3, 0.0417)
            };

            var actual = Statistics.GetBinomialDistribution(minNumberOfTrials, maxNumberOfTrials, probability);

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

        #endregion

        #region Unit Tests - LowerCumulativeDistribution

        /// <summary>
        /// Tests the case where the numberOfTrials argument is out of range.
        /// </summary>
        [TestMethod]
        public void LowerCumulativeDistribution_NumberOfTrialsLessThan1()
        {
            var expected = new List<BinomialOutcome>() { new(0, 1) };
            var actual = Statistics.GetCumulativeDistribution(0, 0.5);

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
        /// Tests the case for the variable trials override where the minNumberOfTrials argument is out of range.
        /// </summary>
        [TestMethod]
        public void LowerCumulativeDistribution_VariableTrials_MinNumberOfTrialsLessThan1()
        {
            var expected = new List<BinomialOutcome>() { new(0, 1) };
            var actual = Statistics.GetCumulativeDistribution(0, 1, 0.5);

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
        /// Tests the case for the variable trials override where the maxNumberOfTrials argument is out of range.
        /// </summary>
        [TestMethod]
        public void LowerCumulativeDistribution_VariableTrials_MaxNumberOfTrialsLessThan1()
        {
            var expected = new List<BinomialOutcome>() { new(0, 1) };
            var actual = Statistics.GetCumulativeDistribution(1, 0, 0.5);

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
            var actual = Statistics.GetCumulativeDistribution(3, -1);

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

            var actual = Statistics.GetCumulativeDistribution(3, 1);

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
        public void LowerCumulativeDistribution_GroupSuccessCountLessThan1()
        {
            var expected = new List<BinomialOutcome> { new BinomialOutcome(0, 1) };
            var actual = Statistics.GetCumulativeDistribution(3, 0.5, 0);

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
        public void LowerCumulativeDistribution_GroupSuccessCountGreaterThanNumberOfTrials()
        {
            var expected = new List<BinomialOutcome> { new BinomialOutcome(0, 1) };
            var actual = Statistics.GetCumulativeDistribution(3, 0.5, 5);

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
        /// Tests the method with given large inputs.
        /// </summary>
        [TestMethod]
        public void CumulativeDistribution_VariableTrials()
        {
            var minNumberOfTrials = 1;
            var maxNumberOfTrials = 3;
            var probability = 0.5;
            var expected = new List<BinomialOutcome>()
            {
                new(0, 0.2917),
                new(1, 0.7500),
                new(2, 0.9583),
                new(3, 1)
            };

            var actual = Statistics.GetCumulativeDistribution(minNumberOfTrials, maxNumberOfTrials, probability);

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

        #endregion

        #region Unit Tests - SurvivorDistribution

        /// <summary>
        /// Tests the case where the numberOfTrials argument is out of range.
        /// </summary>
        [TestMethod]
        public void SurvivorDistribution_NumberOfTrialsLessThan1()
        {
            var expected = new List<BinomialOutcome>() { new(0, 1) };
            var actual = Statistics.GetSurvivorDistribution(0, 0.5);

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
        /// Tests the case for the variable trials override where the minNumberOfTrials argument is out of range.
        /// </summary>
        [TestMethod]
        public void SurvivorDistribution_VariableTrials_MinNumberOfTrialsLessThan1()
        {
            var expected = new List<BinomialOutcome>() { new(0, 1) };
            var actual = Statistics.GetSurvivorDistribution(0, 1, 0.5);

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
        /// Tests the case for the variable trials override where the maxNumberOfTrials argument is out of range.
        /// </summary>
        [TestMethod]
        public void SurvivorDistribution_VariableTrials_MaxNumberOfTrialsLessThan1()
        {
            var expected = new List<BinomialOutcome>() { new(0, 1) };
            var actual = Statistics.GetSurvivorDistribution(1, 0, 0.5);

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
        public void SurvivorDistribution_ProbabilityLessThanOrEqualTo0()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0),
                new(2, 0),
                new(3, 0)
            };

            var actual = Statistics.GetSurvivorDistribution(3, -1);

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
        public void SurvivorDistribution_ProbabilityGreaterThanOrEqualTo1()
        {
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 1),
                new(2, 1),
                new(3, 1)
            };

            var actual = Statistics.GetSurvivorDistribution(3, 1);

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
        public void SurvivorDistribution_GroupSuccessCountLessThan1()
        {
            var expected = new List<BinomialOutcome> { new BinomialOutcome(0, 1) };
            var actual = Statistics.GetSurvivorDistribution(3, 0.5, 0);

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
        public void SurvivorDistribution_GroupSuccessCountGreaterThanNumberOfTrials()
        {
            var expected = new List<BinomialOutcome> { new BinomialOutcome(0, 1) };
            var actual = Statistics.GetSurvivorDistribution(3, 0.5, 5);

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
        /// Tests the case where the number of trials is variable.
        /// </summary>
        [TestMethod]
        public void SurvivorDistribution_VariableTrials()
        {
            var minNumberOfTrials = 1;
            var maxNumberOfTrials = 3;
            var probability = 0.5;
            var expected = new List<BinomialOutcome>()
            {
                new(0, 1),
                new(1, 0.7083),
                new(2, 0.2500),
                new(3, 0.0417)
            };

            var actual = Statistics.GetSurvivorDistribution(minNumberOfTrials, maxNumberOfTrials, probability);

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

        #endregion
    }
}
