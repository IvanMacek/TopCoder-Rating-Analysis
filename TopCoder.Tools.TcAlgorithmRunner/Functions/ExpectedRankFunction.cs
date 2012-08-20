using System;
using System.Collections.Generic;
using System.Linq;

using MathNet.Numerics;

namespace TopCoder.Tools.TcAlgorithmRunner.Functions
{
    public class ExpectedRankFunction
    {
        public double Calculate(Coder coder, IEnumerable<Coder> opponents)
        {
            var r = coder.R;
            var v = coder.V;

            var wpSum = opponents.Sum(x =>
            {
                var rankDiff = (x.R - r) / Math.Sqrt(x.V * x.V + v * v);
                var wp = 0.5 * (1 + SpecialFunctions.Erf(Constants.Sqrt1Over2 * rankDiff));
                return wp;
            });

            var er = wpSum + 0.5;
            return er;
        }
    }
}