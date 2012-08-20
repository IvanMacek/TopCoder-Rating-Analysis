using System.Collections.Generic;

using TopCoder.Analysis.Data;

namespace TopCoder.Analysis.Web.Models.Coders
{
    public class ListModel
    {
        public IList<Coder> Coders { get; set; }
        public IList<Coder> TcTop20Coders { get; set; }
        public IList<Coder> Tc2Top20Coders { get; set; }

        public ListModel()
        {
            Coders = new List<Coder>();
            TcTop20Coders = new List<Coder>();
            Tc2Top20Coders = new List<Coder>();
        }
    }
}