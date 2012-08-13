//
// Test examples are taken from Chess ELO web page
// http://www.chesselo.com/
//

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TopCoder.Tools.EloRunner.Elo;

namespace TopCoder.Tools.EloRunner.Tests
{
    [TestClass]
    public class ChessEloTests
    {
        // Example 1 with K-factor of 10: Player A rated 2000, played against Player B rated 1900 and defeated him. The Rating Change for player A is therefore calculated as this (Result is 1, Expected Result 0.64):
        // Rating Change = K-factor * ( Result – Expected Result )
        // Rating Change = 10 * ( 1 – 0.64) = 10 * 0.36 = 3.6
        [TestMethod]
        public void Page1_Example1()
        {
            const int playerRating = 2000;
            var opposingPlayersRatings = new[] { 1900 };
            var observedScore = new[] { 1.0 };

            var algorithm = new EloAlgorithm();
            var newPlayerRating = algorithm.CalculateNewRating(playerRating, 10, opposingPlayersRatings, observedScore);

            Assert.AreEqual(Math.Round(2000 + 3.6), newPlayerRating);
        }


        // Example 2 with K-factor of 10: Player A rated 2000, played against Player B rated 1900 and lost. The result for player A is therefore calculated as this (Result is 0, Expected Result 0.64):
        // Rating Change = K-factor * ( Result – Expected Result )
        // Rating Change = 10 * ( 0 – 0.64) = 10 * (- 0.64) = - 6.4
        [TestMethod]
        public void Page1_Example2()
        {
            const int playerRating = 2000;
            var opposingPlayersRatings = new[] { 1900 };
            var observedScore = new[] { 0.0 };

            var algorithm = new EloAlgorithm();
            var newPlayerRating = algorithm.CalculateNewRating(playerRating, 10, opposingPlayersRatings, observedScore);

            Assert.AreEqual(Math.Round(2000 - 6.4), newPlayerRating);
        }


        // Example 3 with K-factor of 10: Player A rated 2000, played against Player B rated 1900 and made a draw. The result for player A is therefore calculated as this (Result is 0.5, Expected Result 0.64):
        // Rating Change = K-factor * ( Result – Expected Result )
        // Rating Change = 10 * ( 0.5 – 0.64) = 10 * (- 0.14) = - 1.4
        [TestMethod]
        public void Page1_Example3()
        {
            const int playerRating = 2000;
            var opposingPlayersRatings = new[] { 1900 };
            var observedScore = new[] { 0.5 };

            var algorithm = new EloAlgorithm();
            var newPlayerRating = algorithm.CalculateNewRating(playerRating, 10, opposingPlayersRatings, observedScore);

            Assert.AreEqual(Math.Round(2000 - 1.4), newPlayerRating);
        }
    }
}