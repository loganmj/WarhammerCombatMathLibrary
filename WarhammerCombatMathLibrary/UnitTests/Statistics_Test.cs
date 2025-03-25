using System.Numerics;
using WarhammerCombatMathLibrary;

namespace UnitTests
{
    [TestClass]
    public sealed class Statistics_Test
    {
        /// <summary>
        /// Tests the case where the numberOfPossibleResults argument is out of range.
        /// </summary>
        [TestMethod]
        public void ProbabilityOfSuccess_ArgumentOutOfRange_NumberOfPossibleResults()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Statistics.ProbabilityOfSuccess(-1, 0));
        }

        /// <summary>
        /// Tests the case where the numberOfSuccessfulResults argument is out of range.
        /// </summary>
        [TestMethod]
        public void ProbabilityOfSuccess_ArgumentOutOfRange_NumberOfSuccessfulResults()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Statistics.ProbabilityOfSuccess(1, -1));
        }

        /// <summary>
        /// Tests the case where the numberOfSuccessfulResults argument is greater than the numberOfPossibleResults argument.
        /// </summary>
        [TestMethod]
        public void ProbabilityOfSuccess_ArgumentException_SuccessfulResultsGreaterThanPossibleResults()
        {
            Assert.ThrowsException<ArgumentException>(() => Statistics.ProbabilityOfSuccess(1, 2));
        }

        /// <summary>
        /// Tests the ProbabilityOfSuccess() method with given parameters.
        /// </summary>
        [TestMethod]
        public void ProbabilityOfSuccess_2_Possible_1_Successful()
        {
            Assert.AreEqual(0.5, Statistics.ProbabilityOfSuccess(2, 1));
        }

        /// <summary>
        /// Tests the ProbabilityOfSuccess() method with given parameters.
        /// </summary>
        [TestMethod]
        public void ProbabilityOfSuccess_4_Possible_1_Successful()
        {
            Assert.AreEqual(0.25, Statistics.ProbabilityOfSuccess(4, 1));
        }

        /// <summary>
        /// Tests the ProbabilityOfSuccess() method with given parameters.
        /// </summary>
        [TestMethod]
        public void ProbabilityOfSuccess_10_Possible_7_Successful()
        {
            Assert.AreEqual(0.7, Statistics.ProbabilityOfSuccess(10, 7));
        }

        /// <summary>
        /// Tests the case where the numberOfTrials argument is out of range.
        /// </summary>
        [TestMethod]
        public void BinomialDistribution_ArgumentOutOfRange_NumberOfTrials()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Statistics.BinomialDistribution(0, 1));
        }

        /// <summary>
        /// Tests the case where the probability argument is negative.
        /// </summary>
        [TestMethod]
        public void BinomialDistribution_ArgumentOutOfRange_ProbabilityIsNegative()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Statistics.BinomialDistribution(1, -1));
        }

        /// <summary>
        /// Tests the case where the probability argument is greater than 1.
        /// </summary>
        [TestMethod]
        public void BinomialDistribution_ArgumentOutOfRange_ProbabilityIsGreaterThan1()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Statistics.BinomialDistribution(1, 2));
        }

        /// <summary>
        /// Tests the case where the numberOfTrials argument is out of range.
        /// </summary>
        [TestMethod]
        public void ProbabilityMassFunction_ArgumentOutOfRange_NumberOfTrials()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Statistics.ProbabilityMassFunction(0, 1, 1));
        }

        /// <summary>
        /// Tests the case where the numberOfSuccesses argument is out of range.
        /// </summary>
        [TestMethod]
        public void ProbabilityMassFunction_ArgumentOutOfRange_NumberOfSuccesses()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Statistics.ProbabilityMassFunction(1, -1, 1));
        }

        /// <summary>
        /// Tests the case where the probability argument is negative.
        /// </summary>
        [TestMethod]
        public void ProbabilityMassFunction_ArgumentOutOfRange_ProbabilityIsNegative()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Statistics.ProbabilityMassFunction(1, 1, -1));
        }

        /// <summary>
        /// Tests the case where the probability argument is greater than 1.
        /// </summary>
        [TestMethod]
        public void ProbabilityMassFunction_ArgumentOutOfRange_ProbabilityIsGreaterThan1()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Statistics.ProbabilityMassFunction(1, 1, 2));
        }

        /// <summary>
        /// Tests the probability mass function with given inputs.
        /// </summary>
        [TestMethod]
        public void ProbabilityMassFunction_TestParams_1_1_05() 
        {
            Assert.AreEqual(0.5, Statistics.ProbabilityMassFunction(1, 1, 0.5));
        }

        /// <summary>
        /// Tests the probability mass function with given inputs.
        /// </summary>
        [TestMethod]
        public void ProbabilityMassFunction_TestParams_10_5_025()
        {
            Assert.AreEqual(0.0584, Math.Round(Statistics.ProbabilityMassFunction(10, 5, 0.25), 4));
        }

        /// <summary>
        /// Tests the probability mass function with given large inputs.
        /// </summary>
        [TestMethod]
        public void ProbabilityMassFunction_BigParams_50_32_05()
        {
            Assert.AreEqual(0.016, Math.Round(Statistics.ProbabilityMassFunction(50, 32, 0.5), 3));
        }

        /// <summary>
        /// Tests the case where the totalPopulation argument is out of range.
        /// </summary>
        [TestMethod]
        public void BinomialCoefficient_ArgumentOurOfRange_TotalPopulation()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Statistics.BinomialCoefficient(-1, 1));
        }

        /// <summary>
        /// Tests the case where the combinationSize argument is out of range.
        /// </summary>
        [TestMethod]
        public void BinomialCoefficient_ArgumentOurOfRange_CombinationSize()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Statistics.BinomialCoefficient(1, -1));
        }

        /// <summary>
        /// Tests the case where combinationSize is bigger than totalPopulation.
        /// </summary>
        [TestMethod]
        public void BinomialCoefficient_ArgumentException_CombinationSizeBiggerThanTotalPopulation()
        {
            Assert.ThrowsException<ArgumentException>(() => Statistics.BinomialCoefficient(1, 2));
        }

        /// <summary>
        /// Tests the probability mass function with given inputs.
        /// </summary>
        [TestMethod]
        public void BinomialCoefficient_TestParams_1_1_05()
        {
            Assert.AreEqual(1, Statistics.BinomialCoefficient(1, 1));
        }

        /// <summary>
        /// Tests the probability mass function with given inputs.
        /// </summary>
        [TestMethod]
        public void BinomialCoefficient_TestParams_10_5_025()
        {
            Assert.AreEqual(252, Statistics.BinomialCoefficient(10, 5));
        }

        /// <summary>
        /// Tests the probability mass function with given large inputs.
        /// </summary>
        [TestMethod]
        public void BinomialCoefficient_TestBigParams_50_32_05()
        {
            Assert.AreEqual(BigInteger.Parse("18053528883775"), Statistics.BinomialCoefficient(50, 32));
        }

        /// <summary>
        /// Tests the case where the probability argument is negative.
        /// </summary>
        [TestMethod]
        public void ProbabilityOfMultipleSuccesses_ArgumentOutOfRange_ProbabilityIsNegative()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Statistics.ProbabilityOfMultipleSuccesses(-1, 1));
        }

        /// <summary>
        /// Tests the case where the probability argument is greater than 1.
        /// </summary>
        [TestMethod]
        public void ProbabilityOfMultipleSuccesses_ArgumentOutOfRange_ProbabilityIsGreaterThan1() 
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Statistics.ProbabilityOfMultipleSuccesses(2, 1));
        }

        /// <summary>
        /// Tests the case where the numberOfTrials argument is out of range.
        /// </summary>
        [TestMethod]
        public void ProbabilityOfMultipleSuccesses_ArgumentOutOfRange_NumberOfTrials()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Statistics.ProbabilityOfMultipleSuccesses(1, -1));
        }
    }
}
