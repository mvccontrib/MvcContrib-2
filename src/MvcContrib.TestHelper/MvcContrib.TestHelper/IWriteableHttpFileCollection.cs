using System.Web;

namespace MvcContrib.TestHelper
{
	/// <summary>
	/// Defines an extended <see cref="HttpFileCollectionBase"/> which allows modifications to assist with testing.
	/// </summary>
	/// <remarks>
	/// Using this interface allows us to work around that fact that the indexers on <see cref="HttpPostedFileBase"/>
	/// are read only and cannot be changed by overrides.
	/// </remarks>
	public interface IWriteableHttpFileCollection
	{
		/// <summary>
		/// Gets or sets the <see cref="HttpPostedFileBase"/> with the specified name.
		/// </summary>
		HttpPostedFileBase this[string name]
		{
			get; set;
		}
	}
}
