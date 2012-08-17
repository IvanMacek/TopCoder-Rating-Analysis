using System;
using System.Collections.Generic;

namespace TopCoder.Tools.TcAlgorithmRunner.Functions
{
    public class ActualRankFunction
    {
        public double Calculate(double coderScore, IEnumerable<double> allScores)
        {
            var lessThanScore = 0;
            var equalToScore = 0;
            foreach(var score in allScores)
            {
                if (coderScore < score) { ++lessThanScore; }  
                else if (Math.Abs(score - coderScore) < 0.000000001 ) { ++equalToScore; }  
            }

            var actualRank = 0.5 + lessThanScore + 0.5 * equalToScore;
            return actualRank;
        }
    }
}