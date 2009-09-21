namespace MvcContrib.UnitTests.TestHelper
{
	public class TestHelperWithArgsController : TestHelperController
	{
		private readonly ITestService _service;

		public TestHelperWithArgsController(ITestService service)
		{
			_service = service;
		}

		public string ReturnMooFromService()
		{
			return _service.ReturnMoo();
		}
	}

	public interface ITestService
	{
		string ReturnMoo();
	}

	internal class TestService : ITestService
	{
		public string ReturnMoo()
		{
			return "Moo";
		}
	}
}