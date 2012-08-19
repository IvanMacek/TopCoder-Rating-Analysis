using System.Collections.Generic;

using TopCoder.Analysis.Data;

namespace TopCoder.Analysis.Web.Models.Coders
{
    public class ListModel
    {
        public IList<Coder> TcTop100Coders { get; set; }
        public IList<Coder> Coders { get; set; }

        public ListModel()
        {
            TcTop100Coders = new List<Coder>();
            Coders = new List<Coder>();
        }
    }
}