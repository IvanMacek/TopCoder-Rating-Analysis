namespace TopCoder.Tools.TcAlgorithmRunner.Functions
{
    public class Coder
    {
        public Coder(double rating, double volatility)
        {
            R = rating;
            V = volatility;
        }

        public double R { get; private set; }
        public double V { get; private set; }
    }
}