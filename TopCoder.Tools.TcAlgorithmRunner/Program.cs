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
                    //var competitionFactorFunction = new CompetitionFactorFunction();
                    //var cf = competitionFactorFunction.Calculate(participants);
                    //round.CompetitionFactor = cf;


                    db.SaveChanges();

                    Console.WriteLine(round.Name);
                }
            }
        }
    }
}
