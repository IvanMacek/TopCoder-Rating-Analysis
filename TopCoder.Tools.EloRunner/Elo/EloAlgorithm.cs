using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace TopCoder.Tools.EloRunner.Elo
{
    public partial class EloAlgorithm
    {
        public int CalculateNewRating(int playerRating, int playerKFactor, int[] opposingPlayersRatings, double[] observedScoresVersusOpposingPlayers)
        {
            Contract.Requires(opposingPlayersRatings != null);
            Contract.Requires(observedScoresVersusOpposingPlayers != null);
            Contract.Requires(opposingPlayersRatings.Length == observedScoresVersusOpposingPlayers.Length);

            Contract.Requires(0 <= playerRating);                                               // All ratings must be in [0, +inf> range
            Contract.Requires(0 <= playerKFactor);                                              // K-factor must be be in [0, +inf> range
            Contract.Requires(Contract.ForAll(opposingPlayersRatings, x => 0 <= x));                        // All ratings must be in [0, +inf> range
            Contract.Requires(Contract.ForAll(observedScoresVersusOpposingPlayers, x => 0 <= x && x <= 1));     // All scores must be in [0, 1] range

            var expectedScore = opposingPlayersRatings.Sum(x => _ExpectedScoreRule(playerRating, x));
            var observedScore = observedScoresVersusOpposingPlayers.Sum();

            var newRating = _UpdateRatingRule(playerRating, playerKFactor, expectedScore, observedScore);
            return newRating;
        }

        private static int _UpdateRatingRule(int playerRating, int playerKFactor, double expectedScore, double observerScore)
        {
            var newPlayerRating = playerRating + playerKFactor*(observerScore - expectedScore);
            return (int)Math.Round(newPlayerRating);
        }

        private static double _ExpectedScoreRule(int playerRating, int opposingPlayerRating)
        {
            var qa = Math.Pow(10, playerRating / _ScaleConstant);
            var qb = Math.Pow(10, opposingPlayerRating / _ScaleConstant);
            var qsum = qa + qb;

            var ea = qa / qsum;
            var eb = qb / qsum;

            Contract.Assert(Math.Abs(ea + eb - 1.0) < _DoubleEqualityEpsilon);  // Chect to make sure that ea + eb == 1

            return ea;
        }
    }

    public partial class EloAlgorithm
    {
        private const double _DoubleEqualityEpsilon = 0.000000000001;
        private const double _ScaleConstant = 400.0;

        public EloAlgorithm()
        {
        }
    }
}