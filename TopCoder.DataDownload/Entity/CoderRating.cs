namespace TopCoder.DataDownload.Entity
{
    public class CoderRating
    {
        public int Id { get; set; }
        public string Handle { get; set; }
        public string Country { get; set; }
        public int Rating { get; set; }
        public int Volatility { get; set; }
        public int EventsCount { get; set; }
    }
}