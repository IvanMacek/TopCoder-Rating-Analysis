using System.Collections.Generic;
using System.Linq;

using TopCoder.Analysis.Data;

namespace TopCoder.Analysis.Web.Models.Coders
{
    public class SearchModel
    {
        public IEnumerable<Coder> Coders { get; set; }
        public string SearchQuery { get; set; }

        public SearchModel()
        {
            Coders = Enumerable.Empty<Coder>();
        }
    }
}