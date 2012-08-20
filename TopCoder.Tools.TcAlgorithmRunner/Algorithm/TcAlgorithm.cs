using System;
using System.Linq;

using TopCoder.Analysis.Data;
using TopCoder.Tools.TcAlgorithmRunner.Functions;

namespace TopCoder.Tools.TcAlgorithmRunner.Algorithm
{
    public class TcAlgorithm
    {
        public void Run()
        {
            using (var db = new TcAnalysisDataModel())
            {
                var rounds = db.Rounds.OrderBy(r => r.Date).ToList();
                foreach (var round in rounds)
                {
                    var unratedDivRoundResults = round.RoundResults.Where(x => !x.IsRated).ToList();
                    foreach (var rr in unratedDivRoundResults)
                    {
                        rr.Tc_Weight = 0;
                        rr.Tc_KFactor = 0;
                        rr.Tc_ActualRank = 0;
                        rr.Tc_ExpectedRank = 0;
                        rr.Tc_ActualPerf = 0;
                        rr.Tc_ExpectedPerf = 0;
                        rr.Tc_Cap = 0;
                        rr.Tc_NewRating = 0;
                        rr.Tc_NewVolatility = 0;
                    }

                    foreach (var div in new[] { 1, 2 })
                    {
                        var ratedDivRoundResults = round.RoundResults.Where(x => x.Division == div && x.IsRated).ToList();
                        var codersInDiv = ratedDivRoundResults.Select(y => new Functions.Coder(y.OldRating, y.OldVolatility)).ToList();
                        var scoresInDiv = ratedDivRoundResults.Select(y => y.Points).ToList();

                        if (!codersInDiv.Any()) { continue; }

                        var cf = new CompetitionFactorFunction().Calculate(codersInDiv);
                        if (div == 1) { round.DivOneCompetitionFactor = cf; }
                        if (div == 2) { round.DivTwoCompetitionFactor = cf; }

                        foreach (var rr in ratedDivRoundResults)
                        {
                            var w = new CoderCompetitionWeightFunction().Calculate(rr.OldRating, rr.NumberOfRatings - 1);
                            rr.Tc_Weight = w;

                            var k = new KFactorFunction().Calculate((div == 1) ? round.DivOneCompetitionFactor : round.DivTwoCompetitionFactor, rr.Tc_Weight);
                            rr.Tc_KFactor = k;

                            var arank = new ActualRankFunction().Calculate(rr.Points, scoresInDiv);
                            rr.Tc_ActualRank = arank;

                            var erank = new ExpectedRankFunction().Calculate(new Functions.Coder(rr.OldRating, rr.OldVolatility), codersInDiv);
                            rr.Tc_ExpectedRank = erank;

                            var aperf = new PerformanceFunction().Calculate(rr.Tc_ActualRank, codersInDiv.Count);
                            rr.Tc_ActualPerf = aperf;

                            var eperf = new PerformanceFunction().Calculate(rr.Tc_ExpectedRank, codersInDiv.Count);
                            rr.Tc_ExpectedPerf = eperf;

                            var cap = new CapFunction().Calculate(rr.NumberOfRatings - 1);
                            rr.Tc_Cap = cap;

                            var updatedRating = new UpdateRuleFunction().Calculate(rr.OldRating, rr.Tc_ExpectedPerf, rr.Tc_ActualPerf, rr.Tc_KFactor);
                            var newRating = new NewRatingFunction().Calculate(rr.OldRating, updatedRating, rr.Tc_Cap);
                            rr.Tc_NewRating = newRating;

                            var newVolatility = new NewVolatilityFunction().Calculate(rr.OldRating, rr.OldVolatility, rr.Tc_NewRating, rr.Tc_Weight, rr.NumberOfRatings - 1);
                            rr.Tc_NewVolatility = newVolatility;
                        }
                    }

                    db.SaveChanges();

                    Console.WriteLine(round.Name);
                }
            }
        }
    }
}