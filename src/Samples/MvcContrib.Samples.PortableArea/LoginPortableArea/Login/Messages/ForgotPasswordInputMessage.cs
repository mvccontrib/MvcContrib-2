using System;
using MvcContrib.PortableAreas;

namespace LoginPortableArea.Login.Controllers
{
	public class ForgotPasswordInputMessage : ICommandMessage<ForgotPasswordResult>
	{
		public ForgotPasswordInput Input{ get; set;}
		public ForgotPasswordResult Result{ get; set;}
	}
}