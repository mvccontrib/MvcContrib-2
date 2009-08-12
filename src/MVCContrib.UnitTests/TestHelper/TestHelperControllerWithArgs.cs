namespace MvcContrib.TestHelper.Test
{
	public class TestHelperControllerWithArgs : TestHelperController
	{
		private readonly ITestService _service;

		public TestHelperControllerWithArgs(ITestService service)
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
}
