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
                    foreach (var div in new[] { 1, 2 })
                    {
                        var divRoundResults = round.RoundResults.Where(x => x.Division == div).ToList();
                        if (!divRoundResults.Any()) { continue; }

                        var divN = divRoundResults.Count;
                        var divRatings = divRoundResults.Select(x => x.OldRating).ToList();
                        var divMean = divRatings.Average();
                        var divStdev = Math.Sqrt(divRatings.Sum(r => Math.Pow(r - divMean, 2)) / divN);

                        if (div == 1)
                        {
                            round.DivOneCodersCount = divN;
                            round.DivOneRatingsMean = divMean;
                            round.DivOneRatingsDeviation = divStdev;
                        }
                        if (div == 2)
                        {
                            round.DivTwoCodersCount = divN;
                            round.DivTwoRatingsMean = divMean;
                            round.DivTwoRatingsDeviation = divStdev;
                        }
                    }

                    foreach (var rr in round.RoundResults)
                    {
                        coders[rr.CoderId] = rr.NewRating;
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