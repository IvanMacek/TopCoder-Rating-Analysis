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
                var rounds = db.Rounds.ToList();
                foreach (var round in rounds)
                {
                    // Calculate Competition Factor
                    //var participants = round.RoundResults.Select(x => new Functions.Coder(x.Tc_OldRating, x.Tc_OldVolatility)).ToList();
                    //var cf = new CompetitionFactorFunction().Calculate(participants);
                    //round.CompetitionFactor = cf;

                    foreach (var rr in round.RoundResults)
                    {
                        // Calculate Weight and K-Factor
                        var w = new CoderCompetitionWeightFunction().Calculate(rr.Elo_OldRating, rr.NumberOfRatings);
                        var k = new KFactorFunction().Calculate(round.CompetitionFactor, w);

                        rr.Tc_Weight = w;
                        rr.Tc_KFactor = k;
                    }

                    db.SaveChanges();

                    Console.WriteLine(round.Name);
                }
            }
        }
    }
}
