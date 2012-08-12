using System.Collections.Generic;

using TopCoder.Analysis.Data;

namespace TopCoder.Analysis.Web.Models.Coders
{
    public class Top100Model
    {
        public IList<Coder> TcTop100Coders { get; set; }
        public IList<Coder> EloTop100Coders { get; set; }

        public Top100Model()
        {
            TcTop100Coders = new List<Coder>();
            EloTop100Coders = new List<Coder>();
        }
    }
}