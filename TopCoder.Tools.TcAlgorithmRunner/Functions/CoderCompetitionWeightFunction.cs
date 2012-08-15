namespace TopCoder.Tools.TcAlgorithmRunner.Functions
{
    public class CoderCompetitionWeightFunction
    {
        public double Calculate(int codersRating, int codersNumberOfRatedRounds)
        {
            var r = codersRating;
            var n = codersNumberOfRatedRounds;

            var w = 1 / (0.82 - 0.42 / (n + 1));

            var stabilization =
                (r >= 2500) ? 0.8 :
                (r >= 2000) ? 0.9 : 1;
            
            return w * stabilization;
        }
    }
}