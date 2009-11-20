using System;
using System.Collections.Generic;

namespace MvcContrib.UI.InputBuilder.Views
{
	public class TypeViewModel
	{
		protected Dictionary<string, object> _additionalValues = new Dictionary<string, object>();

		public PropertyViewModel[] Properties { get; set; }

		public string PartialName { get; set; }

		public string Label { get; set; }

		public Type Type { get; set; }

		public string Layout { get; set; }
	
		public virtual object Value { get; set; }

		public string Name { get; set; }

		public int Index { get; set; }

		public Dictionary<string, object> AdditionalValues
		{
			get { return _additionalValues; }
		}
	}

	public class ModelType<T> : TypeViewModel
	{
		public new T Value
		{
			get { return (T)base.Value; }
			set { base.Value = value; }
		}
	}
}