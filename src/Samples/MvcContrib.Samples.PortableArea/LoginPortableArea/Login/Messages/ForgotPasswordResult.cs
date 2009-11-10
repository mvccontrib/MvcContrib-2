using System;
using MvcContrib.PortableAreas;

namespace LoginPortableArea.Login.Controllers
{
	public class ForgotPasswordResult : ICommandResult
	{
		public bool Success { get; set; }

		public string Message { get; set; }
	}
}