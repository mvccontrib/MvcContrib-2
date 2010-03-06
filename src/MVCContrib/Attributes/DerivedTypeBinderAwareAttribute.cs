using System;

namespace MvcContrib.Attributes
{
	/// <summary>
	/// This attribute is used to identify alternate types that should be considered by the default model
	/// binder when running.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
	public class DerivedTypeBinderAwareAttribute : Attribute
	{
		/// <summary>
		/// ctor describing a class that may be alternatively bound to a common base property
		/// </summary>
		/// <param name="derivedType"></param>
		public DerivedTypeBinderAwareAttribute(Type derivedType)
		{
			DerivedType = derivedType;
		}

		/// <summary>
		/// the type declared for binding
		/// </summary>
		public Type DerivedType
		{ get; private set; }

	}
}


