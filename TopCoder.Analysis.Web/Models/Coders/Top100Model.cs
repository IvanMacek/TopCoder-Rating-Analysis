using System.Collections.Generic;
using System.Linq;

using TopCoder.Analysis.Data;

namespace TopCoder.Analysis.Web.Models.Coders
{
    public class Top100Model
    {
        public IEnumerable<Coder> TcTop100Coders { get; set; }
        public IEnumerable<Coder> EloTop100Coders { get; set; }

        public Top100Model()
        {
            TcTop100Coders = Enumerable.Empty<Coder>();
            EloTop100Coders = Enumerable.Empty<Coder>();
        }
    }
}