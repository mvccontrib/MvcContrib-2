using System.Web.Mvc;
using System.Web.Security;
using LoginPortableArea.Login.Messages;
using LoginPortableArea.Login.Models;
using MvcContrib.PortableAreas;

namespace LoginPortableArea.Login.Controllers
{
	public class LoginController : Controller
	{
		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Index(LoginInput loginInput)
		{
			var message = new LoginInputMessage {Input = loginInput, Result = new LoginResult()};

			PortableArea.Bus.Send(message);

			if (message.Result.Success)
			{
				FormsAuthentication.RedirectFromLoginPage(loginInput.Username, false);
			}

			ModelState.AddModelError("model", message.Result.Message);

			return View(loginInput);
		}
	}
}