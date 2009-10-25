using System.ComponentModel.DataAnnotations;
using LoginPortableArea.Login.Models;
using MvcContrib.PortableAreas;

namespace LoginPortableArea.Login.Messages
{
	public class LoginInputMessage : ICommandMessage<LoginResult>
	{
		[Required]
		public LoginResult Result { get; set; }
		[Required]
		public LoginInput Input { get; set; }
	}
}