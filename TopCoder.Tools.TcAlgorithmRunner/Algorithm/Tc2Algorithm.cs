using System;
using System.Linq;

using TopCoder.Analysis.Data;
using TopCoder.Tools.TcAlgorithmRunner.Functions;

namespace TopCoder.Tools.TcAlgorithmRunner.Algorithm
{
    public class Tc2Algorithm
    {
        private const int _Year = 2008;

        public void Run()
        {
            using (var db = new TcAnalysisDataModel())
            {
                var defaultCoder = new { Rating = 1200, Volatility = 535, NumberOfRatings = 0 };
                var coders = new int[0].ToDictionary(x => 1, x => defaultCoder);

                var rounds = db.Rounds.Where(x => x.Date.Year >= _Year).OrderBy(r => r.Date).ToList();
                foreach (var round in rounds)
                {
                    var ratedRoundResults = round.RoundResults.Where(x => x.IsRated).ToList();
                    foreach (var rr in ratedRoundResults)
                    {
                        var coder = coders.ContainsKey(rr.CoderId) ? coders[rr.CoderId] : defaultCoder; rr.Tc2_OldRating = coder.Rating;
                        rr.Tc2_OldRating = coder.Rating;
                        rr.Tc2_OldVolatility = coder.Volatility;
                        rr.Tc2_NumberOfRatings = coder.NumberOfRatings + 1;
                    }

                    foreach (var div in new[] { 1, 2 })
                    {
                        var ratedDivRoundResults = ratedRoundResults.Where(x => x.Division == div).ToList();
                        var codersInDiv = ratedDivRoundResults.Select(y => new Functions.Coder(y.Tc2_OldRating, y.Tc2_OldVolatility)).ToList();
                        var scoresInDiv = ratedDivRoundResults.Select(y => y.Points).ToList();

                        if (!codersInDiv.Any()) { continue; }

                        var cf = new CompetitionFactorFunction().Calculate(codersInDiv);

                        foreach (var rr in ratedDivRoundResults)
                        {
                            var w = new CoderCompetitionWeightFunction().Calculate(rr.Tc2_OldRating, rr.Tc2_NumberOfRatings - 1);

                            var k = new KFactorFunction().Calculate(cf, w);

                            var arank = new ActualRankFunction().Calculate(rr.Points, scoresInDiv);

                            var erank = new ExpectedRankFunction().Calculate(new Functions.Coder(rr.Tc2_OldRating, rr.Tc2_OldVolatility), codersInDiv);

                            var aperf = new PerformanceFunction().Calculate(arank, codersInDiv.Count);

                            var eperf = new PerformanceFunction().Calculate(erank, codersInDiv.Count);

                            var cap = new CapFunction().Calculate(rr.Tc2_NumberOfRatings - 1);

                            var updatedRating = new UpdateRuleFunction().Calculate(rr.Tc2_OldRating, eperf, aperf, k);
                            var newRating = new NewRatingFunction().Calculate(rr.Tc2_OldRating, updatedRating, cap);
                            rr.Tc2_NewRating = newRating;

                            var newVolatility = new NewVolatilityFunction().Calculate(rr.Tc2_OldRating, rr.Tc2_OldVolatility, newRating, w, rr.Tc2_NumberOfRatings - 1);
                            rr.Tc2_NewVolatility = newVolatility;

                            coders[rr.CoderId] = new { Rating = rr.Tc2_NewRating, Volatility = rr.Tc2_NewVolatility, NumberOfRatings = rr.Tc2_NumberOfRatings };
                        }
                    }

                    db.SaveChanges();

                    Console.WriteLine(round.Name);
                }

                foreach (var coder in coders)
                {
                    var dbCoder = db.Coders.First(x => x.Id == coder.Key);
                    dbCoder.Tc2_Rating = coder.Value.Rating;
                    dbCoder.Tc2_Volatility = coder.Value.Volatility;
                    dbCoder.Tc2_RoundsCount = coder.Value.NumberOfRatings;
                }

                db.SaveChanges();

                Console.WriteLine("Coder data saved.");
            }
        }
    }
}