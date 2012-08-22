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

                        var kendalTauDist = 
                            (from xi in ratedDivRoundResults.Select((x, i) => new { val = x, idx = i })
                             from xj in ratedDivRoundResults.Select((x, i) => new { val = x, idx = i })
                             where xi.idx < xj.idx
                             where (xi.val.Tc_ActualRank < xj.val.Tc_ActualRank && xi.val.Tc_ExpectedRank > xj.val.Tc_ExpectedRank
                                 || xi.val.Tc_ActualRank > xj.val.Tc_ActualRank && xi.val.Tc_ExpectedRank < xj.val.Tc_ExpectedRank)
                             select 1
                            ).Count() 
                            / (divN * (divN - 1) / 2.0);

                        var tc3KendalTauDist =
                            (from xi in ratedDivRoundResults.Select((x, i) => new { val = x, idx = i })
                             from xj in ratedDivRoundResults.Select((x, i) => new { val = x, idx = i })
                             where xi.idx < xj.idx
                             where (xi.val.Tc_ActualRank < xj.val.Tc_ActualRank && xi.val.Tc3_ExpectedRank > xj.val.Tc3_ExpectedRank)
                                || (xi.val.Tc_ActualRank > xj.val.Tc_ActualRank && xi.val.Tc3_ExpectedRank < xj.val.Tc3_ExpectedRank)
                             select 1
                            ).Count() 
                            / (divN * (divN - 1) / 2.0);
                        
                        if (div == 1)
                        {
                            round.DivOneCodersCount = divN;
                            round.DivOneRatingsMean = divMean;
                            round.DivOneRatingsDeviation = divStdev;
                            round.DivOneTcRatingsDiffMean = divRatingsDiffMean;
                            round.DivOneTcVolatilityDiffMean = divVolatilityDiffMean;
                            round.DivOneKendalTauDist = kendalTauDist;
                            round.Tc3_DivOneKendalTauDist = tc3KendalTauDist;
                        }
                        if (div == 2)
                        {
                            round.DivTwoCodersCount = divN;
                            round.DivTwoRatingsMean = divMean;
                            round.DivTwoRatingsDeviation = divStdev;
                            round.DivTwoTcRatingsDiffMean = divRatingsDiffMean;
                            round.DivTwoTcVolatilityDiffMean = divVolatilityDiffMean;
                            round.DivTwoKendalTauDist = kendalTauDist;
                            round.Tc3_DivTwoKendalTauDist = tc3KendalTauDist;
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