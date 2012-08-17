namespace TopCoder.Tools.TcAlgorithmRunner.Functions
{
    public class CapFunction
    {
        public double Calculate(int codersNumberOfRatedRounds)
        {
            var cap = 150 + 1500 / (codersNumberOfRatedRounds + 2);
            return cap;
        }
    }
}