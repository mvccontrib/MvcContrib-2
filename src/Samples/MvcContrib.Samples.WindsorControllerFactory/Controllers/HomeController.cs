using System.Web.Mvc;
using MvcContrib.Samples.WindsorControllerFactory.Models;

namespace MvcContrib.Samples.WindsorControllerFactory.Controllers
{
	public class HomeController : Controller
	{
		private IService _service;


		public HomeController(IService service)
		{
			_service = service;
		}

		public ActionResult Index()
		{
			ViewData["numbers"] = _service.GetNumbers();
			return View("index");
		}
	}
}
