using System;
using System.Collections.Generic;
using System.Linq;

namespace TopCoder.Tools.TcAlgorithmRunner.Functions
{
    public class CompetitionFactorFunction
    {
        public double Calculate(IList<Coder> participants)
        {
            var n = participants.Count;
            var ratAvg = participants.Average(x => x.R);

            var sqrRatStdev = participants.Sum(x => Math.Pow(x.R - ratAvg, 2)) / (n - 1);
            var sqrVolAvg = participants.Sum(x => x.V * x.V) / n;

            return Math.Sqrt(sqrVolAvg + sqrRatStdev);
        }
    }
}