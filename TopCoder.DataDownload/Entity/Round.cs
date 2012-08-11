using System;

namespace TopCoder.DataDownload.Entity
{
    public class Round
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string Type { get; set; }
        public DateTime DateTime { get; set; }
    }
}