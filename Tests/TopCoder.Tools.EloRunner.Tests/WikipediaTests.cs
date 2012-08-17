//
// Test examples are taken from Wkipedia ELO article 
// http://en.wikipedia.org/wiki/Elo_rating_system#Mathematical_details
//

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TopCoder.Tools.EloRunner.Tests
{
    [TestClass]
    public class WikipediaTests
    {
        [TestMethod]
        public void Example1()
        {
            const int playerRating = 1613;
            var opposingPlayersRatings = new[] { 1609, 1477, 1388, 1586, 1720 };
            var observedScore = new[] { 0, 0.5, 1, 1, 0 };

            var algorithm = new EloAlgorithm();
            var newPlayerRating = algorithm.CalculateNewRating(playerRating, 32, opposingPlayersRatings, observedScore);

            Assert.AreEqual(1601, newPlayerRating);
        }

        [TestMethod]
        public void Example2()
        {
            const int playerRating = 1613;
            var opposingPlayersRatings = new[] { 1609, 1477, 1388, 1586, 1720 };
            var observedScore = new[] { 1, 1, 0, 0.5, 0.5 };

            var algorithm = new EloAlgorithm();
            var newPlayerRating = algorithm.CalculateNewRating(playerRating, 32, opposingPlayersRatings, observedScore);

            Assert.AreEqual(1617, newPlayerRating);
        }
    }
}