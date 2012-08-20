using System;
using System.Collections.Generic;
using System.Linq;

using TopCoder.Analysis.Data;

namespace TopCoder.Tools.TcAlgorithmRunner.Algorithm
{
    public class RoundStatisticsAlgorithm
    {
        public void Run()
        {
            using (var db = new TcAnalysisDataModel())
            {
                var coders = new Dictionary<int, int>();
                var rounds = db.Rounds.OrderBy(r => r.Date).ToList();
                foreach (var round in rounds)
                {
                    round.NewRatingsDiffSum = 0;

                    foreach (var div in new[] { 1, 2 })
                    {
                        var ratedDivRoundResults = round.RoundResults.Where(x => x.Division == div && x.IsRated).ToList();
                        if (!ratedDivRoundResults.Any()) { continue; }

                        var divN = ratedDivRoundResults.Count;
                        var divRatings = ratedDivRoundResults.Select(x => x.OldRating).ToList();
                        var divMean = divRatings.Average();
                        var divStdev = Math.Sqrt(divRatings.Sum(r => Math.Pow(r - divMean, 2)) / divN);

                        var divRatingsDiffMean = ratedDivRoundResults.Average(x => Math.Abs(x.NewRating - x.Tc_NewRating));
                        var divVolatilityDiffMean = ratedDivRoundResults.Average(x => Math.Abs(x.NewVolatility - x.Tc_NewVolatility));

                        var divNewRatingsDiffSum = ratedDivRoundResults.Sum(x => x.NewRating - x.OldRating);
                        round.NewRatingsDiffSum += divNewRatingsDiffSum;
                        
                        if (div == 1)
                        {
                            round.DivOneCodersCount = divN;
                            round.DivOneRatingsMean = divMean;
                            round.DivOneRatingsDeviation = divStdev;
                            round.DivOneTcRatingsDiffMean = divRatingsDiffMean;
                            round.DivOneTcVolatilityDiffMean = divVolatilityDiffMean;
                        }
                        if (div == 2)
                        {
                            round.DivTwoCodersCount = divN;
                            round.DivTwoRatingsMean = divMean;
                            round.DivTwoRatingsDeviation = divStdev;
                            round.DivTwoTcRatingsDiffMean = divRatingsDiffMean;
                            round.DivTwoTcVolatilityDiffMean = divVolatilityDiffMean;
                        }

                        foreach (var rr in ratedDivRoundResults)
                        {
                            coders[rr.CoderId] = rr.NewRating;
                        }
                    }


                    var ratings = coders.Values;
                    var n = ratings.Count;
                    var mean = ratings.Average();
                    var stdev = Math.Sqrt(ratings.Sum(r => Math.Pow(r - mean, 2)) / n);

                    round.GlobalCodersCount = n;
                    round.GlobalRatingsMean = mean;
                    round.GlobalRatingsDeviation = stdev;

                    db.SaveChanges();

                    Console.WriteLine(round.Name);
                }
            }
        }
    }
}