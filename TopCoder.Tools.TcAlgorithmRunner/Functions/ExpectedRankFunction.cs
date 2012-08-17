using System;
using System.Collections.Generic;
using System.Linq;

using MathNet.Numerics.Distributions;

namespace TopCoder.Tools.TcAlgorithmRunner.Functions
{
    public class ExpectedRankFunction
    {
        private static readonly Normal _StdNormalDistribution = Normal.WithMeanStdDev(0, 1);

        public double Calculate(Coder coder, IEnumerable<Coder> opponents)
        {
            var r = coder.R;
            var v = coder.V;

            var wpSum = opponents.Sum(x =>
            {
                var rankDiff = (x.R - r) / Math.Sqrt(x.V * x.V + v * v);
                var wp = _StdNormalDistribution.CumulativeDistribution(rankDiff);
                return wp;
            });

            var er = wpSum + 0.5;
            return er;
        }
    }
}