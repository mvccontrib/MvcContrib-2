namespace MvcContrib.FluentHtml.Behaviors
{
	public class AutoLabelSettings : IBehaviorMarker
	{
		public bool UseFullNameForNestedProperties { get; private set; }
		public string LabelFormat { get; private set; }
		public string LabelCssClass { get; private set; }

		public AutoLabelSettings() : this(false, null, null) { }

		public AutoLabelSettings(bool useFullNameForNestedProperties, string labelFormat, string labelCssClass)
		{
			UseFullNameForNestedProperties = useFullNameForNestedProperties;
			LabelFormat = labelFormat;
			LabelCssClass = labelCssClass;
		}
	}
}