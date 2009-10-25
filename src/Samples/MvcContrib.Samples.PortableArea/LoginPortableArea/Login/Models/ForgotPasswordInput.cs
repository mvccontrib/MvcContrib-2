using System.ComponentModel.DataAnnotations;

namespace LoginPortableArea.Login.Controllers
{
	public class ForgotPasswordInput
	{
		[Required]
		public string Username { get; set; }
	}
}