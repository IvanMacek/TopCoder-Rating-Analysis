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

        public ViewResult Top100()
        {
            var model = new Top100Model();

            using (var db = new TcAnalysisDataModel())
            {
                model.TcTop100Coders =
                    (from coder in db.Coders.Include("FirstRound").Include("LastRound")
                     orderby coder.Tc_Rating descending
                     select coder 
                    ).Take(100)
                     .ToList();

                model.EloTop100Coders =
                    (from coder in db.Coders.Include("FirstRound").Include("LastRound")
                     orderby coder.Elo_Rating descending
                     select coder
                    ).Take(100)
                     .ToList();
            }

            return View(model);
        }

        public ViewResult Single(string coderHandle)
        {
            return View();
        }
    }
}
