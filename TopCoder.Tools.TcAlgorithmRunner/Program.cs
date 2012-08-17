using System;
using System.Linq;

using TopCoder.Analysis.Data;
using TopCoder.Tools.TcAlgorithmRunner.Functions;

namespace TopCoder.Tools.TcAlgorithmRunner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var db = new TcAnalysisDataModel())
            {
                var rounds = db.Rounds.OrderBy(r => r.Date).ToList();
                foreach (var round in rounds)
                {
                    foreach (var div in new[] { 1, 2 })
                    {
                        var divRoundResults = round.RoundResults.Where(x => x.Division == div).ToList();
                        var codersInDiv = divRoundResults.Select(y => new Functions.Coder(y.OldRating, y.OldVolatility)).ToList();

                        if (!codersInDiv.Any()) { continue; }

                        //var cf = new CompetitionFactorFunction().Calculate(codersInDiv);
                        //if (div == 1) { round.DivOneCompetitionFactor = cf; }
                        //if (div == 2) { round.DivTwoCompetitionFactor = cf; }

                        foreach (var rr in divRoundResults)
                        {
                            //var w = new CoderCompetitionWeightFunction().Calculate(rr.Tc_OldRating, rr.NumberOfRatings - 1);
                            //rr.Tc_Weight = w;

                            //var k = new KFactorFunction().Calculate((div == 1) ? round.DivOneCompetitionFactor : round.DivTwoCompetitionFactor, rr.Tc_Weight);
                            //rr.Tc_KFactor = k;
                            //Console.WriteLine(k);

                            var er = new ExpectedRankFunction().Calculate(new Functions.Coder(rr.OldRating, rr.OldVolatility), codersInDiv);
                            rr.Tc_ExpectedRank = er;
                            //Console.WriteLine(er);
                        }
                    }

                    db.SaveChanges();

                    Console.WriteLine(round.Name);
                }
            }
        }
    }
}
