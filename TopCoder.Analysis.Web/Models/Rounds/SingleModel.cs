using System.Collections.Generic;

using TopCoder.Analysis.Data;

namespace TopCoder.Analysis.Web.Models.Rounds
{
    public class SingleModel
    {
        public Round Round { get; set; }
        public IList<RoundResult> RoundResults { get; set; }

        public SingleModel()
        {
            RoundResults = new List<RoundResult>();
        }
    }
}