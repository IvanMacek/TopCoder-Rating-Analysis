using System.Collections.Generic;

using TopCoder.Analysis.Data;

namespace TopCoder.Analysis.Web.Models.Rounds
{
    public class ListModel
    {
        public IList<Round> Rounds { get; set; }

        public ListModel()
        {
            Rounds = new List<Round>();
        }
    }
}