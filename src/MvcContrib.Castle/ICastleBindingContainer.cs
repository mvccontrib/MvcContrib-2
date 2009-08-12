using Castle.Components.Binder;

namespace MvcContrib.Castle
{
	/// <summary>
	/// Controllers that implement this interface will have access to the IDataBinder instance used by the CastleBinderAttribute.
	/// </summary>
	public interface ICastleBindingContainer
	{
		/// <summary>
		/// The Binder that is used by the CastleBindAttribute to perform parameter binding.
		/// </summary>
		IDataBinder Binder { get; set; }
	}
}