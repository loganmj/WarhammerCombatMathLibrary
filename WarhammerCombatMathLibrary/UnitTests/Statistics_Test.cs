﻿using System.Diagnostics;
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
            Assert.AreEqual(0, Statistics.ProbabilityMassFunction(0, 1, 1));
        }

        /// <summary>
        /// Tests the case where the numberOfSuccesses argument is out of range.
        /// </summary>
        [TestMethod]
        public void ProbabilityMassFunction_NumberOfSuccessesLessThan0()
        {
            Assert.AreEqual(0, Statistics.ProbabilityMassFunction(1, -1, 1));
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
            Assert.AreEqual(0.016, Math.Round(Statistics.ProbabilityMassFunction(50, 32, 0.5), 3));
        }

        /// <summary>
        /// Tests the case where the numberOfTrials argument is out of range.
        /// </summary>
        [TestMethod]
        public void BinomialDistribution_NumberOfTrialsLessThan1()
        {
            var expected = new List<BinomialData>() { new(0,1) };
            var actual = Statistics.BinomialDistribution(0, 1);

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
            var expected = new List<BinomialData>() { new(0, 1) };
            var actual = Statistics.BinomialDistribution(1, -1);

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
            var numberOfTrials = 10;
            var expected = new List<BinomialData>() { new(numberOfTrials, 1) };
            var actual = Statistics.BinomialDistribution(numberOfTrials, 2);

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
    }
}
