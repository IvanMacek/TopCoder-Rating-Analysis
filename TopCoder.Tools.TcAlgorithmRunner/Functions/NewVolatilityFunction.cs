using System;

namespace TopCoder.Tools.TcAlgorithmRunner.Functions
{
    public class NewVolatilityFunction
    {
        public int Calculate(int oldRating, int oldVolatility, double newRating, double codersCompetitionWeight, int codersNumberOfRatedRounds)
        {
            // TopCoders rule form provisional ratings
            if (codersNumberOfRatedRounds == 0) { return 385; }

            var w = codersCompetitionWeight;
            var newVol = Math.Sqrt(Math.Pow(newRating - oldRating, 2) / w + oldVolatility * oldVolatility / (w + 1));
            return (int)Math.Round(newVol);
        }
    }
}