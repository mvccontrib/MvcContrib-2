using System.Web.Mvc;

namespace LoginPortableArea.Login.Controllers
{
    public class RssController : Controller
    {
        public ActionResult Index(string RssUrl)
        {
            return View(new SyndicationService().GetFeed(RssUrl, 10));
        }
    }
}