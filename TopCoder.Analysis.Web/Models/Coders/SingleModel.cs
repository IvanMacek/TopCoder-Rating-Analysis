﻿using System.Collections.Generic;

using TopCoder.Analysis.Data;

namespace TopCoder.Analysis.Web.Models.Coders
{
    public class SingleModel
    {
        public Coder Coder { get; set; }
        public IList<RoundResult> RoundResults { get; set; }

        public SingleModel()
        {
            RoundResults = new List<RoundResult>();
        }
    }
}