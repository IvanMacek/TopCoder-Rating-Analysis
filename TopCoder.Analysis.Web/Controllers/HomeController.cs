using System.Web.Mvc;

namespace TopCoder.Analysis.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("List", "Rounds");
        }
    }
}
