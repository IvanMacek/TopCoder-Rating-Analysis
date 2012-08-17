using MathNet.Numerics.Distributions;

namespace TopCoder.Tools.TcAlgorithmRunner.Functions
{
    public class PerformanceFunction
    {
        private static readonly Normal _StdNormalDistribution = Normal.WithMeanStdDev(0, 1);

        public double Calculate(double rank, int codersCount)
        {
            var val = (rank - 0.5)/codersCount;
            var norminv = _StdNormalDistribution.InverseCumulativeDistribution(val);
            return -norminv;
        }
    }
}