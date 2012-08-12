using System.Collections.Generic;

using TopCoder.Analysis.Data;

namespace TopCoder.Analysis.Web.Models.Coders
{
    public class SearchModel
    {
        public IList<Coder> Coders { get; set; }
        public string SearchQuery { get; set; }

        public SearchModel()
        {
            Coders = new List<Coder>();
        }
    }
}