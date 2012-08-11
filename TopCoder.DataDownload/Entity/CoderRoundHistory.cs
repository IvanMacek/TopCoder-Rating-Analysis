using System;

namespace TopCoder.DataDownload.Entity
{
    public class CoderRoundHistory
    {
        public int RoundId { get; set; }
        public string RoundName { get; set; }
        public DateTime RoundDateTime { get; set; }

        public int CoderId { get; set; }
        public string CoderHandle { get; set; }

        public int Division { get; set; }
        public int DivisionPlace { get; set; }

        public float Points { get; set; }

        public int OldRating { get; set; }
        public int NewRating { get; set; }
        public int NewVolatility { get; set; }
        public int NumberOfRatings { get; set; }
        public bool IsRated { get; set; }

        //public int Rank { get; set; }
        //public float Percentile { get; set; }
        //public int RatingOrder { get; set; }
    }
}