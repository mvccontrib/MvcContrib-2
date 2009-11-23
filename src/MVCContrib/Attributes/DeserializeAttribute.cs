using System;
using System.Collections.Specialized;
using System.Web.Mvc;

namespace MvcContrib.Attributes
{
	[Obsolete("Consider using System.Web.Mvc.DefaultModelBinder instead.")]
	[Serializable]
	public class DeserializeAttribute : AbstractParameterBinderAttribute
	{
		private readonly NameValueDeserializer _deserializer = new NameValueDeserializer();

		public DeserializeAttribute()
		{
		}

		public DeserializeAttribute(string prefix)
			: base(prefix)
		{
		}

		public DeserializeAttribute(string prefix, RequestStore requestStore)
			: base(prefix, requestStore)
		{
		}

		public override object BindModel(ControllerContext controllerContext,    ModelBindingContext bindingContext)
		{
			NameValueCollection store = GetStore(controllerContext);
			return _deserializer.Deserialize(store, Prefix, bindingContext.ModelType);
		}
	}
}