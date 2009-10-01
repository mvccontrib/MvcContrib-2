using System;

namespace MvcContrib.UI.InputBuilder.Views
{
	public class TypeViewModel
	{
		public PropertyViewModel[] Properties { get; set; }

		public string PartialName { get; set; }

		public string Label { get; set; }

		public Type Type { get; set; }

		public string Layout { get; set; }
	
		public virtual object Value { get; set; }

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