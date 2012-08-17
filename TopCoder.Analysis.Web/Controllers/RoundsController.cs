using System.Linq;
using System.Web.Mvc;

using TopCoder.Analysis.Data;
using TopCoder.Analysis.Web.Models.Rounds;

namespace TopCoder.Analysis.Web.Controllers
{
    public class RoundsController : Controller
    {
        public ActionResult List()
        {
            var model = new ListModel();

            using (var db = new TcAnalysisDataModel())
            {
                model.Rounds =
                    (from round in db.Rounds
                     orderby round.Date
                     select round
                    ).ToList();
            }

            return View(model);
        }

        public ViewResult Single(int roundId)
        {
            var model = new SingleModel();

            using (var db = new TcAnalysisDataModel())
            {
                model.Round = db.Rounds.Single(r => r.Id == roundId);

                model.RoundResults =
                    (from rr in db.RoundResults.Include("Round").Include("Coder")
                     where rr.RoundId == roundId
                     orderby rr.DivisionPlace ascending
                     select rr
                    ).ToList();
            }

            return View(model);
        }
    }
}
