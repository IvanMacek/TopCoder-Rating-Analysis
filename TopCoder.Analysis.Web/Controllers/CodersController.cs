using System.Linq;
using System.Web.Mvc;

using TopCoder.Analysis.Data;
using TopCoder.Analysis.Web.Models.Coders;

namespace TopCoder.Analysis.Web.Controllers
{
    public class CodersController : Controller
    {
        public ActionResult Search(string query)
        {
            var model = new SearchModel
            {
                SearchQuery = (query ?? string.Empty).Trim()
            };

            if (model.SearchQuery.Length > 0)
            {
                using (var db = new TcAnalysisDataModel())
                {
                    model.Coders =
                        (from coder in db.Coders.Include("FirstRound").Include("LastRound")
                         where coder.Handle.StartsWith(model.SearchQuery)
                         orderby coder.Handle
                         select coder
                        ).ToList();
                }
            }

            return View(model);
        }

        public ViewResult List()
        {
            var model = new ListModel();

            using (var db = new TcAnalysisDataModel())
            {
                model.Coders = db.Coders.ToList();

                model.TcTop100Coders =
                    (from coder in db.Coders.Include("FirstRound").Include("LastRound")
                     orderby coder.Rating descending
                     select coder 
                    ).Take(100)
                     .ToList();
            }

            return View(model);
        }

        public ViewResult Single(string handle)
        {
            var model = new SingleModel();

            using (var db = new TcAnalysisDataModel())
            {
                model.Coder = 
                    (from coder in db.Coders.Include("FirstRound").Include("LastRound")
                     where coder.Handle == handle
                     select coder
                    ).Single();

                model.RoundResults =
                    (from rr in db.RoundResults.Include("Round").Include("Coder")
                     where rr.CoderId == model.Coder.Id
                     orderby rr.Round.Date ascending
                     select rr
                    ).ToList();
            }

            return View(model);
        }
    }
}
