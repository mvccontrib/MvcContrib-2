using System.Text;
using System.Web;

namespace MvcContrib.UI.Tags.Validators
{
	public interface IValidator
	{
		string Id { get; set; }
		string ReferenceId { get; set; }
		string ErrorMessage { get; set; }
		string ValidationGroup { get; set; }
		string ValidationFunction { get; }
		bool IsValid { get; }
		void RenderClientHookup(StringBuilder output);
		bool Validate(HttpRequestBase request);
	}
}
