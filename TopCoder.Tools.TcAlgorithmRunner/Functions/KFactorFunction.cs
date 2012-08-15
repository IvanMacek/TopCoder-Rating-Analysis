namespace TopCoder.Tools.TcAlgorithmRunner.Functions
{
    public class KFactorFunction
    {
        public double Calculate(double competitionFactor, double codersCompetitionWeight)
        {
            var cf = competitionFactor;
            var w = codersCompetitionWeight;

            return cf * w / (w + 1);
        }
    }
}