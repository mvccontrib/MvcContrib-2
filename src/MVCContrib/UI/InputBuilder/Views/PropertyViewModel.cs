namespace MvcContrib.UI.InputBuilder.Views
{
	public class PropertyViewModel : TypeViewModel
	{

		public bool PropertyIsRequired { get; set; }

		public string Example { get; set; }

		public bool HasValidationMessages { get; set; }

		public bool HasExample()
		{
			return !string.IsNullOrEmpty(Example);
		}
	}

	public class PropertyViewModel<T> : PropertyViewModel
	{
		public new T Value
		{
			get { return (T)base.Value; }
			set { base.Value = value; }
		}
	}
}