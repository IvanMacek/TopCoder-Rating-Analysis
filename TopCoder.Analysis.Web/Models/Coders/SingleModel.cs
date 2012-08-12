using System.Collections.Generic;
using System.Linq;

using TopCoder.Analysis.Data;

namespace TopCoder.Analysis.Web.Models.Coders
{
    public class SingleModel
    {
        public Coder Coder { get; set; }
        public IEnumerable<RoundResult> RoundResults { get; set; }

        public SingleModel()
        {
            RoundResults = Enumerable.Empty<RoundResult>();
        }
    }
}