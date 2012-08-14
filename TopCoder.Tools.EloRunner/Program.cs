using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using TopCoder.Analysis.Data;
using TopCoder.Tools.EloRunner.Elo;

namespace TopCoder.Tools.EloRunner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const int startingRating = 1200;
            const int startingKFactor = 11;

            var globalStopwatch = Stopwatch.StartNew();

            using (var db = new TcAnalysisDataModel())
            {
                Trace.Write(string.Format("Coders ratings setting to {0} points and {1} K-Factor... ", startingRating, startingKFactor));
                
                _ResetAllCodersRatings(db, startingRating, startingKFactor);
                
                Trace.WriteLine("Done!");


                var rounds = db.Rounds.OrderBy(r => r.Date).ToList();

                Trace.WriteLine(string.Format("Started calculating ELO over {0} rounds...", rounds.Count));
                Trace.Indent();

                _UpdateRoundResults(db, rounds);

                Trace.Unindent();
                Trace.WriteLine(string.Format("Done calculating ELO over {0} rounds!", rounds.Count));

                globalStopwatch.Stop();
                Trace.WriteLine(string.Format("Time to finish: {0}", TimeSpan.FromMilliseconds(globalStopwatch.ElapsedMilliseconds)));
            }
        }

        private static void _UpdateRoundResults(TcAnalysisDataModel db, IList<Round> rounds)
        {
            foreach (var x in rounds.Select((x, i) => new { Round = x, Index = i }))
            {
                var index = x.Index;
                var round = x.Round;
                var roundStopwatch = Stopwatch.StartNew();

                Trace.Write(string.Format("{0:000}. {1}... ", index + 1, round.Name));

                var divisionsResults =
                    (from rr in db.RoundResults.Include("Coder")
                     where rr.RoundId == round.Id
                     group rr by rr.Division
                     into divGroups
                     select divGroups
                    ).ToList();

                foreach (var results in divisionsResults)
                {
                    foreach (var result in results.Where(r => r.IsRated))
                    {
                        var currentRating = result.Coder.Elo_Rating;
                        var currentKFactor = result.Coder.Elo_KFactor;

                        var opponentsRatings = results.Select(rr => rr.Coder.Elo_Rating).ToArray();
                        var observedScores =
                            results.Where(rr => rr.DivisionPlace < result.DivisionPlace).Select(rr => 0.0)
                                .Concat(results.Where(rr => rr.DivisionPlace == result.DivisionPlace).Select(rr => 0.5))
                                .Concat(results.Where(rr => rr.DivisionPlace > result.DivisionPlace).Select(rr => 1.0))
                                .ToArray();

                        var eloAlgorithm = new EloAlgorithm();
                        var newRating = eloAlgorithm.CalculateNewRating(currentRating, currentKFactor, opponentsRatings, observedScores);

                        // Update result
                        result.Elo_OldRating = currentRating;
                        result.Elo_NewRating = newRating;
                        result.Elo_NewKFactor = currentKFactor;
                        result.Elo_RatingDiff = newRating - currentRating;
                    }
                }

                // Update coders
                foreach (var result in divisionsResults.SelectMany(a => a).Where(r => r.IsRated))
                {
                    result.Coder.Elo_Rating = result.Elo_NewRating;
                    result.Coder.Elo_KFactor = result.Elo_NewKFactor;
                }

                db.SaveChanges();

                roundStopwatch.Stop();
                Trace.WriteLine(TimeSpan.FromMilliseconds(roundStopwatch.ElapsedMilliseconds));
            }
        }

        private static void _ResetAllCodersRatings(TcAnalysisDataModel db, int startingRating, int startingKFactor)
        {
            var coders = db.Coders.ToList();
            foreach (var coder in coders)
            {
                coder.Elo_Rating = startingRating;
                coder.Elo_KFactor = startingKFactor;
            }

            db.SaveChanges();
        }
    }
}
