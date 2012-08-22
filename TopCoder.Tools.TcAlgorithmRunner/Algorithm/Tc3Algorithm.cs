using System;
using System.Linq;

using TopCoder.Analysis.Data;
using TopCoder.Tools.TcAlgorithmRunner.Functions;

namespace TopCoder.Tools.TcAlgorithmRunner.Algorithm
{
    public class Tc3Algorithm
    {
        public void Run()
        {
            using (var db = new TcAnalysisDataModel())
            {
                var defaultCoder = new { Rating = 1200, Volatility = 535, NumberOfRatings = 0 };
                var coders = new int[0].ToDictionary(x => 1, x => defaultCoder);

                var rounds = db.Rounds.OrderBy(r => r.Date).ToList();
                foreach (var round in rounds)
                {
                    var ratedRoundResults = round.RoundResults.Where(x => x.IsRated).ToList();
                    foreach (var rr in ratedRoundResults)
                    {
                        var coder = coders.ContainsKey(rr.CoderId) ? coders[rr.CoderId] : defaultCoder; rr.Tc3_OldRating = coder.Rating;
                        rr.Tc3_OldRating = coder.Rating;
                        rr.Tc3_OldVolatility = coder.Volatility;
                    }

                    foreach (var div in new[] { 1, 2 })
                    {
                        var ratedDivRoundResults = ratedRoundResults.Where(x => x.Division == div).ToList();
                        var codersInDiv = ratedDivRoundResults.Select(y => new Functions.Coder(y.Tc3_OldRating, y.Tc3_OldVolatility)).ToList();
                        var scoresInDiv = ratedDivRoundResults.Select(y => y.Points).ToList();

                        if (!codersInDiv.Any()) { continue; }

                        var cf = new CompetitionFactorFunction().Calculate(codersInDiv);

                        foreach (var rr in ratedDivRoundResults)
                        {
                            var w = new CoderCompetitionWeightFunction().Calculate(rr.Tc3_OldRating, rr.NumberOfRatings - 1);

                            var k = new KFactorFunction().Calculate(cf, w);

                            var arank = new ActualRankFunction().Calculate(rr.Points, scoresInDiv);

                            var erank = new ExpectedRankFunction().Calculate(new Functions.Coder(rr.Tc3_OldRating, rr.Tc3_OldVolatility), codersInDiv);
                            rr.Tc3_ExpectedRank = erank;

                            var aperf = new PerformanceFunction().Calculate(arank, codersInDiv.Count);

                            var eperf = new PerformanceFunction().Calculate(erank, codersInDiv.Count);

                            var cap = new CapFunction().Calculate(rr.NumberOfRatings - 1);

                            var updatedRating = new UpdateRuleFunction().Calculate(rr.Tc3_OldRating, eperf, aperf, k);
                            var newRating = new NewRatingFunction().Calculate(rr.Tc3_OldRating, updatedRating, cap);
                            rr.Tc3_NewRating = newRating;

                            rr.Tc3_NewVolatility = 385;

                            coders[rr.CoderId] = new { Rating = rr.Tc3_NewRating, Volatility = rr.Tc3_NewVolatility, rr.NumberOfRatings };
                        }
                    }

                    db.SaveChanges();

                    Console.WriteLine(round.Name);
                }

                foreach (var coder in coders)
                {
                    var dbCoder = db.Coders.First(x => x.Id == coder.Key);
                    dbCoder.Tc3_Rating = coder.Value.Rating;
                    dbCoder.Tc3_Volatility = coder.Value.Volatility;
                    dbCoder.Tc3_RoundsCount = coder.Value.NumberOfRatings;
                }

                db.SaveChanges();

                Console.WriteLine("Coder data saved.");
            }
        }
    }
}