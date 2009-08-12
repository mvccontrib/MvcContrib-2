using System.Threading;
using System.Web.Mvc;
using MvcContrib.ActionResults;
using MvcContrib.Attributes;

namespace MvcContrib.UnitTests.ConventionController
{
	class TestControllerWithNoDefaultActions : Controller
	{
		public ActionResult Index()
		{
			return new EmptyResult();
		}
	}

	class TestController : Controller
	{
		public bool ActionWasCalled;
		public bool? OnErrorResult = false;
		public bool ActionExecutingCalled;
		public bool CustomActionResultCalled;
		public string BinderFilterOrdering = string.Empty;

		public TestController()
		{
		}

		public TestController(IActionInvoker invokerToUse)
		{
			this.ActionInvoker = invokerToUse;
		}

		[TestFilter]
		public ActionResult BinderFilterOrderingAction([TestBinder] object item)
		{
			return new EmptyResult();
		}

		public ActionResult BasicAction(int? id)
		{
			return new EmptyResult();
		}

		public ActionResult SimpleAction(string param1)
		{
			return new EmptyResult();
		}

		public ActionResult SimpleAction(string param1, int param2)
		{
			return new EmptyResult();
		}

		[NonAction]
		public ActionResult HiddenAction()
		{
			return new EmptyResult();
		}

		public ActionResult ComplexAction([Deserialize("ids")] int[] ids)
		{
			ActionWasCalled = true;
			return new EmptyResult();
		}

		public ActionResult CustomResult()
		{
			return new CustomActionResult();
		}

		public ActionResult BadAction()
		{
			throw new AbandonedMutexException();
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			ActionExecutingCalled = true;
		}

		public ActionResult XmlResult()
		{
			return new XmlResult("Test 1 2 3");
		}

		public RedirectToRouteResult RedirectActionOnSameController()
		{
			return this.RedirectToAction<TestController>(c => c.BasicAction(1));
		}

		public RedirectToRouteResult RedirectActionOnAnotherController()
		{
			return this.RedirectToAction<AnotherTestController>(c => c.SomeAction(2));
		}
	}
}