using System;

namespace TopCoder.Tools.TcAlgorithmRunner.Functions
{
    public class NewRatingFunction
    {
        public int Calculate(int oldRating, double newRating, double cap)
        {
            var r = Math.Min(Math.Max(newRating, oldRating - cap), oldRating + cap);
            return (int)Math.Round(r);
        }
    }
}