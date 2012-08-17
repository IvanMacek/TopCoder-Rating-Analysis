namespace TopCoder.Tools.TcAlgorithmRunner.Functions
{
    public class UpdateRuleFunction
    {
        public double Calculate(int oldRating, double expectedPreformance, double actualPerformance, double kFactor)
        {
            var newRating = oldRating + kFactor * (actualPerformance - expectedPreformance);
            return newRating;
        }
    }
}