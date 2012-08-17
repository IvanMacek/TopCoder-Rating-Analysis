namespace TopCoder.Analysis.Data
{
    public partial class RoundResult
    {
        public double RatingDiff
        {
            get { return NewRating - OldRating; }
        }

        public double VolatilityDiff
        {
            get { return NewVolatility - OldVolatility; }
        }

        public double Tc_RankDiff
        {
            get { return Tc_ExpectedRank - DivisionRank; }
        }
    }
}